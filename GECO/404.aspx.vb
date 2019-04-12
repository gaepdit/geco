Partial Class Http404Page
    Inherits Page

    Private ReadOnly GecoEmailSender As String = ConfigurationManager.AppSettings("GecoEmailSender")

    Private Sub Http404Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.TrySkipIisCustomErrors = True
        Response.StatusCode = 404
        Response.StatusDescription = "Not Found"

        lnkContact.NavigateUrl = String.Format("mailto:{0}", GecoEmailSender)
    End Sub
End Class