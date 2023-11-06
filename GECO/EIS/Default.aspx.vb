Imports GaEpd.DBUtilities
Imports GECO.DAL.EIS
Imports GECO.GecoModels
Imports GECO.MapHelper

Public Class EIS_Default
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

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
        Master.SelectedTab = EIS.EisTab.Home

        Dim facilityAccess As FacilityAccess = GetCurrentUser().GetFacilityAccess(CurrentAirs)

        If facilityAccess Is Nothing OrElse Not facilityAccess.EisAccess Then
            Response.Redirect("~/Home/")
        End If

        ShowEisLink()
        LoadFacilityDetails()
        LoadCurrentCaersUsers()
    End Sub

    Private Sub ShowEisLink()
        ' | EISACCESSCODE | STRDESC                                                 |
        ' |---------------|---------------------------------------------------------|
        ' | 0             | FI access allowed with edit; EI access allowed, no edit |
        ' | 1             | FI and EI access allowed, both with edit                |
        ' | 2             | FI and EI access allowed, both no edit                  |
        ' | 3             | Facility not in EIS                                     |
        ' | 4             | Facility has no access to FI or EI                      |

        ' EISAccess = 3 indicates that the facility is not in the EIS_Admin table.
        ' "3" is not stored in the admin table; it is set in the GetEisStatus method

        ' | EISSTATUSCODE | STRDESC                  |
        ' |---------------|--------------------------|
        ' | 0             | Not applicable           |
        ' | 1             | Applicable - not started |
        ' | 2             | In progress              |
        ' | 3             | Submitted                |
        ' | 4             | QA Process               |
        ' | 5             | Complete                 |

        ' NOTE: For the new process (using EPA's CAERS app), status codes 2 through 5 are no longer used.

        Dim EiStatus As EisStatus = GetEiStatus(CurrentAirs)

        If Not EiStatus.Enrolled OrElse EiStatus.AccessCode >= 3 Then
            Response.Redirect("~/Facility/")
        End If

        If EiStatus.StatusCode = 0 Then
            lblCdxAlt.Text = "EI not applicable."
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

        If latitude Is Nothing OrElse longitude Is Nothing Then
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(locationAddress.Street, locationAddress.City)
            lnkGoogleMap.NavigateUrl = GetMapLinkUrl(locationAddress.Street, locationAddress.City)
        Else
            Dim coords As New Coordinate(latitude.Value, longitude.Value)
            imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(coords)
            lnkGoogleMap.NavigateUrl = GetMapLinkUrl(coords)
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
