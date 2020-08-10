Imports System.Data.SqlClient

Public Module Events

    Public Function EventExists(eventId As Integer) As Boolean
        Dim query = "select * from dbo.RES_EVENT where NUMRES_EVENTID = @eventId and ACTIVE = '1' "
        Dim param As New SqlParameter("@eventId", eventId)
        Return DB.GetBoolean(query, param)
    End Function

    Public Function GetActiveEvents() As DataTable
        Dim query = "SELECT NUMRES_EVENTID, STRTITLE, STRDESCRIPTION, DATSTARTDATE, DATENDDATE, STREVENTSTARTTIME, STREVENTENDTIME, STRVENUE,
            STRADDRESS, STRCITY, STRSTATE, NUMZIPCODE
            FROM dbo.RES_EVENT WHERE ACTIVE = '1' and NUMEVENTSTATUSCODE = 2 AND DATSTARTDATE >= dateadd(week, -1, getdate())
            ORDER BY DATSTARTDATE DESC, NUMRES_EVENTID"
        Return DB.GetDataTable(query)
    End Function

    Public Function GetEventDetails(eventId As Integer) As DataRow
        Dim query = "SELECT (SELECT count(*)
            FROM dbo.RES_REGISTRATION
            WHERE NUMRES_EVENTID = @eventId
                AND ACTIVE = '1'
                AND NUMREGISTRATIONSTATUSCODE = 1) AS NumConfirmed,
            (SELECT count(*)
            FROM dbo.RES_REGISTRATION
            WHERE NUMRES_EVENTID = @eventId
                AND ACTIVE = '1'
                AND NUMREGISTRATIONSTATUSCODE = 2) AS NumWaitingList,
           convert(date, r.DATSTARTDATE)         as DATSTARTDATE,
           convert(date, r.DATENDDATE)           as DATENDDATE,
           r.NUMEVENTSTATUSCODE, r.STRTITLE, r.STRDESCRIPTION, r.STRVENUE, r.NUMCAPACITY, r.STRNOTES, r.STRPASSCODE,
           r.STRADDRESS, r.STRCITY, r.STRSTATE, r.NUMZIPCODE, r.NUMWEBPHONENUMBER, r.STREVENTSTARTTIME, r.STREVENTENDTIME,
           r.STRWEBURL, p.STRFIRSTNAME, p.STRLASTNAME, p.STREMAILADDRESS
        FROM dbo.RES_EVENT r
            INNER JOIN dbo.EPDUSERPROFILES p
            ON r.STRUSERGCODE = p.NUMUSERID
        WHERE r.ACTIVE = '1'
          AND convert(int, r.NUMRES_EVENTID) = @eventId"

        Dim param As New SqlParameter("@eventId", eventId)

        Return DB.GetDataRow(query, param)
    End Function

    Public Function GetEventAvailability(eventId As Integer) As DataRow
        ' Returns Total, Confirmed, WaitingList
        Dim query = "SELECT NUMCAPACITY As Total,
            (SELECT count(*)
            FROM dbo.RES_REGISTRATION
            WHERE NUMRES_EVENTID = @eventId
                AND ACTIVE = '1'
                AND NUMREGISTRATIONSTATUSCODE = 1) AS Confirmed,
            (SELECT count(*)
            FROM dbo.RES_REGISTRATION
            WHERE NUMRES_EVENTID = @eventId
                AND ACTIVE = '1'
                AND NUMREGISTRATIONSTATUSCODE = 2) AS WaitingList
            FROM dbo.RES_EVENT
            WHERE ACTIVE = '1'
              AND NUMRES_EVENTID = @eventId "

        Dim param As New SqlParameter("@eventId", eventId)

        Return DB.GetDataRow(query, param)
    End Function

    Public Function GetRegistrationStatus(eventId As Integer, userId As Integer) As DataRow
        Dim query = "SELECT convert(date, DATREGISTRATIONDATETIME) as RegistrationDate,
               NUMREGISTRATIONSTATUSCODE as StatusCode,
               STRCONFIRMATIONNUMBER as ConfirmationCode,
               STRCOMMENTS as Comments
        FROM dbo.RES_REGISTRATION
        WHERE NUMGECOUSERID = @userId
          AND NUMRES_EVENTID = @eventId
          AND NUMREGISTRATIONSTATUSCODE <> 3 "

        Dim params As SqlParameter() = {
            New SqlParameter("@userId", userId),
            New SqlParameter("@eventId", eventId)
        }

        Return DB.GetDataRow(query, params)
    End Function

    Public Function UserIsRegisteredForEvent(eventId As Integer, userId As Integer) As Boolean
        Dim query = "SELECT convert(bit, count(*))
        FROM dbo.RES_REGISTRATION
        WHERE NUMGECOUSERID = @userId
          AND NUMRES_EVENTID = @eventId
          AND NUMREGISTRATIONSTATUSCODE <> 3 "

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
