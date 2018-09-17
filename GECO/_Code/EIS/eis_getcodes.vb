Imports System.Data
Imports System.Data.SqlClient

Public Structure MinMaxLatLon
    Public MinLat As Decimal
    Public MaxLat As Decimal
    Public MinLon As Decimal
    Public MaxLon As Decimal
End Structure

Public Structure MinMaxFlowRate
    Public MinFlowRate As Decimal
    Public MaxFlowRate As Decimal
End Structure

Public Module eis_getcodes

    Function GetCountyLatLong(ByVal CountyCode As String) As MinMaxLatLon
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

    Function GetRPMinMaxFlowRate(ByVal diameter As Decimal, ByVal velocity As Decimal) As MinMaxFlowRate
        Return New MinMaxFlowRate With {
            .MaxFlowRate = 1.0495 * Math.PI * (diameter ^ 2) / 4 * velocity,
            .MinFlowRate = 0.955 * Math.PI * (diameter ^ 2) / 4 * velocity
        }
    End Function

    Function GetFacilityStatusCode_Facility(ByVal fsid As String) As String
        Dim query As String = "Select strFacilitySiteStatusCode " &
            " from eis_FacilitySite " &
            " Where FacilitySiteID = @fsid "

        Dim param As New SqlParameter("@fsid", fsid)

        Return DB.GetString(query, param)
    End Function

End Module