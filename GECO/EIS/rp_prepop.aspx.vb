Imports System.Data.SqlClient

Partial Class EIS_rp_prepop
    Inherits Page

    Private eiYear As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        EIAccessCheck(EISAccessCode, EISStatus)

        If Not IsPostBack Then
            eiYear = GetCookie(EisCookie.EISMaxYear)
            LoadEIYears(FacilitySiteID)

            HideFacilityInventoryMenu()
            HideEmissionInventoryMenu()
            ShowEISHelpMenu()
        End If

    End Sub

    Protected Sub btnPrePopulate_Click(sender As Object, e As EventArgs) Handles btnPrePopulate.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim pastYear As String = ddlEIYears.SelectedValue
        eiYear = GetCookie(EisCookie.EISMaxYear)

        If ddlEIYears.SelectedValue = "-Make Selection-" Then
            btnPrePopulate.Enabled = False
        ElseIf ddlEIYears.SelectedValue = "Do not prepopulate" Then
            Response.Redirect("Default.aspx")
        Else
            PrePopulateEI(FacilitySiteID, pastYear, eiYear)
            Response.Redirect("Default.aspx")
        End If

    End Sub

    Private Sub LoadEIYears(fsid As String)
        ddlEIYears.Items.Add("-Make Selection-")
        ddlEIYears.Items.Add("Do not prepopulate")

        Try
            Dim query = "Select InventoryYear FROM eis_Admin " &
                " where FacilitySiteID = @fsid and " &
                " strOptOut = '0' And " &
                " EISStatusCode = '5' and " &
                " InventoryYear > 2010 " &
                " Order By InventoryYear Desc"

            Dim param As New SqlParameter("@fsid", fsid)

            Dim dt = DB.GetDataTable(query, param)

            For Each dr In dt.Rows
                ddlEIYears.Items.Add(dr.Item("InventoryYear"))
            Next

            ddlEIYears.SelectedValue = "-Make Selection-"

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click

        Response.Redirect("Default.aspx")

    End Sub

    Protected Sub ddlEIYears_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEIYears.SelectedIndexChanged

        If ddlEIYears.SelectedValue = "-Make Selection-" Then
            'Do nothing - leave button disabled
            btnPrePopulate.Text = "Continue"
            btnPrePopulate.Enabled = False
        ElseIf ddlEIYears.SelectedValue = "Do not prepopulate" Then
            btnPrePopulate.Text = "Continue without Prepopulating"
            btnPrePopulate.Enabled = True
        Else
            btnPrePopulate.Text = "Continue & Prepopulate with " & ddlEIYears.SelectedValue & " Data"
            btnPrePopulate.Enabled = True
        End If

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