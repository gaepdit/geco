Imports System.Data.SqlClient

Partial Class EIS_rp_process_bulk
    Inherits Page

    Public Property FacilitySiteId As String
    Public Property EIYear As String
    Public Property UpdatingValues As Boolean = False

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        FacilitySiteId = GetCookie(Cookie.AirsNumber)
        EIYear = GetCookie(EisCookie.EISMaxYear)

        FIAccessCheck(GetCookie(EisCookie.EISAccess))

    End Sub

    Private Sub LoadProcessBulk()

        TopPager.PageSize = ddlPager.SelectedValue
        BottomPager.PageSize = ddlPager.SelectedValue

        Dim query = " SELECT " &
        "     EMISSIONSUNITID, " &
        "     PROCESSID, " &
        "     STRPROCESSDESCRIPTION, " &
        "     PREV_INTINVENTORYYEAR, " &
        "     PREV_FLTCALCPARAMETERVALUE, " &
        "     PREV_CALCPARAMUOMCODE, " &
        "     u1.STRDESC as PREV_UOM, " &
        "     CURR_FLTCALCPARAMETERVALUE, " &
        "     CURR_CALCPARAMUOMCODE, " &
        "     u2.STRDESC as CURR_UOM, " &
        "     case " &
        "     when l.UnitOfMeasure is null " &
        "         then convert(bit, 0) " &
        "     when CURR_CALCPARAMUOMCODE is null " &
        "         then convert(bit, 0) " &
        "     when l.UnitOfMeasure = CURR_CALCPARAMUOMCODE " &
        "         then convert(bit, 0) " &
        "     else convert(bit, 1) " &
        "     end        as InvalidUnitOfMeasure " &
        " from VW_EIS_YEARLY_PROCESS p " &
        "     inner join EISLK_CALCPARAMUOMCODE u1 " &
        "         on u1.CALCPARAMUOMCODE = p.PREV_CALCPARAMUOMCODE " &
        "     inner join EISLK_CALCPARAMUOMCODE u2 " &
        "         on u2.CALCPARAMUOMCODE = p.CURR_CALCPARAMUOMCODE " &
        "     left join EISLK_SCC_UOM l " &
        "         on l.SourceClassificationCode = p.SOURCECLASSCODE " &
        " where FACILITYSITEID = @FacilitySiteId and " &
        "       CURR_INTINVENTORYYEAR = @EIYear and " &
        "       RPTPERIODTYPECODE = 'A' " &
        " order by EMISSIONSUNITID, PROCESSID "

        Dim params As SqlParameter() = {
            New SqlParameter("@FacilitySiteId", FacilitySiteId),
            New SqlParameter("@EIYear", EIYear)
        }

        lvProcessBulk.DataSource = DB.GetDataTable(query, params)
        lvProcessBulk.DataBind()

    End Sub

    Protected Sub DataPagerPrerender() Handles TopPager.PreRender
        If Not UpdatingValues Then LoadProcessBulk()
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateBottom.Click, btnUpdateTop.Click
        UpdatingValues = True

        lblUpdateStatusTop.Text = ""
        lblUpdateStatusBottom.Text = ""

        Dim EmissionsUnitID As String
        Dim ProcessID As String
        Dim NewProcessThroughput As Double

        ' First, save all valid data
        For Each item As ListViewDataItem In lvProcessBulk.Items
            EmissionsUnitID = CType(item.FindControl("lblEmissionsUnitID"), Label).Text
            ProcessID = CType(item.FindControl("lblProcessID"), Label).Text

            Dim tb As TextBox = CType(item.FindControl("txtCalcParameterValue"), TextBox)

            If tb.Text <> "" AndAlso Decimal.TryParse(tb.Text, NewProcessThroughput) Then
                UpdateProcessReportingPeriod(EmissionsUnitID, ProcessID, NewProcessThroughput)
            End If
        Next

        ' Second, Rebind the data
        LoadProcessBulk()

        ' Third, highlight all errors
        Dim NumBlank As Integer = 0

        For Each item As ListViewDataItem In lvProcessBulk.Items
            Dim tb As TextBox = CType(item.FindControl("txtCalcParameterValue"), TextBox)

            If tb.Text = "" OrElse Not Decimal.TryParse(tb.Text, Nothing) Then
                NumBlank += 1
                'Set background color of textbox (NOT the cell) to light red
                tb.BackColor = GecoColors.ErrorsTextbox.BackColor
                tb.ForeColor = GecoColors.ErrorsTextbox.ForeColor
            End If
        Next

        If NumBlank > 1 Then
            lblUpdateStatusTop.Text = "Update completed. There are " & NumBlank & " blank values in the table."
            lblUpdateStatusBottom.Text = "Update completed. There are " & NumBlank & " blank values in the table."
        ElseIf NumBlank = 1 Then
            lblUpdateStatusTop.Text = "Update completed. There is 1 blank value in the table."
            lblUpdateStatusBottom.Text = "Update completed. There is 1 blank value in the table."
        Else
            lblUpdateStatusTop.Text = "Update completed"
            lblUpdateStatusBottom.Text = "Update completed"
        End If

    End Sub

    Protected Sub ddlPager_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPager.SelectedIndexChanged

        LoadProcessBulk()

    End Sub

    Private Sub UpdateProcessReportingPeriod(euid As String, prid As String, ByVal newthroughput As String)

        Dim query As String = "update eis_processreportingperiod " &
               " set fltCalcParameterValue = @newthroughput, " &
               " UpdateUser = @upduser, " &
               " UpdateDateTime = getdate() " &
               " where FacilitySiteID = @fsid " &
               " and EmissionsUnitID = @euid " &
               " and ProcessID = @prid " &
               " and intInventoryYear = @eiyr "

        Dim params As SqlParameter() = {
            New SqlParameter("@newthroughput", newthroughput),
            New SqlParameter("@upduser", GetCookie(GecoCookie.UserID) & "-" & GetCookie(GecoCookie.UserName)),
            New SqlParameter("@fsid", FacilitySiteId),
            New SqlParameter("@eiyr", EIYear),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid)
        }

        DB.RunCommand(query, params)

    End Sub

    Protected Sub lvProcessBulk_DataBound(sender As Object, e As EventArgs) Handles lvProcessBulk.DataBound

        Dim invalidUnits As Boolean = False

        If lvProcessBulk.DataSource IsNot Nothing Then
            For Each item As ListViewDataItem In lvProcessBulk.Items
                If CType(item.FindControl("lblInvalidUnits"), Label).Visible Then
                    invalidUnits = True
                    Exit For
                End If
            Next
        End If

        lblInvalidUnitNote.Visible = invalidUnits

    End Sub

End Class