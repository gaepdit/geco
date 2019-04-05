Imports System.Data.SqlClient

Partial Class eis_releasepoint_summary
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim DeletedRPExist As Boolean

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then

            loadFugitivesgvw()
            loadStacksgvw()
            loadStackTypeDDL()

            DeletedRPExist = CheckDeletedRPExist(FacilitySiteID)
            If DeletedRPExist Then
                pnlDeletedRP.Visible = True
            Else
                pnlDeletedRP.Visible = False
            End If

            ShowFacilityInventoryMenu()
            ShowEISHelpMenu()
            If EISStatus = "2" Then
                ShowEmissionInventoryMenu()
            Else
                HideEmissionInventoryMenu()
            End If
            HideTextBoxBorders(Me)
        End If

    End Sub

    Private Sub loadFugitivesgvw()

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        'Fugitive Release Point Gridview
        SqlDataSourceID1.ConnectionString = DBConnectionString

        SqlDataSourceID1.SelectCommand = "select " &
                "ReleasepointID, " &
                "strRPDescription, " &
                "(select strDesc FROM eislk_RPStatusCode where " &
                "eis_ReleasePoint.strRPStatusCode = eislk_RPStatusCode.RPStatusCode and " &
                "eislk_RPStatusCode.Active = '1') as RPStatus, " &
                "LastEISSubmitDate " &
                "from " &
                "eis_ReleasePoint " &
                "where " &
                "FacilitySiteID = @FacilitySiteID and " &
                "Active = '1' and " &
                "eis_ReleasePoint.strRPTypeCode = '1' " &
                "order by ReleasePointID"

        SqlDataSourceID1.SelectParameters.Add("FacilitySiteID", FacilitySiteID)

        gvwFugRPSummary.DataBind()

    End Sub

    Private Sub loadStacksgvw()

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        'Stack Release Point Gridview
        SqlDataSourceID2.ConnectionString = DBConnectionString

        SqlDataSourceID2.SelectCommand = "select " &
                    "ReleasePointID, strRPDescription, " &
                    "(select eislk_RPTypeCode.strDesc FROM eislk_RPTypeCode where " &
                    "eislk_RPTypeCode.RPTypeCode = eis_ReleasePoint.strRPTypeCode and eislk_RPTypeCode.Active = '1') as RPTypeCode, " &
                    "numRPStackHeightMeasure, " &
                    "numRPExitGasFlowRateMeasure, " &
                    "(select eislk_RPStatusCode.strDesc FROM eislk_RPStatusCode where " &
                    "eislk_RPStatusCode.RPStatusCode = eis_ReleasePoint.strRPStatusCode and eislk_RPStatusCode.Active = '1') as RPStatusCode, " &
                    "LastEISSubmitDate " &
                    "FROM eis_ReleasePoint " &
                    "where " &
                    "FacilitySiteID = @FacilitySiteID and " &
                    "Active = '1' and " &
                    "eis_ReleasePoint.strRPTypeCode <> '1' " &
                    "order by ReleasePointID "

        SqlDataSourceID2.SelectParameters.Add("FacilitySiteID", FacilitySiteID)

        gvwRPSummary.DataBind()

    End Sub

    Private Sub loadStackTypeDDL()
        ddlRPtypeCode.Items.Add("--Select Stack Type--")
        Try
            Dim query As String = "select strdesc, RPTypeCode FROM EISLK_RPTYPECODE " &
                "where EISLK_RPTYPECODE.active = '1' " &
                "and EISLK_RPTYPECODE.strdesc <> 'Fugitive' order by strdesc"

            Dim dt As DataTable = DB.GetDataTable(query)

            For Each dr As DataRow In dt.Rows
                Dim newListItem As New ListItem With {
                    .Text = dr.Item("strdesc"),
                    .Value = dr.Item("RPTypeCode")
                }
                ddlRPtypeCode.Items.Add(newListItem)
            Next
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub InsertFugitiveRP()

        InsertReleasePoint(GetCookie(Cookie.AirsNumber), txtNewFugitiveRP.Text.ToUpper, txtNewFugitiveRPDesc.Text, "1")

    End Sub

    Sub FugitiveRPIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim Fugitiveid As String = args.Value.ToUpper
        Dim targetpage As String = "fugitive_edit.aspx" & "?fug=" & Fugitiveid
        Dim FugitiveActive As String = CheckReleasePointIDexist(FacilitySiteID, Fugitiveid)

        Select Case FugitiveActive
            Case "0"
                args.IsValid = True
                Response.Redirect(targetpage)
            Case "1"
                args.IsValid = False
                cusvFugitiveID.ErrorMessage = " Release Point " + Fugitiveid + " is already in use.  Please enter another."
                txtNewFugitiveRP.Text = ""
                btnAddFugitiveRP_ModalPopupExtender.Show()
            Case "n"
                args.IsValid = True
                InsertFugitiveRP()
                Response.Redirect(targetpage)
        End Select

    End Sub

    'Begin Stack routines

    Private Sub InsertStack()

        InsertReleasePoint(GetCookie(Cookie.AirsNumber), txtNewStackID.Text.ToUpper, txtNewStackDesc.Text, ddlRPtypeCode.SelectedValue)

    End Sub

    Sub StackIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim StackID As String = args.Value.ToUpper
        Dim Targetpage As String = "stack_edit.aspx" & "?stk=" & StackID
        Dim StackActive As String = CheckReleasePointIDexist(FacilitySiteID, StackID)

        Select Case StackActive
            Case "0"
                args.IsValid = True
                Response.Redirect(Targetpage)
            Case "1"
                args.IsValid = False
                cusvStackID.ErrorMessage = " Stack " + StackID + " is already in use.  Please enter another."
                txtNewStackID.Text = ""
                btnAddStack_ModalPopupExtender.Show()
            Case "n"
                args.IsValid = True
                InsertStack()
                Response.Redirect(Targetpage)
        End Select
    End Sub

    Protected Sub btnCancelNEWFugitiveRP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelNEWFugitiveRP.Click

        txtNewFugitiveRP.Text = ""
        txtNewFugitiveRPDesc.Text = ""

    End Sub

    Protected Sub btnCancelNewStack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelNewStack.Click

        txtNewStackID.Text = ""
        txtNewStackDesc.Text = ""

    End Sub

#Region "  Deleted Release Point Routines  "

    Protected Sub btnShowDeletedRP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDeletedRP.Click

        Dim btnText As String = Left(btnShowDeletedRP.Text, 4)

        If btnText = "Show" Then
            LoadGVWDeletedRP()
            btnShowDeletedRP.Text = "Hide Deleted Release Points"
        Else
            UnloadGVWDeletedRP()
            btnShowDeletedRP.Text = "Show Deleted Release Points"
        End If

    End Sub

    Private Sub LoadGVWDeletedRP()

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        sqldsDeletedRP.ConnectionString = DBConnectionString

        sqldsDeletedRP.SelectCommand = "select eis_ReleasePoint.ReleasePointID, eis_ReleasePoint.strRPDescription, " &
                            "(select eislk_RPTypeCode.strDesc FROM eislk_RPTypeCode where eislk_RPTypeCode.RPTypeCode = eis_ReleasePoint.strRPTypeCode) as strRPType " &
                            "FROM eis_ReleasePoint " &
                            "where " &
                            "FacilitySiteID = @FacilitySiteID " &
                            "and Active = '0' " &
                            "order by eis_ReleasePoint.ReleasePointID"

        sqldsDeletedRP.SelectParameters.Add("FacilitySiteID", FacilitySiteID)

        gvwDeletedRP.DataBind()

    End Sub

    Private Sub UnloadGVWDeletedRP()

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        sqldsDeletedRP.ConnectionString = DBConnectionString

        sqldsDeletedRP.SelectCommand = "select eis_ReleasePoint.ReleasePointID, eis_ReleasePoint.strRPDescription, " &
                            "(select eislk_RPTypeCode.strDesc FROM eislk_RPTypeCode where eislk_RPTypeCode.RPTypeCode = eis_ReleasePoint.strRPTypeCode) as strRPType " &
                            "FROM eis_ReleasePoint " &
                            "where " &
                            "FacilitySiteID = @FacilitySiteID " &
                            "and Active = '999' " &
                            "order by eis_ReleasePoint.ReleasePointID"

        sqldsDeletedRP.SelectParameters.Add("FacilitySiteID", FacilitySiteID)

        gvwDeletedRP.DataBind()

    End Sub

    Protected Sub gvwDeletedRP_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwDeletedRP.RowCommand

        If e.CommandName = "Undelete" Then
            Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            Dim row As GridViewRow = gvwDeletedRP.Rows(index)
            Dim ReleasePointID As String = Server.HtmlDecode(row.Cells(0).Text)
            Dim StackType As String = Server.HtmlDecode(row.Cells(2).Text)
            Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
            Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
            Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
            Dim targetpage As String = ""

            If StackType = "Fugitive" Then
                targetpage = "fugitive_edit.aspx" & "?fug=" & ReleasePointID
            Else
                targetpage = "stack_edit.aspx" & "?stk=" & ReleasePointID
            End If
            UndeleteRP(FacilitySiteID, ReleasePointID, UpdateUser)
            Response.Redirect(targetpage)

        End If

    End Sub

    Private Sub UndeleteRP(ByVal fsid As String, ByVal rpid As String, ByVal uuser As String)
        Try
            Dim query As String = "Update eis_ReleasePoint Set " &
                             "Active = '1', " &
                             "UpdateUser = @uuser, " &
                             "UpdateDateTime = getdate() " &
                             "where FacilitySiteID = @fsid " &
                             "and ReleasePointID = @rpid "

            Dim params As SqlParameter() = {
                New SqlParameter("@uuser", uuser),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@rpid", rpid)
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