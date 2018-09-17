Partial Class EventRegistration_Default
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            gvwEventList.DataSource = GetActiveEvents()
            gvwEventList.DataBind()

            If UserIsLoggedIn() Then
                pnlLoginLinks.Visible = False
            End If
        End If
    End Sub

End Class