Imports GECO.GecoModels

Partial Class EIS_Default_Old
    Inherits Page

    Private FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
    Private FacilityAIRS As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(FacilitySiteID) Then
            Response.Redirect("~/")
        End If

        FacilityAIRS = New ApbFacilityId(FacilitySiteID)

        lblFacilityNameText.Text = GetFacilityName(FacilitySiteID)
        lblFacilityIDText.Text = FacilityAIRS.FormattedString

        LinkToEpaCaers.NavigateUrl = ConfigurationManager.AppSettings("EpaCaersUrl")

        If Not IsPostBack Then
            EISPanel()
        End If
    End Sub

    Private Sub EISPanel()

        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EIYear As String = ""
        Dim EISStatus As String = ""
        Dim Enrollment As String
        Dim OptOut As String = "NULL"
        Dim DateFinalize As String = ""
        Dim ConfNumber As String = ""

        If EISAccessCode <> "3" Then
            EIYear = GetCookie(EisCookie.EISMaxYear)
            EISStatus = GetCookie(EisCookie.EISStatus)
            Enrollment = GetCookie(EisCookie.Enrollment)

            If Enrollment = "1" Then
                OptOut = GetCookie(EisCookie.OptOut)
                DateFinalize = GetCookie(EisCookie.DateFinalize)
                ConfNumber = GetCookie(EisCookie.ConfNumber)
            End If
        End If

        ' | EISACCESSCODE | STRDESC                                                 |
        ' |---------------|---------------------------------------------------------|
        ' | 0             | FI access allowed with edit; EI access allowed, no edit |
        ' | 1             | FI and EI access allowed, both with edit                |
        ' | 2             | FI and EI access allowed, both no edit                  |
        ' | 3             | Facility not in EIS                                     |
        ' | 4             | Facility has no access to FI or EI                      |

        ' EISAccess = 3 indicates that the facility is not in the EIS_Admin table.
        ' "3" is not stored in the admin table; it is set in the GetEisStatus routine in Facility/Default.aspx.vb

        ' | EISSTATUSCODE | STRDESC                  |
        ' |---------------|--------------------------|
        ' | 0             | Not applicable           |
        ' | 1             | Applicable - not started |
        ' | 2             | In progress              |
        ' | 3             | Submitted                |
        ' | 4             | QA Process               |
        ' | 5             | Complete                 |

        ' NOTE: For the new process (using EPA's CAERS app), status code "2" is no longer used.
        ' When Facility either opts in or out, status is set to "3"

        Select Case EISAccessCode

            Case "0"

                If OptOut = "0" AndAlso EISStatus = "5" Then
                    ' Facility opted in, QA check complete, reset not allowed
                    pnlStatus.BackColor = GecoColors.ActionCompletePanel.BackColor
                    lblHeading.Text = "Participating in Emission Inventory"
                    lblMainMessage.Text = "The facility has reported that it is participating in the " &
                        EIYear & " Emissions Inventory. " &
                        "Use to button above to open EPA's CAERS. CAERS will be used for completing the EI."
                    lblStatusText.Text = GetEISStatusMessage(EISStatus)
                    trOptOutReason.Visible = False
                    lblConfNumberText.Text = ConfNumber
                    lblLastUpdateText.Text = DateFinalize
                    LinkToEpaCaers.Visible = True
                    divCaersInstructions.Visible = False
                    lblOther.Text = "The Air Protection Branch has reviewed and accepted the facility's data. " &
                        "The status may not be changed at this time."

                ElseIf OptOut = "1" AndAlso EISStatus = "5" Then
                    ' Facility opted out of EI, QA check complete, reset not allowed
                    pnlStatus.BackColor = GecoColors.NoActionPanel.BackColor
                    lblHeading.Text = "Not Participating In Emission Inventory"
                    lblMainMessage.Text = "The facility has reported that it is not participating in the" &
                        EIYear & " Emissions Inventory."
                    lblStatusText.Text = GetEISStatusMessage(EISStatus)
                    trOptOutReason.Visible = False
                    lblConfNumberText.Text = ConfNumber
                    lblLastUpdateText.Text = DateFinalize
                    divCaersInstructions.Visible = False
                    lblOther.Text = "The Air Protection Branch has reviewed and accepted the facility's data. " &
                        "The status may not be changed at this time."

                ElseIf OptOut = "NULL" AndAlso EISStatus = "0" Then
                    'Facility not in current EI
                    pnlStatus.BackColor = GecoColors.NoActionPanel.BackColor
                    lblHeading.Text = "Emissions Inventory Not Available"
                    lblMainMessage.Text = "Either Emissions Inventory enrollment has not yet occurred for the " &
                        "current EI submittal event or the Air Protection Branch has determined that the facility " &
                        "does not need to participate in the current EI submittal event."
                    lblOther.Text = "Contact the Air Protection Branch if you believe " &
                        "your facility should be participating in the current EI submittal event."
                    lblStatusText.Text = GetEISStatusMessage(EISStatus)
                    trOptOutReason.Visible = False
                    trConfNumber.Visible = False
                    trLastUpdate.Visible = False

                Else
                    'Some type of error
                    DisplayEIError("A1")
                End If

            Case "1"

                If OptOut = "NULL" AndAlso EISStatus = "1" Then
                    ' Facility in EI, but not started
                    pnlStatus.BackColor = GecoColors.InProgressPanel.BackColor
                    lblHeading.Text = "Ready For Emissions Inventory Process"
                    lblMainMessage.Text = "EPD's Air Protection Branch has determined that this facility " &
                        "may need to participate in the " & EIYear & " Emissions Inventory. Click the button above " &
                        "to begin. You will first verify the facility and contact " &
                        "information and then answer questions about the facility emissions " &
                        "to determine if participation in the EI " &
                        "is necessary."
                    lblStatusText.Text = GetEISStatusMessage(EISStatus)
                    trOptOutReason.Visible = False
                    trConfNumber.Visible = False
                    trLastUpdate.Visible = False
                    btnBegin.Text = "Begin " & EIYear & " EI"
                    btnBegin.Visible = True

                Else
                    'Some type of error
                    DisplayEIError("A2")
                End If

            Case "2"

                If OptOut = "0" AndAlso EISStatus = "3" Then
                    ' Facility opted into EI, reset allowed
                    pnlStatus.BackColor = GecoColors.ActionCompletePanel.BackColor
                    lblHeading.Text = "Participating in Emission Inventory"
                    lblMainMessage.Text = "The facility has reported that it is participating in the " &
                        EIYear & " Emissions Inventory. " &
                        "Use to button above to open EPA's CAERS. CAERS will be used for completing the EI."
                    lblStatusText.Text = GetEISStatusMessage(EISStatus)
                    trOptOutReason.Visible = False
                    lblConfNumberText.Text = ConfNumber
                    lblLastUpdateText.Text = DateFinalize
                    btnReset.Text = "Reset " & EIYear & " Status"
                    btnReset.Visible = True
                    lblOther.Text = "You may choose to reset the facility status and start " &
                        "over. Click the button to reset your status."
                    LinkToEpaCaers.Visible = True
                    divCaersInstructions.Visible = False

                ElseIf OptOut = "1" AndAlso EISStatus = "3" Then
                    ' Facility opted out of EI, reset allowed
                    pnlStatus.BackColor = GecoColors.NoActionPanel.BackColor
                    lblHeading.Text = "Not Participating In Emission Inventory"
                    lblMainMessage.Text = "The facility has reported that it is not participating in the " &
                        EIYear & " Emissions Inventory."
                    lblStatusText.Text = GetEISStatusMessage(EISStatus)
                    lblOptOutReasonText.Text = GetOptOutReason(FacilitySiteID, EIYear)
                    lblConfNumberText.Text = ConfNumber
                    lblLastUpdateText.Text = DateFinalize
                    btnReset.Text = "Reset " & EIYear & " Status"
                    btnReset.Visible = True
                    divCaersInstructions.Visible = False
                    lblOther.Text = "You may choose to reset the facility status and start " &
                        "over. Click the button to reset your status."

                ElseIf OptOut = "0" AndAlso EISStatus = "4" Then
                    ' Facility opted into EI, QA in progress, reset not allowed
                    pnlStatus.BackColor = GecoColors.ActionCompletePanel.BackColor
                    lblHeading.Text = "Participating in Emission Inventory"
                    lblMainMessage.Text = "The facility has reported that it is participating in the " &
                        EIYear & " Emissions Inventory. " &
                        "Use to button above to open EPA's CAERS. CAERS will be used for completing the EI."
                    lblStatusText.Text = GetEISStatusMessage(EISStatus)
                    trOptOutReason.Visible = False
                    lblConfNumberText.Text = ConfNumber
                    lblLastUpdateText.Text = DateFinalize
                    lblOther.Text = "The Air Protection Branch is reviewing the facility's " &
                        "data so the current status may not be changed at this time."
                    LinkToEpaCaers.Visible = True
                    divCaersInstructions.Visible = False

                ElseIf OptOut = "1" AndAlso EISStatus = "4" Then
                    ' Facility opted out of EI, QA in progress, reset not allowed
                    pnlStatus.BackColor = GecoColors.NoActionPanel.BackColor
                    lblHeading.Text = "Not Participating In Emission Inventory"
                    lblMainMessage.Text = "The facility has reported that it is not participating in the " &
                        EIYear & " Emissions Inventory."
                    lblStatusText.Text = GetEISStatusMessage(EISStatus)
                    trOptOutReason.Visible = False
                    lblConfNumberText.Text = ConfNumber
                    lblLastUpdateText.Text = DateFinalize
                    divCaersInstructions.Visible = False
                    lblOther.Text = "The Air Protection Branch is reviewing the facility's " &
                        "data so the current status may not be changed at this time."

                Else
                    'Some type of error
                    DisplayEIError("A3")
                End If

            Case "3", "4"
                'EIS not available to the facility
                pnlEisNotAvailable.Visible = True
                pnlStatus.Visible = False
                pnlResetStatus.Visible = False
                pnlError.Visible = False
                pnlEisNotAvailable.BorderColor = GecoColors.ErrorPanel.BackColor

            Case Else
                'Some type of error
                DisplayEIError("A4")
        End Select
    End Sub

    Private Sub DisplayEIError(errorId As String)

        pnlError.Visible = True
        pnlEisNotAvailable.Visible = False
        pnlResetStatus.Visible = False
        pnlStatus.Visible = False

        pnlError.BackColor = GecoColors.ErrorPanel.BackColor

        lblErrorMessage.Text = "There is a data error. Please contact the Air Protection " &
            "Branch using the contact link above. (Error " & errorId & ".)"

    End Sub

    Protected Sub btnBegin_Click(sender As Object, e As EventArgs) Handles btnBegin.Click

        Response.Redirect("rp_entry.aspx")

    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        If GetCookie(EisCookie.EISAccess) = "3" Then
            DisplayEIError("C1")
            Return
        End If

        If GetCookie(EisCookie.Enrollment) <> "1" Then
            DisplayEIError("C2")
            Return
        End If

        lblResetDate.Text = GetCookie(EisCookie.DateFinalize)
        lblResetYear.Text = GetCookie(EisCookie.EISMaxYear)

        If GetCookie(EisCookie.OptOut) = "1" Then
            lblResetStatus.Text = "Not participating in " & GetCookie(EisCookie.EISMaxYear) & " EI."
        Else
            lblResetStatus.Text = "Participating in " & GetCookie(EisCookie.EISMaxYear) & " EI."
        End If

        pnlResetStatus.Visible = True
        pnlStatus.Visible = False
        pnlEisNotAvailable.Visible = False
        pnlError.Visible = False

        pnlResetStatus.BorderColor = GecoColors.ErrorPanel.BackColor

    End Sub

    Protected Sub btnConfirmResetStatus_Click(sender As Object, e As EventArgs) Handles btnConfirmResetStatus.Click

        Dim EIYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim currentUser = GetCurrentUser()

        ResetEiStatus(FacilityAIRS, currentUser.DbUpdateUser, EIYear)
        LoadEiStatusCookies(FacilitySiteID, Response)
        Response.Redirect("Default.aspx")

    End Sub

    Protected Sub btnCancelResetStatus_Click(sender As Object, e As EventArgs) Handles btnCancelResetStatus.Click

        Response.Redirect("Default.aspx")

    End Sub

End Class
