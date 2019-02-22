Imports System.Reflection
Imports GECO.GecoModels

Partial Class UserRegistration
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If UserIsLoggedIn() Then
                Response.Redirect("~/Home/")
            End If

            Session.Clear()
            ClearAllCookies()
        End If
    End Sub

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        If IsValid Then
            Dim newUser As New GecoUser With {
                .Email = Trim(txtEmail.Text),
                .Salutation = txtSalutation.Text,
                .FirstName = txtFName.Text,
                .LastName = txtLName.Text,
                .Title = txtTitle.Text,
                .Company = txtCoName.Text,
                .Address = New Address() With {
                    .Street = txtAddress.Text,
                    .City = txtCity.Text,
                    .State = txtState.Text.ToUpper,
                    .PostalCode = txtZip.Text
                },
                .PhoneNumber = txtPhone.Text & txtPhoneExt.Text,
                .FaxNumber = txtFax.Text
            }

            Dim token As String = Nothing
            Dim returnvalue As DbResult = CreateGecoAccount(newUser, txtPwd.Text, token)

            Select Case returnvalue
                Case DbResult.Success
                    SendConfirmAccountEmail(newUser.Email, token)
                    Response.Redirect("~/Account.aspx?result=Success", False)

                Case DbResult.Failure
                    '  User already exists
                    Response.Redirect("~/Account.aspx?result=Exists", False)

                Case Else
                    Dim ex As New Exception("GECO Registration Error")
                    ex.Data.Add("Email", txtEmail.Text)
                    ex.Data.Add("Method", MethodBase.GetCurrentMethod.Name)
                    ErrorReport(ex, False)
                    Response.Redirect("~/Account.aspx?result=Error", False)
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