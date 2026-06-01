Partial Class Http404Page
    Inherits Page

    Protected Overrides Sub OnLoad(e As EventArgs)
        AddBreadcrumb(Request, NameOf(Http404Page), ID)
        MyBase.OnLoad(e)
    End Sub

    Private Sub Http404Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Response.TrySkipIisCustomErrors = True
        Response.StatusCode = 404
        Response.StatusDescription = "Not Found"

        lnkContact.NavigateUrl = $"mailto:{ConfigurationManager.AppSettings("GecoEmailSender")}"
    End Sub
End Class
