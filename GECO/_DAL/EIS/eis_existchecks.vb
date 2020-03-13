Imports System.Data.SqlClient

Public Module eis_existchecks

    'Returns a Value of 0(exists and not active), 1(exists and active), or n (does not exist)
    Public Enum UnitActiveStatus
        Inactive = 0
        Active = 1
        DoesNotExist = 2
    End Enum

    Function CheckEUControlApproachAny(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM EIS_UnitControlApproach " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and Active = '1' "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Function CheckProcCtrlApproachAny(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM EIS_ProcessControlApproach " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and Active = '1' "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Function CheckProcCtrlApproachSpec(ByVal fsid As String, ByVal euid As String, ByVal prid As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM EIS_ProcessControlApproach " &
            " where FacilitySiteID = @fsid " &
            " and ProcessID = @prid " &
            " and EmissionsUnitID = @euid " &
            " and Active = '1' "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@prid", prid),
            New SqlParameter("@euid", euid)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Function CheckDeletedEUExist(ByVal fsid As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM EIS_EmissionsUnit " &
            " where FacilitySiteID = @fsid " &
            " and Active = '0' "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Function CheckDeletedRPExist(ByVal fsid As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM eis_ReleasePoint " &
            " where FacilitySiteID = @fsid " &
            " and Active = '0' "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Function CheckAnyRPApportionment(ByVal fsid As String, euid As String, prid As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM EIS_RPAPPORTIONMENT " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @prid " &
            " and Active = '1' "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Function CheckRPGCData(ByVal fsid As String, ByVal rpid As String) As Boolean
        'NOTE: this check assumes a record exists due to being added when the RP is added new.
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM eis_RPGeoCoordinates " &
            " where FacilitySiteID = @fsid " &
            " and ReleasePointID = @rpid " &
            " and " &
            " (numLatitudeMeasure is null Or numLongitudeMeasure is null Or " &
            "  intHorAccuracyMeasure is null Or strHorCollMetCode is null Or " &
            "  strHorRefDatumCode is null)"


        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@rpid", rpid)
        }

        Return DB.GetBoolean(query, params)
    End Function

End Module