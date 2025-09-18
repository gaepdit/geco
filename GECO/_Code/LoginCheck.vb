Public Module LoginCheck

    Public Function UserIsLoggedIn() As Boolean
        Return SessionItemExists(GecoSession.CurrentUser)
    End Function

    Public Sub MainLoginCheck()
        If Not UserIsLoggedIn() Then
            HttpContext.Current.Response.Redirect("~/Login.aspx")
        End If
    End Sub

    ' Checks if appropriate cookies/session data are set or redirects to Facility Home or User Home
    ' (Would fail if a direct URL is entered instead of navigating from Facility Home)

    Public Sub AirsSelectedCheck()
        If GetCookie(Cookie.AirsNumber) Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
        End If
    End Sub

End Module
