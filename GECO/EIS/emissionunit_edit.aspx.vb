Imports System.Data.SqlClient
Imports EpdIt.DBUtilities

Partial Class eis_emissionunit_edit
    Inherits Page

    Public EISSubmitStatus As Boolean
    Public EUProcessExist As Boolean
    Public ProcessActive As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EmissionsUnitID As String = Request.QueryString("eu").ToUpper
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            LoadUnitTypeCode()
            LoadUnitStatusCode()
            LoadDesignCapacityUOM()
            LoadEmissionUnitDetails(EmissionsUnitID)
            EUProcessExist = CheckProcessExistAny(FacilitySiteID, EmissionsUnitID)
            btnSummary.Visible = False

            If EISSubmitStatus Or EUProcessExist Then
                btnDeleteEmissionUnit.Visible = False
            End If

            rngvDateInOperation.MaximumValue = Now.Date
            rngvDateInOperation.MinimumValue = New Date(1900, 1, 1)

        End If

        ShowFacilityInventoryMenu()
        ShowEISHelpMenu()
        If EISStatus = "2" Then
            ShowEmissionInventoryMenu()
        Else
            HideEmissionInventoryMenu()
        End If

    End Sub

#Region " Load Descriptions/Codes "

    Private Sub LoadUnitTypeCode()
        ddlUnitTypeCode.Items.Add("--Select Unit Type--")

        Try
            Dim query = " select UnitTypeCode, strDesc FROM EISLK_UnitTypeCode where Active = '1' order by strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("UnitTypeCode")
                    }
                    ddlUnitTypeCode.Items.Add(newListItem)
                Next
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadUnitStatusCode()
        ddlUnitStatusCode.Items.Add("--Select Unit Status--")

        Try
            Dim query = " select UnitStatusCode, strDesc FROM EISLK_UnitStatusCode where Active = '1' order by strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem() With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("UnitStatusCode")
                    }
                    ddlUnitStatusCode.Items.Add(newListItem)
                Next
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadDesignCapacityUOM()
        ddlUnitDesignCapacityUOMCode.Items.Add("--Select Unit of Measure--")

        Try

            Dim query = " select UnitDesignCapacityUOMCode, strDesc FROM EISLK_UnitDesCapacityUOMCode where Active = '1' order by strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("UnitDesignCapacityUOMCode")
                    }
                    ddlUnitDesignCapacityUOMCode.Items.Add(newListItem)
                Next
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

#Region " Load Emission Unit Details "

    Private Sub LoadEmissionUnitDetails(ByVal euid As String)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim Updateuser As String = ""
        Dim UnitDesignCapacity As Decimal
        Dim MaxNameplateCapacity As Decimal
        Dim UnitTypeCodeDesc As String = ""
        Dim UnitStatusCode As String = ""
        Dim UnitStatusCodeDesc As String = ""
        Dim UnitDesignCapacityUOMCode As String = ""
        Dim UnitDesignCapacityUOMCodeDesc As String = ""
        Dim EISSubmit As String = ""

        euid = euid.ToUpper

        Try
            Dim query = "select " &
                "EmissionsUnitID, " &
                "strUnitDescription, " &
                "strUnitTypeCode, " &
                "strUnitStatusCode, " &
                "fltUnitDesignCapacity, " &
                "strUnitDesignCapacityUOMCode, " &
                "convert(char, datUnitOperationDate, 101) As datUnitOperationDate, " &
                "strUnitComment, " &
                "strEISSubmit, " &
                "convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                "numMaximumNameplateCapacity, " &
                "convert(char, LastEISSubmitDate, 101) LastEISSubmitDate, " &
                "UpdateUser, " &
                "convert(char, UpdateDateTime, 20) As UpdateDateTime " &
                "from " &
                "EIS_EmissionsUnit " &
                "where FacilitySiteID = @FacilitySiteID and " &
                "EmissionsUnitID = @euid  "

            Dim params = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@euid", euid)
            }

            Dim dr = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                If IsDBNull(dr("EmissionsUnitID")) Then
                    txtEmissionsUnitID.Text = ""
                Else
                    txtEmissionsUnitID.Text = dr.Item("EmissionsUnitID")
                End If
                If IsDBNull(dr("strUnitDescription")) Then
                    txtUnitDescription.Text = ""
                Else
                    txtUnitDescription.Text = dr.Item("strUnitDescription")
                End If
                If IsDBNull(dr("strUnitTypeCode")) Then
                    ddlUnitTypeCode.SelectedIndex = 0
                Else
                    ddlUnitTypeCode.SelectedValue = dr.Item("strUnitTypeCode")
                End If

                If IsDBNull(dr("strUnitStatusCode")) Then
                    ddlUnitStatusCode.SelectedIndex = 0
                    txtUnitStatusCodeOnLoad.Text = ""
                Else
                    ddlUnitStatusCode.SelectedValue = dr.Item("strUnitStatusCode")
                    txtUnitStatusCodeOnLoad.Text = ddlUnitStatusCode.SelectedValue
                End If
                If IsDBNull(dr("datUnitOperationDate")) Then
                    txtUnitOperationDate.Text = ""
                Else
                    txtUnitOperationDate.Text = dr.Item("datUnitOperationDate")
                End If
                If IsDBNull(dr("fltUnitDesignCapacity")) Then
                    txtUnitDesignCapacity.Text = ""
                Else
                    UnitDesignCapacity = dr.Item("fltUnitDesignCapacity")
                    If UnitDesignCapacity = -1 Or UnitDesignCapacity = 0 Then
                        txtUnitDesignCapacity.Text = ""
                    Else
                        txtUnitDesignCapacity.Text = UnitDesignCapacity
                    End If
                End If
                If IsDBNull(dr("strUnitDesignCapacityUOMCode")) Then
                    ddlUnitDesignCapacityUOMCode.SelectedIndex = 0
                Else
                    ddlUnitDesignCapacityUOMCode.SelectedValue = dr.Item("strUnitDesignCapacityUOMCode")
                End If
                If IsDBNull(dr("numMaximumNameplateCapacity")) Then
                    txtMaximumNameplateCapacity.Text = ""
                Else
                    MaxNameplateCapacity = dr.Item("numMaximumNameplateCapacity")
                    If MaxNameplateCapacity = -1 Or MaxNameplateCapacity = 0 Then
                        txtMaximumNameplateCapacity.Text = ""
                    Else
                        txtMaximumNameplateCapacity.Text = MaxNameplateCapacity
                    End If
                End If
                If txtMaximumNameplateCapacity.Text = "" Then
                    ddlElecGen.SelectedValue = "No"
                    pnlElecGen.Visible = False
                Else
                    ddlElecGen.SelectedValue = "Yes"
                    pnlElecGen.Visible = True
                End If

                If IsDBNull(dr("strUnitComment")) Then
                    txtUnitComment.Text = ""
                Else
                    txtUnitComment.Text = dr.Item("strUnitComment")
                End If
                If IsDBNull(dr("strEISSubmit")) Then
                    EISSubmit = ""
                    'What to do if blank?????
                Else
                    EISSubmit = dr.Item("strEISSubmit")
                    If EISSubmit = "1" Then
                        EISSubmitStatus = True
                    Else
                        EISSubmitStatus = False
                    End If
                End If
                If IsDBNull(dr("LastEISSubmitDate")) Then
                    txtLastEISSubmit.Text = "Never submitted"
                Else
                    txtLastEISSubmit.Text = dr.Item("LastEISSubmitDate")
                End If

                'Subroutine to show data for fuel burning units (Codes 100, 120, 140, 160, 180 ONLY) or hide if not
                SetFuelBurning(GetNullableString(dr.Item("strUnitTypeCode")))

            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

#Region " Save Emission Unit "

    Protected Sub btnSaveEmissionUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveEmissionUnit1.Click, btnSaveEmissionUnit2.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim StatusCodeOnLoad As String = txtUnitStatusCodeOnLoad.Text
        Dim StatusCodeChanged As String = txtUnitStatusCodeChanged.Text
        Dim ShutDownStatus As String = ""

        If StatusCodeOnLoad = "OP" And (StatusCodeChanged = "PS" Or StatusCodeChanged = "TS") Then
            'Open Unit Status warning
            ShutDownStatus = "S"
            mpeUnitStatusShutdown.Show()
        Else
            'Operating or already shutdown >> Save emission unit
            ShutDownStatus = "O"
            SaveEmissionUnit(FacilitySiteID, UpdateUser, ShutDownStatus)
            txtUnitStatusCodeOnLoad.Text = ddlUnitStatusCode.SelectedValue
            txtUnitStatusCodeChanged.Text = ""
        End If

    End Sub

    Protected Sub btnConfirmShutDownSave_Click(sender As Object, e As System.EventArgs) Handles btnConfirmShutDownSave.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim ShutDownStatus As String = "S"

        SaveEmissionUnit(FacilitySiteID, UpdateUser, ShutDownStatus)
        'Set process active = "0" for all EU processes
        EUProcessesSetActiveZero(txtEmissionsUnitID.Text, FacilitySiteID, UpdateUser)
        lblSaveMessage1.Text = "Emission unit saved successfully"
        lblSaveMessage1.Visible = True
        txtUnitStatusCodeOnLoad.Text = ddlUnitStatusCode.SelectedValue
        txtUnitStatusCodeChanged.Text = ddlUnitStatusCode.SelectedValue
    End Sub

    Private Sub SaveEmissionUnit(ByVal fsid As String, ByVal UpdUser As String, ByVal shutdown As String)

        Dim InventoryYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim EmissionUnitID As String = txtEmissionsUnitID.Text
        Dim UnitDescription As String = txtUnitDescription.Text
        Dim UnitOperationDate As DateTime? = txtUnitOperationDate.Text.ParseAsNullableDateTime()
        Dim UnitDesignCapacity As Double? = txtUnitDesignCapacity.Text.ParseAsNullableDouble()
        Dim MaxNameplateCapacity As Decimal? = txtMaximumNameplateCapacity.Text.ParseAsNullableDecimal()
        Dim UnitComment As String = txtUnitComment.Text
        Dim UnitTypeCode As String = ddlUnitTypeCode.SelectedItem.Value
        Dim UnitStatusCode As String = ddlUnitStatusCode.SelectedItem.Value
        Dim UnitDesCapUoMCode As String = ddlUnitDesignCapacityUOMCode.SelectedItem.Value
        Dim UnitStatusCodeYear = Now.Year
        Dim StatusCodeOnLoad = txtUnitStatusCodeOnLoad.Text
        Dim StatusCodeChanged = txtUnitStatusCodeChanged.Text
        Dim Active As String = "1"

        'Truncate comment if >400 chars
        UnitComment = Left(UnitComment, 400)

        'Truncate emission unit description if > 100 chars
        UnitDescription = Left(UnitDescription, 100)

        If UnitDesignCapacity Is Nothing Then
            UnitDesCapUoMCode = Nothing
        End If

        Try
            Dim query As String

            Dim params = {
                New SqlParameter("@UnitDescription", UnitDescription),
                New SqlParameter("@UnitTypeCode", UnitTypeCode),
                New SqlParameter("@UnitOperationDate", UnitOperationDate),
                New SqlParameter("@UnitDesignCapacity", UnitDesignCapacity),
                New SqlParameter("@UnitDesCapUoMCode", UnitDesCapUoMCode),
                New SqlParameter("@MaxNameplateCapacity", MaxNameplateCapacity),
                New SqlParameter("@UnitComment", UnitComment),
                New SqlParameter("@Active", Active),
                New SqlParameter("@UpdUser", UpdUser),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@EmissionUnitID", EmissionUnitID),
                New SqlParameter("@UnitStatusCode", UnitStatusCode),
                New SqlParameter("@UnitStatusCodeYear", UnitStatusCodeYear)
            }

            If StatusCodeOnLoad = StatusCodeChanged Or StatusCodeChanged = "" Then
                'DON'T update Unit Status and Unit Status Code year
                query = "Update eis_EmissionsUnit Set " &
                    "strUnitDescription = @UnitDescription, " &
                    "strUnitTypeCode = @UnitTypeCode, " &
                    "datUnitOperationDate = @UnitOperationDate, " &
                    "fltUnitDesignCapacity = @UnitDesignCapacity, " &
                    "strUnitDesignCapacityUoMCode = @UnitDesCapUoMCode, " &
                    "numMaximumNameplateCapacity = @MaxNameplateCapacity, " &
                    "strUnitComment = @UnitComment, " &
                    "Active = @Active, " &
                    "UpdateUser = @UpdUser, " &
                    "UpdateDateTime = getdate() " &
                    "where FacilitySiteID = @fsid " &
                    "and EmissionsUnitID = @EmissionUnitID "
            Else
                'Update Unit Status and Unit Status Code year
                query = "Update eis_EmissionsUnit Set " &
                    "strUnitDescription = @UnitDescription, " &
                    "strUnitTypeCode = @UnitTypeCode, " &
                    "strUnitStatusCode = @UnitStatusCode, " &
                    "intUnitStatusCodeYear = @UnitStatusCodeYear, " &
                    "datUnitOperationDate = @UnitOperationDate, " &
                    "fltUnitDesignCapacity = @UnitDesignCapacity, " &
                    "strUnitDesignCapacityUoMCode = @UnitDesCapUoMCode, " &
                    "numMaximumNameplateCapacity = @MaxNameplateCapacity, " &
                    "strUnitComment = @UnitComment, " &
                    "Active = @Active, " &
                    "UpdateUser = @UpdUser, " &
                    "UpdateDateTime = getdate() " &
                    "where FacilitySiteID = @fsid " &
                    "and EmissionsUnitID = @EmissionUnitID "
            End If

            DB.RunCommand(query, params)

            'If emission unit set to temp or perm shutdown, Delete FROM   Reporting Period and set Active for processes = "0"
            If shutdown = "S" Then
                'Remove processes for EU from reporting period
                DeleteRPProcessAndEmissions_EU(InventoryYear, fsid, EmissionUnitID)
                'Set Active = 0 for processes
                EUProcessesSetActiveZero(fsid, txtEmissionsUnitID.Text, UpdUser)
            End If

            lblSaveMessage1.Text = "Emission unit saved successfully"
            lblSaveMessage1.Visible = True

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

#Region "Delete Emission Unit (i.e. set Active = 0) "

    Protected Sub btnConfirmDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmDelete.Click

        Dim EmissionsUnitID As String = txtEmissionsUnitID.Text.ToUpper
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim InventoryYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        DeleteEmissionsUnit(FacilitySiteID, EmissionsUnitID, UpdateUser)
        EUProcessesSetActiveZero(FacilitySiteID, EmissionsUnitID, UpdateUser)
        DeleteEUCtrlAppPollutants(FacilitySiteID, EmissionsUnitID)
        DeleteEUCtrlAppMeasures(FacilitySiteID, EmissionsUnitID)
        DeleteEUControlApproach(FacilitySiteID, EmissionsUnitID)
        DeleteProcessRPApp_EU(FacilitySiteID, EmissionsUnitID)
        DeleteAllProcessControlPollutants_EU(FacilitySiteID, EmissionsUnitID)
        DeleteAllProcessControlMeasures_EU(FacilitySiteID, EmissionsUnitID)
        DeleteProcessControlApproach_EU(FacilitySiteID, EmissionsUnitID)
        lblConfirmDelete1.Text = "Emission Unit " & EmissionsUnitID & " and any associated processes, control approaches, control measures, and pollutants have been deleted."
        lblConfirmDelete2.Text = ""
        btnConfirmDelete.Visible = False
        btnNoDelete.Visible = False
        btnSummary.Visible = True
        mpeDeleteEmissionUnit.Show()

    End Sub

#End Region

    Protected Sub UnitStatusChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnitStatusCode.SelectedIndexChanged

        Dim EmissionUnitID As String = txtEmissionsUnitID.Text

        txtUnitStatusCodeChanged.Text = ddlUnitStatusCode.SelectedValue

    End Sub

    Protected Sub btnCancel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel1.Click, btnCancel2.Click

        Dim EmissionsUnitID As String = txtEmissionsUnitID.Text
        Dim targetpage As String = "~/EIS/emissionunit_details.aspx" & "?eu=" & EmissionsUnitID

        Response.Redirect(targetpage)

    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim EmissionsUnitID As String = txtEmissionsUnitID.Text
        Dim targetpage As String = "~/EIS/emissionunit_details.aspx" & "?eu=" & EmissionsUnitID

        Response.Redirect(targetpage)

    End Sub

    Protected Sub btnSummary1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSummary1.Click

        Response.Redirect("~/EIS/emissionunit_summary.aspx")

    End Sub

    Protected Sub btnSummary2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSummary2.Click

        Response.Redirect("~/EIS/emissionunit_summary.aspx")

    End Sub

    Protected Sub ddlElecGen_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlElecGen.SelectedIndexChanged

        If ddlElecGen.SelectedValue = "Yes" Then
            pnlElecGen.Visible = True
        Else
            pnlElecGen.Visible = False
            txtMaximumNameplateCapacity.Text = ""
        End If

    End Sub

    Private Sub SetFuelBurning(ByVal UnitTypeCode As String)

        If UnitIsFuelBurning(UnitTypeCode) Then
            pnlFuelBurning.Visible = True
            If txtMaximumNameplateCapacity.Text <> "" Then
                pnlElecGen.Visible = True
                ddlElecGen.SelectedValue = "Yes"
            Else
                pnlElecGen.Visible = False
                ddlElecGen.SelectedValue = "No"
            End If
        Else
            pnlFuelBurning.Visible = False
            txtUnitDesignCapacity.Text = ""
            ddlUnitDesignCapacityUOMCode.SelectedIndex = 0
            ddlElecGen.SelectedValue = "No"
            txtMaximumNameplateCapacity.Text = ""
        End If

    End Sub

    Protected Sub ddlUnitTypeCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnitTypeCode.SelectedIndexChanged

        Dim UnitTypeCode As String = ddlUnitTypeCode.SelectedItem.Value

        SetFuelBurning(UnitTypeCode)

    End Sub

    Protected Sub btnSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSummary.Click

        Response.Redirect("~/EIS/emissionunit_summary.aspx")

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