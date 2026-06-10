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
    Public Property AddedEmail As String

    Private IsTerminating As Boolean = False
    Protected Overrides Sub OnLoad(e As EventArgs)
        IsTerminating = MainLoginCheck()
        If IsTerminating Then Return
        MyBase.OnLoad(e)
    End Sub
    Protected Overrides Sub Render(writer As HtmlTextWriter)
        If IsTerminating Then Return
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            If Not ApbFacilityId.TryParse(GetCookie(Cookie.AirsNumber), currentAirs) Then
                CompleteRedirect("~/Home/", IsTerminating)
                Return
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
                CompleteRedirect("~/Home/", IsTerminating)
                Return
            End If

            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        Master.CurrentAirs = currentAirs
        Master.IsFacilitySet = True

        ' Current user and facility access
        currentUser = GetCurrentUser()

        FacilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If FacilityAccess Is Nothing Then
            CompleteRedirect("~/Facility/", IsTerminating)
            Return
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
            CompleteRedirect("~/Facility/Contacts.aspx", IsTerminating)
            Return
        End If

        CurrentCategory = CommunicationCategory.FromName(category)

        If Not FacilityAccess.HasCommunicationPermission(CurrentCategory) Then
            CompleteRedirect("~/Facility/Contacts.aspx", IsTerminating)
            Return
        End If

        SetCookie(Cookie.CommunicationCategory, category)

        If Not IsPostBack Then
            LoadCurrentData()
        End If
    End Sub

    Private Sub LoadCurrentData()
        CurrentCommunicationInfo = GetFacilityCommunicationInfo(currentAirs, CurrentCategory)

        rbCommPref.SelectedValue = CurrentCommunicationInfo.Preference.CommunicationPreference.Name

        If CurrentCategory.CommunicationOption = CommunicationOptionType.Electronic OrElse
            (CurrentCategory.CommunicationOption = CommunicationOptionType.FacilityChoice AndAlso
            CurrentCommunicationInfo.Preference.CommunicationPreference.IncludesElectronic) Then
            pnlElectronicCommunication.Visible = True
            DisplayEmailLists()
        Else
            pnlElectronicCommunication.Visible = False
        End If

        pnlFacilityCommunicationChoice.Visible = CurrentCategory.CommunicationOption = CommunicationOptionType.FacilityChoice

        DisplayMailContact()
    End Sub

    Private Sub DisplayMailContact()
        If CurrentCommunicationInfo.Mail IsNot Nothing Then
            With CurrentCommunicationInfo.Mail
                txtAddress.Text = .Address1
                txtAddress2.Text = .Address2
                txtCity.Text = .City
                txtEmail.Text = .Email
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
        rptEmails.DataSource = CurrentCommunicationInfo.EmailList
        rptEmails.DataBind()
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
            .Email = txtEmail.Text,
            .FirstName = txtFirstName.Text,
            .LastName = txtLastName.Text,
            .Prefix = txtPrefix.Text,
            .Organization = txtOrganization.Text,
            .PostalCode = txtPostalCode.Text,
            .State = txtState.Text,
            .Telephone = txtTelephone.Text,
            .Title = txtTitle.Text
        }

        If SaveMailContact(currentAirs, CurrentCategory, contact, currentUser.UserId) Then
            pContactSaveSuccess.Visible = True
        Else
            pContactSaveError.Visible = True
        End If

        LoadCurrentData()
    End Sub

    Protected Sub RemoveEmail(sender As Object, e As EventArgs)
        ClearWarnings()

        Dim email As String = CType(sender, Button).CommandArgument

        If RemoveEmailContact(currentAirs, CurrentCategory, email, currentUser.UserId) Then
            pEmailRemovedSuccess.Visible = True
        Else
            pEmailListError.Visible = True
        End If

        LoadCurrentData()
    End Sub

    Protected Sub AddNewEmail(sender As Object, e As EventArgs) Handles btnAddNewEmail.Click
        ClearWarnings()

        Dim email As String = Trim(txtNewEmail.Text)

        If Not IsValidEmailAddress(email) Then
            pAddEmailInvalid.Visible = True
        Else
            Dim result As AddEmailContactResultStatus = AddEmailContact(currentAirs, CurrentCategory, email, currentUser.UserId)

            Select Case result
                Case AddEmailContactResultStatus.DbError
                    pAddEmailError.Visible = True

                Case AddEmailContactResultStatus.EmailExists
                    pAddEmailExists.Visible = True

                Case AddEmailContactResultStatus.Success
                    SendEmailContactNotificationEmail(currentAirs, email, CurrentCategory)
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

        pContactSaveError.Visible = False
        pContactSaveSuccess.Visible = False

        pEmailRemovedSuccess.Visible = False
        pEmailListError.Visible = False
        pEmailAddedSuccess.Visible = False
        pEmailAlreadyRemoved.Visible = False
    End Sub

End Class
