Imports System.Data.SqlClient

Partial Class EIS_rp_emissions_edit
    Inherits Page

    Private EmissionTotal As Double

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EIYEar As Integer = GetCookie(EisCookie.EISMaxYear)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim SummerDayRequired As Boolean = CheckSummerDayRequired(FacilitySiteID)
        Dim EmissionsUnitID As String = Request.QueryString("eu")
        Dim ProcessID As String = Request.QueryString("ep")
        Dim PollutantCode As String = Request.QueryString("em")

        EIAccessCheck(EISAccessCode, EISStatus)

        If CheckProcessExist(FacilitySiteID, EmissionsUnitID, ProcessID) <> UnitActiveStatus.Active Then
            HttpContext.Current.Response.Redirect("~/EIS/rp_summary.aspx")
        End If

        txtProcessID.Text = ProcessID
        txtEmissionUnitID.Text = EmissionsUnitID

        If Not IsPostBack Then

            LoadPollutantsGVW(FacilitySiteID, EmissionsUnitID, ProcessID)
            LoadPollutantsDDL()
            LoadEMCalcMethod()
            LoadEFNumUoM()
            LoadEFDenUoM()

            If PollutantCode <> "" Then
                LoadPollutantDetails(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, EIYEar)

                'Minimum value for Summer Day pollutant is 0.001 tpy; hide Summer Day panel if Actual Emissions < 0.300 tpy
                If SummerDayRequired And (PollutantCode = "VOC" Or PollutantCode = "NOX") And EmissionTotal >= 0.3 Then
                    pnlSummerDay.Visible = True
                    LoadSummerDay(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, EIYEar)
                Else
                    pnlSummerDay.Visible = False
                End If

            Else
                LoadTopDetails(FacilitySiteID, EmissionsUnitID, ProcessID)
                pnlSummerDay.Visible = False
            End If

            CheckCalcMethodFields()

        End If

    End Sub

#Region "Load Routines  "

    Private Sub LoadPollutantsGVW(ByVal fsid As String, ByVal euid As String, ByVal prid As String)
        Dim query = " select " &
            "     EMISSIONSUNITID, " &
            "     PROCESSID, " &
            "     POLLUTANTCODE, " &
            "     STRPOLLUTANTDESCRIPTION, " &
            "     CASE WHEN RPTPERIODTYPECODE = 'O3D' " &
            "         THEN 'Summer Day' " &
            "     ELSE 'Annual' " &
            "     END RPTPeriodType, " &
            "     CASE WHEN RPTPERIODTYPECODE = 'O3D' " &
            "         THEN 'TPD' " &
            "     ELSE 'TPY' " &
            "     END PollutantUnit, " &
            "     PREV_INTINVENTORYYEAR, " &
            "     PREV_FLTTOTALEMISSIONS, " &
            "     CURR_FLTTOTALEMISSIONS, " &
            "     STREMISSIONSCOMMENT, " &
            "     EmissionsChangeGreaterThan20Percent " &
            " FROM VW_EIS_YEARLY_EMISSIONS " &
            " where FACILITYSITEID = @fsid " &
            "       and EMISSIONSUNITID = @euid " &
            "       and PROCESSID = @prid " &
            " order by POLLUTANTCODE, RPTPeriodType "

        Dim params = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid)
        }

        gvwPollutants.DataSource = DB.GetDataTable(query, params)
        gvwPollutants.DataBind()

    End Sub

    Private Sub LoadPollutantsDDL()
        ddlPollutant.Items.Clear()
        ddlPollutant.Items.Add("--Select Pollutant--")

        Try
            Dim query = "select PollutantCode, strDesc FROM eislk_PollutantCode " &
                    " where strPollutantType = 'CAP' and Active = '1' order by strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("PollutantCode")
                    }
                    ddlPollutant.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadEMCalcMethod()
        ddlEMCalcMethod.Items.Clear()
        ddlEMCalcMethod.Items.Add(New ListItem("-- Select Calculation Method (listed in order of preference) --", 0))

        Try
            Dim query = "select EMCalcMethodCode, strDesc FROM EISLK_EMCalcMethodCode " &
                " where Active = '1' order by Priority, strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("EMCalcMethodCode")
                    }
                    ddlEMCalcMethod.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadEFNumUoM()
        ddlEFNumUoM.Items.Clear()
        ddlEFNumUoM.Items.Add("--Select Numerator--")

        Try
            Dim query = " select EFNumUoMCode, strDesc FROM EISLK_EFNumUoMCode where Active = '1' order by strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("EFNumUoMCode")
                    }
                    ddlEFNumUoM.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadEFDenUoM()
        ddlEFDenUoM.Items.Clear()
        ddlEFDenUoM.Items.Add("--Select Denominator--")

        Try
            Dim query = " select EFDenUoMCode, strDesc FROM EISLK_EFDenUoMCode where Active = '1' order by strDesc"

            Dim dt = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("EFDenUoMCode")
                    }
                    ddlEFDenUoM.Items.Add(newListItem)
                Next
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadPollutantDetails(ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal pcode As String, ByVal eiyr As Integer)

        Try
            Dim query = "select " &
                    "EMCalcMethodCode, " &
                    "(select strUnitDescription FROM eis_EmissionsUnit where eis_EmissionsUnit.FacilitySiteID = @fsid and " &
                    "eis_EmissionsUnit.EmissionsUnitID = @euid) As EUDescription, " &
                    "(select strProcessDescription FROM eis_Process where eis_Process.FacilitySiteID = @fsid and " &
                    "eis_Process.EmissionsUnitID = @euid and eis_Process.ProcessID = @prid) As ProcessDescription, " &
                    "fltEmissionFactor, " &
                    "EFNumUomCode, " &
                    "EFDenUomCode, " &
                    "fltTotalEmissions, " &
                    "strEmissionFactorText, " &
                    "strEmissionsComment " &
                    "from " &
                    "eis_ReportingPeriodEmissions " &
                    "where " &
                    "intInventoryYear = @eiyr and " &
                    "FacilitySiteID = @fsid and " &
                    "EmissionsUnitID = @euid and " &
                    "ProcessID = @prid and " &
                    "PollutantCode = @pcode and " &
                    "rptPeriodTypeCode = 'A'"

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@prid", prid),
                New SqlParameter("@pcode", pcode),
                New SqlParameter("@eiyr", eiyr)
            }

            Dim dr = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                If IsDBNull(dr("EUDescription")) Then
                    txtEmissionUnitDescription.Text = ""
                Else
                    txtEmissionUnitDescription.Text = dr.Item("EUDescription")
                End If
                If IsDBNull(dr("ProcessDescription")) Then
                    txtProcessDescription.Text = ""
                Else
                    txtProcessDescription.Text = dr.Item("ProcessDescription")
                End If
                If pcode = "" Then
                    ddlPollutant.SelectedIndex = 0
                Else
                    ddlPollutant.SelectedValue = pcode
                End If

                If IsDBNull(dr("EMCalcMethodCode")) Then
                    ddlEMCalcMethod.SelectedValue = 0
                Else
                    ddlEMCalcMethod.SelectedValue = dr.Item("EMCalcMethodCode")
                End If
                CheckCalcMethodFields()

                If IsDBNull(dr("fltEmissionFactor")) Then
                    txtEmissionFactor.Text = ""
                Else
                    txtEmissionFactor.Text = dr.Item("fltEmissionFactor")
                End If
                If IsDBNull(dr("EFNumUomCode")) Or txtEmissionFactor.Text = "" Then
                    ddlEFNumUoM.SelectedIndex = 0
                Else
                    ddlEFNumUoM.SelectedValue = dr.Item("EFNumUomCode")
                End If
                If IsDBNull(dr("EFDenUomCode")) Or txtEmissionFactor.Text = "" Then
                    ddlEFDenUoM.SelectedIndex = 0
                Else
                    ddlEFDenUoM.SelectedValue = dr.Item("EFDenUomCode")
                End If
                If IsDBNull(dr("fltTotalEmissions")) Then
                    txtTotalEmissions.Text = ""
                    EmissionTotal = 0
                Else
                    txtTotalEmissions.Text = dr.Item("fltTotalEmissions")
                    EmissionTotal = dr.Item("fltTotalEmissions")
                End If
                If IsDBNull(dr("strEmissionFactorText")) Then
                    txtEFExplanation.Text = ""
                Else
                    txtEFExplanation.Text = dr.Item("strEmissionFactorText")
                End If
                If IsDBNull(dr("strEmissionsComment")) Then
                    txtEmissionsComment.Text = ""
                Else
                    txtEmissionsComment.Text = Left(dr.Item("strEmissionsComment"), 7500)
                End If

            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

        txtPollutantToDelete.Text = ddlPollutant.SelectedItem.Text

    End Sub

    Private Sub LoadSummerDay(ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal pcode As String, ByVal eiyr As Integer)

        Dim SummerDayCode As String = ""

        SummerDayCode = CheckSummerDayExist(fsid, euid, prid, pcode, eiyr)

        If (SummerDayCode = pcode) Then
            pnlSummerDayPollutant.Visible = True
            LoadSummerDay(fsid, euid, prid, eiyr, pcode)
        Else
            ddlSummerDay.SelectedIndex = 0
            txtSummerDayPollutant.Text = ""
            pnlSummerDayPollutant.Visible = False
        End If

    End Sub

    Private Sub LoadSummerDay(ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal eiyr As Integer, ByVal pcode As String)
        Dim SummerDayEmissions As String = ""

        Try
            Dim query = "select fltTotalEmissions FROM vw_eis_RPEmissions " &
                        " where intInventoryYear = @eiyr " &
                        " and FacilitySiteID = @fsid " &
                        " and EmissionsUnitID = @euid " &
                        " and ProcessId = @prid " &
                        " and PollutantCode = @pcode " &
                        " and RPTPeriodTypeCode = 'O3D'"

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@prid", prid),
                New SqlParameter("@pcode", pcode),
                New SqlParameter("@eiyr", eiyr)
            }

            Dim dr1 = DB.GetDataRow(query, params)

            If dr1 IsNot Nothing Then
                If IsDBNull(dr1("fltTotalEmissions")) Then
                    SummerDayEmissions = ""
                Else
                    SummerDayEmissions = dr1.Item("fltTotalEmissions")
                End If
            End If

            If SummerDayEmissions IsNot Nothing AndAlso IsNumeric(SummerDayEmissions) Then
                txtSummerDayPollutant.Text = Math.Round(CDbl(SummerDayEmissions), 3)
            End If

            ddlSummerDay.SelectedValue = "Yes"

            If pcode = "VOC" Then
                lblSummerDayPollutant.Text = "Summer Day VOC Emissions (tons/day):"
                lblSummerDayPollutant.ToolTip = "Average daily summer emissions of VOC (Summer Day emissions) in tons/day"
            Else
                lblSummerDayPollutant.Text = "Summer Day NOx Emissions (tons/day):"
                lblSummerDayPollutant.ToolTip = "Average daily summer emissions of NOx (Summer Day emissions) in tons/day"
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadTopDetails(ByVal fsid As String, ByVal euid As String, ByVal prid As String)

        txtEmissionUnitDescription.Text = GetEmissionUnitDesc(fsid, euid)
        txtProcessDescription.Text = GetProcessDesc(fsid, euid, prid)

    End Sub

#End Region

#Region " Button Routines "

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click

        ClearForm()
        ddlPollutant.SelectedIndex = 0
        'Clear textbox in Delete Confirm panel
        txtPollutantToDelete.Text = ""

    End Sub

    Protected Sub btnDetails2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDetails2.Click

        ReturnToDetails()

    End Sub

    Protected Sub btnDetails1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDetails1.Click

        ReturnToDetails()

    End Sub

    Protected Sub btnConfirmDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmDelete.Click

        Dim EIYear As Integer = CInt(GetCookie(EisCookie.EISMaxYear))
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        Dim PollutantCode As String = ddlPollutant.SelectedValue
        Dim SummerDayCode As String = CheckSummerDayExist(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, EIYear)
        Dim SummerDayRequired As Boolean = CheckSummerDayRequired(FacilitySiteID)
        Dim SummerDayPollutantCount As Integer

        'delete annual emissions record for pollutant
        DeleteReportingPeriodEmissions(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, "A")

        'If summer day emissions exist delete in 1 OR 3 tables.
        If (SummerDayCode = "VOC" Or SummerDayCode = "NOX") And SummerDayRequired Then
            'get summer day pollutant count for process BEFORE deleting in eis_reportingperiodemissions
            DeleteReportingPeriodEmissions(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, "O3D")
            SummerDayPollutantCount = CountSummerDayPollutants(FacilitySiteID, EmissionsUnitID, ProcessID, EIYear)
            If SummerDayPollutantCount >= 1 Then
                'Do nothing
            Else
                DeleteProcessOperatingDetails(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID, "O3D")
                DeleteProcessReportingPeriod(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID, "O3D")
            End If
        End If
        LoadPollutantsGVW(FacilitySiteID, EmissionsUnitID, ProcessID)
        ClearForm()
        'Clear textbox in Delete Confirm panel
        txtPollutantToDelete.Text = ""

    End Sub

#End Region

    Protected Sub ddlPollutant_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPollutant.SelectedIndexChanged

        'If pollutant chosen already exists for the emission unit/processid then the details get loaded.
        Dim PollutantCode As String = ddlPollutant.SelectedValue
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EIYEar As Integer = CInt(GetCookie(EisCookie.EISMaxYear))
        Dim SummerDayRequired As Boolean = CheckSummerDayRequired(FacilitySiteID)

        Try
            Dim query = "Select PollutantCode FROM EIS_ReportingPeriodEmissions " &
                    "where FacilitySiteID = @FacilitySiteID and " &
                    "EmissionsUnitID = @EmissionsUnitID and " &
                    "ProcessID = @ProcessID  and " &
                    "PollutantCode = @PollutantCode and " &
                    "intInventoryYear = @EIYEar and " &
                    "Active = '1'"

            Dim params = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionsUnitID", EmissionsUnitID),
                New SqlParameter("@ProcessID", ProcessID),
                New SqlParameter("@PollutantCode", PollutantCode),
                New SqlParameter("@EIYEar", EIYEar)
            }

            If DB.ValueExists(query, params) Then
                LoadPollutantDetails(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, EIYEar)
                txtPollutantToDelete.Text = ddlPollutant.SelectedItem.Text

                If SummerDayRequired And (PollutantCode = "VOC" Or PollutantCode = "NOX") And EmissionTotal >= 0.3 Then
                    LoadSummerDay(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, EIYEar)
                    pnlSummerDay.Visible = True
                Else
                    txtSummerDayPollutant.Text = ""
                    ddlSummerDay.SelectedIndex = 0
                    pnlSummerDay.Visible = False
                End If
            Else
                ClearForm()
                txtPollutantToDelete.Text = ""

                If SummerDayRequired And (PollutantCode = "VOC" Or PollutantCode = "NOX") Then
                    pnlSummerDay.Visible = True
                    ddlSummerDay.SelectedIndex = 0
                    txtSummerDayPollutant.Text = ""
                    pnlSummerDayPollutant.Visible = False
                Else
                    txtSummerDayPollutant.Text = ""
                    ddlSummerDay.SelectedIndex = 0
                    pnlSummerDay.Visible = False
                End If
            End If

            ClearSaveMessages()
            If ddlPollutant.SelectedIndex = 0 Then
                txtPollutantToDelete.Text = ""
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub ClearForm()

        'Clears form except for upper identification area and pollutant ddl
        ddlEMCalcMethod.SelectedIndex = 0
        txtEmissionFactor.Text = ""
        txtEmissionFactor.Enabled = True
        ddlEFNumUoM.SelectedIndex = 0
        ddlEFNumUoM.Enabled = True
        ddlEFDenUoM.SelectedIndex = 0
        ddlEFDenUoM.Enabled = True
        ddlSummerDay.SelectedIndex = 0
        txtSummerDayPollutant.Text = ""
        txtTotalEmissions.Text = ""
        txtEFExplanation.Text = ""
        txtEFExplanation.Enabled = True
        txtEmissionsComment.Text = ""

    End Sub

    Private Sub ClearSaveMessages()

        lblSaveMessage1.Text = ""
        lblSaveMessage2.Text = ""

    End Sub

    'This custom validator check is not done if the Summer Day panel containing the Summer Day NOx or VOC value is not visible
    'i.e. when the SummerDay panel is hidden the validator that calls this routine is also hidden (disabled)
    Protected Sub CheckSummerDay(Sender As Object, args As ServerValidateEventArgs) Handles cusvSummerDayPollutant.ServerValidate
        'Checks Emission Unit ID when adding new emission unit in panel
        Dim EIYear As Integer = GetCookie(EisCookie.EISMaxYear)
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        Dim SummerDayEntered As Boolean = False
        Dim PollutantCode As String = ddlPollutant.SelectedValue
        Dim TotalEmissions As Double = CDbl(txtTotalEmissions.Text)
        Dim MinSummerDay As Double = 0.001
        Dim MaxSummerDay As Double = Math.Round(TotalEmissions / 300, 3)
        Dim SummerDay As Double = CDbl(txtSummerDayPollutant.Text)

        'Show warning for max Summer Day being over 1/300 of annual emission
        If CDbl(txtSummerDayPollutant.Text) > MaxSummerDay Then
            lblSummerDayValue.Visible = True
        Else
            lblSummerDayValue.Visible = False
        End If

        If SummerDay < MinSummerDay Then
            args.IsValid = False
            cusvSummerDayPollutant.ErrorMessage = "Invalid entry. Summer Day emissions must be greater than 0.001 tons per day."
        Else
            args.IsValid = True

            If txtSummerDayPollutant.Text <> "" Then
                SummerDayEntered = True
            End If

            SaveEmissions()
            If SummerDayEntered And (PollutantCode = "VOC" Or PollutantCode = "NOX") Then
                SaveSummerDay(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, EIYear)
            End If
            LoadPollutantsGVW(FacilitySiteID, EmissionsUnitID, ProcessID)
            lblSaveMessage1.Text = "Emissions information saved successfully"
            lblSaveMessage2.Text = lblSaveMessage1.Text
            lblSaveMessage1.Visible = True
            lblSaveMessage2.Visible = True
            txtPollutantToDelete.Text = ddlPollutant.SelectedItem.Text

        End If

    End Sub

    'If Summer Day Pollutant panel is not visible the CheckSummerDay validator will not fire and notehing will happen. This fixes that issue
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        Dim EIYear As Integer = CInt(GetCookie(EisCookie.EISMaxYear))
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        Dim PollutantCode As String = ddlPollutant.SelectedValue
        Dim SummerDayRequired As Boolean = CheckSummerDayRequired(FacilitySiteID)
        Dim SummerDayPollutantCount As Integer

        'NOTE: If summer day emission present then annual emissions ('A') already saved in CheckSummerDay routine.
        'If no summer day emissions exist for process/pollutant then this routine saves annual emissions ('A') AND also deletes any summer day data if present.
        'This is possible if summer day existed and user chooses "No" for summer day and saves the annual only.

        If pnlSummerDayPollutant.Visible = False Then

            'Save annual emissions only ('A')
            SaveEmissions()

            If SummerDayRequired And (PollutantCode = "VOC" Or PollutantCode = "NOX") Then
                'If summer day pollutant existed and user changed answer to "No" for summer day pollutant
                'this routine deletes the summer day pollutant from ReportingPeriodEmissions, ProcessOperatingDetails, ProcessReportingPeriod

                'Count summer day pollutants in ReportingPeriodEmissions to determine if data in ProcessOperatingDetails and
                'ProcessReportingPeriod also needs to be deleted.
                DeleteReportingPeriodEmissions(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, "O3D")
                SummerDayPollutantCount = CountSummerDayPollutants(FacilitySiteID, EmissionsUnitID, ProcessID, EIYear)

                If SummerDayPollutantCount = 0 Then
                    DeleteProcessOperatingDetails(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID, "O3D")
                    DeleteProcessReportingPeriod(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID, "O3D")
                End If

                EmissionTotal = GetEmissionTotal(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, "A", EIYear)

                If SummerDayRequired And EmissionTotal > 0.3 Then
                    pnlSummerDay.Visible = True
                End If
            End If

            LoadPollutantsGVW(FacilitySiteID, EmissionsUnitID, ProcessID)
            lblSaveMessage1.Text = "Emissions information saved successfully"
            lblSaveMessage2.Text = "Emissions information saved successfully"
            txtPollutantToDelete.Text = ddlPollutant.SelectedItem.Text

        End If

    End Sub

    Private Sub SaveEmissions()

        Dim EIYear As Integer = CInt(GetCookie(EisCookie.EISMaxYear))
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        Dim PollutantCode As String = ddlPollutant.SelectedValue
        Dim RPTypeCode As String = "A"
        Dim EmissionFactor As Double? = txtEmissionFactor.Text.ParseAsNullableDouble()
        Dim EFNumerator As String = ""
        Dim EFDenominator As String = ""
        Dim EmissionsUoM As String = "TON"
        Dim EMCalcMethodCode As String = ddlEMCalcMethod.SelectedValue
        Dim EmissionFactorText As String = Left(txtEFExplanation.Text, 100)
        Dim Comment As String = Left(txtEmissionsComment.Text, 8000)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim Active = "1"

        EmissionTotal = CDbl(txtTotalEmissions.Text)

        If ddlEFNumUoM.SelectedIndex = 0 Then
            'Nothing - leave EFNumerator null
        Else
            EFNumerator = ddlEFNumUoM.SelectedValue
        End If

        If ddlEFDenUoM.SelectedIndex = 0 Then
            'Nothing - leave EFDenominator null
        Else
            EFDenominator = ddlEFDenUoM.SelectedValue
        End If

        Try
            'Should RptTypeCode be added to criteria?
            Dim query = "Select PollutantCode FROM EIS_ReportingPeriodEmissions " &
                    "where FacilitySiteID = @FacilitySiteID and " &
                    "EmissionsUnitID = @EmissionsUnitID and " &
                    "ProcessID = @ProcessID and " &
                    "PollutantCode = @PollutantCode and " &
                    "intInventoryYear = @EIYear and " &
                    "RptPeriodTypeCode = 'A' and " &
                    "Active = '1'"

            Dim params = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionsUnitID", EmissionsUnitID),
                New SqlParameter("@ProcessID", ProcessID),
                New SqlParameter("@PollutantCode", PollutantCode),
                New SqlParameter("@EIYEar", EIYear)
            }

            If DB.ValueExists(query, params) Then
                query = "Update eis_ReportingPeriodEmissions Set " &
                         " fltTotalEmissions = @EmissionTotal , " &
                         " EmissionsUoMCode = @EmissionsUoM, " &
                         " fltEmissionFactor = @EmissionFactor, " &
                         " EMCalcMethodCode = @EMCalcMethodCode, " &
                         " EFNumUoMCode = @EFNumerator, " &
                         " EFDenUoMCode = @EFDenominator, " &
                         " strEmissionFactorText = @EmissionFactorText, " &
                         " strEmissionsComment = @Comment, " &
                         " Active = @Active, " &
                         " UpdateUser = @UpdateUser, " &
                         " UpdateDateTime = getdate() " &
                         " where " &
                         " intInventoryYear = @EIYear and " &
                         " FacilitySiteID = @FacilitySiteID and " &
                         " EmissionsUnitID = @EmissionsUnitID and " &
                         " ProcessID = @ProcessID and " &
                         " PollutantCode = @PollutantCode and " &
                         " RptPeriodTypeCode = @RPTypeCode "
            Else
                query = "insert into eis_ReportingPeriodEmissions " &
                        " (intInventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, " &
                        " RptPeriodTypeCode, fltTotalEmissions, EmissionsUoMCode, EMCalcMethodCode, " &
                        " fltEmissionFactor, EFNumUoMCode, EFDenUoMCode, strEmissionFactorText, strEmissionsComment, " &
                        " Active, UpdateUser, UpdateDatetime, CreateDateTime) " &
                        " values (" &
                        " @EIYear, " &
                        " @FacilitySiteID, " &
                        " @EmissionsUnitID, " &
                        " @ProcessID, " &
                        " @PollutantCode, " &
                        " @RPTypeCode, " &
                        " @EmissionTotal, " &
                        " @EmissionsUoM, " &
                        " @EMCalcMethodCode, " &
                        " @EmissionFactor, " &
                        " @EFNumerator, " &
                        " @EFDenominator, " &
                        " @EmissionFactorText, " &
                        " @Comment, " &
                        " @Active, " &
                        " @UpdateUser, " &
                        " getdate(), " &
                        " getdate()) "
            End If

            Dim params2 = {
                New SqlParameter("@EmissionTotal", EmissionTotal),
                New SqlParameter("@EmissionsUoM", EmissionsUoM),
                New SqlParameter("@EmissionFactor", EmissionFactor),
                New SqlParameter("@EMCalcMethodCode", If(Not String.IsNullOrEmpty(EMCalcMethodCode), EMCalcMethodCode, Nothing)),
                New SqlParameter("@EFNumerator", If(Not String.IsNullOrEmpty(EFNumerator), EFNumerator, Nothing)),
                New SqlParameter("@EFDenominator", If(Not String.IsNullOrEmpty(EFDenominator), EFDenominator, Nothing)),
                New SqlParameter("@EmissionFactorText", EmissionFactorText),
                New SqlParameter("@Comment", Comment),
                New SqlParameter("@Active", Active),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionsUnitID", EmissionsUnitID),
                New SqlParameter("@ProcessID", ProcessID),
                New SqlParameter("@PollutantCode", PollutantCode),
                New SqlParameter("@RPTypeCode", If(Not String.IsNullOrEmpty(RPTypeCode), RPTypeCode, Nothing)),
                New SqlParameter("@EIYEar", EIYear)
            }

            DB.RunCommand(query, params2)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub SaveSummerDay(ByVal fsid As String, ByVal euid As String, ByVal prid As String, ByVal pcode As String, ByVal eiyr As Integer)

        Dim RPTypeCode As String = "O3D"
        Dim TotalEmissions As Double? = txtSummerDayPollutant.Text.ParseAsNullableDouble()
        Dim EmissionFactor As Double? = txtEmissionFactor.Text.ParseAsNullableDouble()
        Dim EFNumerator As String = ""
        Dim EFDenominator As String = ""
        Dim EmissionsUoM As String = "TON"
        Dim EMCalcMethodCode As String = ddlEMCalcMethod.SelectedValue
        Dim EmissionFactorText As String = Left(txtEFExplanation.Text, 100)
        Dim Comment As String = Left(txtEmissionsComment.Text, 8000)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim Active = "1"

        If ddlEFNumUoM.SelectedIndex = 0 Then
            'Nothing - leave EFNumerator null
        Else
            EFNumerator = ddlEFNumUoM.SelectedValue
        End If

        If ddlEFDenUoM.SelectedIndex = 0 Then
            'Nothing - leave EFDenominator null
        Else
            EFDenominator = ddlEFDenUoM.SelectedValue
        End If

        'Create records in eis_ProcessReportingPeriod and eis_ProcessOperatingDetails
        InsertRPProcess(eiyr, fsid, euid, prid, "O3D", UpdateUser)

        Try
            Dim query = "Select PollutantCode FROM eis_ReportingPeriodEmissions " &
                    "where FacilitySiteID = @fsid and " &
                    "EmissionsUnitID = @euid and " &
                    "ProcessID = @prid and " &
                    "PollutantCode = @pcode and " &
                    "RPTPeriodTypeCode = 'O3D' and " &
                    "intInventoryYear = @eiyr and " &
                    "Active = '1'"

            Dim params = {
                New SqlParameter("@eiyr", eiyr),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@pcode", pcode),
                New SqlParameter("@prid", prid)
            }

            If DB.ValueExists(query, params) Then
                query = "Update eis_ReportingPeriodEmissions Set " &
                         "fltTotalEmissions = @TotalEmissions, " &
                         "EmissionsUoMCode = @EmissionsUoM , " &
                         "fltEmissionFactor = @EmissionFactor, " &
                         "EMCalcMethodCode = @EMCalcMethodCode, " &
                         "EFNumUoMCode = @EFNumerator, " &
                         "EFDenUoMCode = @EFDenominator, " &
                         "strEmissionFactorText = @EmissionFactorText, " &
                         "strEmissionsComment = @Comment, " &
                         "Active = @Active, " &
                         "UpdateUser = @UpdateUser, " &
                         "UpdateDateTime = getdate() " &
                         "where intInventoryYear = @eiyr and " &
                         "FacilitySiteID = @fsid and " &
                         "EmissionsUnitID = @euid and " &
                         "ProcessID = @prid and " &
                         "PollutantCode = @pcode and " &
                         "RPTPeriodTypeCode = @RPTypeCode "
            Else
                query = "insert into eis_ReportingPeriodEmissions " &
                        "(intInventoryYear, FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, " &
                        "RptPeriodTypeCode, fltTotalEmissions, EmissionsUoMCode, EMCalcMethodCode, " &
                        "fltEmissionFactor, EFNumUoMCode, EFDenUoMCode, strEmissionFactorText, strEmissionsComment, " &
                        "Active, UpdateUser, UpdateDatetime, CreateDateTime) " &
                        "values (" &
                        "@eiyr, " &
                        "@fsid, " &
                        "@euid, " &
                        "@prid, " &
                        "@pcode, " &
                        "@RPTypeCode, " &
                        "@TotalEmissions, " &
                        "@EmissionsUoM, " &
                        "@EMCalcMethodCode, " &
                        "@EmissionFactor, " &
                        "@EFNumerator, " &
                        "@EFDenominator, " &
                        "@EmissionFactorText, " &
                        "@Comment, " &
                        "@Active, " &
                        "@UpdateUser, " &
                        "getdate(), " &
                        "getdate()) "
            End If

            Dim params2 = {
                New SqlParameter("@eiyr", eiyr),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@pcode", pcode),
                New SqlParameter("@prid", prid),
                New SqlParameter("@RPTypeCode", If(Not String.IsNullOrEmpty(RPTypeCode), RPTypeCode, Nothing)),
                New SqlParameter("@TotalEmissions", TotalEmissions),
                New SqlParameter("@EmissionsUoM", EmissionsUoM),
                New SqlParameter("@EMCalcMethodCode", If(Not String.IsNullOrEmpty(EMCalcMethodCode), EMCalcMethodCode, Nothing)),
                New SqlParameter("@EmissionFactor", EmissionFactor),
                New SqlParameter("@EFNumerator", If(Not String.IsNullOrEmpty(EFNumerator), EFNumerator, Nothing)),
                New SqlParameter("@EFDenominator", If(Not String.IsNullOrEmpty(EFDenominator), EFDenominator, Nothing)),
                New SqlParameter("@EmissionFactorText", EmissionFactorText),
                New SqlParameter("@Comment", Comment),
                New SqlParameter("@Active", Active),
                New SqlParameter("@UpdateUser", UpdateUser)
            }

            DB.RunCommand(query, params2)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub ReturnToDetails()

        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        Dim EIYear As Integer = GetCookie(EisCookie.EISMaxYear)
        Dim target As String = "rp_details.aspx?eu=" & EmissionsUnitID & "&ep=" & ProcessID & "&yr=" & EIYear

        Response.Redirect(target)

    End Sub

    Protected Sub gvwPollutants_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwPollutants.RowCommand

        Dim EIYear As Integer = GetCookie(EisCookie.EISMaxYear)
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim index As Integer = Convert.ToInt32(e.CommandArgument)
        Dim row As GridViewRow = gvwPollutants.Rows(index)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text
        Dim ProcessID As String = txtProcessID.Text
        Dim PollutantCode As String = row.Cells(1).Text
        Dim SummerDayRequired As Boolean = CheckSummerDayRequired(FacilitySiteID)

        LoadPollutantDetails(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, EIYear)
        txtPollutantToDelete.Text = ddlPollutant.SelectedItem.Text
        ClearSaveMessages()

        If SummerDayRequired And (PollutantCode = "VOC" Or PollutantCode = "NOX") And EmissionTotal > 0.3 Then
            pnlSummerDay.Visible = True
            LoadSummerDay(FacilitySiteID, EmissionsUnitID, ProcessID, PollutantCode, EIYear)
        Else
            txtSummerDayPollutant.Text = ""
            ddlSummerDay.SelectedIndex = 0
            pnlSummerDay.Visible = False
        End If

    End Sub

#Region "  Emission Factor Validator Routines  "

    Protected Sub ddlEMCalcMethod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlEMCalcMethod.SelectedIndexChanged
        CheckCalcMethodFields()
    End Sub

    Private Sub CheckCalcMethodFields()

        EnableEmissionFactorFieldsIf(CalcMethodIsEmissionFactor(ddlEMCalcMethod.SelectedValue))

        RequireCalcMethodCommentIf(CalcMethodIsVague(ddlEMCalcMethod.SelectedValue))

    End Sub

    Private Sub RequireCalcMethodCommentIf(require As Boolean)

        rqvEmissionsComment.Enabled = require
        lblExplanationRequired.Visible = require

        If require Then
            txtEmissionsComment_TextBoxWatermarkExtender.WatermarkText = "REQUIRED"
            lblEmissionsComment.Text = "Comment and Calculation Method Explanation:"
        Else
            txtEmissionsComment_TextBoxWatermarkExtender.WatermarkText = "OPTIONAL"
            lblEmissionsComment.Text = "Comment:"
        End If

    End Sub

    Private Sub EnableEmissionFactorFieldsIf(enable As Boolean)

        txtEmissionFactor.Enabled = enable
        txtEFExplanation.Enabled = enable
        rngvEmissionFactor.Enabled = enable
        reqvEmissionFactor.Enabled = enable
        reqvEFNumUoM.Enabled = enable
        reqvEFDenUoM.Enabled = enable
        ddlEFNumUoM.Enabled = enable
        ddlEFDenUoM.Enabled = enable

        If enable Then
            txtEmissionFactor.Text = ""
            ddlEFNumUoM.SelectedIndex = 0
            ddlEFDenUoM.SelectedIndex = 0
        End If

    End Sub

#End Region

    Protected Sub ddlSummerDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSummerDay.SelectedIndexChanged

        Dim Pollutant As String = ddlPollutant.SelectedValue

        If ddlSummerDay.SelectedValue = "Yes" And Pollutant = "VOC" Then
            lblSummerDayPollutant.Text = "Summer Day VOC Emissions (tons/day):"
            lblSummerDayPollutant.ToolTip = "Average daily summer emissions of VOC (Summer Day emissions)"
            pnlSummerDayPollutant.Visible = True

        ElseIf ddlSummerDay.SelectedValue = "Yes" And Pollutant = "NOX" Then
            lblSummerDayPollutant.Text = "Summer Day NOx Emissions (tons/day):"
            lblSummerDayPollutant.ToolTip = "Average daily summer emissions of NOx (Summer Day emissions)"
            pnlSummerDayPollutant.Visible = True

        ElseIf ddlSummerDay.SelectedValue = "No" Then
            txtSummerDayPollutant.Text = ""
            pnlSummerDayPollutant.Visible = False

        Else
            txtSummerDayPollutant.Text = ""
            pnlSummerDayPollutant.Visible = False
        End If

    End Sub

    Protected Sub gvwPollutants_DataBound(sender As Object, e As EventArgs) Handles gvwPollutants.DataBound

        lblNoPollutantData.Visible = False
        btnDelete.Visible = False

        Select Case gvwPollutants.Rows.Count
            Case 0
                lblNoPollutantData.Text = "No pollutants entered for this Emission Unit and Process. Add at least one pollutant."
                lblNoPollutantData.Visible = True

            Case 1
                lblNoPollutantData.Visible = True

            Case 2
                'Check if the 2 rows contain the same pollutant (possible if Summer Day pollutant)
                If gvwPollutants.Rows(0).Cells(1).Text = gvwPollutants.Rows(1).Cells(1).Text Then
                    lblNoPollutantData.Visible = True
                Else
                    btnDelete.Visible = True
                End If

            Case Else ' Greater than 2
                btnDelete.Visible = True

        End Select

        Dim summerDay As Boolean = False
        Dim twentyPercent As Boolean = False

        If gvwPollutants.DataSource IsNot Nothing Then
            For Each row As DataRow In gvwPollutants.DataSource.Rows
                If row.Item(4) = "Summer Day" Then
                    summerDay = True
                End If
                If row.Item(10) Then
                    twentyPercent = True
                End If
            Next
        End If

        lblSummerDayNote.Visible = summerDay
        lbl20PercentNote.Visible = twentyPercent

    End Sub

End Class