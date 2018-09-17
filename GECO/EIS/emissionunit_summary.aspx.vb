Imports System.Data.SqlClient

Partial Class eis_emissionunit_summary
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim DeletedEUExist As Boolean

        FIAccessCheck(EISAccessCode)

        DeletedEUExist = CheckDeletedEUExist(FacilitySiteID)
        If DeletedEUExist Then
            pnlDeletedEmissionUnits.Visible = True
        Else
            pnlDeletedEmissionUnits.Visible = False
        End If

        LoadGVWEmissionUnitSummary(FacilitySiteID)

        ShowFacilityInventoryMenu()
        ShowEISHelpMenu()
        If EISStatus = "2" Then
            ShowEmissionInventoryMenu()
        Else
            HideEmissionInventoryMenu()
        End If

    End Sub

    Private Sub LoadGVWEmissionUnitSummary(ByVal fsid As String)
        Dim query = "SELECT eis_EmissionsUnit.EmissionsUnitID " &
            ", strUnitDescription " &
            ", (SELECT strDesc " &
            "FROM eislk_unittypecode " &
            "WHERE eislk_unittypecode.unittypecode = eis_emissionsunit.strunittypecode " &
            "AND eislk_unittypecode.Active = '1' " &
            ") AS unittypecode " &
            ", CASE " &
            "WHEN eis_emissionsunit.strUnitStatusCode = 'OP' " &
            "THEN 'Operating' " &
            "WHEN eis_emissionsunit.strUnitStatusCode = 'PS' " &
            "THEN 'Perm Shutdown' " &
            "WHEN eis_emissionsunit.strUnitStatusCode = 'TS' " &
            "THEN 'Temp Shutdown' " &
            "ELSE '' " &
            "END AS strUnitStatusCode " &
            ", eis_EmissionsUnit.LastEISSubmitDate " &
            ", CASE " &
            "WHEN eis_UnitControlApproach.EmissionsUnitID IS NULL " &
            "THEN 'No' " &
            "ELSE 'Yes' " &
            "END AS ControlApproach " &
            "FROM EIS_EmissionsUnit " &
            "LEFT JOIN eis_UnitControlApproach " &
            "ON eis_EmissionsUnit.FacilitySiteID = EIS_UnitControlApproach.FacilitySiteID " &
            "AND EIS_EmissionsUnit.FacilitySiteID = EIS_UnitControlApproach.FacilitySiteID " &
            "AND eis_EmissionsUnit.EmissionsUnitID = eis_UnitControlApproach.EmissionsUnitID " &
            "WHERE eis_EmissionsUnit.ACTIVE = '1' " &
            "AND eis_EmissionsUnit.FacilitySiteID = @fsid " &
            "ORDER BY eis_EmissionsUnit.EmissionsUnitID "

        Dim param As New SqlParameter("@fsid", fsid)

        gvwEmissionUnitSummary.DataSource = DB.GetDataTable(query, param)

        gvwEmissionUnitSummary.DataBind()
    End Sub

    Private Sub InsertEmissionUnit(ByVal fsid As String, ByVal euid As String)

        'Code to insert a new emission unit
        'Reminder: insert only FacilitySiteID, Unit/Stack/Etc ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim UnitDescription As String = Left(txtNewEmissionUnitDesc.Text, 100)
        Dim UnitStatusCode As String = "OP"
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Try
            Dim query = "Insert into eis_EmissionsUnit (" &
                "FacilitySiteID, " &
                "EmissionsUnitID, " &
                "strUnitDescription, " &
                "strUnitStatusCode, " &
                "fltUnitDesignCapacity, " &
                "Active, " &
                "UpdateUser, " &
                "UpdateDateTime, " &
                "CreateDateTime) " &
                "Values (" &
                "@fsid, " &
                "@euid, " &
                "@UnitDescription, " &
                "@UnitStatusCode, " &
                "null, " &
                "'1', " &
                "@UpdateUser, " &
                "getdate(), " &
                "getdate()) "

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@UnitDescription", UnitDescription),
                New SqlParameter("@UnitStatusCode", UnitStatusCode),
                New SqlParameter("@UpdateUser", UpdateUser)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnShowDeletedEU_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDeletedEU.Click

        Dim btnText As String = Left(btnShowDeletedEU.Text, 4)

        If btnText = "Show" Then
            LoadGVWDeletedEU()
            btnShowDeletedEU.Text = "Hide Deleted Emission Units"
        Else
            UnloadGVWDeletedEU()
            btnShowDeletedEU.Text = "Show Deleted Emission Units"
        End If

    End Sub

    Private Sub LoadGVWDeletedEU()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        Dim query = "select EmissionsUnitID, strUnitDescription " &
            "FROM eis_EmissionsUnit " &
            "where " &
            "FacilitySiteID = @FacilitySiteID " &
            "and Active = '0' " &
            "order by EmissionsUnitID"

        gvwDeletedEU.DataSource = DB.GetDataTable(query, New SqlParameter("@FacilitySiteID", FacilitySiteID))

        gvwDeletedEU.DataBind()
    End Sub

    Private Sub UnloadGVWDeletedEU()
        Dim query = "select EmissionsUnitID, strUnitDescription " &
            "FROM eis_EmissionsUnit " &
            "where 1 = 0"

        gvwDeletedEU.DataSource = DB.GetDataTable(query)

        gvwDeletedEU.DataBind()
    End Sub

    Protected Sub EmissionsUnitIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = args.Value.ToUpper
        Dim targetpage As String = "~/EIS/emissionunit_edit.aspx" & "?eu=" & EmissionsUnitID
        Dim EUIDActive = CheckEUIDExist(FacilitySiteID, EmissionsUnitID)

        Select Case EUIDActive
            Case UnitActiveStatus.Inactive
                args.IsValid = True
                Response.Redirect(targetpage)
            Case UnitActiveStatus.Active
                args.IsValid = False
                cusvEmissionUnitID.ErrorMessage = " Emission Unit " + EmissionsUnitID + " is already in use.  Please enter another."
                txtNewEmissionsUnitID.Text = ""
                btnAdd_ModalPopupExtender.Show()
            Case UnitActiveStatus.DoesNotExist
                args.IsValid = True
                InsertEmissionUnit(FacilitySiteID, EmissionsUnitID)
                Response.Redirect(targetpage)
        End Select

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        txtNewEmissionsUnitID.Text = ""
        txtNewEmissionUnitDesc.Text = ""

    End Sub

#Region " Undelete Routines "

    Protected Sub gvwDeletedEU_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwDeletedEU.RowCommand

        If e.CommandName = "Undelete" Then
            Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
            Dim InventoryYear As String = GetCookie(EisCookie.EISMaxYear)
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvwDeletedEU.Rows(index)
            Dim EmissionsUnitID As String = Server.HtmlDecode(row.Cells(0).Text)
            Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
            Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
            Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
            Dim targetpage As String = "~/EIS/emissionunit_edit.aspx" & "?eu=" & EmissionsUnitID

            UndeleteEU(FacilitySiteID, EmissionsUnitID, UpdateUser)
            UndeleteEUProcesses(FacilitySiteID, EmissionsUnitID, UpdateUser)
            Response.Redirect(targetpage)

        End If

    End Sub

    Private Sub UndeleteEU(ByVal fsid As String, ByVal euid As String, ByVal uuser As String)
        Try
            Dim query = "Update eis_EmissionsUnit Set " &
                "Active = '1', " &
                "UpdateUser = @uuser, " &
                "UpdateDateTime = getdate() " &
                "where FacilitySiteID = @fsid " &
                "and EmissionsUnitID = @euid "

            Dim params = {
                New SqlParameter("@uuser", uuser),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub UndeleteEUProcesses(ByVal fsid As String, ByVal euid As String, ByVal uuser As String)
        Try
            Dim query = "Update eis_Process Set " &
                "Active = '1', " &
                "UpdateUser = @uuser, " &
                "UpdateDateTime = getdate() " &
                "where FacilitySiteID = @fsid " &
                "and EmissionsUnitID = @euid "

            Dim params = {
                New SqlParameter("@uuser", uuser),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

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