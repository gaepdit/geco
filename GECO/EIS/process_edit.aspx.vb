Imports System.Data.SqlClient

Partial Class eis_process_edit
    Inherits Page
    Public conn, conn1 As New SqlConnection(DBConnectionString)
    Public SCCExists As Boolean
    Public ProcessEISSubmit As Boolean
    Public ProcessUsedInProcessReportingPeriod As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim emissionunitID As String = Request.QueryString("eu")
        Dim processid As String = Request.QueryString("ep")

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            txtProcessID.Text = processid
            LoadProcessDetails(emissionunitID, processid) 'Loads Process Info and sets ProcessEISSubmit based on EIS_Process.strEISSubmit value
            pnlProcess.Visible = True
            ProcessUsedInProcessReportingPeriod = CheckProcessReportingPeriod(FacilitySiteID, emissionunitID, processid)

            If ProcessEISSubmit = True Or ProcessUsedInProcessReportingPeriod = True Then
                btnDeleteProcess.Visible = False
            Else
                btnDeleteProcess.Visible = True
            End If
            lblConfirmDelete1.Text = "Do you want to delete Process " + processid + " for Emission Unit " + emissionunitID + "?"
            btnDeleteSummary.Visible = False
        End If

        ShowFacilityInventoryMenu()
        ShowEISHelpMenu()
        If EISStatus = "2" Then
            ShowEmissionInventoryMenu()
        Else
            HideEmissionInventoryMenu()
        End If
        HideTextBoxBorders(Me)

    End Sub

    Private Sub LoadProcessDetails(ByVal euid As String, ByVal prid As String)
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISSubmit As Boolean

        Try
            Dim query = "select  EIS_PROCESS.PROCESSID, " &
                        "EIS_PROCESS.EMISSIONSUNITID, " &
                        "EIS_PROCESS.SOURCECLASSCODE, " &
                        "EIS_PROCESS.STRPROCESSDESCRIPTION, " &
                        "EIS_PROCESS.STREISSUBMIT, " &
                        "EIS_PROCESS.STRPROCESSCOMMENT " &
                        "FROM EIS_PROCESS where EIS_PROCESS.FACILITYSITEID = @facsiteid " &
                        "and EIS_PROCESS.EMISSIONSUNITID = @euid " &
                        "and EIS_PROCESS.PROCESSID = @prid"

            Dim params = {
                New SqlParameter("@facsiteid", FacilitySiteID),
                New SqlParameter("@euid", euid),
                New SqlParameter("@prid", prid)
            }

            Dim dr = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                If IsDBNull(dr("PROCESSID")) Then
                    txtProcessID.Text = ""
                Else
                    txtProcessID.Text = dr.Item("PROCESSID")
                End If

                If IsDBNull(dr("EMISSIONSUNITID")) Then
                    txtEmissionUnitID.Text = ""
                Else
                    txtEmissionUnitID.Text = dr.Item("EMISSIONSUNITID")
                    txtEmissionUnitDesc.Text = GetEmissionUnitDesc(FacilitySiteID, euid)
                End If

                If IsDBNull(dr("STRPROCESSDESCRIPTION")) Then
                    txtProcessDescription.Text = ""
                Else
                    txtProcessDescription.Text = dr.Item("STRPROCESSDESCRIPTION")
                End If
                If IsDBNull(dr("STRPROCESSCOMMENT")) Then
                    txtProcessComment.Text = ""

                Else
                    txtProcessComment.Text = dr.Item("STRPROCESSCOMMENT")
                End If
                If IsDBNull(dr("SOURCECLASSCODE")) Then
                    txtSourceClassCode.Text = ""
                Else
                    txtSourceClassCode.Text = dr.Item("SOURCECLASSCODE")
                End If
                If IsDBNull(dr.Item("strEISSubmit")) Then
                    ProcessEISSubmit = False
                Else
                    EISSubmit = dr.Item("strEISSubmit")
                    If EISSubmit = "0" Then
                        ProcessEISSubmit = False
                    Else
                        ProcessEISSubmit = True
                    End If
                End If

            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub SCCCheck(Sender As Object, args As ServerValidateEventArgs)
        args.IsValid = IsValidScc(args.Value)
        SCCExists = args.IsValid
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Page.IsValid Then
            SaveProcessInfo()
            lblMessageTop.Text = "Process saved succesfully"
            lblMessageTop.Visible = True
        End If
    End Sub

    Private Sub SaveProcessInfo()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim processID As String = txtProcessID.Text.ToUpper
        Dim euID As String = txtEmissionUnitID.Text.ToUpper
        Dim SCC As String = txtSourceClassCode.Text
        Dim processDescription As String = txtProcessDescription.Text
        Dim processcomment As String = txtProcessComment.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        'Truncate comment if >400
        processcomment = Left(processcomment, 400)

        Try
            Dim query = "Update EIS_PROCESS Set SOURCECLASSCODE = @SCC, " &
                " STRPROCESSDESCRIPTION = @processDescription, " &
                " STRPROCESSCOMMENT = @processcomment, " &
                " UpdateUser = @UpdateUser," &
                " UpdateDateTime = getdate() " &
                " where FACILITYSITEID = @FacilitySiteID" &
                " and EMISSIONSUNITID = @euID" &
                " and ACTIVE = '1' " &
                " and PROCESSID = @processID"

            Dim params = {
                New SqlParameter("@SCC", SCC),
                New SqlParameter("@processDescription", processDescription),
                New SqlParameter("@processcomment", processcomment),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@euID", euID),
                New SqlParameter("@processID", processID)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        SaveProcessInfo()
        lblMessageTop.Text = "Process saved succesfully"
        lblMessageTop.Visible = True
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim Processid As String = txtProcessID.Text
        Dim EmissionUnitId As String = txtEmissionUnitID.Text
        Dim targetpage As String = "Process_details.aspx" & "?eu=" & EmissionUnitId & "&ep=" & Processid
        Response.Redirect(targetpage)
    End Sub
    ' top return to Details button
    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click
        Dim eu As String = txtEmissionUnitID.Text
        Dim ep As String = txtProcessID.Text
        Dim targetpage As String = "process_details.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnDeleteProcessOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteProcessOK.Click
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        DeleteProcessRPApp(FacilitySiteID, EmissionsUnitID, ProcessID)
        DeleteAllProcessControlPollutants(FacilitySiteID, EmissionsUnitID, ProcessID)
        DeleteAllProcessControlMeasures(FacilitySiteID, EmissionsUnitID, ProcessID)
        DeleteProcessControlApproach(FacilitySiteID, EmissionsUnitID, ProcessID)
        DeleteProcess(FacilitySiteID, EmissionsUnitID, ProcessID, UpdateUser)
        lblConfirmDelete1.Text = "Process " + ProcessID + " for Emission Unit " + EmissionsUnitID + " has been deleted."
        btnNoDelete.Visible = False
        btnDeleteProcessOK.Visible = False
        btnDeleteSummary.Visible = True
        lblConfirmDelete2.Visible = False
        mpeDeleteProcess.Show()

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