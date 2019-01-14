Imports System.Data.SqlClient
Imports GECO.GecoModels

Namespace DAL
    Public Module FacilityFinances

        Public Function FacilityCredits(facilityId As ApbFacilityId) As Decimal
            Return DB.GetSingleValue(Of Decimal)("select fees.FacilityCredits(@FacilityId)", New SqlParameter("@FacilityId", facilityId.DbFormattedString))
        End Function

    End Module
End Namespace