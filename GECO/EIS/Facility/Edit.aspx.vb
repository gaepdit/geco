Imports EpdIt.DBUtilities
Imports GECO.DAL.EIS
Imports GECO.GecoModels
Imports GECO.MapHelper
Imports Reimers.Core.Maps
Imports Reimers.Google.Map

Partial Class EIS_Facility_EditPage
    Inherits Page

    Public Property CurrentAirs As ApbFacilityId
    Private Property CurrentUser As GecoUser
    Public Property IsBeginEisProcess As Boolean = False

    Private NAICSExists As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        CurrentUser = GetCurrentUser()
        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.Facility

        Dim eiStatus As EisStatus = GetEiStatus(CurrentAirs)
        If eiStatus.AccessCode > 1 Then Response.Redirect("~/EIS/Facility/")

        If Session("EisProcessStarted") IsNot Nothing Then
            IsBeginEisProcess = True
            Master.IsBeginEisProcess = True
            SetUpEisProcess()
        End If

        If Not IsPostBack Then
            LoadDropdownLists()
            LoadFacilityLatLongValidation()
            LoadFacilityDetails()
            LoadPhoneNumbers()
            CheckIfFacilityLatLonIsLocked()
            PopulateNAICSGridView()

            pnlFacilityEdit.Visible = True
            pnlNAICSCodeLookup.Visible = False
            pnlMap.Visible = False

            txtMapLat.Attributes.Add("readonly", "readonly")
            txtMapLon.Attributes.Add("readonly", "readonly")

            Master.Master.SetDefaultButton(btnSave)
        End If
    End Sub

    Private Sub SetUpEisProcess()
        btnSave.CssClass = "button-large button-proceed"
        btnSave.Text = "Save Facility Information and Continue →"
    End Sub

    Private Sub LoadDropdownLists()
        LoadHorCollectDDL()
        LoadHorRefDatumDDL()
        LoadFacilityStateDDL()
        LoadContactStateDDL()
    End Sub

    Private Sub LoadHorCollectDDL()
        ddlHorCollectionMetCode.Items.Add("--Select Horizontal Collection Method--")

        Dim query = "select strDesc, HorCollMetCode FROM EISLK_HORCOLLMETCODE where Active = '1' order by strDesc"
        Dim dt As DataTable = DB.GetDataTable(query)

        If dt IsNot Nothing Then
            For Each dr As DataRow In dt.Rows
                Dim newListItem As New ListItem With {
                    .Text = dr.Item("strdesc").ToString,
                    .Value = dr.Item("HorCollMetCode").ToString
                }
                ddlHorCollectionMetCode.Items.Add(newListItem)
            Next
        End If
    End Sub

    Private Sub LoadHorRefDatumDDL()
        ddlHorReferenceDatCode.Items.Add("--Select Horizontal Reference Datum--")

        Dim query = " select strDesc, HorRefDatumCode FROM EISLK_HORREFDATUMCODE where ACTIVE = '1' order by strDesc"
        Dim dt As DataTable = DB.GetDataTable(query)

        If dt IsNot Nothing Then
            For Each dr As DataRow In dt.Rows
                Dim newListItem As New ListItem With {
                    .Text = dr.Item("strdesc").ToString,
                    .Value = dr.Item("HorRefDatumCode").ToString
                }
                ddlHorReferenceDatCode.Items.Add(newListItem)
            Next
        End If
    End Sub

    Private Sub LoadFacilityStateDDL()
        ddlFacility_StateMail.Items.Add("--Select a State--")

        Dim query As String = "Select strState, strAbbrev FROM LookUpStates order by strAbbrev"
        Dim dt As DataTable = DB.GetDataTable(query)

        For Each dr As DataRow In dt.Rows
            Dim newListItem As New ListItem With {
                .Text = dr.Item("strState").ToString,
                .Value = dr.Item("strAbbrev").ToString
            }
            ddlFacility_StateMail.Items.Add(newListItem)
        Next
    End Sub

    Private Sub LoadContactStateDDL()
        ddlContact_MailState.Items.Add("--Select a State--")

        Dim query As String = "Select strState, strAbbrev FROM LookUpStates order by strAbbrev"
        Dim dt As DataTable = DB.GetDataTable(query)

        For Each dr As DataRow In dt.Rows
            Dim newListItem As New ListItem With {
                .Text = dr.Item("strState").ToString,
                .Value = dr.Item("strAbbrev").ToString
            }
            ddlContact_MailState.Items.Add(newListItem)
        Next
    End Sub

    Private Sub LoadFacilityLatLongValidation()
        If IsFacilityLatLonLocked(CurrentAirs) Then
            rngvLatitudeMeasure.Enabled = False
            rngvLongitudeMeasure.Enabled = False
            Return
        End If

        Dim mm As MinMaxLatLon = GetCountyLatLong(CurrentAirs.CountySubstring)

        rngvLatitudeMeasure.MaximumValue = mm.MaxLat.ToString
        rngvLatitudeMeasure.MinimumValue = mm.MinLat.ToString
        rngvLatitudeMeasure.ErrorMessage = "The Latitude must be between " & mm.MinLat.ToString & " and " & mm.MaxLat.ToString & "."
        rngvLatitudeMeasure.Text = "Must be between " & mm.MinLat.ToString & " and " & mm.MaxLat.ToString

        rngvLongitudeMeasure.MaximumValue = mm.MaxLon.ToString
        rngvLongitudeMeasure.MinimumValue = mm.MinLon.ToString
        rngvLongitudeMeasure.ErrorMessage = "The Latitude must be between " & mm.MinLon.ToString & " and " & mm.MaxLon.ToString & "."
        rngvLongitudeMeasure.Text = "Must be between " & mm.MinLon.ToString & " and " & mm.MaxLon.ToString
    End Sub

    Private Sub LoadFacilityDetails()
        Dim dr As DataRow = GetEisFacilityDetails(CurrentAirs)

        If dr Is Nothing Then
            Throw New ArgumentException($"EIS Facility Details not available for {CurrentAirs.FormattedString}")
        End If

        ' Name and Address
        lblFacilityName.Text = GetNullableString(dr.Item("strFacilitySiteName"))

        Dim siteAddress As New Address() With {
            .Street = GetNullableString(dr.Item("strLocationAddressText")).NonEmptyStringOrNothing(),
            .Street2 = GetNullableString(dr.Item("strSupplementalLocationText")).NonEmptyStringOrNothing(),
            .City = GetNullableString(dr.Item("strLocalityName")).NonEmptyStringOrNothing(),
            .State = "GA",
            .PostalCode = GetNullableString(dr.Item("strLocationAddressPostalCode")).NonEmptyStringOrNothing()
        }
        lblSiteAddress.Text = siteAddress.ToHtmlString()

        ' Mailing Address
        txtMailingAddressText.Text = GetNullableString(dr.Item("strMailingAddressText"))
        txtSupplementalAddressText.Text = GetNullableString(dr.Item("strSupplementalAddressText"))
        txtMailingAddressCityName.Text = GetNullableString(dr.Item("strMailingAddressCityName"))
        txtMailingAddressPostalCode.Text = GetNullableString(dr.Item("strMailingAddressPostalCode"))
        txtMailingAddressComment.Text = GetNullableString(dr.Item("strAddressComment"))

        If IsDBNull(dr("strMailingAddressStateCode")) Then
            ddlFacility_StateMail.SelectedValue = ""
        Else
            ddlFacility_StateMail.SelectedValue = dr.Item("strMailingAddressStateCode").ToString
        End If

        ' Facility Description

        txtFacilitySiteDescription.Text = GetNullableString(dr.Item("strFacilitySiteDescription"))
        txtNAICSCode.Text = GetNullableString(dr.Item("strNAICSCode"))
        txtFacilitySiteComment.Text = GetNullableString(dr.Item("strFacilitySiteComment"))

        ' Geographic Coordinates

        Dim latMeasure As Decimal = GetNullable(Of Decimal)(dr("numLatitudeMeasure"))
        Dim lonMeasure As Decimal = GetNullable(Of Decimal)(dr("numLongitudeMeasure"))

        txtLatitudeMeasure.Text = latMeasure.ToString
        txtLongitudeMeasure.Text = lonMeasure.ToString

        If latMeasure > 0 AndAlso lonMeasure > 0 Then
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(New Coordinate(latMeasure, lonMeasure))
            lnkGoogleMap.NavigateUrl = GetMapLinkUrl(New Coordinate(latMeasure, lonMeasure))
        Else
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(siteAddress.Street, siteAddress.City)
            lnkGoogleMap.NavigateUrl = GetMapLinkUrl(siteAddress.Street, siteAddress.City)
        End If

        If IsDBNull(dr("HorCollMetCode")) Then
            ddlHorCollectionMetCode.SelectedValue = ""
        Else
            ddlHorCollectionMetCode.SelectedValue = dr.Item("HorCollMetCode").ToString
        End If

        txtHorizontalAccuracyMeasure.Text = GetNullableString(dr("intHorAccuracyMeasure"))

        If txtHorizontalAccuracyMeasure.Text = "-1" Then
            txtHorizontalAccuracyMeasure.Text = ""
        End If

        If IsDBNull(dr("HorRefDatumCode")) Then
            ddlHorReferenceDatCode.SelectedValue = ""
        Else
            ddlHorReferenceDatCode.SelectedValue = dr.Item("HorRefDatumCode").ToString
        End If

        txtGeographicComment.Text = GetNullableString(dr.Item("strGeographicComment"))

        ' Contact
        txtPrefix.Text = GetNullableString(dr.Item("strNamePrefixText"))
        txtFirstName.Text = GetNullableString(dr.Item("strFirstName"))
        txtLastName.Text = GetNullableString(dr.Item("strLastName"))
        txtIndividualTitleText.Text = GetNullableString(dr.Item("STRINDIVIDUALTITLETEXT"))
        txtMailingAddressText_Contact.Text = GetNullableString(dr.Item("STRFSAIMADDRESSTEXT"))
        txtSupplementalAddressText_Contact.Text = GetNullableString(dr.Item("STRFSAISADDRESSTEXT"))
        txtMailingAddressCityName_Contact.Text = GetNullableString(dr.Item("STRFSAIMADDRESSCITYNAME"))


        If IsDBNull(dr("STRFSAIMADDRESSSTATECODE")) Then
            ddlContact_MailState.SelectedValue = ""
        Else
            ddlContact_MailState.SelectedValue = dr.Item("STRFSAIMADDRESSSTATECODE").ToString
        End If

        txtMailingAddressPostalCode_Contact.Text = GetNullableString(dr.Item("STRFSAIMADDRESSPOSTALCODE"))
        txtElectronicAddressText.Text = GetNullableString(dr.Item("StrElectronicAddressText"))
        txtAddressComment_Contact.Text = GetNullableString(dr.Item("strFSAIAddressComment"))

        ' Store previous Geographic Data for comparison on submit
        hidLatitude.Value = txtLatitudeMeasure.Text
        hidLongitude.Value = txtLongitudeMeasure.Text
        hidHorCollectionMetCode.Value = ddlHorCollectionMetCode.SelectedValue
        hidHorCollectionMetDesc.Value = ddlHorCollectionMetCode.SelectedItem.Text
        hidHorizontalAccuracyMeasure.Value = txtHorizontalAccuracyMeasure.Text
        hidHorReferenceDatCode.Value = ddlHorReferenceDatCode.SelectedValue
        hidHorReferenceDatDesc.Value = ddlHorReferenceDatCode.SelectedItem.Text
        hidGeographicComment.Value = txtGeographicComment.Text
    End Sub

    Private Sub LoadPhoneNumbers()
        Dim dt As DataTable = GetEisContactPhoneNumbers(CurrentAirs)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                Select Case dr.Item("TELEPHONENUMBERTYPECODE").ToString
                    Case "W"
                        txtTelephoneNumberText.Text = GetNullableString(dr.Item("STRTELEPHONENUMBERTEXT"))
                    Case "F"
                        txtTelephoneNumber_Fax.Text = GetNullableString(dr.Item("STRTELEPHONENUMBERTEXT"))
                    Case "M"
                        txtTelephoneNumber_Mobile.Text = GetNullableString(dr.Item("STRTELEPHONENUMBERTEXT"))
                End Select
            Next
        End If
    End Sub

    Private Sub CheckIfFacilityLatLonIsLocked()
        If IsFacilityLatLonLocked(CurrentAirs) Then
            txtLatitudeMeasure.Enabled = False
            txtLongitudeMeasure.Enabled = False
            ddlHorCollectionMetCode.Enabled = False
            txtHorizontalAccuracyMeasure.Enabled = False
            ddlHorReferenceDatCode.Enabled = False
            txtGeographicComment.Enabled = False

            btnGetLatLon.Visible = False
            pGeoInfo.Visible = False
            pLatLonLocked.Visible = True
        End If
    End Sub

    Private Sub PopulateNAICSGridView()
        If Cache("NAICSDataTable") Is Nothing Then
            Dim NAICSDataTable As DataTable = GetNaicsDataTable()
            Cache.Insert("NAICSDataTable", NAICSDataTable, Nothing, Now.AddHours(24), Cache.NoSlidingExpiration)
            Session("MyNAICSView") = NAICSDataTable
        Else
            Session("MyNAICSView") = DirectCast(Cache("NAICSDataTable"), DataTable)
        End If

        Dim dt As DataTable = GetSessionItem(Of DataTable)("MyNAICSView")
        Dim rowCount = dt.Rows.Count
        If rowCount > 0 Then
            gvwNAICS.DataSource = dt
            gvwNAICS.DataBind()
            lblRowCount.Text = "No. of NAICS Codes: " & rowCount
        Else
            gvwNAICS.DataSource = Nothing
            lblRowCount.Text = ""
        End If
    End Sub

    ' Data save routines

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SaveAll()
    End Sub

    Private Sub SaveAll()

        If Not NAICSExists Then
            'Do nothing - enables display of custom validator message
        Else
            SaveFacilityMailingAddress()
            SaveFacilitySiteinfo()
            SaveFacilityGCInfo()
            SaveFacilityContact()
            SaveContactPhoneNumber()
            SaveAPBContactInformation()

            If IsBeginEisProcess Then
                Response.Redirect("~/EIS/Process/")
            Else
                Session("FacilityUpdated") = True.ToString
                Response.Redirect("~/EIS/Facility/")
            End If
        End If

    End Sub


    Private Sub SaveFacilitySiteinfo()
        SaveEisFacilitySiteInfo(CurrentAirs, txtFacilitySiteDescription.Text, txtNAICSCode.Text,
            txtFacilitySiteComment.Text, CurrentUser)
    End Sub

    Private Sub SaveFacilityMailingAddress()
        Dim address As New Address With {
            .Street = txtMailingAddressText.Text,
            .Street2 = txtSupplementalAddressText.Text,
            .City = txtMailingAddressCityName.Text,
            .State = ddlFacility_StateMail.SelectedValue,
            .PostalCode = txtMailingAddressPostalCode.Text
        }

        SaveEisFacilityMailingAddress(CurrentAirs, address, txtMailingAddressComment.Text, CurrentUser)
    End Sub

    Private Sub SaveFacilityGCInfo()
        If IsFacilityLatLonLocked(CurrentAirs) Then Return

        Dim curGoogleMapLink As String = "none"
        If Decimal.TryParse(hidLatitude.Value, Nothing) AndAlso Decimal.TryParse(hidLongitude.Value, Nothing) Then
            curGoogleMapLink = GetMapLinkUrl(New Coordinate(CDec(hidLatitude.Value), CDec(hidLongitude.Value)))
        End If

        Dim gcUpdated As Boolean =
                hidLatitude.Value <> txtLatitudeMeasure.Text OrElse
                hidLongitude.Value <> txtLongitudeMeasure.Text OrElse
                hidHorCollectionMetCode.Value <> ddlHorCollectionMetCode.SelectedValue OrElse
                hidHorizontalAccuracyMeasure.Value <> txtHorizontalAccuracyMeasure.Text OrElse
                hidHorReferenceDatCode.Value <> ddlHorReferenceDatCode.SelectedValue

        If gcUpdated Then
            ' Email APB if any Geographic Coordinate Information changed

            Dim newGoogleMapLink As String = "none"
            Dim latMeasure As Decimal
            Dim lonMeasure As Decimal
            If Decimal.TryParse(txtLatitudeMeasure.Text, latMeasure) AndAlso Decimal.TryParse(txtLongitudeMeasure.Text, lonMeasure) Then
                newGoogleMapLink = GetMapLinkUrl(New Coordinate(latMeasure, lonMeasure))
            End If

            Dim plainBody As String = "An update has been submitted for the EIS Facility Geographic Coordinate Information " &
                    "for the following facility. The updated information has NOT been saved in the database. If approved, you must " &
                    "manually update the facility using the IAIP EIS Tool." & vbNewLine &
                    vbNewLine &
                    "Facility Site ID: " & CurrentAirs.FormattedString & vbNewLine &
                    vbNewLine &
                    "Update User: " & CurrentUser.FullName & " (" & CurrentUser.UserId & ")" & vbNewLine &
                    vbNewLine &
                    "Current Geographic Coordinate Information: " & vbNewLine &
                    vbNewLine &
                    "    Latitude: " & hidLatitude.Value & vbNewLine &
                    "    Longitude: " & hidLongitude.Value & vbNewLine &
                    "    Horizontal Collection Method: " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & vbNewLine &
                    "    Accuracy Measure: " & hidHorizontalAccuracyMeasure.Value & vbNewLine &
                    "    Horizontal Reference Datum: " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & vbNewLine &
                    "    Google Map: " & curGoogleMapLink & vbNewLine &
                    vbNewLine &
                    "Updated Geographic Coordinate Information submitted by user: " & vbNewLine &
                    vbNewLine &
                    "    Latitude: " & txtLatitudeMeasure.Text & vbNewLine &
                    "    Longitude: " & txtLongitudeMeasure.Text & vbNewLine &
                    "    Horizontal Collection Method: " & ddlHorCollectionMetCode.SelectedValue & " - " & ddlHorCollectionMetCode.SelectedItem.Text & vbNewLine &
                    "    Accuracy Measure: " & txtHorizontalAccuracyMeasure.Text & vbNewLine &
                    "    Horizontal Reference Datum: " & ddlHorReferenceDatCode.SelectedValue & " - " & ddlHorReferenceDatCode.SelectedItem.Text & vbNewLine &
                    "    Google Map: " & newGoogleMapLink & vbNewLine &
                    vbNewLine &
                    "Comment submitted by user: " & vbNewLine &
                    vbNewLine &
                    txtGeographicComment.Text & vbNewLine

            Dim htmlBody As String = "<p>An update has been submitted for the EIS Facility Geographic Coordinate Information " &
                    "for the following facility. The updated information has <em>NOT</em> been saved in the database. If approved, you must " &
                    "manually update the facility using the IAIP EIS Tool.</p>" &
                    "<p><b>Facility Site ID:</b> " & CurrentAirs.FormattedString & "</p>" &
                    "<p><b>Update User:</b> " & CurrentUser.FullName & " (" & CurrentUser.UserId & ")" & "</p>" &
                    "<p><b>Current Geographic Coordinate Information:</b> " & "</p>" &
                    "<ul>" &
                    "<li><b>Latitude:</b> " & hidLatitude.Value & "</li>" &
                    "<li><b>Longitude:</b> " & hidLongitude.Value & "</li>" &
                    "<li><b>Horizontal Collection Method:</b> " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & "</li>" &
                    "<li><b>Accuracy Measure:</b> " & hidHorizontalAccuracyMeasure.Value & "</li>" &
                    "<li><b>Horizontal Reference Datum:</b> " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & "</li>" &
                    "<li><b>Google Map:</b> " & curGoogleMapLink & "</li>" &
                    "</ul>" &
                    "<p><b>Updated Geographic Coordinate Information submitted by user:</b> " & "</p>" &
                    "<ul>" &
                    "<li><b>Latitude:</b> " & txtLatitudeMeasure.Text & "</li>" &
                    "<li><b>Longitude:</b> " & txtLongitudeMeasure.Text & "</li>" &
                    "<li><b>Horizontal Collection Method:</b> " & ddlHorCollectionMetCode.SelectedValue & " - " & ddlHorCollectionMetCode.SelectedItem.Text & "</li>" &
                    "<li><b>Accuracy Measure:</b> " & txtHorizontalAccuracyMeasure.Text & "</li>" &
                    "<li><b>Horizontal Reference Datum:</b> " & ddlHorReferenceDatCode.SelectedValue & " - " & ddlHorReferenceDatCode.SelectedItem.Text & "</li>" &
                    "<li><b>Google Map:</b> " & newGoogleMapLink & "</li>" &
                    "</ul>" &
                    "<p><b>Comment submitted by user:</b> " & "</p>" &
                    "<blockquote><pre>" & txtGeographicComment.Text & "</pre></blockquote>"

            SendEmail(GecoContactEmail, "GECO Emission Inventory - Facility Geographic Info Update Request", plainBody, htmlBody,
                          caller:="eis_facility_edit.SaveFacilityGCInfo (1)")

        ElseIf hidGeographicComment.Value <> txtGeographicComment.Text Then
            ' Send a different email to APB if only the Geographic Coordinate Information comment was changed

            Dim plainBody As String = "A comment has been submitted for the EIS Facility Geographic Coordinate Information " &
                    "for the following facility. No other geographic information was changed." & vbNewLine &
                    vbNewLine &
                    "Facility Site ID: " & CurrentAirs.FormattedString & vbNewLine &
                    vbNewLine &
                    "Update User: " & CurrentUser.FullName & " (" & CurrentUser.UserId & ")" & vbNewLine &
                    vbNewLine &
                    "Current Geographic Coordinate Information: " & vbNewLine &
                    vbNewLine &
                    "    Latitude: " & hidLatitude.Value & vbNewLine &
                    "    Longitude: " & hidLongitude.Value & vbNewLine &
                    "    Horizontal Collection Method: " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & vbNewLine &
                    "    Accuracy Measure: " & hidHorizontalAccuracyMeasure.Value & vbNewLine &
                    "    Horizontal Reference Datum: " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & vbNewLine &
                    "    Google Map: " & curGoogleMapLink & vbNewLine &
                    vbNewLine &
                    "Comment submitted by user: " & vbNewLine &
                    vbNewLine &
                    txtGeographicComment.Text & vbNewLine

            Dim htmlBody As String = "<p>A comment has been submitted for the EIS Facility Geographic Coordinate Information " &
                    "for the following facility. No other geographic information was changed.</p>" &
                    "<p><b>Facility Site ID:</b> " & CurrentAirs.FormattedString & "</p>" &
                    "<p><b>Update User:</b> " & CurrentUser.FullName & " (" & CurrentUser.UserId & ")" & "</p>" &
                    "<p><b>Current Geographic Coordinate Information:</b> " & "</p>" &
                    "<ul>" &
                    "<li><b>Latitude:</b> " & hidLatitude.Value & "</li>" &
                    "<li><b>Longitude:</b> " & hidLongitude.Value & "</li>" &
                    "<li><b>Horizontal Collection Method:</b> " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & "</li>" &
                    "<li><b>Accuracy Measure:</b> " & hidHorizontalAccuracyMeasure.Value & "</li>" &
                    "<li><b>Horizontal Reference Datum:</b> " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & "</li>" &
                    "<li><b>Google Map:</b> " & curGoogleMapLink & "</li>" &
                    "</ul>" &
                    "<p><b>Comment submitted by user:</b> " & "</p>" &
                    "<blockquote><pre>" & txtGeographicComment.Text & "</pre></blockquote>"

            SendEmail(GecoContactEmail, "GECO Emission Inventory - Facility Geographic Info Update Request", plainBody, htmlBody,
                          caller:="eis_facility_edit.SaveFacilityGCInfo (2)")
        End If

        ' Update database if comment was changed
        If hidGeographicComment.Value <> txtGeographicComment.Text Then
            UpdateEisGeographicComment(CurrentAirs, txtGeographicComment.Text, CurrentUser)
        End If
    End Sub

    Private Sub SaveFacilityContact()
        eis_FacilityData.SaveFacilityContact(txtPrefix.Text, txtFirstName.Text, txtLastName.Text,
            txtIndividualTitleText.Text, txtElectronicAddressText.Text, txtMailingAddressText_Contact.Text,
            txtSupplementalAddressText_Contact.Text, txtMailingAddressCityName_Contact.Text,
            ddlContact_MailState.SelectedValue, txtMailingAddressPostalCode_Contact.Text, txtAddressComment_Contact.Text,
            CurrentUser.DbUpdateUser, CurrentAirs.ShortString)
    End Sub

    Private Sub SaveContactPhoneNumber()
        SaveEisContactPhoneNumbers(CurrentAirs, txtTelephoneNumberText.Text, txtTelephoneNumber_Mobile.Text,
            txtTelephoneNumber_Fax.Text, CurrentUser)
    End Sub

    Private Sub SaveAPBContactInformation()
        Dim contactComment As String = "Contact info updated from the GECO EIS Application on " &
            Now.Date.ToString & ". Comments from GECO: " & txtAddressComment_Contact.Text

        DAL.Facility.SaveApbContactInformation(CurrentAirs, "41", txtPrefix.Text, txtFirstName.Text, txtLastName.Text,
            txtIndividualTitleText.Text, txtElectronicAddressText.Text, txtMailingAddressText_Contact.Text,
            txtSupplementalAddressText_Contact.Text, txtMailingAddressCityName_Contact.Text,
            ddlContact_MailState.SelectedValue, txtMailingAddressPostalCode_Contact.Text, txtTelephoneNumberText.Text,
            txtTelephoneNumber_Mobile.Text, txtTelephoneNumber_Fax.Text, contactComment, Nothing, GecoUserID)
    End Sub

    ' NAICS search tool

    Protected Sub btnNAICSLookup_Click(sender As Object, e As EventArgs) Handles btnNAICSLookup.Click
        MaintainScrollPositionOnPostBack = False
        pnlFacilityEdit.Visible = False
        pnlNAICSCodeLookup.Visible = True
        pnlMap.Visible = False
    End Sub

    Protected Sub btnSearchNAICS_Click(sender As Object, e As EventArgs) Handles btnSearchNAICS.Click
        Dim dview As New DataView With {
            .Table = DirectCast(Cache("NAICSDataTable"), DataTable)
        }

        'No search text in any field
        If txtLookupNAICSCode.Text = "" AndAlso txtLookupNAICSDesc.Text = "" Then
            dview.RowFilter = ""
        End If

        'Search text only in NAICS Code
        If txtLookupNAICSCode.Text <> "" AndAlso txtLookupNAICSDesc.Text = "" Then
            dview.RowFilter = "NAICSCode LIKE '%" & txtLookupNAICSCode.Text & "%'"
        End If

        'Search text only in NAICS Description
        If txtLookupNAICSCode.Text = "" AndAlso txtLookupNAICSDesc.Text <> "" Then
            dview.RowFilter = "strDesc LIKE '%" & txtLookupNAICSDesc.Text & "%'"
        End If

        'Search text in both fields
        If txtLookupNAICSCode.Text <> "" AndAlso txtLookupNAICSDesc.Text <> "" Then
            dview.RowFilter = "NAICSCode LIKE '%" & txtLookupNAICSCode.Text & "%' or strDesc LIKE '%" & txtLookupNAICSDesc.Text & "%'"
        End If

        gvwNAICS.DataSource = dview
        Session("MyNAICSView") = dview.ToTable

        gvwNAICS.DataBind()
        lblRowCount.Text = "No. of NAICS Codes: " & dview.ToTable.Rows.Count
    End Sub

    Protected Sub gvwNAICS_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvwNAICS.PageIndexChanging
        NotNull(e, NameOf(e))

        Dim dt As DataTable = GetSessionItem(Of DataTable)("MyNAICSView")
        gvwNAICS.DataSource = dt
        gvwNAICS.PageIndex = e.NewPageIndex
        gvwNAICS.DataBind()
        lblRowCount.Text = "No. of NAICS Codes: " & dt.Rows.Count
    End Sub

    Protected Sub btnCancelNAICS_Click(sender As Object, e As EventArgs) Handles btnCancelNAICS.Click
        txtLookupNAICSCode.Text = ""
        txtLookupNAICSDesc.Text = ""
        lblSelectedNaicsCode.Text = ""
        pNaicsSelected.Visible = False

        PopulateNAICSGridView()

        pnlFacilityEdit.Visible = True
        pnlNAICSCodeLookup.Visible = False
        pnlMap.Visible = False
    End Sub

    Protected Sub btnUseNAICSCode_Click(sender As Object, e As EventArgs) Handles btnUseNAICSCode.Click
        txtNAICSCode.Text = lblSelectedNaicsCode.Text

        pnlFacilityEdit.Visible = True
        pnlNAICSCodeLookup.Visible = False
        pnlMap.Visible = False

        txtNAICSCode.Focus()
    End Sub

    Protected Sub NAICSCheck(Sender As Object, args As ServerValidateEventArgs)
        NotNull(args, NameOf(args))

        args.IsValid = False
        NAICSExists = False

        If DoesNaicsCodeExist(args.Value) Then
            args.IsValid = True
            NAICSExists = True
        End If
    End Sub

    Protected Sub gvwNAICS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvwNAICS.SelectedIndexChanged
        Dim NAICSCode As String = gvwNAICS.SelectedValue.ToString
        pNaicsSelected.Visible = True
        lblSelectedNaicsCode.Text = NAICSCode
    End Sub

    ' Map lat/long selection tool

    Protected Sub btnGetLatLon_Click(sender As Object, e As EventArgs) Handles btnGetLatLon.Click
        MaintainScrollPositionOnPostBack = False
        pnlFacilityEdit.Visible = False
        pnlNAICSCodeLookup.Visible = False
        pnlMap.Visible = True

        Dim latitude = CDbl(txtLatitudeMeasure.Text)
        Dim longitude = CDbl(txtLongitudeMeasure.Text)
        Const epsilon = 0.0000000001#
        
        If Math.Abs(latitude) > epsilon AndAlso Math.Abs(longitude) > epsilon Then
            GMap.Center = LatLng.Create(latitude, longitude)
            txtMapLat.Text = Left(latitude.ToString(), 8)
            txtMapLon.Text = Left(longitude.ToString(), 9)
        Else
            Dim dr As DataRow = GetEisFacilityDetails(CurrentAirs)

            Dim siteAddress As New Address() With {
                .Street = GetNullableString(dr.Item("strLocationAddressText")).NonEmptyStringOrNothing(),
                .Street2 = GetNullableString(dr.Item("strSupplementalLocationText")).NonEmptyStringOrNothing(),
                .City = GetNullableString(dr.Item("strLocalityName")).NonEmptyStringOrNothing(),
                .State = "GA",
                .PostalCode = GetNullableString(dr.Item("strLocationAddressPostalCode")).NonEmptyStringOrNothing()
            }

            Dim geocoder = New Geocoding.GoogleGeocoder()
            Dim georesult = geocoder.Geocode(siteAddress.ToLinearString)

            If georesult.Status = GeocodeStatus.OK Then
                GMap.Center = georesult.Locations.FirstOrDefault.Point.ToLatLng()
                txtMapLat.Text = GMap.Center.Latitude.ToString
                txtMapLon.Text = GMap.Center.Longitude.ToString
            End If
        End If

        GMap.MapControls.Add(New Controls.ScaleControl())
        GMap.MapControls.Add(New Controls.ZoomControl())
        GMap.MapControls.Add(New Controls.MapTypeControl())
        GMap.Zoom = 15
    End Sub

    Protected Sub GMap_Click(sender As Object, e As CoordinatesEventArgs) Handles GMap.Click
        NotNull(e, NameOf(e))

        GMap.Overlays.Clear()

        Dim latitude As String = Left(e.Coordinates.Latitude.ToString, 8)
        Dim longitude As String = Left(e.Coordinates.Longitude.ToString, 9)

        Dim myOverlay As New Marker(New Guid(), CDbl(latitude), CDbl(longitude))
        GMap.Overlays.Add(myOverlay)

        Dim Mapcommand As String = GMap.UpdateOverlays()
        Mapcommand += String.Format("document.getElementById('{0}').value = " & latitude & ";", txtMapLat.ClientID)
        Mapcommand += String.Format("document.getElementById('{0}').value = " & longitude & ";", txtMapLon.ClientID)

        e.MapCommand = Mapcommand
    End Sub

    Protected Sub btnUseLatLon_Click(sender As Object, e As EventArgs) Handles btnUseLatLon.Click
        txtLatitudeMeasure.Text = txtMapLat.Text
        txtLongitudeMeasure.Text = txtMapLon.Text

        ddlHorCollectionMetCode.SelectedValue = "007"
        txtHorizontalAccuracyMeasure.Text = "25"
        ddlHorReferenceDatCode.SelectedValue = "002"

        Dim latMeasure As Decimal
        Dim lonMeasure As Decimal
        If Decimal.TryParse(txtLatitudeMeasure.Text, latMeasure) AndAlso Decimal.TryParse(txtLongitudeMeasure.Text, lonMeasure) Then
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(New Coordinate(latMeasure, lonMeasure))
        End If

        pnlFacilityEdit.Visible = True
        pnlNAICSCodeLookup.Visible = False
        pnlMap.Visible = False
    End Sub

    Protected Sub btnCloseMap_Click(sender As Object, e As EventArgs) Handles btnCloseMap.Click
        pnlFacilityEdit.Visible = True
        pnlNAICSCodeLookup.Visible = False
        pnlMap.Visible = False
    End Sub

End Class
