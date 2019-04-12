Imports System.Data.SqlClient
Imports EpdIt.DBUtilities

Public Module eis_reportingperiod

    Public Function GetEIType(eiyear As String) As String
        Dim query As String = "Select strEIType " &
            " FROM eiThresholdYears " &
            " Where strYear = @eiyear "

        Dim param As New SqlParameter("@eiyear", eiyear)

        Return DB.GetString(query, param)
    End Function

#Region " Pre-pop "

    Public Function PrePopulateEI(FSID As String, PastEIYear As String, CurrentEIYear As String) As Boolean
        Dim UserID As String = GetCookie(GecoCookie.UserID)
        Dim UserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UserID & "-" & UserName

        Dim queryList As New List(Of String)
        Dim paramsList As New List(Of SqlParameter())

        Dim params As SqlParameter() = {
            New SqlParameter("@CurrentEIYear", CurrentEIYear),
            New SqlParameter("@FSID", FSID),
            New SqlParameter("@PastEIYear", PastEIYear),
            New SqlParameter("@UpdateUser", UpdateUser)
        }

        queryList.Add("INSERT INTO EIS_ProcessReportingPeriod " &
            " (intInventoryYear, " &
            " FacilitySiteID, " &
            " EmissionsUnitID, " &
            " ProcessID, " &
            " RptPeriodTypeCode, " &
            " strEmissionOpTypeCode, " &
            " strCalcParaTypeCode, " &
            " CalcParamUoMCode, " &
            " CalculateMaterialCode, " &
            " intCalcDataYear, " &
            " STRCALCDATASOURCE, " &
            " STRREPORTINGPERIODCOMMENT, " &
            " Active, " &
            " UpdateUser, " &
            " UpdateDateTime, " &
            " CreateDateTime) " &
            "    SELECT " &
            "        @CurrentEIYear, " &
            "        @FSID, " &
            "        r.EmissionsUnitID, " &
            "        r.ProcessID, " &
            "        r.RptPeriodTypeCode, " &
            "        r.strEmissionOpTypeCode, " &
            "        r.strCalcParaTypeCode, " &
            "        r.CalcParamUoMCode, " &
            "        r.CalculateMaterialCode, " &
            "        r.intCalcDataYear, " &
            "        r.strCalcDataSource, " &
            "        left(concat('Comment from ', @PastEIYear) + concat(' EIS: ', r.strReportingPeriodComment), 400), " &
            "        '1', " &
            "        @UpdateUser, " &
            "        getdate(), " &
            "        getdate() " &
            "    FROM EIS_ProcessReportingPeriod r " &
            "        INNER JOIN EIS_EmissionsUnit e " &
            "            ON r.FacilitySiteID = e.FacilitySiteID AND " &
            "               r.EmissionsUnitID = e.EmissionsUnitID " &
            "    WHERE r.FacilitySiteID = @FSID " &
            "          AND r.intInventoryYear = @PastEIYear " &
            "          AND e.strUnitStatusCode = 'OP' ")

        paramsList.Add(params)

        queryList.Add("INSERT INTO EIS_ProcessOperatingDetails " &
            " (intInventoryYear, " &
            "  FacilitySiteID, " &
            "  EmissionsUnitID, " &
            "  ProcessID, " &
            "  RptPeriodTypeCode, " &
            "  intActualHoursPerPeriod, " &
            "  numAverageDaysPerWeek, " &
            "  numAverageHoursPerDay, " &
            "  numAverageWeeksPerPeriod, " &
            "  numPercentWinterActivity, " &
            "  numPercentSpringActivity, " &
            "  numPercentSummerActivity, " &
            "  numPercentFallActivity, " &
            "  Active, " &
            "  UpdateUser, " &
            "  UpdateDateTime, " &
            "  CreateDateTime) " &
            "     SELECT " &
            "         @CurrentEIYear, " &
            "         @FSID, " &
            "         o.EmissionsUnitID, " &
            "         o.ProcessID, " &
            "         o.RptPeriodTypeCode, " &
            "         o.intActualHoursPerPeriod, " &
            "         o.numAverageDaysPerWeek, " &
            "         o.numAverageHoursPerDay, " &
            "         o.numAverageWeeksPerPeriod, " &
            "         o.numPercentWinterActivity, " &
            "         o.numPercentSpringActivity, " &
            "         o.numPercentSummerActivity, " &
            "         o.numPercentFallActivity, " &
            "         '1', " &
            "         @UpdateUser, " &
            "         getdate(), " &
            "         getdate() " &
            "     FROM EIS_ProcessOperatingDetails o " &
            "         INNER JOIN EIS_EmissionsUnit u " &
            "             ON o.FacilitySiteID = u.FacilitySiteID " &
            "                AND o.EmissionsUnitID = u.EmissionsUnitID " &
            "     WHERE o.FacilitySiteID = @FSID " &
            "           AND o.intInventoryYear = @PastEIYear " &
            "           AND u.strUnitStatusCode = 'OP' ")

        paramsList.Add(params)

        queryList.Add("INSERT INTO EIS_ProcessRptPeriodSCP " &
            " (intInventoryYear, " &
            "  FacilitySiteID, " &
            "  EmissionsUnitID, " &
            "  ProcessID, " &
            "  RptPeriodTypeCode, " &
            "  SCPTypeCode, " &
            "  fltSCPValue, " &
            "  strSCPNumerator, " &
            "  strSCPDenominator, " &
            "  intSCPDataYear, " &
            "  STRSCPCOMMENT, " &
            "  Active, " &
            "  UpdateUser, " &
            "  UpdateDateTime, " &
            "  CreateDateTime) " &
            "     SELECT " &
            "         @CurrentEIYear, " &
            "         @FSID, " &
            "         p.EmissionsUnitID, " &
            "         p.ProcessID, " &
            "         'A', " &
            "         p.SCPTypeCode, " &
            "         p.fltSCPValue, " &
            "         p.strSCPNumerator, " &
            "         p.strSCPDenominator, " &
            "         p.intSCPDataYear, " &
            "         p.strSCPComment, " &
            "         '1', " &
            "         @UpdateUser, " &
            "         getdate(), " &
            "         getdate() " &
            "     FROM EIS_ProcessRptPeriodSCP p " &
            "         INNER JOIN EIS_EmissionsUnit u " &
            "             ON p.FacilitySiteID = u.FacilitySiteID " &
            "                AND p.EmissionsUnitID = u.EmissionsUnitID " &
            "     WHERE p.FacilitySiteID = @FSID " &
            "           AND p.intInventoryYear = @PastEIYear " &
            "           AND u.strUnitStatusCode = 'OP' ")

        paramsList.Add(params)

        queryList.Add("INSERT INTO EIS_ReportingPeriodEmissions " &
            " (intInventoryYear, " &
            "  FacilitySiteID, " &
            "  EmissionsUnitID, " &
            "  ProcessID, " &
            "  RptPeriodTypeCode, " &
            "  PollutantCode, " &
            "  EmissionsUoMCode, " &
            "  fltEmissionFactor, " &
            "  EmCalcMethodCode, " &
            "  EFNumUoMCode, " &
            "  EFDenUoMCode, " &
            "  STREMISSIONFACTORTEXT, " &
            "  strEmissionsComment, " &
            "  Active, " &
            "  UpdateUser, " &
            "  UpdateDateTime, " &
            "  CreateDateTime) " &
            "     SELECT " &
            "         @CurrentEIYear, " &
            "         @FSID, " &
            "         e.EmissionsUnitID, " &
            "         e.ProcessID, " &
            "         e.RptPeriodTypeCode, " &
            "         e.PollutantCode, " &
            "         e.EmissionsUoMCode, " &
            "         e.fltEmissionFactor, " &
            "         e.EmCalcMethodCode, " &
            "         e.EFNumUoMCode, " &
            "         e.EFDenUoMCode, " &
            "         e.strEmissionFactorText, " &
            "         left(concat('Comment from ', @PastEIYear) + concat(' EIS: ', e.strEmissionsComment), 400), " &
            "         '1', " &
            "         @UpdateUser, " &
            "         getdate(), " &
            "         getdate() " &
            "     FROM EIS_ReportingPeriodEmissions e " &
            "         INNER JOIN EIS_EmissionsUnit u " &
            "             ON e.FacilitySiteID = u.FacilitySiteID " &
            "                AND e.EmissionsUnitID = u.EmissionsUnitID " &
            "     WHERE e.FacilitySiteID = @FSID " &
            "           AND e.intInventoryYear = @PastEIYear " &
            "           AND u.strUnitStatusCode = 'OP' ")

        paramsList.Add(params)

        queryList.Add("update eis_Admin set " &
            " intPrePopYear = @PastEIYear " &
            " where FacilitySiteID = @FSID " &
            " and InventoryYear = @CurrentEIYear ")

        paramsList.Add(params)

        Return DB.RunCommand(queryList, paramsList)
    End Function

#End Region

#Region " Reset "

    Public Function ResetEI(fsid As String, eiyear As String) As Boolean
        Dim queryList As New List(Of String)
        Dim paramsList As New List(Of SqlParameter())

        Dim params As SqlParameter() = {
            New SqlParameter("@eiyear", eiyear),
            New SqlParameter("@fsid", fsid)
        }

        queryList.Add(
            "Delete FROM EIS_ProcessOperatingDetails " &
            " where FacilitySiteID = @fsid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "Delete FROM EIS_ReportingPeriodEmissions " &
            " where FacilitySiteID = @fsid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "Delete FROM EIS_ProcessRptPeriodSCP " &
            " where FacilitySiteID = @fsid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "Delete FROM EIS_ProcessReportingPeriod " &
            " where FacilitySiteID = @fsid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        Return DB.RunCommand(queryList, paramsList)
    End Function

#End Region

#Region " Insert New RP Process "

    Public Function InsertRPProcess(Year As Integer, FSID As String, EUID As String, EPID As String, RptType As String, EndUser As String) As Boolean
        Dim CalcParaTypeCode As String = Nothing
        Dim CalcParameterValue As Decimal? = Nothing
        Dim CalcParamUOMCode As String = Nothing
        Dim CalculateMaterialCode As String = Nothing

        Dim params As SqlParameter() = {
            New SqlParameter("@CurrentYear", Year),
            New SqlParameter("@FacilitySiteID", FSID),
            New SqlParameter("@EmissionsUnitID", EUID),
            New SqlParameter("@ProcessID", EPID),
            New SqlParameter("@RptPeriodTypeCode", RptType),
            New SqlParameter("@UpdateUser", EndUser)
        }

        If RptType = "O3D" Then
            Dim query As String = " SELECT " &
            "     strCalcParaTypeCode, " &
            "     fltCalcParameterValue, " &
            "     CalcParamUOMCode, " &
            "     CalculateMaterialCode " &
            " FROM " &
            "     eis_ProcessReportingPeriod " &
            " WHERE intInventoryYear = @CurrentYear " &
            "       AND FacilitySiteID = @FacilitySiteID " &
            "       AND EmissionsUnitID = @EmissionsUnitID " &
            "       AND ProcessID = @ProcessID " &
            "       AND RptPeriodTypeCode = 'A' "

            Dim dr As DataRow = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then
                CalcParaTypeCode = GetNullableString(dr("strCalcParaTypeCode"))
                CalcParameterValue = GetNullableString(dr("fltCalcParameterValue")).ParseAsNullableDecimal()
                CalcParamUOMCode = GetNullableString(dr("CalcParamUOMCode"))
                CalculateMaterialCode = GetNullableString(dr("CalculateMaterialCode"))
            End If
        End If

        Dim queryList As New List(Of String)
        Dim paramsList As New List(Of SqlParameter())

        queryList.Add(" INSERT INTO eis_ProcessReportingPeriod " &
            " (intInventoryYear, " &
            "  FacilitySiteID, " &
            "  EmissionsUnitID, " &
            "  ProcessID, " &
            "  RptPeriodTypeCode, " &
            "  strEmissionOpTypeCode, " &
            "  strCalcParaTypeCode, " &
            "  fltCalcParameterValue, " &
            "  CalcParamUOMCode, " &
            "  CalculateMaterialCode, " &
            "  intCalcDataYear, " &
            "  Active, " &
            "  UpdateUser, " &
            "  UpdateDateTime, " &
            "  CreateDateTime) " &
            "     SELECT " &
            "         @CurrentYear, " &
            "         @FacilitySiteID, " &
            "         @EmissionsUnitID, " &
            "         @ProcessID, " &
            "         @RptPeriodTypeCode, " &
            "         'R', " &
            "         @CalcParaTypeCode, " &
            "         @CalcParameterValue, " &
            "         @CalcParamUOMCode, " &
            "         @CalculateMaterialCode, " &
            "         @CurrentYear, " &
            "         '1', " &
            "         @UpdateUser, " &
            "         getdate(), " &
            "         getdate() " &
            "     WHERE NOT exists " &
            "     (SELECT intInventoryYear " &
            "      FROM EIS_ProcessReportingPeriod " &
            "      WHERE FacilitySiteID = @FacilitySiteID " &
            "            AND EmissionsUnitID = @EmissionsUnitID " &
            "            AND ProcessID = @ProcessID " &
            "            AND intInventoryYear = @CurrentYear " &
            "            AND RPTPeriodTypeCode = @RptPeriodTypeCode) ")

        paramsList.Add({
            New SqlParameter("@CurrentYear", Year),
            New SqlParameter("@FacilitySiteID", FSID),
            New SqlParameter("@EmissionsUnitID", EUID),
            New SqlParameter("@ProcessID", EPID),
            New SqlParameter("@RptPeriodTypeCode", RptType),
            New SqlParameter("@UpdateUser", EndUser),
            New SqlParameter("@CalcParaTypeCode", CalcParaTypeCode),
            New SqlParameter("@CalcParameterValue", CalcParameterValue),
            New SqlParameter("@CalcParamUOMCode", CalcParamUOMCode),
            New SqlParameter("@CalculateMaterialCode", CalculateMaterialCode)
        })

        queryList.Add("INSERT INTO EIS_ProcessOperatingDetails " &
            " (intInventoryYear, " &
            "  FacilitySiteID, " &
            "  EmissionsUnitID, " &
            "  ProcessID, " &
            "  RptPeriodTypeCode, " &
            "  Active, " &
            "  UpdateUser, " &
            "  UpdateDateTime, " &
            "  CreateDateTime) " &
            "     SELECT " &
            "         @CurrentYear, " &
            "         @FacilitySiteID, " &
            "         @EmissionsUnitID, " &
            "         @ProcessID, " &
            "         @RptPeriodTypeCode, " &
            "         '1', " &
            "         @UpdateUser, " &
            "         getdate(), " &
            "         getdate() " &
            "     WHERE NOT exists " &
            "     (SELECT intInventoryYear " &
            "      FROM EIS_ProcessOperatingDetails " &
            "      WHERE FacilitySiteID = @FacilitySiteID " &
            "            AND EmissionsUnitID = @EmissionsUnitID " &
            "            AND ProcessID = @ProcessID " &
            "            AND intInventoryYear = @CurrentYear " &
            "            AND RPTPeriodTypeCode = @RptPeriodTypeCode) ")

        paramsList.Add(params)

        Return DB.RunCommand(queryList, paramsList)
    End Function

#End Region

#Region " Delete Routines "

    Public Function DeleteRPProcessAndEmissions(eiyear As String, fsid As String, euid As String, epid As String) As Boolean
        'This routine removes a process from the reporting period based on the FSID, EUID, PRID and EI Year
        'Used when deleting an emission unit or making an emission unit temporarily or permanently shut down
        Dim queryList As New List(Of String)
        Dim paramsList As New List(Of SqlParameter())

        Dim params As SqlParameter() = {
            New SqlParameter("@eiyear", eiyear),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid)
        }

        queryList.Add(
            "delete FROM EIS_ReportingPeriodEmissions " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "delete FROM EIS_ProcessOperatingDetails " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "delete FROM EIS_ProcessRptPeriodSCP " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "delete FROM EIS_ProcessReportingPeriod " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        Return DB.RunCommand(queryList, paramsList)
    End Function

    Public Function DeleteRPProcessAndEmissions_EU(eiyear As String, fsid As String, euid As String) As Boolean
        'This routine removes all processes for an emission unit from the reporting period based on the FSID, EUID and EI Year
        'Used when deleting an emission unit or making an emission unit temporarily or permanently shut down
        Dim queryList As New List(Of String)
        Dim paramsList As New List(Of SqlParameter())

        Dim params As SqlParameter() = {
            New SqlParameter("@eiyear", eiyear),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        queryList.Add(
            "delete FROM EIS_ReportingPeriodEmissions " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "delete FROM EIS_ProcessOperatingDetails " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "delete FROM EIS_ProcessRptPeriodSCP " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        queryList.Add(
            "delete FROM EIS_ProcessReportingPeriod " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and intInventoryYear = @eiyear "
        )
        paramsList.Add(params)

        Return DB.RunCommand(queryList, paramsList)
    End Function

    Public Function DeleteRPSCP(eiyear As String, fsid As String, euid As String, epid As String) As Boolean
        Dim query As String = "delete FROM EIS_ProcessRptPeriodSCP " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid " &
            " and intInventoryYear = @eiyear "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid),
            New SqlParameter("@eiyear", eiyear)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteSulfurAsh(eiyear As String, fsid As String, euid As String, epid As String, contype As String) As Boolean
        Dim DeleteType As String = ""

        If contype = "SULFUR" Then
            DeleteType = "Percent Sulfur Content"
        ElseIf contype = "ASH" Then
            DeleteType = "Percent Ash Content"
        End If

        Dim query As String = "delete FROM EIS_ProcessRptPeriodSCP " &
            " where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid " &
            " and intInventoryYear = @eiyear " &
            " and SCPTypeCode = @DeleteType "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid),
            New SqlParameter("@eiyear", eiyear),
            New SqlParameter("@DeleteType", DeleteType)
        }

        Return DB.RunCommand(query, params)
    End Function

#End Region

    Public Sub SaveOption(fsid As String, opt As String, uuser As String, eiyr As String,
                               Optional ooreason As String = Nothing,
                               Optional colocated As Boolean? = Nothing,
                               Optional colocation As String = Nothing)
        Dim eisAccessCode As String
        Dim eisStatusCode As String

        If opt = "1" Then
            eisAccessCode = "2"
            eisStatusCode = "3"
        Else
            eisAccessCode = "1"
            eisStatusCode = "2"
        End If

        Dim query As String

        Dim params As SqlParameter() = {
            New SqlParameter("@opt", opt),
            New SqlParameter("@eisAccessCode", eisAccessCode),
            New SqlParameter("@eisStatusCode", eisStatusCode),
            New SqlParameter("@ooreason", ooreason),
            New SqlParameter("@uuser", uuser),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@eiyr", eiyr),
            New SqlParameter("@colocated", colocated),
            New SqlParameter("@colocation", colocation)
        }

        If opt = "1" Then
            'facility not participating in EI
            Dim query0 As String = "select datFinalize " &
                " FROM eis_Admin " &
                " where FacilitySiteID = @fsid " &
                " and InventoryYear = @eiyr " &
                " and datInitialFinalize is not null "

            If DB.ValueExists(query0, params) Then
                'facility not participating in EI
                'If datInitialFinalize is null
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
            Else
                'facility not participating in EI
                'If datInitialFinalize is not null
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
            End If
        Else
            'OptOut = '0' - facility participating
            query = "update eis_Admin set " &
                " strOptOut = @opt, " &
                " eisAccessCode = @eisAccessCode, " &
                " eisStatusCode = @eisStatusCode, " &
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

            SendEmail(GecoContactEmail, "GECO EIS - Facility opt out and co-location", plainBody, htmlBody,
                      caller:="eis_reportingperiod.SaveOption")
        End If
    End Sub

#Region " Get Values "

    Public Function GetEmissionTotal(fsid As String,
                                      euid As String,
                                      prid As String,
                                      pollcode As String,
                                      rptperiodtype As String,
                                      eiyr As Integer) As Double

        Dim query As String = "select fltTotalEmissions " &
            " from eis_ReportingPeriodEmissions " &
            " where intInventoryYear = @eiyr " &
            " and FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @prid " &
            " and PollutantCode = @pollcode " &
            " and RptPeriodTypeCode = @rptperiodtype "

        Dim param As SqlParameter() = {
            New SqlParameter("@eiyr", eiyr),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid),
            New SqlParameter("@pollcode", pollcode),
            New SqlParameter("@rptperiodtype", rptperiodtype)
        }

        Return DB.GetString(query, param)
    End Function

#End Region

End Module