Imports System.Data.SqlClient

Public Structure MinMaxLatLon
    Public MinLat As Decimal
    Public MaxLat As Decimal
    Public MinLon As Decimal
    Public MaxLon As Decimal
End Structure

Public Structure MinMaxValues
    Public MinValue As Decimal
    Public MaxValue As Decimal
End Structure

Public Module eis_getcodes

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

    Public Function GetRPMinMaxFlowRate(diameter As Decimal, velocity As Decimal) As MinMaxValues
        Return New MinMaxValues With {
            .MaxValue = Math.Round(1.0495 * CalculateFlowRate(diameter, velocity), 1),
            .MinValue = Math.Round(0.955 * CalculateFlowRate(diameter, velocity), 1)
        }
    End Function

    Public Function CalculateFlowRate(diameter As Decimal, velocity As Decimal) As Decimal
        Return Math.PI * (diameter ^ 2) / 4 * velocity
    End Function

    Public Function CalculateVelocity(diameter As Decimal, flowRate As Decimal) As Decimal
        Return flowRate / (Math.PI * (diameter ^ 2) / 4)
    End Function

    Public Function GetFacilityStatusCode_Facility(fsid As String) As String
        Dim query As String = "Select strFacilitySiteStatusCode " &
            " from eis_FacilitySite " &
            " Where FacilitySiteID = @fsid "

        Dim param As New SqlParameter("@fsid", fsid)

        Return DB.GetString(query, param)
    End Function

End Module