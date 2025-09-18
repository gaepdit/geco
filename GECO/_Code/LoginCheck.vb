Public Module LoginCheck

    Public Sub CompleteRedirect(toPage As String)
        HttpContext.Current.Response.Redirect(toPage, False)
        HttpContext.Current.ApplicationInstance.CompleteRequest()
    End Sub

    Public Sub CompleteRedirect(toPage As String, ByRef terminate As Boolean)
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
