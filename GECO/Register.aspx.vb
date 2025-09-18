Imports System.Reflection
Imports GECO.EmailTemplates

Partial Class Register
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If UserIsLoggedIn() Then
                CompleteRedirect("~/Home/")
                Return
            End If

            Session.Clear()
            ClearAllCookies()
        End If
    End Sub

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        If IsValid Then
            Dim email As String = txtEmail.Text.Trim()

            Dim token As String = Nothing
            Dim returnvalue As DbResult = CreateGecoAccount(email, txtPwd.Text, token)

            Select Case returnvalue
                Case DbResult.Success
                    SendConfirmAccountEmail(email, token)
                    CompleteRedirect("~/Account.aspx?result=Success")
                    Return

                Case DbResult.Failure
                    '  User already exists
                    CompleteRedirect("~/Account.aspx?result=Exists")
                    Return

                Case Else
                    Dim ex As New Exception("GECO Registration Error")
                    ex.Data.Add("Email", email)
                    ex.Data.Add("Method", MethodBase.GetCurrentMethod.Name)
                    ErrorReport(ex, False)
                    CompleteRedirect("~/Account.aspx?result=Error")
                    Return

            End Select
        End If
    End Sub

    Protected Sub lbtnRefreshCaptcha_Click(sender As Object, e As EventArgs) Handles lbtnRefreshCaptcha.Click
        captchaControl.ValidateCaptcha(String.Empty)
        txtCaptcha.Text = ""
    End Sub

    Private Sub cvEmailExists_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvEmailExists.ServerValidate
        args.IsValid = Not GecoUserExists(args.Value)
    End Sub

    Private Sub cvCaptcha_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvCaptcha.ServerValidate
        captchaControl.ValidateCaptcha(txtCaptcha.Text)
        txtCaptcha.Text = ""
        args.IsValid = captchaControl.UserValidated
    End Sub

End Class
