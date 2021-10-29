Imports GECO.EmailTemplates
Imports GECO.GecoModels

Partial Class Account_Password
    Inherits Page

    Private Property currentUser As GecoUser

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck("Account/Password.aspx")

        currentUser = GetCurrentUser()

        Title = "GECO Account - " & currentUser.FullName

        If Not IsPostBack Then
            LoadProfile()
        End If
    End Sub

    Protected Sub LoadProfile()
        lblDisplayName.Text = currentUser.FullName & ", " & currentUser.Email
    End Sub

    Private Sub HideMessages()
        lblPasswordMessage.Visible = False
    End Sub

    Protected Sub btnPwdUpdate_Click(sender As Object, e As EventArgs) Handles btnPwdUpdate.Click
        HideMessages()

        If IsValid Then
            Dim result As UpdatePasswordResult = UpdatePassword(currentUser.Email, txtOldPassword.Text, txtNewPassword.Text)

            Select Case result
                Case UpdatePasswordResult.Success
                    SendPasswordChangeNotification(currentUser.Email)
                    lblPasswordMessage.Text = "Password successfully updated."

                Case UpdatePasswordResult.InvalidPassword
                    lblPasswordMessage.Text = "The old password is incorrect. The password has not been changed."

                Case Else
                    lblPasswordMessage.Text = "An error occurred. The password has not been changed."

            End Select

            lblPasswordMessage.Visible = True
        End If
    End Sub

End Class
