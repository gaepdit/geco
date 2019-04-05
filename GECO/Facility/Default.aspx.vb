Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.FeeBusinessEntity
Imports GECO.GecoModels
Imports GECO.DAL

Partial Class FacilityHome
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property facilityAccess As FacilityAccess
    Private Property currentAirs As ApbFacilityId
    Private Property currentFacility As String = Nothing

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
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            currentAirs = New ApbFacilityId(airsString)
            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        MainLoginCheck(Page.ResolveUrl("~/Facility/?airs=" & currentAirs.ShortString))

        ' Current user
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
        End If

        If Not IsPostBack Then
            LoadFacilityInfo()
            GetApplicationStatus()

            Master.Facility = ConcatNonEmptyStrings(", ", {currentAirs.FormattedString(), currentFacility})

            Title = "GECO Facility Summary - " & currentFacility
            lblAIRS.Text = currentAirs.FormattedString
        End If
    End Sub

    Protected Sub GetApplicationStatus()
        GetFeesStatus()
        GetEisStatus()
        GetEmissionStatementStatus()
        GetTestNotificationStatus()
        GetPermitAppStatus()

        GetCurrentContacts()
    End Sub

    Private Sub LoadFacilityInfo()
        currentFacility = GetFacilityName(currentAirs) & ", " & GetFacilityCity(currentAirs)
        lblFacilityDisplay.Text = currentFacility
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
            AppsPermitFees.Visible = False
            Exit Sub
        End If

        PFLink.NavigateUrl = "~/Fees/?airs=" & currentAirs.ShortString

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

    Private Sub GetPermitAppStatus()
        PALink.NavigateUrl = "~/Permits/?airs=" & currentAirs.ShortString

        Dim dr As DataRow = GetPermitApplicationCounts(currentAirs)

        If dr IsNot Nothing Then
            Dim openCount As Integer = CInt(dr.Item("OpenApplications"))
            PAText.Text = openCount & " open permit application" & If(openCount = 1, "", "s") & "."
        End If
    End Sub

    Protected Sub GetCurrentContacts()
        Try
            Dim query = " SELECT " &
            "     convert(int, STRKEY) as [Key], " &
            "     STRCONTACTFIRSTNAME, " &
            "     STRCONTACTLASTNAME " &
            " FROM APBCONTACTINFORMATION " &
            " where STRAIRSNUMBER = @airs " &
            "       and STRKEY in ('40', '41', '42', '10', '30') "

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

                        If AppsPermitFees.Visible Then
                            lbtnPFContact.Text = name
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

                    Case 30 'Permitting Contact
                        If AppsPermits.Visible Then
                            PAContact.Text = name
                        End If

                End Select
            Next

            If String.IsNullOrWhiteSpace(lbtnEFContact.Text) Then
                lbtnEFContact.Text = "None"
            End If

            If String.IsNullOrWhiteSpace(lbtnPFContact.Text) Then
                lbtnPFContact.Text = "None"
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

            If String.IsNullOrWhiteSpace(PAContact.Text) Then
                PAContact.Text = "None"
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
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
                        lbtnPFContact.Text = txtFName.Text & " " & txtLName.Text

                    Case 41 ' emission inventory
                        lbtnEIContact.Text = txtFName.Text & " " & txtLName.Text

                    Case 42 ' emission statement
                        lbtnESContact.Text = txtFName.Text & " " & txtLName.Text

                    Case 10 ' testing
                        TNContact.Text = txtFName.Text & " " & txtLName.Text

                    Case 30 ' permitting
                        PAContact.Text = txtFName.Text & " " & txtLName.Text

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
            rblContact.Items(0).Text = "Use the current information for the Permit Fees Contact"
            rblContact.Items(1).Text = "Use my GECO contact information for the Permit Fees Contact"
            lblContactHeader.Text = "Update Permit Fees Contact"

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

    Protected Sub lbtnPFContact_Click(sender As Object, e As EventArgs) Handles lbtnPFContact.Click
        Try
            rblContact.SelectedIndex = 0
            rblContact.Items(0).Text = "Use the current information for the Permit Fees Contact"
            rblContact.Items(1).Text = "Use my GECO contact information for the Permit Fees Contact"
            lblContactHeader.Text = "Update Permit Fees Contact"

            lblContactMsg.Visible = False
            hidContactKey.Value = 40

            If lbtnPFContact.Text = "None" Then
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

    Protected Sub PAContact_Click(sender As Object, e As EventArgs) Handles PAContact.Click
        Try
            rblContact.SelectedIndex = 0
            rblContact.Items(0).Text = "Use the current information for the Permitting Contact"
            rblContact.Items(1).Text = "Use my GECO contact information for the Permitting Contact"
            lblContactHeader.Text = "Update Permitting Contact"

            lblContactMsg.Visible = False
            hidContactKey.Value = 30

            If PAContact.Text = "None" Then
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
                txtFax.Text = ""
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

End Class
