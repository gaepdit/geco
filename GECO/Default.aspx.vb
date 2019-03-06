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

            Master.IncludeSignInLink = True

            ClearCurrentLogin()
        End If
    End Sub

    Private Sub ClearCurrentLogin()
        Session.Clear()
        ClearAllCookies()
    End Sub

    Private Sub GetUserFromSession()

        Dim id As String = GetCookie(SessionCookie.Series)
        Dim token As String = GetCookie(SessionCookie.Token)

        If String.IsNullOrEmpty(id) OrElse String.IsNullOrEmpty(token) Then
            Exit Sub
        End If

        Dim userSession As New UserSession(id, token)
        Dim user As New GecoUser

        If GetSavedUserSession(userSession, user) Then
            SessionAdd(GecoSession.CurrentUser, user)
            CreateGecoUserCookie(user)
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
