Imports GaEpd.DBUtilities
Imports GECO.DAL.EIS
Imports GECO.GecoModels
Imports GECO.GecoModels.EIS
Imports GECO.MapHelper

Public Class EIS_Default
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId
    Private Property EiStatus As EisStatus

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        LoadCurrentAirs()

        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.Home
        Master.Master.ClearDefaultButton()

        Dim facilityAccess As FacilityAccess = GetCurrentUser().GetFacilityAccess(CurrentAirs)

        If facilityAccess Is Nothing OrElse Not facilityAccess.EisAccess Then
            Response.Redirect("~/Home/")
        End If

        If Not IsPostBack Then
            LoadEisStatus()
            LoadFacilityDetails()
            LoadStates()
            LoadCurrentCaersUsers()
        End If
    End Sub

    Private Sub LoadCurrentAirs()
        If IsPostBack Then
            Dim airs As String = GetCookie(Cookie.AirsNumber)

            If String.IsNullOrEmpty(airs) Then
                Response.Redirect("~/")
            End If

            CurrentAirs = New ApbFacilityId(airs)
        Else
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.IsValidAirsNumberFormat(airsString) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            CurrentAirs = New ApbFacilityId(airsString)
            SetCookie(Cookie.AirsNumber, CurrentAirs.ShortString())
        End If
    End Sub

    Private Sub LoadEisStatus()
        EiStatus = GetEiStatus(CurrentAirs)

        If EiStatus.AccessCode >= 3 Then
            Response.Redirect("~/Facility/")
        End If

        Dim eiYear As Integer = GetCurrentEiYear()

        If Not EiStatus.Enrolled Then
            lblEnrollmentStatus.Text = $"This facility is not enrolled in the {eiYear} EI."
            lblEnrollmentStatus.Visible = True
            submitEiSection.Visible = False
        End If

        If EiStatus.StatusCode = 0 Then
            lblCdxAlt.Visible = True
        Else
            CdxLink.NavigateUrl = ConfigurationManager.AppSettings("EpaCaersUrl")
            CdxLink.Visible = True
        End If
    End Sub

    Private Sub LoadFacilityDetails()
        Dim dr As DataRow = GetEisFacilityDetails(CurrentAirs)

        If dr Is Nothing Then
            Throw New ArgumentException($"EIS Facility Details not available for {CurrentAirs.FormattedString}")
        End If

        ' Description
        lblDescription.Text = GetNullableString(dr.Item("strFacilitySiteDescription"))
        lblOperatingStatus.Text = GetNullableString(dr.Item("strFacilitySiteStatusDesc"))
        If Not Convert.IsDBNull(dr("intFacilitySiteStatusCodeYear")) Then
            lblOperatingStatus.Text &= " as reported in " & dr("intFacilitySiteStatusCodeYear").ToString
        End If
        lblNAICS.Text = GetNullableString(dr.Item("strNAICSCode")) &
            " - " & GetNaicsCodeDesc(GetNullableString(dr.Item("strNAICSCode")))

        ' Location
        Dim locationAddress As New Address() With {
            .Street = GetNullableString(dr.Item("strLocationAddressText")).NonEmptyStringOrNothing(),
            .Street2 = GetNullableString(dr.Item("strSupplementalLocationText")).NonEmptyStringOrNothing(),
            .City = GetNullableString(dr.Item("strLocalityName")).NonEmptyStringOrNothing(),
            .State = "GA",
            .PostalCode = GetNullableString(dr.Item("strLocationAddressPostalCode")).NonEmptyStringOrNothing()
        }
        lblSiteAddress.Text = locationAddress.ToHtmlString()

        Dim latitude = GetNullable(Of Decimal?)(dr("numLatitudeMeasure"))
        Dim longitude = GetNullable(Of Decimal?)(dr("numLongitudeMeasure"))
        lblLatitude.Text = latitude.ToString
        lblLongitude.Text = longitude.ToString

        If latitude.HasValue AndAlso longitude.HasValue Then
            Dim coords As New Coordinate(latitude.Value, longitude.Value)
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(coords)
            lnkGoogleMap.NavigateUrl = GetMapLinkUrl(coords)
        Else
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(locationAddress.Street, locationAddress.City)
            lnkGoogleMap.NavigateUrl = GetMapLinkUrl(locationAddress.Street, locationAddress.City)
        End If
    End Sub

    ' ===== CAERS Users =====

    Private Sub LoadCurrentCaersUsers()
        Dim dt As DataTable = GetFacilityCaerContacts(CurrentAirs)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            hidCertifiersCount.Value = dt.Select($"CaerRole = '{CaerRole.Certifier}'").Length.ToString
            hidPreparersCount.Value = dt.Select($"CaerRole = '{CaerRole.Preparer}'").Length.ToString

            grdCaersUsers.Visible = True
            grdCaersUsers.DataSource = dt
            grdCaersUsers.DataBind()

            pAddNew.Visible = True
            btnCancelNew.Visible = True
            pnlAddNew.Visible = False
        Else
            hidCertifiersCount.Value = "0"
            hidPreparersCount.Value = "0"

            grdCaersUsers.DataSource = Nothing
            grdCaersUsers.Visible = False

            pAddNew.Visible = False
            btnCancelNew.Visible = False
            pnlAddNew.Visible = True
        End If

        rRoleNew.Visible = CInt(hidCertifiersCount.Value) = 0
        reqvRoleNew.Visible = CInt(hidCertifiersCount.Value) = 0
        rRolePreparer.Visible = CInt(hidCertifiersCount.Value) > 0
    End Sub

    Private Sub LoadStates()
        ddlStateNew.Items.Add("--Select a State--")
        ddlStateEdit.Items.Add("--Select a State--")

        Dim query As String = "Select strState, strAbbrev FROM LookUpStates order by strAbbrev"
        Dim dt As DataTable = DB.GetDataTable(query)

        For Each dr As DataRow In dt.Rows
            Dim newListItem As New ListItem With {
                .Text = dr.Item("strState").ToString,
                .Value = dr.Item("strAbbrev").ToString
            }
            ddlStateNew.Items.Add(newListItem)
            ddlStateEdit.Items.Add(newListItem)
        Next
    End Sub

    ' Delete existing user

    Private Sub grdCaersUsers_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCaersUsers.RowDeleting
        NotNull(e, NameOf(e))
        Dim userid As Guid = Guid.Parse(grdCaersUsers.DataKeys(e.RowIndex).Values("Id").ToString())
        DeleteCaerContact(userid)
        LoadCurrentCaersUsers()
    End Sub

    ' Add new user

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        pAddNew.Visible = False
        pnlAddNew.Visible = True
        pnlEditUser.Visible = False
    End Sub

    Private Sub CancelNewUser_Click(sender As Object, e As EventArgs) Handles btnCancelNew.Click
        pAddNew.Visible = True
        pnlAddNew.Visible = False
    End Sub

    Private Sub btnSaveNew_Click(sender As Object, e As EventArgs) Handles btnSaveNew.Click
        If Page.IsValid Then
            Dim address As New Address With {
            .Street = txtStreetNew.Text,
            .Street2 = txtStreet2New.Text,
            .City = txtCityNew.Text,
            .State = ddlStateNew.SelectedValue,
            .PostalCode = txtPostalCodeNew.Text
        }

            Dim contact As New Person With {
                .Address = address,
                .Company = txtCompanyNew.Text,
                .Email = txtEmailNew.Text,
                .Honorific = txtPrefixNew.Text,
                .FirstName = txtFirstNameNew.Text,
                .LastName = txtLastNameNew.Text,
                .PhoneNumber = txtTelephoneNew.Text,
                .Title = txtTitleNew.Text
            }

            Dim caerContact As New CaerContact With {
                .Active = True,
                .Contact = contact,
                .FacilitySiteId = CurrentAirs
            }

            Select Case rRoleNew.SelectedValue
                Case CaerRole.Certifier.ToString
                    caerContact.CaerRole = CaerRole.Certifier
                    SaveCaerContact(caerContact)
                Case "Both"
                    caerContact.CaerRole = CaerRole.Preparer
                    SaveCaerContact(caerContact)
                    caerContact.CaerRole = CaerRole.Certifier
                    SaveCaerContact(caerContact)
                Case Else
                    caerContact.CaerRole = CaerRole.Preparer
                    SaveCaerContact(caerContact)
            End Select

            pAddNew.Visible = True
            pnlAddNew.Visible = False

            LoadCurrentCaersUsers()
        End If
    End Sub

    Private Sub custEmailNew_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custEmailNew.ServerValidate
        args.IsValid = rRoleNew.SelectedValue = CaerRole.Certifier.ToString OrElse Not CaerPreparerExists(args.Value, CurrentAirs)
    End Sub

    ' Edit existing user

    Private Sub grdCaersUsers_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdCaersUsers.RowEditing
        NotNull(e, NameOf(e))

        Dim userId As Guid = CType(grdCaersUsers.DataKeys(e.NewEditIndex).Item("Id"), Guid)
        Dim caerUser As CaerContact = GetCaerContact(userId)

        If caerUser IsNot Nothing AndAlso caerUser.Active Then
            pnlEditUser.Visible = True
            pnlAddNew.Visible = False
            hidEditId.Value = userId.ToString
            pAddNew.Visible = False

            With caerUser.Contact
                txtStreetEdit.Text = .Address.Street
                txtStreet2Edit.Text = .Address.Street2
                txtCityEdit.Text = .Address.City
                ddlStateEdit.SelectedValue = .Address.State
                txtPostalCodeEdit.Text = .Address.PostalCode
                txtCompanyEdit.Text = .Company
                txtEmailEdit.Text = .Email
                txtPrefixEdit.Text = .Honorific
                txtFirstNameEdit.Text = .FirstName
                txtLastNameEdit.Text = .LastName
                txtTelephoneEdit.Text = .PhoneNumber
                txtTitleEdit.Text = .Title
            End With

            ddlRoleEdit.Items.Clear()
            ddlRoleEdit.Items.Add(CaerRole.Preparer.ToString)
            If CInt(hidCertifiersCount.Value) = 0 OrElse caerUser.CaerRole = CaerRole.Certifier Then
                ddlRoleEdit.Items.Add(CaerRole.Certifier.ToString)
                ddlRoleEdit.Enabled = True
            Else
                ddlRoleEdit.Enabled = False
            End If
            ddlRoleEdit.SelectedValue = caerUser.CaerRole.ToString()
        End If
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object, e As EventArgs) Handles btnCancelEdit.Click
        pnlEditUser.Visible = False
        pAddNew.Visible = True
    End Sub

    Private Sub btnSaveEdit_Click(sender As Object, e As EventArgs) Handles btnSaveEdit.Click
        If Page.IsValid Then
            Dim address As New Address With {
                .Street = txtStreetEdit.Text,
                .Street2 = txtStreet2Edit.Text,
                .City = txtCityEdit.Text,
                .State = ddlStateEdit.SelectedValue,
                .PostalCode = txtPostalCodeEdit.Text
            }

            Dim contact As New Person With {
                .Address = address,
                .Company = txtCompanyEdit.Text,
                .Email = txtEmailEdit.Text,
                .Honorific = txtPrefixEdit.Text,
                .FirstName = txtFirstNameEdit.Text,
                .LastName = txtLastNameEdit.Text,
                .PhoneNumber = txtTelephoneEdit.Text,
                .Title = txtTitleEdit.Text
            }

            Dim role As CaerRole = CType([Enum].Parse(GetType(CaerRole), ddlRoleEdit.SelectedValue), CaerRole)

            Dim caerContact As New CaerContact With {
                .Active = True,
                .CaerRole = role,
                .Contact = contact,
                .FacilitySiteId = CurrentAirs
            }

            UpdateCaerContact(caerContact, New Guid(hidEditId.Value))

            pAddNew.Visible = True
            pnlAddNew.Visible = False
            pnlEditUser.Visible = False

            LoadCurrentCaersUsers()
        End If
    End Sub

    Private Sub custEmailEdit_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custEmailEdit.ServerValidate
        args.IsValid = Not (ddlRoleEdit.SelectedValue = CaerRole.Preparer.ToString AndAlso
            CaerPreparerExists(args.Value, CurrentAirs, New Guid(hidEditId.Value)))
    End Sub

End Class
