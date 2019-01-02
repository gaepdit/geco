Imports System.Data.SqlClient
Imports GECO.GecoModels

Partial Class Account_Default
    Inherits Page

    Private Property currentUser As GecoUser

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        currentUser = GetCurrentUser()

        Title = "GECO"

        If Not IsPostBack Then
            LoadProfile()
        End If
    End Sub

    Protected Sub LoadProfile()
        Dim mpUserLabel As Label = CType(Master.FindControl("lblUserName"), Label)
        mpUserLabel.Text = "Welcome, " & currentUser.FullName
        lblDisplayName.Text = currentUser.FullName & ", " & currentUser.Email

        txtSalutation.Text = currentUser.Salutation
        txtFName.Text = currentUser.FirstName
        txtLName.Text = currentUser.LastName
        txtTitle.Text = currentUser.Title
        txtCoName.Text = currentUser.Company
        txtEmail.Text = currentUser.Email
        txtFax.Text = currentUser.FaxNumber
        txtAddress.Text = currentUser.Address.Street
        txtCity.Text = currentUser.Address.City
        txtState.Text = currentUser.Address.State
        txtZip.Text = currentUser.Address.PostalCode
        txtPhone.Text = currentUser.PhoneMain
        txtPhoneExt.Text = currentUser.PhoneExt
        ddlUserType.Text = currentUser.GecoUserTypeString
    End Sub

    Private Sub HideMessages()
        lblProfileMessage.Visible = False
        lblEmailMessage.Visible = False
        lblPasswordMessage.Visible = False
    End Sub

    Protected Sub btnUpdateProfile_Click(sender As Object, e As EventArgs) Handles btnUpdateProfile.Click
        HideMessages()

        If IsValid Then
            Dim phone As String = txtPhone.Text & txtPhoneExt.Text

            Dim query = "Update OlapUserProfile set " &
            " strsalutation = @txtSalutation, " &
            " strfirstname = @txtFName, " &
            " strlastname = @txtLName, " &
            " strtitle = @txtTitle, " &
            " strcompanyname = @txtCoName, " &
            " straddress = @txtAddress, " &
            " strzip = @txtZip, " &
            " strcity = @txtCity, " &
            " strstate = @txtState, " &
            " strphonenumber = @phone, " &
            " strfaxnumber = @txtFax, " &
            " strusertype = @ddlUserType " &
            " where numuserid = @UserID "

            Dim params As SqlParameter() = {
            New SqlParameter("@txtSalutation", txtSalutation.Text),
            New SqlParameter("@txtFName", txtFName.Text),
            New SqlParameter("@txtLName", txtLName.Text),
            New SqlParameter("@txtTitle", txtTitle.Text),
            New SqlParameter("@txtCoName", txtCoName.Text),
            New SqlParameter("@txtAddress", txtAddress.Text),
            New SqlParameter("@txtZip", txtZip.Text),
            New SqlParameter("@txtCity", txtCity.Text),
            New SqlParameter("@txtState", txtState.Text.ToUpper),
            New SqlParameter("@phone", phone),
            New SqlParameter("@txtFax", txtFax.Text),
            New SqlParameter("@ddlUserType", ddlUserType.Text),
            New SqlParameter("@UserID", currentUser.UserId)
        }

            If DB.RunCommand(query, params) Then
                lblProfileMessage.Text = "Profile was updated successfully."
            Else
                lblProfileMessage.Text = "There was an error updating your profile."
            End If

            lblProfileMessage.Visible = True

            UpdateCurrentUser()
        End If
    End Sub

    Private Sub UpdateCurrentUser()
        Dim user As GecoUser = GetCurrentUser()

        With user
            .Salutation = txtSalutation.Text
            .FirstName = txtFName.Text
            .LastName = txtLName.Text
            .Company = txtCoName.Text
            .Address.Street = txtAddress.Text
            .Address.PostalCode = txtZip.Text
            .Address.City = txtCity.Text
            .Address.State = txtState.Text
            .PhoneNumber = txtPhone.Text & txtPhoneExt.Text
            .FaxNumber = txtFax.Text
            .GecoUserTypeString = ddlUserType.Text
        End With

        SessionAdd(GecoSession.CurrentUser, user)
    End Sub

    Private Sub btnSaveEmail_Click(sender As Object, e As EventArgs) Handles btnSaveEmail.Click
        HideMessages()

        If IsValid Then
            Dim token As String = Nothing
            Dim result As UpdateUserEmailResult = UpdateUserEmail(currentUser.Email, txtEmail.Text, token)

            Select Case result
                Case UpdateUserEmailResult.Success
                    SendConfirmEmailUpdateEmail(txtEmail.Text, token)
                    lblEmailMessage.Text = "An email has been sent to the address you provided with an activation link to confirm your new address."

                Case UpdateUserEmailResult.NewEmailExists
                    lblEmailMessage.Text = "An account already exists for that email address."

                Case Else
                    lblEmailMessage.Text = "An error occurred. The email address has not been changed."

            End Select

            lblEmailMessage.Visible = True
        End If
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