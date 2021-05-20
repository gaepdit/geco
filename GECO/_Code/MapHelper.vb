Namespace MapHelper

    Public Class Coordinate

        Public Sub New(latitude As Decimal, longitude As Decimal)
            Me.Latitude = latitude
            Me.Longitude = longitude
        End Sub

        Private Property Latitude As Decimal
        Private Property Longitude As Decimal

        Public Overrides Function ToString() As String
            Return Latitude.ToString & "," & Longitude.ToString
        End Function
    End Class

    Public Enum GoogleMapType
        roadmap
        satellite
        hybrid
        terrain
    End Enum

    Public Module GoogleMaps
#Disable Warning S1075 ' URIs should not be hardcoded
        Private Const _googleLinkUri As String = "https://maps.google.com/maps/place/{0}"
        Private Const _googleStaticMapUri As String = "https://maps.googleapis.com/maps/api/staticmap?center={0}&zoom={1}&size={2}&maptype={3}&markers={4}&key={5}"
#Enable Warning S1075 ' URIs should not be hardcoded
        Private Const _defaultSize As String = "600x200"
        Private Const _defaultZoom As Integer = 16
        Private Const _defaultMapType As GoogleMapType = GoogleMapType.hybrid

        <CodeAnalysis.SuppressMessage("Design", "CA1055")>
        Public Function GetMapLinkUrl(coordinates As Coordinate) As String
            NotNull(coordinates, NameOf(coordinates))

            Return String.Format(_googleLinkUri, coordinates.ToString())
        End Function

        <CodeAnalysis.SuppressMessage("Design", "CA1055")>
        Public Function GetMapLinkUrl(address As String, city As String) As String
            Return String.Format(_googleLinkUri, address & ", " & city & " GA")
        End Function

        <CodeAnalysis.SuppressMessage("Design", "CA1055")>
        Public Function GetStaticMapUrl(coordinates As Coordinate,
                                        Optional size As String = _defaultSize,
                                        Optional zoom As Integer = _defaultZoom,
                                        Optional mapType As GoogleMapType = _defaultMapType
                                        ) As String
            NotNull(coordinates, NameOf(coordinates))

            Dim key As String = ConfigurationManager.AppSettings("GoogleMapsAPIKey")
            Dim marker As String = "color:0x01009A|label:X|" & coordinates.ToString()

            Return String.Format(_googleStaticMapUri, coordinates.ToString(), zoom, size, mapType, marker, key)
        End Function

        <CodeAnalysis.SuppressMessage("Design", "CA1055")>
        Public Function GetStaticMapUrl(address As String,
                                        city As String,
                                        Optional size As String = _defaultSize,
                                        Optional zoom As Integer = _defaultZoom,
                                        Optional mapType As GoogleMapType = GoogleMapType.hybrid
                                        ) As String
            Dim center As String = address & "," & city & " GA"
            Dim key As String = ConfigurationManager.AppSettings("GoogleMapsAPIKey")
            Dim marker As String = "color:0x01009A|label:X|" & center

            Return String.Format(_googleStaticMapUri, center, zoom, size, mapType, marker, key)
        End Function

    End Module
End Namespace
