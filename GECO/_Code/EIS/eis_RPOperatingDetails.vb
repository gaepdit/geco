Imports System.Data.SqlClient

Public Module eis_RPOperatingDetails

    ' Calc param UOM
    Public Function GetCalcParamUoMCodes() As DataTable
        Dim query = "select STRDESC as Description, CALCPARAMUOMCODE as Code
            from EISLK_CALCPARAMUOMCODE
            where ACTIVE = '1'
            order by Description"

        Return DB.GetDataTable(query)
    End Function

    Public Function GetCalcParamUoMCodesForScc(SCC As String) As DataTable
        If SCC Is Nothing Then
            Return Nothing
        End If

        Dim query As String = "select c.CALCPARAMUOMCODE as Code,
                   c.STRDESC          as Description
            from EISLK_SCC_UOM u
                 inner join EISLK_CALCPARAMUOMCODE c
                            on u.UnitOfMeasure = c.CALCPARAMUOMCODE
            where ACTIVE = '1'
              and SourceClassificationCode = @SCC
            order by Description"

        Dim param As New SqlParameter("@SCC", SCC)

        Return DB.GetDataTable(query, param)
    End Function

    Public Function GetParamUomDesc(code As String) As String
        Dim query As String = " select STRDESC " &
        " from EISLK_CALCPARAMUOMCODE " &
        " where CALCPARAMUOMCODE = @code "

        Dim param As New SqlParameter("@code", code)

        Return DB.GetString(query, param)
    End Function

    ' Calc Param Types
    Public Function GetCalcParamTypes() As DataTable
        Dim query = "select strdesc, calcparatypecode FROM EISLK_CALCPARAMETERTYPECODE where Active = '1' order by strDesc "

        Return DB.GetDataTable(query)
    End Function

    ' Calc param material
    Public Function GetCalcMaterialCodes() As DataTable
        Dim query = "select STRDESC as Description, CALCULATEMATERIALCODE as Code
            FROM EISLK_CALCULATEMATERIALCODE
            where ACTIVE = '1'
            order by STRDESC"

        Return DB.GetDataTable(query)
    End Function

    Public Function GetCalcMaterialCodesForScc(SCC As String) As DataTable
        If SCC Is Nothing Then
            Return Nothing
        End If

        Dim query As String = "select c.CALCULATEMATERIALCODE as Code, c.STRDESC as Description
            FROM EISLK_CALCULATEMATERIALCODE c
                 inner join EISLK_SCC_CALCULATEMATERIALCODE s
                            on s.CALCULATEMATERIALCODE = c.CALCULATEMATERIALCODE
            where ACTIVE = '1'
              and s.SCC = @SCC
            order by Description"

        Dim param As New SqlParameter("@SCC", SCC)

        Return DB.GetDataTable(query, param)
    End Function

    ' Fuel burning processes
    Public Function GetFuelBurningSccList() As DataTable
        Dim query As String = "select distinct SCC from EISLK_SCC_CALCULATEMATERIALCODE"

        Dim dt As DataTable = DB.GetDataTable(query)
        dt.PrimaryKey = {dt.Columns(0)}

        Return dt
    End Function

    ' SCP denominator
    Public Function GetScpDenomUoMCodes() As DataTable
        Dim query = "select SCPDENOMUOMCODE as Code, STRDESC as Description
            from EISLK_SCPDENOMUOMCODE
            where ACTIVE = '1'
            order by Description"

        Return DB.GetDataTable(query)
    End Function

    Public Function GetScpDenomUoMCodesForScc(SCC As String) As DataTable
        Dim query = "select c.SCPDENOMUOMCODE as Code, c.STRDESC as Description
            from EISLK_SCPDENOMUOMCODE c
                 inner join EISLK_SCC_SCPDENOMUOMCODE s
                            on s.SCPDENOMUOMCODE = c.SCPDENOMUOMCODE
            where ACTIVE = '1'
              and s.SCC = @SCC"

        Dim param As New SqlParameter("@SCC", SCC)

        Return DB.GetDataTable(query, param)
    End Function

    ' Reporting period operating details
    Public Function GetRPOperatingDetails(Year As String, FSID As String, EUID As String, EPID As String) As DataRow
        Dim query = "select INTINVENTORYYEAR, " &
            " EMISSIONSUNITID, " &
            " STRUNITDESCRIPTION, " &
            " PROCESSID, " &
            " STRPROCESSDESCRIPTION, " &
            " FLTCALCPARAMETERVALUE, " &
            " CALCPARAMUOMCODE, " &
            " STRCALCPARATYPECODE, " &
            " CALCULATEMATERIALCODE, " &
            " STRREPORTINGPERIODCOMMENT, " &
            " INTACTUALHOURSPERPERIOD, " &
            " SOURCECLASSCODE, " &
            " NUMAVERAGEDAYSPERWEEK, " &
            " NUMAVERAGEHOURSPERDAY, " &
            " NUMAVERAGEWEEKSPERPERIOD, " &
            " NUMPERCENTWINTERACTIVITY, " &
            " NUMPERCENTSPRINGACTIVITY, " &
            " NUMPERCENTSUMMERACTIVITY, " &
            " NUMPERCENTFALLACTIVITY, " &
            " HEATCONTENT, " &
            " HCNUMER, " &
            " HCDENOM, " &
            " ASHCONTENT, " &
            " SULFURCONTENT, " &
            " convert(char, UpdateDateTime, 20) As UpdateDateTime, " &
            " convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
            " UpdateUser" &
            " FROM VW_EIS_RPDETAILS" &
            " where INTINVENTORYYEAR = @Year " &
            " and FACILITYSITEID = @FSID " &
            " and EMISSIONSUNITID = @EUID " &
            " and PROCESSID = @EPID " &
            " AND RPTPERIODTYPECODE = 'A' "

        Dim params = {
            New SqlParameter("@Year", Year),
            New SqlParameter("@FSID", FSID),
            New SqlParameter("@EUID", EUID),
            New SqlParameter("@EPID", EPID)
        }

        Return DB.GetDataRow(query, params)
    End Function

    Public Sub SaveReportingPeriodData(eiyear As Integer, fsid As String, euid As String, epid As String, uuser As String, EmissionTypeCode As String,
                                   CalcParamValue As Double, CalcParamUoMCode As String, MaterialCode As String, RPComment As String,
                                   ActHrsPerPeriod As Integer?, AvgDaysPerWeek As Decimal?, AvgHoursPerDay As Decimal?, AvgWeeksPerYear As Decimal?,
                                   PctWinter As Decimal?, PctSpring As Decimal?, PctSummer As Decimal?, PctFall As Decimal?)
        ' Combine SaveReportingPeriod and SaveRPDetails from rp_operscp_edit
        Dim queryList As New List(Of String)

        Dim paramsList As New List(Of SqlParameter())

        queryList.Add(
            "Update EIS_ProcessReportingPeriod Set" &
            " strCalcParaTypeCode = @EmissionTypeCode," &
            " fltCalcParameterValue = @CalcParamValue," &
            " CalcParamUoMCode = @CalcParamUoMCode," &
            " CalculateMaterialCode = @MaterialCode," &
            " strReportingPeriodComment = @RPComment," &
            " UpdateUser = @uuser," &
            " UpdateDateTime = getdate()" &
            " Where EIS_ProcessReportingPeriod.intInventoryYear = @eiyear " &
            " And EIS_ProcessReportingPeriod.FacilitySiteID = @fsid " &
            " And EIS_ProcessReportingPeriod.EmissionsUnitID = @euid " &
            " And EIS_ProcessReportingPeriod.ProcessID = @epid "
        )

        paramsList.Add({
            New SqlParameter("@EmissionTypeCode", EmissionTypeCode),
            New SqlParameter("@CalcParamValue", CalcParamValue),
            New SqlParameter("@CalcParamUoMCode", CalcParamUoMCode),
            New SqlParameter("@MaterialCode", MaterialCode),
            New SqlParameter("@RPComment", Left(RPComment, 400)),
            New SqlParameter("@uuser", uuser),
            New SqlParameter("@eiyear", eiyear),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid)
        })

        queryList.Add(
            "Update EIS_ProcessOperatingDetails Set " &
            " INTACTUALHOURSPERPERIOD = @ActHrsPerPeriod, " &
            " NUMAVERAGEDAYSPERWEEK = @AvgDaysPerWeek, " &
            " NUMAVERAGEHOURSPERDAY = @AvgHoursPerDay, " &
            " NUMAVERAGEWEEKSPERPERIOD = @AvgWeeksPerYear, " &
            " NUMPERCENTWINTERACTIVITY = @PctWinter, " &
            " NUMPERCENTSPRINGACTIVITY = @PctSpring, " &
            " NUMPERCENTSUMMERACTIVITY = @PctSummer, " &
            " NUMPERCENTFALLACTIVITY = @PctFall, " &
            " UpdateUser = @uuser, " &
            " UpdateDateTime = getdate() " &
            " Where intInventoryYear = @eiyear " &
            " and FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and ProcessID = @epid "
        )

        paramsList.Add({
                New SqlParameter("@ActHrsPerPeriod", ActHrsPerPeriod),
                New SqlParameter("@AvgDaysPerWeek", AvgDaysPerWeek),
                New SqlParameter("@AvgHoursPerDay", AvgHoursPerDay),
                New SqlParameter("@AvgWeeksPerYear", AvgWeeksPerYear),
                New SqlParameter("@PctWinter", PctWinter),
                New SqlParameter("@PctSpring", PctSpring),
                New SqlParameter("@PctSummer", PctSummer),
                New SqlParameter("@PctFall", PctFall),
                New SqlParameter("@uuser", uuser),
                New SqlParameter("@eiyear", eiyear),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid)
            })

        DB.RunCommand(queryList, paramsList)
    End Sub

    Public Sub SaveRptPeriodScp(eiyear As Integer, fsid As String, euid As String, epid As String, uuser As String, ScpType As String,
                                ScpValue As Double?, Optional ScpNumer As String = Nothing, Optional ScpDenom As String = Nothing)
        Dim query As String = "SELECT convert(bit, count(*))
            FROM EIS_PROCESSRPTPERIODSCP
            WHERE INTINVENTORYYEAR = @eiyear
              AND FACILITYSITEID = @fsid
              AND EMISSIONSUNITID = @euid
              AND PROCESSID = @epid
              AND SCPTYPECODE = @ScpType"

        Dim params As SqlParameter() = {
            New SqlParameter("@eiyear", eiyear),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@epid", epid),
            New SqlParameter("@ScpType", ScpType),
            New SqlParameter("@ScpValue", ScpValue),
            New SqlParameter("@ScpNumer", ScpNumer),
            New SqlParameter("@ScpDenom", ScpDenom),
            New SqlParameter("@uuser", uuser)
        }

        If DB.GetBoolean(query, params) Then
            query = "UPDATE EIS_PROCESSRPTPERIODSCP
                SET FLTSCPVALUE       = @ScpValue,
                    STRSCPNUMERATOR   = @ScpNumer,
                    STRSCPDENOMINATOR = @ScpDenom,
                    INTSCPDATAYEAR    = @eiyear,
                    UPDATEUSER        = @uuser,
                    UPDATEDATETIME    = sysdatetime()
                WHERE INTINVENTORYYEAR = @eiyear
                  AND FACILITYSITEID = @fsid
                  AND EMISSIONSUNITID = @euid
                  AND PROCESSID = @epid
                  AND SCPTYPECODE = @ScpType"
        Else
            query = "INSERT INTO EIS_PROCESSRPTPERIODSCP
                (INTINVENTORYYEAR,
                 FACILITYSITEID,
                 EMISSIONSUNITID,
                 PROCESSID,
                 RPTPERIODTYPECODE,
                 SCPTYPECODE,
                 FLTSCPVALUE,
                 STRSCPNUMERATOR,
                 STRSCPDENOMINATOR,
                 INTSCPDATAYEAR,
                 ACTIVE,
                 UPDATEUSER,
                 UPDATEDATETIME,
                 CREATEDATETIME)
                Values (@eiyear,
                        @fsid,
                        @euid,
                        @epid,
                        'A',
                        @ScpType,
                        @ScpValue,
                        @ScpNumer,
                        @ScpDenom,
                        @eiyear,
                        '1',
                        @uuser,
                        sysdatetime(),
                        sysdatetime())"
        End If

        DB.RunCommand(query, params)
    End Sub

End Module