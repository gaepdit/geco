Imports System.Data.SqlClient
Imports EpdIt.DBUtilities

Partial Class EIS_rp_details
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim CkYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim QYear As String = Request.QueryString("yr")
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EmissionsUnitID As String = Request.QueryString("eu")
        Dim ProcessID As String = Request.QueryString("ep")
        Dim RptPeriodTypeCode As String = "A"

        EIAccessCheck(EISAccessCode, EISStatus)

        If CheckProcessExist(FacilitySiteID, EmissionsUnitID, ProcessID) <> UnitActiveStatus.Active Then
            HttpContext.Current.Response.Redirect("~/EIS/rp_summary.aspx")
        End If

        If Not IsPostBack Then
            Dim InventoryYear As String = If(QYear, CkYear)
            txtEISYear.Text = InventoryYear

            LoadRPDetails(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, RptPeriodTypeCode)
            LoadPollutantsGVW(InventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID)

            If gvwPollutants.Rows.Count > 0 Then
                lblSummerDayNote.Text = "* Summer Day emissions = emissions on an average summer day May&nbsp;1 through Sep&nbsp;30. Units are in tons per day (TPD)"
                lblSummerDayNote.Visible = True
            Else
                lblSummerDayNote.Text = ""
                lblSummerDayNote.Visible = False
            End If

            SetEIAccess(EISAccessCode)
        End If

    End Sub

    Private Sub SetEIAccess(ByVal acc As String)
        Select Case acc
            Case 1
                btnSummary1.Visible = True
                btnSummary2.Visible = True
                btnProcess1.Visible = False
                btnProcess2.Visible = False
            Case Else
                btnSummary1.Visible = False
                btnSummary2.Visible = False
                btnProcess1.Visible = True
                btnProcess2.Visible = True
        End Select

    End Sub

    Private Sub LoadPollutantsGVW(ByVal EIYR As String, ByVal FSID As String, ByVal EUID As String, ByVal EPID As String)

        Dim query = " select " &
            "     EMISSIONSUNITID, " &
            "     PROCESSID, " &
            "     POLLUTANTCODE, " &
            "     STRPOLLUTANT, " &
            "     CASE WHEN RPTPERIODTYPECODE = 'O3D' " &
            "         THEN 'Summer Day*' " &
            "     ELSE 'Annual' " &
            "     END RPTPeriodType, " &
            "     CASE WHEN RPTPERIODTYPECODE = 'O3D' " &
            "         THEN 'TPD' " &
            "     ELSE 'TPY' " &
            "     END PollutantUnit, " &
            "     FLTTOTALEMISSIONS, " &
            "     LASTEISSUBMITDATE " &
            " FROM VW_EIS_RPEMISSIONS " &
            " where INTINVENTORYYEAR = @EIYR " &
            "       and FACILITYSITEID = @FSID " &
            "       and EMISSIONSUNITID = @EUID " &
            "       and PROCESSID = @EPID " &
            " order by EMISSIONSUNITID, PROCESSID, POLLUTANTCODE, RPTPeriodType "

        Dim params = {
            New SqlParameter("@EIYR", EIYR),
            New SqlParameter("@FSID", FSID),
            New SqlParameter("@EUID", EUID),
            New SqlParameter("@EPID", EPID)
        }

        gvwPollutants.DataSource = DB.GetDataTable(query, params)
        gvwPollutants.DataBind()

    End Sub

    Private Sub LoadRPDetails(ByVal Year As String, ByVal FSID As String, ByVal EUID As String, ByVal EPID As String, ByVal RPTYPE As String)
        Dim HeatContent As String
        Dim SulfurContent As String
        Dim AshContent As String
        Dim UpdateUser As String
        Dim UpdateDateTime As String
        Dim Negligible As String = "Zero or negligible"

        Try
            Dim query = " select " &
            "     INTINVENTORYYEAR, " &
            "     EMISSIONSUNITID, " &
            "     STRUNITDESCRIPTION, " &
            "     PROCESSID, " &
            "     STRPROCESSDESCRIPTION, " &
            "     FLTCALCPARAMETERVALUE, " &
            "     STRCPUOMDESC, " &
            "     STRCPTYPEDESC, " &
            "     STRMATERIAL, " &
            "     STRREPORTINGPERIODCOMMENT, " &
            "     INTACTUALHOURSPERPERIOD, " &
            "     SOURCECLASSCODE, " &
            "     NUMAVERAGEDAYSPERWEEK, " &
            "     NUMAVERAGEHOURSPERDAY, " &
            "     NUMAVERAGEWEEKSPERPERIOD, " &
            "     NUMPERCENTWINTERACTIVITY, " &
            "     NUMPERCENTSPRINGACTIVITY, " &
            "     NUMPERCENTSUMMERACTIVITY, " &
            "     NUMPERCENTFALLACTIVITY, " &
            "     HEATCONTENT, " &
            "     case " &
            "     when HCNUMER = 'E6BTU' " &
            "         then 'MILLION BTU' " &
            "     else HCNUMER " &
            "     end                                   as HCNUMER, " &
            "     c.STRDESC                             as HCDenom, " &
            "     ASHCONTENT, " &
            "     SULFURCONTENT, " &
            "     convert(char, d.UpdateDateTime, 20)   As UpdateDateTime, " &
            "     convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
            "     d.UpdateUser " &
            " FROM VW_EIS_RPDETAILS d " &
            "     left join EISLK_SCPDENOMUOMCODE c " &
            "         on d.HCDENOM = c.SCPDENOMUOMCODE " &
            " where INTINVENTORYYEAR = @Year " &
            "       and FACILITYSITEID = @FSID " &
            "       and EMISSIONSUNITID = @EUID " &
            "       and PROCESSID = @EPID " &
            "       and RPTPERIODTYPECODE = @RPTYPE "

            Dim params = {
                New SqlParameter("@Year", Year),
                New SqlParameter("@FSID", FSID),
                New SqlParameter("@EUID", EUID),
                New SqlParameter("@EPID", EPID),
                New SqlParameter("@RPTYPE", RPTYPE)
            }

            Dim dr = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then
                'Emission Unit and Process Descriptions
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
                    lblSccDesc.Text = ""
                Else
                    txtSourceClassCode.Text = dr.Item("SOURCECLASSCODE")

                    If IsValidScc(dr.Item("SOURCECLASSCODE")) Then
                        lblSccDesc.Text = GetSccDetails(dr.Item("SOURCECLASSCODE"))?.Description
                    End If
                End If

                'Begin Process Operating Details
                If IsDBNull(dr("LastEISSubmitDate")) Then
                    txtLastEISSubmit.Text = "Never submitted"
                Else
                    txtLastEISSubmit.Text = dr.Item("LastEISSubmitDate")
                End If
                If IsDBNull(dr("UpdateUser")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr.Item("UpdateUser")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr("UpdateDateTime")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr.Item("UpdateDateTime")
                End If
                txtLastUpdated.Text = UpdateDateTime & " by " & UpdateUser

                'Process Details
                If IsDBNull(dr("STRCPTYPEDESC")) Then
                    txtCalcParamType.Text = ""
                Else
                    txtCalcParamType.Text = dr.Item("STRCPTYPEDESC")
                End If
                If IsDBNull(dr("FLTCALCPARAMETERVALUE")) Then
                    txtCalcParamValue.Text = ""
                Else
                    txtCalcParamValue.Text = dr.Item("FLTCALCPARAMETERVALUE")
                End If
                If IsDBNull(dr("STRCPUOMDESC")) Then
                    txtCalcParamUoM.Text = ""
                Else
                    txtCalcParamUoM.Text = dr.Item("STRCPUOMDESC")
                End If
                If IsDBNull(dr("STRMATERIAL")) Then
                    txtCalcMaterial.Text = ""
                Else
                    txtCalcMaterial.Text = dr.Item("STRMATERIAL")
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
                    txtWinterPct.Text = ""
                Else
                    txtWinterPct.Text = dr.Item("NUMPERCENTWINTERACTIVITY")
                End If
                If IsDBNull(dr("NUMPERCENTSPRINGACTIVITY")) Then
                    txtSpringPct.Text = ""
                Else
                    txtSpringPct.Text = dr.Item("NUMPERCENTSPRINGACTIVITY")
                End If
                If IsDBNull(dr("NUMPERCENTSUMMERACTIVITY")) Then
                    txtSummerPct.Text = ""
                Else
                    txtSummerPct.Text = dr.Item("NUMPERCENTSUMMERACTIVITY")
                End If
                If IsDBNull(dr("NUMPERCENTFALLACTIVITY")) Then
                    txtFallPct.Text = ""
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
                Else
                    SulfurContent = dr.Item("SULFURCONTENT")
                End If
                If IsDBNull(dr("ASHCONTENT")) Then
                    AshContent = ""
                Else
                    AshContent = dr.Item("ASHCONTENT")
                End If

                'SCP Parameter Details
                If String.IsNullOrEmpty(HeatContent) AndAlso String.IsNullOrEmpty(SulfurContent) AndAlso String.IsNullOrEmpty(AshContent) Then
                    txtFuelUsage.Text = "No"
                    pnlFuelBurning.Visible = False
                Else
                    txtFuelUsage.Text = "Yes"
                    txtHeatContent.Text = HeatContent
                    txtSulfurPct.Text = SulfurContent
                    If String.IsNullOrEmpty(SulfurContent) Then txtSulfurPct.Text = Negligible
                    txtAshPct.Text = AshContent
                    If String.IsNullOrEmpty(AshContent) Then txtAshPct.Text = Negligible
                    txtHeatContentNumUoM.Text = GetNullableString(dr.Item("HCNUMER"))
                    txtHeatContentDenUoM.Text = GetNullableString(dr.Item("HCDENOM"))
                    pnlFuelBurning.Visible = True
                End If
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnProcess1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcess1.Click, btnProcess2.Click
        Dim eu As String = txtEmissionsUnitID.Text.ToUpper
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim targetpage As String = "~/EIS/process_details.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)
    End Sub

End Class