Imports System.Diagnostics
Imports GECO.GecoModels

<DebuggerStepThrough()>
Public Module CookieHelper

    Public Const COOKIE_EXPIRATION_DAYS = 1D
    Public Const COOKIE_EXPIRATION_LONGTERM = 14D

    Public Enum Cookie
        AirsNumber
    End Enum

    Public Enum EisCookie
        ConfNumber
        DateFinalize
        EISAccess
        EISMaxYear
        EISStatus
        Enrollment
        OptOut
    End Enum

    Public Enum SessionCookie
        Series
        Token
    End Enum

    <Obsolete("Use CurrentUser Session item instead")>
    Public Enum GecoCookie
        UserEmail
        UserID
        UserName
    End Enum

    Public Enum CookieCollection
        EISAccessInfo
        GECOUserInfo
        SessionCookie
    End Enum

    Public Function GetCookie(cookie As Cookie) As String
        Dim c = GetHttpCookie(cookie.ToString)

        ' Must test for null value before attempting to decrypt
        If c Is Nothing OrElse String.IsNullOrEmpty(c.Value) Then
            Return Nothing
        End If

        Return EncryptDecrypt.DecryptText(c.Value)
    End Function

    Public Function GetCookie(cookie As EisCookie) As String
        Return GetCookieCollectionItem(CookieCollection.EISAccessInfo.ToString, cookie.ToString)
    End Function

    Public Function GetCookie(cookie As SessionCookie) As String
        Return GetCookieCollectionItem(CookieCollection.SessionCookie.ToString, cookie.ToString)
    End Function

    Public Function GetCookie(cookie As GecoCookie) As String
        Return GetCookieCollectionItem(CookieCollection.GECOUserInfo.ToString, cookie.ToString)
    End Function

    Private Function GetCookieCollectionItem(name As String, item As String) As String
        Dim collection = GetCookieCollection(name)

        ' Must test for null value before attempting to decrypt
        If collection Is Nothing OrElse collection.Count = 0 OrElse collection.Item(item) Is Nothing Then
            Return Nothing
        End If

        Return EncryptDecrypt.DecryptText(collection(item))
    End Function

    Private Function GetCookieCollection(name As String) As NameValueCollection
        Dim httpCookie = GetHttpCookie(name)

        If httpCookie Is Nothing OrElse Not httpCookie.HasKeys Then
            Return Nothing
        End If

        Return httpCookie.Values
    End Function

    Private Function GetHttpCookie(name As String) As HttpCookie
        Dim httpCookies = HttpContext.Current.Request.Cookies

        If httpCookies(name) Is Nothing Then
            Return Nothing
        End If

        Return httpCookies(name)
    End Function

    Public Sub SetCookie(cookie As Cookie, value As String)
        Dim response = HttpContext.Current.Response

        Dim c As New HttpCookie(cookie.ToString) With {
            .Value = EncryptDecrypt.EncryptText(value),
            .Expires = DateTime.Now.AddDays(COOKIE_EXPIRATION_DAYS)
        }

        response.Cookies.Add(c)
    End Sub

    Public Sub ClearCookie(cookie As Cookie)
        ClearCookie(cookie.ToString)
    End Sub

    Public Sub ClearCookie(cookie As CookieCollection)
        ClearCookie(cookie.ToString)
    End Sub

    Private Sub ClearCookie(cookieName As String)
        If GetHttpCookie(cookieName) IsNot Nothing Then
            HttpContext.Current.Response.Cookies.Add(New HttpCookie(cookieName) With {.Expires = DateTime.Now.AddDays(-1D)})
        End If
    End Sub

    Public Sub ClearAllCookies()
        Dim request = HttpContext.Current.Request
        Dim response = HttpContext.Current.Response

        For i = 0 To request.Cookies.Count - 1
            response.Cookies.Add(New HttpCookie(request.Cookies(i).Name) With {.Expires = DateTime.Now.AddDays(-1)})
        Next
    End Sub

    Public Sub CreateSessionCookie(userSession As UserSession)
        Dim response = HttpContext.Current.Response

        Dim c As New HttpCookie(CookieCollection.SessionCookie.ToString)
        c.Values(SessionCookie.Series.ToString) = EncryptDecrypt.EncryptText(userSession.Series)
        c.Values(SessionCookie.Token.ToString) = EncryptDecrypt.EncryptText(userSession.Token)
        c.Expires = Now.AddDays(COOKIE_EXPIRATION_LONGTERM)

        response.Cookies.Add(c)
    End Sub

    <Obsolete("Use CurrentUser Session item instead")>
    Public Sub CreateGecoUserCookie(user As GecoUser)
        Dim response = HttpContext.Current.Response

        Dim c As New HttpCookie(CookieCollection.GECOUserInfo.ToString)
        c.Values(GecoCookie.UserID.ToString) = EncryptDecrypt.EncryptText(user.UserId)
        c.Values(GecoCookie.UserEmail.ToString) = EncryptDecrypt.EncryptText(user.Email)
        c.Values(GecoCookie.UserName.ToString) = EncryptDecrypt.EncryptText(user.FullName)
        c.Expires = Now.AddDays(COOKIE_EXPIRATION_DAYS)

        response.Cookies.Add(c)
    End Sub

End Module
