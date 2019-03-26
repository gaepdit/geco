Partial Class EIS_rp_operscp_edit
    Inherits Page

    Private Property SelectedSCC As String = ""
    Private Property SelectedCalcParamUomCode As String = ""
    Private CalcParamUomCodesTable As DataTable = Nothing

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

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
            LoadCalcParamUoM()

            SumSeasonalPercentages()
        End If

    End Sub

#Region " Load Drop Down Lists "

    Private Sub LoadCalcParamTypeDDL()
        ddlCalcParamType.Items.Add("-- Select Type --")

        Dim dt As DataTable = GetCalcParamTypes()

        If dt IsNot Nothing Then
            For Each dr In dt.Rows
                Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("calcparatypecode")
                    }
                ddlCalcParamType.Items.Add(newListItem)
            Next
        End If
    End Sub

    Private Sub LoadCalcParamUoM()
        Dim reqExists As Boolean = True

        If Not String.IsNullOrEmpty(SelectedSCC) Then
            CalcParamUomCodesTable = GetCalcParamUoMCodesForScc(SelectedSCC)
        End If

        If CalcParamUomCodesTable Is Nothing OrElse CalcParamUomCodesTable.Rows.Count = 0 Then
            CalcParamUomCodesTable = GetCalcParamUoMCodes()
            reqExists = False
        End If

        With ddlCalcParamUoM
            .DataSource = CalcParamUomCodesTable
            .DataValueField = "Code"
            .DataTextField = "UnitOfMeasureDesc"
            .DataBind()
            .Items.Insert(0, "-- Select Units --")
            .SelectedIndex = 0
        End With

        If Not String.IsNullOrEmpty(SelectedCalcParamUomCode) AndAlso CalcParamUomCodesTable.Rows.Contains(SelectedCalcParamUomCode) Then
            ddlCalcParamUoM.SelectedValue = SelectedCalcParamUomCode

            If reqExists AndAlso CalcParamUomCodesTable.Rows.Count = 1 Then
                ddlCalcParamUoM.Enabled = False
            End If
        End If
    End Sub

    Private Sub LoadMaterialCodeDDL()
        ddlMaterialCode.Items.Add("-- Select Material --")

        Dim dt As DataTable = GetCalcParamMaterialCodes()

        If dt IsNot Nothing Then
            For Each dr In dt.Rows
                Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("calculatematerialcode")
                    }
                ddlMaterialCode.Items.Add(newListItem)
            Next
        End If
    End Sub

    Private Sub LoadFuelBurningDDL()
        ddlFuelBurning.Items.Add("No")
        ddlFuelBurning.Items.Add("Yes")
    End Sub

    Private Sub LoadHeatContentDenUoMDDL()
        ddlHeatContentDenUoM.Items.Add("-Select a Value-")

        Dim dt As DataTable = GetScpDenomUoMCodes()

        If dt IsNot Nothing Then
            For Each dr In dt.Rows
                Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("SCPDenomUOMCode")
                    }
                ddlHeatContentDenUoM.Items.Add(newListItem)
            Next
        End If
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

    Private Sub LoadRPDetails(Year As String, FSID As String, EUID As String, EPID As String)
        Dim HeatContent As String = ""
        Dim SulfurContent As String = ""
        Dim AshContent As String = ""

        Dim dr As DataRow = GetRPOperatingDetails(Year, FSID, EUID, EPID)

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
                SelectedSCC = ""
                txtSourceClassCode.Text = ""
                lblSccDesc.Text = ""
            Else
                SelectedSCC = dr.Item("SOURCECLASSCODE")
                txtSourceClassCode.Text = SelectedSCC

                If IsValidScc(SelectedSCC) Then
                    lblSccDesc.Text = GetSccDetails(SelectedSCC)?.Description
                End If
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
                SelectedCalcParamUomCode = dr.Item("CALCPARAMUOMCODE")
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
    End Sub

#Region "Save Routines"

    Private Sub SaveReportingPeriod(eiyear As Integer, fsid As String, euid As String, epid As String, uuser As String)
        Dim EmissionTypeCode As String = ddlCalcParamType.SelectedValue
        Dim CalcParamValue As Double = txtCalcParamValue.Text
        Dim CalcParamUoMCode As String = ddlCalcParamUoM.SelectedValue
        Dim MaterialCode As String = ddlMaterialCode.SelectedValue
        Dim RPComment As String = txtRPComment.Text

        Dim ActHrsPerPeriod As Integer? = txtActualHoursPerYear.Text.ParseAsNullableInteger()
        Dim AvgDaysPerWeek As Decimal? = txtAvgDaysPerWeek.Text.ParseAsNullableDecimal()
        Dim AvgHoursPerDay As Decimal? = txtAvgHoursPerDay.Text.ParseAsNullableDecimal()
        Dim AvgWeeksPerYear As Decimal? = txtAvgWeeksPerYear.Text.ParseAsNullableDecimal()
        Dim PctWinter As Decimal? = txtWinterPct.Text.ParseAsNullableDecimal()
        Dim PctSpring As Decimal? = txtSpringPct.Text.ParseAsNullableDecimal()
        Dim PctSummer As Decimal? = txtSummerPct.Text.ParseAsNullableDecimal()
        Dim PctFall As Decimal? = txtFallPct.Text.ParseAsNullableDecimal()

        SaveReportingPeriodData(eiyear, fsid, euid, epid, uuser, EmissionTypeCode,
                                CalcParamValue, CalcParamUoMCode, MaterialCode, RPComment,
                                ActHrsPerPeriod, AvgDaysPerWeek, AvgHoursPerDay, AvgWeeksPerYear,
                                PctWinter, PctSpring, PctSummer, PctFall)
    End Sub

    Private Sub SaveHeatContent(eiyear As Integer, fsid As String, euid As String, epid As String, uuser As String)
        Dim HCType As String = "Heat Content"
        Dim HCValue As Double? = txtHeatContent.Text.ParseAsNullableDouble()
        Dim HCNum As String = "E6BTU"
        Dim HCDen As String = ddlHeatContentDenUoM.SelectedValue

        SaveRptPeriodScp(eiyear, fsid, euid, epid, uuser, HCType, HCValue, HCNum, HCDen)
    End Sub

    Private Sub SaveSulfurContent(eiyear As Integer, fsid As String, euid As String, epid As String, uuser As String)
        Dim SCType As String = "Percent Sulfur Content"
        Dim SCValue As Double? = txtSulfurPct.Text.ParseAsNullableDouble()

        SaveRptPeriodScp(eiyear, fsid, euid, epid, uuser, SCType, SCValue)
    End Sub

    Private Sub SaveAshContent(eiyear As Integer, fsid As String, euid As String, epid As String, uuser As String)
        Dim ACType As String = "Percent Ash Content"
        Dim ACValue As Double? = txtAshPct.Text.ParseAsNullableDouble()

        SaveRptPeriodScp(eiyear, fsid, euid, epid, uuser, ACType, ACValue)
    End Sub

#End Region

#Region "Button Routines"
    Protected Sub btnSumSeasonalPct_Click(sender As Object, e As EventArgs) Handles btnSumSeasonalPct.Click
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

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave1.Click, btnSave2.Click
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

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel1.Click, btnCancel2.Click
        Dim eu As String = txtEmissionsUnitID.Text.ToUpper
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim targetpage As String = "~/EIS/rp_details.aspx" & "?ep=" & ep & "&eu=" & eu
        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnConfirmDelete_Click(sender As Object, e As EventArgs) Handles btnConfirmDelete.Click
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

    Protected Sub ddlFuelBurning_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFuelBurning.SelectedIndexChanged
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

    Protected Sub cbxSulfur_CheckedChanged(sender As Object, e As EventArgs) Handles cbxSulfurNegligible.CheckedChanged
        If cbxSulfurNegligible.Checked Then
            txtSulfurPct.Text = ""
            txtSulfurPct.Enabled = False
            reqvSulfurPct.Enabled = False
        Else
            txtSulfurPct.Enabled = True
            reqvSulfurPct.Enabled = True
        End If
    End Sub

    Protected Sub cbxAsh_CheckedChanged(sender As Object, e As EventArgs) Handles cbxAshNegligible.CheckedChanged
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