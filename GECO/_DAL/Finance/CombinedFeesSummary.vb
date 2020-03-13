Imports System.Data.SqlClient
Imports GECO.GecoModels

Namespace DAL
    Public Module CombinedFeesSummary

        Public Function GetCombinedFeesSummary(facilityId As ApbFacilityId) As DataSet
            NotNull(facilityId, NameOf(facilityId))

            Dim ds As DataSet = DB.SPGetDataSet("iaip_facility.GetCombinedFeesSummary", New SqlParameter("@FacilityId", facilityId.DbFormattedString))

            If ds Is Nothing OrElse ds.Tables.Count <> 2 Then
                Return Nothing
            End If

            ds.Tables(0).TableName = "AnnualFees"
            ds.Tables(1).TableName = "ApplicationFees"

            Return ds
        End Function

        Public Function GetCombinedFeesInvoices(facilityId As ApbFacilityId) As DataSet
            NotNull(facilityId, NameOf(facilityId))

            Dim ds As DataSet = DB.SPGetDataSet("iaip_facility.GetCombinedFeesInvoices", New SqlParameter("@FacilityId", facilityId.DbFormattedString))

            If ds Is Nothing OrElse ds.Tables.Count <> 2 Then
                Return Nothing
            End If

            ds.Tables(0).TableName = "AnnualFees"
            ds.Tables(1).TableName = "ApplicationFees"

            Return ds
        End Function

        Public Function GetCombinedFeesDeposits(facilityId As ApbFacilityId) As DataSet
            NotNull(facilityId, NameOf(facilityId))

            Dim ds As DataSet = DB.SPGetDataSet("iaip_facility.GetCombinedFeesDeposits", New SqlParameter("@FacilityId", facilityId.DbFormattedString))

            If ds Is Nothing OrElse ds.Tables.Count <> 3 Then
                Return Nothing
            End If

            ds.Tables(0).TableName = "AnnualFeesTransactions"
            ds.Tables(1).TableName = "ApplicationFeesDeposits"
            ds.Tables(2).TableName = "ApplicationFeesRefunds"

            Return ds
        End Function

    End Module
End Namespace