Partial Class EIS_help_ei
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        HideFacilityInventoryMenu()
        HideEmissionInventoryMenu()
        HideSubmitMenu()

    End Sub

#Region "  Menu Routines  "

    Private Sub HideFacilityInventoryMenu()

        Dim menu = CType(Master.FindControl("pnlFacilityInventory"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

    Private Sub HideEmissionInventoryMenu()

        Dim menu = CType(Master.FindControl("pnlEmissionInventory"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

    Private Sub HideSubmitMenu()

        Dim menu = CType(Master.FindControl("pnlSubmit"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

        menu = CType(Master.FindControl("pnlReset"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

#End Region

End Class