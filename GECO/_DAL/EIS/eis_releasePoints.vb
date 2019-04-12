Imports System.Data.SqlClient

Public Module eis_releasePoints

    Public Function ReleasePointExists(fsid As String, rpid As String) As Boolean
        Dim query As String = "select convert(bit, count(*)) " &
            " From EIS_RELEASEPOINT " &
            " where FACILITYSITEID = @fsid " &
            " and RELEASEPOINTID = @rpid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@rpid", rpid)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Public Sub InsertReleasePoint(fsId As String, rpId As String, rpDescription As String, rpType As String)

        'Code to insert a new release point into EIS_RELEASEPOINT and EIS_RPGEOCOORDINATES
        'Reminder: insert only FacilitySiteID, ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Dim queryList As List(Of String) = New List(Of String) From {
            "Insert into EIS_RELEASEPOINT (" &
                " FacilitySiteID, " &
                " RELEASEPOINTID, " &
                " STRRPDESCRIPTION, " &
                " strrptypecode, " &
                " strrpstatuscode, " &
                " strEISSubmit, " &
                " Active, " &
                " UpdateUser, " &
                " UpdateDateTime, " &
                " CreateDateTime " &
                " ) Values (" &
                " @facilitySiteID, " &
                " @stackId, " &
                " @stackDescription, " &
                " @rpType, " &
                " 'OP', " &
                " '0', " &
                " '1', " &
                " @UpdateUser, " &
                " getdate(), " &
                " getdate()) ",
            "Insert into EIS_RPGEOCOORDINATES ( " &
                " FacilitySiteID, " &
                " RELEASEPOINTID, " &
                " Active, " &
                " UpdateUser, " &
                " UpdateDateTime, " &
                " CreateDateTime " &
                " ) Values (" &
                " @facilitySiteID, " &
                " @stackId, " &
                " '1', " &
                " @UpdateUser, " &
                " getdate(), " &
                " getdate()) "
        }

        Dim params As SqlParameter() = {
            New SqlParameter("@facilitySiteID", fsId),
            New SqlParameter("@stackId", Left(rpId, 6)),
            New SqlParameter("@stackDescription", Left(rpDescription, 100)),
            New SqlParameter("@rpType", rpType),
            New SqlParameter("@UpdateUser", UpdateUser)
        }

        Dim paramsList As List(Of SqlParameter()) = New List(Of SqlParameter()) From {params, params}

        DB.RunCommand(queryList, paramsList)

    End Sub

    Public Function GetRPApportionmentTotal(ByVal fsid As String, ByVal euid As String, ByVal prid As String) As Integer
        Dim query As String = "select sum(intAveragePercentEmissions) As RPApportionmentTotal FROM eis_RPApportionment " &
            "where " &
            "FacilitySiteID = @fsid and " &
            "EmissionsUnitID = @euid and " &
            "ProcessID = @prid and " &
            "Active = '1'"

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid)
        }

        Return DB.GetInteger(query, params)
    End Function

End Module