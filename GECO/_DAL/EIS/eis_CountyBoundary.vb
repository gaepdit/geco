Imports System.Data.SqlClient

Public Class CountyBoundary
    Public Property MinLat As Decimal
    Public Property MaxLat As Decimal
    Public Property MinLon As Decimal
    Public Property MaxLon As Decimal
End Class

Public Module eis_CountyBoundary

    Public Function GetCountyBoundary(countyCode As String) As CountyBoundary
        Dim countyMinMax As New CountyBoundary With {
            .MaxLat = 35.200028D,
            .MaxLon = -85.60889D,
            .MinLat = 30.35944D,
            .MinLon = -80.84417D
        }

        Dim query As String = "select NUMMINIMUMLONGITUDEDECIMAL, NUMMAXIMUMLONGITUDEDECIMAL,
                   NUMMINIMUMLATITUDEDECIMAL, NUMMAXIMUMLATITUDEDECIMAL
            from EISLK_COUNTYLATLON
            where STRCOUNTYCODE = @CountyCode
              and ACTIVE = '1'"

        Dim param As New SqlParameter("@CountyCode", countyCode)

        Dim dr As DataRow = DB.GetDataRow(query, param)

        If dr IsNot Nothing Then
            countyMinMax.MinLat = Math.Min(CDec(dr.Item("NUMMINIMUMLATITUDEDECIMAL")), CDec(dr.Item("NUMMAXIMUMLATITUDEDECIMAL")))
            countyMinMax.MaxLon = Math.Max(CDec(dr.Item("NUMMINIMUMLATITUDEDECIMAL")), CDec(dr.Item("NUMMAXIMUMLATITUDEDECIMAL")))
            countyMinMax.MinLon = Math.Min(CDec(dr.Item("NUMMINIMUMLONGITUDEDECIMAL")), CDec(dr.Item("NUMMAXIMUMLONGITUDEDECIMAL")))
            countyMinMax.MaxLon = Math.Max(CDec(dr.Item("NUMMINIMUMLONGITUDEDECIMAL")), CDec(dr.Item("NUMMAXIMUMLONGITUDEDECIMAL")))
        End If

        Return countyMinMax
    End Function

    Public Function GetCountyName(countyCode As String) As String
        Dim query As String = "select STRCOUNTYNAME
            from EISLK_COUNTYLATLON
            where STRCOUNTYCODE = @CountyCode"

        Return DB.GetString(query, New SqlParameter("@CountyCode", countyCode))
    End Function

End Module
