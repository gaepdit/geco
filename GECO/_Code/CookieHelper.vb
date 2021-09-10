Imports GECO.GecoModels

<DebuggerStepThrough()>
Public Module CookieHelper

    Public Const COOKIE_EXPIRATION_DAYS As Decimal = 1D
    Public Const COOKIE_EXPIRATION_LONGTERM As Decimal = 14D

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

    Public Enum CookieCollection
        EISAccessInfo
        SessionCookie
    End Enum

    Public Function GetCookie(cookie As Cookie) As String
        Dim c = GetHttpCookie(cookie.ToString)

        If c Is Nothing OrElse String.IsNullOrEmpty(c.Value) Then
            Return Nothing
        End If

        Return DecryptText(c.Value)
    End Function

    Public Function GetCookie(cookie As EisCookie) As String
        Return GetCookieCollectionItem(CookieCollection.EISAccessInfo.ToString, cookie.ToString)
    End Function

    Public Function GetCookie(cookie As SessionCookie) As String
        Return GetCookieCollectionItem(CookieCollection.SessionCookie.ToString, cookie.ToString)
    End Function

    Private Function GetCookieCollectionItem(name As String, item As String) As String
        Dim collection = GetCookieCollection(name)

        If collection Is Nothing OrElse collection.Count = 0 OrElse collection.Item(item) Is Nothing Then
            Return Nothing
        End If

        Return DecryptText(collection(item))
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
            .Value = EncryptText(value),
            .Expires = Date.Now.AddDays(COOKIE_EXPIRATION_DAYS)
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
            HttpContext.Current.Response.Cookies.Add(New HttpCookie(cookieName) With {.Expires = Date.Now.AddDays(-1D)})
        End If
    End Sub

    Public Sub ClearAllCookies()
        Dim request = HttpContext.Current.Request
        Dim response = HttpContext.Current.Response

        For i = 0 To request.Cookies.Count - 1
            response.Cookies.Add(New HttpCookie(request.Cookies(i).Name) With {.Expires = Date.Now.AddDays(-1)})
        Next
    End Sub

    Public Sub CreateSessionCookie(userSession As UserSession)
        NotNull(userSession, NameOf(userSession))

        Dim response = HttpContext.Current.Response

        Dim c As New HttpCookie(CookieCollection.SessionCookie.ToString)

        With c
            .Values(SessionCookie.Series.ToString) = EncryptText(userSession.Series)
            .Values(SessionCookie.Token.ToString) = EncryptText(userSession.Token)
            .Expires = Date.Now.AddDays(COOKIE_EXPIRATION_LONGTERM)
        End With

        response.Cookies.Add(c)
    End Sub

End Module
