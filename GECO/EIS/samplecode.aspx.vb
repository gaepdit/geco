Partial Class eis_template
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Below are the sqlDataSource Select commands for the gridviews - copy and edit and place where needed changing the ID etc
        'Remember the ID must match the ID of the sqlDataSource that accompanies the gridview in the source view
        'REMINDER: When transitioning to PRODUCTION must edit eis_dbfunctions.vb

        Response.Redirect("~EIS/Default.aspx")
        SqlDataSourceID1.ConnectionString = oradb
        SqlDataSourceID1.ProviderName = setProviderName()
        SqlDataSourceID1.SelectCommand = "select emissionsunitid, strunitdescription, strunittypecode FROM EIS_EmissionsUnit Where emissionsunitid = 'blah' Order by emissionsunitid"
        GridView1.DataBind()

        HideFacilityInventoryMenu()
        HideEmissionInventoryMenu()
        HideEISHelpMenu()

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