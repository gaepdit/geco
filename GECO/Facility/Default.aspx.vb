Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.DAL
Imports GECO.GecoModels

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

            Master.SetFacility(ConcatNonEmptyStrings(", ", {currentAirs.FormattedString(), currentFacility}))

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
        Dim dr As DataRow = GetAPBContactInformation(hidContactKey.Value)

        If dr IsNot Nothing Then
            'Getting details for the user from table apbcontactinformation.
            'This table has all the information that goes into panel facility information.

            txtFName.Text = GetNullableString(dr.Item("strcontactfirstname"))
            txtLName.Text = GetNullableString(dr.Item("strcontactlastname"))
            txtTitle.Text = GetNullableString(dr.Item("strcontacttitle"))
            txtCoName.Text = GetNullableString(dr.Item("strcontactcompanyname"))
            txtPhone.Text = GetNullableString(dr.Item("strcontactphonenumber"))
            txtFax.Text = GetNullableString(dr.Item("strcontactfaxnumber"))
            txtEmailContact.Text = GetNullableString(dr.Item("strcontactemail"))
            txtAddress.Text = GetNullableString(dr.Item("strcontactaddress"))
            txtCity.Text = GetNullableString(dr.Item("strcontactcity"))
            txtState.Text = GetNullableString(dr.Item("strcontactstate"))
            txtZip.Text = GetNullableString(dr.Item("strcontactzipcode"))
        Else
            ClearContact()
        End If
    End Sub

    Protected Sub GetEisStatus()
        If Not facilityAccess.EisAccess Then
            AppsEmissionInventory.Visible = False
            Return
        End If

        ' This procedure obtains variable values from the EIS_Admin table and saves values in cookies
        ' Steps: 1 - read stored database values for EISStatusCode, EISStatusCode date, EISAccessCode, OptOut, Enrollment status, date finalized, last conf number
        '        2 - Saves EISAccessCode for use on entering the EIS home page
        '        3 - If facility is enrolled for current EI year, EISStatus, OptOut, date finalized and conf number cookies are created
        '            Based on values of above, EI status message is created and displayed on Facility Home page
        '        4 - If facility not enrolled - message indicating that the EI is not applicable is displayed
        Dim EIYear As Integer = Now.Year - 1

        Dim eiStatus As EiStatus = LoadEiStatusCookies(currentAirs, Response)

        If eiStatus.Access = "3" Then
            AppsEmissionInventory.Visible = False
            Return
        End If

        ' enrollment status: 0 = not enrolled; 1 = enrolled for EI year
        If eiStatus.Enrolled <> "1" Then
            lblEIText.Text = "Not enrolled in " & EIYear & " EI."
            lblEIDate.Text = ""
            Return
        End If

        lblEIDate.Text = GetEIDeadline(eiStatus.EIMaxYear)

        ' | EISSTATUSCODE | STRDESC                  |
        ' |---------------|--------------------------|
        ' | 0             | Not applicable           |
        ' | 1             | Applicable - not started |
        ' | 2             | In progress              |
        ' | 3             | Submitted                |
        ' | 4             | QA Process               |
        ' | 5             | Complete                 |

        Select Case eiStatus.Status
            Case "0"
                lblEIText.Text = EIYear & " EI not applicable."
                lblEIDate.Text = ""
            Case "1"
                lblEIText.Text = "Ready for " & EIYear & " EI."
            Case "2"
                lblEIText.Text = EIYear & " EI in progress."
            Case "3", "4"
                lblEIText.Text = EIYear & " EI submitted on " & eiStatus.DateFinalized & "."
            Case "5"
                lblEIText.Text = EIYear & " EI completed on " & eiStatus.DateFinalized & "."
            Case Else
                lblEIText.Text = "To be determined."
                lblEIDate.Text = ""
        End Select
    End Sub

    Protected Sub GetFeesStatus()
        If Not facilityAccess.FeeAccess Then
            AppsEmissionFees.Visible = False
            AppsFeesSummary.Visible = False
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

        If Not (facilityAccess.FeeAccess OrElse facilityAccess.AdminAccess) Then
            PAContact.Enabled = False
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
                If String.IsNullOrWhiteSpace(name) OrElse name = "N/A N/A" Then
                    name = "None"
                End If

                Select Case dr.Item("Key")

                    Case 40 'Fee Contact
                        If AppsEmissionFees.Visible Then
                            lbtnEFContact.Text = name
                        End If

                        If AppsFeesSummary.Visible Then
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
            Dim contactDescription As String = "Contact updated from GECO Facility Home page by " & currentUser.FullName &
                " on " & Now.ToShortDateString()

            Dim params As SqlParameter() = {
                New SqlParameter("@strcontactfirstname", txtFName.Text),
                New SqlParameter("@strcontactlastname", txtLName.Text),
                New SqlParameter("@strcontacttitle", txtTitle.Text),
                New SqlParameter("@strcontactphonenumber1", txtPhone.Text),
                New SqlParameter("@strcontactfaxnumber", txtFax.Text),
                New SqlParameter("@strcontactemail", txtEmailContact.Text),
                New SqlParameter("@strcontactaddress1", txtAddress.Text),
                New SqlParameter("@strcontactcity", txtCity.Text),
                New SqlParameter("@strcontactstate", Address.ProbableStateCode(txtState.Text)),
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
                txtPhone.Text = currentUser.PhoneNumber
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
    End Sub

#End Region

End Class
