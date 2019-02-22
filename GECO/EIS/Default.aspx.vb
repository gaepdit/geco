Imports System.Data.SqlClient

Partial Class eis_Default
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            EISPanel()
            ShowEISHelpMenu()
        End If
    End Sub

#Region "  Intro page panel routines   "

    Private Sub EISPanel()

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EIYear As String = ""
        Dim FacilityName As String = GetFacilityName(FacilitySiteID)
        Dim EISStatus As String = ""
        Dim Enrollment As String = ""
        Dim OptOut As String = "NULL"
        Dim DateFinalize As String = ""
        Dim ConfNumber As String = ""
        Dim eisStatusMessage As String = ""

        Try
            'Reminder: EISAccess = 3 indicates that the facility is not in the EIS_Admin table.
            ' "3" is never stored in the admin table. It is set in the GetEISStatus routine in Facility/Default.aspx.vb
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

            If EISAccessCode = "3" Then
                eisStatusMessage = "Facility not in EIS"
            ElseIf EISAccessCode = "4" Then
                'Do nothing
            Else
                eisStatusMessage = GetEISStatusMessage(EISStatus)
            End If

            Select Case EISAccessCode
                Case "0"
                    If OptOut = "0" And EISStatus = "5" Then
                        'Facility opted in, data submitted and sent to EPA - No EI access, FI access allowed
                        ShowFacilityInventoryMenu()
                        HideEmissionInventoryMenu()
                        pnlStatus_Inner.BackColor = GecoColors.CompletedPanel.BackColor
                        lblHeading.Text = EIYear & " Emissions Inventory Complete - Submitted to EPA"
                        lblMainMessage.Text = "The facility's EI data has been successfully submitted to " &
                                            "the US EPA. The facility submitted data to Georgia EPD on " & DateFinalize & ". " &
                                            "The Facility Inventory area is now accessible using the menu on the left."
                        lblMainMessage.Width = 550
                        lblFacilityName.Text = "Facility Name:"
                        lblFacilityNameText.Text = FacilityName
                        lblFacilityID.Text = "Facility AIRS No:"
                        lblFacilityIDText.Text = FacilitySiteID
                        lblStatus.Text = "Status:"
                        lblStatusText.Text = eisStatusMessage
                        lblConfNumber.Text = EIYear & " Confirmation No:"
                        lblConfNumberText.Text = ConfNumber
                        lblLastUpdate.Text = "Submitted on:"
                        lblLastUpdateText.Text = GetEISAdminUpdateDateTime(FacilitySiteID, EIYear)
                        btnAction1.Text = ""
                        btnAction2.Text = "Facility Home"
                        btnAction1.Visible = False
                        btnAction2.Visible = True
                    ElseIf OptOut = "1" And EISStatus = "5" Then
                        'Facility opted out and the bulk complete process was run - No EI access, FI access allowed
                        ShowFacilityInventoryMenu()
                        HideEmissionInventoryMenu()
                        pnlStatus_Inner.BackColor = GecoColors.CompletedPanel.BackColor
                        lblHeading.Text = EIYear & "Emissions Inventory Complete - Facility Did Not Participate"
                        lblMainMessage.Text = "The facility opted out of the EI process on " & DateFinalize & ". " &
                                            "The confirmation number appears below. The " &
                                            "Facility Inventory area is now accessible using the menu on the left."
                        lblMainMessage.Width = 500
                        lblFacilityName.Text = "Facility Name:"
                        lblFacilityNameText.Text = FacilityName
                        lblFacilityID.Text = "Facility AIRS No:"
                        lblFacilityIDText.Text = FacilitySiteID
                        lblStatus.Text = "Status:"
                        lblStatusText.Text = eisStatusMessage
                        lblOptOutReason.Text = "Reason Not Participating:"
                        lblOptOutReasonText.Text = GetOptOutReason(FacilitySiteID, EIYear)
                        lblConfNumber.Text = EIYear & " Confirmation No:"
                        lblConfNumberText.Text = ConfNumber
                        lblLastUpdate.Text = "Submitted on:"
                        lblLastUpdateText.Text = GetEISAdminUpdateDateTime(FacilitySiteID, EIYear)
                        btnAction1.Text = ""
                        btnAction2.Text = "Facility Home"
                        btnAction1.Visible = False
                        btnAction2.Visible = True
                    ElseIf OptOut = "NULL" And EISStatus = "0" Then
                        'Facility not in current EI - No EI access, FI access allowed
                        ShowFacilityInventoryMenu()
                        HideEmissionInventoryMenu()
                        pnlStatus_Inner.BackColor = GecoColors.CompletedPanel.BackColor
                        lblHeading.Text = "Facility Inventory Only Available"
                        lblMainMessage.Text = "Either Emissions Inventory enrollment has not yet occurred for the current " &
                                            "EI submittal event or EPD's Air Protection Branch has determined that the facility does " &
                                            "not need to participate in the current EI submittal event."
                        lblOther.Text = "Contact the Air Protection Branch's Data Management Unit if you believe " &
                                        "your facility should be participating in the current EI submittal event. If " &
                                        "you are attempting the EI in early January you may want to wait a few days before " &
                                        "trying again."
                        lblMainMessage.Width = 500
                        lblFacilityName.Text = "Facility Name:"
                        lblFacilityNameText.Text = FacilityName
                        lblFacilityID.Text = "Facility AIRS No:"
                        lblFacilityIDText.Text = FacilitySiteID
                        lblStatus.Text = "Status:"
                        lblStatusText.Text = eisStatusMessage
                        lblConfNumber.Text = ""
                        lblConfNumberText.Text = ""
                        lblLastUpdate.Text = ""
                        lblLastUpdateText.Text = ""
                        btnAction1.Text = ""
                        btnAction2.Text = "Facility Home"
                        btnAction1.Visible = False
                        btnAction2.Visible = True
                    Else
                        'Some type of error
                        DisplayEIError(FacilitySiteID, "A1")
                    End If
                Case "1"
                    If OptOut = "NULL" And EISStatus = "1" Then
                        'If null, EI not started - show panel to begin - EI and FI menus not yet available
                        HideFacilityInventoryMenu()
                        HideEmissionInventoryMenu()
                        pnlStatus_Inner.BackColor = GecoColors.InProgressPanel.BackColor
                        lblHeading.Text = "Begin " & EIYear & " EI Process"
                        lblMainMessage.Text = "EPD's Air Protection Branch has determined that this facility needs " &
                                            "to participate in the " & EIYear & " EI process. Click the button above " &
                                            "to begin. You will need to complete/verify the facility and contact " &
                                            "information before answering questions about the facility emissions " &
                                            "that will determine if participation in the Emissions Inventory process " &
                                            "is necessary. The decision will be based on the facility's emission levels " &
                                            "of the pollutants indicated."
                        lblMainMessage.Width = 550
                        lblFacilityName.Text = "Facility Name:"
                        lblFacilityNameText.Text = FacilityName
                        lblFacilityID.Text = "Facility AIRS No:"
                        lblFacilityIDText.Text = FacilitySiteID
                        lblStatus.Text = "Status:"
                        lblStatusText.Text = eisStatusMessage
                        lblConfNumber.Text = ""
                        lblConfNumberText.Text = ""
                        lblLastUpdate.Text = ""
                        lblLastUpdateText.Text = ""
                        btnAction1.Text = "Begin " & EIYear & " EI"
                        btnAction2.Text = "Facility Home"
                        btnAction1.Visible = True
                        btnAction2.Visible = True
                    ElseIf OptOut = "0" And EISStatus = "2" Then
                        'Facility opted in and is in progress - FI and EI menus available
                        ShowFacilityInventoryMenu()
                        ShowEmissionInventoryMenu()
                        pnlStatus_Inner.BackColor = GecoColors.InProgressPanel.BackColor
                        lblHeading.Text = EIYear & " Emission Inventory - In Progess"
                        lblMainMessage.Text = "The facility is currently in the process of completing the " &
                                                "Emission Inventory for the " & EIYear & " calendar year. " &
                                                "Use the menu on the left to continue working on the EI. At any " &
                                                "time during the EI process you may choose to reset the EI data and " &
                                                "start again from the beginning of the process. The reset process cannot be undone."
                        lblMainMessage.Width = 550
                        lblFacilityName.Text = "Facility Name:"
                        lblFacilityNameText.Text = FacilityName
                        lblFacilityID.Text = "Facility AIRS No:"
                        lblFacilityIDText.Text = FacilitySiteID
                        lblStatus.Text = "Status:"
                        lblStatusText.Text = eisStatusMessage
                        lblConfNumber.Text = ""
                        lblConfNumberText.Text = ""
                        lblLastUpdate.Text = ""
                        lblLastUpdateText.Text = ""
                        btnAction1.Text = ""
                        btnAction2.Text = "Facility Home"
                        btnAction1.Visible = False
                        btnAction2.Visible = True
                    Else
                        'Some type of error
                        DisplayEIError(FacilitySiteID, "A2")
                    End If
                Case "2"
                    If OptOut = "0" And EISStatus = "3" Then
                        'Facility opted in, submitted but data not yet sent to EPA. Change allowed - FI and EI menus not available
                        HideFacilityInventoryMenu()
                        HideEmissionInventoryMenu()
                        pnlStatus_Inner.BackColor = GecoColors.CompletedPanel.BackColor
                        lblHeading.Text = EIYear & " Emission Inventory Submitted to Georgia EPD"
                        lblMainMessage.Text = "The facility " & EIYear & " EI data has been submitted to Georgia EPD, " &
                                                "but the review process has not yet started. You may choose to " &
                                                "withdraw the submittal to make changes by clicking the button above."
                        lblMainMessage.Width = 500
                        lblFacilityName.Text = "Facility Name:"
                        lblFacilityNameText.Text = FacilityName
                        lblFacilityID.Text = "Facility AIRS No:"
                        lblFacilityIDText.Text = FacilitySiteID
                        lblStatus.Text = "Status:"
                        lblStatusText.Text = eisStatusMessage
                        lblConfNumber.Text = EIYear & " Confirmation No:"
                        lblConfNumberText.Text = ConfNumber
                        lblLastUpdate.Text = "Submitted on:"
                        lblLastUpdateText.Text = GetEISAdminUpdateDateTime(FacilitySiteID, EIYear)
                        btnAction1.Text = "Unsubmit & Make Changes"
                        btnAction2.Text = "Facility Home"
                        btnAction1.Visible = True
                        btnAction2.Visible = True
                    ElseIf OptOut = "1" And EISStatus = "3" Then
                        'Facility opted out of EI but not yet "complete." May choose to opt in - FI and EI menus not available
                        HideFacilityInventoryMenu()
                        HideEmissionInventoryMenu()
                        pnlStatus_Inner.BackColor = GecoColors.ErrorPanel.BackColor
                        lblHeading.Text = "Facility Not Participating In " & EIYear & " Emission Inventory"
                        lblMainMessage.Text = "It has been determined that the facility does not need to participate in the " &
                                                "" & EIYear & " EI process. At this point you have the " &
                                                "ability to change the status. To change the status, click the button above."
                        lblMainMessage.Width = 500
                        lblFacilityName.Text = "Facility Name:"
                        lblFacilityNameText.Text = FacilityName
                        lblFacilityID.Text = "Facility AIRS No:"
                        lblFacilityIDText.Text = FacilitySiteID
                        lblStatus.Text = "Status:"
                        lblStatusText.Text = eisStatusMessage
                        lblOptOutReason.Text = "Reason Not Participating:"
                        lblOptOutReasonText.Text = GetOptOutReason(FacilitySiteID, EIYear)
                        lblConfNumber.Text = EIYear & " Confirmation No:"
                        lblConfNumberText.Text = ConfNumber
                        lblLastUpdate.Text = "Submitted on:"
                        lblLastUpdateText.Text = GetEISAdminUpdateDateTime(FacilitySiteID, EIYear)
                        btnAction1.Text = "Change Status"
                        btnAction2.Text = "Facility Home"
                        btnAction1.Visible = True
                        btnAction2.Visible = True
                    ElseIf OptOut = "0" And EISStatus = "4" Then
                        'Facility opted in, submitted data and is in QA process. No option to make changes. FI and EI menus not available
                        HideFacilityInventoryMenu()
                        HideEmissionInventoryMenu()
                        pnlStatus_Inner.BackColor = GecoColors.ErrorPanel.BackColor
                        lblHeading.Text = EIYear & " Emission Inventory Data Being Reviewed by Georgia EPD"
                        lblMainMessage.Text = "The facility's submitted " & EIYear & " EI data is in the review process and " &
                                                "access to the Facility and Emissions Inventory are restricted. " &
                                                "When the submission is deemed complete access to the Facility " &
                                                "Inventory will be restored."
                        lblMainMessage.Width = 500
                        lblFacilityName.Text = "Facility Name:"
                        lblFacilityNameText.Text = FacilityName
                        lblFacilityID.Text = "Facility AIRS No:"
                        lblFacilityIDText.Text = FacilitySiteID
                        lblStatus.Text = "Status:"
                        lblStatusText.Text = eisStatusMessage
                        lblConfNumber.Text = EIYear & " Confirmation No:"
                        lblConfNumberText.Text = ConfNumber
                        lblLastUpdate.Text = "Submitted on:"
                        lblLastUpdateText.Text = GetEISAdminUpdateDateTime(FacilitySiteID, EIYear)
                        btnAction1.Text = ""
                        btnAction2.Text = "Facility Home"
                        btnAction1.Visible = False
                        btnAction2.Visible = True
                    Else
                        'Some type of error
                        DisplayEIError(FacilitySiteID, "A3")
                    End If
                Case "3"
                    'Facility not in EIS admin table. Contact APB
                    HideFacilityInventoryMenu()
                    HideEmissionInventoryMenu()
                    pnlStatus_Inner.BackColor = GecoColors.ErrorPanel.BackColor
                    lblHeading.Text = "Facility Not In Emission Inventory System"
                    lblMainMessage.Text = "This facility is not in the Emission Inventory System. Contact the " &
                                            "Air Protection Branch for enrollment information."
                    lblMainMessage.Width = 500
                    lblFacilityName.Text = "Facility Name:"
                    lblFacilityNameText.Text = FacilityName
                    lblFacilityID.Text = "Facility AIRS No:"
                    lblFacilityIDText.Text = FacilitySiteID
                    lblStatus.Text = ""
                    lblStatusText.Text = ""
                    lblConfNumber.Text = ""
                    lblConfNumberText.Text = ""
                    lblLastUpdate.Text = ""
                    lblLastUpdateText.Text = ""
                    btnAction1.Text = ""
                    btnAction2.Text = "Facility Home"
                    btnAction1.Visible = False
                    btnAction2.Visible = True
                Case "4"
                    'EIS not available to the facility
                    HideFacilityInventoryMenu()
                    HideEmissionInventoryMenu()
                    pnlStatus_Outer.Visible = True
                    pnlStatus_Inner.Visible = False
                    pnlChange_Inner.Visible = False
                    pnlEISNotAvailable.Visible = True
                Case Else
                    'Some type of error
                    DisplayEIError(FacilitySiteID, "A4")
            End Select
        Catch ex As Exception
            ErrorReport(ex)
        Finally
            'nothing
        End Try

    End Sub

    Private Sub DisplayEIError(fsid As String, errorId As String)

        Dim FacilityName As String = GetFacilityName(fsid)

        HideFacilityInventoryMenu()
        HideEmissionInventoryMenu()
        pnlStatus_Inner.BackColor = GecoColors.ErrorPanel.BackColor
        lblHeading.Text = "Contact the Air Protection Branch"
        lblMainMessage.Text = "There seems to be a data error. Please contact the Air Protection Branch using the contact link above. (Error " & errorId & ".)"
        lblMainMessage.Width = 500
        lblFacilityName.Text = "Facility Name:"
        lblFacilityNameText.Text = FacilityName
        lblFacilityID.Text = "Facility AIRS No:"
        lblFacilityIDText.Text = fsid
        lblStatus.Text = ""
        lblStatusText.Text = ""
        lblConfNumber.Text = ""
        lblConfNumberText.Text = ""
        lblLastUpdate.Text = ""
        lblLastUpdateText.Text = ""
        btnAction1.Text = ""
        btnAction2.Text = "Facility Home"
        btnAction1.Visible = False
        btnAction2.Visible = True

    End Sub

    Protected Sub btnAction2_Click(sender As Object, e As EventArgs) Handles btnAction2.Click

        Response.Redirect("~/Facility/")

    End Sub

    Protected Sub btnAction1_Click(sender As Object, e As EventArgs) Handles btnAction1.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EIYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim OptOut As String = GetCookie(EisCookie.OptOut)

        Select Case EISAccessCode
            Case "1"
                If OptOut = "NULL" And EISStatus = "1" Then
                    'If null, EI not started - show panel to begin - EI and FI menus not yet available
                    'Send to EI reporting period entry
                    Response.Redirect("rp_entry.aspx")
                Else
                    'Some type of error
                    DisplayEIError(FacilitySiteID, "B1")
                End If
            Case "2"
                If OptOut = "0" And EISStatus = "3" Then
                    'Facility opted in and submitted but data not yet n QA or to EPA. Change allowed - FI and EI menus not available
                    'Procedure to make changes
                    lblChangeText.Text = "Withdraw  " & EIYear & " Emissions Inventory submission to make changes. " &
                                            "Data entered will not be affected. Only the submittal status will be changed."
                    pnlStatus_Inner.Visible = False
                    pnlChange_Inner.Visible = True
                    pnlEISNotAvailable.Visible = False
                ElseIf OptOut = "1" And EISStatus = "3" Then
                    'Facility opted out of EI but not yet "complete." May choose to opt in - FI and EI menus not available
                    'Procedure to make changes
                    lblChangeText.Text = "Change status for " & EIYear & " Emissions Inventory. " &
                                            "You will be starting over the Emissions Inventory process."
                    pnlStatus_Inner.Visible = False
                    pnlChange_Inner.Visible = True
                    pnlEISNotAvailable.Visible = False
                Else
                    'Some type of error
                    DisplayEIError(FacilitySiteID, "B2")
                End If
            Case "4"
                'EIS not available to the facility
                HideFacilityInventoryMenu()
                HideEmissionInventoryMenu()
                pnlStatus_Outer.Visible = True
                pnlStatus_Inner.Visible = False
                pnlChange_Inner.Visible = False
                pnlEISNotAvailable.Visible = True
            Case Else
                'Some type of error
                DisplayEIError(FacilitySiteID, "B3")
        End Select

    End Sub

    Protected Sub btnConfirmChange_Click(sender As Object, e As EventArgs) Handles btnConfirmChange.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EIYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Unsubmit(FacilitySiteID, EIYear)
        ResetCookies(FacilitySiteID, EIYear, UpdateUser)
        Response.Redirect("Default.aspx")

    End Sub

    Private Sub Unsubmit(fsid As String, eiyr As String)

        Dim EISAccessCode As String
        Dim EISStatus As String
        Dim OptOut As String = GetCookie(EisCookie.OptOut)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Try
            If OptOut = "0" Then
                'Facility already opted in; no need to change optout status; allow to make changes
                EISAccessCode = "1"
                EISStatus = "2"
            Else
                'Facility was opted out and needs to start over; make optout null
                EISAccessCode = "1"
                EISStatus = "1"
                OptOut = Nothing
            End If

            Dim query = "Update eis_Admin set " &
                " eisStatusCode = @EISStatus, " &
                " eisAccessCode = @EISAccessCode, " &
                " strOptout = @OptOut, " &
                " strOptOutReason = null, " &
                " strConfirmationNumber = null, " &
                " datFinalize = null, " &
                " datEISStatus = getdate(), " &
                " UpdateUser = @UpdateUser, " &
                " UpdateDateTime = getdate() " &
                " where FacilitySiteID = @fsid and " &
                " InventoryYear = @eiyr "

            Dim params = {
                New SqlParameter("@EISStatus", EISStatus),
                New SqlParameter("@EISAccessCode", EISAccessCode),
                New SqlParameter("@OptOut", OptOut),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@eiyr", eiyr)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub ResetCookies(fsid As String, eiyr As String, uuser As String)
        Dim EISCookies As New HttpCookie("EISAccessInfo")
        Dim EISMaxYear As Integer
        Dim enrolled As String = ""
        Dim eisStatus As String = ""
        Dim accesscode As String = ""
        Dim optout As String = ""
        Dim dateFinalize As String = ""
        Dim confirmationnumber As String = ""
        Dim CurrentEIYear As Integer = Now.Year - 1

        Try
            Dim query = "SELECT " &
                "     a.FacilitySiteID, " &
                "     InventoryYear, " &
                "     EISStatusCode, " &
                "     datEISStatus, " &
                "     EISAccessCode, " &
                "     strOptOut, " &
                "     strEnrollment, " &
                "     datFinalize, " &
                "     strConfirmationNumber " &
                " FROM EIS_Admin a " &
                "     INNER JOIN " &
                "     (SELECT " &
                "          max(inventoryYear) AS MaxYear, " &
                "          FacilitySiteID " &
                "      FROM EIS_Admin " &
                "      GROUP BY FacilitySiteID) MaxResults " &
                "         ON MaxResults.FACILITYSITEID = a.FACILITYSITEID " &
                "            AND a.inventoryYear = maxresults.maxyear " &
                " WHERE a.FacilitySiteID = @fsid "

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
                    EISMaxYear = dr.Item("InventoryYear")
                End If
                EISCookies.Values("EISMaxYear") = EncryptDecrypt.EncryptText(EISMaxYear)

                If EISMaxYear = CurrentEIYear Then
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

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        pnlStatus_Inner.Visible = True
        pnlChange_Inner.Visible = False

    End Sub

    Protected Sub btnFacilityHome_Click(sender As Object, e As EventArgs) Handles btnFacilityHome.Click

        Response.Redirect("~/Facility/")

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

End Class