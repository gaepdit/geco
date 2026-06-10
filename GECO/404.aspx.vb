Partial Class Http404Page
    Inherits Page

    Private Sub Http404Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.TrySkipIisCustomErrors = True
        Response.StatusCode = 404
        Response.StatusDescription = "Not Found"

        lnkContact.NavigateUrl = $"mailto:{ConfigurationManager.AppSettings("GecoEmailSender")}"
    End Sub
End Class
