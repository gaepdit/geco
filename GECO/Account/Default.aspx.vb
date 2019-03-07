Imports GECO.GecoModels

Partial Class Account_Default
    Inherits Page

    Private Property currentUser As GecoUser

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        currentUser = GetCurrentUser()

        Title = "GECO Account - " & currentUser.FullName

        If Request.QueryString("action") = "updateprofile" Then
            pUpdateRequired.Visible = True
        End If

        If Not IsPostBack Then
            LoadProfile()
        End If
    End Sub

    Private Sub LoadProfile()
        lblDisplayName.Text = ConcatNonEmptyStrings(", ", {currentUser.FullName, currentUser.Email})

        txtFName.Text = currentUser.FirstName
        txtLName.Text = currentUser.LastName
        txtTitle.Text = currentUser.Title
        txtCoName.Text = currentUser.Company
        txtAddress.Text = currentUser.Address.Street
        txtCity.Text = currentUser.Address.City
        txtState.Text = currentUser.Address.State
        txtZip.Text = currentUser.Address.PostalCode
        txtPhone.Text = currentUser.PhoneNumber

        If Not String.IsNullOrEmpty(currentUser.GecoUserType) Then
            ddlUserType.Text = currentUser.GecoUserType
        End If
    End Sub

    Private Sub HideMessages()
        lblProfileMessage.Visible = False
    End Sub

    Protected Sub btnUpdateProfile_Click(sender As Object, e As EventArgs) Handles btnUpdateProfile.Click
        HideMessages()

        Dim gecoUserType As String = If(ddlUserType.Text = "-- Select --", Nothing, ddlUserType.Text)

        If IsValid Then
            Dim newUser As New GecoUser(currentUser.UserId) With {
                .FirstName = txtFName.Text,
                .LastName = txtLName.Text,
                .Title = txtTitle.Text,
                .Company = txtCoName.Text,
                .PhoneNumber = txtPhone.Text,
                .GecoUserType = gecoUserType,
                .Address = New Address() With {
                    .Street = txtAddress.Text,
                    .City = txtCity.Text,
                    .State = txtState.Text,
                    .PostalCode = txtZip.Text
                }
            }

            If UpdateUserProfile(newUser) = DbResult.Success Then
                lblProfileMessage.Text = "Profile was updated successfully."
                UpdateCurrentUser(newUser)
            Else
                lblProfileMessage.Text = "There was an error updating your profile."
            End If

            lblProfileMessage.Visible = True
        End If
    End Sub

    Private Sub UpdateCurrentUser(newUser As GecoUser)
        Dim currentUser As GecoUser = GetCurrentUser()

        With currentUser
            .FirstName = newUser.FirstName
            .LastName = newUser.LastName
            .Company = newUser.Company
            .Address.Street = newUser.Address.Street
            .Address.PostalCode = newUser.Address.PostalCode
            .Address.City = newUser.Address.City
            .Address.State = newUser.Address.State
            .PhoneNumber = newUser.PhoneNumber
            .GecoUserType = newUser.GecoUserType
            .ProfileUpdateRequired = False
        End With

        SessionAdd(GecoSession.CurrentUser, currentUser)
    End Sub

End Class