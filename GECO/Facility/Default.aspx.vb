Imports EpdIt.DBUtilities
Imports GECO.DAL
Imports GECO.DAL.Facility
Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Partial Class FacilityHome
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property facilityAccess As FacilityAccess
    Private Property currentAirs As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            If Not ApbFacilityId.TryParse(GetCookie(Cookie.AirsNumber), currentAirs) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If
        Else
            ' AIRS number
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.TryParse(airsString, currentAirs) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        Master.CurrentAirs = currentAirs
        Master.IsFacilitySet = True

        MainLoginCheck(Page.ResolveUrl("~/Facility/?airs=" & currentAirs.ShortString))

        ' Current user
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
        End If

        If Not IsPostBack Then
            CheckForMandatoryFeesCommunicationUpdate()

            Title = "GECO Facility Summary - " & GetFacilityNameAndCity(currentAirs)
            GetApplicationStatus()
        End If
    End Sub

    Private Sub CheckForMandatoryFeesCommunicationUpdate()
        If facilityAccess.HasCommunicationPermission(CommunicationCategory.Fees) AndAlso
          InitialCommunicationPreferenceSettingRequired(currentAirs, CommunicationCategory.Fees) Then
            HttpContext.Current.Response.Redirect("~/Facility/SetCommunicationPreferences.aspx")
        End If

        If GetCommunicationUpdate(currentAirs, facilityAccess).ResponseRequired Then
            HttpContext.Current.Response.Redirect("~/Facility/Contacts.aspx")
        End If
    End Sub

    Protected Sub GetApplicationStatus()
        GetFeesStatus()
        GetEisStatus()
        GetEmissionStatementStatus()
        GetTestNotificationStatus()
        GetPermitAppStatus()
    End Sub

    Protected Sub GetEisStatus()
        If Not facilityAccess.EisAccess Then
            AppsEmissionInventory.Visible = False
            Return
        End If

        EisLink.NavigateUrl = "~/EIS/?airs=" & currentAirs.ShortString

        ' This procedure obtains variable values from the EIS_Admin table and saves values in cookies
        ' Steps: 1 - read stored database values for EISStatusCode, EISStatusCode date, EISAccessCode, OptOut,
        '            Enrollment status, date finalized, last conf number
        '        2 - Saves EISAccessCode for use on entering the EIS home page
        '        3 - If facility is enrolled for current EI year, EISStatus, OptOut, date finalized and conf number cookies are created
        '            Based on values of above, EI status message is created and displayed on Facility Home page
        '        4 - If facility not enrolled - message indicating that the EI is not applicable is displayed

        Dim EIYear As Integer = Now.Year - 1
        Dim eiStatus As EisStatus = GetEiStatus(currentAirs)
        SetEiStatusCookies(currentAirs, Response)

        If eiStatus.AccessCode = 3 Then
            AppsEmissionInventory.Visible = False
            Return
        End If

        ' enrollment status: 0 = not enrolled; 1 = enrolled for EI year
        If Not eiStatus.Enrolled Then
            litEmissionsInventory.Text = "Not enrolled in " & EIYear & " EI."
            Return
        End If

        ' | EISSTATUSCODE | STRDESC                  |
        ' |---------------|--------------------------|
        ' | 0             | Not applicable           |
        ' | 1             | Applicable - not started |
        ' | 2             | In progress              |
        ' | 3             | Submitted                |
        ' | 4             | QA Process               |
        ' | 5             | Complete                 |

        Select Case eiStatus.StatusCode
            Case 0
                litEmissionsInventory.Text = EIYear & " EI not applicable."
            Case 1
                litEmissionsInventory.Text = "Ready for " & EIYear & " EI.<br /><em>Due: " &
                  GetEIDeadline(eiStatus.MaxYear).ToLongDate() & "</em>"
            Case 2
                litEmissionsInventory.Text = EIYear & " EI in progress.<br /><em>Due :" &
                  GetEIDeadline(eiStatus.MaxYear).ToLongDate() & "</em>"
            Case 3, 4
                litEmissionsInventory.Text = EIYear & " EI submitted on " & eiStatus.DateFinalized &
                    ".<br /><em>Due: " & GetEIDeadline(eiStatus.MaxYear).ToLongDate() & "</em>"
            Case 5
                litEmissionsInventory.Text = EIYear & " EI completed on " & eiStatus.DateFinalized &
                    ".<br /><em>Due: " & GetEIDeadline(eiStatus.MaxYear).ToLongDate() & "</em>"
            Case Else
                litEmissionsInventory.Text = "To be determined."
        End Select
    End Sub

    Protected Sub GetFeesStatus()
        If Not facilityAccess.FeeAccess Then
            AppsEmissionFees.Visible = False
            AppsFeesSummary.Visible = False
            Return
        End If

        PFLink.NavigateUrl = "~/Fees/?airs=" & currentAirs.ShortString

        Dim dr As DataRow = GetFeeStatus(currentAirs)

        If dr Is Nothing Then
            litEmissionsFees.Text = "Not subject to fees."
            Return
        End If

        Dim submittal As Boolean = CBool(dr.Item("intsubmittal"))
        Dim year As Integer = CInt(dr.Item("numFeeYear"))
        Dim dateSubmitted As Date? = GetNullableDateTime(dr.Item("datsubmittal"))
        Dim dateDue As Date? = GetNullableDateTime(dr.Item("datFeeDueDate"))

        If submittal AndAlso dateSubmitted.HasValue Then
            litEmissionsFees.Text = GetNullableString(dr.Item("strGECODesc")) & " " & year.ToString &
                    " on " & dateSubmitted.Value.ToLongDate() & "."
        Else
            litEmissionsFees.Text = GetNullableString(dr.Item("strGECODesc")) & " " & year.ToString & "."
        End If

        If dateDue.HasValue Then
            litEmissionsFees.Text &= "<br /><em>Due: " & dateDue.Value.ToLongDate & "</em>"
        End If
    End Sub

    Protected Sub GetEmissionStatementStatus()
        If Not facilityAccess.ESAccess Then
            AppsEmissionsStatement.Visible = False
            Return
        End If

        Dim inESCounty As Boolean = CheckFacilityEmissionStatement(currentAirs)
        Dim esStatus As String = StatusES(currentAirs.DbFormattedString & CStr(Now.Year - 1))
        MyBase.Session.Add("esState", esStatus)

        If esStatus = "N/A" OrElse Not inESCounty Then
            AppsEmissionsStatement.Visible = False
            Return
        End If

        litEmissionsStatement.Text = esStatus & "<br /><em>Due: " & New Date(Now.Year, 6, 15).ToLongDate() & "</em>"
    End Sub

    Private Sub GetTestNotificationStatus()
        TNLink.NavigateUrl = "~/TN/?airs=" & currentAirs.ShortString

        Dim dr As DataRow = GetPendingTestNotifications(currentAirs)

        If dr Is Nothing OrElse CInt(dr("total")) = 0 Then
            AppsTestNotifications.Visible = False
            Return
        End If

        Dim pendingTests As Integer = CInt(dr("pending"))

        Select Case pendingTests
            Case 0
                litTestNotifications.Text = "No pending test notifications."
            Case 1
                litTestNotifications.Text = "One pending test notification."
            Case Else
                litTestNotifications.Text = pendingTests & " pending test notifications."
        End Select
    End Sub

    Private Sub GetPermitAppStatus()
        PALink.NavigateUrl = "~/Permits/?airs=" & currentAirs.ShortString

        Dim dr As DataRow = GetPermitApplicationCounts(currentAirs)

        If dr Is Nothing Then
            Return
        End If

        Dim openCount As Integer = CInt(dr.Item("OpenApplications"))
        litPermits.Text = openCount & " open permit application" & If(openCount = 1, "", "s") & "."
    End Sub

End Class
