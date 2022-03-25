Partial Class EventRegistration_Default
    Inherits Page

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.RedirectPermanent("~/Events/")
    End Sub

End Class
