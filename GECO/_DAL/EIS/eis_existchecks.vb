Imports System.Data
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

    Function CheckProcessExistAny(ByVal fsid As String, ByVal euid As String) As Boolean
        'This routine checks to see if ANY processes exists for the emission unit sent to the routine
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM EIS_Process " &
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

    Function CheckProcessExist(ByVal fsid As String, ByVal euid As String, ByVal epid As String) As UnitActiveStatus
        'Returns a Value of (exists and not active), (exists and active), or (does not exist)
        Dim query As String = "Select Active " &
            " FROM EIS_Process " &
            " where FacilitySiteID = @fsid " &
            " and ProcessID = @epid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@epid", epid),
            New SqlParameter("@euid", euid)
        }

        Dim result As String = DB.GetString(query, params)

        If String.IsNullOrEmpty(result) Then
            Return UnitActiveStatus.DoesNotExist
        Else
            Return Convert.ToInt32(result)
        End If
    End Function

    Function CheckReleasePointIDexist(ByVal fsid As String, ByVal rpid As String) As String
        'Returns a Value of 0(exists and not active), 1(exists and active), or n (does not exist)
        Dim query As String = "Select Active " &
            " FROM EIS_RELEASEPOINT " &
            " where FacilitySiteID = @fsid " &
            " and RELEASEPOINTID = @rpid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@rpid", rpid)
        }

        Dim result As String = DB.GetString(query, params)

        If String.IsNullOrEmpty(result) Then
            Return "n"
        Else
            Return result
        End If
    End Function

    Function CheckRPIDExist_Dup(ByVal fsid As String, ByVal rpid As String) As String
        'Checks if either a stack ID or Fugitive ID exists
        'Duplication not allowed if either exists
        Dim query As String = "Select Active, strRPTypeCode " &
            " FROM EIS_RELEASEPOINT " &
            " where FacilitySiteID = @fsid " &
            " and RELEASEPOINTID = @rpid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@rpid", rpid)
        }

        Dim dr As DataRow = DB.GetDataRow(query, params)

        If dr IsNot Nothing Then
            If dr("strRPTypeCode") = "1" Then
                If dr("Active") = "0" Then
                    Return "DFUG" 'Deleted fugitive RP ID
                Else
                    Return "AFUG" 'Active fugitive RP ID
                End If
            Else
                If dr("Active") = "0" Then
                    Return "DSTK" 'Deleted stack RP ID
                Else
                    Return "ASTK" 'Active stack RP ID
                End If
            End If
        Else
            Return "DNE" 'RP ID does not exist; duplication allowed
        End If
    End Function

    Function CheckEUIDExist(ByVal fsid As String, ByVal euid As String) As UnitActiveStatus
        'Returns a Value of (exists and not active), (exists and active), or (does not exist)
        Dim query As String = "Select Active " &
            " FROM EIS_EmissionsUnit " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Dim result As String = DB.GetString(query, params)

        If String.IsNullOrEmpty(result) Then
            Return UnitActiveStatus.DoesNotExist
        Else
            Return Convert.ToInt32(result)
        End If
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

    Function CheckRPApportionment(ByVal fsid As String, ByVal rpid As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM EIS_RPAPPORTIONMENT " &
            " where FacilitySiteID = @fsid " &
            " and RELEASEPOINTID = @rpid " &
            " and Active = '1' "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@rpid", rpid)
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

    Function CheckProcessReportingPeriod(ByVal fsid As String, ByVal euid As String, ByVal epid As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
            " FROM eis_processreportingperiod " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid " &
            " and Active = '1' "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid)
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

    Function CheckSummerDayExist(ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal pcode As String, ByVal eiyr As Integer) As String
        'This routine checks to see if summer day emissions of NOx or VOC exists and returns a code to indicate if VOC, NOX or N for none
        Dim query As String = "Select PollutantCode " &
            " FROM vw_eis_RPEmissions " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @prid " &
            " and PollutantCode = @pcode " &
            " and intInventoryYear = @eiyr " &
            " and RptPeriodTypeCode = 'O3D'"

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid),
            New SqlParameter("@pcode", pcode),
            New SqlParameter("@eiyr", eiyr)
        }

        Dim result As String = DB.GetString(query, params)

        If String.IsNullOrEmpty(result) Then
            Return "N"
        Else
            Return result
        End If
    End Function

    Function CountSummerDayPollutants(ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal eiyr As Integer) As Integer
        'NOTE: this check assumes a record exists due to being added when the RP is added new.
        Dim RptPeriodTypeCode As String = "O3D"

        Dim query As String = "Select Count(EmissionsUnitID) as SummerDayPollutantCount " &
            " FROM eis_ReportingPeriodEmissions " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @prid " &
            " and intInventoryYear = @eiyr " &
            " and RptPeriodTypeCode = @RptPeriodTypeCode "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid),
            New SqlParameter("@eiyr", eiyr),
            New SqlParameter("@RptPeriodTypeCode", RptPeriodTypeCode)
        }

        Return DB.GetInteger(query, params)
    End Function

End Module