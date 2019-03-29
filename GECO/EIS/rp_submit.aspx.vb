Imports System.Data.SqlClient

Partial Class EIS_rp_submit
    Inherits Page

    Private ErrorsExist As Boolean = False
    Private WarningsExist As Boolean = False

    Private FacilitySiteID As String
    Private EIYear As String
    Private EISStatus As String
    Private EISAccessCode As String

    Private UpdateUserID As String
    Private UpdateUserName As String
    Private UpdateUser As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FacilitySiteID = GetCookie(Cookie.AirsNumber)
        EIYear = GetCookie(EisCookie.EISMaxYear)
        EISStatus = GetCookie(EisCookie.EISStatus)
        EISAccessCode = GetCookie(EisCookie.EISAccess)
        UpdateUserID = GetCookie(GecoCookie.UserID)
        UpdateUserName = GetCookie(GecoCookie.UserName)
        UpdateUser = UpdateUserID & "-" & UpdateUserName

        EIAccessCheck(EISAccessCode, EISStatus)

        If Not IsPostBack Then
            LoadgvwEISErrorsWarnings()
            lblEISCheckResults.Visible = False
            pnlStart.Visible = True
            pnlErrorPresent.Visible = False
            pnlNoErrors.Visible = False
            pnlErrorList_Outer.Visible = False
            pnlWarningList_Outer.Visible = False
        End If

    End Sub

#Region "  Submit Routine  "

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        ErrorsExist = False
        WarningsExist = False

        pnlGeneralErrors.Visible = False
        pnlStart.Visible = False
        pnlErrorPresent.Visible = False
        pnlNoErrors.Visible = False
        pnlErrorList_Outer.Visible = False
        pnlWarningList_Outer.Visible = False

        CheckEIData(FacilitySiteID, EIYear)

        pnlErrorPresent.Visible = ErrorsExist
        pnlErrorList_Outer.Visible = ErrorsExist
        pnlWarningList_Outer.Visible = WarningsExist
        pnlNoErrors.Visible = Not ErrorsExist

        If ErrorsExist Then
            lblMessage1.Text = "Errors and/or warnings were found in the data. Address the errors and warnings as necessary and submit again."
            lblMessage1.Visible = True
            lblMessage2.Visible = False
        ElseIf WarningsExist Then
            lblMessage1.Text = "Congratulations! No errors were found. However, please review any warnings and fix as necessary. You may click the button below to submit the data."
            lblMessage1.ForeColor = Drawing.Color.Orange
            lblMessage1.Visible = True
            lblMessage2.Visible = False
        Else
            lblMessage1.Visible = False
            lblMessage2.Text = "Congratulations! No errors or warnings were found in this check. Click the button below to submit the data."
            lblMessage2.Visible = True
        End If

        lblEISCheckResults.Visible = True

    End Sub

    Protected Sub btnConfSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfSubmit.Click

        EISubmit(FacilitySiteID, EIYear, UpdateUser)
        ResetCookies(FacilitySiteID)
        lblMessage2.Text = "The facility's Emissions Inventory data for " & EIYear & " have been submitted. Click continue ..."
        lblMessage2.Visible = True
        btnContinue.Visible = True
        btnConfSubmit.Visible = False

        HideFacilityInventoryMenu()
        HideEmissionInventoryMenu()

        EmailNotificationsToECSU()

    End Sub

    Private Function EISubmit(ByVal fsid As String, ByVal eiyr As Integer, ByVal uuser As String) As Boolean

        Dim eisAccessCode As String = "2"
        Dim eisStatusCode As String = "3"

        Dim query = "select datFinalize FROM eis_Admin where " &
            " FacilitySiteID = @fsid and InventoryYear = @eiyr and datInitialFinalize is not null "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@eiyr", eiyr)
        }

        If DB.ValueExists(query, params) Then
            query = "update eis_Admin set " &
                " eisAccessCode = @eisAccessCode, " &
                " eisStatusCode = @eisStatusCode, " &
                " datEISStatus = getdate(), " &
                " strConfirmationNumber = Next Value for EIS_SEQ_ConfNum, " &
                " datFinalize = getdate(), " &
                " UpdateUser = @uuser, " &
                " UpdateDateTime = getdate() " &
                " where " &
                " FacilitySiteID = @fsid and " &
                " InventoryYear = @eiyr "
        Else
            query = "update eis_Admin set " &
                " eisAccessCode = @eisAccessCode, " &
                " eisStatusCode = @eisStatusCode, " &
                " datEISStatus = getdate(), " &
                " strConfirmationNumber = Next Value for EIS_SEQ_ConfNum, " &
                " datInitialFinalize = getdate(), " &
                " datFinalize = getdate(), " &
                " UpdateUser = @uuser, " &
                " UpdateDateTime = getdate() " &
                " where " &
                " FacilitySiteID = @fsid and " &
                " InventoryYear = @eiyr "
        End If

        Dim params2 As SqlParameter() = {
            New SqlParameter("@eisAccessCode", eisAccessCode),
            New SqlParameter("@eisStatusCode", eisStatusCode),
            New SqlParameter("@uuser", uuser),
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@eiyr", eiyr)
        }

        Return DB.RunCommand(query, params2)

    End Function

    Private Sub ResetCookies(ByVal fsid As String)

        Dim EISCookies As New HttpCookie("EISAccessInfo")
        Dim EIYear As String = ""
        Dim enrolled As String = ""
        Dim eisStatus As String = ""
        Dim accesscode As String = ""
        Dim optout As String = ""
        Dim dateFinalize As String = ""
        Dim confirmationnumber As String = ""
        Dim CurrentEIYear As Integer = Now.Year - 1

        Try
            Dim query = "Select eis_admin.FacilitySiteID, eis_admin.InventoryYear, " &
                "EIS_Admin.EISStatusCode, EIS_Admin.datEISStatus, " &
                "EIS_Admin.EISAccessCode, EIS_Admin.strOptOut, " &
                "EIS_Admin.strEnrollment, EIS_Admin.datFinalize, " &
                "EIS_Admin.strConfirmationNumber FROM EIS_Admin, " &
                "(select max(inventoryYear) as MaxYear, " &
                "EIS_Admin.FacilitySiteID " &
                "FROM EIS_Admin GROUP BY EIS_Admin.FacilitySiteID ) MaxResults  " &
                "where EIS_Admin.FacilitySiteID = @fsid " &
                "and EIS_Admin.inventoryYear = maxresults.maxyear " &
                "and EIS_Admin.FacilitySiteID = maxresults.FacilitySiteID " &
                "group by EIS_Admin.FacilitySiteID, " &
                "EIS_Admin.inventoryYear, " &
                "EIS_Admin.EISStatusCode, EIS_Admin.datEISStatus, " &
                "EIS_Admin.EISAccessCode, EIS_Admin.strOptOut, " &
                "EIS_Admin.strEnrollment, EIS_Admin.datFinalize, " &
                "EIS_Admin.strConfirmationNumber"

            Dim param As New SqlParameter("@fsid", fsid)

            Dim dr = DB.GetDataRow(query, param)

            If dr Is Nothing Then
                'Set EISAccess cookie to "3" id facility does not exist in EIS Admin table
                EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText("3")
            Else
                'get max year from EIS Admin table
                If IsDBNull(dr("InventoryYear")) Then
                    'Do nothing - leave EISMaxYear null
                Else
                    EIYear = dr.Item("InventoryYear")
                End If
                EISCookies.Values("EISMaxYear") = EncryptDecrypt.EncryptText(EIYear)

                If EIYear = CurrentEIYear Then
                    'Check enrollment
                    'get enrollment status: 0 = not enrolled; 1 = enrolled for EI year
                    If IsDBNull(dr("strEnrollment")) Then
                        enrolled = "NULL"
                    Else
                        enrolled = dr.Item("strEnrollment")
                    End If
                    EISCookies.Values("Enrollment") = EncryptDecrypt.EncryptText(enrolled)

                    If enrolled = "1" Then
                        'getEISStatus for EISMaxYear
                        If IsDBNull(dr("EISStatusCode")) Then
                            eisStatus = "NULL"
                        Else
                            eisStatus = dr.Item("EISStatusCode")
                        End If
                        EISCookies.Values("EISStatus") = EncryptDecrypt.EncryptText(eisStatus)

                        'get EIS Access Code from database
                        If IsDBNull(dr("EISAccessCode")) Then
                            accesscode = "NULL"
                        Else
                            accesscode = dr.Item("EISAccessCode")
                        End If
                        EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText(accesscode)

                        If IsDBNull(dr("strOptOut")) Then
                            optout = "NULL"
                        Else
                            optout = dr.Item("strOptOut")
                        End If
                        EISCookies.Values("OptOut") = EncryptDecrypt.EncryptText(optout)

                        If IsDBNull(dr("datFinalize")) Then
                            dateFinalize = "NULL"
                        Else
                            dateFinalize = dr.Item("datFinalize")
                        End If
                        EISCookies.Values("DateFinalize") = EncryptDecrypt.EncryptText(dateFinalize)

                        If IsDBNull(dr("strConfirmationNumber")) Then
                            confirmationnumber = "NULL"
                        Else
                            confirmationnumber = dr.Item("strConfirmationNumber")
                        End If
                        EISCookies.Values("ConfNumber") = EncryptDecrypt.EncryptText(confirmationnumber)
                    End If
                Else
                    EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText("0")
                End If
            End If

            EISCookies.Expires = DateTime.Now.AddHours(8)
            Response.Cookies.Add(EISCookies)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

#Region "  Submit Checks  "

    Private Sub CheckEIData(ByVal fsid As String, ByVal eiyr As Integer)
        Dim ds As DataSet = GetEISSubmittalErrors(fsid, eiyr)

        If ds.Tables.Count > 0 Then
            ' Recordset  0 - E01 - No Processes have been added to the Reporting Period.
            CheckRPDataExist(ds.Tables(0))

            ' Recordset  1 - E02 - Missing or invalid Fuel Burning Emission Unit information.
            CheckEmissionUnit(ds.Tables(1))

            ' Recordset  2 - E03 - One or more missing or invalid stack parameters. See Stack Release Points in Facility Inventory Help for details.
            CheckStackErrors(ds.Tables(2))

            ' Recordset  3 - E04 - Missing required Fugitive Release Point and Fugitive Release Point Geographical information.
            CheckFugitiveErrors(ds.Tables(3))

            ' Recordset  4 - E21 - Missing/Invalid Process Throughput Values or Units for given Process Reporting Period Details.
            ProcessThroughputNull(ds.Tables(4))

            ' Recordset  5 - E23 - Invalid pollutant or missing Emissions value for a given pollutant.
            CheckEmissionsNull(ds.Tables(5))

            ' Recordset  6 - E25 - Summer Day emission quantity missing. Process operates during summer, therefore summer day emission expected.
            CheckSummerDayExist(ds.Tables(6))

            ' Recordset  7 - E24 - Emission Calculation Method or Emission Factor need correction.
            CheckEFFactor(ds.Tables(7))

            ' Recordset  8 - E26 - Seasonal Operating Parameters do not total 100% and/or missing operating average values.
            CheckSeasonTotals(ds.Tables(8))

            ' Recordset  9 - E27 - Errors in Particulate Emissions values.
            CheckPMTotalEmissionsError(ds.Tables(9))

            ' Recordset 10 - E08 - Multiple Stack release points have the same geographic coordinates.
            CheckStackRPGeoCoords(ds.Tables(10))

            ' Recordset 11 - E09 - Multiple Fugitive release points have the same geographic coordinates.
            CheckFugitiveRPGeoCoords(ds.Tables(11))

            ' Recordset 12 - E22 - Missing the Process Calculation Parameter Type. Must indicate Input or Output.
            CheckProcessCalcParamType(ds.Tables(12))

            ' Recordset 13 - E10 - Source Classification Code (SCC) is either not entered or not valid.
            CheckProcessSCC(ds.Tables(13))

            ' Recordset 14 - E28 - No emissions have been reported for the following processes which are included in the reporting period.
            CheckRepPeriodMissingEmissions(ds.Tables(14))

            ' Recordset 15 - E05 - Release Point Apportionment sum not equal 100%.
            CheckRPApportionmentTotal(ds.Tables(15))

            ' Recordset 16 - E20 - Emission units operating but not in Reporting Period.
            CheckEmissionUnitsNotInRptPeriod(ds.Tables(16))

            ' Recordset 17 - E06 - Emission Unit Control Approach has no Control Measure or has no Pollutant.
            CheckEmissionUnitCtrlApp(ds.Tables(17))

            ' Recordset 18 - E07 - Process Control Approach has no Control Measure or has no Pollutant.
            CheckProcessCtrlApp(ds.Tables(18))

            ' Recordset 19 - E06b - Emission Unit Control Approach data out of range.
            CheckEmissionUnitCtrlAppData(ds.Tables(19))

            ' Recordset 20 - E07b - Process Control Approach data out of range.
            CheckProcessCtrlAppData(ds.Tables(20))

            ' Recordset 21 - E06c - Emission Unit Control Approach Pollutant data out of range.
            CheckEmissionUnitCtrlAppPollData(ds.Tables(21))

            ' Recordset 22 - E07c - Process Control Approach Pollutant data out of range.
            CheckProcessCtrlAppPollData(ds.Tables(22))

            ' Recordset 23 - E34a - If there is a PM2.5 control pollutant there must be a PM10 control pollutant.
            CheckPMCtrlPollutantDependancy(ds.Tables(23))

            ' Recordset 24 - E29 - Emission Unit Type is missing or invalid.
            CheckEmissionUnitType(ds.Tables(24))

            ' Recordset 25 - E26a - Process Operating Details data out of range.
            CheckOperatingDetailsAppData(ds.Tables(25))

            ' Recordset 26 - E30 - NAICS code is missing or invalid.
            CheckE30(ds.Tables(26))

            ' Recordset 27 - E31 - Emission unit status code is missing or invalid
            CheckE31(ds.Tables(27))

            ' Recordset 28 - E32 - Emission unit operation date is invalid.
            CheckE32(ds.Tables(28))

            ' Recordset 29 - E33 - Emission unit type code is missing or invalid.
            CheckE33(ds.Tables(29))

            ' Recordset 30 - E34b - If there is a PM2.5 control pollutant there must be a PM10 control pollutant.
            CheckE34b(ds.Tables(30))

            ' Recordset 31 - E35 - Invalid Facility Site Status or missing Site Status Code Year
            CheckE35(ds.Tables(31))

            ' Recordset 32 - E36 - Missing facility site address
            CheckE36(ds.Tables(32))

            ' Recordset 33 - E37 - Missing facility geographic coordinates
            CheckE37(ds.Tables(33))

            ' Recordset 34 - E03b - One or more missing or invalid release point parameters. See Release Points in Facility Inventory Help for details.
            CheckE03b(ds.Tables(34))

            ' Recordset 35 - E38 - Invalid fuel burning information
            CheckE38(ds.Tables(35))

            ' Recordset 39 - E39 - Process has invalid throughput units for the selected Source Classification Code.
            CheckE39(ds.Tables(39))

            'Check Warnings
            CheckPMTotalEmissionsWarning(ds.Tables(36))
            CheckFugitiveRPWarnings(ds.Tables(37))
            Check20PercentChange(ds.Tables(38))
        End If

    End Sub

    ' >>>>>>>>>>>>  Error Check Subs Begin

    Private Sub CheckRPDataExist(ByVal dtList As DataTable)
        'Checks if any records in EIS_ReportingPeriod for the inventory year
        Try
            If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
                lblNoRPData.Text = ""
                hlnkNORPData.Visible = False
            Else
                lblNoRPData.Text = "E01 - No Processes have been added to the Reporting Period."
                hlnkNORPData.Visible = True

                pnlGeneralErrors.Visible = True
                ErrorsExist = True
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub CheckEmissionUnit(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionUnitErrors.Text = "E02 - Missing or invalid Fuel Burning Emission Unit information."
            gvwEmissionUnit.DataSource = dtList
            gvwEmissionUnit.DataBind()
            ErrorsExist = True
        Else
            lblEmissionUnitErrors.Text = ""
        End If
    End Sub

    Private Sub CheckStackErrors(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblStackErrors.Text = "E03 - One or more missing or invalid stack parameters. See Stack Release Points in Facility Inventory Help for details."
            gvwStackErrors.DataSource = dtList
            gvwStackErrors.DataBind()
            ErrorsExist = True
        Else
            lblStackErrors.Text = ""
        End If
    End Sub

    Private Sub CheckFugitiveErrors(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblFugitiveErrors.Text = "E04 - Missing required Fugitive Release Point and Fugitive Release Point Geographical information."
            gvwFugitiveErrors.DataSource = dtList
            gvwFugitiveErrors.DataBind()
            ErrorsExist = True
        Else
            lblFugitiveErrors.Text = ""
        End If
    End Sub

    Private Sub ProcessThroughputNull(ByVal dtList As DataTable)
        'Checks for any Process Throughput values that are null
        ' EIS QA Checks 394, 395
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblProcessThroughputNullErrors.Text = "E21 - Missing/Invalid Process Throughput Values or Units for given Process Reporting Period Details."
            gvwProcessThroughputNullErrors.DataSource = dtList
            gvwProcessThroughputNullErrors.DataBind()
            ErrorsExist = True
        Else
            lblProcessThroughputNullErrors.Text = ""
        End If
    End Sub

    Private Sub CheckEmissionsNull(ByVal dtList As DataTable)
        'Checks if annual emissions for pollutant from emission unit/process/pollutant is null
        ' EIS QA Checks 394, 395
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionNullErrors.Text = "E23 - Invalid pollutant or missing Emissions value for a given pollutant."
            gvwEmissionNullErrors.DataSource = dtList
            gvwEmissionNullErrors.DataBind()
            ErrorsExist = True
        Else
            lblEmissionNullErrors.Text = ""
        End If
    End Sub

    Private Sub CheckSummerDayExist(ByVal dtList As DataTable)

        'VW_EIS_SummerDay_Chk contains records meeting the criteria for this error.
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblSummerDay.Text = "E25 - Summer Day emission quantity missing. Process operates during summer, therefore summer day emission expected."
            gvwSummerDay.DataSource = dtList
            gvwSummerDay.DataBind()
            ErrorsExist = True
        Else
            lblSummerDay.Text = ""
        End If
    End Sub

    Private Sub CheckEFFactor(ByVal dtList As DataTable)
        ' EIS QA Checks 394, 395
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionFactors.Text = "E24 - Emission Calculation Method or Emission Factor need correction."
            gvwEmissionFactors.DataSource = dtList
            gvwEmissionFactors.DataBind()
            ErrorsExist = True
        Else
            lblEmissionFactors.Text = ""
        End If
    End Sub

    Private Sub CheckSeasonTotals(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblSeasonTotals.Text = "E26 - Seasonal Operating Parameters do not total 100% and/or missing operating average values."
            gvwSeasonTotals.DataSource = dtList
            gvwSeasonTotals.DataBind()
            ErrorsExist = True
        Else
            lblSeasonTotals.Text = ""
        End If
    End Sub

    Private Sub CheckPMTotalEmissionsError(ByVal dtList As DataTable)
        'Checks if annual emissions for pollutant from emission unit/process/pollutant is null
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblPMTotalEmissionsError.Text = "E27 - Error(s) in Particulate Emissions values."
            gvwPMTotalEmissionsError.DataSource = dtList
            gvwPMTotalEmissionsError.DataBind()
            ErrorsExist = True
        Else
            lblPMTotalEmissionsError.Text = ""
        End If
    End Sub

    Private Sub CheckStackRPGeoCoords(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblStackRPGeoCoordDupes.Text = "E08 - Multiple Stack release points have the same geographic coordinates."
            gvwStackRPGeoCoordDupes.DataSource = dtList
            gvwStackRPGeoCoordDupes.DataBind()
            ErrorsExist = True
        Else
            lblStackRPGeoCoordDupes.Text = ""
        End If
    End Sub

    Private Sub CheckFugitiveRPGeoCoords(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblFugitiveRPGeoCoordDupes.Text = "E09 - Multiple Fugitive release points have the same geographic coordinates."
            gvwFugitiveRPGeoCoordDupes.DataSource = dtList
            gvwFugitiveRPGeoCoordDupes.DataBind()
            ErrorsExist = True
        Else
            lblFugitiveRPGeoCoordDupes.Text = ""
        End If
    End Sub

    Private Sub CheckProcessCalcParamType(ByVal dtList As DataTable)
        'Checks if Process Calc Parameter is null or "Existing"
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblProcessCalcParamType.Text = "E22 - Missing the Process Calculation Parameter Type. Must indicate Input or Output."
            gvwProcessCalcParamType.DataSource = dtList
            gvwProcessCalcParamType.DataBind()
            ErrorsExist = True
        Else
            lblProcessCalcParamType.Text = ""
        End If
    End Sub

    Private Sub CheckProcessSCC(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblProcessSCC.Text = "E10 - Source Classification Code (SCC) is either not entered or not valid."
            gvwProcessSCC.DataSource = dtList
            gvwProcessSCC.DataBind()
            ErrorsExist = True
        Else
            lblProcessSCC.Text = ""
        End If
    End Sub

    Private Sub CheckRepPeriodMissingEmissions(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionsNotReported.Text = "E28 - No emissions have been reported for the following processes which are included in the reporting period."
            gvwEmissionsNotReported.DataSource = dtList
            gvwEmissionsNotReported.DataBind()
            ErrorsExist = True
        Else
            lblEmissionsNotReported.Text = ""
        End If
    End Sub

    Private Sub CheckRPApportionmentTotal(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblRPApportionmentErrors.Text = "E05 - Release Point Apportionment sum not equal 100%."
            gvwRPApportionmentTotals.DataSource = dtList
            gvwRPApportionmentTotals.DataBind()
            ErrorsExist = True
        Else
            lblRPApportionmentErrors.Text = ""
        End If
    End Sub

    Private Sub CheckEmissionUnitsNotInRptPeriod(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionUnitsNotInRptPeriod.Text = "E20 - Emission units operating but not in Reporting Period."
            gvwEmissionUnitsNotInRptPeriod.DataSource = dtList
            gvwEmissionUnitsNotInRptPeriod.DataBind()
            ErrorsExist = True
        Else
            lblEmissionUnitsNotInRptPeriod.Text = ""
        End If
    End Sub

    Private Sub CheckEmissionUnitCtrlApp(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionUnitCtrlApp.Text = "E06 - Emission Unit Control Approach has no Control Measure or has no Pollutant."
            gvwEmissionUnitCtrlApp.DataSource = dtList
            gvwEmissionUnitCtrlApp.DataBind()
            ErrorsExist = True
        Else
            lblEmissionUnitCtrlApp.Text = ""
        End If
    End Sub

    Private Sub CheckProcessCtrlApp(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblProcessCtrlApp.Text = "E07 - Process Control Approach has no Control Measure or has no Pollutant."
            gvwProcessCtrlApp.DataSource = dtList
            gvwProcessCtrlApp.DataBind()
            ErrorsExist = True
        Else
            lblProcessCtrlApp.Text = ""
        End If
    End Sub

    Private Sub CheckEmissionUnitCtrlAppData(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionUnitCtrlAppData.Text = "E06b - Emission Unit Control Approach data out of range."
            gvwEmissionUnitCtrlAppData.DataSource = dtList
            gvwEmissionUnitCtrlAppData.DataBind()
            ErrorsExist = True
        Else
            lblEmissionUnitCtrlAppData.Text = ""
        End If
    End Sub

    Private Sub CheckProcessCtrlAppData(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblProcessCtrlAppData.Text = "E07b - Process Control Approach data out of range."
            gvwProcessCtrlAppData.DataSource = dtList
            gvwProcessCtrlAppData.DataBind()
            ErrorsExist = True
        Else
            lblProcessCtrlAppData.Text = ""
        End If
    End Sub

    Private Sub CheckEmissionUnitCtrlAppPollData(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionUnitCtrlAppPollData.Text = "E06c - Emission Unit Control Approach Pollutant data out of range."
            gvwEmissionUnitCtrlAppPollData.DataSource = dtList
            gvwEmissionUnitCtrlAppPollData.DataBind()
            ErrorsExist = True
        Else
            lblEmissionUnitCtrlAppPollData.Text = ""
        End If
    End Sub

    Private Sub CheckProcessCtrlAppPollData(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblProcessCtrlAppPollData.Text = "E07c - Process Control Approach Pollutant data out of range."
            gvwProcessCtrlAppPollData.DataSource = dtList
            gvwProcessCtrlAppPollData.DataBind()
            ErrorsExist = True
        Else
            lblProcessCtrlAppPollData.Text = ""
        End If
    End Sub

    Private Sub CheckPMCtrlPollutantDependancy(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblPMControlPollutantDependancy.Text = "E34a - If there is a PM2.5 control pollutant there must be a PM10 control pollutant."
            gvwPMControlPollutantDependancy.DataSource = dtList
            gvwPMControlPollutantDependancy.DataBind()
            ErrorsExist = True
        Else
            lblPMControlPollutantDependancy.Text = ""
        End If
    End Sub

    Private Sub CheckEmissionUnitType(ByVal dtList As DataTable)
        ' Error - check if emission unit type is unclassified (999), null, or invalid
        ' EPA checks 67 & 68
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblEmissionUnitType.Text = "E29 - Emission Unit Type is missing or invalid."
            gvwEmissionUnitType.DataSource = dtList
            gvwEmissionUnitType.DataBind()
            ErrorsExist = True
        Else
            lblEmissionUnitType.Text = ""
        End If
    End Sub

    Private Sub CheckOperatingDetailsAppData(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblOperatingDetailsAppData.Text = "E26a - Process Operating Details data out of range."
            gvwOperatingDetailsAppData.DataSource = dtList
            gvwOperatingDetailsAppData.DataBind()
            ErrorsExist = True
        Else
            lblOperatingDetailsAppData.Text = ""
        End If
    End Sub

    Private Sub CheckE30(dtList As DataTable)
        Try
            If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 AndAlso dtList.Rows(0)(0) = True Then
                lblBadNaics.Text = ""
                hlBadNaics.Visible = False
            Else
                lblBadNaics.Text = "E30 - NAICS code is missing or invalid."
                hlBadNaics.Visible = True

                pnlGeneralErrors.Visible = True
                ErrorsExist = True
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub CheckE31(dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblE31.Text = "E31 - Emission unit status code is missing or invalid."
            gvwE31.DataSource = dtList
            gvwE31.DataBind()
            ErrorsExist = True
        Else
            lblE31.Text = ""
        End If
    End Sub

    Private Sub CheckE32(dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblE32.Text = "E32 - Emission unit operation date is invalid."
            gvwE32.DataSource = dtList
            gvwE32.DataBind()
            ErrorsExist = True
        Else
            lblE32.Text = ""
        End If
    End Sub

    Private Sub CheckE33(dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblE33.Text = "E33 - Emission unit type code is missing or invalid."
            gvwE33.DataSource = dtList
            gvwE33.DataBind()
            ErrorsExist = True
        Else
            lblE33.Text = ""
        End If
    End Sub

    Private Sub CheckE34b(dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblE34b.Text = "E34b - If there is a PM2.5 control pollutant there must be a PM10 control pollutant."
            gvwE34b.DataSource = dtList
            gvwE34b.DataBind()
            ErrorsExist = True
        Else
            lblE34b.Text = ""
        End If
    End Sub

    Private Sub CheckE35(dtList As DataTable)
        Try
            If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 AndAlso dtList.Rows(0)(0) = True Then
                lblInvalidSiteStatus.Text = "Invalid Facility Site Status or missing Site Status Code Year. Please contact the APB to correct."
                lblInvalidSiteStatus.Visible = True

                pnlGeneralErrors.Visible = True
                ErrorsExist = True
            Else
                lblInvalidSiteStatus.Text = ""
                lblInvalidSiteStatus.Visible = False
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub CheckE36(dtList As DataTable)
        Try
            If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 AndAlso dtList.Rows(0)(0) = True Then
                lblMissingSiteAddress.Text = ""
                lblMissingSiteAddress.Visible = False
            Else
                lblMissingSiteAddress.Text = "Missing Facility Site Address. Please contact the APB to correct."
                lblMissingSiteAddress.Visible = True

                pnlGeneralErrors.Visible = True
                ErrorsExist = True
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub CheckE37(dtList As DataTable)
        Try
            If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 AndAlso dtList.Rows(0)(0) = True Then
                lblMissingFacilityGeo.Text = ""
                lblMissingFacilityGeo.Visible = False
            Else
                lblMissingFacilityGeo.Text = "Missing Facility Geographic Coordinates. Please contact the APB to correct."
                lblMissingFacilityGeo.Visible = True

                pnlGeneralErrors.Visible = True
                ErrorsExist = True
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub CheckE03b(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblE03b.Text = "E03b - One or more missing or invalid release point parameters. See Release Points in Facility Inventory Help for details."
            gvwE03b.DataSource = dtList
            gvwE03b.DataBind()
            ErrorsExist = True
        Else
            lblE03b.Text = ""
        End If
    End Sub

    Private Sub CheckE38(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblE38.Text = "E38 - Invalid fuel burning information."
            gvwE38.DataSource = dtList
            gvwE38.DataBind()
            ErrorsExist = True
        Else
            lblE38.Text = ""
        End If
    End Sub

    Private Sub CheckE39(ByVal dtList As DataTable)
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblE39.Text = "E39 - Process has invalid throughput units for the selected Source Classification Code."
            gvwE39.DataSource = dtList
            gvwE39.DataBind()
            ErrorsExist = True
        Else
            lblE39.Text = ""
        End If
    End Sub

    ' >>>>>>>>>>>>  Warning Check Subs Begin
    Private Sub CheckPMTotalEmissionsWarning(dtList As DataTable)
        'Checks if particulate matter pollutant emissions are missing

        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblPMTotalEmissionsWarning.Text = "W20 - Warning(s) for missing particulate matter emissions values."

            gvwPMTotalEmissionsWarning.DataSource = dtList
            gvwPMTotalEmissionsWarning.DataBind()

            WarningsExist = True
        Else
            lblPMTotalEmissionsWarning.Text = ""
        End If
    End Sub

    Private Sub CheckFugitiveRPWarnings(dtList As DataTable)
        'Checks for null optional values for fugitive release points

        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblFugitiveWarning.Text = "W02 - Optional fugitive information has not been entered."

            gvwFugitiveWarning.DataSource = dtList
            gvwFugitiveWarning.DataBind()

            WarningsExist = True
        Else
            lblFugitiveWarning.Text = ""
        End If
    End Sub

    Private Sub Check20PercentChange(dtList As DataTable)
        ' Checks if any reported emissions have changed by at least 20 percent over the previously reported value
        If dtList IsNot Nothing AndAlso dtList.Rows.Count > 0 Then
            lblTwentyPercent.Text = "W04 - Reported emissions have increased or decreased by at least 20 percent compared to previously reported values."

            gvwTwentyPercent.DataSource = dtList
            gvwTwentyPercent.DataBind()

            WarningsExist = True
        Else
            lblTwentyPercent.Text = ""
        End If
    End Sub

#End Region

#Region "  Error and Warning List gridviews  "

    Private Sub LoadgvwEISErrorsWarnings()
        Dim query1 = "select ErrorID, strConditionForError, strCorrectiveAction " &
            "from eislk_EISErrors where active='1' Order by ErrorID"

        gvwEISErrors.DataSource = DB.GetDataTable(query1)
        gvwEISErrors.DataBind()

        Dim query2 = "select WarningID, strConditionForWarning, strCorrectiveAction " &
            "from eislk_EISWarnings where active='1' Order by WarningID"

        gvwEISWarnings.DataSource = DB.GetDataTable(query2)
        gvwEISWarnings.DataBind()
    End Sub

#End Region

#Region "  Menu Routines  "

    Private Sub ShowFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = True
        End If

    End Sub

    Private Sub HideFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = False
        End If

    End Sub

    Private Sub ShowEmissionInventoryMenu()

        Dim menuEmissionInventory As Panel

        menuEmissionInventory = CType(Master.FindControl("pnlEmissionInventory"), Panel)

        If Not menuEmissionInventory Is Nothing Then
            menuEmissionInventory.Visible = True
        End If

    End Sub

    Private Sub HideEmissionInventoryMenu()

        Dim menuEmissionInventory As Panel

        menuEmissionInventory = CType(Master.FindControl("pnlEmissionInventory"), Panel)

        If Not menuEmissionInventory Is Nothing Then
            menuEmissionInventory.Visible = False
        End If

    End Sub

    Private Sub ShowEISHelpMenu()

        Dim menuEISHelp As Panel

        menuEISHelp = CType(Master.FindControl("pnlEISHelp"), Panel)

        If Not menuEISHelp Is Nothing Then
            menuEISHelp.Visible = True
        End If

    End Sub

    Private Sub HideEISHelpMenu()

        Dim menuEISHelp As Panel

        menuEISHelp = CType(Master.FindControl("pnlEISHelp"), Panel)

        If Not menuEISHelp Is Nothing Then
            menuEISHelp.Visible = False
        End If

    End Sub

#End Region

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel1.Click, btnContinue.Click, btnEIHome.Click
        Response.Redirect("Default.aspx")
    End Sub

#Region " Notification Emails "

    Private Sub EmailNotificationsToECSU()

        EmailIf20PercentChange()
        EmailIfEmissionCalcMethodIsVague()

    End Sub

    Private Sub EmailIf20PercentChange()

        Dim query = " SELECT distinct " &
        "     EMISSIONSUNITID, " &
        "     PROCESSID, " &
        "     STRPROCESSDESCRIPTION " &
        " from VW_EIS_YEARLY_EMISSIONS " &
        " where FACILITYSITEID = @FacilitySiteID " &
        "         and CURR_INTINVENTORYYEAR = @year " &
        "         and EmissionsChangeGreaterThan20Percent = 1 " &
        " order by EMISSIONSUNITID, PROCESSID "

        Dim params As SqlParameter() = {
            New SqlParameter("@FacilitySiteID", FacilitySiteID),
            New SqlParameter("@year", EIYear)
        }

        Dim dt = DB.GetDataTable(query, params)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim airsNumber As String = Left(FacilitySiteID, 3) & "-" & Right(FacilitySiteID, 5)
            Dim facilityName As String = GetFacilityName(FacilitySiteID)

            Dim plainBody As String = "The following facility has completed a submission to the Emission Inventory System. " &
                "One or more processes at the facility (listed below) have reported emissions that have increased or decreased " &
                "by at least 20 percent compared to previously reported values. " & vbNewLine &
                vbNewLine &
                airsNumber & ": " & facilityName & vbNewLine &
                vbNewLine &
                "Emission Unit ID, Process ID, Process Description" & vbNewLine

            For Each row As DataRow In dt.Rows
                plainBody &= row("EMISSIONSUNITID").ToString & ", " & row("PROCESSID").ToString & ", " & row("STRPROCESSDESCRIPTION").ToString & vbNewLine
            Next

            Dim htmlBody As String = "<p>The following facility has completed a submission to the Emission Inventory System. " &
                "One or more processes at the facility (listed below) have reported emissions that have increased or decreased " &
                "by at least 20 percent compared to previously reported values. </p>" &
                "<p><b>" & airsNumber & "</b>: " & facilityName & "</p>" &
                "<table><thead><tr><th style='text-align:left'>Emission Unit ID</th><th style='text-align:left'>Process ID</th>" &
                "<th style='text-align:left'>Process Description</th></tr></thead><tbody>"

            For Each row As DataRow In dt.Rows
                htmlBody &= "<tr><td>" & row("EMISSIONSUNITID").ToString & "</td><td>" & row("PROCESSID").ToString & "</td><td>" & row("STRPROCESSDESCRIPTION").ToString & "</td></tr>"
            Next

            htmlBody &= "</tbody></table>"

            SendEmail(GecoContactEmail, "GECO EIS - Notice of large changes in reported emissions", plainBody, htmlBody,
                      caller:="EIS_rp_submit.EmailIf20PercentChange")
        End If

    End Sub

    Private Sub EmailIfEmissionCalcMethodIsVague()

        Dim query = " select distinct " &
        "     e.EMISSIONSUNITID, " &
        "     e.PROCESSID, " &
        "     p.STRPROCESSDESCRIPTION, " &
        "     c.STRDESC " &
        " from EIS_REPORTINGPERIODEMISSIONS e " &
        "     inner join EISLK_EMCALCMETHODCODE c " &
        "         on c.EMCALCMETHODCODE = e.EMCALCMETHODCODE " &
        "     inner join EIS_PROCESS p " &
        "         on e.FACILITYSITEID = p.FACILITYSITEID " &
        "            and e.EMISSIONSUNITID = p.EMISSIONSUNITID " &
        "            and e.PROCESSID = p.PROCESSID " &
        " where e.FACILITYSITEID = @FacilitySiteID " &
        "       and INTINVENTORYYEAR = @year " &
        "       and convert(int, e.EMCALCMETHODCODE) in (2, 13, 33) "

        Dim params As SqlParameter() = {
            New SqlParameter("@FacilitySiteID", FacilitySiteID),
            New SqlParameter("@year", EIYear)
        }

        Dim dt = DB.GetDataTable(query, params)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim airsNumber As String = Left(FacilitySiteID, 3) & "-" & Right(FacilitySiteID, 5)
            Dim facilityName As String = GetFacilityName(FacilitySiteID)

            Dim plainBody As String = "The following facility has completed a submission to the Emission Inventory System. " &
                "One or more processes at the facility (listed below) have reported emissions using ""Engineering Judgment"" " &
                "or an ""Other"" emission factor. " & vbNewLine &
                vbNewLine &
                airsNumber & ": " & facilityName & vbNewLine &
                vbNewLine &
                "Emission Unit ID, Process ID, Process Description, Emission Calculation Method" & vbNewLine

            For Each row As DataRow In dt.Rows
                plainBody &= row("EMISSIONSUNITID").ToString & ", " & row("PROCESSID").ToString & ", " &
                    row("STRPROCESSDESCRIPTION").ToString & ", " & row("STRDESC").ToString & vbNewLine
            Next

            Dim htmlBody As String = "<p>The following facility has completed a submission to the Emission Inventory System. " &
                "One or more processes at the facility (listed below) have reported emissions using &ldquo;Engineering Judgment&rdquo; " &
                "or an &ldquo;Other&rdquo; emission factor.</p>" &
                "<p><b>" & airsNumber & "</b>: " & facilityName & "</p>" &
                "<table><thead><tr>" &
                "<th style='text-align:left'>Emission Unit ID</th><th style='text-align:left'>Process ID</th>" &
                "<th style='text-align:left'>Process Description</th><th style='text-align:left'>Emission Calculation Method</th>" &
                "</tr></thead><tbody>"

            For Each row As DataRow In dt.Rows
                htmlBody &= "<tr><td>" & row("EMISSIONSUNITID").ToString & "</td><td>" & row("PROCESSID").ToString & "</td><td>" &
                    row("STRPROCESSDESCRIPTION").ToString & "</td><td>" & row("STRDESC").ToString & "</td></tr>"
            Next

            htmlBody &= "</tbody></table>"

            SendEmail(GecoContactEmail, "GECO EIS - Notice of emission calculation method", plainBody, htmlBody,
                      caller:="EIS_rp_submit.EmailIfEmissionCalcMethodIsVague")
        End If

    End Sub

#End Region

End Class