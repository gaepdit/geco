Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module eis_reportingperiod

    Public Function GetEIType(eiyear As String) As String
        Dim query As String = "Select strEIType " &
            " FROM eiThresholdYears " &
            " Where strYear = @eiyear "

        Dim param As New SqlParameter("@eiyear", eiyear)

        Return DB.GetString(query, param)
    End Function

    Public Sub SaveEisOptOut(
            fsid As ApbFacilityId,
            optOut As Boolean,
            uuser As String,
            eiyr As String,
            Optional ooreason As String = Nothing,
            Optional colocated As Boolean? = Nothing,
            Optional colocation As String = Nothing)

        NotNull(fsid, NameOf(fsid))

        If Not optOut Then
            ' Following only needed for opt out = true
            ooreason = Nothing
            colocated = Nothing
            colocation = Nothing
        End If

        Dim query As String

        Dim params As SqlParameter() = {
            New SqlParameter("@opt", If(optOut, "1", "0")),
            New SqlParameter("@eisAccessCode", "2"), ' FI and EI access allowed, both no edit
            New SqlParameter("@eisStatusCode", "3"), ' Submitted
            New SqlParameter("@ooreason", ooreason),
            New SqlParameter("@uuser", uuser),
            New SqlParameter("@fsid", fsid.ShortString),
            New SqlParameter("@eiyr", eiyr),
            New SqlParameter("@colocated", colocated),
            New SqlParameter("@colocation", colocation)
        }

        query = "select datInitialFinalize " &
            " FROM eis_Admin " &
            " where FacilitySiteID = @fsid " &
            " and InventoryYear = @eiyr " &
            " and datInitialFinalize is not null "

        If DB.ValueExists(query, params) Then
            ' Don't update datInitialFinalize if it already exists
            query = "update eis_Admin set " &
                " strOptOut = @opt, " &
                " eisAccessCode = @eisAccessCode, " &
                " eisStatusCode = @eisStatusCode, " &
                " strOptOutReason = @ooreason, " &
                " strConfirmationNumber = Next Value for EIS_SEQ_ConfNum, " &
                " datFinalize = getdate(), " &
                " IsColocated = @colocated, " &
                " ColocatedWith = @colocation, " &
                " UpdateUser = @uuser, " &
                " UpdateDateTime = getdate() " &
                " where FacilitySiteID = @fsid " &
                " and InventoryYear = @eiyr "
        Else
            ' Update datInitialFinalize if null
            query = "update eis_Admin set " &
                " strOptOut = @opt, " &
                " eisAccessCode = @eisAccessCode, " &
                " eisStatusCode = @eisStatusCode, " &
                " datEISStatus = getdate(), " &
                " strOptOutReason = @ooreason, " &
                " strConfirmationNumber = Next Value for EIS_SEQ_ConfNum, " &
                " datInitialFinalize = getdate(), " &
                " datFinalize = getdate(), " &
                " IsColocated = @colocated, " &
                " ColocatedWith = @colocation, " &
                " UpdateUser = @uuser, " &
                " UpdateDateTime = getdate() " &
                " where FacilitySiteID = @fsid " &
                " and InventoryYear = @eiyr "
        End If

        DB.RunCommand(query, params)

        If colocated AndAlso Not String.IsNullOrWhiteSpace(colocation) Then
            'Send email to APB
            Dim airs As String = New GecoModels.ApbFacilityId(fsid).FormattedString
            Dim facilityName As String = GetFacilityName(fsid)
            Dim reason As String = DecodeOptOutReason(ooreason)

            Dim plainBody As String = "The following facility has opted out of the Emissions Inventory for " & eiyr &
                " and has provided co-location information." & vbNewLine &
                vbNewLine &
                "Facility: " & airs & ", " & facilityName & vbNewLine &
                "Opt-out reason: " & reason & vbNewLine &
                "Co-location info: " & vbNewLine &
                colocation & vbNewLine

            Dim htmlBody As String = "<p>The following facility has opted out of the Emissions Inventory for " & eiyr &
                " and has provided co-location information.</p>" &
                "<p><b>Facility</b>: " & airs & ", " & facilityName & "<br />" &
                "<b>Opt-out reason</b>: " & reason & "<br />" &
                "<b>Co-location info</b>:</p>" &
                "<blockquote><pre>" & colocation & "</pre></blockquote>"

            SendEmail(GecoContactEmail, "GECO EI - Facility opt out and co-location", plainBody, htmlBody,
                      caller:="eis_reportingperiod.SaveOption")
        End If
    End Sub

    Public Sub ResetEiStatus(fsid As ApbFacilityId, uuser As String, eiyr As Integer)
        NotNull(fsid, NameOf(fsid))

        'Facility needs to start over; make optout null
        Dim query = "Update eis_Admin set " &
            " eisStatusCode = '1', " &
            " eisAccessCode = '1', " &
            " strOptout = null, " &
            " strOptOutReason = null, " &
            " strConfirmationNumber = null, " &
            " datFinalize = null, " &
            " datEISStatus = getdate(), " &
            " IsColocated = null, " &
            " ColocatedWith = null, " &
            " UpdateUser = @UpdateUser, " &
            " UpdateDateTime = getdate() " &
            " where FacilitySiteID = @fsid and " &
            " InventoryYear = @eiyr "

        Dim params = {
            New SqlParameter("@UpdateUser", uuser),
            New SqlParameter("@fsid", fsid.ShortString),
            New SqlParameter("@eiyr", eiyr)
        }

        DB.RunCommand(query, params)
    End Sub

End Module
