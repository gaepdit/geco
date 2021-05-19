Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module eis_reportingperiod

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

        Dim params As SqlParameter() = {
            New SqlParameter("@UpdateUser", uuser),
            New SqlParameter("@fsid", fsid.ShortString),
            New SqlParameter("@eiyr", eiyr)
        }

        DB.RunCommand(query, params)
    End Sub

    Public Function GetEiThresholds(year As Integer, naa As Boolean) As DataTable
        Dim query As String = "select STRPOLLUTANT as [Pollutant],
                   IIF(@naa = 1, NUMTHRESHOLDNAA, NUMTHRESHOLD) as [Threshold]
            from EITHRESHOLDS t
                inner join EITHRESHOLDYEARS y
                on y.STREITYPE = t.STRTYPE
            where STRYEAR = @year
              and NUMTHRESHOLD is not null
            order by STRYEAR"

        Dim params As SqlParameter() = {
            New SqlParameter("@year", year),
            New SqlParameter("@naa", naa)
        }

        Return DB.GetDataTable(query, params)
    End Function

End Module
