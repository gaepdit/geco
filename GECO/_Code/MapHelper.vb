Namespace MapHelper

    Public Interface ISpatialCoordinate
        Property Latitude() As Decimal
        Property Longitude() As Decimal
    End Interface

    Public Class Coordinate
        Implements ISpatialCoordinate

        Public Sub New(ByVal latitude As Decimal, ByVal longitude As Decimal)
            Me.Latitude = latitude
            Me.Longitude = longitude
        End Sub

        Public Property Latitude() As Decimal Implements ISpatialCoordinate.Latitude
        Public Property Longitude() As Decimal Implements ISpatialCoordinate.Longitude

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

        <CodeAnalysis.SuppressMessage("Design", "CA1055")>
        Public Function GetMapLinkUrl(coordinates As Coordinate) As String
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
                                               Optional mapType As GoogleMapType = GoogleMapType.hybrid
                                               ) As String
            Dim key As String = ConfigurationManager.AppSettings("GoogleMapsAPIKey")
            Dim marker As String = "color:blue|label:X|" & coordinates.ToString()

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

            Return String.Format(_googleStaticMapUri, center, zoom, size, mapType, "", key)
        End Function

    End Module
End Namespace
