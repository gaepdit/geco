Imports System.Data.SqlClient
Imports GECO.GecoModels

Namespace DAL
    Public Module IssuedAirPermits

        Public Function GetPermits(airsNumber As ApbFacilityId) As DataTable
            Dim spName As String = "iaip_facility.Permits"
            Return DB.SPGetDataTable(spName, New SqlParameter("@AirsNumber", airsNumber.DbFormattedString))
        End Function

    End Module
End Namespace