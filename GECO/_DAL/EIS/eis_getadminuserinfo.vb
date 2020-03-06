Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module eis_getadminuserinfo

    Public Function GetEISAdminUpdateDateTime(fsid As String, eiyr As Integer) As String
        Dim query = "select UpdateDateTime " &
            " FROM eis_Admin " &
            " where FacilitySiteID = @fsid " &
            " and InventoryYear = @eiyr "

        Dim params = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@eiyr", eiyr)
        }

        Dim result = DB.GetSingleValue(Of DateTime?)(query, params)

        If Not result.HasValue Then
            Return "Unknown"
        End If

        Return result.Value.ToShortDateString
    End Function

    Public Function GetAdminComment(fsid As String, eiyr As Integer) As String
        Dim query = "select strComment " &
            " FROM eis_Admin " &
            " where FacilitySiteID = @fsid " &
            " and InventoryYear = @eiyr "

        Dim params = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@eiyr", eiyr)
        }

        Return DB.GetString(query, params)
    End Function

    Public Function SaveAdminComment(fsid As ApbFacilityId, eiyr As Integer, aComment As String) As Boolean
        'Truncate comment if greater than 4000 characters
        Dim query = "update eis_Admin " &
            " set strComment = @aComment " &
            " where FacilitySiteID = @fsid " &
            " and InventoryYear = @eiyr "

        Dim params = {
            New SqlParameter("@aComment", Left(aComment, 4000)),
            New SqlParameter("@fsid", fsid.ShortString),
            New SqlParameter("@eiyr", eiyr)
        }

        Return DB.RunCommand(query, params)
    End Function

End Module