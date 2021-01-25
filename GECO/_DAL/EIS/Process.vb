Imports System.Data.SqlClient
Imports GECO.GecoModels.EIS

Namespace DAL.EIS
    Module Process

        Public Function SaveEisProcess(process As EisProcess) As Boolean
            NotNull(process, NameOf(process))
            NotNull(process.FacilitySiteId, NameOf(process.FacilitySiteId))

            Dim queries As New List(Of String)
            Dim params As New List(Of SqlParameter())

            ' Save FacilityStatus as OP regardless
            queries.Add("update EIS_FACILITYSITE
                set STRFACILITYSITESTATUSCODE     = 'OP',
                    INTFACILITYSITESTATUSCODEYEAR = @y,
                    UPDATEUSER                    = @u,
                    UPDATEDATETIME                = sysdatetime()
                where FACILITYSITEID = @f ")
            params.Add({
                New SqlParameter("@f", process.FacilitySiteId.ShortString),
                New SqlParameter("@y", process.InventoryYear),
                New SqlParameter("@u", process.UpdateUser)
            })

            ' Save admin comment
            queries.Add("update EIS_ADMIN
                set STRCOMMENT = @c
                where FACILITYSITEID = @f
                  and INVENTORYYEAR = @y")
            params.Add({
                New SqlParameter("@f", process.FacilitySiteId.ShortString),
                New SqlParameter("@y", process.InventoryYear),
                New SqlParameter("@c", process.AdminComment)
            })

            ' Save EIS process
            ' Access Code 2: FI and EI access allowed, both no edit
            ' Status Code 3: Submitted
            queries.Add("update EIS_ADMIN
                set STROPTOUT             = @o,
                    EISACCESSCODE         = '2',
                    EISSTATUSCODE         = '3',
                    DATEISSTATUS          = sysdatetime(),
                    STROPTOUTREASON       = @r,
                    STRCONFIRMATIONNUMBER = next value for EIS_SEQ_CONFNUM,
                    DATINITIALFINALIZE    = iif(DATINITIALFINALIZE is null, sysdatetime(), DATINITIALFINALIZE),
                    DATFINALIZE           = sysdatetime(),
                    IsColocated           = @c,
                    ColocatedWith         = @w,
                    UPDATEUSER            = @u,
                    UPDATEDATETIME        = sysdatetime()
                where FACILITYSITEID = @f
                  and INVENTORYYEAR = @y")
            params.Add({
                New SqlParameter("@f", process.FacilitySiteId.ShortString),
                New SqlParameter("@y", process.InventoryYear),
                New SqlParameter("@o", process.GetOptOutCode),
                New SqlParameter("@r", process.GetOptOutReasonCode),
                New SqlParameter("@c", process.Colocated),
                New SqlParameter("@w", process.Colocation),
                New SqlParameter("@u", process.UpdateUser)
            })

            If DB.RunCommand(queries, params) Then
                SendColocationEmail(process)
                Return True
            Else
                Return False
            End If
        End Function

        Private Sub SendColocationEmail(process As EisProcess)
            If process.Colocated AndAlso Not String.IsNullOrWhiteSpace(process.Colocation) Then
                Dim facilityName As String = GetFacilityName(process.FacilitySiteId)

                Dim plainBody As String = "The following facility has opted out of the Emissions Inventory for " &
                    process.InventoryYear & " and has provided co-location information." & vbNewLine & vbNewLine &
                    "Facility: " & process.FacilitySiteId.FormattedString & ", " & facilityName & vbNewLine &
                    "Opt-out reason: " & process.GetOptOutReason() & vbNewLine &
                    "Co-location info: " & vbNewLine &
                    process.Colocation & vbNewLine

                Dim htmlBody As String = "<p>The following facility has opted out of the Emissions Inventory for " &
                    process.InventoryYear & " and has provided co-location information.</p>" &
                    "<p><b>Facility</b>: " & process.FacilitySiteId.FormattedString & ", " & facilityName & "<br />" &
                    "<b>Opt-out reason</b>: " & process.GetOptOutReason() & "<br />" &
                    "<b>Co-location info</b>:</p>" &
                    "<blockquote><pre>" & process.Colocation & "</pre></blockquote>"

                SendEmail(GecoContactEmail, "GECO EI - Facility opt out and co-location",
                          plainBody, htmlBody,
                          caller:="DAL.EIS.Process.SendColocationEmail")
            End If
        End Sub

    End Module
End Namespace
