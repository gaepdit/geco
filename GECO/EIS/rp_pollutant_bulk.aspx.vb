Imports System.Data.SqlClient

Partial Class EIS_rp_pollutant_bulk
    Inherits Page

    Public Property FacilitySiteId As String
    Public Property EIYear As String
    Public Property UpdatingValues As Boolean = False

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        FacilitySiteId = GetCookie(Cookie.AirsNumber)
        EIYear = GetCookie(EisCookie.EISMaxYear)

        FIAccessCheck(GetCookie(EisCookie.EISAccess))

        lblErrorForBlanks.Visible = False

    End Sub

    Private Sub LoadEmissionBulk()

        TopPager.PageSize = ddlPager.SelectedValue
        BottomPager.PageSize = ddlPager.SelectedValue

        ' IMPORTANT >> 
        ' Sort order set for use by UpdateChanges subroutine (summer day check needs annual amt to work correctly 
        ' so Annual must be before Summer Day value in the gvw)
        Dim query = " SELECT " &
        "     EMISSIONSUNITID, " &
        "     PROCESSID, " &
        "     STRPROCESSDESCRIPTION, " &
        "     STRPOLLUTANTDESCRIPTION, " &
        "     Case When RPTPERIODTYPECODE = 'O3D' " &
        "         Then 'Summer Day' " &
        "     Else 'Annual' END RptPeriodType, " &
        "     Case When RPTPERIODTYPECODE = 'O3D' " &
        "         Then 'TPD' " &
        "     Else 'TPY' END PollutantUnit, " &
        "     PREV_INTINVENTORYYEAR, " &
        "     PREV_FLTTOTALEMISSIONS, " &
        "     CURR_FLTTOTALEMISSIONS, " &
        "     POLLUTANTCODE, " &
        "     EmissionsChangeGreaterThan20Percent " &
        " from VW_EIS_YEARLY_EMISSIONS " &
        " where FACILITYSITEID = @FacilitySiteId " &
        "     and CURR_INTINVENTORYYEAR = @EIYear " &
        " order by EMISSIONSUNITID, PROCESSID, " &
        "     STRPOLLUTANTDESCRIPTION, RPTPERIODTYPECODE "

        Dim params As SqlParameter() = {
            New SqlParameter("@FacilitySiteId", FacilitySiteId),
            New SqlParameter("@EIYear", EIYear)
        }

        lvEmissionsBulk.DataSource = DB.GetDataTable(query, params)
        lvEmissionsBulk.DataBind()

    End Sub

    Protected Sub DataPagerPrerender() Handles TopPager.PreRender
        If Not UpdatingValues Then
            LoadEmissionBulk()
            lblUpdateStatusTop.Text = ""
            lblUpdateStatusBottom.Text = ""
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateBottom.Click, btnUpdateTop.Click
        UpdatingValues = True

        lblUpdateStatusTop.Text = ""
        lblUpdateStatusBottom.Text = ""

        Dim NewTotalEmission As Decimal
        Dim ProcessID As String
        Dim EmissionsUnitID As String
        Dim PollutantCode As String
        Dim ReportingPerTypeCode As String
        Dim SummerDayOK As Boolean

        lblErrorForBlanks.Visible = False

        Try
            ' First, save all valid data
            For Each item As ListViewDataItem In lvEmissionsBulk.Items
                EmissionsUnitID = CType(item.FindControl("lblEmissionsUnitID"), Label).Text
                ProcessID = CType(item.FindControl("lblProcessID"), Label).Text
                PollutantCode = CType(item.FindControl("lblPollutantCode"), Label).Text

                Dim tb As TextBox = CType(item.FindControl("txtNewTotalEmission"), TextBox)

                If tb.Text <> "" AndAlso Decimal.TryParse(tb.Text, NewTotalEmission) Then
                    ' IMPORTANT!! >> gridview must be sorted so that ANNUAL EMISSION is above SUMMER DAY for each process (VOC and NOx)
                    Dim periodType As String = CType(item.FindControl("lblRptPeriodType"), Label).Text
                    If periodType = "Summer Day" Then
                        ReportingPerTypeCode = "O3D"
                        SummerDayOK = CheckSummerDayBulk(NewTotalEmission)
                    Else
                        ReportingPerTypeCode = "A"
                        SummerDayOK = True
                    End If

                    If SummerDayOK Then
                        SaveReportingPeriodEmissions(NewTotalEmission, ProcessID, EmissionsUnitID, PollutantCode, ReportingPerTypeCode)
                    End If
                End If
            Next

            ' Second, Rebind the data
            LoadEmissionBulk()

            ' Third, highlight all errors

            lblErrorForBlanks.Visible = False

            Dim SummerDayErrors As Integer = 0
            Dim NumBlanks As Integer = 0

            For Each item As ListViewDataItem In lvEmissionsBulk.Items
                Dim tb As TextBox = CType(item.FindControl("txtNewTotalEmission"), TextBox)

                If tb.Text = "" OrElse Not Decimal.TryParse(tb.Text, NewTotalEmission) Then
                    NumBlanks += 1
                    tb.BackColor = GecoColors.ErrorsTextbox.BackColor
                    tb.ForeColor = GecoColors.ErrorsTextbox.ForeColor
                Else
                    ' IMPORTANT!! >> gridview must be sorted so that ANNUAL EMISSION is above SUMMER DAY for each process (VOC and NOx)
                    Dim periodType As String = CType(item.FindControl("lblRptPeriodType"), Label).Text
                    If periodType = "Summer Day" Then
                        SummerDayOK = CheckSummerDayBulk(NewTotalEmission)
                        If Not SummerDayOK Then
                            SummerDayErrors += 1
                            'Set background color of textbox (NOT the cell) to light red
                            tb.BackColor = GecoColors.ErrorsTextbox.BackColor
                            tb.ForeColor = GecoColors.ErrorsTextbox.ForeColor
                        End If
                    Else
                        SummerDayOK = True
                    End If

                    If SummerDayOK Then
                        tb.BackColor = GecoColors.Normal.BackColor
                        tb.ForeColor = GecoColors.Normal.ForeColor
                    End If
                End If
            Next

            If SummerDayErrors > 0 Or NumBlanks > 0 Then

                lblUpdateStatusTop.Text = "Updates in red not completed."
                lblUpdateStatusBottom.Text = "Updates in red not completed. See error message above the table."
                lblUpdateStatusTop.BackColor = GecoColors.ErrorsHighlighted.BackColor
                lblUpdateStatusBottom.BackColor = GecoColors.ErrorsHighlighted.BackColor

                If SummerDayErrors > 0 Then
                    Dim valueText As String
                    If SummerDayErrors > 1 Then
                        valueText = "values"
                    Else
                        valueText = "value"
                    End If
                    lblErrorForSummerDay.Text = SummerDayErrors & " summer day emission " & valueText &
                        " out of range or blank or associated annual emission blank. Summer day emission must be at least 0.001 TPD."
                    lblErrorForSummerDay.BackColor = GecoColors.ErrorsHighlighted.BackColor
                    lblErrorForSummerDay.Visible = True
                End If

                If NumBlanks > 0 Then
                    If NumBlanks > 1 Then
                        lblErrorForBlanks.Text = "There are " & NumBlanks & " blank values in the table below."
                    Else
                        lblErrorForBlanks.Text = "There is 1 blank value in the table below."
                    End If
                    lblErrorForBlanks.BackColor = GecoColors.ErrorsHighlighted.BackColor
                    lblErrorForBlanks.Visible = True
                End If

            Else
                lblUpdateStatusTop.Text = "Update completed"
                lblUpdateStatusBottom.Text = "Update completed"
                lblErrorForSummerDay.Text = ""
                lblUpdateStatusTop.BackColor = Drawing.Color.Transparent
                lblUpdateStatusBottom.BackColor = Drawing.Color.Transparent
                lblErrorForSummerDay.BackColor = Drawing.Color.Transparent
                lblErrorForBlanks.BackColor = Drawing.Color.Transparent
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Function CheckSummerDayBulk(newemission As Decimal) As Boolean
        Return newemission >= 0.001
    End Function

    Protected Sub ddlPager_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPager.SelectedIndexChanged

        LoadEmissionBulk()

    End Sub

    Private Function SaveReportingPeriodEmissions(
        emissions As Decimal,
        processId As String,
        emissionsUnitId As String,
        pollutantCode As String,
        reportingPerTypeCode As String
        ) As Boolean

        Dim query = "update eis_ReportingPeriodEmissions set " &
            "fltTotalEmissions = @fltTotalEmissions, " &
            "UpdateUser = @UpdateUser, " &
            "UpdateDateTime = getdate() " &
            "where FacilitySiteID = @FacilitySiteID " &
            "and EmissionsUnitID = @EmissionsUnitID " &
            "and ProcessID = @ProcessID " &
            "and PollutantCode = @PollutantCode " &
            "and RPTPeriodTypeCode = @RPTPeriodTypeCode " &
            "and intInventoryYear = @intInventoryYear "

        Dim params As SqlParameter() = {
            New SqlParameter("@fltTotalEmissions", emissions),
            New SqlParameter("@UpdateUser", GetCookie(GecoCookie.UserID) & "-" & GetCookie(GecoCookie.UserName)),
            New SqlParameter("@FacilitySiteID", FacilitySiteId),
            New SqlParameter("@EmissionsUnitID", emissionsUnitId),
            New SqlParameter("@ProcessID", processId),
            New SqlParameter("@PollutantCode", pollutantCode),
            New SqlParameter("@RPTPeriodTypeCode", reportingPerTypeCode),
            New SqlParameter("@intInventoryYear", EIYear)
        }

        Return DB.RunCommand(query, params)

    End Function

    Protected Sub lvEmissionsBulk_DataBound(sender As Object, e As EventArgs) Handles lvEmissionsBulk.DataBound

        Dim summerDay As Boolean = False
        Dim twentyPercent As Boolean = False

        If lvEmissionsBulk.DataSource IsNot Nothing Then
            For Each item As ListViewDataItem In lvEmissionsBulk.Items
                If CType(item.FindControl("lblRptPeriodType"), Label).Text = "Summer Day" Then
                    summerDay = True
                End If
                If CType(item.FindControl("lbl20Percent"), Label).Visible Then
                    twentyPercent = True
                End If
            Next
        End If

        lblSummerDayNote.Visible = summerDay
        lbl20PercentNote.Visible = twentyPercent

    End Sub

End Class