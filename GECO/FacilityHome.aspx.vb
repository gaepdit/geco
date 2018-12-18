Imports System.Data
Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.FeeBusinessEntity
Imports GECO.GecoModels

Partial Class FacilityHome
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property facilityAccess As FacilityAccess
    Private Property currentAirs As ApbFacilityId

#Region " Page Load "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            currentAirs = New ApbFacilityId(GetCookie(Cookie.AirsNumber))
        Else
            ' AIRS number
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.IsValidAirsNumberFormat(airsString) Then
                HttpContext.Current.Response.Redirect("~/UserHome.aspx")
            End If

            currentAirs = New ApbFacilityId(airsString)
            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        MainLoginCheck(Page.ResolveUrl("~/FacilityHome.aspx?airs=" & currentAirs.ShortString))

        ' Current user
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/UserHome.aspx")
        End If

        If Not IsPostBack Then
            txtEmail.Enabled = facilityAccess.AdminAccess
            btnAddUser.Enabled = facilityAccess.AdminAccess
            pnlAddNewUser.Visible = facilityAccess.AdminAccess

            GetApplicationStatus()

            LoadFacilityLocation()

            Dim mpUserLabel, mpFacilityLabel, mpAirsLabel As Label
            mpUserLabel = CType(Master.FindControl("lblUserName"), Label)
            mpUserLabel.Text = "Welcome, " & currentUser.FullName & " | "
            mpFacilityLabel = CType(Master.FindControl("lblFacilityName"), Label)
            mpFacilityLabel.Text = "Facility: " & lblFacilityName.Text & " | "
            mpAirsLabel = CType(Master.FindControl("lblAirsNo"), Label)
            mpAirsLabel.Text = "AIRS No: " & currentAirs.FormattedString()

            Title = "GECO - " & lblFacilityName.Text
            lblAIRS.Text = currentAirs.FormattedString
        End If
    End Sub

    Protected Sub GetApplicationStatus()
        GetFeesStatus()
        GetEisStatus()
        GetEmissionStatementStatus()
        GetTestNotificationStatus()

        GetCurrentContacts()
    End Sub

#End Region

#Region " Admin/User Tools "

    Private Sub LoadUserGrid()
        grdUsers.DataSource = GetUserAccess()
        grdUsers.DataBind()
    End Sub

    Protected Sub grdUsers_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdUsers.RowDeleting
        Dim userid As Decimal = Convert.ToDecimal(grdUsers.DataKeys(e.RowIndex).Values("NUMUSERID").ToString())
        Dim airsnumber As String = grdUsers.DataKeys(e.RowIndex).Values("STRAIRSNUMBER").ToString()

        DeleteUserAccess(userid, airsnumber)

        LoadUserGrid()
    End Sub

    Protected Sub grdUsers_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdUsers.RowUpdating
        Dim userid As Decimal = Convert.ToDecimal(grdUsers.DataKeys(e.RowIndex).Values("NUMUSERID").ToString())
        Dim airsnumber As String = grdUsers.DataKeys(e.RowIndex).Values("STRAIRSNUMBER").ToString()
        Dim intAdminAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(5).Controls(0), CheckBox).Checked
        Dim intFeeAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(6).Controls(0), CheckBox).Checked
        Dim intEIAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(7).Controls(0), CheckBox).Checked
        Dim intESAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(8).Controls(0), CheckBox).Checked

        UpdateUserAccess(intAdminAccess, intFeeAccess, intEIAccess, intESAccess, userid, airsnumber)

        grdUsers.EditIndex = -1
        LoadUserGrid()
    End Sub

    Protected Sub grdUsers_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdUsers.RowEditing
        grdUsers.EditIndex = e.NewEditIndex
        LoadUserGrid()
    End Sub

    Protected Sub grdUsers_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdUsers.RowCancelingEdit
        grdUsers.EditIndex = -1
        LoadUserGrid()
    End Sub

    Protected Sub grdUsers_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUsers.RowDataBound
        If facilityAccess.AdminAccess Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(0), LinkButton)
                linkBtn.ValidationGroup = False
            End If
        Else
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        Dim returnValue As Integer = InsertUserAccess(txtEmail.Text, currentAirs)

        Select Case returnValue
            Case 1 'Successfully added
                lblMessage.Visible = False
                txtEmail.Text = ""
                LoadUserGrid()

            Case -1 'User not registered
                lblMessage.Visible = True

            Case -2 'User access already exists
                lblMessage.Visible = True
                lblMessage.Text = "The user already has access to the facility."

            Case Else
                lblMessage.Visible = True
                lblMessage.Text = "There was an error adding the User. Please try again or contact us if the problem persists"

        End Select
    End Sub

#End Region

#Region " Load data "

    Protected Sub LoadFacilityContact()
        Dim phonenumber As String = ""
        Dim dr As DataRow = GetAPBContactInformation(hidContactKey.Value)

        If dr IsNot Nothing Then
            'Getting details for the user from table apbcontactinformation.
            'This table has all the information that goes into panel facility information.

            txtFName.Text = GetNullableString(dr.Item("strcontactfirstname"))
            txtLName.Text = GetNullableString(dr.Item("strcontactlastname"))
            txtTitle.Text = GetNullableString(dr.Item("strcontacttitle"))
            txtCoName.Text = GetNullableString(dr.Item("strcontactcompanyname"))
            phonenumber = GetNullableString(dr.Item("strcontactphonenumber"))
            txtFax.Text = GetNullableString(dr.Item("strcontactfaxnumber"))
            txtEmailContact.Text = GetNullableString(dr.Item("strcontactemail"))
            txtAddress.Text = GetNullableString(dr.Item("strcontactaddress"))
            txtCity.Text = GetNullableString(dr.Item("strcontactcity"))
            txtState.Text = GetNullableString(dr.Item("strcontactstate"))
            txtZip.Text = GetNullableString(dr.Item("strcontactzipcode"))

            If phonenumber.Length > 10 Then
                txtPhone.Text = Mid(phonenumber, 1, 10)
                txtPhoneExt.Text = Mid(phonenumber, 11)
            Else
                txtPhone.Text = phonenumber
                txtPhoneExt.Text = ""
            End If
        Else
            ClearContact()
        End If
    End Sub

    Protected Sub LoadFacilityLocation()
        Try
            Dim query = "Select * " &
            " FROM VW_APBFacilityLocation " &
            " where strAIRSNumber = @airs "

            Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then
                lblFacilityName.Text = GetNullableString(dr.Item("strFacilityname"))
                lblAddress.Text = GetNullableString(dr.Item("strFacilityStreet1"))
                lblCityStateZip.Text = GetNullableString(dr.Item("strFacilityCity")) & ", " &
                    GetNullableString(dr.Item("strFacilityState")) & " " &
                    Address.FormatPostalCode(GetNullableString(dr.Item("strFacilityZipCode")))
                lblLatitude.Text = GetNullableString(dr.Item("numFacilityLatitude"))
                lblLongitude.Text = GetNullableString(dr.Item("numFacilityLongitude"))
                lblCounty.Text = GetNullableString(dr.Item("strCountyName"))
                lblDistrict.Text = GetNullableString(dr.Item("strDistrictName"))
                lblOffice.Text = GetNullableString(dr.Item("strOfficeName"))

                If IsDBNull(dr.Item("strDistrictResponsible")) OrElse
                    dr.Item("strDistrictResponsible") <> "True" Then
                    hlDistrict.Visible = False
                Else
                    hlDistrict.Visible = True
                End If
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub LoadFacilityHeaderData()
        Dim query = "Select * " &
        " FROM VW_APBFacilityHeader " &
        " where strAIRSNumber = @airs "

        Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

        Dim dr As DataRow = DB.GetDataRow(query, param)

        If dr IsNot Nothing Then

            Select Case GetNullableString(dr.Item("strOperationalstatus"))
                Case "O"
                    lblOpStatus.Text = "Operational"
                Case "P"
                    lblOpStatus.Text = "Planned"
                Case "C"
                    lblOpStatus.Text = "Under Construction"
                Case "T"
                    lblOpStatus.Text = "Temporarily Closed"
                Case "X"
                    lblOpStatus.Text = "Closed/Dismantled"
                Case "I"
                    lblOpStatus.Text = "Seasonal Operation"
                Case Else
                    lblOpStatus.Text = "Unknown (fix needed)"
            End Select

            lblClassification.Text = GetNullableString(dr.Item("strClass"))

            Dim startupDate As Date? = GetNullableDateTime(dr.Item("datStartupDate"))
            lblStartUp.Text = If(startupDate.HasValue, startupDate.Value.ToShortDateString, String.Empty)

            Dim shutdownDate As Date? = GetNullableDateTime(dr.Item("datShutDownDate"))
            lblClosed.Text = If(shutdownDate.HasValue, shutdownDate.Value.ToShortDateString, String.Empty)

            lblCMSStatus.Text = GetNullableString(dr.Item("strCMSMember"))

            lblSICCode.Text = GetNullableString(dr.Item("strSICCode"))

            AddAirProgramCodes(GetNullableString(dr.Item("strAIRProgramcodes")))
        End If
    End Sub

    Protected Sub AddAirProgramCodes(AirProgramCode As String)
        If String.IsNullOrEmpty(AirProgramCode) Then
            Exit Sub
        End If

        Dim codes As New List(Of String)

        If Mid(AirProgramCode, 1, 1) = 1 Then
            codes.Add("SIP")
        End If
        If Mid(AirProgramCode, 5, 1) = 1 Then
            codes.Add("PSD")
        End If
        If Mid(AirProgramCode, 6, 1) = 1 Then
            codes.Add("NSR")
        End If
        If Mid(AirProgramCode, 7, 1) = 1 Then
            codes.Add("NESHAP")
        End If
        If Mid(AirProgramCode, 8, 1) = 1 Then
            codes.Add("NSPS")
        End If
        If Mid(AirProgramCode, 12, 1) = 1 Then
            codes.Add("MACT")
        End If
        If Mid(AirProgramCode, 13, 1) = 1 Then
            codes.Add("Title V")
        End If
        If Mid(AirProgramCode, 10, 1) = 1 Then
            codes.Add("Acid Precipitation")
        End If

        lblAirProgramCodes.Text = ConcatNonEmptyStrings(", ", codes.ToArray)
    End Sub

    Protected Sub GetEisStatus()
        If Not facilityAccess.EisAccess Then
            AppsEmissionInventory.Visible = False
            Exit Sub
        End If

        ' This procedure obtains variable values from the EIS_Admin table and saves values in cookies
        ' Steps: 1 - read stored database values for EISStatusCode, EISStatusCode date, EISAccessCode, OptOut, Enrollment status, date finalized, last conf number
        '        2 - Saves EISAccessCode for use on entering the EIS home page
        '        3 - If facility is enrolled for current EI year, EISStatus, OptOut, date finalized and conf number cookies are created
        '            Based on values of above, EI status message is created and displayed on Facility Home page
        '        4 - If facility not enrolled - message indicating that the EI is not applicable is displayed
        Dim eiStatus As String = ""
        Dim CurrentEIYear As Integer = Now.Year - 1
        Dim EISMaxYear As Integer = 0
        Dim EIDeadlineDate As String = ""
        Dim enrolled As String = ""
        Dim eisStatus As String = ""
        Dim accesscode As String = ""
        Dim eisStatusMessage As String = ""
        Dim optout As String = ""
        Dim dateFinalize As String = ""
        Dim confirmationnumber As String = ""
        Dim eisAirsNumber As String = currentAirs.ShortString
        Dim EISCookies As New HttpCookie("EISAccessInfo")

        Try
            Dim query As String = "Select eis_admin.FacilitySiteID, eis_admin.InventoryYear, " &
                    "EIS_Admin.EISStatusCode, EIS_Admin.datEISStatus, " &
                    "EIS_Admin.EISAccessCode, EIS_Admin.strOptOut, " &
                    "EIS_Admin.strEnrollment, EIS_Admin.datFinalize, " &
                    "EIS_Admin.strConfirmationNumber FROM EIS_Admin, " &
                    "(select max(inventoryYear) as MaxYear, " &
                    "EIS_Admin.FacilitySiteID " &
                    "FROM EIS_Admin GROUP BY EIS_Admin.FacilitySiteID ) MaxResults  " &
                    "where EIS_Admin.FacilitySiteID = @eisAirsNumber " &
                    "and EIS_Admin.inventoryYear = maxresults.maxyear " &
                    "and EIS_Admin.FacilitySiteID = maxresults.FacilitySiteID " &
                    "group by EIS_Admin.FacilitySiteID, " &
                    "EIS_Admin.inventoryYear, " &
                    "EIS_Admin.EISStatusCode, EIS_Admin.datEISStatus, " &
                    "EIS_Admin.EISAccessCode, EIS_Admin.strOptOut, " &
                    "EIS_Admin.strEnrollment, EIS_Admin.datFinalize, " &
                    "EIS_Admin.strConfirmationNumber"

            Dim param As New SqlParameter("@eisAirsNumber", eisAirsNumber)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr Is Nothing Then
                'Set EISAccess cookie to "3" if facility does not exist in EIS Admin table
                EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText("3")
                AppsEmissionInventory.Visible = False
            Else
                'get max year from EIS Admin table
                If IsDBNull(dr("InventoryYear")) Then
                    'Do nothing - leave EISMaxYear null
                Else
                    EISMaxYear = dr.Item("InventoryYear")
                End If
                EISCookies.Values("EISMaxYear") = EncryptDecrypt.EncryptText(EISMaxYear)

                If EISMaxYear = CurrentEIYear Then

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

                    'Check enrollment
                    'get enrollment status: 0 = not enrolled; 1 = enrolled for EI year
                    If IsDBNull(dr("strEnrollment")) Then
                        enrolled = "NULL"
                    Else
                        enrolled = dr.Item("strEnrollment")
                    End If
                    EISCookies.Values("Enrollment") = EncryptDecrypt.EncryptText(enrolled)

                    If enrolled = "1" Then
                        If IsDBNull(dr("strOptOut")) OrElse String.IsNullOrEmpty(dr("stroptout")) Then
                            optout = "NULL"
                        Else
                            optout = dr.Item("strOptOut")
                        End If
                        EISCookies.Values("OptOut") = EncryptDecrypt.EncryptText(optout)

                        If IsDBNull(dr("datFinalize")) Then
                            dateFinalize = "NULL"
                        Else
                            dateFinalize = GetNullableDateTime(dr.Item("datFinalize")).Value.ToShortDateString
                        End If
                        EISCookies.Values("DateFinalize") = EncryptDecrypt.EncryptText(dateFinalize)

                        If IsDBNull(dr("strConfirmationNumber")) Then
                            confirmationnumber = "NULL"
                        Else
                            confirmationnumber = dr.Item("strConfirmationNumber")
                        End If
                        EISCookies.Values("ConfNumber") = EncryptDecrypt.EncryptText(confirmationnumber)

                        EIDeadlineDate = GetEIDeadline(EISMaxYear)
                        lblEIDate.Text = EIDeadlineDate

                        Select Case eisStatus
                            Case "0"
                                eisStatusMessage = "Not applicable for " & CurrentEIYear & "."
                                lblEIDate.Text = ""
                            Case "1"
                                eisStatusMessage = "Applicable; not started."
                            Case "2"
                                eisStatusMessage = "EI in progress."
                            Case "3"
                                If optout = "1" Then
                                    eisStatusMessage = "Completed & submitted on " & dateFinalize & "."
                                Else
                                    eisStatusMessage = "EI data submitted on " & dateFinalize & "."
                                End If
                            Case "4"
                                eisStatusMessage = "In review; submitted on " & dateFinalize & "."
                            Case "5"
                                If optout = "1" Then
                                    eisStatusMessage = "Completed on " & dateFinalize & " (no EI data submitted)."
                                Else
                                    eisStatusMessage = "EI data submitted on " & dateFinalize & "."
                                End If
                            Case Else
                                eisStatusMessage = "To be determined."
                                lblEIDate.Text = ""
                        End Select

                        lblEIText.Text = eisStatusMessage

                    Else
                        lblEIText.Text = "Not enrolled for " & CurrentEIYear & "*."
                        lblEIDate.Text = ""
                    End If
                Else
                    lblEIText.Text = "Facility not in " & CurrentEIYear & " EI*."
                    lblEIDate.Text = ""
                    'Set EISAccessCode = "0" for Facility Inventory Access only
                    EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText("0")
                    EISCookies.Values("EISStatus") = EncryptDecrypt.EncryptText("0")
                    EISCookies.Values("Enrollment") = EncryptDecrypt.EncryptText("0")
                End If

            End If

            EISCookies.Expires = DateTime.Now.AddHours(8)
            Response.Cookies.Add(EISCookies)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub GetFeesStatus()
        If Not facilityAccess.FeeAccess Then
            AppsEmissionFees.Visible = False
            Exit Sub
        End If

        Dim dr As DataRow = GetFeeStatus(currentAirs)

        If dr IsNot Nothing Then
            Dim submittal As Boolean = dr.Item("intsubmittal")
            Dim year As Integer = dr.Item("numFeeYear")
            Dim dateSubmitted As Date? = GetNullableDateTime(dr.Item("datsubmittal"))
            Dim dateDue As Date? = GetNullableDateTime(dr.Item("datFeeDueDate"))

            If submittal AndAlso dateSubmitted.HasValue Then
                litEFText.Text = GetNullableString(dr.Item("strGECODesc")) & " " & year.ToString & " on " & dateSubmitted.Value.ToShortDateString & "."
            Else
                litEFText.Text = GetNullableString(dr.Item("strGECODesc")) & " " & year.ToString & "."
            End If

            If dateDue.HasValue Then
                lblEFDate.Text = dateDue.Value.ToShortDateString
            Else
                lblEFDate.Text = "N/A"
            End If
        Else
            litEFText.Text = "Not subject to fees."
        End If
    End Sub

    Protected Sub GetEmissionStatementStatus()
        If Not facilityAccess.ESAccess Then
            AppsEmissionsStatement.Visible = False
            Exit Sub
        End If

        Try
            Dim esYear As String = Now.Year - 1
            Dim AirsYear As String = currentAirs.DbFormattedString & esYear
            Dim esStatus As String = ""
            Dim inESCounty As Boolean = CheckFacilityEmissionStatement(currentAirs)

            esStatus = StatusES(AirsYear)

            If esStatus = "N/A" Then
                AppsEmissionsStatement.Visible = False
            Else
                ESLink.Text = "Emission Statement"
                lblESText.Text = esStatus
                lblESDate.Text = "June&nbsp;15, " & Now.Year
            End If

            If Not inESCounty Then
                AppsEmissionsStatement.Visible = False
            End If

            Session.Add("esState", esStatus)
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub GetTestNotificationStatus()
        Dim query As String = " select " &
        "     count(*)        as total, " &
        "     sum(case when DATPROPOSEDSTARTDATE >= getdate() " &
        "         then 1 " &
        "         else 0 end) as pending " &
        " FROM ISMPTESTNOTIFICATION " &
        " where STRAIRSNUMBER = @AirsNumber "

        Dim param As New SqlParameter("@AirsNumber", currentAirs.DbFormattedString)

        Dim dr As DataRow = DB.GetDataRow(query, param)

        If dr Is Nothing OrElse CInt(dr("total")) = 0 Then
            AppsTestNotifications.Visible = False
            Exit Sub
        End If

        Dim pendingTests As Integer = dr("pending")

        Select Case pendingTests
            Case 0
                TNText.Text = "No pending test notifications."
            Case 1
                TNText.Text = "One pending test notification."
            Case Else
                TNText.Text = pendingTests.ToString & " pending test notifications."
        End Select
    End Sub

    Protected Sub GetCurrentContacts()
        Try
            Dim query = " SELECT " &
            "     convert(int, STRKEY) as [Key], " &
            "     STRCONTACTFIRSTNAME, " &
            "     STRCONTACTLASTNAME " &
            " FROM APBCONTACTINFORMATION " &
            " where STRAIRSNUMBER = @airs " &
            "       and STRKEY in ('40', '41', '42', '10') "

            Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

            Dim dt As DataTable = DB.GetDataTable(query, param)

            For Each dr As DataRow In dt.Rows
                Dim name As String = GetNullableString(dr.Item("STRCONTACTFIRSTNAME")) & " " & GetNullableString(dr.Item("STRCONTACTLASTNAME"))
                If String.IsNullOrWhiteSpace(name) Or name = "N/A N/A" Then
                    name = "None"
                End If

                Select Case dr.Item("Key")

                    Case 40 'Fee Contact
                        If AppsEmissionFees.Visible Then
                            lbtnEFContact.Text = name
                        End If

                    Case 41 'EIS Contact
                        If AppsEmissionInventory.Visible Then
                            lbtnEIContact.Text = name
                        End If

                    Case 42 'ES Contact
                        If AppsEmissionsStatement.Visible Then
                            lbtnESContact.Text = name
                            ESLink.Text = "Emission Statement"
                            ESLink.Visible = True
                        End If

                    Case 10 'Testing Contact
                        If AppsTestNotifications.Visible Then
                            TNContact.Text = name
                        End If

                End Select
            Next

            If String.IsNullOrWhiteSpace(lbtnEFContact.Text) Then
                lbtnEFContact.Text = "None"
            End If

            If String.IsNullOrWhiteSpace(lbtnEIContact.Text) Then
                lbtnEIContact.Text = "None"
            End If

            If String.IsNullOrWhiteSpace(lbtnESContact.Text) Then
                lbtnESContact.Text = "None"
            End If

            If String.IsNullOrWhiteSpace(TNContact.Text) Then
                TNContact.Text = "None"
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub LoadStateContactInformation()
        Try
            Dim query As String =
            " SELECT PermittingStaff.strAIRSNumber, " &
            "        SSCPEngineer, " &
            "        SSCPUnit, " &
            "        SSCPEmailAddress, " &
            "        SSCPPhone, " &
            "        ISMPEngineer, " &
            "        ISMPUnit, " &
            "        ISMPEmailAddress, " &
            "        ISMPPhone, " &
            "        SSPPEngineer, " &
            "        SSPPUnit, " &
            "        SSPPEmailAddress, " &
            "        SSPPPhone, " &
            "        DistrictOffice " &
            " FROM " &
            " ( " &
            "     SELECT DISTINCT " &
            "            ((strLastName+', '+strFirstName)) AS SSPPEngineer, " &
            "            SSPPApplicationMaster.strAIRSnumber, " &
            "            strEmailAddress AS SSPPEmailAddress, " &
            "            strPhone AS SSPPPhone, " &
            "            strUnitDesc AS SSPPUnit " &
            "     FROM EPDUserProfiles " &
            "          LEFT JOIN LookUpEPDUnits  " &
            " 	    ON EPDUserProfiles.numUnit = LookUpEPDUnits.numUnitCode " &
            "          LEFT JOIN SSPPApplicationMaster  " &
            " 	    ON EPDUserProfiles.numUserID = strStaffResponsible " &
            "                                             AND SSPPApplicationMaster.strApplicationNumber = " &
            "     ( " &
            "         SELECT DISTINCT " &
            "                (MAX(CAST(strApplicationNumber AS INT))) AS GreatestApplication " &
            "         FROM SSPPApplicationMaster " &
            "         WHERE SSPPApplicationMaster.strAIRSNumber = @airs " &
            "     ) " &
            "     WHERE SSPPApplicationMaster.strAIRSnumber = @airs " &
            " ) PermittingStaff " &
            "  " &
            " LEFT JOIN " &
            " ( " &
            "     SELECT(strLastName+', '+strFirstName) AS SSCPEngineer, " &
            "           strEmailAddress AS SSCPEmailAddress, " &
            "           strPhone AS SSCPPhone, " &
            "           SSCPFacilityAssignment.strAIRSNumber, " &
            "           strUnitDesc AS SSCPUnit " &
            "     FROM EPDUserProfiles " &
            "          LEFT JOIN LookUpEPDUnits  " &
            " 	    ON EPDUserProfiles.numUnit = LookUpEPDUnits.numUnitCode " &
            "          LEFT JOIN SSCPFacilityAssignment  " &
            " 	    ON EPDUserProfiles.numUserID = SSCPFacilityAssignment.strSSCPEngineer " &
            "     WHERE SSCPFacilityAssignment.[strAIRSNumber] = @airs " &
            " ) ComplianceStaff ON PermittingStaff.strairsnumber = ComplianceStaff.strairsnumber " &
            "  " &
            " LEFT JOIN " &
            " ( " &
            "     SELECT DISTINCT " &
            "            ((strLastName+', '+strFirstName)) AS ISMPEngineer, " &
            "            ISMPMaster.strAIRSNumber, " &
            "            strEmailAddress AS ISMPEmailAddress, " &
            "            strPhone AS ISMPPhone, " &
            "            strUnitDesc AS ISMPUnit " &
            "     FROM EPDUserProfiles " &
            "          LEFT JOIN LookUpEPDUnits  " &
            " 	    ON EPDUserProfiles.numUnit = LookUpEPDunits.numunitCode " &
            "          LEFT JOIN ISMPReportInformation  " &
            " 	    ON EPDUserProfiles.numUserID = strReviewingEngineer " &
            "          INNER JOIN ISMPMaster  " &
            " 	    ON ISMPMaster.strReferenceNumber = ISMPReportInformation.strReferenceNumber " &
            "                                   AND strClosed = 'True' " &
            "     WHERE ISMPReportInformation.datCompleteDate = " &
            "     ( " &
            "         SELECT DISTINCT " &
            "                (MAX(datCompleteDate)) AS CompleteDate " &
            "         FROM ISMPReportInformation, " &
            "              ISMPMaster " &
            "         WHERE ISMPReportInformation.strReferenceNumber = ISMPMaster.strReferenceNumber " &
            "               AND ISMPMaster.strAIRSNumber = @airs " &
            "               AND strClosed = 'True' " &
            "     ) " &
            "           AND ISMPMaster.strAIRSNumber = @airs " &
            " ) MonitoringStaff  " &
            " ON PermittingStaff.strairsnumber = monitoringstaff.strairsnumber " &
            "  " &
            " LEFT JOIN " &
            " ( " &
            "     SELECT SSCPDistrictResponsible.strAIRSNumber, " &
            "            CASE " &
            "                WHEN strDistrictResponsible = 'True' " &
            "                THEN strOfficeName " &
            "                ELSE '' " &
            "            END DistrictOffice " &
            "     FROM SSCPDistrictResponsible, " &
            "          LookUpDistrictInformation, " &
            "          LookUpDistrictOffice " &
            "     WHERE LookUpDistrictOffice.strDistrictCode = LookUpDistrictInformation.strDistrictCode " &
            "           AND LookUpDistrictInformation.strDistrictCounty = SUBSTRING(SSCPDistrictResponsible.strAIRSNumber, 5, 3) " &
            "           AND SSCPDistrictResponsible.strAIRSNumber = @airs " &
            " ) DistrictStaff  " &
            " ON PermittingStaff.strairsnumber = DistrictStaff.strairsnumber"

            Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then

                If IsDBNull(dr.Item("SSCPengineer")) OrElse
                    Left(GetNullableString(dr.Item("SSCPengineer")), 10) = "Unassigned" Then
                    lblComplianceContactName.Text = "Unassigned"
                Else
                    lblComplianceContactName.Text = GetNullableString(dr.Item("SSCPengineer"))
                End If
                If IsDBNull(dr.Item("SSCPemailaddress")) Then
                    hlComplianceContactEmail.Text = "N/A"
                Else
                    hlComplianceContactEmail.Text = GetNullableString(dr.Item("SSCPemailaddress"))
                    hlComplianceContactEmail.NavigateUrl = "mailto:" & GetNullableString(dr.Item("SSCPemailaddress"))
                End If
                If IsDBNull(dr.Item("SSCPphone")) Then
                    lblComplianceContactPhone.Text = "N/A"
                Else
                    lblComplianceContactPhone.Text = GecoUser.FormatPhoneNumber(GetNullableString(dr.Item("SSCPphone")))
                End If

                If IsDBNull(dr.Item("ISMPengineer")) OrElse
                    Left(GetNullableString(dr.Item("ISMPengineer")), 10) = "Unassigned" Then
                    lblMonitoringContactName.Text = "Unassigned"
                Else
                    lblMonitoringContactName.Text = GetNullableString(dr.Item("ISMPengineer"))
                End If
                If IsDBNull(dr.Item("ISMPemailaddress")) Then
                    hlMonitoringContactEmail.Text = "N/A"
                Else
                    hlMonitoringContactEmail.Text = GetNullableString(dr.Item("ISMPemailaddress"))
                    hlMonitoringContactEmail.NavigateUrl = "mailto:" & GetNullableString(dr.Item("ISMPemailaddress"))
                End If
                If IsDBNull(dr.Item("ISMPphone")) Then
                    lblMonitoringContactPhone.Text = "N/A"
                Else
                    lblMonitoringContactPhone.Text = GecoUser.FormatPhoneNumber(GetNullableString(dr.Item("ISMPphone")))
                End If

                If IsDBNull(dr.Item("SSPPengineer")) OrElse
                    Left(GetNullableString(dr.Item("SSPPengineer")), 10) = "Unassigned" Then
                    lblPermitContactName.Text = "Unassigned"
                Else
                    lblPermitContactName.Text = GetNullableString(dr.Item("SSPPengineer"))
                End If
                If IsDBNull(dr.Item("SSPPemailaddress")) Then
                    hlPermitContactEmail.Text = "N/A"
                Else
                    hlPermitContactEmail.Text = GetNullableString(dr.Item("SSPPemailaddress"))
                    hlPermitContactEmail.NavigateUrl = "mailto:" & GetNullableString(dr.Item("SSPPemailaddress"))
                End If
                If IsDBNull(dr.Item("SSPPphone")) Then
                    lblPermitContactPhone.Text = "N/A"
                Else
                    lblPermitContactPhone.Text = GecoUser.FormatPhoneNumber(GetNullableString(dr.Item("SSPPphone")))
                End If
            Else
                lblPermitContactPhone.Text = "N/A"
                hlPermitContactEmail.Text = "N/A"
                lblPermitContactName.Text = "Unassigned"
                lblMonitoringContactPhone.Text = "N/A"
                hlMonitoringContactEmail.Text = "N/A"
                lblMonitoringContactName.Text = "Unassigned"
                lblComplianceContactPhone.Text = "N/A"
                hlComplianceContactEmail.Text = "N/A"
                lblComplianceContactName.Text = "Unassigned"
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

#Region " Tab panels tab changed "

    Protected Sub FacilityTabs_ActiveTabChanged(sender As Object, e As EventArgs) Handles FacilityTabs.ActiveTabChanged
        Select Case FacilityTabs.ActiveTabIndex
            'Case 0 'Application Status

            Case 1 'Facility Summary
                LoadFacilityHeaderData()
                LoadStateContactInformation()

            Case 2 ' Air Programs
                LoadPermitStatus()
                LoadFeesData()

            Case 3 'Admin Tools
                LoadUserGrid()

        End Select
    End Sub

#End Region

#Region " Contacts editing "

    Protected Sub rblContact_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblContact.SelectedIndexChanged
        LoadCurrentContact()
    End Sub

    Protected Sub btnUpdateContact_Click(sender As Object, e As EventArgs) Handles btnUpdateContact.Click
        Try
            Dim phone, contactDescription As String
            phone = txtPhone.Text & txtPhoneExt.Text

            contactDescription = "Contact updated from GECO Facility Home page by " & currentUser.FullName &
                " on " & Now.ToShortDateString()

            Dim params As SqlParameter() = {
                New SqlParameter("@strcontactfirstname", txtFName.Text),
                New SqlParameter("@strcontactlastname", txtLName.Text),
                New SqlParameter("@strcontacttitle", txtTitle.Text),
                New SqlParameter("@strcontactphonenumber1", phone),
                New SqlParameter("@strcontactfaxnumber", txtFax.Text),
                New SqlParameter("@strcontactemail", txtEmailContact.Text),
                New SqlParameter("@strcontactaddress1", txtAddress.Text),
                New SqlParameter("@strcontactcity", txtCity.Text),
                New SqlParameter("@strcontactstate", txtState.Text),
                New SqlParameter("@strcontactzipcode", txtZip.Text),
                New SqlParameter("@strcontactcompanyname", txtCoName.Text),
                New SqlParameter("@strcontactdescription", contactDescription),
                New SqlParameter("@strmodifingperson", "0"),
                New SqlParameter("@STRAIRSNUMBER", currentAirs.DbFormattedString),
                New SqlParameter("@strkey", hidContactKey.Value),
                New SqlParameter("@strcontactkey", currentAirs.DbFormattedString & hidContactKey.Value.ToString)
            }

            Dim query As String = "select convert(bit, count(*)) " &
            " FROM APBCONTACTINFORMATION " &
            " where STRAIRSNUMBER = @STRAIRSNUMBER " &
            " and STRKEY = convert(varchar(2), @strkey)"

            If DB.GetBoolean(query, params) Then
                query = "Update apbcontactinformation set " &
                    "strcontactfirstname = @strcontactfirstname, " &
                    "strcontactlastname = @strcontactlastname, " &
                    "strcontacttitle = @strcontacttitle, " &
                    "strcontactphonenumber1 = @strcontactphonenumber1, " &
                    "strcontactfaxnumber = @strcontactfaxnumber, " &
                    "strcontactemail = @strcontactemail, " &
                    "strcontactaddress1 = @strcontactaddress1, " &
                    "strcontactcity = @strcontactcity, " &
                    "strcontactstate = @strcontactstate, " &
                    "strcontactzipcode = @strcontactzipcode, " &
                    "strcontactcompanyname = @strcontactcompanyname, " &
                    "strcontactdescription = @strcontactdescription, " &
                    "datmodifingdate = getdate(), " &
                    "strmodifingperson = @strmodifingperson " &
                    "where strcontactkey = @strcontactkey "
            Else
                query = " insert into APBCONTACTINFORMATION ( " &
                    "     STRCONTACTKEY, " &
                    "     STRAIRSNUMBER, " &
                    "     STRKEY, " &
                    "     STRCONTACTFIRSTNAME, " &
                    "     STRCONTACTLASTNAME, " &
                    "     STRCONTACTTITLE, " &
                    "     STRCONTACTCOMPANYNAME, " &
                    "     STRCONTACTPHONENUMBER1, " &
                    "     STRCONTACTFAXNUMBER, " &
                    "     STRCONTACTEMAIL, " &
                    "     STRCONTACTADDRESS1, " &
                    "     STRCONTACTCITY, " &
                    "     STRCONTACTSTATE, " &
                    "     STRCONTACTZIPCODE, " &
                    "     STRMODIFINGPERSON, " &
                    "     DATMODIFINGDATE, " &
                    "     STRCONTACTDESCRIPTION " &
                    " ) " &
                    " values ( " &
                    "     @STRCONTACTKEY, " &
                    "     @STRAIRSNUMBER, " &
                    "     @STRKEY, " &
                    "     @STRCONTACTFIRSTNAME, " &
                    "     @STRCONTACTLASTNAME, " &
                    "     @STRCONTACTTITLE, " &
                    "     @STRCONTACTCOMPANYNAME, " &
                    "     @STRCONTACTPHONENUMBER1, " &
                    "     @STRCONTACTFAXNUMBER, " &
                    "     @STRCONTACTEMAIL, " &
                    "     @STRCONTACTADDRESS1, " &
                    "     @STRCONTACTCITY, " &
                    "     @STRCONTACTSTATE, " &
                    "     @STRCONTACTZIPCODE, " &
                    "     0, " &
                    "     getdate(), " &
                    "     @STRCONTACTDESCRIPTION " &
                    " ) "
            End If

            If DB.RunCommand(query, params) Then
                lblContactMsg.Visible = True
                lblContactMsg.Text = "The current contact has been updated successfully."

                Select Case hidContactKey.Value
                    Case 40  ' emission fees
                        lbtnEFContact.Text = txtFName.Text & " " & txtLName.Text

                    Case 41 ' emission inventory
                        lbtnEIContact.Text = txtFName.Text & " " & txtLName.Text

                    Case 42 ' emission statement
                        lbtnESContact.Text = txtFName.Text & " " & txtLName.Text

                    Case 10 ' testing
                        TNContact.Text = txtFName.Text & " " & txtLName.Text

                End Select
            Else
                lblContactMsg.Visible = True
                lblContactMsg.Text = "There was an error updating the contact."
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub lbtnEFContact_Click(sender As Object, e As EventArgs) Handles lbtnEFContact.Click
        Try
            rblContact.SelectedIndex = 0
            rblContact.Items(0).Text = "Use the current information for the Emission Fees Contact"
            rblContact.Items(1).Text = "Use my GECO contact information for the Emission Fees Contact"
            lblContactHeader.Text = "Update Emission Fees Contact"

            lblContactMsg.Visible = False
            hidContactKey.Value = 40

            If lbtnEFContact.Text = "None" Then
                ClearContact()
            Else
                LoadCurrentContact()
            End If

            pnlContact.Visible = True
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub lbtnEIContact_Click(sender As Object, e As EventArgs) Handles lbtnEIContact.Click
        Try
            rblContact.SelectedIndex = 0
            rblContact.Items(0).Text = "Use the current information for the Emission Inventory Contact"
            rblContact.Items(1).Text = "Use my GECO contact information for the Emission Inventory Contact"
            lblContactHeader.Text = "Update Emission Inventory Contact"

            lblContactMsg.Visible = False
            hidContactKey.Value = 41

            If lbtnEIContact.Text = "None" Then
                ClearContact()
            Else
                LoadCurrentContact()
            End If

            pnlContact.Visible = True
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub lbtnESContact_Click(sender As Object, e As EventArgs) Handles lbtnESContact.Click
        Try
            rblContact.SelectedIndex = 0
            rblContact.Items(0).Text = "Use the current information for the Emission Statement Contact"
            rblContact.Items(1).Text = "Use my GECO contact information for the Emission Statement Contact"
            lblContactHeader.Text = "Update Emission Statement Contact"

            lblContactMsg.Visible = False
            hidContactKey.Value = 42

            If lbtnESContact.Text = "None" Then
                ClearContact()
            Else
                LoadCurrentContact()
            End If

            pnlContact.Visible = True
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub TNContact_Click(sender As Object, e As EventArgs) Handles TNContact.Click
        Try
            rblContact.SelectedIndex = 0
            rblContact.Items(0).Text = "Use the current information for the Testing Contact"
            rblContact.Items(1).Text = "Use my GECO contact information for the Testing Contact"
            lblContactHeader.Text = "Update Testing Contact"

            lblContactMsg.Visible = False
            hidContactKey.Value = 10

            If TNContact.Text = "None" Then
                ClearContact()
            Else
                LoadCurrentContact()
            End If

            pnlContact.Visible = True
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnCloseContact_Click(sender As Object, e As EventArgs) Handles btnCloseContact.Click
        pnlContact.Visible = False
    End Sub

    Protected Sub LoadCurrentContact()
        If rblContact.SelectedIndex = 0 Then
            LoadFacilityContact()
        Else
            If currentUser IsNot Nothing Then
                txtFName.Text = currentUser.FirstName
                txtLName.Text = currentUser.LastName
                txtTitle.Text = currentUser.Title
                txtCoName.Text = currentUser.Company
                txtEmailContact.Text = currentUser.Email
                txtFax.Text = currentUser.FaxNumber
                txtAddress.Text = currentUser.Address.Street
                txtCity.Text = currentUser.Address.City
                txtState.Text = currentUser.Address.State
                txtZip.Text = currentUser.Address.PostalCode

                If currentUser.PhoneNumber.Length > 10 Then
                    txtPhone.Text = Mid(currentUser.PhoneNumber, 1, 10)
                    txtPhoneExt.Text = Mid(currentUser.PhoneNumber, 11)
                Else
                    txtPhone.Text = currentUser.PhoneNumber
                    txtPhoneExt.Text = ""
                End If
            End If
        End If
    End Sub

    Protected Sub ClearContact()
        txtFName.Text = ""
        txtLName.Text = ""
        txtTitle.Text = ""
        txtCoName.Text = ""
        txtEmailContact.Text = ""
        txtFax.Text = ""
        txtAddress.Text = ""
        txtCity.Text = ""
        txtState.Text = ""
        txtZip.Text = ""
        txtPhone.Text = ""
        txtPhoneExt.Text = ""
    End Sub

#End Region

#Region "PermitStatus"

    Private Property GridViewSortDirection() As String

        Get
            Return IIf(ViewState("SortDirection") = Nothing, "ASC", ViewState("SortDirection"))
        End Get
        Set(value As String)
            ViewState("SortDirection") = value
        End Set

    End Property

    Private Property GridViewSortExpression() As String

        Get
            Return IIf(ViewState("SortExpression") = Nothing, String.Empty, ViewState("SortExpression"))
        End Get
        Set(value As String)
            ViewState("SortExpression") = value
        End Set

    End Property

    Private Function GetSortDirection() As String

        Select Case GridViewSortDirection
            Case "ASC"
                GridViewSortDirection = "DESC"
            Case "DESC"
                GridViewSortDirection = "ASC"
        End Select

        Return GridViewSortDirection

    End Function

    Protected Sub gridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            GridView1.DataSource = SortDataTable(Session("MyView"), True)
            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataBind()
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Function SortDataTable(pdataTable As DataTable, isPageIndexChanging As Boolean) As DataView

        If Not pdataTable Is Nothing Then
            Dim pdataView As New DataView(pdataTable)
            If GridViewSortExpression <> String.Empty Then
                If isPageIndexChanging Then
                    pdataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection)
                Else
                    pdataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GetSortDirection())
                End If
            End If
            Return pdataView
        Else
            Return New DataView()
        End If

    End Function

    Protected Sub gridView1_Sorting(sender As Object, e As GridViewSortEventArgs)
        Try
            GridViewSortExpression = e.SortExpression

            Dim pageIndex As Integer = GridView1.PageIndex
            GridView1.DataSource = SortDataTable(Session("MyView"), False)

            Dim gridView As GridView = DirectCast(sender, GridView)
            GridView1.DataBind()
            GridView1.PageIndex = pageIndex
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub grdInvoices_Sorting(sender As Object, e As GridViewSortEventArgs)
        Try
            GridViewSortExpression = e.SortExpression

            Dim pageIndex As Integer = grdInvoices.PageIndex
            grdInvoices.DataSource = SortDataTable(Session("grdInvoices"), False)

            Dim gridView As GridView = DirectCast(sender, GridView)
            grdInvoices.DataBind()
            grdInvoices.PageIndex = pageIndex

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub grdInvoices_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            grdInvoices.DataSource = SortDataTable(Session("grdInvoices"), True)
            grdInvoices.PageIndex = e.NewPageIndex
            grdInvoices.DataBind()
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub grdDeposits_Sorting(sender As Object, e As GridViewSortEventArgs)
        Try
            GridViewSortExpression = e.SortExpression

            Dim pageIndex As Integer = grdDeposits.PageIndex
            grdDeposits.DataSource = SortDataTable(Session("grdDeposits"), False)

            Dim gridView As GridView = DirectCast(sender, GridView)
            grdDeposits.DataBind()
            grdDeposits.PageIndex = pageIndex

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub grdDeposits_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Try
            grdDeposits.DataSource = SortDataTable(Session("grdDeposits"), True)
            grdDeposits.PageIndex = e.NewPageIndex
            grdDeposits.DataBind()
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim gridView As GridView = DirectCast(sender, GridView)

            If e.Row.RowType = DataControlRowType.Header Then
                Dim cellIndex As Integer = -1
                For Each field As DataControlField In gridView.Columns
                    e.Row.Cells(gridView.Columns.IndexOf(field)).CssClass = "headerstyle"

                    If field.SortExpression = GridViewSortExpression Then
                        cellIndex = gridView.Columns.IndexOf(field)
                    End If
                Next

                If cellIndex > -1 Then
                    ' this is a header row,
                    ' set the sort style
                    e.Row.Cells(cellIndex).CssClass = IIf(GridViewSortDirection = "DESC", "sortascheaderstyle", "sortdescheaderstyle")
                End If
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub GridView1_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        Try
            Dim gridView As GridView = DirectCast(sender, GridView)

            If e.Row.RowType = DataControlRowType.Header Then
                Dim cellIndex As Integer = -1
                For Each field As DataControlField In gridView.Columns
                    e.Row.Cells(gridView.Columns.IndexOf(field)).CssClass = "headerstyle"

                    If field.SortExpression = GridViewSortExpression Then
                        cellIndex = gridView.Columns.IndexOf(field)
                    End If
                Next

                If cellIndex > -1 Then
                    ' this is a header row,
                    ' set the sort style
                    e.Row.Cells(cellIndex).CssClass = IIf(GridViewSortDirection = "DESC", "sortascheaderstyle", "sortdescheaderstyle")
                End If
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim hlFinal As HyperLink = DirectCast(e.Row.FindControl("hlFinalPermit"), HyperLink)
                Dim hlApp As HyperLink = DirectCast(e.Row.FindControl("hlAppDetails"), HyperLink)

                Dim appNo As String = ""
                Dim Permit As String = ""

                appNo = GridView1.DataKeys(e.Row.RowIndex).Values("strApplicationNumber").ToString()

                Dim query = "Select VFinal, PSDFinal, OtherPermit " &
                    " FROM VW_GA_Permits " &
                    " where applicationNumber = @appNo "

                Dim param As New SqlParameter("@appNo", appNo)

                Dim dr = DB.GetDataRow(query, param)

                If dr IsNot Nothing Then
                    If IsDBNull(dr("VFinal")) Then
                    Else
                        Permit = dr.Item("VFinal")
                    End If
                    If IsDBNull(dr("PSDFinal")) Then
                    Else
                        Permit = dr.Item("PSDFinal")
                    End If
                    If IsDBNull(dr("OtherPermit")) Then
                    Else
                        Permit = dr.Item("OtherPermit")
                    End If

                    hlFinal.NavigateUrl = "http://permitsearch.gaepd.org/permit.aspx?id=" & Permit
                End If
                hlApp.NavigateUrl = "~/AppNoDetails.aspx?id=" & appNo & "&Permit=" & Permit
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub LoadPermitStatus()
        Try
            Dim query = " SELECT DISTINCT " &
            "     convert(INT, m.strApplicationNumber)                                                 AS strApplicationNumber, " &
            "     isnull(d.strFacilityName, '')                                                        AS strFacilityName, " &
            "     CASE WHEN strPermitNumber IS NULL " &
            "         THEN '' " &
            "     ELSE SUBSTRING(strPermitNumber, 1, 4) + '-' + SUBSTRING(strPermitNumber, 5, 3) + '-' + " &
            "          SUBSTRING(strPermitNumber, 8, 4) + '-' + SUBSTRING(strPermitNumber, 12, 1) + '-' + " &
            "          SUBSTRING(strPermitNumber, 13, 2) + '-' + SUBSTRING(strPermitNumber, 15, 1) END AS strPermitNumber, " &
            "     isnull(strApplicationTypeDesc, '')                                                   AS strApplicationType, " &
            "     CASE " &
            "     WHEN datPermitIssued IS NOT NULL OR datFinalizedDate IS NOT NULL " &
            "         THEN '11-Closed Out' " &
            "     WHEN datToDirector IS NOT NULL AND datFinalizedDate IS NULL AND " &
            "          (datDraftIssued IS NULL OR datDraftIssued < datToDirector) " &
            "         THEN '09-Administrative Review' " &
            "     WHEN datToBranchCheif IS NOT NULL AND datFinalizedDate IS NULL AND datToDirector IS NULL AND " &
            "          (datDraftIssued IS NULL OR datDraftIssued < datToBranchCheif) " &
            "         THEN '09-Administrative Review' " &
            "     WHEN datEPAEnds IS NOT NULL " &
            "         THEN '08-EPA 45-day Review' " &
            "     WHEN datPNExpires IS NOT NULL AND datPNExpires < GETDATE() " &
            "         THEN '07-Public Notice Expired' " &
            "     WHEN datPNExpires IS NOT NULL AND datPNExpires >= GETDATE() " &
            "         THEN '06-Public Notice' " &
            "     WHEN datDraftIssued IS NOT NULL AND datPNExpires IS NULL " &
            "         THEN '05-Draft Issued' " &
            "     WHEN dattoPMII IS NOT NULL " &
            "         THEN '04-At Program Manager' " &
            "     WHEN dattoPMI IS NOT NULL " &
            "         THEN '03-At Unit Coordinator' " &
            "     WHEN datReviewSubmitted IS NOT NULL AND (strSSCPUnit <> '0' OR strISMPUnit <> '0') " &
            "         THEN '02-Internal Review' " &
            "     WHEN strStaffResponsible IS NULL OR strStaffResponsible = '000' " &
            "         THEN '0-Unassigned' " &
            "     ELSE '01-At Engineer' " &
            "     END                                                                                  AS AppStatus, " &
            "     CASE " &
            "     WHEN datPermitIssued IS NOT NULL " &
            "         THEN CONVERT(VARCHAR(10), datPermitIssued, 120) " &
            "     WHEN convert(date, DATFINALIZEDDATE) = '1776-07-04' " &
            "         THEN null " &
            "     WHEN datFinalizedDate IS NOT NULL " &
            "         THEN CONVERT(VARCHAR(10), datFinalizedDate, 120) " &
            "     WHEN datToDirector IS NOT NULL " &
            "          AND (datDraftIssued IS NULL OR datDraftIssued < datToDirector) " &
            "         THEN CONVERT(VARCHAR(10), datToDirector, 120) " &
            "     WHEN datToBranchCheif IS NOT NULL AND datToDirector IS NULL " &
            "          AND (datDraftIssued IS NULL OR datDraftIssued < datToBranchCheif) " &
            "         THEN CONVERT(VARCHAR(10), DatTOBranchCheif, 120) " &
            "     WHEN datEPAEnds IS NOT NULL " &
            "         THEN CONVERT(VARCHAR(10), datEPAEnds, 120) " &
            "     WHEN datPNExpires IS NOT NULL AND datPNExpires < GETDATE() " &
            "         THEN CONVERT(VARCHAR(10), datPNExpires, 120) " &
            "     WHEN datPNExpires IS NOT NULL AND datPNExpires >= GETDATE() " &
            "         THEN CONVERT(VARCHAR(10), datPNExpires, 120) " &
            "     WHEN datDraftIssued IS NOT NULL AND datPNExpires IS NULL " &
            "         THEN CONVERT(VARCHAR(10), datDraftIssued, 120) " &
            "     WHEN dattoPMII IS NOT NULL " &
            "         THEN CONVERT(VARCHAR(10), datToPMII, 120) " &
            "     WHEN dattoPMI IS NOT NULL " &
            "         THEN CONVERT(VARCHAR(10), datToPMI, 120) " &
            "     WHEN datReviewSubmitted IS NOT NULL AND (strSSCPUnit <> '0' OR strISMPUnit <> '0') " &
            "         THEN CONVERT(VARCHAR(10), datReviewSubmitted, 120) " &
            "     WHEN strStaffResponsible IS NULL OR strStaffResponsible = '000' " &
            "         THEN 'Unknown' " &
            "     ELSE CONVERT(VARCHAR(10), datAssignedToEngineer, 120) " &
            "     END                                                                                  AS StatusDate, " &
            "     case when u.STRLASTNAME is null and u.STRFIRSTNAME is null " &
            "     then '' else " &
            "     isnull(u.STRLASTNAME, '') + ', ' + isnull(u.STRFIRSTNAME, '') end                    AS StaffResponsible, " &
            "     isnull(strPermitTypeDescription, '')                                                 AS strPermitType, " &
            "     isnull(u.STREMAILADDRESS, '')                                                        AS strUserEmail " &
            " FROM SSPPAPPLICATIONMASTER m " &
            "     INNER JOIN SSPPAPPLICATIONDATA d " &
            "         ON m.strApplicationNumber = d.strApplicationNumber " &
            "     INNER JOIN SSPPAPPLICATIONTRACKING t " &
            "         ON m.strApplicationNumber = t.strApplicationNumber " &
            "     LEFT JOIN EPDUSERPROFILES u " &
            "         ON m.strStaffResponsible = u.NUMUSERID " &
            "            AND u.NUMEMPLOYEESTATUS = 1 " &
            "     LEFT JOIN LOOKUPAPPLICATIONTYPES la " &
            "         ON m.strApplicationType = la.STRAPPLICATIONTYPECODE " &
            "     LEFT JOIN LOOKUPPERMITTYPES lp " &
            "         ON m.STRPERMITTYPE = lp.strPermitTypeCode " &
            " WHERE m.strAIRSNumber = @airs " &
            " ORDER BY strApplicationNumber DESC "

            Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

            Dim permitsDataTable = DB.GetDataTable(query, param)

            Session("MyView") = permitsDataTable

            If permitsDataTable IsNot Nothing AndAlso
                permitsDataTable.Rows.Count > 0 Then

                GridView1.DataSource = permitsDataTable
                GridView1.DataBind()
                lblGridView1.Visible = False
            Else
                lblGridView1.Visible = True
            End If
        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub
       
    Protected Sub LoadFeesData()
        Try
            grdInvoices.DataSource = GetInvoices()
            grdInvoices.DataBind()
            grdDeposits.DataSource = GetDeposits()
            grdDeposits.DataBind()

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

End Class
