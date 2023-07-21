Imports GECO.EmailTemplates

Partial Class Account
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim query = Request.QueryString
            If query Is Nothing OrElse query.Count = 0 Then
                HttpContext.Current.Response.Redirect("~/", False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
                Return
            End If

            Dim registerResult As String = Request.QueryString("result")

            If Not String.IsNullOrEmpty(registerResult) Then
                Select Case registerResult
                    Case "Exists"
                        MultiView1.SetActiveView(RegisterExists)
                    Case "Success"
                        MultiView1.SetActiveView(RegisterSuccess)
                    Case "Sent"
                        MultiView1.SetActiveView(NewEmailSent)
                    Case "Error"
                        MultiView1.SetActiveView(ErrorResult)
                    Case Else
                        Throw New HttpException(404, "Not found")
                End Select

                Return
            End If

            Dim action As String = Request.QueryString("action")
            Dim email As String = Request.QueryString("acct")
            Dim token As String = Request.QueryString("token")

            If String.IsNullOrEmpty(action) Then
                Throw New HttpException(404, "Not found")
            End If

            Select Case action
                Case "change-email"
                    ' Used by IAIP for sending new email confirmation email
                    ' (IAIP can't directly send email)
                    If String.IsNullOrEmpty(token) OrElse String.IsNullOrEmpty(email) Then
                        Throw New HttpException(404, "Not found")
                    End If

                    SendConfirmEmailUpdateEmail(email, token)
                    HttpContext.Current.Response.Redirect("~/Account.aspx?result=Sent", False)
                    HttpContext.Current.ApplicationInstance.CompleteRequest()
                    Return

                Case "resend"
                    MultiView1.SetActiveView(ResendConfirmation)

                Case "confirm"
                    ' Used for confirming a new GECO account
                    If String.IsNullOrEmpty(token) OrElse String.IsNullOrEmpty(email) Then
                        Throw New HttpException(404, "Not found")
                    End If

                    Session.Clear()
                    ClearAllCookies()

                    Select Case ConfirmAccount(email, token)
                        Case DbResult.Success
                            MultiView1.SetActiveView(ConfirmSuccess)
                        Case DbResult.Failure
                            MultiView1.SetActiveView(ConfirmFailed)
                        Case Else
                            MultiView1.SetActiveView(ErrorResult)
                    End Select

                Case "reset"
                    ' Used for resetting a forgotten password
                    ' (Password reset confirmation and new account
                    ' confirmation are identical on the database)
                    If String.IsNullOrEmpty(token) OrElse String.IsNullOrEmpty(email) Then
                        Throw New HttpException(404, "Not found")
                    End If

                    Session.Clear()
                    ClearAllCookies()

                    Select Case ConfirmAccount(email, token)
                        Case DbResult.Success
                            hidEmail.Value = email
                            MultiView1.SetActiveView(ResetAllowed)
                        Case DbResult.Failure
                            MultiView1.SetActiveView(ResetFailed)
                        Case Else
                            MultiView1.SetActiveView(ErrorResult)
                    End Select

                Case "update"
                    ' Used for changing the email address on the account
                    If String.IsNullOrEmpty(token) OrElse String.IsNullOrEmpty(email) Then
                        Throw New HttpException(404, "Not found")
                    End If

                    Session.Clear()
                    ClearAllCookies()

                    Dim oldEmail As String = Nothing

                    Select Case ConfirmEmailChange(email, token, oldEmail)
                        Case DbResult.Success
                            MultiView1.SetActiveView(ConfirmEmailSuccess)
                            SendEmailChangeNotification(oldEmail, email)
                        Case DbResult.Failure
                            MultiView1.SetActiveView(ConfirmEmailFailed)
                        Case Else
                            MultiView1.SetActiveView(ErrorResult)
                    End Select

                Case Else
                    Throw New HttpException(404, "Not found")

            End Select

        End If
    End Sub

    Private Sub lbtResend_Click(sender As Object, e As EventArgs) Handles lbtResend.Click
        If MultiView1.GetActiveView IsNot ResendConfirmation Then
            MultiView1.SetActiveView(ResendConfirmation)
        End If
    End Sub

    Private Sub btnResend_Click(sender As Object, e As EventArgs) Handles btnResend.Click
        If IsValid Then
            SendConfirmationEmail(txtEmailAddress.Text)
        End If
    End Sub

    Private Sub SendConfirmationEmail(email As String)
        Dim token As String = Nothing
        Dim dbResult As DbResult = RenewAccountToken(email, token)

        Select Case dbResult
            Case DbResult.Success
                Dim emailResult = SendConfirmAccountEmail(email, token)
                If emailResult Then
                    MultiView1.SetActiveView(RegisterSuccess)
                    Return
                End If
        End Select

        MultiView1.SetActiveView(ErrorResult)
    End Sub

    Protected Sub lbtnRefreshCaptcha_Click(sender As Object, e As EventArgs) Handles lbtnRefreshCaptcha.Click
        captchaControl.ValidateCaptcha(String.Empty)
        txtCaptcha.Text = ""
    End Sub

    Private Sub cvEmailExists_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvEmailExists.ServerValidate
        args.IsValid = GecoUserExists(args.Value)
    End Sub

    Private Sub cvCaptcha_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvCaptcha.ServerValidate
        captchaControl.ValidateCaptcha(txtCaptcha.Text)
        txtCaptcha.Text = ""
        args.IsValid = captchaControl.UserValidated
    End Sub

    Private Sub btnSetPassword_Click(sender As Object, e As EventArgs) Handles btnSetPassword.Click
        If IsValid Then
            Dim email As String = hidEmail.Value

            If email Is Nothing Then
                MultiView1.SetActiveView(ErrorResult)
            Else
                Dim result As DbResult = SetAccountPassword(email, txtPwd.Text)

                Select Case result
                    Case DbResult.Success
                        MultiView1.SetActiveView(ResetSuccess)
                        SendPasswordChangeNotification(email)
                    Case Else
                        MultiView1.SetActiveView(ErrorResult)
                End Select
            End If
        End If
    End Sub

End Class
