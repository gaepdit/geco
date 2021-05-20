Imports GECO.GecoModels

Partial Class _Default
    Inherits Page

    ' Page load 

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("do") = "SignOut" Then
                ClearCurrentLogin()
            ElseIf UserIsLoggedIn() Then
                Response.Redirect("~/Home/")
            Else
                GetUserFromSession()
            End If

            ClearCurrentLogin()
        End If
    End Sub

    Private Sub ClearCurrentLogin()
        Session.Clear()
        ClearAllCookies()
    End Sub

    Private Sub GetUserFromSession()

        Dim series As String = GetCookie(SessionCookie.Series)
        Dim token As String = GetCookie(SessionCookie.Token)

        If String.IsNullOrEmpty(series) OrElse String.IsNullOrEmpty(token) Then
            Return
        End If

        Dim userSession As New UserSession(series, token)
        Dim gecoUser As New GecoUser

        If GetSavedUserSession(userSession, gecoUser) Then
            SessionAdd(GecoSession.CurrentUser, gecoUser)
            CreateSessionCookie(userSession)

            Dim strRedirect As String = Request.QueryString("ReturnUrl")

            If String.IsNullOrEmpty(strRedirect) Then
                Response.Redirect("~/Home/")
            Else
                Response.Redirect(strRedirect)
            End If
        End If

    End Sub

End Class
