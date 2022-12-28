Imports GECO.EmailTemplates
Imports GECO.GecoModels
Imports System.Net

Partial Class Login
    Inherits Page

#Region " Page load "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If UserIsLoggedIn() Then
                Response.Redirect("~/Account/")
            Else
                GetUserFromSession()
            End If

            ClearCurrentLogin()

            Title = "GECO - Login"
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
        SignIn()
    End Sub

    Protected Sub SignIn()
        lblMessage.Visible = False
        lblUnconfirmed.Visible = False

        Dim gecoUser As New GecoUser
        Dim userSession As New UserSession

        Dim ipAddress As String = GetIPv4Address()

        Dim loginResult As LoginResult = LogInUser(txtUserId.Text, txtPassword.Text, chkRememberMe.Checked, ipAddress, gecoUser, userSession)

        Select Case loginResult
            Case LoginResult.Invalid
                lblMessage.Text = "Either the email is not registered or the password is incorrect. Please try again."
                lblMessage.Visible = True

            Case LoginResult.AccountUnconfirmed
                lblUnconfirmed.Visible = True

            Case LoginResult.LoginThrottled
                lblMessage.Text = "Too many login attempts made. Please wait a few seconds and try again."
                lblMessage.Visible = True

            Case LoginResult.Success
                If gecoUser.UserId = 0 Then
                    Response.Redirect("~/ErrorPage.aspx", False)
                End If

                SessionAdd(GecoSession.CurrentUser, gecoUser)

                If chkRememberMe.Checked Then
                    CreateSessionCookie(userSession)
                Else
                    ClearCookie(SessionCookie)
                End If

                If gecoUser.ProfileUpdateRequired Then
                    Response.Redirect("~/Account/?action=updateprofile")
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
    
    Private Shared Function GetIPv4Address() As String
        ' Retrieving IP Address from the computer does not work all the time
        ' Therefore, we are calling external websites for more accurate information
        Dim ipv4Address As String
        
        Dim client As New WebClient
        ' Add a user agent header in case the requested URI contains a query.
        client.Headers.Add("user-agent",
                           "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR1.0.3705;)")
        ' with proxy server only:
        Dim proxy As IWebProxy = WebRequest.GetSystemWebProxy()
        proxy.Credentials = CredentialCache.DefaultNetworkCredentials
        client.Proxy = proxy
        
        ' 2 URLs in case one of them break
        Dim firstUrl As String = "http://checkip.dyndns.org/"
        Dim secondUrl As String = "http://icanhazip.com/"
        
        ' have try catch statement in case first link die
        ' (i.e http://automation.whatismyip.com/n09230945.asp)
        Try
            Dim ip as String = client.DownloadString(firstUrl).
                Replace("<html><head><title>Current IP Check</title></head><body>Current IP Address: ", "").
                Replace("</body></html>", "").
                Replace("\r\n", "").
                Replace("\n", "").
                Trim()
            ' map to IPv4 in case the returned address is in IPv6
            ipv4Address = IPAddress.Parse(ip).MapToIPv4().ToString()
        Catch ex As Exception
            Try
                Console.WriteLine(ex.ToString())
                Dim ip as String = client.DownloadString(secondUrl).
                    Replace("\r\n", "").
                    Replace("\n", "").
                    Trim()
                ' map to IPv4 in case the returned address is in IPv6
                ipv4Address = IPAddress.Parse(ip).MapToIPv4().ToString()
            Catch
                ' use the IP Address from the computer if both of the links failed
                ipv4Address = Dns.GetHostEntry(Dns.GetHostName()).AddressList.GetValue(1).ToString()
            End Try
        End Try
        Return ipv4Address
    End Function

    Private Sub GetUserFromSession()

        Dim series As String = GetCookie(UserSessionCookie.Series)
        Dim token As String = GetCookie(UserSessionCookie.Token)

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
