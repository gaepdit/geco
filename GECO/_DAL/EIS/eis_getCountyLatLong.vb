﻿Imports System.Data.SqlClient

Public Class MinMaxLatLon
    Public Property MinLat As Decimal
    Public Property MaxLat As Decimal
    Public Property MinLon As Decimal
    Public Property MaxLon As Decimal
End Class

Public Module eis_getCountyLatLong

    Public Function GetCountyLatLong(CountyCode As String) As MinMaxLatLon
        Dim countyMinMax As New MinMaxLatLon With {
            .MaxLat = 35.200028,
            .MaxLon = -85.60889,
            .MinLat = 30.35944,
            .MinLon = -80.84417
        }

        Dim query As String = "Select NUMMINIMUMLONGITUDEDECIMAL, NUMMAXIMUMLONGITUDEDECIMAL, " &
            " NUMMINIMUMLATITUDEDECIMAL, NUMMAXIMUMLATITUDEDECIMAL " &
            " FROM eislk_countylatlon " &
            " Where strCountyCode = @CountyCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@CountyCode", CountyCode)

        Dim dr As DataRow = DB.GetDataRow(query, param)

        If dr IsNot Nothing Then
            If Not IsDBNull(dr("NUMMINIMUMLATITUDEDECIMAL")) AndAlso Not IsDBNull(dr("NUMMAXIMUMLATITUDEDECIMAL")) Then
                countyMinMax.MinLat = Math.Min(dr.Item("NUMMINIMUMLATITUDEDECIMAL"), dr.Item("NUMMAXIMUMLATITUDEDECIMAL"))
                countyMinMax.MaxLon = Math.Max(dr.Item("NUMMINIMUMLATITUDEDECIMAL"), dr.Item("NUMMAXIMUMLATITUDEDECIMAL"))
            End If

            If Not IsDBNull(dr("NUMMINIMUMLONGITUDEDECIMAL")) AndAlso Not IsDBNull(dr("NUMMAXIMUMLONGITUDEDECIMAL")) Then
                countyMinMax.MinLon = Math.Min(dr.Item("NUMMINIMUMLONGITUDEDECIMAL"), dr.Item("NUMMAXIMUMLONGITUDEDECIMAL"))
                countyMinMax.MaxLon = Math.Max(dr.Item("NUMMINIMUMLONGITUDEDECIMAL"), dr.Item("NUMMAXIMUMLONGITUDEDECIMAL"))
            End If
        End If

        Return countyMinMax
    End Function

End Module