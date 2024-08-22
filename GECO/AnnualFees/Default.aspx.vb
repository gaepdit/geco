Imports System.Data.SqlClient
Imports System.DateTime
Imports GaEpd.DBUtilities
Imports GECO.DAL.Facility
Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Partial Class AnnualFees_Default
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property currentAirs As ApbFacilityId
    Public Property feeYear As Integer? = Nothing
    Private Property feeYearCompleted As Boolean
    Public Property feeCalc As AnnualFeeCalc
    Public Property info As FacilityCommunicationInfo
    Public ReadOnly FeeContactInfo As String = ConfigurationManager.AppSettings("FeeContactInfo")

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        AirsSelectedCheck()

        currentUser = GetCurrentUser()
        currentAirs = New ApbFacilityId(GetCookie(Cookie.AirsNumber))

        'Check if the user has access to the Application
        Dim facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If Not facilityAccess.FeeAccess Then
            Response.Redirect("~/NoAccess.aspx")
        End If

        Master.IsFacilitySet = True

        If ViewState(NameOf(feeCalc)) Is Nothing Then
            feeCalc = New AnnualFeeCalc With {
                .CountyCode = GetCookie(Cookie.AirsNumber).Substring(3, 3),
                .EntryDate = Now.Date
            }
        Else
            feeCalc = CType(ViewState(NameOf(feeCalc)), AnnualFeeCalc)
        End If

        'Javascript pop up window for final submission confirmation
        btnSubmit.Attributes.Add("onclick", "return confirm('Are you sure you want to make the final submission?');")

        If Not Page.IsPostBack Then
            tabFeeCalculation.Visible = False
            lblMessage.Visible = True

            ddlFeeYear.Items.Add("-Select Year-")
            Dim dt As DataTable = CheckFinalSubmit(currentAirs)

            If dt.Rows.Count > 0 Then
                'Don't pre-select any Year. Let the user select a year they want to work on.
                For Each row As DataRow In dt.Rows
                    Dim intsubmittal As Integer = GetNullable(Of Integer)(row.Item("intsubmittal"))
                    ddlFeeYear.Items.Add(New ListItem(row.Item("intyear").ToString, row.Item("intyear").ToString & intsubmittal.ToString))
                Next
            End If
        End If

        If ddlFeeYear.SelectedItem.Text = "-Select Year-" Then
            feeYear = Nothing
        Else
            feeYear = CInt(ddlFeeYear.SelectedItem.Text)
            feeYearCompleted = ddlFeeYear.SelectedValue.Substring(4) = "1"
        End If
    End Sub

    Private Sub AnnualFees_Default_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        ViewState.Add(NameOf(feeCalc), feeCalc)
    End Sub

#Region "Button Click Events"

    Private Function DoubleCheckFeeYear() As Boolean
        If Not feeYear.HasValue Then
            lblMessage.Visible = True
            btnBeginFeeReport.Visible = False
            tabFeeCalculation.Visible = False
            Return False
        Else
            tabFeeCalculation.Visible = Not feeYearCompleted
            Return True
        End If
    End Function

    Protected Sub btnBeginFeeReport_Click(sender As Object, e As EventArgs) Handles btnBeginFeeReport.Click
        BeginFeeReport()
    End Sub

    Private Sub BeginFeeReport()
        If Not DoubleCheckFeeYear() Then Return

        If info Is Nothing Then
            info = GetFacilityCommunicationInfo(currentAirs, CommunicationCategory.Fees)
        End If

        If info Is Nothing OrElse
          (CommunicationCategory.Fees.CommunicationPreferenceEnabled AndAlso
            Not info.Preference.IsConfirmed) OrElse
          info.Mail Is Nothing Then

            pnlFeeContactInfo.Visible = False
            pContactInfoMissing.Visible = True
            btnVerifyContact.Enabled = False
        Else
            pnlFeeContactInfo.Visible = True
            pContactInfoMissing.Visible = False
            btnVerifyContact.Enabled = True
        End If

        pnlFeeContact.Visible = True
        pnlFeeCalculation.Visible = False
        pnlFeeSignature.Visible = False
        pnlFeeSubmit.Visible = False

        UserTabs.ActiveTab = tabFeeCalculation
    End Sub

    Protected Sub btnVerifyContact_Click(sender As Object, e As EventArgs) Handles btnVerifyContact.Click
        If Not DoubleCheckFeeYear() Then Return

        ' Load fee data from database
        ' If Emissions data is already loaded, then do not re-load

        If String.IsNullOrEmpty(lblTotalFee.Text) Then
            LoadFeeData()
        End If

        pFinalSubmitError.Visible = False
        pnlFeeContact.Visible = False
        pnlFeeCalculation.Visible = True
    End Sub

    Protected Sub btnSavePnlFeeCalc_Click(sender As Object, e As EventArgs) Handles btnSavePnlFeeCalc.Click
        If chkNSPSExempt.Checked Then
            If Not AnyNspsExemptionSelected() Then
                pNspsExemptionWarning.Visible = True
                Return
            End If

            pNspsExemptionWarning.Visible = False
        End If

        RecalculateFees()

        If Not SaveFeeData() Then
            lblSaveFeeCalcMessage.Visible = True
            lblSaveFeeCalcMessage.Text = "There was an error saving the fee calculations. Please double-check your entries."
            Return
        End If

        LoadSignAndPay()
        pnlFeeCalculation.Visible = False
        pnlFeeSignature.Visible = True
    End Sub

    Private Function AnyNspsExemptionSelected() As Boolean
        For Each item As ListItem In NspsExemptionsChecklist.Items
            If item.Selected Then Return True
        Next

        Return False
    End Function

    Protected Sub btnSavepnlSign_Click(sender As Object, e As EventArgs) Handles btnSavepnlSign.Click
        If rblPaymentType.SelectedIndex = 0 Then
            txtPayType.Text = "Entire Annual Year"
        Else
            txtPayType.Text = "Four Quarterly Payments"
        End If

        If Not SavePayandSignInfo() Then
            lblSaveFeeCalcMessage.Visible = True
            lblSaveFeeCalcMessage.Text = "There was an error saving the signature info. Please double-check your entries."
            Return
        End If

        pnlFeeSignature.Visible = False
        pnlFeeSubmit.Visible = True
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        If ActiveInvoiceExists(currentAirs, feeYear.Value) Then
            feeYearCompleted = True
            ResetPage()
            Return
        End If

        If UpdateDatabase() Then
            Page.Dispose()
            Response.BufferOutput = True
            Response.Redirect($"~/Invoice/?FeeYear={feeYear.Value}&Facility={currentAirs.ShortString}")
        Else
            pFinalSubmitError.Visible = True
            pnlFeeContact.Visible = True
            pnlFeeCalculation.Visible = False
            pnlFeeSignature.Visible = False
            pnlFeeSubmit.Visible = False
        End If
    End Sub

    Protected Sub btnCancelFeeCalcSubmit_Click(sender As Object, e As EventArgs) Handles btnCancelFeeCalc.Click
        pnlFeeCalculation.Visible = False
        pnlFeeContact.Visible = True
    End Sub

    Protected Sub btnCancelSignature_Click(sender As Object, e As EventArgs) Handles btnCancelSignature.Click
        pnlFeeSignature.Visible = False
        pnlFeeCalculation.Visible = True
    End Sub

    Protected Sub btnCancelSubmit_Click(sender As Object, e As EventArgs) Handles btnCancelSubmit.Click
        pnlFeeSubmit.Visible = False
        pnlFeeSignature.Visible = True
    End Sub

#End Region

#Region "Fee Calculation Functions"

    Public Sub RecalculateFees()
        ' Calculate
        If ddlClass.SelectedValue = "A" Then
            If Not IsNumeric(txtVOCTons.Text) Then txtVOCTons.Text = "0"
            If Not IsNumeric(txtNOxTons.Text) Then txtNOxTons.Text = "0"
            If Not IsNumeric(txtPMTons.Text) Then txtPMTons.Text = "0"
            If Not IsNumeric(txtSO2Tons.Text) Then txtSO2Tons.Text = "0"
        Else
            txtVOCTons.Text = "0"
            txtNOxTons.Text = "0"
            txtSO2Tons.Text = "0"
            txtPMTons.Text = "0"
        End If

        feeCalc.Emissions.VocTons = CInt(txtVOCTons.Text)
        feeCalc.Emissions.NoxTons = CInt(txtNOxTons.Text)
        feeCalc.Emissions.PmTons = CInt(txtPMTons.Text)
        feeCalc.Emissions.So2Tons = CInt(txtSO2Tons.Text)

        feeCalc.RulePart70Applies = chkPart70Source.Checked
        feeCalc.RuleSmApplies = chkSmSource.Checked
        feeCalc.RuleNspsApplies = chkNSPS1.Checked AndAlso Not chkNSPSExempt.Checked
        feeCalc.EntryDate = Now.Date

        ' Display values
        lblVOCFee.Text = feeCalc.CalcVocFee.ToString("c")
        lblNOxFee.Text = feeCalc.CalcNoxFee.ToString("c")
        lblPMFee.Text = feeCalc.CalcPmFee.ToString("c")
        lblSO2Fee.Text = feeCalc.CalcSo2Fee.ToString("c")

        lblEmissionsTotal.Text = feeCalc.Emissions.Total.ToString
        lblEmissionFeeTotal.Text = feeCalc.CalcEmissionFee.ToString("c")

        lblpart70SMFee.Text = Math.Max(feeCalc.CalcPart70Fee, feeCalc.CalcSmFee).ToString("c")
        lblPart70MaintenanceFee.Text = feeCalc.CalcMaintenanceFee.ToString("c")
        lblNSPSFee.Text = feeCalc.CalcNspsFee.ToString("c")

        lblTotalFee.Text = feeCalc.CalcTotalFee.ToString("c")
        lblPayment.Text = feeCalc.CalcFeeSubtotal.ToString("c")

        If feeCalc.CalcAdminFee > 0 Then
            lblAdminFeeText.Text = "Admin Fee (due if submitted after " &
                feeCalc.FeeRates.AdminFeeDate.ToString(LongishDateFormat) & "): "
            lblAdminFeeText.Visible = True
            lbladminfeeSign.Visible = True
            lbladminfeeSign.Text = "Admin fee:"
            lblAdminFeeAmount.Visible = True
            lblAdminfeeamtSign.Visible = True

            lblAdminFeeAmount.Text = feeCalc.CalcAdminFee.ToString("c")
            lblAdminfeeamtSign.Text = feeCalc.CalcAdminFee.ToString("c")
        End If

        If feeCalc.CalcTotalFee < 10000D Then
            rblPaymentType.SelectedIndex = 0
            txtPayType.Text = "Entire Annual Year"
            rblPaymentType.Enabled = False
        Else
            rblPaymentType.Enabled = True
        End If
    End Sub

#End Region

#Region "Load from Database"

    Private Sub LoadFeeData()
        Dim dr As DataRow = GetClassInfo(feeYear.Value)

        Dim initClass As String = "B"

        If dr IsNot Nothing Then
            'Getting details for the facility from table fs_mailout.
            'This table has all the information that goes into top panel and
            'is disabled so that the user cannot change it.

            initClass = GetNullableString(dr.Item("strclass"))
            Select Case initClass
                Case "A"
                    lblCurrentClass.Text = "A - Major Source"
                Case "SM"
                    lblCurrentClass.Text = "SM - Synthetic Minor Source"
                Case "B"
                    lblCurrentClass.Text = "B - Minor Source"
                Case "PR"
                    lblCurrentClass.Text = "PR - Permit-by-Rule Source"
                Case Else
                    ResetPage()
                    tabFeeCalculation.Visible = False
                    btnBeginFeeReport.Visible = False
                    pIncorrectClass.Visible = True
                    Return
            End Select

            chkPart70Source.Checked = False
            chkSmSource.Checked = False

            UpdateUiFromClass(initClass)

            Dim initNsps As Boolean = GetNullable(Of Integer)(dr.Item("strnsps")) = 1
            chkNSPS.Checked = initNsps
            chkNSPS1.Checked = initNsps
            chkNSPS1.Visible = Not initNsps
            chkNSPS1.Enabled = Not initNsps
            lblNspsRemovalNotice.Visible = initNsps
            lblNspsRemovalNotice.Text = "If it is believed that this stationary source is not subject to any NSPS standard, call the number listed in Section 6.0 of this manual."
            chkNSPSExempt.Visible = initNsps

            If initNsps Then
                chkNSPSExempt.Checked = GetNullable(Of Boolean)(dr.Item("NspsFeeExempt"))
                If chkNSPSExempt.Checked Then
                    LoadNSPSExemptList()
                    NspsExemptionsChecklist.Visible = True
                End If
            End If

            chkPart70Source.Checked = GetNullable(Of Integer)(dr.Item("strpart70")) = 1
        End If

        'Next get data from the fs_feeauditeddata table
        dr = GetExistingFeeData(feeYear.Value)

        If dr IsNot Nothing Then
            'If the NumFeeRate in the AuditedData table is different, then replace the pertonrate with the new value
            If Not Convert.IsDBNull(dr.Item("numfeerate")) AndAlso CDec(dr.Item("numfeerate")) <> 0 Then
                feeCalc.FeeRates.PerTonRate = CDec(dr.Item("numfeerate"))
            End If

            txtVOCTons.Text = GetNullable(Of Integer)(dr.Item("intvoctons")).ToString
            txtPMTons.Text = GetNullable(Of Integer)(dr.Item("intpmtons")).ToString
            txtSO2Tons.Text = GetNullable(Of Integer)(dr.Item("intso2tons")).ToString
            txtNOxTons.Text = GetNullable(Of Integer)(dr.Item("intnoxtons")).ToString

            If GetNullable(Of Integer)(dr.Item("strnsps")) = 1 Then
                chkNSPS1.Checked = True
            Else
                chkNSPS.Checked = False
                chkNSPS1.Visible = True
            End If

            If GetNullable(Of Integer)(dr.Item("strnspsexempt")) = 1 Then
                chkNSPSExempt.Checked = True
                If NspsExemptionsChecklist.Items.Count < 1 Then LoadNSPSExemptList()
            Else
                chkNSPSExempt.Checked = False
            End If

            NspsExemptionsChecklist.Visible = chkNSPSExempt.Checked

            chkPart70Source.Checked = GetNullable(Of Integer)(dr.Item("strpart70")) = 1
            chkSmSource.Checked = GetNullable(Of Integer)(dr.Item("strsyntheticminor")) = 1

            If Convert.IsDBNull(dr.Item("strclass")) Then
                ddlClass.SelectedValue = initClass
            Else
                ddlClass.SelectedValue = dr.Item("strclass").ToString
            End If

            UpdateUiFromClass(ddlClass.SelectedValue)

            Dim nspsReason As String = GetNullableString(dr.Item("strnspsexemptreason"))

            If Not String.IsNullOrEmpty(nspsReason) Then
                Dim nspsItems As String() = nspsReason.ToString().Split(","c)
                For Each item As String In nspsItems
                    Dim currentCheckBox As ListItem = NspsExemptionsChecklist.Items.FindByValue(item)

                    If currentCheckBox IsNot Nothing Then
                        currentCheckBox.Selected = True
                    End If
                Next
            End If
        Else
            ddlClass.SelectedValue = initClass
        End If

        RecalculateFees()
    End Sub

    Protected Sub LoadSignAndPay()
        'If Signature data is already loaded, then do not re-load
        If String.IsNullOrEmpty(txtOwner.Text) Then
            Dim dr As DataRow = GetPaySubmitInfo(feeYear.Value)

            If dr IsNot Nothing Then
                txtPayType.Text = GetNullableString(dr.Item("STRPAYMENTPLAN"))
                txtOwner.Text = GetNullableString(dr.Item("strofficialname"))
                txtOwnerTitle.Text = GetNullableString(dr.Item("strofficialtitle"))
                txtComments.Text = GetNullableString(dr.Item("strcomment"))
            End If

            If txtPayType.Text = "Four Quarterly Payments" Then
                rblPaymentType.SelectedIndex = 1
            Else
                rblPaymentType.SelectedIndex = 0
            End If
        End If
    End Sub

    Protected Sub LoadNSPSExemptList()
        If feeYear.HasValue Then
            Dim dt As DataTable = GetNSPSExemptList(feeYear.Value)

            If dt IsNot Nothing Then
                For Each row As DataRow In dt.Rows
                    NspsExemptionsChecklist.Items.Add(New ListItem(CStr(row.Item("Reason")), CStr(row.Item("ReasonID"))))
                Next
            End If
        End If
    End Sub

    Protected Sub LoadAnnualFeesHistory()
        If grdFeeHistory.DataSource Is Nothing Then
            grdFeeHistory.DataSource = GetAnnualFeeHistory(currentAirs)
            grdFeeHistory.DataBind()
        End If
    End Sub

#End Region

#Region "Save and Update to Database"

    Private Function SaveFeeData() As Boolean
        Dim nspsreason As String = "0"
        If chkNSPSExempt.Checked Then
            Dim nspsReasons As New List(Of String)

            For Each item As ListItem In NspsExemptionsChecklist.Items
                If item.Selected Then nspsReasons.Add(item.Value)
            Next

            nspsreason = ConcatNonEmptyStrings(",", nspsReasons)
        End If

        Dim params As SqlParameter() = {
            New SqlParameter("@AirsNumber", currentAirs.DbFormattedString),
            New SqlParameter("@FeeYear", feeYear.Value),
            New SqlParameter("@VocTons", feeCalc.Emissions.VocTons),
            New SqlParameter("@NoxTons", feeCalc.Emissions.NoxTons),
            New SqlParameter("@PmTons", feeCalc.Emissions.PmTons),
            New SqlParameter("@So2Tons", feeCalc.Emissions.So2Tons),
            New SqlParameter("@Part70Fee", feeCalc.CalcPart70Fee),
            New SqlParameter("@MaintenanceFee", feeCalc.CalcMaintenanceFee),
            New SqlParameter("@SmFee", feeCalc.CalcSmFee),
            New SqlParameter("@NspsFee", feeCalc.CalcNspsFee),
            New SqlParameter("@TotalFee", feeCalc.CalcTotalFee),
            New SqlParameter("@FeeRate", feeCalc.FeeRates.PerTonRate),
            New SqlParameter("@CalculatedFee", feeCalc.CalcEmissionFee),
            New SqlParameter("@AdminFee", feeCalc.CalcAdminFee),
            New SqlParameter("@NspsExempt", If(chkNSPSExempt.Checked, "1", "0")),
            New SqlParameter("@NspsExemptReason", If(chkNSPSExempt.Checked, nspsreason, "0")),
            New SqlParameter("@Class", ddlClass.SelectedValue),
            New SqlParameter("@NspsApplies", If(chkNSPS1.Checked, "1", "0")),
            New SqlParameter("@Part70Applies", If(chkPart70Source.Checked, "1", "0")),
            New SqlParameter("@SmApplies", If(chkSmSource.Checked, "1", "0")),
            New SqlParameter("@UpdateUser", "GECO||" & currentUser.Email),
            New SqlParameter("@UserId", currentUser.UserId)
        }

        Return DB.SPRunCommand("dbo.PD_FEE_SaveFeeData", params)
    End Function

    Private Function SavePayandSignInfo() As Boolean
        Dim qList As New List(Of String)
        Dim pList As New List(Of SqlParameter())

        Dim SQL1 As String = "Update fs_feedata set " &
            "STRPAYMENTPLAN = @PayType, " &
            "strofficialname = @Owner, " &
            "strofficialtitle = @OwnerTitle, " &
            "strcomment = @Comments, " &
            "updatedatetime = getdate(), " &
            "updateuser = @UEmail " &
            "where strairsnumber = @Airs " &
            "and numfeeyear = @FeeYear "

        Dim params1 As SqlParameter() = {
            New SqlParameter("@PayType", txtPayType.Text),
            New SqlParameter("@Owner", txtOwner.Text),
            New SqlParameter("@OwnerTitle", txtOwnerTitle.Text),
            New SqlParameter("@Comments", txtComments.Text),
            New SqlParameter("@UEmail", "GECO||" & currentUser.Email),
            New SqlParameter("@Airs", currentAirs.DbFormattedString),
            New SqlParameter("@FeeYear", feeYear.Value)
        }

        'Add SQL and Parameters to each list
        qList.Add(SQL1)
        pList.Add(params1)

        Dim Sql2 As String = "Update fs_admin set numcurrentstatus = 7, " &
            "updatedatetime = getdate(), " &
            "DATSTATUSDATE = getdate(), " &
            "updateuser = @UEmail " &
            "where strairsnumber = @Airs " &
            "And numfeeyear = @FeeYear " &
            "And numcurrentstatus < 7"

        Dim params2 As SqlParameter() = {
            New SqlParameter("@UEmail", "GECO||" & currentUser.Email),
            New SqlParameter("@Airs", currentAirs.DbFormattedString),
            New SqlParameter("@FeeYear", feeYear.Value)
        }

        'Add SQL and Parameters to each list
        qList.Add(Sql2)
        pList.Add(params2)

        Return DB.RunCommand(qList, pList)
    End Function

    Private Function SaveFinalSubmit() As Boolean
        Dim qList As New List(Of String)
        Dim pList As New List(Of SqlParameter())

        qList.Add("Update fs_admin Set " &
                  "numcurrentstatus = 8, " &
                  "intsubmittal = 1, " &
                  "updatedatetime = getdate(), " &
                  "DATSTATUSDATE = getdate(), " &
                  "datsubmittal = getdate(), " &
                  "updateuser = @UpdUser " &
                  "where strairsnumber = @airs " &
                  "and numfeeyear = @FY")

        pList.Add({
                  New SqlParameter("@UpdUser", "GECO||" & currentUser.Email),
                  New SqlParameter("@airs", currentAirs.DbFormattedString),
                  New SqlParameter("@FY", feeYear.Value)
                  })

        Dim numberOfInvoices As Integer
        Dim payType As Integer

        If txtPayType.Text = "Entire Annual Year" Then
            numberOfInvoices = 1
        Else 'Four Quarterly Payments
            numberOfInvoices = 4
        End If

        Dim invoiceSql As String = "Insert into fs_feeinvoice " &
            "(invoiceid, numfeeyear, strairsnumber, numamount, datinvoicedate, updatedatetime, " &
            "createdatetime, updateuser, strpaytype, strinvoicestatus, active) " &
            "values(Next Value for feeinvoice_id, @feeYear, @AirsNo, @AmtDue, @InvDate, " &
            "getdate(), getdate(), @UpdUser, @PayType, @InvStatus, '1')"

        Dim dt As Date
        Dim i As Integer = 1

        While i <= numberOfInvoices
            Select Case i
                Case 1
                    dt = If(numberOfInvoices = 1, feeCalc.FeeRates.DueDate, feeCalc.FeeRates.FirstQuarterDueDate)
                    payType = If(numberOfInvoices = 1, 1, 2)
                Case 2
                    dt = feeCalc.FeeRates.SecondQuarterDueDate
                    payType = 3
                Case 3
                    dt = feeCalc.FeeRates.ThirdQuarterDueDate
                    payType = 4
                Case 4
                    dt = feeCalc.FeeRates.FourthQuarterDueDate
                    payType = 5
            End Select

            Dim invoiceParams As SqlParameter() = {
                New SqlParameter("@feeYear", feeYear.Value),
                New SqlParameter("@AirsNo", currentAirs.DbFormattedString),
                New SqlParameter("@AmtDue", feeCalc.CalcTotalFee / numberOfInvoices),
                New SqlParameter("@InvDate", dt),
                New SqlParameter("@UpdUser", "GECO||" & currentUser.Email),
                New SqlParameter("@PayType", payType),
                New SqlParameter("@InvStatus", If(feeCalc.CalcTotalFee = 0, "1", "0"))
            }

            'Add SQL and Parameters to each list
            qList.Add(invoiceSql)
            pList.Add(invoiceParams)

            i += 1
        End While

        qList.Add("Update fs_feedata set " &
                  "strconfirmationnumber = @conf, " &
                  "strconfirmationuser = @UID, " &
                  "updatedatetime = getdate(), " &
                  "updateuser = @Uemail " &
                  "where strairsnumber = @AIRSno " &
                  "and numfeeyear = @FY")

        pList.Add({
                  New SqlParameter("@conf", currentAirs.ShortString & "-" & Now.ToString("yyyyMMddhhmm")),
                  New SqlParameter("@UID", currentUser.UserId),
                  New SqlParameter("@Uemail", "GECO||" & currentUser.Email),
                  New SqlParameter("@AIRSno", currentAirs.DbFormattedString),
                  New SqlParameter("@FY", feeYear.Value)
                  })

        Return DB.RunCommand(qList, pList)
    End Function

    Private Function UpdateDatabase() As Boolean
        Page.Validate()

        If Not Page.IsValid Then
            Return False
        End If

        Return SaveFinalSubmit()
    End Function

#End Region

#Region "Controls Auto Postback"

    Protected Sub ddlClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlClass.SelectedIndexChanged
        UpdateUiFromClass(ddlClass.SelectedValue)
        RecalculateFees()
    End Sub

    Private Sub UpdateUiFromClass(selClass As String)
        Select Case selClass
            Case "A"
                pnlEmissions.Visible = True
                chkPart70Source.Checked = True
                chkPart70Source.Enabled = False
                chkSmSource.Checked = False

            Case "B", "PR"
                pnlEmissions.Visible = False
                chkPart70Source.Enabled = True
                chkSmSource.Checked = False

            Case "SM"
                pnlEmissions.Visible = False
                chkPart70Source.Enabled = True
                chkSmSource.Checked = True

        End Select
    End Sub

    Protected Sub chkNSPS1_CheckedChanged(sender As Object, e As EventArgs) Handles chkNSPS1.CheckedChanged
        chkNSPSExempt.Checked = Not chkNSPS1.Checked
        chkNSPSExempt.Visible = chkNSPS1.Checked

        If Not chkNSPS1.Checked Then
            chkNSPSExempt.Checked = False
        End If

        NspsExemptionsChecklist.Visible = chkNSPS1.Checked AndAlso chkNSPSExempt.Checked
        feeCalc.RuleNspsApplies = chkNSPS1.Checked AndAlso Not chkNSPSExempt.Checked
        pNspsExemptionWarning.Visible = False

        If chkNSPSExempt.Checked Then
            If NspsExemptionsChecklist.Items.Count < 1 Then LoadNSPSExemptList()
        End If

        RecalculateFees()
    End Sub

    Protected Sub chkNSPSExempt_CheckedChanged(sender As Object, e As EventArgs) Handles chkNSPSExempt.CheckedChanged
        NspsExemptionsChecklist.Visible = chkNSPSExempt.Checked
        feeCalc.RuleNspsApplies = chkNSPS1.Checked AndAlso Not chkNSPSExempt.Checked

        NspsExemptionsChecklist.Visible = chkNSPS1.Checked AndAlso chkNSPSExempt.Checked
        feeCalc.RuleNspsApplies = chkNSPS1.Checked AndAlso Not chkNSPSExempt.Checked
        pNspsExemptionWarning.Visible = False

        If chkNSPSExempt.Checked Then
            If NspsExemptionsChecklist.Items.Count < 1 Then LoadNSPSExemptList()
        End If

        RecalculateFees()
    End Sub

    Protected Sub chkPart70Source_CheckedChanged(sender As Object, e As EventArgs) Handles chkPart70Source.CheckedChanged
        RecalculateFees()
    End Sub

    Protected Sub ddlFeeYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlFeeYear.SelectedIndexChanged
        ResetPage()
    End Sub

    Private Sub ResetPage()
        UserTabs.ActiveTab = tabWelcome
        pIncorrectClass.Visible = False

        ClearAll(pnlFeeCalculation) 'Clear controls in the Calculation Tab
        ClearAll(pnlFeeSignature) 'Clear controls in the Sign and Pay Tab

        If Not feeYear.HasValue Then
            lblDeadline.Visible = False
            linkInvoice.Visible = False
            linkInvoice.NavigateUrl = Nothing
            btnBeginFeeReport.Visible = False
            lblMessage.Visible = True
            feeRatesSection.Visible = False
            tabFeeCalculation.Visible = False
        Else
            feeCalc.FeeRates = GetFeeRates(feeYear.Value)
            info = Nothing

            NspsExemptionsChecklist.Items.Clear()
            lblMessage.Visible = False
            feeRatesSection.Visible = True

            tabFeeCalculation.Visible = Not feeYearCompleted
            lblDeadline.Visible = Not feeYearCompleted
            linkInvoice.Visible = feeYearCompleted
            btnBeginFeeReport.Visible = Not feeYearCompleted

            pnlFeeContact.Visible = True
            pnlFeeCalculation.Visible = False
            pnlFeeSignature.Visible = False
            pnlFeeSubmit.Visible = False

            If feeYearCompleted Then
                linkInvoice.NavigateUrl = $"~/Invoice/?FeeYear={feeYear.Value}&Facility={currentAirs.ShortString}"
            Else
                lblDeadline.Text = $"Submission Deadline: {feeCalc.FeeRates.DueDate.ToString(LongishDateFormat)}"
            End If
        End If
    End Sub

    Private Sub PollutantAmount_TextChanged(sender As Object, e As EventArgs) _
        Handles txtVOCTons.TextChanged, txtNOxTons.TextChanged, txtPMTons.TextChanged, txtSO2Tons.TextChanged

        RecalculateFees()
    End Sub

#End Region

    Protected Sub UserTabs_ActiveTabChanged(sender As Object, e As EventArgs) Handles UserTabs.ActiveTabChanged
        Select Case UserTabs.ActiveTab.ID
            Case NameOf(tabWelcome)
                DoubleCheckFeeYear()

            Case NameOf(tabHistory)
                LoadAnnualFeesHistory()

            Case NameOf(tabFeeCalculation)
                BeginFeeReport()

        End Select
    End Sub

End Class
