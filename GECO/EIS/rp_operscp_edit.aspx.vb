Imports System.Data.SqlClient

Partial Class EIS_rp_operscp_edit
    Inherits Page

    Private Property SourceClassCode As String = ""
    Private Property CalcParamUomCode As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim InventoryYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim EmissionsUnitID As String = Request.QueryString("eu")
        Dim ProcessID As String = Request.QueryString("ep")
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim DeleteMsg As String = "Are you sure you want to remove Emissions Unit ID " & EmissionsUnitID &
            " and Process ID " & ProcessID & " from the current reporting period?"

        EIAccessCheck(EISAccessCode, EISStatus)

        If Not IsPostBack Then
            LoadCalcParamTypeDDL()
            LoadMaterialCodeDDL()
            LoadFuelBurningDDL()
            LoadHeatContentDenUoMDDL()
            btnSummary2.Visible = False
            lblTotalSeasonValidate.Visible = False
            lblDeleteConfirm1.Text = DeleteMsg

            LoadRPDetails(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID)
            LoadCalcParamUoMDDL()
            SumSeasonalPercentages()
        End If

    End Sub

#Region " Load Drop Down Lists "

    Private Sub LoadCalcParamTypeDDL()
        ddlCalcParamType.Items.Add("-- Select Type --")

        Try
            Dim query = "select strdesc, calcparatypecode FROM EISLK_CALCPARAMETERTYPECODE where Active = '1' order by strDesc "

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("calcparatypecode")
                    }
                    ddlCalcParamType.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadCalcParamUoMDDL()
        Dim RequiredParamUomForScc As Tuple(Of String, String) = Nothing

        If Not String.IsNullOrEmpty(SourceClassCode) Then
            RequiredParamUomForScc = GetRequiredUomForScc(SourceClassCode)
        End If

        ddlCalcParamUoM.Items.Add("-- Select Units --")

        Try
            If RequiredParamUomForScc Is Nothing Then

                Dim query As String = "select strdesc, calcparamuomcode FROM EISLK_CALCPARAMUOMCODE " &
                    " where Active = '1' order by strDesc"

                Dim dt = DB.GetDataTable(query)

                If dt IsNot Nothing Then
                    For Each dr In dt.Rows
                        Dim newListItem As New ListItem With {
                            .Text = dr.Item("strdesc"),
                            .Value = dr.Item("calcparamuomcode")
                        }
                        ddlCalcParamUoM.Items.Add(newListItem)
                    Next
                End If

                cmpCalcParamUom.Enabled = False
                reqvCalcParamUoM.Enabled = True

            Else

                ddlCalcParamUoM.Items.Add(New ListItem(RequiredParamUomForScc.Item2, RequiredParamUomForScc.Item1))

                If RequiredParamUomForScc.Item1 <> CalcParamUomCode Then
                    ddlCalcParamUoM.Items.Add(New ListItem(GetParamUomDesc(CalcParamUomCode), CalcParamUomCode))
                End If

                cmpCalcParamUom.ValueToCompare = RequiredParamUomForScc.Item1
                cmpCalcParamUom.ErrorMessage = String.Concat("Selected unit must be ", RequiredParamUomForScc.Item2, " based on the SCC.")
                cmpCalcParamUom.Enabled = True
                reqvCalcParamUoM.Enabled = False

            End If

            If String.IsNullOrEmpty(CalcParamUomCode) OrElse ddlCalcParamUoM.Items.FindByValue(CalcParamUomCode) Is Nothing Then
                ddlCalcParamUoM.SelectedIndex = 0
            Else
                ddlCalcParamUoM.SelectedValue = CalcParamUomCode
                cmpCalcParamUom.Validate()
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadMaterialCodeDDL()
        ddlMaterialCode.Items.Add("-- Select Material --")

        Try
            Dim query = "select strdesc, calculatematerialcode FROM EISLK_CALCULATEMATERIALCODE " &
                " where Active = '1' order by strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("calculatematerialcode")
                    }
                    ddlMaterialCode.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadFuelBurningDDL()
        ddlFuelBurning.Items.Add("No")
        ddlFuelBurning.Items.Add("Yes")
    End Sub

    Private Sub LoadHeatContentDenUoMDDL()
        ddlHeatContentDenUoM.Items.Add("-Select a Value-")

        Try
            Dim query = "select SCPDenomUOMCode, strDesc FROM eislk_SCPDenomUOMCode " &
                " where Active = '1' order by strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("SCPDenomUOMCode")
                    }
                    ddlHeatContentDenUoM.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

    Private Function SumSeasonalPercentages() As Decimal

        Dim Winter As Decimal
        Dim Spring As Decimal
        Dim Summer As Decimal
        Dim Fall As Decimal
        Dim TotalSeasons As Decimal

        If txtWinterPct.Text = "" Then
            Winter = 0
        Else
            Winter = CDec(txtWinterPct.Text)
        End If
        If txtSpringPct.Text = "" Then
            Spring = 0
        Else
            Spring = CDec(txtSpringPct.Text)
        End If
        If txtSummerPct.Text = "" Then
            Summer = 0
        Else
            Summer = CDec(txtSummerPct.Text)
        End If
        If txtFallPct.Text = "" Then
            Fall = 0
        Else
            Fall = CDec(txtFallPct.Text)
        End If

        TotalSeasons = Winter + Spring + Summer + Fall
        If TotalSeasons = 100 Then
            txtTotalPct.ForeColor = Drawing.Color.Green
            lblTotalSeasonValidate.Visible = False
        Else
            txtTotalPct.ForeColor = Drawing.Color.Red
        End If
        txtTotalPct.Text = TotalSeasons
        Return TotalSeasons
    End Function

    Private Sub LoadRPDetails(ByVal Year As String, ByVal FSID As String, ByVal EUID As String, ByVal EPID As String)
        Dim HeatContent As String = ""
        Dim SulfurContent As String = ""
        Dim AshContent As String = ""

        Try
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

            Dim dr = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                'Emission Unit and Process Descriptions
                If IsDBNull(dr("INTINVENTORYYEAR")) Then
                    txtEISYear.Text = ""
                Else
                    txtEISYear.Text = dr.Item("INTINVENTORYYEAR")
                End If
                If IsDBNull(dr("EmissionsUnitID")) Then
                    txtEmissionsUnitID.Text = ""
                Else
                    txtEmissionsUnitID.Text = dr.Item("EmissionsUnitID")
                End If
                If IsDBNull(dr("strUnitDescription")) Then
                    txtUnitDescription.Text = ""
                Else
                    txtUnitDescription.Text = dr.Item("strUnitDescription")
                End If
                If IsDBNull(dr("PROCESSID")) Then
                    txtProcessID.Text = ""
                Else
                    txtProcessID.Text = dr.Item("PROCESSID")
                End If
                If IsDBNull(dr("strProcessDescription")) Then
                    txtProcessDescription.Text = ""
                Else
                    txtProcessDescription.Text = dr.Item("strProcessDescription")
                End If
                If IsDBNull(dr("SOURCECLASSCODE")) Then
                    txtSourceClassCode.Text = ""
                Else
                    txtSourceClassCode.Text = dr.Item("SOURCECLASSCODE")
                    SourceClassCode = dr.Item("SOURCECLASSCODE")
                End If

                'Begin Process Operating Details
                If IsDBNull(dr("STRCALCPARATYPECODE")) Then
                    ddlCalcParamType.SelectedIndex = 0
                Else
                    ddlCalcParamType.SelectedValue = dr.Item("STRCALCPARATYPECODE")
                End If
                If IsDBNull(dr("FLTCALCPARAMETERVALUE")) Then
                    txtCalcParamValue.Text = ""
                Else
                    txtCalcParamValue.Text = dr.Item("FLTCALCPARAMETERVALUE")
                End If

                If Not IsDBNull(dr("CALCPARAMUOMCODE")) Then
                    CalcParamUomCode = dr.Item("CALCPARAMUOMCODE")
                End If

                If IsDBNull(dr("CALCULATEMATERIALCODE")) Then
                    ddlMaterialCode.SelectedIndex = 0
                Else
                    ddlMaterialCode.SelectedValue = dr.Item("CALCULATEMATERIALCODE")
                End If
                If IsDBNull(dr("STRREPORTINGPERIODCOMMENT")) Then
                    txtRPComment.Text = ""
                Else
                    txtRPComment.Text = dr.Item("STRREPORTINGPERIODCOMMENT")
                End If

                'Daily, Weekly, Annual Information
                If IsDBNull(dr("NUMAVERAGEHOURSPERDAY")) Then
                    txtAvgHoursPerDay.Text = ""
                Else
                    txtAvgHoursPerDay.Text = dr.Item("NUMAVERAGEHOURSPERDAY")
                End If
                If IsDBNull(dr("NUMAVERAGEDAYSPERWEEK")) Then
                    txtAvgDaysPerWeek.Text = ""
                Else
                    txtAvgDaysPerWeek.Text = dr.Item("NUMAVERAGEDAYSPERWEEK")
                End If
                If IsDBNull(dr("NUMAVERAGEWEEKSPERPERIOD")) Then
                    txtAvgWeeksPerYear.Text = ""
                Else
                    txtAvgWeeksPerYear.Text = dr.Item("NUMAVERAGEWEEKSPERPERIOD")
                End If
                If IsDBNull(dr("INTACTUALHOURSPERPERIOD")) Then
                    txtActualHoursPerYear.Text = ""
                Else
                    txtActualHoursPerYear.Text = dr.Item("INTACTUALHOURSPERPERIOD")
                End If

                'Seasonal Operation Precentages
                If IsDBNull(dr("NUMPERCENTWINTERACTIVITY")) Then
                    txtWinterPct.Text = "0"
                Else
                    txtWinterPct.Text = dr.Item("NUMPERCENTWINTERACTIVITY")
                End If
                If IsDBNull(dr("NUMPERCENTSPRINGACTIVITY")) Then
                    txtSpringPct.Text = "0"
                Else
                    txtSpringPct.Text = dr.Item("NUMPERCENTSPRINGACTIVITY")
                End If
                If IsDBNull(dr("NUMPERCENTSUMMERACTIVITY")) Then
                    txtSummerPct.Text = "0"
                Else
                    txtSummerPct.Text = dr.Item("NUMPERCENTSUMMERACTIVITY")
                End If
                If IsDBNull(dr("NUMPERCENTFALLACTIVITY")) Then
                    txtFallPct.Text = "0"
                Else
                    txtFallPct.Text = dr.Item("NUMPERCENTFALLACTIVITY")
                End If

                'Fuel Burning Information
                If IsDBNull(dr("HEATCONTENT")) Then
                    HeatContent = ""
                Else
                    HeatContent = dr.Item("HEATCONTENT")
                End If
                If IsDBNull(dr("SULFURCONTENT")) Then
                    SulfurContent = ""
                    cbxSulfurNegligible.Checked = False
                    txtSulfurPct.Enabled = True
                    reqvSulfurPct.Enabled = True
                Else
                    SulfurContent = dr.Item("SULFURCONTENT")
                End If
                If IsDBNull(dr("ASHCONTENT")) Then
                    AshContent = ""
                    cbxAshNegligible.Checked = False
                    txtAshPct.Enabled = True
                    reqvAshPct.Enabled = True
                Else
                    AshContent = dr.Item("ASHCONTENT")
                End If

                If (HeatContent = "") Then
                    ddlFuelBurning.SelectedValue = "No"
                    pnlFuelBurning.Visible = False
                Else
                    ddlFuelBurning.SelectedValue = "Yes"
                    pnlFuelBurning.Visible = True
                    txtHeatContent.Text = HeatContent
                    txtSulfurPct.Text = SulfurContent
                    txtAshPct.Text = AshContent
                    If txtSulfurPct.Text = "" Then
                        cbxSulfurNegligible.Checked = True
                        reqvSulfurPct.Enabled = False
                        txtSulfurPct.Enabled = False
                    Else
                        cbxSulfurNegligible.Checked = False
                        reqvSulfurPct.Enabled = True
                        txtSulfurPct.Enabled = True
                    End If
                    If txtAshPct.Text = "" Then
                        cbxAshNegligible.Checked = True
                        reqvAshPct.Enabled = False
                        txtAshPct.Enabled = False
                    Else
                        cbxAshNegligible.Checked = False
                        reqvAshPct.Enabled = True
                        txtAshPct.Enabled = True
                    End If

                    If IsDBNull(dr("HCDENOM")) Then
                        ddlHeatContentDenUoM.SelectedIndex = 0
                    Else
                        ddlHeatContentDenUoM.SelectedValue = dr.Item("HCDENOM")
                    End If
                End If

                'Hide Delete Button if LastEISSubmit is not null
                If IsDBNull(dr("LastEISSubmitDate")) Then
                    btnDelete.Visible = True
                Else
                    btnDelete.Visible = False
                End If

            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#Region "Save Routines"

    Private Sub SaveReportingPeriod(ByVal eiyear As Integer, ByVal fsid As String, ByVal euid As String, ByVal epid As String, ByVal uuser As String)
        Dim EmissionTypeCode As String = ddlCalcParamType.SelectedValue
        Dim CalcParamValue As Double = txtCalcParamValue.Text
        Dim CalcParamUoMCode As String = ddlCalcParamUoM.SelectedValue
        Dim MaterialCode As String = ddlMaterialCode.SelectedValue
        Dim RPComment As String = txtRPComment.Text

        Dim query As String = "Update EIS_ProcessReportingPeriod Set" &
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

        Dim params As SqlParameter() = {
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
        }

        Try
            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub SaveRPDetails(ByVal eiyear As Integer, ByVal fsid As String, ByVal euid As String, ByVal epid As String, ByVal uuser As String)
        Dim ActHrsPerPeriod As Integer? = txtActualHoursPerYear.Text.ParseAsNullableInteger()
        Dim AvgDaysPerWeek As Decimal? = txtAvgDaysPerWeek.Text.ParseAsNullableDecimal()
        Dim AvgHoursPerDay As Decimal? = txtAvgHoursPerDay.Text.ParseAsNullableDecimal()
        Dim AvgWeeksPerYear As Decimal? = txtAvgWeeksPerYear.Text.ParseAsNullableDecimal()
        Dim PctWinter As Decimal? = txtWinterPct.Text.ParseAsNullableDecimal()
        Dim PctSpring As Decimal? = txtSpringPct.Text.ParseAsNullableDecimal()
        Dim PctSummer As Decimal? = txtSummerPct.Text.ParseAsNullableDecimal()
        Dim PctFall As Decimal? = txtFallPct.Text.ParseAsNullableDecimal()

        Try
            Dim query = "Update EIS_ProcessOperatingDetails Set " &
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

            Dim params As SqlParameter() = {
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
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub SaveHeatContent(ByVal eiyear As Integer, ByVal fsid As String, ByVal euid As String, ByVal epid As String, ByVal uuser As String)
        Dim HCType As String = "Heat Content"
        Dim HCValue As Double? = txtHeatContent.Text.ParseAsNullableDouble()
        Dim HCNum As String = "E6BTU"
        Dim HCDen As String = ddlHeatContentDenUoM.SelectedValue

        'Save Heat Content Information
        Try
            'Check if Heat Content exists
            Dim query = "Select SCPTYPECODE FROM EIS_PROCESSRPTPERIODSCP " &
                  " Where INTINVENTORYYEAR = @eiyear " &
                  " AND FACILITYSITEID = @fsid " &
                  " AND EMISSIONSUNITID = @euid " &
                  " AND PROCESSID = @epid " &
                  " AND SCPTYPECODE = @HCType "

            Dim params = {
                New SqlParameter("@eiyear", eiyear),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid),
                New SqlParameter("@HCType", HCType),
                New SqlParameter("@HCValue", HCValue),
                New SqlParameter("@HCNum", HCNum),
                New SqlParameter("@HCDen", HCDen),
                New SqlParameter("@uuser", uuser)
            }

            If DB.ValueExists(query, params) Then
                'Update HeatContent into SCP
                query = "Update EIS_PROCESSRPTPERIODSCP Set " &
                       " FLTSCPVALUE = @HCValue, " &
                       " STRSCPNUMERATOR = @HCNum, " &
                       " STRSCPDENOMINATOR = @HCDen, " &
                       " intSCPDataYear = @eiyear, " &
                       " UpdateUser = @uuser, " &
                       " UpdateDateTime = getdate()" &
                       " Where INTINVENTORYYEAR = @eiyear " &
                       " AND FACILITYSITEID = @fsid " &
                       " AND EMISSIONSUNITID = @euid " &
                       " AND PROCESSID = @epid " &
                       " AND SCPTYPECODE = @HCType "

            Else
                'Insert new HeatContent record into SCP table
                query = "Insert into EIS_PROCESSRPTPERIODSCP (" &
                           " INTINVENTORYYEAR," &
                           " FACILITYSITEID," &
                           " EMISSIONSUNITID," &
                           " PROCESSID," &
                           " RPTPERIODTYPECODE," &
                           " SCPTYPECODE," &
                           " FLTSCPVALUE," &
                           " STRSCPNUMERATOR," &
                           " STRSCPDENOMINATOR," &
                           " intSCPDataYear, " &
                           " ACTIVE," &
                           " UPDATEUSER," &
                           " UPDATEDATETIME," &
                           " CREATEDATETIME)" &
                       " Values (" &
                           " @eiyear, " &
                           " @fsid, " &
                           " @euid, " &
                           " @epid, " &
                           " 'A', " &
                           " @HCType, " &
                           " @HCValue, " &
                           " @HCNum, " &
                           " @HCDen, " &
                           " @eiyear, " &
                           " '1', " &
                           " @uuser," &
                           " getdate(), " &
                           " getdate())"
            End If

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub SaveSulfurContent(ByVal eiyear As Integer, ByVal fsid As String, ByVal euid As String, ByVal epid As String, ByVal uuser As String)
        Dim SCType As String = "Percent Sulfur Content"
        Dim SCValue As Double? = txtSulfurPct.Text.ParseAsNullableDouble()

        Try
            'Check if Sulfur Content exists
            Dim query = "Select SCPTYPECODE FROM EIS_PROCESSRPTPERIODSCP " &
                  " Where INTINVENTORYYEAR = @eiyear " &
                  " AND FACILITYSITEID = @fsid " &
                  " AND EMISSIONSUNITID = @euid " &
                  " AND PROCESSID = @epid " &
                  " AND SCPTYPECODE = @SCType "

            Dim params = {
                New SqlParameter("@eiyear", eiyear),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid),
                New SqlParameter("@uuser", uuser),
                New SqlParameter("@SCType", SCType),
                New SqlParameter("@SCValue", SCValue)
            }

            If DB.ValueExists(query, params) Then
                'Update Sulfur content into SCP table
                query = "Update EIS_PROCESSRPTPERIODSCP Set " &
                       " FLTSCPVALUE = @SCValue, " &
                       " UpdateUser = @uuser, " &
                       " UpdateDateTime = getdate() " &
                       " Where INTINVENTORYYEAR = @eiyear " &
                       " AND FACILITYSITEID = @fsid " &
                       " AND EMISSIONSUNITID = @euid " &
                       " AND PROCESSID = @epid " &
                       " AND SCPTYPECODE = @SCType "
            Else
                'Insert new Sulfur Content record into SCP table
                query = "Insert into EIS_PROCESSRPTPERIODSCP (" &
                           " INTINVENTORYYEAR," &
                           " FACILITYSITEID," &
                           " EMISSIONSUNITID," &
                           " PROCESSID," &
                           " RPTPERIODTYPECODE," &
                           " SCPTYPECODE," &
                           " FLTSCPVALUE," &
                           " ACTIVE," &
                           " UPDATEUSER," &
                           " UPDATEDATETIME," &
                           " CREATEDATETIME)" &
                       " Values (" &
                           " @eiyear," &
                           " @fsid, " &
                           " @euid, " &
                           " @epid, " &
                           " 'A'," &
                           " @SCType, " &
                           " @SCValue, " &
                           " '1'," &
                           " @uuser, " &
                           " getdate()," &
                           " getdate() ) "
            End If

            DB.RunCommand(query, params)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub SaveAshContent(ByVal eiyear As Integer, ByVal fsid As String, ByVal euid As String, ByVal epid As String, ByVal uuser As String)
        Dim ACType As String = "Percent Ash Content"
        Dim ACValue As Double? = txtAshPct.Text.ParseAsNullableDouble()

        Try
            'Check if Ash Content exists
            Dim query = "Select SCPTYPECODE FROM EIS_PROCESSRPTPERIODSCP " &
                  " Where INTINVENTORYYEAR = @eiyear " &
                  " AND FACILITYSITEID = @fsid" &
                  " AND EMISSIONSUNITID = @euid " &
                  " AND PROCESSID = @epid " &
                  " AND SCPTYPECODE = @ACType "

            Dim params = {
                New SqlParameter("@eiyear", eiyear),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid),
                New SqlParameter("@uuser", uuser),
                New SqlParameter("@ACType", ACType),
                New SqlParameter("@ACValue", ACValue)
            }

            If DB.ValueExists(query, params) Then
                'Update Ash Content if record exists
                query = "Update EIS_PROCESSRPTPERIODSCP Set " &
                       " FLTSCPVALUE = @ACValue, " &
                       " UpdateUser = @uuser, " &
                       " UpdateDateTime = getdate() " &
                       " Where INTINVENTORYYEAR = @eiyear " &
                       " AND FACILITYSITEID = @fsid " &
                       " AND EMISSIONSUNITID = @euid " &
                       " AND PROCESSID = @epid " &
                       " AND SCPTYPECODE = @ACType "
            Else
                'Insert new Ash Content record into SCP table
                query = "Insert into EIS_PROCESSRPTPERIODSCP (" &
                           " INTINVENTORYYEAR," &
                           " FACILITYSITEID," &
                           " EMISSIONSUNITID," &
                           " PROCESSID," &
                           " RPTPERIODTYPECODE," &
                           " SCPTYPECODE," &
                           " FLTSCPVALUE," &
                           " ACTIVE," &
                           " UPDATEUSER," &
                           " UPDATEDATETIME," &
                           " CREATEDATETIME)" &
                       " Values (" &
                           " @eiyear, " &
                           " @fsid, " &
                           " @euid, " &
                           " @epid, " &
                           " 'A', " &
                           " @ACType, " &
                           " @ACValue, " &
                           " '1', " &
                           " @uuser, " &
                           " getdate(), " &
                           " getdate() )"
            End If

            DB.RunCommand(query, params)

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

#Region "Button Routines"
    Protected Sub btnSumSeasonalPct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSumSeasonalPct.Click
        Dim TotalSeasons As Decimal

        lblMessageTop.Visible = False
        lblMessageBottom.Visible = False
        TotalSeasons = SumSeasonalPercentages()
        If TotalSeasons = 100 Then
            lblTotalSeasonValidate.Visible = False
        Else
            lblTotalSeasonValidate.Visible = True
            lblTotalSeasonValidate.Text = "Must equal 100%"
        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click, btnSave2.Click
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim InventoryYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim EmissionsUnitID As String = txtEmissionsUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        Dim SaveMessage As String = "Reporting Period Details saved."
        Dim NotSavedMessage1 As String = "Reporting Period Details not saved.  Please check the seasonal percentages."
        Dim TotalSeasons As Decimal

        lblTotalSeasonValidate.Visible = False
        lblMessageTop.Visible = False
        lblMessageBottom.Visible = False

        TotalSeasons = SumSeasonalPercentages()
        If TotalSeasons = 100 Then
            SaveReportingPeriod(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, UpdateUser)
            SaveRPDetails(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, UpdateUser)
            If ddlFuelBurning.SelectedValue = "Yes" Then
                SaveHeatContent(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, UpdateUser)
                If cbxSulfurNegligible.Checked = False Then
                    SaveSulfurContent(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, UpdateUser)
                Else
                    DeleteSulfurAsh(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, "SULFUR")
                End If
                If cbxAshNegligible.Checked = False Then
                    SaveAshContent(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, UpdateUser)
                Else
                    DeleteSulfurAsh(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, "ASH")
                End If

            Else 'Not fuel burning
                DeleteRPSCP(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID)
            End If

            lblMessageTop.Visible = True
            lblMessageBottom.Visible = True
            lblMessageTop.Text = SaveMessage
            lblMessageBottom.Text = SaveMessage
        Else
            lblTotalSeasonValidate.Visible = True
            lblMessageTop.Visible = True
            lblMessageBottom.Visible = True
            lblTotalSeasonValidate.Text = "Must equal 100%"
            lblMessageTop.Text = NotSavedMessage1
            lblMessageBottom.Text = NotSavedMessage1

        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel1.Click, btnCancel2.Click
        Dim eu As String = txtEmissionsUnitID.Text.ToUpper
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim targetpage As String = "~/EIS/rp_details.aspx" & "?ep=" & ep & "&eu=" & eu
        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnConfirmDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmDelete.Click
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim InventoryYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim EmissionsUnitID As String = txtEmissionsUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper

        DeleteRPProcessAndEmissions(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID)

        btnConfirmDelete.Visible = False
        btnCancelDelete.Visible = False
        btnSummary2.Visible = False
        Response.Redirect("~/EIS/rp_summary.aspx")

    End Sub

#End Region

    Protected Sub ddlFuelBurning_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFuelBurning.SelectedIndexChanged

        Dim FuelBurning As String = ddlFuelBurning.SelectedItem.Value

        If FuelBurning = "Yes" Then
            pnlFuelBurning.Visible = True
        Else
            pnlFuelBurning.Visible = False
            txtHeatContent.Text = ""
            ddlHeatContentDenUoM.SelectedIndex = 0
            txtAshPct.Text = ""
            txtSulfurPct.Text = ""
        End If

    End Sub

    Protected Sub cbxSulfur_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxSulfurNegligible.CheckedChanged

        If cbxSulfurNegligible.Checked Then
            txtSulfurPct.Text = ""
            txtSulfurPct.Enabled = False
            reqvSulfurPct.Enabled = False
        Else
            txtSulfurPct.Enabled = True
            reqvSulfurPct.Enabled = True
        End If

    End Sub

    Protected Sub cbxAsh_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cbxAshNegligible.CheckedChanged

        If cbxAshNegligible.Checked Then
            txtAshPct.Text = ""
            txtAshPct.Enabled = False
            reqvAshPct.Enabled = False
        Else
            txtAshPct.Enabled = True
            reqvAshPct.Enabled = True
        End If

    End Sub

End Class