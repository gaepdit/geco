Public Module LoginCheck

    Public Sub CompleteRedirect(toPage As String)
        Dim data As New Dictionary(Of String, Object) From {{"To Page", toPage}, {"Terminating", "False"}}
        AddBreadcrumb("Page redirect", data)

        HttpContext.Current.Response.Redirect(toPage, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

    Public Sub CompleteRedirect(toPage As String, ByRef terminate As Boolean)
        Dim data As New Dictionary(Of String, Object) From {{"To Page", toPage}, {"Terminating", "True"}}
        AddBreadcrumb("Page redirect", data)

        terminate = True
        HttpContext.Current.Response.Redirect(toPage, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

    Public Function UserIsLoggedIn() As Boolean
        Return SessionItemExists(GecoSession.CurrentUser)
    End Function

    Public Function MainLoginCheck() As Boolean
        If UserIsLoggedIn() Then Return False

        HttpContext.Current.Response.Redirect("~/Login.aspx", False)
        Return True
    End Function

End Module
