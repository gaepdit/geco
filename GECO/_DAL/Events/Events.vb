Imports System.Data
Imports System.Data.SqlClient

Public Module Events

    Public Function EventExists(eventId As Integer) As Boolean
        Dim query = " select convert(bit, count(*)) " &
            " from RES_EVENT " &
            " where convert(int, NUMRES_EVENTID) = @eventId "

        Dim param As New SqlParameter("@eventId", eventId)

        Return DB.GetBoolean(query, param)
    End Function

    Public Function GetActiveEvents() As DataTable
        Dim query = " SELECT * " &
            " FROM dbo.RES_EVENT " &
            " WHERE ACTIVE = '1' " &
            "       AND DATSTARTDATE >= dateadd(m, -2, SYSDATETIME()) " &
            " ORDER BY DATSTARTDATE DESC, " &
            "     NUMRES_EVENTID "

        Return DB.GetDataTable(query)
    End Function

    Public Function GetEventDetails(eventId As Integer) As DataRow
        Dim query = " SELECT " &
            "     (SELECT count(*) " &
            "      FROM dbo.RES_REGISTRATION " &
            "      WHERE " &
            "          convert(int, NUMRES_EVENTID) = @eventId " &
            "          AND ACTIVE = '1' " &
            "          AND convert(int, NUMREGISTRATIONSTATUSCODE) = 1 " &
            "     ) AS NumConfirmed, " &
            "     (SELECT count(*) " &
            "      FROM dbo.RES_REGISTRATION " &
            "      WHERE " &
            "          convert(int, NUMRES_EVENTID) = @eventId " &
            "          AND ACTIVE = '1' " &
            "          AND convert(int, NUMREGISTRATIONSTATUSCODE) = 2 " &
            "     ) AS NumWaitingList, " &
            "     r.NUMRES_EVENTID, " &
            "     r.NUMEVENTSTATUSCODE, " &
            "     r.STRUSERGCODE, " &
            "     r.STRTITLE, " &
            "     r.STRDESCRIPTION, " &
            "     r.DATSTARTDATE, " &
            "     r.DATENDDATE, " &
            "     r.STRVENUE, " &
            "     r.NUMCAPACITY, " &
            "     r.STRNOTES, " &
            "     r.STRMULTIPLEREGISTRATIONS, " &
            "     r.ACTIVE, " &
            "     r.CREATEDATETIME, " &
            "     r.UPDATEUSER, " &
            "     r.UPDATEDATETIME, " &
            "     r.STRPASSCODE, " &
            "     r.STRADDRESS, " &
            "     r.STRCITY, " &
            "     r.STRSTATE, " &
            "     r.NUMZIPCODE, " &
            "     r.NUMAPBCONTACT, " &
            "     r.NUMWEBPHONENUMBER, " &
            "     r.STREVENTSTARTTIME, " &
            "     r.STREVENTENDTIME, " &
            "     r.STRWEBURL, " &
            "     p.STRFIRSTNAME, " &
            "     p.STRLASTNAME, " &
            "     p.STRPHONE, " &
            "     p.STREMAILADDRESS " &
            " FROM dbo.RES_EVENT r " &
            "     INNER JOIN dbo.EPDUSERPROFILES p " &
            "         ON r.STRUSERGCODE = p.NUMUSERID " &
            " WHERE r.ACTIVE = '1' " &
            "       AND convert(int, r.NUMRES_EVENTID) = @eventId "

        Dim param As New SqlParameter("@eventId", eventId)

        Return DB.GetDataRow(query, param)
    End Function

    Public Function GetEventAvailability(eventId As Integer) As DataRow
        ' Returns Total, Confirmed, WaitingList
        Dim query = " SELECT " &
            "     convert(int, NUMCAPACITY) As Total, " &
            "     (SELECT count(*) " &
            "      FROM dbo.RES_REGISTRATION " &
            "      WHERE " &
            "          convert(int, NUMRES_EVENTID) = @eventId " &
            "          AND ACTIVE = '1' " &
            "          AND convert(int, NUMREGISTRATIONSTATUSCODE) = 1 " &
            "     ) AS Confirmed, " &
            "     (SELECT count(*) " &
            "      FROM dbo.RES_REGISTRATION " &
            "      WHERE " &
            "          convert(int, NUMRES_EVENTID) = @eventId " &
            "          AND ACTIVE = '1' " &
            "          AND convert(int, NUMREGISTRATIONSTATUSCODE) = 2 " &
            "     ) AS WaitingList " &
            " FROM dbo.RES_EVENT " &
            " WHERE ACTIVE = '1' " &
            "       AND convert(int, NUMRES_EVENTID) = @eventId "

        Dim param As New SqlParameter("@eventId", eventId)

        Return DB.GetDataRow(query, param)
    End Function

    Public Function GetRegistrationStatus(eventId As Integer, userId As Integer) As DataRow
        Dim query = " SELECT " &
            "     DATREGISTRATIONDATETIME as RegistrationDate, " &
            "     convert(int, NUMREGISTRATIONSTATUSCODE) as StatusCode, " &
            "     STRCONFIRMATIONNUMBER as ConfirmationCode, " &
            "     STRCOMMENTS as Comments " &
            " FROM RES_REGISTRATION " &
            " WHERE convert(int, NUMGECOUSERID) = @userId " &
            "       AND convert(int, NUMRES_EVENTID) = @eventId " &
            "       AND convert(int, NUMREGISTRATIONSTATUSCODE) <> 3 "

        Dim params As SqlParameter() = {
            New SqlParameter("@userId", userId),
            New SqlParameter("@eventId", eventId)
        }

        Return DB.GetDataRow(query, params)
    End Function

    Public Function UserIsRegisteredForEvent(eventId As Integer, userId As Integer) As Boolean
        Dim query = " SELECT convert(bit, count(*)) " &
        " FROM RES_REGISTRATION " &
        " WHERE convert(int, NUMGECOUSERID) = @userId " &
        "       AND convert(int, NUMRES_EVENTID) = @eventId " &
        "       AND convert(int, NUMREGISTRATIONSTATUSCODE) <> 3 "

        Dim params As SqlParameter() = {
            New SqlParameter("@userId", userId),
            New SqlParameter("@eventId", eventId)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Public Function RegisterUserForEvent(userId As Integer, eventId As Integer, confirmationCode As String, comment As String, ByRef status As Integer) As DbResult
        Dim spName = "geco.RegisterForEvent"

        Dim params As SqlParameter() = {
            New SqlParameter("@UserId", userId),
            New SqlParameter("@EventId", eventId),
            New SqlParameter("@ConfirmationCode", confirmationCode),
            New SqlParameter("@Comment", comment)
        }

        Dim returnValue As Integer
        status = DB.SPGetInteger(spName, params, returnValue)

        Select Case returnValue
            Case 0
                Return DbResult.Success
            Case Else
                Return DbResult.Failure
        End Select
    End Function

    Public Function CancelEventRegistration(userId As Integer, eventId As Integer, comment As String, ByRef newConfirmedUser As Integer) As DbResult
        Dim spName = "geco.CancelEventRegistration"

        Dim params As SqlParameter() = {
            New SqlParameter("@UserId", userId),
            New SqlParameter("@EventId", eventId),
            New SqlParameter("@Comment", comment)
        }

        Dim returnValue As Integer
        newConfirmedUser = DB.SPGetInteger(spName, params, returnValue)

        Select Case returnValue
            Case 0
                Return DbResult.Success
            Case Else
                Return DbResult.Failure
        End Select
    End Function

End Module