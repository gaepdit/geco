Imports Microsoft.Data.SqlClient
Imports GECO.GecoModels

Namespace DAL.EIS
    Module EisFacility

        Public Function GetEisFacilityDetails(airs As ApbFacilityId) As DataRow
            NotNull(airs, NameOf(airs))

            Dim query = "select *
            FROM VW_EIS_FACILITY
            where FACILITYSITEID = @FacilitySiteID"

            Dim param As New SqlParameter("@FacilitySiteID", airs.ShortString)

            Return DB.GetDataRow(query, param)
        End Function

    End Module
End Namespace
