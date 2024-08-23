Imports Microsoft.Data.SqlClient
Imports GaEpd.DBUtilities
Imports GECO.GecoModels
Imports GECO.MapHelper

Partial Class FacilitySummary
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property facilityAccess As FacilityAccess
    Private Property currentAirs As ApbFacilityId
    Private Property currentFacility As String = Nothing

#Region " Page Load "

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

        MainLoginCheck(Page.ResolveUrl("~/Facility/Summary.aspx?airs=" & currentAirs.ShortString))

        ' Current user
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
        End If

        If Not IsPostBack Then
            LoadFacilityLocation()
            LoadFacilityHeaderData()
            LoadStateContactInformation()

            Title = "GECO Facility Summary - " & GetFacilityNameAndCity(currentAirs)
        End If
    End Sub

#End Region

#Region " Load data "

    Private Sub LoadFacilityLocation()
        Try
            Const query As String = "Select * " &
                                    " FROM VW_APBFacilityLocation " &
                                    " where strAIRSNumber = @airs "

            Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then
                currentFacility = GetNullableString(dr.Item("STRFACILITYNAME")) & ", " & GetNullableString(dr.Item("STRFACILITYCITY"))

                Dim street = GetNullableString(dr.Item("strFacilityStreet1"))
                lblAddress.Text = street
                Dim city = GetNullableString(dr.Item("strFacilityCity"))
                Dim state = GetNullableString(dr.Item("strFacilityState"))
                Dim zip = Address.FormatPostalCode(GetNullableString(dr.Item("strFacilityZipCode")))
                lblCityStateZip.Text = city & ", " & state & " " & zip
                Dim latitude = GetNullable(Of Decimal?)(dr("numFacilityLatitude"))
                Dim longitude = GetNullable(Of Decimal?)(dr("numFacilityLongitude"))
                lblLatitude.Text = latitude.ToString()
                lblLongitude.Text = longitude.ToString()
                lblCounty.Text = GetNullableString(dr.Item("strCountyName"))
                lblDistrict.Text = GetNullableString(dr.Item("strDistrictName"))

                If Convert.IsDBNull(dr.Item("strDistrictResponsible")) OrElse
                    dr.Item("strDistrictResponsible").ToString <> "True" Then
                    hlDistrict.Visible = False
                Else
                    hlDistrict.Visible = True
                End If

                Const size = "400x300"
                Const zoom = 14

                If latitude Is Nothing OrElse longitude Is Nothing Then
                    imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(street, city, size, zoom, GoogleMapType.roadmap)
                    lnkGoogleMap.NavigateUrl = GetMapLinkUrl(street, city)
                Else
                    Dim coords As New Coordinate(latitude.Value, longitude.Value)
                    imgGoogleStaticMap.ImageUrl = GetStaticMapUrl(coords, size, zoom, GoogleMapType.roadmap)
                    lnkGoogleMap.NavigateUrl = GetMapLinkUrl(coords)
                End If
            End If

#Disable Warning S108 ' Nested blocks of code should not be left empty
        Catch exThreadAbort As Threading.ThreadAbortException
#Enable Warning S108 ' Nested blocks of code should not be left empty
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub LoadFacilityHeaderData()
        Dim query = "Select * " &
        " FROM VW_APBFacilityHeader " &
        " where strAIRSNumber = @airs "

        Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

        Dim dr As DataRow = DB.GetDataRow(query, param)

        If dr IsNot Nothing Then

            Select Case GetNullableString(dr.Item("strOperationalstatus"))
                Case "O"
                    lblOpStatus.Text = "Operational"
                Case "P"
                    lblOpStatus.Text = "Planned"
                Case "C"
                    lblOpStatus.Text = "Under Construction"
                Case "T"
                    lblOpStatus.Text = "Temporarily Closed"
                Case "X"
                    lblOpStatus.Text = "Closed/Dismantled"
                Case "I"
                    lblOpStatus.Text = "Seasonal Operation"
                Case Else
                    lblOpStatus.Text = "Unknown (fix needed)"
            End Select

            lblClassification.Text = GetNullableString(dr.Item("strClass"))

            Dim startupDate As Date? = GetNullableDateTime(dr.Item("datStartupDate"))
            lblStartUp.Text = If(startupDate.HasValue, startupDate.Value.ToShortDateString, String.Empty)

            Dim shutdownDate As Date? = GetNullableDateTime(dr.Item("datShutDownDate"))
            lblClosed.Text = If(shutdownDate.HasValue, shutdownDate.Value.ToShortDateString, String.Empty)

            lblCMSStatus.Text = GetNullableString(dr.Item("strCMSMember"))

            lblSICCode.Text = GetNullableString(dr.Item("strSICCode"))

            AddAirProgramCodes(GetNullableString(dr.Item("strAIRProgramcodes")))
        End If
    End Sub

    Protected Sub AddAirProgramCodes(AirProgramCode As String)
        If String.IsNullOrEmpty(AirProgramCode) Then
            Return
        End If

        Dim codes As New List(Of String)

        If AirProgramCode.Substring(0, 1) = "1" Then
            codes.Add("SIP")
        End If
        If AirProgramCode.Substring(4, 1) = "1" Then
            codes.Add("PSD")
        End If
        If AirProgramCode.Substring(5, 1) = "1" Then
            codes.Add("NSR")
        End If
        If AirProgramCode.Substring(6, 1) = "1" Then
            codes.Add("NESHAP")
        End If
        If AirProgramCode.Substring(7, 1) = "1" Then
            codes.Add("NSPS")
        End If
        If AirProgramCode.Substring(11, 1) = "1" Then
            codes.Add("MACT")
        End If
        If AirProgramCode.Substring(12, 1) = "1" Then
            codes.Add("Title V")
        End If
        If AirProgramCode.Substring(9, 1) = "1" Then
            codes.Add("Acid Precipitation")
        End If

        lblAirProgramCodes.Text = ConcatNonEmptyStrings(", ", codes.ToArray)
    End Sub

    Protected Sub LoadStateContactInformation()
        Dim param As New SqlParameter("@AirsNumber", currentAirs.DbFormattedString)
        gvStateContacts.DataSource = DB.SPGetDataTable("iaip_facility.GetContactsStaff", param)
        gvStateContacts.DataBind()
    End Sub



#End Region

End Class
