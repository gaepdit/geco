Imports System.Net

Namespace GoogleGeocoder

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

    End Class

    Public Module Geocode
#Disable Warning S1075 ' URIs should not be hardcoded
        Private Const googleUri As String = "https://maps.google.com/maps/geo?q="
#Enable Warning S1075 ' URIs should not be hardcoded
        Private Const outputType As String = "csv"

        Private Function GetGeocodeUri(ByVal address As String) As Uri
            Dim googleKey As String = ConfigurationManager.AppSettings("MapKey").ToString
            address = HttpUtility.UrlEncode(address)
            Return New Uri($"{googleUri}{address}&output={outputType}&key={googleKey}")
        End Function

        Public Function GetCoordinates(ByVal address As String) As Coordinate
            Using client As WebClient = New WebClient
                Dim uri As Uri = GetGeocodeUri(address)
                Dim geocodeInfo As String() = client.DownloadString(uri).Split(","c)
                Return New Coordinate(Convert.ToDecimal(geocodeInfo(2)), Convert.ToDecimal(geocodeInfo(3)))
            End Using
        End Function
    End Module

End Namespace