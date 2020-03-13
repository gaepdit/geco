Imports System.Data.SqlClient

Public Module ES_County

    Public Function GetCounty(AirsNumber As String) As String
        NotNull(AirsNumber, NameOf(AirsNumber))

        Dim CountyCode As String = ""

        If AirsNumber.Length = 12 Then
            CountyCode = Mid(AirsNumber, 5, 3)
        ElseIf AirsNumber.Length = 8 Then
            CountyCode = Mid(AirsNumber, 1, 3)
        End If

        Dim query As String = "SELECT STRCOUNTYNAME FROM EILOOKUPCOUNTYLATLON WHERE STRCOUNTYCODE = @CountyCode "

        Dim param As New SqlParameter("@CountyCode", CountyCode)

        Return DB.GetString(query, param)
    End Function

    Public Function GetLatMin(County As String) As Decimal
        If String.IsNullOrEmpty(County) Then
            Return 0
        Else
            Dim query As String = "Select nbrMinimumLatitudeDecimal FROM EILookupCountyLatLon where StrCountyName = @County "
            Dim param As New SqlParameter("@County", County)
            Return DB.GetSingleValue(Of Decimal)(query, param)
        End If
    End Function

    Public Function GetLatMax(County As String) As Decimal
        If String.IsNullOrEmpty(County) Then
            Return 0
        Else
            Dim query As String = "Select nbrMaximumLatitudeDecimal FROM EILookupCountyLatLon where StrCountyName = @County "
            Dim param As New SqlParameter("@County", County)
            Return DB.GetSingleValue(Of Decimal)(query, param)
        End If
    End Function

    Public Function GetLongMin(County As String) As Decimal
        If String.IsNullOrEmpty(County) Then
            Return 0
        Else
            Dim query As String = "Select nbrMinimumLongitudeDecimal FROM EILookupCountyLatLon where StrCountyName = @County "
            Dim param As New SqlParameter("@County", County)
            Return DB.GetSingleValue(Of Decimal)(query, param)
        End If
    End Function

    Public Function GetLongMax(County As String) As Decimal
        If String.IsNullOrEmpty(County) Then
            Return 0
        Else
            Dim query As String = "Select nbrMaximumLongitudeDecimal FROM EILookupCountyLatLon where StrCountyName = @County "
            Dim param As New SqlParameter("@County", County)
            Return DB.GetSingleValue(Of Decimal)(query, param)
        End If
    End Function

End Module