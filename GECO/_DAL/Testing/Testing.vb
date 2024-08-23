Imports Microsoft.Data.SqlClient
Imports GECO.GecoModels

Module Testing

    Public Function GetPendingTestNotifications(airsNumber As ApbFacilityId) As DataRow
        NotNull(airsNumber, NameOf(airsNumber))

        Dim query As String = " select " &
        "     count(*)        as total, " &
        "     sum(case when DATPROPOSEDSTARTDATE >= getdate() " &
        "         then 1 " &
        "         else 0 end) as pending " &
        " FROM ISMPTESTNOTIFICATION " &
        " where STRAIRSNUMBER = @AirsNumber "

        Dim param As New SqlParameter("@AirsNumber", airsNumber.DbFormattedString)

        Return DB.GetDataRow(query, param)
    End Function

End Module
