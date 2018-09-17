Imports System.Data
Imports System.Data.SqlClient
Imports GECO.GecoModels

Partial Class UserHome
    Inherits Page

    Private Property currentUser As GecoUser

#Region " Page load "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        currentUser = GetCurrentUser()

        Title = "GECO - " & currentUser.FullName

        If Not IsPostBack Then
            LoadAccessTable()
            LoadProfile()
            LoadYearLabels()
        End If
    End Sub

    Private Sub LoadYearLabels()
        lblEIyear1.Text = Now.Year - 1 'This is the EI calendar year
        lblEIYear2.Text = Now.Year - 1 'This is the EI calendar year
        lblEIYear3.Text = Now.Year - 1 'This is the EI calendar year
        lblEIYear4.Text = Now.Year 'This is the EI due date
        lblEIYear5.Text = Now.Year - 1 'This is the EI calendar year
        lblEIYear6.Text = Now.Year - 1 'This is the EI calendar year

        lblESYear1.Text = Now.Year - 1 'This is the ES calendar year
        lblESYear2.Text = Now.Year - 1 'This is the ES calendar year
        lblESYear3.Text = Now.Year 'This is the ES due date

        lblFeeYear1.Text = Now.Year - 1 ' Fee Calendar year
        lblFeeYear2.Text = Now.Year - 1 ' Fee Calendar year
        lblFeeYear3.Text = Now.Year ' Fees dues year
        lblFeeYear4.Text = Now.Year ' Fees dues year

        If Now.Year Mod 3 = 2 Then
            lblTriennialEIText.Visible = True
            lblAnnualEIText.Visible = False
        End If
    End Sub

    Private Sub LoadAccessTable()
        Dim dtAccess As DataTable = currentUser.FacilityAccessTable

        If dtAccess Is Nothing OrElse dtAccess.Rows.Count = 0 Then
            'This user has NO Facility assigned
            lblNone.Visible = True
            lblAccess.Visible = False
            grdAccess.Visible = False
        Else
            lblNone.Visible = False
            lblAccess.Visible = True
            grdAccess.Visible = True
            grdAccess.DataSource = dtAccess
            grdAccess.DataBind()
        End If
    End Sub

    Protected Sub grdAccess_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdAccess.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As DataRowView = e.Row.DataItem
            Dim facilityName As String = row.Item("Facility").ToString()
            Dim airsNumber As ApbFacilityId = New ApbFacilityId(row.Item("AirsNumber").ToString)
            Dim url As String = String.Format("~/FacilityHome.aspx?airs={0}", airsNumber.ShortString())

            Dim hlFacility As HyperLink = e.Row.FindControl("hlFacility")
            If Not hlFacility Is Nothing Then
                hlFacility.Text = facilityName
                hlFacility.NavigateUrl = url
            End If

            Dim hlAirs As HyperLink = e.Row.FindControl("hlAirs")
            If Not hlAirs Is Nothing Then
                hlAirs.Text = airsNumber.FormattedString
                hlAirs.NavigateUrl = url
            End If
        End If
    End Sub

    Protected Sub LoadProfile()
        Dim mpUserLabel As Label = CType(Master.FindControl("lblUserName"), Label)
        mpUserLabel.Text = "Welcome, " & currentUser.FullName

        lblProfileMsg.Visible = False
        lblPwdMsg.Visible = False

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

#End Region

#Region " Data editing "

    Protected Sub btnUpdateProfile_Click(sender As Object, e As EventArgs) Handles btnUpdateProfile.Click
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
            lblProfileMsg.Text = "Information updated successfully."
        Else
            lblProfileMsg.Text = "There was an error updating your information."
        End If

        lblProfileMsg.Visible = True

        UpdateCurrentUser()
        DisableProfileEdit()
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
        If IsValid Then
            Dim token As String = Nothing
            Dim result As UpdateUserEmailResult = UpdateUserEmail(currentUser.Email, txtEmail.Text, token)

            Select Case result
                Case UpdateUserEmailResult.Success
                    SendConfirmEmailUpdateEmail(txtEmail.Text, token)
                    DisableEmailEdit()
                    lblPwdMsg.Text = "An email has been sent to the address you provided with an activation link " &
                        "to confirm your new address."
                    lblPwdMsg.Visible = True

                Case UpdateUserEmailResult.NewEmailExists
                    lblPwdMsg.Text = "An account already exists for that email address."
                    lblPwdMsg.Visible = True

                Case Else
                    lblPwdMsg.Text = "An error occurred. The email address has not been changed."
                    lblPwdMsg.Visible = True

            End Select
        End If
    End Sub

    Protected Sub btnPwdUpdate_Click(sender As Object, e As EventArgs) Handles btnPwdUpdate.Click
        If IsValid Then
            Dim result As UpdatePasswordResult = UpdatePassword(currentUser.Email, txtOldPassword.Text, txtNewPassword.Text)

            Select Case result
                Case UpdatePasswordResult.Success
                    DisablePasswordEdit()
                    lblPwdMsg.Text = "Password successfully updated."
                    lblPwdMsg.Visible = True
                    SendPasswordChangeNotification(currentUser.Email)

                Case UpdatePasswordResult.InvalidPassword
                    lblPwdMsg.Text = "The old password is incorrect. The password has not been changed."
                    lblPwdMsg.Visible = True

                Case Else
                    lblPwdMsg.Text = "An error occurred. The password has not been changed."
                    lblPwdMsg.Visible = True

            End Select
        End If
    End Sub

#End Region

#Region " Button Click Events "

    Protected Sub btnEditProfile_Click(sender As Object, e As EventArgs) Handles btnEditProfile.Click
        EnableProfileEdit()
    End Sub

    Protected Sub btnCancelProfile_Click(sender As Object, e As EventArgs) Handles btnCancelProfile.Click
        DisableProfileEdit()
    End Sub

    Protected Sub btnEditPwd_Click(sender As Object, e As EventArgs) Handles btnEditPwd.Click
        EnablePasswordEdit()
    End Sub

    Protected Sub btnPwdCancel_Click(sender As Object, e As EventArgs) Handles btnPwdCancel.Click
        DisablePasswordEdit()
    End Sub

    Protected Sub btnEditEmail_Click(sender As Object, e As EventArgs) Handles btnEditEmail.Click
        EnableEmailEdit()
    End Sub

    Protected Sub btnCancelEmail_Click(sender As Object, e As EventArgs) Handles btnCancelEmail.Click
        DisableEmailEdit()
    End Sub

#End Region

#Region " Disable/enable controls "
    'Individually disable-enable all controls

    Protected Sub EnableProfileEdit()
        DisablePasswordEdit()
        DisableEmailEdit()

        btnEditProfile.Enabled = False
        btnEditProfile.Visible = False
        btnCancelProfile.Enabled = True
        btnUpdateProfile.Enabled = True
        btnCancelProfile.Visible = True
        btnUpdateProfile.Visible = True
        txtSalutation.Enabled = True
        txtFName.Enabled = True
        txtLName.Enabled = True
        txtTitle.Enabled = True
        txtCoName.Enabled = True
        txtAddress.Enabled = True
        txtZip.Enabled = True
        txtCity.Enabled = True
        txtState.Enabled = True
        txtPhone.Enabled = True
        txtPhoneExt.Enabled = True
        txtFax.Enabled = True
        ddlUserType.Enabled = True
    End Sub

    Protected Sub DisableProfileEdit()
        btnEditProfile.Enabled = True
        btnEditProfile.Visible = True
        btnCancelProfile.Enabled = False
        btnUpdateProfile.Enabled = False
        btnCancelProfile.Visible = False
        btnUpdateProfile.Visible = False
        txtSalutation.Enabled = False
        txtFName.Enabled = False
        txtLName.Enabled = False
        txtTitle.Enabled = False
        txtCoName.Enabled = False
        txtAddress.Enabled = False
        txtZip.Enabled = False
        txtCity.Enabled = False
        txtState.Enabled = False
        txtPhone.Enabled = False
        txtPhoneExt.Enabled = False
        txtFax.Enabled = False
        ddlUserType.Enabled = False
    End Sub

    Protected Sub EnablePasswordEdit()
        DisableEmailEdit()
        DisableProfileEdit()

        btnEditEmail.Visible = False

        tblChangePwd.Visible = True

        txtOldPassword.Enabled = True
        txtNewPassword.Enabled = True
        txtPwdConfirm.Enabled = True
        btnPwdUpdate.Enabled = True
        btnPwdCancel.Enabled = True
        btnPwdUpdate.Visible = True
        btnPwdCancel.Visible = True
        btnEditPwd.Enabled = False
        btnEditPwd.Visible = False

        lblPwdMsg.Text = ""
        lblPwdMsg.Visible = False
    End Sub

    Protected Sub DisablePasswordEdit()
        btnEditEmail.Visible = True

        tblChangePwd.Visible = False

        txtOldPassword.Enabled = False
        txtNewPassword.Enabled = False
        txtPwdConfirm.Enabled = False
        btnPwdUpdate.Enabled = False
        btnPwdCancel.Enabled = False
        btnPwdUpdate.Visible = False
        btnPwdCancel.Visible = False
        btnEditPwd.Enabled = True
        btnEditPwd.Visible = True

        lblPwdMsg.Text = ""
        lblPwdMsg.Visible = False
    End Sub

    Protected Sub EnableEmailEdit()
        DisablePasswordEdit()
        DisableProfileEdit()

        btnEditPwd.Visible = False
        lblEmailWarning.Visible = True

        txtEmail.Enabled = True
        btnEditEmail.Enabled = False
        btnEditEmail.Visible = False
        btnSaveEmail.Enabled = True
        btnSaveEmail.Visible = True
        btnCancelEmail.Enabled = True
        btnCancelEmail.Visible = True

        lblPwdMsg.Text = ""
        lblPwdMsg.Visible = False
    End Sub

    Protected Sub DisableEmailEdit()
        btnEditPwd.Visible = True
        lblEmailWarning.Visible = False
        txtEmail.Enabled = False
        btnEditEmail.Enabled = True
        btnEditEmail.Visible = True
        btnSaveEmail.Enabled = False
        btnSaveEmail.Visible = False
        btnCancelEmail.Enabled = False
        btnCancelEmail.Visible = False

        lblPwdMsg.Text = ""
        lblPwdMsg.Visible = False
    End Sub

#End Region

End Class