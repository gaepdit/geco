Imports System.Data.SqlClient

Public Module eis_deletefunctions

    Public Function DeleteReleasePoint(ByVal fsid As String, ByVal rpid As String, ByVal UpdUser As String) As Boolean
        Dim queryList As New List(Of String)
        Dim parametersList As New List(Of SqlParameter())

        Dim params As SqlParameter() = {
            New SqlParameter("@UpdUser", UpdUser),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@rpid", rpid)
        }

        queryList.Add(
            "Update EIS_RELEASEPOINT Set " &
            " Active = '0', " &
            " UPDATEUSER = @UpdUser, " &
            " UpdateDateTime = sysdatetime() " &
            " where FacilitySiteID = @fsid " &
            " and ReleasePointID = @rpid "
        )
        parametersList.Add(params)

        queryList.Add(
            "Update EIS_RPGEOCOORDINATES Set " &
            " Active = '0', " &
            " UPDATEUSER = @UpdUser, " &
            " UpdateDateTime = sysdatetime() " &
            " where FacilitySiteID = @fsid " &
            " and ReleasePointID = @rpid "
        )
        parametersList.Add(params)

        Return DB.RunCommand(queryList, parametersList)
    End Function

    Public Function DeleteEmissionsUnit(ByVal fsid As String, ByVal euid As String, ByVal UpdUser As String) As Boolean
        Dim query As String = "Update eis_EmissionsUnit Set " &
            " Active = '0', " &
            " UpdateUser = @UpdUser, " &
            " UpdateDateTime = sysdatetime() " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@UpdUser", UpdUser),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteEUControlApproach(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Delete FROM eis_UnitControlApproach where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteEUCtrlAppMeasures(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Delete FROM eis_UnitControlMeasure where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteEUCtrlAppPollutants(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Delete FROM eis_UnitControlPollutant where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteProcessRPApp(ByVal fsid As String, ByVal euid As String, ByVal epid As String) As Boolean
        Dim query As String = "Delete FROM EIS_RPAPPORTIONMENT where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteProcessRPApp_EU(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Delete FROM EIS_RPAPPORTIONMENT where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteProcessControlApproach(ByVal fsid As String, ByVal euid As String, ByVal epid As String) As Boolean
        Dim query As String = "Delete FROM EIS_PROCESSCONTROLAPPROACH where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteProcessControlApproach_EU(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Delete FROM EIS_PROCESSCONTROLAPPROACH where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteAllProcessControlMeasures(ByVal fsid As String, ByVal euid As String, ByVal epid As String) As Boolean
        Dim query As String = "Delete FROM EIS_PROCESSCONTROLMEASURE where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteAllProcessControlMeasures_EU(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Delete FROM EIS_PROCESSCONTROLMEASURE where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteAllProcessControlPollutants(ByVal fsid As String, ByVal euid As String, ByVal epid As String) As Boolean
        Dim query As String = "Delete FROM EIS_PROCESSCONTROLPOLLUTANT where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteAllProcessControlPollutants_EU(ByVal fsid As String, ByVal euid As String) As Boolean
        Dim query As String = "Delete FROM EIS_PROCESSCONTROLPOLLUTANT where " &
            " FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteProcess(ByVal fsid As String, ByVal euid As String, ByVal epid As String, ByVal UpdUser As String) As Boolean
        Dim query As String = "update EIS_PROCESS
            set ACTIVE         = '0',
                UPDATEUSER     = @UpdUser,
                UPDATEDATETIME = sysdatetime()
            where FACILITYSITEID = @fsid
              and EMISSIONSUNITID = @euid
              and PROCESSID = @epid"

        Dim params As SqlParameter() = {
            New SqlParameter("@UpdUser", UpdUser),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function EUProcessesSetActiveZero(ByVal fsid As String, ByVal euid As String, ByVal UpdUser As String) As Boolean
        Dim query As String = "update EIS_PROCESS
            set ACTIVE         = '0',
                UPDATEUSER     = @UpdUser,
                UPDATEDATETIME = sysdatetime()
            where FACILITYSITEID = @fsid
              and EMISSIONSUNITID = @euid"

        Dim params As SqlParameter() = {
            New SqlParameter("@UpdUser", UpdUser),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.RunCommand(query, params)
    End Function

    ' Delete Reporting Period elements (Emissions, SCP, Details, Reporting Period) mainly for handling summer day
    ' (Rather than set `ACTIVE = 0`, we allow data to be deleted for current (in process) year.)

    Public Function DeleteReportingPeriodEmissions(ByVal eiyr As Integer, ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal pcode As String, ByVal rptpertypecode As String) As Boolean
        Dim query As String = "Delete FROM eis_ReportingPeriodEmissions " &
            " where intInventoryYear = @eiyr " &
            " and FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @prid " &
            " and PollutantCode = @pcode " &
            " and RPTPeriodTypeCode = @rptpertypecode "

        Dim params As SqlParameter() = {
            New SqlParameter("@eiyr", eiyr),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid),
            New SqlParameter("@pcode", pcode),
            New SqlParameter("@rptpertypecode", rptpertypecode)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteProcessOperatingDetails(ByVal eiyr As Integer, ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal rptperiodcode As String) As Boolean
        Dim query As String = "Delete FROM eis_ProcessOperatingDetails " &
            " where intInventoryYear = @eiyr " &
            " and FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @prid " &
            " and RPTPeriodTypeCode = @rptperiodcode "

        Dim params As SqlParameter() = {
            New SqlParameter("@eiyr", eiyr),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid),
            New SqlParameter("@rptperiodcode", rptperiodcode)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteProcessReportingPeriod(ByVal eiyr As Integer, ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal rptperiodcode As String) As Boolean
        Dim query As String = "Delete FROM eis_ProcessReportingPeriod " &
            " where intInventoryYear = @eiyr " &
            " and FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @prid " &
            " and RPTPeriodTypeCode = @rptperiodcode "

        Dim params As SqlParameter() = {
            New SqlParameter("@eiyr", eiyr),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid),
            New SqlParameter("@rptperiodcode", rptperiodcode)
        }

        Return DB.RunCommand(query, params)
    End Function

End Module