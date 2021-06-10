Imports EpdIt.DBUtilities
Imports GECO.DAL.EIS
Imports GECO.GecoModels
Imports GECO.MapHelper

Partial Class EIS_Facility_Default
    Inherits Page

    Public Property CurrentAirs As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.Facility

        Dim eiStatus As EisStatus = GetEiStatus(CurrentAirs)
        If eiStatus.AccessCode > 1 Then btnEdit.Visible = False

        If Session("FacilityUpdated") IsNot Nothing Then
            Session("FacilityUpdated") = Nothing
            updateMessage.Visible = True
        End If

        If Not IsPostBack Then
            LoadFacilityDetails()
        End If
    End Sub

    Private Sub LoadFacilityDetails()
        Dim updateUser As String
        Dim updateDateTime As Date?

        Dim dr As DataRow = GetEisFacilityDetails(CurrentAirs)

        If dr Is Nothing Then
            Throw New ArgumentException($"EIS Facility Details not available for {CurrentAirs.FormattedString}")
        End If

        ' Description
        lblFacilityName.Text = GetNullableString(dr.Item("strFacilitySiteName"))
        lblDescription.Text = GetNullableString(dr.Item("strFacilitySiteDescription"))
        lblOperatingStatus.Text = GetNullableString(dr.Item("strFacilitySiteStatusDesc"))
        If Not IsDBNull(dr("intFacilitySiteStatusCodeYear")) Then
            lblOperatingStatus.Text &= " as reported in " & dr("intFacilitySiteStatusCodeYear").ToString
        End If
        lblNAICS.Text = GetNullableString(dr.Item("strNAICSCode")) &
            " - " & GetNaicsCodeDesc(GetNullableString(dr.Item("strNAICSCode")))
        lblDescriptionComment.Text = GetNullableString(dr.Item("strFacilitySiteComment"))

        updateDateTime = GetNullableDateTime(dr.Item("UpdateDateTime_FacilitySite"))
        updateUser = GetNullableString(dr.Item("UpdateUser_FacilitySite"))
        If Not String.IsNullOrEmpty(updateUser) Then
            updateUser = " by " & updateUser.Remove(0, updateUser.IndexOf("-", StringComparison.Ordinal) + 1)
        End If
        lblDescriptionUpdated.Text = updateDateTime?.ToString("G") & updateUser

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
        lblHorizontalCollectionMethod.Text = GetNullableString(dr("STRHORCOLLMETDesc"))
        lblAccuracyMeasure.Text = GetNullable(Of Integer)(dr("INTHORACCURACYMEASURE")).ToString
        lblHorizontalReferenceDatum.Text = GetNullableString(dr("STRHORREFDATUMDesc"))
        lblLocationComment.Text = GetNullableString(dr("strGeographicComment"))

        updateDateTime = GetNullableDateTime(dr.Item("UpdateDateTime_GeoCoord"))
        updateUser = GetNullableString(dr.Item("UpdateUser_GeoCoord"))
        If Not String.IsNullOrEmpty(updateUser) Then
            updateUser = " by " & updateUser.Remove(0, updateUser.IndexOf("-", StringComparison.Ordinal) + 1)
        End If
        lblLocationUpdated.Text = updateDateTime?.ToString("G") & updateUser

        If latitude.HasValue AndAlso longitude.HasValue Then
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(New Coordinate(latitude.Value, longitude.Value))
            lnkGoogleMap.NavigateUrl = GetMapLinkUrl(New Coordinate(latitude.Value, longitude.Value))
        Else
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(locationAddress.Street, locationAddress.City)
            lnkGoogleMap.NavigateUrl = GetMapLinkUrl(locationAddress.Street, locationAddress.City)
        End If
    End Sub

End Class
