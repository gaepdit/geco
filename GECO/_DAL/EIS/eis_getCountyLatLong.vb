Imports System.Data.SqlClient

Public Class MinMaxLatLon
    Public Property MinLat As Decimal
    Public Property MaxLat As Decimal
    Public Property MinLon As Decimal
    Public Property MaxLon As Decimal
End Class

Public Module eis_getCountyLatLong

    Public Function GetCountyLatLong(CountyCode As String) As MinMaxLatLon
        Dim countyMinMax As New MinMaxLatLon With {
            .MaxLat = 35.200028D,
            .MaxLon = -85.60889D,
            .MinLat = 30.35944D,
            .MinLon = -80.84417D
        }

        Dim query As String = "Select NUMMINIMUMLONGITUDEDECIMAL, NUMMAXIMUMLONGITUDEDECIMAL, " &
            " NUMMINIMUMLATITUDEDECIMAL, NUMMAXIMUMLATITUDEDECIMAL " &
            " FROM eislk_countylatlon " &
            " Where strCountyCode = @CountyCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@CountyCode", CountyCode)

        Dim dr As DataRow = DB.GetDataRow(query, param)

        If dr IsNot Nothing AndAlso
            Not IsDBNull(dr("NUMMINIMUMLATITUDEDECIMAL")) AndAlso
            Not IsDBNull(dr("NUMMAXIMUMLATITUDEDECIMAL")) AndAlso
            Not IsDBNull(dr("NUMMINIMUMLONGITUDEDECIMAL")) AndAlso
            Not IsDBNull(dr("NUMMAXIMUMLONGITUDEDECIMAL")) Then

            countyMinMax.MinLat = Math.Min(CDec(dr.Item("NUMMINIMUMLATITUDEDECIMAL")), CDec(dr.Item("NUMMAXIMUMLATITUDEDECIMAL")))
            countyMinMax.MaxLon = Math.Max(CDec(dr.Item("NUMMINIMUMLATITUDEDECIMAL")), CDec(dr.Item("NUMMAXIMUMLATITUDEDECIMAL")))
            countyMinMax.MinLon = Math.Min(CDec(dr.Item("NUMMINIMUMLONGITUDEDECIMAL")), CDec(dr.Item("NUMMAXIMUMLONGITUDEDECIMAL")))
            countyMinMax.MaxLon = Math.Max(CDec(dr.Item("NUMMINIMUMLONGITUDEDECIMAL")), CDec(dr.Item("NUMMAXIMUMLONGITUDEDECIMAL")))
        End If

        Return countyMinMax
    End Function

End Module
