Imports System.Data.SqlClient
Imports GECO.GecoModels

Partial Class AnnualFees_Default
    Inherits Page

#Region "Global Variables"

    Private Property currentUser As GecoUser
    Private Property currentAirs As ApbFacilityId

    Dim feetotal, feepart70, feesm, feensps, feecalculated As Double

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        currentUser = GetCurrentUser()
        currentAirs = GetCookie(Cookie.AirsNumber)

        'Javascript pop up window for final submission confirmation
        btnSubmit.Attributes.Add("onclick",
                "return confirm('Are you sure you want to make the Final Submission?');")
        'Clear the Global message text and make it invisible
        lblMessage.Text = ""
        lblMessage.Visible = False

        If Not Page.IsPostBack Then
            lblMessage.Text = "Please select a year to work on."
            lblMessage.Visible = True

            Try
                'Use FindControl to get the controls from the master page
                'Add the User data in the Top Header
                Dim mpUserLabel, mpFacilityLabel As Label
                mpUserLabel = CType(Master.FindControl("lblUserName"), Label)
                mpUserLabel.Text = "Welcome, " & currentUser.FullName & " | "
                mpFacilityLabel = CType(Master.FindControl("lblFacilityName"), Label)
                mpFacilityLabel.Text = "Facility: " & GetFacilityName(currentAirs) & " | "
                Dim mpAirsLabel As Label = CType(Master.FindControl("lblAirsNo"), Label)
                mpAirsLabel.Text = "AIRS No: " & currentAirs.FormattedString

                'Check if the final submission has already been made.
                'If the final submission has been made, the user cannot
                'access the fee forms, they will be directed to view
                'the summary report and payment coupons.
                Dim dt As DataTable = CheckFinalSubmit(currentAirs)

                If dt.Rows.Count > 0 Then
                    'Don't pre-select any Year. Let the user select a year they want to work on.
                    ddlFeeYear.Items.Add("-Select Year-")
                    UserTabs.Tabs(2).Enabled = False
                    UserTabs.Tabs(3).Enabled = False
                    For Each row As DataRow In dt.Rows
                        Dim intsubmittal As String
                        If IsDBNull(row.Item("intsubmittal")) Then
                            intsubmittal = "0"
                        Else
                            intsubmittal = row.Item("intsubmittal")
                        End If
                        ddlFeeYear.Items.Add(New ListItem(row.Item("intyear"), row.Item("intyear") & intsubmittal))
                    Next
                Else
                    ddlFeeYear.Items.Add("-Select Year-")
                    UserTabs.Tabs(2).Enabled = False
                    UserTabs.Tabs(3).Enabled = False
                End If
            Catch exThreadAbort As System.Threading.ThreadAbortException
            Catch ex As Exception
                ErrorReport(ex)
            End Try
        End If

    End Sub

#Region "Button Click Events"

    Private Sub btnSavePnlFeeCalc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavePnlFeeCalc.Click
        Try
            If chkNSPSExempt.Checked Then
                Dim chkNSPSReason As Boolean
                Dim i As Integer
                For i = 0 To (cblNSPSExempt.Items.Count - 1)
                    If (cblNSPSExempt.Items(i).Selected) Then
                        chkNSPSReason = True
                        Exit For
                    End If
                Next
                If Not chkNSPSReason Then
                    lblcblnspsreason.Visible = True
                    lblcblnspsreason.Text = "Please select all the NSPS exemptions that apply to your facility."
                    Exit Sub
                Else
                    lblcblnspsreason.Visible = False
                End If
            End If

            If chkDidNotOperate.Checked Then
                Dim chkNoOperate As Boolean
                Dim i As Integer
                For i = 0 To (rblNoOperateReason.Items.Count - 1)
                    If (rblNoOperateReason.Items(i).Selected) Then
                        chkNoOperate = True
                        Exit For
                    End If
                Next
                If Not chkNoOperate Then
                    lblNoOperateReason.Visible = True
                    lblNoOperateReason.Text = "You must select at least one checkbox"
                    Exit Sub
                Else
                    lblNoOperateReason.Visible = False
                End If
            End If

            PerformCalculations()
            SaveFeeCalcInfo()

            If txtOwner.Text = "" Then
                'Load Data from database
                LoadSignandPay()
                lblDate.Text = Format$(Now, "dd-MMM-yyyy hh:mm")
                If txtPayType.Text = "Four Quarterly Payments" Then
                    rblPaymentType.SelectedIndex = 1
                Else
                    rblPaymentType.SelectedIndex = 0
                End If
            Else
            End If
            UserTabs.ActiveTabIndex = 3

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnUpdateContact_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            lblContactMsg.Visible = False
            'If the facility indicated change in facility info
            'check to see if there is a change indeed.
            If ddlFacilityInfoChange.SelectedIndex = 1 Then
                Dim i As Integer = ValidateFacilityInfoChange()
                If i = -1 Then 'No Changes
                    lblContactMsg.Visible = True
                    lblContactMsg.Text = "You have indicated that the Facility Information is incorrect, " &
                        "but you have not made any changes to the existing information."
                    Exit Sub
                Else
                    'Changes Detected, Continue
                End If
            Else
            End If
            SaveFacilityInfo()
            lblContactMsg.Visible = True
            lblContactMsg.Text = "Contact information saved! "

            If Not linkInvoice.Visible AndAlso lblTotalFee.Text = "" AndAlso feeyear.Text <> "" Then
                'Load Data from database
                LoadFeeCalculations()
                ClassCalculate()
                UserTabs.ActiveTabIndex = 2
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub btnCalculate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCalculate.Click

        PerformCalculations()

    End Sub

    Private Sub btnSavepnlSign_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSavepnlSign.Click
        Try
            If rblPaymentType.SelectedIndex = 0 Then
                txtPayType.Text = "Entire Annual Year"
            Else
                txtPayType.Text = "Four Quarterly Payments"
            End If

            SavePayandSignInfo()
            pnlSignandPay.Visible = False
            pnlSubmit.Visible = True
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub btnCancelSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancelSubmit.Click
        Try
            pnlSignandPay.Visible = True
            pnlSubmit.Visible = False
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub btnSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        Try
            Dim pagevalid As Boolean = UpdateDatabase()

            If pagevalid Then
                SaveConfirmation()
                Page.Dispose()
                Response.BufferOutput = True
                Response.Redirect(String.Format("~/Invoice/?FeeYear={0}&Facility={1}", feeyear.Text, currentAirs.ShortString))
            Else
                pnlSignandPay.Visible = True
                pnlSubmit.Visible = False
                lblMessage.Text = "Please click on the Fee Contact and Fee Calculations tabs and verify the data"
                lblMessage.Visible = True
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnProceed_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProceed.Click
        Try
            If ddlFeeYear.SelectedItem.Text = "-Select Year-" Then
                lblMessage.Text = "Please select a year to work on"
                lblMessage.Visible = True
                Exit Sub
            End If

            If txtFName.Text = "" Then
                'Load Data from database
                LoadFacilityContact()
                LoadFacilityInfo()
                rblFeeContact.SelectedIndex = 0
                btnUpdateContact.Enabled = True
            End If

            UserTabs.ActiveTabIndex = 1
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

#Region "Fee Calculation Functions"
    Private Sub ClassCalculate()
        Try
            Select Case ddlClass.SelectedValue

                Case "A"

                    pnlEmissions.Visible = True
                    btnCalculate.Visible = True
                    chkPart70SM.Items.FindByValue("Part 70 Fee").Selected = True
                    If IsNumeric(titlevfee.Text) Then
                        feepart70 = CDbl(titlevfee.Text)
                    Else
                        feepart70 = 0
                    End If

                    ResetFees()

                    'lblpart70SMFee.Text = String.Format("{0:C}", part70smfee)

                Case "B", "PR"

                    pnlEmissions.Visible = False
                    btnCalculate.Visible = True
                    ResetFees()

                Case "SM"

                    pnlEmissions.Visible = False
                    btnCalculate.Visible = True
                    chkPart70SM.Items.FindByValue("Synthetic Minor Fee").Selected = True
                    If IsNumeric(smfee.Text) Then
                        feesm = CDbl(smfee.Text)
                    Else
                        feesm = 0
                    End If

                    ResetFees()

                Case Else

            End Select

            CalculateFees()
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub ResetFees()
        Try
            If ddlClass.SelectedValue = "A" Then
                If txtVOCTons.Text = "" Then
                    txtVOCTons.Text = 0
                End If
                If txtPMTons.Text = "" Then
                    txtPMTons.Text = 0
                End If
                If txtNOxTons.Text = "" Then
                    txtNOxTons.Text = 0
                End If
                If txtSO2Tons.Text = "" Then
                    txtSO2Tons.Text = 0
                End If

            Else
                txtVOCTons.Text = 0
                txtNOxTons.Text = 0
                txtSO2Tons.Text = 0
                txtPMTons.Text = 0
                lblVOCFee.Text = 0
                lblNOxFee.Text = 0
                lblSO2Fee.Text = 0
                lblPMFee.Text = 0
            End If
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Function PollutantVOCNOx(ByVal tons As Integer) As Double
        Try
            Dim county = Mid(GetCookie(Cookie.AirsNumber), 1, 3)

            If county = "057" OrElse county = "063" OrElse county = "067" OrElse county = "077" OrElse county = "089" _
                OrElse county = "097" OrElse county = "113" OrElse county = "117" OrElse county = "121" _
                OrElse county = "135" OrElse county = "151" OrElse county = "223" OrElse county = "247" Then
                chkNonAttainment.Checked = True
            Else
                chkNonAttainment.Checked = False
            End If
            Dim fee As Double

            'For 1-hour zone non-attainment counties, the VOC/NOx emissions
            'threshold is 25 tons

            If chkNonAttainment.Checked Then
                If tons <= CDbl(numnathres.Text) Then
                    fee = 0.0
                Else
                    fee = tons * CDbl(pertonrate.Text)
                End If
            Else
                If tons <= CDbl(numaathres.Text) Then
                    fee = 0.0
                Else
                    fee = tons * CDbl(pertonrate.Text)
                End If

            End If

            Return fee

        Catch exThreadAbort As System.Threading.ThreadAbortException
            Return Nothing
        Catch ex As Exception
            ErrorReport(ex)
            Return Nothing
        End Try
    End Function

    Private Function PollutantPMSO2(ByVal tons As Integer) As Double
        Try
            Dim county = Mid(GetCookie(Cookie.AirsNumber), 1, 3)

            If county = "057" OrElse county = "063" OrElse county = "067" OrElse county = "077" OrElse county = "089" _
                OrElse county = "097" OrElse county = "113" OrElse county = "117" OrElse county = "121" _
                OrElse county = "135" OrElse county = "151" OrElse county = "223" OrElse county = "247" Then
                chkNonAttainment.Checked = True
            Else
                chkNonAttainment.Checked = False
            End If
            Dim fee As Double

            If chkNonAttainment.Checked Then
                If tons <= CDbl(numnathres.Text) Then
                    fee = 0.0
                Else
                    fee = tons * CDbl(pertonrate.Text)
                End If
            Else
                If tons <= CDbl(numaathres.Text) Then
                    fee = 0.0
                Else
                    fee = tons * CDbl(pertonrate.Text)
                End If

            End If

            Return fee
        Catch exThreadAbort As System.Threading.ThreadAbortException
            Return Nothing
        Catch ex As Exception
            ErrorReport(ex)
            Return Nothing
        End Try
    End Function

    Private Sub PerformCalculations()
        Try
            Dim fee As Double

            If IsNumeric(txtVOCTons.Text) Then
                fee = PollutantVOCNOx(CInt(txtVOCTons.Text))
                lblVOCFee.Text = String.Format("{0:C}", fee)
            Else
                txtVOCTons.Text = ""
                lblVOCFee.Text = String.Format("{0:C}", 0.0)
            End If

            If IsNumeric(txtNOxTons.Text) Then
                fee = PollutantVOCNOx(CInt(txtNOxTons.Text))
                lblNOxFee.Text = String.Format("{0:C}", fee)
            Else
                txtNOxTons.Text = ""
                lblNOxFee.Text = String.Format("{0:C}", 0.0)
            End If

            If IsNumeric(txtPMTons.Text) Then
                fee = PollutantPMSO2(CInt(txtPMTons.Text))
                lblPMFee.Text = String.Format("{0:C}", fee)
            Else
                txtPMTons.Text = ""
                lblPMFee.Text = String.Format("{0:C}", 0.0)
            End If

            If IsNumeric(txtSO2Tons.Text) Then
                fee = PollutantPMSO2(CInt(txtSO2Tons.Text))
                lblSO2Fee.Text = String.Format("{0:C}", fee)
            Else
                txtSO2Tons.Text = ""
                lblSO2Fee.Text = String.Format("{0:C}", 0.0)
            End If

            ClassCalculate()
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub CalculateFees()
        Try
            If CInt(feeyear.Text) < 2006 AndAlso chkDidNotOperate.Checked Then
                DidNotOperate()
                Exit Sub
            End If
            If lblVOCFee.Text = "" Then
                lblVOCFee.Text = 0
            End If
            If lblPMFee.Text = "" Then
                lblPMFee.Text = 0
            End If
            If lblNOxFee.Text = "" Then
                lblNOxFee.Text = 0
            End If
            If lblSO2Fee.Text = "" Then
                lblSO2Fee.Text = 0
            End If

            feecalculated = CDbl(lblVOCFee.Text) + CDbl(lblSO2Fee.Text) +
            CDbl(lblPMFee.Text) + CDbl(lblNOxFee.Text)

            lblPart70Fee.Text = String.Format("{0:C}", feecalculated)

            If chkPart70SM.Items.FindByValue("Part 70 Fee").Selected Then
                If feecalculated < CDbl(titlevfee.Text) Then
                    feepart70 = CDbl(titlevfee.Text)
                Else
                    feepart70 = feecalculated
                End If
            End If

            If chkPart70SM.Items.FindByValue("Synthetic Minor Fee").Selected Then
                If feepart70 < CDbl(smfee.Text) Then
                    feesm = CDbl(smfee.Text)
                Else
                    feesm = 0
                End If
            End If

            If chkPart70SM.Items.FindByValue("Part 70 Fee").Selected AndAlso
                chkPart70SM.Items.FindByValue("Synthetic Minor Fee").Selected Then
                If feecalculated < CDbl(titlevfee.Text) Then
                    feepart70 = CDbl(titlevfee.Text)
                Else
                    feepart70 = feecalculated
                End If
            End If

            If feepart70 < CDbl(smfee.Text) Then
                lblpart70SMFee.Text = String.Format("{0:C}", feesm)
            Else
                lblpart70SMFee.Text = String.Format("{0:C}", feepart70)
            End If

            If chkNSPS1.Checked Then
                feensps = CDbl(nspsfee.Text)
            Else
                chkNSPSExempt.Checked = False
                lblcblnspsreason.Visible = False
                feensps = 0
            End If

            If chkNSPSExempt.Checked Then
                chkNSPS1.Checked = True
                If cblNSPSExempt.Items.Count < 1 Then
                    LoadNSPSExemptList()
                End If
                cblNSPSExempt.Visible = True
                feensps = 0
            Else
                cblNSPSExempt.Visible = False
                lblcblnspsreason.Visible = False
            End If

            lblNSPSFee.Text = String.Format("{0:C}", feensps)

            feetotal = feepart70 + feensps + feesm

            'The following three lines are just for writing to the database purpose
            'All the three labels never come into picture otherwise
            lblcalculated.Text = String.Format("{0:C}", feecalculated)
            lblpart70.Text = String.Format("{0:C}", feepart70)
            lblsm.Text = String.Format("{0:C}", feesm)
            lblcalculated.Visible = False
            lblpart70.Visible = False
            lblsm.Visible = False

            lblTotalFee.Text = String.Format("{0:C}", feetotal)
            lblPayment.Text = String.Format("{0:C}", feetotal)

            If CDbl(lblPayment.Text) < 10000 Then
                rblPaymentType.SelectedIndex = 0
                txtPayType.Text = "Entire Annual Year"
                rblPaymentType.Enabled = False
            Else
                rblPaymentType.Enabled = True
            End If

            'Added by Mahesh 01/30/2010
            'To incorporate late admin fees

            If CDate(adminfeedate.Text) < Now.Date Then
                CalulateAdminFee()
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub CalulateAdminFee()
        Try
            lblAdminFeeText.Text = "Admin Fee (due if submitted after " & CDate(adminfeedate.Text) & "): "
            lbladminfeeSign.Text = "Admin Fee:"
            lblAdminFeeText.Visible = True
            lbladminfeeSign.Visible = True
            lblAdminFeeAmount.Visible = True
            lblAdminfeeamtSign.Visible = True
            Dim intdays As Integer
            intdays = DateDiff(DateInterval.Day, CDate(adminfeedate.Text), Now.Date)
            If intdays <= 0 Then
            Else
                Dim dbladminFee As Double = 0
                dbladminFee = feetotal * intdays * (CDbl(adminfee.Text) / 100)
                lblAdminFeeAmount.Text = String.Format("{0:C}", dbladminFee)
                hidAdminFee.Value = String.Format("{0:C}", dbladminFee)
                lblAdminfeeamtSign.Text = String.Format("{0:C}", dbladminFee)
            End If
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub DidNotOperate()

        'If the facility has checked did not operate do thefollowing:
        Try
            rblNoOperateReason.Visible = True
            btnSavePnlFeeCalc.CausesValidation = False
            btnCalculate.Visible = False
            pnlEmissions.Visible = False
            chkPart70SM.Visible = False
            chkNSPSExempt.Visible = False
            cblNSPSExempt.Visible = False
            txtVOCTons.Text = 0
            txtNOxTons.Text = 0
            txtSO2Tons.Text = 0
            txtPMTons.Text = 0
            feepart70 = 0.0
            smfee.Text = 0.0
            feetotal = 0.0
            feensps = 0.0
            feecalculated = 0.0
            lblPart70Fee.Text = String.Format("{0:C}", 0.0)
            lblTotalFee.Text = String.Format("{0:C}", 0.0)
            lblNSPSFee.Text = String.Format("{0:C}", 0.0)
            lblpart70SMFee.Text = String.Format("{0:C}", 0.0)
            lblPayment.Text = String.Format("{0:C}", 0.0)
            lblpart70.Text = String.Format("{0:C}", 0.0)
            lblsm.Text = String.Format("{0:C}", 0.0)
            lblcalculated.Text = String.Format("{0:C}", 0.0)

            'CalculateFees()
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub
#End Region

#Region "Load from Database"

    Protected Sub GetInitialFeeRates()
        Try
            If ddlFeeYear.SelectedItem.Text = "-Select Year-" Then
                Exit Sub
            End If

            Dim dr As DataRow = GetFeeRates(ddlFeeYear.SelectedItem.Text)

            If dr IsNot Nothing Then
                If IsDBNull(dr.Item("numpertonrate")) Then
                    pertonrate.Text = ""
                Else
                    pertonrate.Text = String.Format("{0:C}", dr.Item("numpertonrate"))
                End If

                If IsDBNull(dr.Item("numsmfee")) Then
                    smfee.Text = ""
                Else
                    smfee.Text = String.Format("{0:C}", dr.Item("numsmfee"))
                End If

                If IsDBNull(dr.Item("numnspsfee")) Then
                    nspsfee.Text = ""
                Else
                    nspsfee.Text = String.Format("{0:C}", dr.Item("numnspsfee"))
                End If

                If IsDBNull(dr.Item("numpart70fee")) Then
                    titlevfee.Text = ""
                Else
                    titlevfee.Text = String.Format("{0:C}", dr.Item("numpart70fee"))
                End If
                If IsDBNull(dr.Item("numadminfeerate")) Then
                    adminfee.Text = "0"
                Else
                    adminfee.Text = dr.Item("numadminfeerate")
                End If
                If IsDBNull(dr.Item("datadminapplicable")) Then
                    adminfeedate.Text = "01-JAN-2099"
                Else
                    adminfeedate.Text = dr.Item("datadminapplicable")
                End If
                If IsDBNull(dr.Item("datfeeduedate")) Then
                    lblDeadline.Text = "01-SEP-" & (CInt(ddlFeeYear.SelectedItem.Text) + 1)
                Else
                    lblDeadline.Text = dr.Item("datfeeduedate")
                End If
                If IsDBNull(dr.Item("numaathres")) Then
                    numaathres.Text = "100"
                Else
                    numaathres.Text = dr.Item("numaathres")
                End If
                If IsDBNull(dr.Item("numnathres")) Then
                    numnathres.Text = "25"
                Else
                    numnathres.Text = dr.Item("numnathres")
                End If
            End If

            feeyear.Text = ddlFeeYear.SelectedItem.Text
            'lblDeadline.Text = CInt(feeyear.Text) + 1
            ResetFeeCalculationTab()
            'Take the user back to the welcome page
            UserTabs.ActiveTabIndex = 0

            If Mid(ddlFeeYear.SelectedItem.Value, 5) = 1 Then
                'UserTabs.Tabs(1).Enabled = True
                UserTabs.Tabs(2).Enabled = False
                UserTabs.Tabs(3).Enabled = False
                linkInvoice.Visible = True
                btnProceed.Visible = False
                btnUpdateContact.Text = "Save Fee Contact"
            Else
                'UserTabs.Tabs(1).Enabled = True
                UserTabs.Tabs(2).Enabled = True
                UserTabs.Tabs(3).Enabled = True
                linkInvoice.Visible = False
                btnProceed.Visible = True
                btnUpdateContact.Text = "Save and Continue"
            End If

            If CInt(feeyear.Text) < 2006 Then
                pnlDidNotOperate.Visible = True
            Else
                pnlDidNotOperate.Visible = False
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub LoadFacilityContact()
        Try
            Dim dr As DataRow

            If ddlFeeYear.SelectedItem.Text <> "-Select Year-" Then
                dr = GetFS_ContactInfo(ddlFeeYear.SelectedItem.Text)
                If dr Is Nothing Then
                    dr = GetAPBContactInformation(40)
                End If
            Else
                dr = GetAPBContactInformation(40)
            End If

            If dr IsNot Nothing Then

                'Getting details for the user from table apbcontactinformation.
                'This table has all the information that goes into panel facility information.

                If IsDBNull(dr.Item("strcontactfirstname")) Then
                    txtFName.Text = ""
                Else
                    txtFName.Text = dr.Item("strcontactfirstname")
                End If

                If IsDBNull(dr.Item("strcontactlastname")) Then
                    txtLName.Text = ""
                Else
                    txtLName.Text = dr.Item("strcontactlastname")
                End If

                If IsDBNull(dr.Item("strcontacttitle")) Then
                    txtTitle.Text = ""
                Else
                    txtTitle.Text = dr.Item("strcontacttitle")
                End If

                If IsDBNull(dr.Item("strcontactcompanyname")) Then
                    txtCoName.Text = ""
                Else
                    txtCoName.Text = dr.Item("strcontactcompanyname")
                End If

                If IsDBNull(dr.Item("strcontactphonenumber")) Then
                Else
                    txtPhone.Text = dr.Item("strcontactphonenumber")
                End If

                If IsDBNull(dr.Item("strcontactfaxnumber")) Then
                Else
                    txtFax.Text = dr.Item("strcontactfaxnumber")
                End If

                If IsDBNull(dr.Item("strcontactemail")) OrElse dr.Item("strcontactemail") = "N/A" Then
                    txtEmail.Text = currentUser.Email
                Else
                    txtEmail.Text = dr.Item("strcontactemail")
                End If

                If IsDBNull(dr.Item("strcontactaddress")) Then
                    txtAddress.Text = ""
                Else
                    txtAddress.Text = dr.Item("strcontactaddress")
                End If

                If IsDBNull(dr.Item("strcontactcity")) Then
                    txtCity.Text = ""
                Else
                    txtCity.Text = dr.Item("strcontactcity")
                End If

                If IsDBNull(dr.Item("strcontactstate")) Then
                    txtState.Text = ""
                Else
                    txtState.Text = Address.ProbableStateCode(dr.Item("strcontactstate"))
                End If

                If IsDBNull(dr.Item("strcontactzipcode")) Then
                    txtZip.Text = ""
                Else
                    txtZip.Text = dr.Item("strcontactzipcode")
                End If
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub LoadFacilityInfo()
        Try
            If ddlFeeYear.SelectedItem.Text <> "-Select Year-" Then
                ddlFacilityInfoChange.Enabled = True
                Dim dr As DataRow = GetFacilityInfo(ddlFeeYear.SelectedItem.Text)

                If dr IsNot Nothing Then

                    If IsDBNull(dr.Item("strfacilityname")) Then
                        lblFacilityName.Text = ""
                    Else
                        lblFacilityName.Text = dr.Item("strfacilityname")
                    End If

                    If IsDBNull(dr.Item("strfacilityaddress1")) Then
                        lblFacilityStreet.Text = ""
                    Else
                        lblFacilityStreet.Text = dr.Item("strfacilityaddress1")
                    End If

                    If IsDBNull(dr.Item("strfacilitycity")) Then
                        lblFacilityCity.Text = "a"
                    Else
                        lblFacilityCity.Text = dr.Item("strfacilitycity")
                    End If

                End If

                dr = GetFacilityInfoTemp()

                If dr IsNot Nothing Then

                    'Getting details for the facility from table apbfacilityinfotemp.
                    'This table has all the temporary information that goes into panel
                    'facility information changed by facility.
                    pnlfacInfo.Visible = True
                    ddlFacilityInfoChange.SelectedIndex = 1

                    If IsDBNull(dr.Item("strfacilityname")) Then
                        txtfacName.Text = ""
                    Else
                        txtfacName.Text = dr.Item("strfacilityname")
                    End If

                    If IsDBNull(dr.Item("strfacilitystreet1")) Then
                        txtfacStreet.Text = ""
                    Else
                        txtfacStreet.Text = dr.Item("strfacilitystreet1")
                    End If

                    If IsDBNull(dr.Item("strfacilitycity")) Then
                        txtfacCity.Text = ""
                    Else
                        txtfacCity.Text = dr.Item("strfacilitycity")
                    End If
                Else
                    pnlfacInfo.Visible = False
                End If
            Else
                ddlFacilityInfoChange.Enabled = False
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub LoadFeeCalculations()
        Dim dr As DataRow

        Try
            'First Get Info from Header Table
            dr = GetClassInfo(feeyear.Text)

            If dr IsNot Nothing Then
                'Getting details for the facility from table fs_mailout.
                'This table has all the information that goes into top panel and
                'is disabled so that the user cannot change it.

                '    'If IsDBNull(dr.Item("strclass")) Then
                '    txtClass.Text = ""
                'Else
                txtClass.Text = dr.Item("strclass")
                'End If

                If dr.Item("strnsps") = 1 Then
                    chkNSPS.Checked = True
                    chkNSPS1.Checked = True
                    chkNSPS1.Visible = False
                Else
                    chkNSPS.Checked = False
                    'chkNSPS1.Checked = False
                    chkNSPS1.Visible = True
                End If

                If dr.Item("strpart70") = 1 Then
                    chkPart70SM.SelectedIndex = 0
                Else
                End If
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

        'Next get Data from fs_feeauditeddata Tables
        Dim nspsReason As String = ""
        Dim i As Integer
        Dim fee As Double

        dr = GetCalculationInfo(feeyear.Text)
        Try
            If dr IsNot Nothing Then
                'Getting emission details from fs_feeauditeddata.
                'This table has all the information that goes into panel fee calculations.

                'If the NumFeeRate in the AuditedData table is different, then replace the pertonrate.Text with the new value
                If IsDBNull(dr.Item("numfeerate")) Then
                ElseIf CInt(dr.Item("numfeerate")) = 0 Then
                Else
                    pertonrate.Text = String.Format("{0:C}", dr.Item("numfeerate"))
                End If

                If IsDBNull(dr.Item("intvoctons")) Then
                    txtVOCTons.Text = ""
                    lblVOCFee.Text = ""
                Else
                    txtVOCTons.Text = dr.Item("intvoctons")
                    fee = PollutantVOCNOx(CInt(txtVOCTons.Text))
                    lblVOCFee.Text = String.Format("{0:C}", fee)
                End If

                If IsDBNull(dr.Item("intpmtons")) Then
                    txtPMTons.Text = ""
                    lblPMFee.Text = ""
                Else
                    txtPMTons.Text = dr.Item("intpmtons")
                    fee = PollutantPMSO2(CInt(txtPMTons.Text))
                    lblPMFee.Text = String.Format("{0:C}", fee)
                End If

                If IsDBNull(dr.Item("intso2tons")) Then
                    txtSO2Tons.Text = ""
                    lblSO2Fee.Text = ""
                Else
                    txtSO2Tons.Text = dr.Item("intso2tons")
                    fee = PollutantPMSO2(CInt(txtSO2Tons.Text))
                    lblSO2Fee.Text = String.Format("{0:C}", fee)
                End If

                If IsDBNull(dr.Item("intnoxtons")) Then
                    txtNOxTons.Text = ""
                    lblNOxFee.Text = ""
                Else
                    txtNOxTons.Text = dr.Item("intnoxtons")
                    fee = PollutantVOCNOx(CInt(txtNOxTons.Text))
                    lblNOxFee.Text = String.Format("{0:C}", fee)
                End If

                If IsDBNull(dr.Item("numpart70fee")) Then
                    feepart70 = 0
                Else
                    feepart70 = dr.Item("numpart70fee")
                End If

                If IsDBNull(dr.Item("numsmfee")) Then
                    feesm = 0
                ElseIf CInt(dr.Item("numsmfee")) = 0 Then
                    feesm = 0
                Else
                    feesm = dr.Item("numsmfee")
                    smfee.Text = String.Format("{0:C}", dr.Item("numsmfee"))
                End If

                If IsDBNull(dr.Item("numnspsfee")) Then
                    feensps = 0
                ElseIf CInt(dr.Item("numnspsfee")) = 0 Then
                    feensps = 0
                Else
                    feensps = dr.Item("numnspsfee")
                    nspsfee.Text = String.Format("{0:C}", dr.Item("numnspsfee"))
                End If

                If IsDBNull(dr.Item("numtotalfee")) Then
                    feetotal = 0
                Else
                    feetotal = dr.Item("numtotalfee")
                End If

                If IsDBNull(dr.Item("numcalculatedfee")) Then
                    feecalculated = 0
                Else
                    feecalculated = dr.Item("numcalculatedfee")
                End If

                If Not IsDBNull(dr.Item("strnspsexempt")) Then
                    If dr.Item("strnspsexempt") = "1" Then
                        chkNSPSExempt.Checked = True
                        If cblNSPSExempt.Items.Count < 1 Then
                            LoadNSPSExemptList()
                        End If
                        cblNSPSExempt.Visible = True
                    Else
                        chkNSPSExempt.Checked = False
                    End If
                End If

                If IsDBNull(dr.Item("strnspsreason")) Then
                    nspsReason = ""
                Else
                    nspsReason = dr.Item("strnspsreason")
                End If

                If IsDBNull(dr.Item("strnsps")) Then
                    chkNSPS1.Checked = False
                Else
                    If dr.Item("strnsps") = "1" Then
                        chkNSPS1.Checked = True
                    Else
                        chkNSPS1.Checked = False
                    End If
                End If

                If IsDBNull(dr("strpart70")) Then
                Else
                    If dr.Item("strpart70") = "1" Then
                        chkPart70SM.SelectedIndex = 0
                    End If
                End If

                If IsDBNull(dr.Item("strsyntheticminor")) Then
                Else
                    If dr.Item("strsyntheticminor") = "1" Then
                        chkPart70SM.SelectedIndex = 1
                    End If
                End If

                If IsDBNull(dr("strclass")) Then
                    ddlClass.SelectedValue = txtClass.Text
                Else
                    ddlClass.SelectedValue = dr.Item("strclass")
                End If
            Else
                ddlClass.SelectedValue = txtClass.Text
            End If

            If nspsReason = "" Then
            Else
                Dim items As String() = nspsReason.ToString().Split(",")
                For i = 0 To items.GetUpperBound(0)
                    Dim currentCheckBox As ListItem
                    currentCheckBox = cblNSPSExempt.Items.FindByValue(items(i).ToString())
                    If currentCheckBox IsNot Nothing Then
                        currentCheckBox.Selected = True
                    End If
                Next
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub LoadCityState(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtZip.TextChanged
        Try
            lblZipError.Visible = False
            Dim dt As DataTable = GetCityStateFromZip(txtZip.Text)

            If dt IsNot Nothing Then
                txtCity.Text = dt.Rows(0)("city")
                txtState.Text = Address.ProbableStateCode(dt.Rows(0)("state"))
            End If
        Catch ex As Exception
            lblZipError.Text = "Please make sure the zip code entered is correct"
            lblZipError.Visible = True
        End Try

        Dim sm As ScriptManager = Master.FindControl("ScriptManager1")
        sm.SetFocus(txtPhone)
    End Sub

    Protected Sub LoadSignandPay()
        Try
            Dim dr As DataRow = GetPaySubmitInfo(feeyear.Text)

            If dr IsNot Nothing Then
                'This sub will get information from fspayndsubmit
                'It gets all the informatio that gos in panel Pay and Submit

                If IsDBNull(dr.Item("STRPAYMENTPLAN")) Then
                    txtPayType.Text = ""
                Else
                    txtPayType.Text = dr.Item("STRPAYMENTPLAN")
                End If

                If IsDBNull(dr.Item("strofficialname")) Then
                    txtOwner.Text = ""
                Else
                    txtOwner.Text = dr.Item("strofficialname")
                End If

                If IsDBNull(dr.Item("strofficialtitle")) Then
                    txtOwnerTitle.Text = ""
                Else
                    txtOwnerTitle.Text = dr.Item("strofficialtitle")
                End If

                If IsDBNull(dr.Item("strcomment")) Then
                    txtComments.Text = ""
                Else
                    txtComments.Text = dr.Item("strcomment")
                End If
            End If

            If txtPayType.Text = "Four Quarterly Payments" Then
                rblPaymentType.SelectedIndex = 1
            Else
                rblPaymentType.SelectedIndex = 0
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub LoadNSPSExemptList()
        Dim dt As DataTable = GetNSPSExemptList(ddlFeeYear.SelectedItem.Text)

        If dt IsNot Nothing Then
            For Each row As DataRow In dt.Rows
                cblNSPSExempt.Items.Add(New ListItem(CStr(row.Item("Reason")), CStr(row.Item("ReasonID"))))
            Next
        End If
    End Sub

    Protected Sub LoadAnnualFeesHistory()
        grdFeeHistory.DataSource = GetAnnualFeeHistory(currentAirs)
        grdFeeHistory.DataBind()
    End Sub

#End Region

#Region "Save and Update to Database"

    Private Sub SaveFacilityInfo()

        Try
            Dim SQL As String

            If ddlFeeYear.SelectedItem.Text = "-Select Year-" Then
                SQL = "Select strairsnumber " &
                    "FROM apbcontactinformation " &
                    "where strairsnumber = @airs " &
                    "and strkey = '40'"
            Else
                SQL = "Select strairsnumber " &
                    "FROM fs_contactinfo " &
                    "where strairsnumber = @airs " &
                    "and numfeeyear = @feeyear"
            End If

            Dim params As SqlParameter() = {
                New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@feeyear", ddlFeeYear.SelectedItem.Text)
            }

            Dim dr As DataRow = DB.GetDataRow(SQL, params)

            If dr IsNot Nothing Then
                UpdateContactInfo()
            Else
                InsertContactInfo()
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub UpdateContactInfo()
        'This sub will save the nformation entered or changed in the
        'Facility Information panel.
        Try
            Dim contactDescription As String = "Fee Contact updated from GECO Fee application page by " & currentUser.FullName & " on " & Format$(Now, "dd-MMM-yyyy")

            If ddlFeeYear.SelectedItem.Text = "-Select Year-" Then

                Dim SQL As String = "Update apbcontactinformation set " &
                    "strcontactfirstname = @FirstName, " &
                    "strcontactlastname = @LastName, " &
                    "strcontacttitle = @Title, " &
                    "strcontactphonenumber1 = @Phone, " &
                    "strcontactfaxnumber = @Fax, " &
                    "strcontactemail = @Email, " &
                    "strcontactcity = @City, " &
                    "strcontactstate = @State, " &
                    "strcontactzipcode = @Zip, " &
                    "strcontactcompanyname = @Company, " &
                    "strcontactdescription = @ContDesc, " &
                    "datmodifingdate = getdate(), " &
                    "strmodifingperson = '0' " &
                    "where strcontactkey = @ContKey"

                Dim params As SqlParameter() = {
                    New SqlParameter("@FirstName", txtFName.Text),
                    New SqlParameter("@LastName", txtLName.Text),
                    New SqlParameter("@Title", txtTitle.Text),
                    New SqlParameter("@Phone", txtPhone.Text),
                    New SqlParameter("@Fax", txtFax.Text),
                    New SqlParameter("@Email", txtEmail.Text),
                    New SqlParameter("@Street", txtAddress.Text),
                    New SqlParameter("@City", txtCity.Text),
                    New SqlParameter("@State", txtState.Text),
                    New SqlParameter("@Zip", txtZip.Text),
                    New SqlParameter("@Company", txtCoName.Text),
                    New SqlParameter("@ContDesc", contactDescription),
                    New SqlParameter("@ContKey", "0413" & GetCookie(Cookie.AirsNumber) & "40")
                }

                DB.RunCommand(SQL, params)

            Else
                Dim qList As New List(Of String)
                Dim pList As New List(Of SqlParameter())

                Dim SQL2 As String = "Update fs_contactinfo set " &
                    "strcontactfirstname = @FirstName, " &
                    "strcontactlastname = @LastName, " &
                    "strcontacttitle = @Title, " &
                    "strcontactphonenumber = @Phone, " &
                    "strcontactfaxnumber = @Fax, " &
                    "strcontactemail = @Email, " &
                    "strcontactaddress = @Street, " &
                    "strcontactcity = @City, " &
                    "strcontactstate = @State, " &
                    "strcontactzipcode = @Zip, " &
                    "strcontactcompanyname = @Company, " &
                    "updatedatetime = getdate(), " &
                    "updateuser = @User " &
                    "where strairsnumber = @Airs " &
                    "and numfeeyear = @FeeYear"

                Dim params2 As SqlParameter() = {
                    New SqlParameter("@FirstName", txtFName.Text),
                    New SqlParameter("@LastName", txtLName.Text),
                    New SqlParameter("@Title", txtTitle.Text),
                    New SqlParameter("@Phone", txtPhone.Text),
                    New SqlParameter("@Fax", txtFax.Text),
                    New SqlParameter("@Email", txtEmail.Text),
                    New SqlParameter("@Street", txtAddress.Text),
                    New SqlParameter("@City", txtCity.Text),
                    New SqlParameter("@State", txtState.Text),
                    New SqlParameter("@Zip", txtZip.Text),
                    New SqlParameter("@Company", txtCoName.Text),
                    New SqlParameter("@User", "GECO||" & currentUser.Email),
                    New SqlParameter("@Airs", "0413" & GetCookie(Cookie.AirsNumber)),
                    New SqlParameter("@FeeYear", ddlFeeYear.SelectedItem.Text)
                }

                'Add SQL and Parameters to each list
                qList.Add(SQL2)
                pList.Add(params2)

                contactDescription = "Fee Contact updated during " & ddlFeeYear.SelectedItem.Text & " by " & currentUser.FullName & " on " & Format$(Now, "dd-MMM-yyyy")

                Dim Sql3 As String = "Update apbcontactinformation set " &
                    "strcontactfirstname = @FirstName, " &
                    "strcontactlastname = @LastName, " &
                    "strcontacttitle = @Title, " &
                    "strcontactphonenumber1 = @Phone, " &
                    "strcontactfaxnumber = @Fax, " &
                    "strcontactemail = @Email, " &
                    "strcontactaddress1 = @Street, " &
                    "strcontactcity = @City, " &
                    "strcontactstate = @State, " &
                    "strcontactzipcode = @Zip,             " &
                    "strcontactcompanyname = @Company, " &
                    "strcontactdescription = @ContDesc, " &
                    "datmodifingdate = getdate(), " &
                    "strmodifingperson = '0' " &
                    "where strcontactkey = @ContKey"

                Dim params3 As SqlParameter() = {
                    New SqlParameter("@FirstName", txtFName.Text),
                    New SqlParameter("@LastName", txtLName.Text),
                    New SqlParameter("@Title", txtTitle.Text),
                    New SqlParameter("@Phone", txtPhone.Text),
                    New SqlParameter("@Fax", txtFax.Text),
                    New SqlParameter("@Email", txtEmail.Text),
                    New SqlParameter("@Street", txtAddress.Text),
                    New SqlParameter("@City", txtCity.Text),
                    New SqlParameter("@State", txtState.Text),
                    New SqlParameter("@Zip", txtZip.Text),
                    New SqlParameter("@Company", txtCoName.Text),
                    New SqlParameter("@ContDesc", contactDescription),
                    New SqlParameter("@ContKey", "0413" & GetCookie(Cookie.AirsNumber) & "40")
                }

                'Add SQL and Parameters to each list
                qList.Add(Sql3)
                pList.Add(params3)

                Dim SQL4 As String = "Update fs_admin set numcurrentstatus = 5, " &
                    "updatedatetime = getdate(), " &
                    "DATSTATUSDATE = getdate(), " &
                    "updateuser = @UpdUser " &
                    "where strairsnumber = @Airs " &
                    "and numfeeyear = @FeeYear " &
                    "and numcurrentstatus < 5"

                Dim params4 As SqlParameter() = {
                    New SqlParameter("@UpdUser", "GECO||" & currentUser.Email),
                    New SqlParameter("@Airs", "0413" & GetCookie(Cookie.AirsNumber)),
                    New SqlParameter("@FeeYear", ddlFeeYear.SelectedItem.Text)
                }

                'Add SQL and Parameters to each list
                qList.Add(SQL4)
                pList.Add(params4)

                DB.RunCommand(qList, pList)
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub InsertContactInfo()
        'This sub will save the information entered or changed in the
        'Facility Information panel.
        Try
            Dim SQL As String
            Dim FirstName As String = txtFName.Text
            Dim LastName As String = txtLName.Text
            Dim User As String = "GECO||" & currentUser.Email
            Dim Title As String = txtTitle.Text
            Dim Phone As String = txtPhone.Text
            Dim Fax As String = txtFax.Text
            Dim Email As String = txtEmail.Text
            Dim Street As String = txtAddress.Text
            Dim City As String = txtCity.Text
            Dim State As String = txtState.Text
            Dim Zip As String = txtZip.Text
            Dim CompName As String = txtCoName.Text
            Dim AirsNo As String = "0413" & GetCookie(Cookie.AirsNumber)
            Dim Key As String = "0413" & GetCookie(Cookie.AirsNumber) & "40"
            Dim ContactDescription As String = "Fee Contact updated from GECO Fee application page by " & currentUser.FullName & " on " & Format$(Now, "dd-MMM-yyyy")
            Dim FeeYear As String = ddlFeeYear.SelectedItem.Text

            If ddlFeeYear.SelectedItem.Text = "-Select Year-" Then
                SQL = "Insert into apbcontactinformation ( " &
                    "strcontactfirstname, strcontactlastname, strcontacttitle, " &
                    "strcontactphonenumber1, strcontactfaxnumber, strcontactemail, strcontactaddress1, " &
                    "strcontactcity, strcontactstate, strcontactzipcode, strcontactcompanyname, " &
                    "STRMODIFINGPERSON, DATMODIFINGDATE, strairsnumber, strkey, strcontactkey, strcontactdescription) " &
                    "values(@FirstName, " &
                    "@LastName, " &
                    "@Title, " &
                    "@Phone, " &
                    "@Fax, " &
                    "@Email, " &
                    "@Street, " &
                    "@City, " &
                    "@State, " &
                    "@Zip, " &
                    "@CompName, " &
                    "'0', " &
                    "getdate(), " &
                    "@Airs, " &
                    "'40', " &
                    "@Key, " &
                    "@ContDesc)"

                Dim params As SqlParameter() = {
                    New SqlParameter("@FirstName", FirstName),
                    New SqlParameter("@LastName", LastName),
                    New SqlParameter("@Title", Title),
                    New SqlParameter("@Phone", Phone),
                    New SqlParameter("@Fax", Fax),
                    New SqlParameter("@Email", Email),
                    New SqlParameter("@Street", Street),
                    New SqlParameter("@City", City),
                    New SqlParameter("@State", State),
                    New SqlParameter("@Zip", Zip),
                    New SqlParameter("@CompName", CompName),
                    New SqlParameter("@Airs", AirsNo),
                    New SqlParameter("@Key", Key),
                    New SqlParameter("@ContDesc", ContactDescription)
                }

                DB.RunCommand(SQL, params)

            Else
                Dim qList As New List(Of String)
                Dim pList As New List(Of SqlParameter())

                SQL = "Insert into fs_contactinfo ( " &
                    "strcontactfirstname, strcontactlastname, strcontacttitle, " &
                    "strcontactphonenumber, strcontactfaxnumber, strcontactemail, strcontactaddress, " &
                    "strcontactcity, strcontactstate, strcontactzipcode, strcontactcompanyname, " &
                    "strairsnumber, numfeeyear) " &
                    "values(@FirstName, " &
                    "@LastName, " &
                    "@Title, " &
                    "@Phone, " &
                    "@Fax, " &
                    "@Email, " &
                    "@Street, " &
                    "@City, " &
                    "@State, " &
                    "@Zip, " &
                    "@CompName, " &
                    "@Airs, " &
                    "@FeeYear)"

                Dim params1 As SqlParameter() = {
                    New SqlParameter("@FirstName", FirstName),
                    New SqlParameter("@LastName", LastName),
                    New SqlParameter("@Title", Title),
                    New SqlParameter("@Phone", Phone),
                    New SqlParameter("@Fax", Fax),
                    New SqlParameter("@Email", Email),
                    New SqlParameter("@Street", Street),
                    New SqlParameter("@City", City),
                    New SqlParameter("@State", State),
                    New SqlParameter("@Zip", Zip),
                    New SqlParameter("@CompName", CompName),
                    New SqlParameter("@Airs", AirsNo),
                    New SqlParameter("@FeeYear", FeeYear)
                }

                'Add SQL and Parameters to each list
                qList.Add(SQL)
                pList.Add(params1)

                Dim SQL1 As String = "Update fs_admin set numcurrentstatus = 5, " &
                    "updatedatetime = getdate(), " &
                    "DATSTATUSDATE = getdate(), " &
                    "updateuser = @User " &
                    "where strairsnumber = @Airs " &
                    "and numfeeyear = @FeeYear " &
                    "And numcurrentstatus < 5"

                Dim params2 As SqlParameter() = {
                    New SqlParameter("@User", User),
                    New SqlParameter("@Airs", AirsNo),
                    New SqlParameter("@FeeYear", FeeYear)
                }

                'Add SQL and Parame3ters to each list
                qList.Add(SQL1)
                pList.Add(params2)

                DB.RunCommand(qList, pList)

                If ddlFacilityInfoChange.SelectedIndex = 1 Then
                    SaveTempFacInfo(GetCookie(Cookie.AirsNumber), txtfacName.Text, txtfacStreet.Text, txtfacCity.Text)
                Else
                    RemoveTempFacInfo(GetCookie(Cookie.AirsNumber))
                End If
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub SaveFeeCalcInfo()

        'This sub will first check if a record for this facility exists in
        'the table fs_feedata for the fee year If the record exists it
        'will update the record or else it will insert a new record for the
        'facility in the table

        Try
            Dim SQL As String = "Select strairsnumber " &
                "FROM fs_feedata " &
                "where strairsnumber = @airsno " &
                "and numfeeyear = @feeyear"

            Dim params As SqlParameter() = {
                New SqlParameter("@airsno", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@feeyear", CInt(feeyear.Text))
            }

            Dim dr As DataRow = DB.GetDataRow(SQL, params)

            If dr IsNot Nothing Then
                UpdateFeeCalculations()
            Else
                InsertFeeCalculations()
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub UpdateFeeCalculations()

        Dim tran As SqlTransaction = Nothing
        Try

            Dim SQL As String = ""
            Dim operate As String = "1"
            Dim exemptnsps As String = "0"
            Dim nspsreason As String = ""
            Dim nsps1 As String = "0"
            Dim part70 As String = "0"
            Dim syntheticminor As String = "0"
            Dim i As Integer

            If ddlClass.SelectedValue = "" Then
                ddlClass.SelectedValue = txtClass.Text
            End If

            If chkNSPS1.Checked Then
                nsps1 = "1"
            End If

            If chkPart70SM.Items(0).Selected Then
                part70 = "1"
            End If

            If chkPart70SM.Items(1).Selected Then
                syntheticminor = "1"
            End If

            If chkNSPSExempt.Checked Then
                exemptnsps = "1"
                Dim sb As StringBuilder = New StringBuilder()
                For i = 0 To cblNSPSExempt.Items.Count - 1
                    If cblNSPSExempt.Items(i).Selected Then
                        sb.Append(cblNSPSExempt.Items(i).Value & ",")
                    End If
                Next
                'Create the value to be inserted by removing the last comma in sb
                nspsreason = Left(sb.ToString(), Len(sb.ToString()) - 1)
            Else
                nspsreason = "0"
            End If

            If chkDidNotOperate.Checked Then
                operate = rblNoOperateReason.SelectedIndex
            End If

            'First SQL command withing the transaction
            Dim voctons As Integer = CInt(txtVOCTons.Text)
            Dim noxtons As Integer = CInt(txtNOxTons.Text)
            Dim pmtons As Integer = CInt(txtPMTons.Text)
            Dim so2tons As Integer = CInt(txtSO2Tons.Text)
            Dim part70fee As Double = CDbl(lblpart70.Text)
            Dim smfee As Double = CDbl(lblsm.Text)
            Dim nspsfee As Double = CDbl(lblNSPSFee.Text)
            Dim totalfee As Double = (CDbl(lblTotalFee.Text) + CDbl(hidAdminFee.Value))
            Dim nspsexempt As String = exemptnsps
            Dim nspsexemptreason As String = nspsreason
            Dim Sclass As String = ddlClass.SelectedValue
            Dim nsps As String = nsps1
            Dim feerate As Double = CDbl(pertonrate.Text)
            Dim calculatedfee As Double = CDbl(lblcalculated.Text)
            Dim adminfee As Double = CDbl(hidAdminFee.Value)
            Dim updateuser As String = "GECO||" & currentUser.Email
            Dim airs As String = "0413" & GetCookie(Cookie.AirsNumber)
            Dim feeyr As String = feeyear.Text

            SQL = "Update fs_feedata set " &
                "intvoctons = @voctons, " &
                "intnoxtons = @noxtons, " &
                "intpmtons = @pmtons, " &
                "intso2tons = @so2tons, " &
                "numpart70fee = @part70fee, " &
                "numsmfee = @smfee, " &
                "numnspsfee = @nspsfee, " &
                "numtotalfee = @totalfee, " &
                "strnspsexempt = @nspsexempt, " &
                "strnspsexemptreason = @nspsreason, " &
                "stroperate = @operate, " &
                "strclass = @sclass, " &
                "strnsps = @nsps, " &
                "strpart70 = @part70, " &
                "numfeerate = @feerate, " &
                "strsyntheticminor = @synminor, " &
                "numcalculatedfee = @calcfee, " &
                "numadminfee = @adminfee, " &
                "updatedatetime = getdate(), " &
                "updateuser = @upduser " &
                "where strairsnumber = @AIRS " &
                "and numfeeyear = @feeyear "

            Dim conn As New SqlConnection(DBConnectionString)
            Dim cmd As New SqlCommand(SQL, conn)

            'add SQL parameters for first SQL update
            cmd.Parameters.Add(New SqlParameter("@voctons", SqlDbType.Int)).Value = voctons
            cmd.Parameters.Add(New SqlParameter("@noxtons", SqlDbType.Int)).Value = noxtons
            cmd.Parameters.Add(New SqlParameter("@pmtons", SqlDbType.Int)).Value = pmtons
            cmd.Parameters.Add(New SqlParameter("@so2tons", SqlDbType.Int)).Value = so2tons
            cmd.Parameters.Add(New SqlParameter("@part70fee", SqlDbType.Decimal)).Value = part70fee
            cmd.Parameters.Add(New SqlParameter("@smfee", SqlDbType.Decimal)).Value = smfee
            cmd.Parameters.Add(New SqlParameter("@nspsfee", SqlDbType.Decimal)).Value = nspsfee
            cmd.Parameters.Add(New SqlParameter("@totalfee", SqlDbType.Decimal)).Value = totalfee
            cmd.Parameters.Add(New SqlParameter("@nspsexempt", SqlDbType.VarChar)).Value = nspsexempt
            cmd.Parameters.Add(New SqlParameter("@nspsreason", SqlDbType.VarChar)).Value = nspsexemptreason
            cmd.Parameters.Add(New SqlParameter("@operate", SqlDbType.VarChar)).Value = operate
            cmd.Parameters.Add(New SqlParameter("@sclass", SqlDbType.VarChar)).Value = Sclass
            cmd.Parameters.Add(New SqlParameter("@nsps", SqlDbType.VarChar)).Value = nsps
            cmd.Parameters.Add(New SqlParameter("@part70", SqlDbType.VarChar)).Value = part70
            cmd.Parameters.Add(New SqlParameter("@feerate", SqlDbType.Decimal)).Value = feerate
            cmd.Parameters.Add(New SqlParameter("@synminor", SqlDbType.VarChar)).Value = syntheticminor
            cmd.Parameters.Add(New SqlParameter("@calcfee", SqlDbType.Decimal)).Value = calculatedfee
            cmd.Parameters.Add(New SqlParameter("@adminfee", SqlDbType.Decimal)).Value = adminfee
            cmd.Parameters.Add(New SqlParameter("@upduser", SqlDbType.VarChar)).Value = updateuser
            cmd.Parameters.Add(New SqlParameter("@AIRS", SqlDbType.VarChar)).Value = airs
            cmd.Parameters.Add(New SqlParameter("@feeyear", SqlDbType.VarChar)).Value = feeyr

            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If

            tran = conn.BeginTransaction()
            cmd.Transaction = tran
            cmd.CommandType = CommandType.Text

            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()

            'Second SQL command within the transaction
            SQL = "Update fs_admin set numcurrentstatus = 6, " &
                "updatedatetime = getdate(), " &
                "DATSTATUSDATE = getdate(), " &
                "updateuser = @UpdUsr " &
                "where strairsnumber = @AIRS " &
                "and numfeeyear = @feeyear " &
                "and numcurrentstatus < 6"

            cmd.CommandText = SQL  ' = New SqlCommand(SQL, conn)
            cmd.CommandType = CommandType.Text

            cmd.Parameters.Add(New SqlParameter("@UpdUsr", SqlDbType.VarChar)).Value = updateuser
            cmd.Parameters.Add(New SqlParameter("@AIRS", SqlDbType.VarChar)).Value = airs
            cmd.Parameters.Add(New SqlParameter("@feeyear", SqlDbType.VarChar)).Value = feeyr

            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If

            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()

            'Third SQL command within the transaction
            cmd.CommandText = "PD_FeeAmendment" ' = New SqlCommand("PD_FeeAmendment", conn)

            If conn.State <> ConnectionState.Open Then
                conn.Open()
            End If

            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(New SqlParameter("AIRSNumber", SqlDbType.VarChar)).Value = airs
            cmd.Parameters.Add(New SqlParameter("FeeYear", SqlDbType.Decimal)).Value = feeyr

            cmd.ExecuteNonQuery()

            tran.Commit()

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
            tran.Rollback()
        Catch ex As Exception
            ErrorReport(ex)
            tran.Rollback()
        End Try

    End Sub

    Private Sub InsertFeeCalculations()

        Dim tran As SqlTransaction = Nothing
        Try
            Dim SQL As String
            Dim operate As String = "1"
            Dim exemptnsps As String = "0"
            Dim part70 As String = "0"
            Dim syntheticminor As String = "0"
            Dim nsps1 As String = "0"
            Dim nspsreason As String = "0"
            Dim i As Integer


            If ddlClass.SelectedValue = "" Then
                ddlClass.SelectedValue = txtClass.Text
            End If

            If chkNSPS1.Checked Then
                nsps1 = "1"
            End If

            If chkPart70SM.SelectedIndex = 0 Then
                part70 = "1"
            End If

            If chkPart70SM.SelectedIndex = 1 Then
                syntheticminor = "1"
            End If

            If chkNSPSExempt.Checked Then
                exemptnsps = "1"
                Dim sb As StringBuilder = New StringBuilder()
                'Dim sb1 As StringBuilder = New StringBuilder()
                For i = 0 To cblNSPSExempt.Items.Count - 1
                    If cblNSPSExempt.Items(i).Selected Then
                        sb.Append(cblNSPSExempt.Items(i).Value & ",")
                        'sb1.Append(cblNSPSExempt.Items(i).Text & ";")
                    End If
                Next
                'Create the value to be inserted by removing the last comma in sb
                nspsreason = Left(sb.ToString(), Len(sb.ToString()) - 1)
                'exemptreasontext = Left(sb1.ToString(), Len(sb1.ToString()) - 1)

                'For i = 0 To cblNSPSExempt.Items.Count - 1
                '    If cblNSPSExempt.Items(i).Selected Then
                '        nspsreason = nspsreason & 1
                '        exemptreasontext = exemptreasontext & cblNSPSExempt.Items(i).Text & "; "
                '    Else
                '        nspsreason = nspsreason & 0
                '    End If
                'Next
            End If

            If chkDidNotOperate.Checked Then
                operate = rblNoOperateReason.SelectedIndex
            End If

            'First SQL command withing the transaction
            Dim voctons As Integer = CInt(txtVOCTons.Text)
            Dim noxtons As Integer = CInt(txtNOxTons.Text)
            Dim pmtons As Integer = CInt(txtPMTons.Text)
            Dim so2tons As Integer = CInt(txtSO2Tons.Text)
            Dim part70fee As Double = CDbl(lblpart70.Text)
            Dim smfee As Double = CDbl(lblsm.Text)
            Dim nspsfee As Double = CDbl(lblNSPSFee.Text)
            Dim totalfee As Double = (CDbl(lblTotalFee.Text) + CDbl(hidAdminFee.Value))
            Dim nspsexempt As String = exemptnsps
            Dim nspsexemptreason As String = nspsreason
            Dim Sclass As String = ddlClass.SelectedValue
            Dim nsps As String = nsps1
            Dim feerate As Double = CDbl(pertonrate.Text)
            Dim calculatedfee As Double = CDbl(lblcalculated.Text)
            Dim adminfee As Double = CDbl(hidAdminFee.Value)
            Dim updateuser As String = "GECO||" & currentUser.Email
            Dim airs As String = "0413" & GetCookie(Cookie.AirsNumber)
            Dim feeyr As String = feeyear.Text

            SQL = "Insert into fs_feedata " &
                "(strairsnumber, numfeeyear, " &
                "intvoctons, intpmtons, intso2tons, intnoxtons, " &
                "numpart70fee, numsmfee, numnspsfee, " &
                "numtotalfee, strnspsexempt, strnspsexemptreason, stroperate, " &
                "strclass, strnsps, strpart70, numfeerate, " &
                "strsyntheticminor, numcalculatedfee, numadminfee, " &
                "updatedatetime, createdatetime, updateuser) " &
                "values(@AIRS, " &
                "@feeyear, " &
                "@voctons" &
                "@pmtons, " &
                "@so2tons, " &
                "@noxtons, " &
                "@part70fee, " &
                "@smfee, " &
                "@nspsfee, " &
                "@totalfee, " &
                "@nspsexempt, " &
                "@nspsreason, " &
                "@operate, " &
                "@sclass, " &
                "@nsps, " &
                "@part70, " &
                "@feerate, " &
                "@synminor, " &
                "@calcfee, " &
                "@adminfee, " &
                "getdate(), " &
                "getdate(), " &
                "@upduser)"

            Dim conn As New SqlConnection(DBConnectionString)
            Dim cmd As New SqlCommand(SQL, conn)

            'add SQL parameters for first SQL update
            cmd.Parameters.Add(New SqlParameter("@AIRS", SqlDbType.VarChar)).Value = airs
            cmd.Parameters.Add(New SqlParameter("@feeyear", SqlDbType.VarChar)).Value = feeyr
            cmd.Parameters.Add(New SqlParameter("@voctons", SqlDbType.Int)).Value = voctons
            cmd.Parameters.Add(New SqlParameter("@noxtons", SqlDbType.Int)).Value = noxtons
            cmd.Parameters.Add(New SqlParameter("@pmtons", SqlDbType.Int)).Value = pmtons
            cmd.Parameters.Add(New SqlParameter("@so2tons", SqlDbType.Int)).Value = so2tons
            cmd.Parameters.Add(New SqlParameter("@part70fee", SqlDbType.Decimal)).Value = part70fee
            cmd.Parameters.Add(New SqlParameter("@smfee", SqlDbType.Decimal)).Value = smfee
            cmd.Parameters.Add(New SqlParameter("@nspsfee", SqlDbType.Decimal)).Value = nspsfee
            cmd.Parameters.Add(New SqlParameter("@totalfee", SqlDbType.Decimal)).Value = totalfee
            cmd.Parameters.Add(New SqlParameter("@nspsexempt", SqlDbType.VarChar)).Value = nspsexempt
            cmd.Parameters.Add(New SqlParameter("@nspsreason", SqlDbType.VarChar)).Value = nspsexemptreason
            cmd.Parameters.Add(New SqlParameter("@operate", SqlDbType.VarChar)).Value = operate
            cmd.Parameters.Add(New SqlParameter("@sclass", SqlDbType.VarChar)).Value = Sclass
            cmd.Parameters.Add(New SqlParameter("@nsps", SqlDbType.VarChar)).Value = nsps
            cmd.Parameters.Add(New SqlParameter("@part70", SqlDbType.VarChar)).Value = part70
            cmd.Parameters.Add(New SqlParameter("@feerate", SqlDbType.Decimal)).Value = feerate
            cmd.Parameters.Add(New SqlParameter("@synminor", SqlDbType.VarChar)).Value = syntheticminor
            cmd.Parameters.Add(New SqlParameter("@calcfee", SqlDbType.Decimal)).Value = calculatedfee
            cmd.Parameters.Add(New SqlParameter("@adminfee", SqlDbType.Decimal)).Value = adminfee
            cmd.Parameters.Add(New SqlParameter("@upduser", SqlDbType.VarChar)).Value = updateuser

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            tran = conn.BeginTransaction()
            cmd.Transaction = tran
            cmd.CommandType = CommandType.Text

            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()

            'Second SQL command within the transaction
            SQL = "Update fs_admin set numcurrentstatus = 6, " &
                "updatedatetime = getdate(), " &
                "DATSTATUSDATE = getdate(), " &
                "updateuser = @UpdUsr " &
                "where strairsnumber = @AIRS " &
                "and numfeeyear = @feeyear " &
                "and numcurrentstatus < 6"

            cmd.CommandText = SQL  ' = New SqlCommand(SQL, conn)
            cmd.CommandType = CommandType.Text

            cmd.Parameters.Add(New SqlParameter("@UpdUsr", SqlDbType.VarChar)).Value = updateuser
            cmd.Parameters.Add(New SqlParameter("@AIRS", SqlDbType.VarChar)).Value = airs
            cmd.Parameters.Add(New SqlParameter("@feeyear", SqlDbType.VarChar)).Value = feeyr

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            cmd.ExecuteNonQuery()

            cmd.Parameters.Clear()

            'cmd = New SqlCommand("PD_FeeAmendment", conn)
            cmd.CommandText = "PD_FeeAmendment"
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add(New SqlParameter("FeeYear", SqlDbType.Decimal)).Value = feeyear.Text
            cmd.Parameters.Add(New SqlParameter("AIRSNumber", SqlDbType.VarChar)).Value = "0413" & GetCookie(Cookie.AirsNumber)

            cmd.ExecuteNonQuery()

            tran.Commit()

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch exThreadAbort As System.Threading.ThreadAbortException
            tran.Rollback()
        Catch ex As Exception
            ErrorReport(ex)
            tran.Rollback()
        End Try

    End Sub

    Private Sub SavePayandSignInfo()
        Try
            UpdatePayandSubmit()

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub UpdatePayandSubmit()

        Try
            Dim qList As New List(Of String)
            Dim pList As New List(Of SqlParameter())

            Dim SQL1 As String = "Update fs_feedata set " &
                "STRPAYMENTPLAN = @PayType, " &
                "strofficialname = @Owner, " &
                "strofficialtitle = @OwnerTitle, " &
                "strcomment = @Comments, " &
                "updatedatetime = @Date, " &
                "updateuser = @UEmail " &
                "where strairsnumber = @Airs " &
                "and numfeeyear = @FeeYear "

            Dim params1 As SqlParameter() = {
                New SqlParameter("@PayType", txtPayType.Text),
                New SqlParameter("@Owner", txtOwner.Text),
                New SqlParameter("@OwnerTitle", txtOwnerTitle.Text),
                New SqlParameter("@Comments", txtComments.Text),
                New SqlParameter("@Date", lblDate.Text),
                New SqlParameter("@UEmail", "GECO||" & currentUser.Email),
                New SqlParameter("@Airs", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@FeeYear", feeyear.Text)
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
                New SqlParameter("@Airs", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@FeeYear", ddlFeeYear.SelectedItem.Text)
            }

            'Add SQL and Parameters to each list
            qList.Add(Sql2)
            pList.Add(params2)

            DB.RunCommand(qList, pList)

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub SaveFinalSubmit()

        Try
            Dim UpdUser As String = "GECO||" & currentUser.Email
            Dim AirsNo As String = "0413" & GetCookie(Cookie.AirsNumber)
            Dim FY As String = ddlFeeYear.SelectedItem.Text

            Dim SQL As String = "Update fs_admin Set " &
                "numcurrentstatus = 8, " &
                "intsubmittal = 1, " &
                "updatedatetime = getdate(), " &
                "DATSTATUSDATE = getdate(), " &
                "datsubmittal = getdate(), " &
                "updateuser = @UpdUser " &
                "where strairsnumber = @airs " &
                "and numfeeyear = @FY"

            Dim params As SqlParameter() = {
                New SqlParameter("@UpdUser", UpdUser),
                New SqlParameter("@airs", AirsNo),
                New SqlParameter("@FY", FY)
            }

            DB.RunCommand(SQL, params)

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub SaveInvoiceNumber()

        Try
            Dim invoices As Integer
            Dim strPayType As Integer
            Dim amountDue As Double
            Dim AdminFee As Double
            Dim dt As Date
            Dim dr As DataRow = Nothing
            Dim qList As New List(Of String)
            Dim pList As New List(Of SqlParameter())
            Dim intfeeyear As Integer = feeyear.Text
            Dim AirsNo As String = "0413" & GetCookie(Cookie.AirsNumber)
            Dim updateUser As String = "GECO||" & currentUser.Email

            If lblAdminFeeAmount.Text = "" Then
                AdminFee = 0
            Else
                AdminFee = CDbl(lblAdminFeeAmount.Text)
            End If

            'Load DataRow with Fee Rates
            dr = GetFeeRates(ddlFeeYear.SelectedItem.Text)

            If txtPayType.Text = "Entire Annual Year" Then
                invoices = 1
                strPayType = 1
                amountDue = CDbl(lblPayment.Text) + AdminFee
                dt = dr.Item("datfeeduedate")
            Else 'Four Quarterly Payments
                invoices = 4
                strPayType = 2
                amountDue = ((CDbl(lblPayment.Text) + AdminFee) / 4)
                If IsDBNull(dr.Item("DATFIRSTQRTDUE")) Then
                    dt = dr.Item("datfeeduedate")
                Else
                    dt = dr.Item("DATFIRSTQRTDUE")
                End If
            End If

            Dim SQL As String
            Dim i As Integer = 1

            While i <= invoices
                Dim strInvoiceStatus As String

                If amountDue = 0 Then
                    strInvoiceStatus = "1"
                Else
                    strInvoiceStatus = "0"
                End If

                SQL = "Insert into fs_feeinvoice " &
                    "(invoiceid, numfeeyear, strairsnumber, numamount, datinvoicedate, updatedatetime, " &
                    "createdatetime, updateuser, strpaytype, strinvoicestatus, active) " &
                    "values(Next Value for feeinvoice_id, " &
                    "@feeYear, " &
                    "@AirsNo, " &
                    "@AmtDue, " &
                    "@InvDate, " &
                    "getdate(), " &
                    "getdate(), " &
                    "@UpdUser, " &
                    "@PayType, " &
                    "@InvStatus, '1')"

                Dim params As SqlParameter() = {
                    New SqlParameter("@feeYear", intfeeyear),
                    New SqlParameter("@AirsNo", AirsNo),
                    New SqlParameter("@AmtDue", amountDue),
                    New SqlParameter("@InvDate", dt),
                    New SqlParameter("@UpdUser", updateUser),
                    New SqlParameter("@PayType", strPayType.ToString),
                    New SqlParameter("@InvStatus", strInvoiceStatus)
                }

                'Add SQL and Parameters to each list
                qList.Add(SQL)
                pList.Add(params)

                i += 1
                dt = New DateTime

                Select Case i
                    Case 1
                        'first cases done above
                    Case 2
                        strPayType = 3
                        If IsDBNull(dr.Item("DATSECONDQRTDUE")) Then
                            dt = dr.Item("datfeeduedate")
                        Else
                            dt = dr.Item("DATSECONDQRTDUE")
                        End If
                    Case 3
                        strPayType = 4
                        If IsDBNull(dr.Item("DATTHIRDQRTDUE")) Then
                            dt = dr.Item("datfeeduedate")
                        Else
                            dt = dr.Item("DATTHIRDQRTDUE")
                        End If
                    Case 4
                        strPayType = 5
                        If IsDBNull(dr.Item("DATFOURTHQRTDUE")) Then
                            dt = dr.Item("datfeeduedate")
                        Else
                            dt = dr.Item("DATFOURTHQRTDUE")
                        End If
                End Select

            End While

            DB.RunCommand(qList, pList)

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub SaveConfirmation()

        Try
            Dim confirmation As String = GetCookie(Cookie.AirsNumber) & "-" & Format(Now, "yyyyMMddhhmm")
            Dim UID As String = currentUser.UserId
            Dim updDT As String = lblDate.Text
            Dim Uemail As String = "GECO||" & currentUser.Email
            Dim AIRSno As String = "0413" & GetCookie(Cookie.AirsNumber)
            Dim FY As String = feeyear.Text

            Dim SQL As String = "Update fs_feedata set " &
                "strconfirmationnumber = @conf, " &
                "strconfirmationuser = @UID, " &
                "updatedatetime = @UpdDT, " &
                "updateuser = @Uemail " &
                "where strairsnumber = @AIRSno " &
                "and numfeeyear = @FY"

            Dim params As SqlParameter() = {
                New SqlParameter("@conf", confirmation),
                New SqlParameter("@UID", UID),
                New SqlParameter("@UpdDT", updDT),
                New SqlParameter("@Uemail", Uemail),
                New SqlParameter("@AIRSno", AIRSno),
                New SqlParameter("@FY", FY)
            }

            DB.RunCommand(SQL, params)

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Function UpdateDatabase() As Boolean
        Page.Validate()
        If Page.IsValid Then
            SaveFacilityInfo()
            SaveFeeCalcInfo()
            SavePayandSignInfo()
            SaveFinalSubmit()
            SaveInvoiceNumber()
            Return True
        Else
            pnlSignandPay.Visible = True
            pnlSubmit.Visible = False
            lblMessage.Text = "Please click on the Fee Contact and Fee Calculations tabs and verify the data"
            lblMessage.Visible = True
            Return False
        End If
    End Function

#End Region

#Region "Control's Auto Postback"

    Protected Sub ddlClass_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            btnSavePnlFeeCalc.CausesValidation = True
            chkPart70SM.Visible = True
            chkNSPSExempt.Visible = True
            If chkNSPSExempt.Checked Then
                If cblNSPSExempt.Items.Count < 1 Then
                    LoadNSPSExemptList()
                End If
                cblNSPSExempt.Visible = True
            End If
            ClassCalculate()
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub chkNSPS1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            CalculateFees()
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub chkNSPSExempt_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkNSPSExempt.CheckedChanged
        Try
            If chkNSPSExempt.Checked Then
                If cblNSPSExempt.Items.Count < 1 Then
                    LoadNSPSExemptList()
                End If
                cblNSPSExempt.Visible = True
            Else
                cblNSPSExempt.Visible = False
            End If

            CalculateFees()
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub chkDidNotOperate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDidNotOperate.CheckedChanged
        Try
            If chkDidNotOperate.Checked Then
                DidNotOperate()
            Else
                If rblNoOperateReason.SelectedValue <> "" Then
                    rblNoOperateReason.SelectedItem.Selected = False
                End If
                rblNoOperateReason.Visible = False
                lblNoOperateReason.Visible = False
                btnSavePnlFeeCalc.CausesValidation = True
                btnCalculate.Visible = True
                chkPart70SM.Visible = True
                chkNSPSExempt.Visible = True
                If chkNSPSExempt.Checked Then
                    If cblNSPSExempt.Items.Count < 1 Then
                        LoadNSPSExemptList()
                    End If
                    cblNSPSExempt.Visible = True
                End If
                ClassCalculate()
            End If
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub ddlFacilityInfoChange_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFacilityInfoChange.SelectedIndexChanged
        Try
            If ddlFacilityInfoChange.SelectedIndex = 1 Then
                pnlfacInfo.Visible = True
                txtfacName.Text = lblFacilityName.Text
                txtfacStreet.Text = lblFacilityStreet.Text
                txtfacCity.Text = lblFacilityCity.Text
            Else
                pnlfacInfo.Visible = False
                txtfacName.Text = ""
                txtfacStreet.Text = ""
                txtfacCity.Text = ""
            End If
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub ddlFeeYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFeeYear.SelectedIndexChanged
        Try
            pnlSignandPay.Visible = True
            pnlSubmit.Visible = False

            If ddlFeeYear.SelectedItem.Text = "-Select Year-" Then
                UserTabs.Tabs(2).Enabled = False
                UserTabs.Tabs(3).Enabled = False
                linkInvoice.Visible = False
                linkInvoice.NavigateUrl = Nothing
                btnProceed.Visible = False
                feeRatesSection.Visible = False
            Else
                txtFName.Text = ""
                cblNSPSExempt.Items.Clear()
                feeRatesSection.Visible = True
                linkInvoice.NavigateUrl = String.Format("~/Invoice/?FeeYear={0}&Facility={1}", ddlFeeYear.SelectedItem.Text, currentAirs.ShortString)
                GetInitialFeeRates()
            End If
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub rblFeeContact_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rblFeeContact.SelectedIndexChanged
        If rblFeeContact.SelectedIndex = 0 Then
            LoadFacilityContact()
        Else
            Dim user As GecoUser = GetCurrentUser()

            If user IsNot Nothing Then
                txtFName.Text = user.FirstName
                txtLName.Text = user.LastName
                txtTitle.Text = user.Title
                txtCoName.Text = user.Company
                txtEmail.Text = user.Email
                txtFax.Text = ""
                txtAddress.Text = user.Address.Street
                txtCity.Text = user.Address.City
                txtState.Text = Address.ProbableStateCode(user.Address.State)
                txtZip.Text = user.Address.PostalCode
                txtPhone.Text = user.PhoneNumber
            End If
        End If
    End Sub

    Private Sub txtVOCTons_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtVOCTons.TextChanged
        Try
            Dim fee As Double
            If Not IsNumeric(txtVOCTons.Text) Then
                Exit Sub
            End If
            fee = PollutantVOCNOx(CInt(txtVOCTons.Text))
            lblVOCFee.Text = String.Format("{0:C}", fee)
            CalculateFees()
            Me.SetFocus(txtNOxTons)
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub txtNOxTons_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNOxTons.TextChanged
        Try
            Dim fee As Double
            If Not IsNumeric(txtNOxTons.Text) Then
                Exit Sub
            End If
            fee = PollutantVOCNOx(CInt(txtNOxTons.Text))
            lblNOxFee.Text = String.Format("{0:C}", fee)
            CalculateFees()
            Me.SetFocus(txtPMTons)
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub txtPMTons_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPMTons.TextChanged
        Try
            Dim fee As Double
            If Not IsNumeric(txtPMTons.Text) Then
                Exit Sub
            End If
            fee = PollutantPMSO2(CInt(txtPMTons.Text))
            lblPMFee.Text = String.Format("{0:C}", fee)
            CalculateFees()
            Me.SetFocus(txtSO2Tons)
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub txtSO2Tons_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSO2Tons.TextChanged
        Try
            Dim fee As Double
            If Not IsNumeric(txtSO2Tons.Text) Then
                Exit Sub
            End If
            fee = PollutantPMSO2(CInt(txtSO2Tons.Text))
            lblSO2Fee.Text = String.Format("{0:C}", fee)
            CalculateFees()
            Me.SetFocus(btnCalculate)
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

#Region "Reset, Disable, Enable Controls"

    Protected Sub ClearAll(ByVal C As Control)
        Try
            Dim Ctrl As Control 'Declare generic control object

            For Each Ctrl In C.Controls
                Select Case TypeName(Ctrl)
                    Case "TextBox" 'Do the following code if control is a Text Box
                        CType(Ctrl, TextBox).Text = String.Empty
                    Case "CheckBox" 'Do the following code if control is a Check Box
                        CType(Ctrl, CheckBox).Checked = False
                    Case "DropDownList" 'Do the following code if control is a Drop Down List
                        CType(Ctrl, DropDownList).ClearSelection()
                    Case "CheckBoxList" 'Do the following code if control is a Checkbox List
                        CType(Ctrl, CheckBoxList).ClearSelection()
                    Case "Label" 'Do the following code if control is a Label
                        CType(Ctrl, Label).Text = String.Empty
                    Case "RadioButtonList" 'Do the following code if control is a Radiobutton List
                        CType(Ctrl, RadioButtonList).ClearSelection()
                    Case Else
                        If Ctrl.Controls.Count > 0 Then 'Check for container control
                            ClearAll(Ctrl) 'Recursively call sub for controls in container
                        End If
                End Select
            Next Ctrl
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub ResetFeeCalculationTab()
        'This will clear all the data from the Fee Calculations and Sign and Pay tab
        'Since the viewstate will still keep information previously entered.
        ClearAll(Calculation) 'Clear controls in the Calculation Tab
        ClearAll(SignPay) 'Clear controls in the Sign and Pay Tab
    End Sub

#End Region

#Region "Validation Functions"

    Protected Function ValidateFacilityInfoChange() As Integer
        Try
            If UCase(lblFacilityName.Text) = UCase(txtfacName.Text) AndAlso
                UCase(lblFacilityStreet.Text) = UCase(txtfacStreet.Text) AndAlso
                UCase(lblFacilityCity.Text) = UCase(txtfacCity.Text) Then
                'No change in Facility Info
                Return -1
            End If

            Return 1
        Catch exThreadAbort As System.Threading.ThreadAbortException
            Return -1
        Catch ex As Exception
            ErrorReport(ex)
            Return -1
        End Try
    End Function

    Sub ErrorMessage()

        lblMessage.Text = "There are errors in the form. <br> Please check each section and submit again."
        lblMessage.Visible = True

    End Sub

#End Region

#Region "Miscellaneous Subs"

    Protected Sub DoServerSideCode(ByVal sender As Object, ByVal e As EventArgs)
        Dim xmldatasource As New XmlDataSource

        Try
            'Based on which tab is clicked the following will be executed
            Select Case UserTabs.ActiveTab.ID
                Case "Welcome" 'Facility Access

                Case "Contact"
                    'If Facility Contact is already loaded, then do not re-load
                    If ddlFeeYear.SelectedItem.Text = "-Select Year-" Then
                        lblMessage.Text = "Please select a fee year above.."
                        lblMessage.Visible = True
                    End If

                    If txtFName.Text = "" Then
                        'Load Data from database
                        LoadFacilityContact()
                        LoadFacilityInfo()
                        rblFeeContact.SelectedIndex = 0
                        btnUpdateContact.Enabled = True
                    End If

                Case "Calculation"
                    'If Emissions data is already loaded, then do not re-load
                    If lblTotalFee.Text = "" Then
                        'Load Data from database
                        LoadFeeCalculations()
                        ClassCalculate()
                    End If

                Case "SignPay"
                    'If Signature data is already loaded, then do not re-load
                    If txtOwner.Text = "" Then
                        'Load Data from database
                        LoadSignandPay()
                        lblDate.Text = Format$(Now, "dd-MMM-yyyy hh:mm")
                        If txtPayType.Text = "Four Quarterly Payments" Then
                            rblPaymentType.SelectedIndex = 1
                        Else
                            rblPaymentType.SelectedIndex = 0
                        End If
                    End If

                Case "SupDoc"

                Case "Reports"
                    LoadAnnualFeesHistory()

            End Select
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)

        Finally
            xmldatasource.Dispose()
        End Try
    End Sub

#End Region

End Class
