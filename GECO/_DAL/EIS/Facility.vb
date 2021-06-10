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

        Public Function SaveEisFacilitySiteInfo(airs As ApbFacilityId, description As String, naics As String,
                comment As String, user As GecoUser) As Boolean
            NotNull(airs, NameOf(airs))
            NotNull(user, NameOf(user))

            Dim query As String = "Update EIS_FACILITYSITE " &
               "Set STRFACILITYSITEDESCRIPTION = @facilityDescription, " &
               "STRNAICSCODE = @NAICScode, " &
               "STRFACILITYSITECOMMENT = @facilityComment, " &
               "UPDATEUSER = @UpdateUser, " &
               "UpdateDateTime = getdate() " &
               "where FACILITYSITEID = @FacilitySiteID "

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityDescription", description),
                New SqlParameter("@NAICScode", naics),
                New SqlParameter("@facilityComment", Left(comment, 400)),
                New SqlParameter("@UpdateUser", user.DbUpdateUser),
                New SqlParameter("@FacilitySiteID", airs.ShortString)
            }

            Return DB.RunCommand(query, params)
        End Function

        Public Function UpdateEisGeographicComment(airs As ApbFacilityId, comment As String, user As GecoUser) As Boolean
            NotNull(airs, NameOf(airs))
            NotNull(user, NameOf(user))

            Dim query = "Update EIS_FACILITYGEOCOORD " &
                    " Set STRGEOGRAPHICCOMMENT = @GeographicComment, " &
                    " UPDATEUSER = @UpdateUser, " &
                    " UpdateDateTime = getdate() " &
                    " where FACILITYSITEID = @FacilitySiteID "

            Dim params = {
                    New SqlParameter("@GeographicComment", Left(comment, 200)),
                    New SqlParameter("@UpdateUser", user.DbUpdateUser),
                    New SqlParameter("@FacilitySiteID", airs.ShortString)
                }

            Return DB.RunCommand(query, params)
        End Function

    End Module
End Namespace
