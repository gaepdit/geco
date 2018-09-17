Imports Reimers.Core.Maps
Imports Reimers.Google.Map

Partial Class FacilityMap
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        GMap.ApiKey = ConfigurationManager.AppSettings("GoogleMapsAPIKey")
        Dim address As String = Request.QueryString("address")
        Dim facilityname = Request.QueryString("name")
        Dim citystatezip = Request.QueryString("city")
        Dim latitude = CDbl(Request.QueryString("lat"))
        Dim longitude = CDbl(Request.QueryString("lon"))

        If Not Page.IsPostBack Then
            If latitude <> 0 And longitude <> 0 Then
                GMap.Center = LatLng.Create(latitude, longitude)
            Else
                Dim geocoder = New Geocoding.GoogleGeocoder()
                Dim georesult = geocoder.Geocode(address & ", " & citystatezip)
                If georesult.Status = GeocodeStatus.OK Then
                    GMap.Center = georesult.Locations.FirstOrDefault.Point.ToLatLng()
                    latitude = GMap.Center.Latitude
                    longitude = GMap.Center.Longitude
                End If
            End If
            GMap.MapControls.Add(New Controls.ScaleControl())
            GMap.MapControls.Add(New Controls.ZoomControl())
            GMap.MapControls.Add(New Controls.MapTypeControl())
            GMap.Zoom = 15
            Dim infoWindow As New InfoWindow() With {
                .Content = facilityname,
                .Position = LatLng.Create(latitude, longitude)
            }
            GMap.Overlays.Add(infoWindow)

            ' Sample map:
            ' /FacilityMap.aspx?name=Douglas Asphalt Company&address=US Hwy 341&city=Surrency, GA 31563&lat=31.7257&lon=-82.198120
        End If
    End Sub

End Class