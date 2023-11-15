Imports GaEpd.DBUtilities
Imports GECO.DAL.EIS
Imports GECO.GecoModels
Imports GECO.MapHelper

Public Class EIS_Default
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId
    Private Property EiStatus As EisStatus

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        LoadCurrentAirs()
        Master.SelectedTab = EIS.EisTab.Home

        Dim facilityAccess As FacilityAccess = GetCurrentUser().GetFacilityAccess(CurrentAirs)

        If facilityAccess Is Nothing OrElse Not facilityAccess.EisAccess Then
            Response.Redirect("~/Home/")
        End If

        LoadEisStatus()
        LoadFacilityDetails()
        LoadCurrentCaersUsers()
    End Sub

    Private Sub LoadCurrentAirs()
        If IsPostBack Then
            CurrentAirs = New ApbFacilityId(GetCookie(Cookie.AirsNumber))
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

        Master.CurrentAirs = CurrentAirs
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

    Private Sub LoadCurrentCaersUsers()
        Dim dt As DataTable = GetFacilityCaerContacts(CurrentAirs)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            grdCaersUsers.Visible = True
            grdCaersUsers.DataSource = dt
            grdCaersUsers.DataBind()
            pNoUsersNotice.Visible = False
        Else
            grdCaersUsers.DataSource = Nothing
            grdCaersUsers.Visible = False
            pNoUsersNotice.Visible = True
        End If
    End Sub

End Class
