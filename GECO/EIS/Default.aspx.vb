Imports GECO.GecoModels

Public Class EIS_Default
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId
    Public Property EiStatus As EisStatus

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClearCookie(Cookie.EiProcess)
        MainLoginCheck()

        If IsPostBack Then
            CurrentAirs = New ApbFacilityId(GetCookie(Cookie.AirsNumber))
        Else
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.IsValidAirsNumberFormat(airsString) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            CurrentAirs = New ApbFacilityId(airsString)
            SetCookie(Cookie.AirsNumber, CurrentAirs.ShortString())
        End If

        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.Home

        Dim facilityAccess As FacilityAccess = GetCurrentUser().GetFacilityAccess(CurrentAirs)

        If facilityAccess Is Nothing OrElse Not facilityAccess.EisAccess Then
            Response.Redirect("~/Home/")
        End If

        EiStatus = GetEiStatus(CurrentAirs)

        If Not IsPostBack Then
            CdxLink.NavigateUrl = ConfigurationManager.AppSettings("EpaCaersUrl")
            ShowEisPanel()
        End If
    End Sub

    Private Sub ShowEisPanel()
        ' | EISACCESSCODE | STRDESC                                                 |
        ' |---------------|---------------------------------------------------------|
        ' | 0             | FI access allowed with edit; EI access allowed, no edit |
        ' | 1             | FI and EI access allowed, both with edit                |
        ' | 2             | FI and EI access allowed, both no edit                  |
        ' | 3             | Facility not in EIS                                     |
        ' | 4             | Facility has no access to FI or EI                      |

        ' EISAccess = 3 indicates that the facility is not in the EIS_Admin table.
        ' "3" is not stored in the admin table; it is set in the GetEisStatus method

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

        Select Case EiStatus.AccessCode

            Case 0 ' FI access allowed with edit; EI access allowed, no edit

                If Not EiStatus.OptOut AndAlso EiStatus.StatusCode = 5 Then
                    ' Facility opted in, QA check complete, reset not allowed

                    pnlStatus.CssClass = "panel panel-complete"
                    lblMainStatus.Text = "Participating in the Emissions Inventory"
                    lblMainMessage.Text = "The facility has reported that it is participating in the " &
                        EiStatus.MaxYear & " Emissions Inventory. " &
                        "The Air Protection Branch has reviewed and accepted the facility's data. " &
                        "The status may not be changed at this time."
                    pBeginProcess.Visible = False
                    dCdxNext.Visible = True
                    trOptOutReason.Visible = False
                    lblConfNumberText.Text = EiStatus.ConfirmationNumber
                    lblLastUpdateText.Text = EiStatus.DateFinalized.ToString
                    pReset.Visible = False
                    dNewProcess.Visible = False

                ElseIf EiStatus.OptOut AndAlso EiStatus.StatusCode = 5 Then
                    ' Facility opted out of EI, QA check complete, reset not allowed

                    pnlStatus.CssClass = "panel panel-complete"
                    lblMainStatus.Text = "Not Participating in the Emissions Inventory"
                    lblMainMessage.Text = "The facility has reported that it is not participating in the" &
                        EiStatus.MaxYear & " Emissions Inventory. " &
                        "The Air Protection Branch has reviewed and accepted the facility's data. " &
                        "The status may not be changed at this time."
                    pBeginProcess.Visible = False
                    dCdxNext.Visible = False
                    trOptOutReason.Visible = True
                    lblOptOutReasonText.Text = DecodeOptOutReason(EiStatus.OptOutReason)
                    lblConfNumberText.Text = EiStatus.ConfirmationNumber
                    lblLastUpdateText.Text = EiStatus.DateFinalized.ToString
                    pReset.Visible = False
                    dNewProcess.Visible = False

                ElseIf EiStatus.OptOut Is Nothing AndAlso EiStatus.StatusCode = 0 Then
                    'Facility not in current EI

                    pnlStatus.CssClass = "panel panel-noaction"
                    lblMainStatus.Text = "Emissions Inventory Not Available"
                    lblMainMessage.Text = "Either Emissions Inventory enrollment has not yet occurred for the " &
                        "current EI submittal event or the Air Protection Branch has determined that the facility " &
                        "does not need to participate in the current EI submittal event. " &
                        "Contact the Air Protection Branch if you believe " &
                        "your facility should be participating in the current EI submittal event."
                    pBeginProcess.Visible = False
                    dCdxNext.Visible = False
                    trOptOutReason.Visible = False
                    trConfNumber.Visible = False
                    trLastUpdate.Visible = False
                    pReset.Visible = False
                    dNewProcess.Visible = True

                Else
                    'Some type of error
                    DisplayEisError("A1")
                End If

            Case 1 ' FI and EI access allowed, both with edit

                If EiStatus.OptOut Is Nothing AndAlso EiStatus.StatusCode = 1 Then
                    ' Facility in EI, but not started

                    pnlStatus.CssClass = "panel panel-inprogress"
                    lblMainStatus.Text = "Ready for the Emissions Inventory Process"
                    lblMainMessage.Text = "EPD's Air Protection Branch has determined that this facility " &
                        "may need to participate in the " &
                        EiStatus.MaxYear & " Emissions Inventory. " &
                        "Click the button below to begin. You will first verify the facility and " &
                        "contact information and then answer questions about the facility emissions " &
                        "to determine if participation in the EI is necessary."
                    pBeginProcess.Visible = True
                    dCdxNext.Visible = False
                    StatusTable.Visible = False
                    pReset.Visible = False
                    dNewProcess.Visible = True

                Else
                    'Some type of error
                    DisplayEisError("A2")
                End If

            Case 2 ' FI and EI access allowed, both no edit

                If Not EiStatus.OptOut AndAlso EiStatus.StatusCode = 3 Then
                    ' Facility opted into EI, reset allowed

                    pnlStatus.CssClass = "panel panel-complete"
                    lblMainStatus.Text = "Participating in the Emissions Inventory (Submitted)"
                    lblMainMessage.Text = "The facility has reported that it is participating in the " &
                        EiStatus.MaxYear & " Emissions Inventory. " &
                        "You may reset the current status and start over."
                    pBeginProcess.Visible = False
                    dCdxNext.Visible = True
                    trOptOutReason.Visible = False
                    lblConfNumberText.Text = EiStatus.ConfirmationNumber
                    lblLastUpdateText.Text = EiStatus.DateFinalized.ToString
                    pReset.Visible = True
                    dNewProcess.Visible = False

                ElseIf EiStatus.OptOut AndAlso EiStatus.StatusCode = 3 Then
                    ' Facility opted out of EI, reset allowed

                    pnlStatus.CssClass = "panel panel-complete"
                    lblMainStatus.Text = "Not Participating in the Emissions Inventory (Submitted)"
                    lblMainMessage.Text = "The facility has reported that it is not participating in the " &
                        EiStatus.MaxYear & " Emissions Inventory. " &
                        "You may reset the current status."
                    pBeginProcess.Visible = False
                    dCdxNext.Visible = False
                    trOptOutReason.Visible = True
                    lblOptOutReasonText.Text = DecodeOptOutReason(EiStatus.OptOutReason)
                    lblConfNumberText.Text = EiStatus.ConfirmationNumber
                    lblLastUpdateText.Text = EiStatus.DateFinalized.ToString
                    pReset.Visible = True
                    dNewProcess.Visible = False

                ElseIf Not EiStatus.OptOut AndAlso EiStatus.StatusCode = 4 Then
                    ' Facility opted into EI, QA in progress, reset not allowed

                    pnlStatus.CssClass = "panel panel-complete"
                    lblMainStatus.Text = "Participating in the Emissions Inventory (Under Review)"
                    lblMainMessage.Text = "The facility has reported that it is participating in the " &
                        EiStatus.MaxYear & " Emissions Inventory. " &
                        "The Air Protection Branch is reviewing the facility's " &
                        "data so the current status may not be changed at this time."
                    pBeginProcess.Visible = False
                    dCdxNext.Visible = True
                    trOptOutReason.Visible = False
                    lblConfNumberText.Text = EiStatus.ConfirmationNumber
                    lblLastUpdateText.Text = EiStatus.DateFinalized.ToString
                    pReset.Visible = False
                    dNewProcess.Visible = False

                ElseIf EiStatus.OptOut AndAlso EiStatus.StatusCode = 4 Then
                    ' Facility opted out of EI, QA in progress, reset not allowed

                    pnlStatus.CssClass = "panel panel-complete"
                    lblMainStatus.Text = "Not Participating in the Emissions Inventory (Under Review)"
                    lblMainMessage.Text = "The facility has reported that it is not participating in the " &
                        EiStatus.MaxYear & " Emissions Inventory. " &
                        "The Air Protection Branch is reviewing the facility's " &
                        "data so the current status may not be changed at this time."
                    pBeginProcess.Visible = False
                    dCdxNext.Visible = False
                    trOptOutReason.Visible = True
                    lblOptOutReasonText.Text = DecodeOptOutReason(EiStatus.OptOutReason)
                    lblConfNumberText.Text = EiStatus.ConfirmationNumber
                    lblLastUpdateText.Text = EiStatus.DateFinalized.ToString
                    pReset.Visible = False
                    dNewProcess.Visible = False

                Else
                    'Some type of error
                    DisplayEisError("A3")
                End If

            Case 3, 4
                ' 3 - Facility not in EIS
                ' 4 - Facility has no access to FI or EI

                pnlEisNotAvailable.Visible = True
                pnlStatus.Visible = False
                pnlResetStatus.Visible = False
                pnlError.Visible = False
                dNewProcess.Visible = True

            Case Else
                'Some type of error
                DisplayEisError("A4")
        End Select
    End Sub

    Private Sub DisplayEisError(errorId As String)
        pnlError.Visible = True
        pnlEisNotAvailable.Visible = False
        pnlResetStatus.Visible = False
        pnlStatus.Visible = False

        lblErrorId.Text = errorId
    End Sub

    Private Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click
        If EiStatus.AccessCode = 3 Then
            DisplayEisError("C1")
            Return
        End If

        If Not EiStatus.Enrolled Then
            DisplayEisError("C2")
            Return
        End If

        If EiStatus.OptOut Then
            lblResetStatus.Text = "Not participating in the " & EiStatus.MaxYear & " EI."
        ElseIf Not EiStatus.OptOut Then
            lblResetStatus.Text = "Participating in the " & EiStatus.MaxYear & " EI."
        Else
            DisplayEisError("C3")
            Return
        End If

        lblResetDate.Text = EiStatus.DateFinalized

        pnlResetStatus.Visible = True
        pnlStatus.Visible = False
    End Sub

    Protected Sub btnConfirmResetStatus_Click(sender As Object, e As EventArgs) Handles btnConfirmResetStatus.Click
        ResetEiStatus(CurrentAirs, GetCurrentUser().DbUpdateUser, EiStatus.MaxYear)
        SetEiStatusCookies(CurrentAirs, Response)
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub btnCancelResetStatus_Click(sender As Object, e As EventArgs) Handles btnCancelResetStatus.Click
        pnlResetStatus.Visible = False
        pnlStatus.Visible = True
        ShowEisPanel()
    End Sub

    Private Sub btnBeginEiProcess_Click(sender As Object, e As EventArgs) Handles btnBeginEiProcess.Click
        SetCookie(Cookie.EiProcess, True.ToString)
        Response.Redirect("~/EIS/Facility/Edit.aspx")
    End Sub

End Class
