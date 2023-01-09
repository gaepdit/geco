Imports System.Data.SqlClient
Imports GECO.GecoModels


Namespace DAL.EIS
    Module Facility

        Public Function GetEisFacilityDetails(airs As ApbFacilityId) As DataRow
            NotNull(airs, NameOf(airs))

            Dim query = "select strFacilitySiteName, 
                   strLocationAddressText, strSupplementalLocationText,
                   strLocalityName, strLocationAddressPostalCode, strFacilitySiteStatusDesc,
                   intFacilitySiteStatusCodeYear, strFacilitySiteDescription, strNAICSCode, strFacilitySiteComment,
                   numLatitudeMeasure, numLongitudeMeasure, 
                   HorCollMetCode, STRHORCOLLMETDesc, INTHORACCURACYMEASURE, HorRefDatumCode, STRHORREFDATUMDesc,
                   strGeographicComment, 
                   LastEPASubmitDate_MAddress,
                   UpdateUser_FacilitySite, UpdateDateTime_FacilitySite, LastEPASubmitDate_FacilitySite, UpdateUser_GeoCoord,
                   UpdateDateTime_GeoCoord, LastEPASubmitDate_GeoCoord, 
                   LastEPASubmitDate_AffIndiv
            FROM VW_EIS_FACILITY
            where FACILITYSITEID = @FacilitySiteID"

            Dim param As New SqlParameter("@FacilitySiteID", airs.ShortString)

            Return DB.GetDataRow(query, param)
        End Function

    End Module
End Namespace
