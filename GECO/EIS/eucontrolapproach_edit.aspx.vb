Imports System.Data.SqlClient

Partial Class eis_eucontrolapproach_edit
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EmissionsUnitID As String = Request.QueryString("eu").ToUpper
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            LoadcontrolMeasureDDL()
            LoadControlPollutantDDL()
            LoadUnitControlApproach(FacilitySiteID, EmissionsUnitID)
            LoadEUControlMeasureDGV(FacilitySiteID, EmissionsUnitID)
            LoadEUControlPollutantsDGV(FacilitySiteID, EmissionsUnitID)
            If gvwEUControlMeasure.Rows.Count = 0 Then
                lblEUControlMeasureWarning.Text = "Click Add to include at least one Control Measure."
                lblEUControlMeasureWarning.Visible = True
                btnAddControlMeasure.Visible = True
            Else
                lblEUControlMeasureWarning.Visible = False
            End If
            If gvwEUControlPollutant.Rows.Count = 0 Then
                lblEUControlPollutantWarning.Text = "Click Add to include at least one Control Pollutant."
                lblEUControlPollutantWarning.Visible = True
                btnAddControlPollutant.Visible = True
            Else
                lblEUControlPollutantWarning.Visible = False
            End If
            btnDeleteDetails.Visible = False
        End If

        ShowFacilityInventoryMenu()
        ShowEISHelpMenu()

        If EISStatus = "2" Then
            ShowEmissionInventoryMenu()
        Else
            HideEmissionInventoryMenu()
        End If
    End Sub

    Private Sub LoadcontrolMeasureDDL()
        ddlControlMeasure.Items.Add("--Select a Control Measure --")

        Try
            Dim query = "select STRCONTROLMEASURECODE, strdesc " &
                " FROM EISLK_CONTROLMEASURECODE where Active = '1' order by strdesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("STRCONTROLMEASURECODE")
                    }
                    ddlControlMeasure.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadControlPollutantDDL()
        ddlControlPollutants.Items.Add("--Select a Pollutant --")

        Try
            Dim query = " select POLLUTANTCODE, STRDESC " &
                " FROM EISLK_POLLUTANTCODE " &
                " where EISLK_POLLUTANTCODE.STRPOLLUTANTTYPE = 'CAP' and EISLK_POLLUTANTCODE.ACTIVE ='1' order by STRDESC"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("POLLUTANTCODE")
                    }
                    ddlControlPollutants.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadEUControlMeasureDGV(ByVal fsid As String, ByVal euid As String)
        Try
            Dim query = "select " &
                " EIS_UNITCONTROLMEASURE.FACILITYSITEID, " &
                " EIS_UNITCONTROLMEASURE.EMISSIONSUNITID, " &
                " EIS_UNITCONTROLMEASURE.STRCONTROLMEASURECODE as MeasureCode, " &
                " EISLK_CONTROLMEASURECODE.STRDESC as CDType, " &
                " EIS_UNITCONTROLMEASURE.LastEISSubmitDate " &
                " FROM EIS_UNITCONTROLMEASURE, EISLK_CONTROLMEASURECODE " &
                " where EIS_UNITCONTROLMEASURE.FACILITYSITEID = @fsid " &
                " and EIS_UNITCONTROLMEASURE.EMISSIONSUNITID = @euid " &
                " and EIS_UNITCONTROLMEASURE.ACTIVE = '1' " &
                " and EISLK_CONTROLMEASURECODE.Active = '1' " &
                " and EIS_UNITCONTROLMEASURE.STRCONTROLMEASURECODE = EISLK_CONTROLMEASURECODE.STRCONTROLMEASURECODE " &
                " order by CDType "

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid)
            }

            gvwEUControlMeasure.DataSource = DB.GetDataTable(query, params)
            gvwEUControlMeasure.DataBind()

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadEUControlPollutantsDGV(ByVal fsid As String, ByVal euid As String)
        Try
            Dim query = "SELECT u.FACILITYSITEID " &
                " , u.EMISSIONSUNITID " &
                " , u.POLLUTANTCODE " &
                " , l.STRDESC AS PollutantType " &
                " , CONVERT(decimal(4, 1), u.NUMPCTCTRLMEASURESREDEFFIC) AS MeasureEfficiency " &
                " , FORMAT(u.LASTEISSUBMITDATE, 'MM/dd/yyyy') AS LASTEISSUBMITDATE " &
                " , CONVERT(decimal(5, 2), c.NUMPCTCTRLAPPROACHCAPEFFIC * c.NUMPCTCTRLAPPROACHEFFECT * u.NUMPCTCTRLMEASURESREDEFFIC / 10000) " &
                " AS CalculatedReduction " &
                " FROM dbo.EIS_UNITCONTROLPOLLUTANT AS u " &
                " LEFT JOIN dbo.EISLK_POLLUTANTCODE AS l " &
                " ON u.POLLUTANTCODE = l.POLLUTANTCODE " &
                " AND l.ACTIVE = '1' " &
                " LEFT JOIN dbo.EIS_UNITCONTROLAPPROACH AS c " &
                " ON c.FACILITYSITEID = u.FACILITYSITEID " &
                " AND c.EMISSIONSUNITID = u.EMISSIONSUNITID " &
                " AND c.ACTIVE = '1' " &
                " WHERE u.FACILITYSITEID = @fsid " &
                " AND u.EMISSIONSUNITID = @euid " &
                " AND u.ACTIVE = '1' " &
                " ORDER BY PollutantType "

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid)
            }

            gvwEUControlPollutant.DataSource = DB.GetDataTable(query, params)
            gvwEUControlPollutant.DataBind()

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadUnitControlApproach(ByVal fsid As String, ByVal euid As String)
        Try
            Dim query = "select " &
                " EmissionsUnitID, " &
                " strControlApproachDescription, " &
                " numPctCtrlApproachCapEffic, " &
                " numPctCtrlApproachEffect, " &
                " strControlApproachComment " &
                " from " &
                " EIS_UnitControlApproach " &
                " where FacilitySiteID = @fsid and " &
                " EmissionsUnitID = @euid and " &
                " Active = '1'"

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid)
            }

            Dim dr = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                If IsDBNull(dr("EmissionsUnitID")) Then
                    txtEmissionUnitID.Text = ""
                Else
                    txtEmissionUnitID.Text = dr.Item("EmissionsUnitID")
                End If

                'Get emission unit description using eisairsnumber and emission unit ID
                txtEmissionUnitDesc.Text = GetEmissionUnitDesc(fsid, euid)

                If IsDBNull(dr("strControlApproachDescription")) Then
                    txtControlApproachDescription.Text = ""
                Else
                    txtControlApproachDescription.Text = dr.Item("strControlApproachDescription")
                End If
                If IsDBNull(dr("numPctCtrlApproachCapEffic")) Then
                    txtPctCtrlApproachCapEffic.Text = ""
                Else
                    txtPctCtrlApproachCapEffic.Text = dr.Item("numPctCtrlApproachCapEffic")
                End If
                If IsDBNull(dr("numPctCtrlApproachEffect")) Then
                    txtPctCtrlApproachEffect.Text = ""
                Else
                    txtPctCtrlApproachEffect.Text = dr.Item("numPctCtrlApproachEffect")
                End If
                If IsDBNull(dr("strControlApproachComment")) Then
                    txtControlApproachComment.Text = ""
                Else
                    txtControlApproachComment.Text = dr.Item("strControlApproachComment")
                End If

            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        SaveEUControlApproach()
        lblMessage.Text = "Control Approach saved succesfully"
        lblMessage.Visible = True
    End Sub

    'Save Emission Unit Control Approach
    Private Sub SaveEUControlApproach()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim CtrlApproachDesc As String = txtControlApproachDescription.Text
        Dim CtrlApprCapEffic As String = txtPctCtrlApproachCapEffic.Text
        Dim CtrlApprEffect As String = txtPctCtrlApproachEffect.Text
        Dim CtrlApproachComment As String = txtControlApproachComment.Text

        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        'Truncate comment if >400 chars
        CtrlApproachComment = Left(CtrlApproachComment, 400)

        'Truncate description if >200 chars
        CtrlApproachDesc = Left(CtrlApproachDesc, 200)

        Try
            Dim query = "Update eis_UnitControlApproach Set " &
                "strControlApproachDescription = @CtrlApproachDesc, " &
                "numPctCtrlApproachCapEffic = @CtrlApprCapEffic, " &
                "numPctCtrlApproachEffect = @CtrlApprEffect, " &
                "strControlApproachComment = @CtrlApproachComment, " &
                "UpdateUser = @UpdateUser, " &
                "UpdateDateTime = getdate() " &
                "where " &
                "FacilitySiteID = @FacilitySiteID and " &
                "EmissionsUnitID = @EmissionsUnitID and " &
                "Active = '1'"

            Dim params = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@CtrlApproachDesc", CtrlApproachDesc),
                New SqlParameter("@CtrlApprCapEffic", CtrlApprCapEffic),
                New SqlParameter("@CtrlApprEffect", CtrlApprEffect),
                New SqlParameter("@CtrlApproachComment", CtrlApproachComment),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@EmissionsUnitID", EmissionsUnitID)
            }

            DB.RunCommand(query, params)

            LoadEUControlPollutantsDGV(FacilitySiteID, EmissionsUnitID)
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    'Add Emission Unit Control Approach Control Measure
    Protected Sub btnAddControlMeasure_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddControlMeasure.Click
        lblEUControlMeasureWarning.Visible = False
        lblMessage.Visible = False
        InsertControlMeasure()
    End Sub

    Private Sub InsertControlMeasure()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionUnitID As String = txtEmissionUnitID.Text
        Dim CMcode As String = ddlControlMeasure.SelectedValue

        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Try
            Dim query = "Select STRCONTROLMEASURECODE FROM EIS_UNITCONTROLMEASURE " &
                " where EIS_UNITCONTROLMEASURE.FACILITYSITEID = @FacilitySiteID " &
                " and EIS_UNITCONTROLMEASURE.EMISSIONSUNITID = @EmissionUnitID " &
                " and EIS_UNITCONTROLMEASURE.STRCONTROLMEASURECODE = @CMcode " &
                " and EIS_UNITCONTROLMEASURE.ACTIVE = 1 "

            Dim params = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionUnitID", EmissionUnitID.ToUpper),
                New SqlParameter("@CMcode", CMcode.ToUpper),
                New SqlParameter("@UpdateUser", UpdateUser)
            }

            If DB.ValueExists(query, params) Then
                lblEUControlMeasureWarning.Text = "Please select a new control measure. This one already exists in the Control Approach."
                lblEUControlMeasureWarning.Visible = True
            Else
                Dim query2 = "Insert Into EIS_UNITCONTROLMEASURE " &
                    " (FACILITYSITEID, " &
                    " EMISSIONSUNITID, " &
                    " STRCONTROLMEASURECODE, " &
                    " ACTIVE, " &
                    " UPDATEUSER, " &
                    " UPDATEDATETIME, " &
                    " CreateDateTime) " &
                    "  Values (" &
                    " @FacilitySiteID, " &
                    " @EmissionUnitID, " &
                    " @CMcode, " &
                    " 1, " &
                    " @UpdateUser, " &
                    " getdate(), " &
                    " getdate() ) "

                DB.RunCommand(query2, params)

                ddlControlMeasure.SelectedIndex = 0
                LoadEUControlMeasureDGV(FacilitySiteID, EmissionUnitID)
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnAddControlPollutant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddControlPollutant.Click
        lblMessage.Visible = False
        lblEUControlMeasureWarning.Visible = False
        lblEUControlPollutantWarning.Visible = False
        InsertControlPollutant()
    End Sub

    Private Sub InsertControlPollutant()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text
        Dim CPcode As String = ddlControlPollutants.SelectedValue
        Dim CMReductEff As Decimal = Decimal.Round(CDec(txtCMReductionEff.Text), 1)

        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        lblEUControlPollutantWarning.Visible = False

        Try

            Dim _iReturnCode As Integer = InsertUpdateEUControlPollutant(FacilitySiteID, EmissionsUnitID, CPcode, CMReductEff, UpdateUser, DbChangeMode.Insert)

            If _iReturnCode = 0 Then
                LoadEUControlPollutantsDGV(FacilitySiteID, EmissionsUnitID)
                ddlControlPollutants.SelectedIndex = 0
                txtCMReductionEff.Text = ""
                lblEUControlPollutantWarning.Text = "Emission Unit Control Pollutant saved successfully."
                lblEUControlPollutantWarning.Visible = True
            ElseIf _iReturnCode = -20 Then
                lblEUControlPollutantWarning.Text = "The PM2.5 Control Measure Reduction Efficiency can not be larger than the PM10 Control Measure Reduction Efficiency."
                lblEUControlPollutantWarning.Visible = True
            ElseIf _iReturnCode = -30 Then
                lblEUControlPollutantWarning.Text = "Please enter a new pollutant. This one already exists in the Control Approach."
                lblEUControlPollutantWarning.Visible = True
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    'Delete Emission Unit Control Measure
    Protected Sub gvwEUControlMeasure_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwEUControlMeasure.RowDeleting
        Dim FacilitySiteID As String = gvwEUControlMeasure.DataKeys(e.RowIndex).Values(0).ToString
        Dim EmissionsUnitID As String = gvwEUControlMeasure.DataKeys(e.RowIndex).Values(1).ToString
        Dim MeasureCode As String = gvwEUControlMeasure.DataKeys(e.RowIndex).Values(2).ToString

        If gvwEUControlMeasure.Rows.Count = 1 Then
            lblEUControlMeasureWarning.Text = "Please add an additional control measure before the only control measure in the control approach can be deleted."
            lblEUControlMeasureWarning.Visible = True
            e.Cancel = True
        Else
            Dim query = "Delete FROM  EIS_UNITCONTROLMEASURE " &
                "where EIS_UNITCONTROLMEASURE.FACILITYSITEID = @FacilitySiteID " &
                "and EIS_UNITCONTROLMEASURE.EMISSIONSUNITID = @EmissionsUnitID " &
                "and EIS_UNITCONTROLMEASURE.STRCONTROLMEASURECODE = @MeasureCode "

            Dim params = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionsUnitID", EmissionsUnitID),
                New SqlParameter("@MeasureCode", MeasureCode)
            }

            DB.RunCommand(query, params)

            LoadEUControlMeasureDGV(FacilitySiteID, EmissionsUnitID)
        End If
    End Sub

    'Edit the Control Pollutant Efficiency
    Protected Sub gvwEUControlPollutant_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwEUControlPollutant.RowEditing
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        gvwEUControlPollutant.EditIndex = e.NewEditIndex
        LoadEUControlPollutantsDGV(FacilitySiteID, EmissionsUnitID)
    End Sub

    'Update the Control Pollutant Efficiency
    Protected Sub gvwEUControlPollutant_RowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs) Handles gvwEUControlPollutant.RowUpdating

        Dim MeasureEfficiency As Decimal = Decimal.Round(CDec(DirectCast(gvwEUControlPollutant.Rows(e.RowIndex).FindControl("txtMeasureEfficiency"), TextBox).Text), 1)
        Dim FacilitySiteID As String = gvwEUControlPollutant.DataKeys(e.RowIndex).Values(0).ToString
        Dim EmissionsUnitID As String = gvwEUControlPollutant.DataKeys(e.RowIndex).Values(1).ToString
        Dim PollutantCode As String = gvwEUControlPollutant.DataKeys(e.RowIndex).Values(2).ToString

        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Try

            lblEUControlPollutantWarning.Visible = False

            Dim _iReturnCode As Integer = InsertUpdateEUControlPollutant(FacilitySiteID, EmissionsUnitID, PollutantCode, MeasureEfficiency, UpdateUser, DbChangeMode.Update)

            If _iReturnCode = 0 Then
                gvwEUControlPollutant.EditIndex = -1
                LoadEUControlPollutantsDGV(FacilitySiteID, EmissionsUnitID)
                ddlControlPollutants.SelectedIndex = 0
                txtCMReductionEff.Text = ""
            ElseIf _iReturnCode = -20 Then
                Dim row As GridViewRow = gvwEUControlPollutant.Rows(e.RowIndex)
                Dim label As Label = TryCast(row.FindControl("lblError"), Label)
                label.Text = "*"
                label.Visible = True
                lblEUControlPollutantWarning.Text = "The PM2.5 Control Measure Reduction Efficiency can not be larger than the PM10 Control Measure Reduction Efficiency."
                lblEUControlPollutantWarning.Visible = True
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    'Cancels the Edit without updating the database
    Protected Sub gvwEUControlPollutant_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvwEUControlPollutant.RowCancelingEdit
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        gvwEUControlPollutant.EditIndex = -1
        LoadEUControlPollutantsDGV(FacilitySiteID, EmissionsUnitID)
    End Sub

    'Delete Emission Unit Control Pollutant
    Protected Sub gvwEUControlPollutant_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles gvwEUControlPollutant.RowDeleting
        Try
            If gvwEUControlPollutant.Rows.Count = 1 Then
                lblEUControlPollutantWarning.Text = "Please add an additional control pollutant before the only control pollutant in the control approach can be deleted."
                lblEUControlPollutantWarning.Visible = True
            Else
                Dim FacilitySiteID As String = gvwEUControlPollutant.DataKeys(e.RowIndex).Values(0).ToString
                Dim EmissionsUnitID As String = gvwEUControlPollutant.DataKeys(e.RowIndex).Values(1).ToString
                Dim PollutantCode As String = gvwEUControlPollutant.DataKeys(e.RowIndex).Values(2).ToString

                Dim query = "Delete FROM  EIS_UNITCONTROLPOLLUTANT " &
                    " where EIS_UNITCONTROLPOLLUTANT.FACILITYSITEID = @FacilitySiteID " &
                    " and EIS_UNITCONTROLPOLLUTANT.EMISSIONSUNITID = @EmissionsUnitID " &
                    " and EIS_UNITCONTROLPOLLUTANT.pollutantcode = @PollutantCode "

                Dim params = {
                    New SqlParameter("@FacilitySiteID", FacilitySiteID),
                    New SqlParameter("@EmissionsUnitID", EmissionsUnitID),
                    New SqlParameter("@PollutantCode", PollutantCode)
                }

                DB.RunCommand(query, params)

                LoadEUControlPollutantsDGV(FacilitySiteID, EmissionsUnitID)
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim EmissionUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim targetpage As String = "~/EIS/emissionunit_details.aspx?eu=" & EmissionUnitID
        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim targetpage As String = "~/EIS/emissionunit_details.aspx?eu=" & EmissionsUnitID
        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnDeleteDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDetails.Click
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim targetpage As String = "~/EIS/emissionunit_details.aspx?eu=" & EmissionsUnitID
        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnDeleteOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteOK.Click
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim targetpage As String = "~/EIS/emissionunit_details.aspx?eu=" & EmissionsUnitID

        DeleteEUCtrlAppMeasures(FacilitySiteID, EmissionsUnitID)
        DeleteEUCtrlAppPollutants(FacilitySiteID, EmissionsUnitID)
        DeleteEUControlApproach(FacilitySiteID, EmissionsUnitID)

        lblDeleteUnitCA.Text = "The Emissions Unit Control Approach, Pollutants and Measures have been deleted."
        btnDeleteCancel.Visible = False
        btnDeleteOK.Visible = False
        btnDeleteDetails.Visible = True
        mpeDelete.Show()
    End Sub

#Region "  Menu Routines  "

    Private Sub ShowFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = True
        End If

    End Sub

    Private Sub HideFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = False
        End If

    End Sub

    Private Sub ShowEmissionInventoryMenu()

        Dim menuEmissionInventory As Panel

        menuEmissionInventory = CType(Master.FindControl("pnlEmissionInventory"), Panel)

        If Not menuEmissionInventory Is Nothing Then
            menuEmissionInventory.Visible = True
        End If

    End Sub

    Private Sub HideEmissionInventoryMenu()

        Dim menuEmissionInventory As Panel

        menuEmissionInventory = CType(Master.FindControl("pnlEmissionInventory"), Panel)

        If Not menuEmissionInventory Is Nothing Then
            menuEmissionInventory.Visible = False
        End If

    End Sub

    Private Sub ShowEISHelpMenu()

        Dim menuEISHelp As Panel

        menuEISHelp = CType(Master.FindControl("pnlEISHelp"), Panel)

        If Not menuEISHelp Is Nothing Then
            menuEISHelp.Visible = True
        End If

    End Sub

    Private Sub HideEISHelpMenu()

        Dim menuEISHelp As Panel

        menuEISHelp = CType(Master.FindControl("pnlEISHelp"), Panel)

        If Not menuEISHelp Is Nothing Then
            menuEISHelp.Visible = False
        End If

    End Sub

#End Region

End Class