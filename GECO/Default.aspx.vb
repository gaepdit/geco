Imports GECO.GecoModels

Partial Class _Default
    Inherits Page

#Region " Page load "

    Dim isDefaultPage As Boolean = True

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("do") = "SignOut" Then
                ClearCurrentLogin()
            ElseIf UserIsLoggedIn() Then
                Response.Redirect("~/Home/")
            Else
                GetUserFromSession()
            End If

            Dim m As MainMaster = Master
            m.IncludeSignInLink = False

            ClearCurrentLogin()
        End If
    End Sub

    Private Sub ClearCurrentLogin()
        lblMessage.Visible = False
        Session.Clear()
        ClearAllCookies()
    End Sub

#End Region

#Region " Login "

    Protected Sub btnSignIn_Click(sender As Object, e As EventArgs) Handles btnSignIn.Click
        lblMessage.Visible = False
        lblUnconfirmed.Visible = False
        SignIn()
    End Sub

    Protected Sub SignIn()
        Dim user As New GecoUser
        Dim userSession As New UserSession

        Dim loginResult As LoginResult = LogInUser(txtUserId.Text, txtPassword.Text, chkRememberMe.Checked, user, userSession)

        Select Case loginResult
            Case LoginResult.Invalid
                lblMessage.Visible = True

            Case LoginResult.AccountUnconfirmed
                lblUnconfirmed.Visible = True

            Case LoginResult.Success
                If user.UserId = 0 Then
                    Response.Redirect("~/ErrorPage.aspx", False)
                End If

                SessionAdd(GecoSession.CurrentUser, user)
                CreateGecoUserCookie(user)

                If chkRememberMe.Checked Then
                    CreateSessionCookie(userSession)
                Else
                    ClearCookie(CookieCollection.SessionCookie)
                End If

                Dim strRedirect As String = Request.QueryString("ReturnUrl")

                If String.IsNullOrEmpty(strRedirect) Then
                    Response.Redirect("~/Home/")
                Else
                    Response.Redirect(strRedirect)
                End If

            Case Else 'Some Error
                Response.Redirect("~/ErrorPage.aspx", False)
        End Select

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

#End Region

#Region " Password reset "

    Protected Sub lbtForgotPwd_Click(sender As Object, e As EventArgs) Handles lbtForgotPwd.Click
        If mvLogin.ActiveViewIndex <> 1 Then
            mvLogin.ActiveViewIndex = 1
            mvResetPassword.SetActiveView(vResetPassword)
            lblMessage.Visible = False
            lbtForgotPwd.Visible = False
        End If
    End Sub

    Protected Sub lbCancel_Click(sender As Object, e As EventArgs) Handles lbCancel.Click, lbReturn.Click
        CancelClick()
    End Sub

    Protected Sub CancelClick()
        mvLogin.ActiveViewIndex = 0
        txtEmailAddress.Text = ""
        lblMessage.Visible = False
        lbtForgotPwd.Visible = True
    End Sub

    Protected Sub btnResetPassword_Click(sender As Object, e As EventArgs) Handles btnResetPassword.Click
        If IsValid Then
            Dim token As String = Nothing
            Dim result As DbResult = RenewAccountToken(txtEmailAddress.Text, token)

            Select Case result
                Case DbResult.Success
                    SendPasswordResetEmail(txtEmailAddress.Text, token)
                    mvResetPassword.SetActiveView(vResetResult)
                Case Else
                    mvResetPassword.SetActiveView(vResetError)
            End Select
        End If
    End Sub

    Private Sub cvEmailExists_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvEmailExists.ServerValidate
        args.IsValid = GecoUserExists(args.Value)
    End Sub

#End Region

End Class
