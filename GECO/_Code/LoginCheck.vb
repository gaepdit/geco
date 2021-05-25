Public Module LoginCheck

    Public Function UserIsLoggedIn() As Boolean
        Return SessionItemExists(GecoSession.CurrentUser)
    End Function

    Public Sub MainLoginCheck(Optional returnUrl As String = Nothing)
        If Not UserIsLoggedIn() Then
            Dim path As String = "~/Login.aspx"

            If returnUrl IsNot Nothing Then
                path = "~/Login.aspx?ReturnUrl=" & returnUrl
            End If

            HttpContext.Current.Response.Redirect(path)
        End If
    End Sub

    ' Checks if appropriate cookies/session data are set or redirects to Facility Home or User Home
    ' (Would fail if a direct URL is entered instead of navigating from Facility Home)

    Public Sub AirsSelectedCheck()
        If GetCookie(Cookie.AirsNumber) Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
        End If
    End Sub

    Public Sub EisLoginCheck()
        AirsSelectedCheck()

        If GetCookie(EisCookie.EISAccess) Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Facility/")
        End If
    End Sub

End Module
