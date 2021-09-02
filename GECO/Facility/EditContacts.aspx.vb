Imports GECO.DAL.Facility
Imports GECO.EmailTemplates
Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Public Class EditContacts
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property currentAirs As ApbFacilityId

    Public Property FacilityAccess As FacilityAccess
    Public Property CurrentCategory As CommunicationCategory
    Public Property CurrentCommunicationInfo As FacilityCommunicationInfo
    Public Property ResentVerificationEmail As String
    Public Property AddedEmail As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            If Not ApbFacilityId.TryParse(GetCookie(Cookie.AirsNumber), currentAirs) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If
        Else
            ' AIRS number
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.TryParse(airsString, currentAirs) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        Master.CurrentAirs = currentAirs
        Master.IsFacilitySet = True

        MainLoginCheck(Page.ResolveUrl("~/Facility/Contacts.aspx?airs=" & currentAirs.ShortString))

        ' Current user and facility access
        currentUser = GetCurrentUser()

        FacilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If FacilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Facility/")
        End If

        If Not IsPostBack Then
            Title = "GECO Edit Facility Contacts - " & GetFacilityNameAndCity(currentAirs)
        End If

        Dim category As String = Nothing

        If IsPostBack Then
            category = GetCookie(Cookie.CommunicationCategory)
        ElseIf Request.QueryString("category") IsNot Nothing Then
            category = Request.QueryString("category")
        End If

        If Not CommunicationCategory.IsValidCategory(category) Then
            HttpContext.Current.Response.Redirect("~/Facility/Contacts.aspx")
        End If

        CurrentCategory = CommunicationCategory.FromName(category)

        If Not FacilityAccess.HasCommunicationPermission(CurrentCategory) Then
            HttpContext.Current.Response.Redirect("~/Facility/Contacts.aspx")
        End If

        SetCookie(Cookie.CommunicationCategory, category)

        If Not IsPostBack Then
            LoadCurrentData()
        End If
    End Sub

    Private Sub LoadCurrentData()
        CurrentCommunicationInfo = GetFacilityCommunicationInfo(currentAirs, CurrentCategory)

        rbCommPref.SelectedValue = CurrentCommunicationInfo.Preference.CommunicationPreference.Name
        pnlElectronicCommunication.Visible = CurrentCategory.ElectronicCommunicationAllowed AndAlso
                CurrentCommunicationInfo.Preference.CommunicationPreference.IncludesElectronic

        DisplayEmailLists()
        DisplayMailContact()
    End Sub

    Private Sub DisplayMailContact()
        If CurrentCommunicationInfo.Mail IsNot Nothing Then
            With CurrentCommunicationInfo.Mail
                txtAddress.Text = .Address1
                txtAddress2.Text = .Address2
                txtCity.Text = .City
                txtFirstName.Text = .FirstName
                txtLastName.Text = .LastName
                txtPrefix.Text = .Prefix
                txtOrganization.Text = .Organization
                txtPostalCode.Text = .PostalCode
                txtState.Text = .State
                txtTelephone.Text = .Telephone
                txtTitle.Text = .Title
            End With
        End If
    End Sub

    Private Sub DisplayEmailLists()
        If CurrentCategory.ElectronicCommunicationAllowed Then
            rptVerifiedEmails.DataSource = CurrentCommunicationInfo.VerifiedEmails
            rptVerifiedEmails.DataBind()

            rptUnverifiedEmails.DataSource = CurrentCommunicationInfo.UnverifiedEmails
            rptUnverifiedEmails.DataBind()
        End If
    End Sub

    Protected Sub SavePreference(sender As Object, e As EventArgs) Handles btnSavePref.Click
        ClearWarnings()

        If Not CommunicationPreference.IsValidPreference(rbCommPref.SelectedValue) Then
            Throw New ArgumentException($"Invalid preference selected: {rbCommPref.SelectedValue}")
        End If

        Dim preference As CommunicationPreference = CommunicationPreference.FromName(rbCommPref.SelectedValue)

        If Not SaveCommunicationPreference(currentAirs, CurrentCategory, preference, currentUser.UserId) Then
            pPrefSaveError.Visible = True
        Else
            pPrefSaveSuccess.Visible = True
        End If

        LoadCurrentData()
    End Sub

    Protected Sub SaveContact(sender As Object, e As EventArgs) Handles btnSaveContact.Click
        ClearWarnings()

        Dim contact As New MailContact With {
            .Address1 = txtAddress.Text,
            .Address2 = txtAddress2.Text,
            .City = txtCity.Text,
            .FirstName = txtFirstName.Text,
            .LastName = txtLastName.Text,
            .Prefix = txtPrefix.Text,
            .Organization = txtOrganization.Text,
            .PostalCode = txtPostalCode.Text,
            .State = txtState.Text,
            .Telephone = txtTelephone.Text,
            .Email = txtEmail.Text,
            .Title = txtTitle.Text
        }

        If SaveMailContact(currentAirs, CurrentCategory, contact, currentUser.UserId) Then
            pMailSaveSuccess.Visible = True
        Else
            pMailSaveError.Visible = True
        End If

        LoadCurrentData()
    End Sub

    Protected Sub RemoveEmail(sender As Object, e As EventArgs)
        ClearWarnings()

        Dim button As Button = CType(sender, Button)
        Dim email As String = button.CommandArgument

        If RemoveEmailContact(currentAirs, CurrentCategory, email) Then
            If button.ID = "btnRemoveUnverifiedEmail" Then
                pUnverifiedEmailRemovedSuccess.Visible = True
            Else
                pVerifiedEmailRemovedSuccess.Visible = True
            End If
        Else
            If button.ID = "btnRemoveUnverifiedEmail" Then
                pUnverifiedEmailListError.Visible = True
            Else
                pVerifiedEmailListError.Visible = True
            End If
        End If

        LoadCurrentData()
    End Sub

    Protected Sub ResendVerificationEmail(sender As Object, e As EventArgs)
        ClearWarnings()

        Dim button As Button = CType(sender, Button)
        Dim email As String = button.CommandArgument

        If Not IsValidEmailAddress(email) Then
            Throw New HttpException(400, "Bad request")
        End If

        Dim result As ResendEmailVerificationResult = RefreshEmailContactToken(currentAirs, CurrentCategory, email, currentUser.UserId)

        Select Case result.Status
            Case ResendEmailVerificationResultStatus.DbError
                If button.ID = "btnRemoveUnverifiedEmail" Then
                    pUnverifiedEmailListError.Visible = True
                Else
                    pVerifiedEmailListError.Visible = True
                End If

            Case ResendEmailVerificationResultStatus.EmailDoesNotExist
                pEmailAlreadyRemoved.Visible = True

            Case ResendEmailVerificationResultStatus.Success
                SendEmailContactConfirmationEmail(currentAirs, email, CurrentCategory, result.Seq, result.Token)
                ResentVerificationEmail = email
                pEmailVerificationSuccess.Visible = True
                pEmailVerificationSuccess.InnerText = $"A confirmation email has been sent to {email}."

        End Select

        LoadCurrentData()
    End Sub

    Protected Sub AddNewEmail(sender As Object, e As EventArgs) Handles btnAddNewEmail.Click
        ClearWarnings()

        Dim email As String = Trim(txtNewEmail.Text)

        If Not IsValidEmailAddress(email) Then
            pAddEmailInvalid.Visible = True
        Else
            Dim result As AddEmailContactResult = AddEmailContact(currentAirs, CurrentCategory, email, currentUser.UserId)

            Select Case result.Status
                Case AddEmailContactResultStatus.DbError
                    pAddEmailError.Visible = True

                Case AddEmailContactResultStatus.EmailExists
                    pAddEmailExists.Visible = True

                Case AddEmailContactResultStatus.Success
                    SendEmailContactConfirmationEmail(currentAirs, email, CurrentCategory, result.Seq, result.Token)
                    AddedEmail = email
                    pAddEmailSuccess.Visible = True
                    txtNewEmail.Text = ""

            End Select
        End If

        LoadCurrentData()
    End Sub

    Private Sub ClearWarnings()
        pAddEmailInvalid.Visible = False
        pAddEmailError.Visible = False
        pAddEmailExists.Visible = False
        pAddEmailSuccess.Visible = False

        pPrefSaveError.Visible = False
        pPrefSaveSuccess.Visible = False

        pMailSaveError.Visible = False
        pMailSaveSuccess.Visible = False

        pVerifiedEmailRemovedSuccess.Visible = False
        pUnverifiedEmailRemovedSuccess.Visible = False
        pVerifiedEmailListError.Visible = False
        pUnverifiedEmailListError.Visible = False
        pEmailVerificationSuccess.Visible = False
        pEmailAlreadyRemoved.Visible = False
    End Sub

End Class
