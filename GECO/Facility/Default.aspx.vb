Imports System.DateTime
Imports GaEpd.DBUtilities
Imports GECO.DAL
Imports GECO.DAL.Facility
Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Partial Class FacilityHome
    Inherits Page

    Private Property CurrentUser As GecoUser
    Private Property FacilityAccess As FacilityAccess
    Private Property CurrentAirs As ApbFacilityId

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            If Not ApbFacilityId.TryParse(GetCookie(Cookie.AirsNumber), CurrentAirs) Then
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

            If Not ApbFacilityId.TryParse(airsString, CurrentAirs) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            SetCookie(Cookie.AirsNumber, CurrentAirs.ShortString())
        End If

        Master.CurrentAirs = CurrentAirs
        Master.IsFacilitySet = True

        MainLoginCheck(Page.ResolveUrl($"~/Facility/?airs={CurrentAirs.ShortString}"))

        ' Current user
        CurrentUser = GetCurrentUser()

        FacilityAccess = CurrentUser.GetFacilityAccess(CurrentAirs)

        If FacilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
        End If

        If Not IsPostBack Then
            CheckForMandatoryUpdates()

            Title = $"GECO Facility Summary - {GetFacilityNameAndCity(CurrentAirs)}"
            GetApplicationStatus()
        End If
    End Sub

    Private Sub CheckForMandatoryUpdates()
        ' Require user to set communication preferences if they have never been set.
        If InitialCommunicationPreferenceSettingRequired(CurrentAirs, FacilityAccess, CommunicationCategory.Fees) Then
            HttpContext.Current.Response.Redirect("~/Facility/SetCommunicationPreferences.aspx")
        End If

        ' Require user to confirm preferences every 275 days.
        If CommunicationUpdateResponseRequired(CurrentAirs, FacilityAccess) Then
            HttpContext.Current.Response.Redirect("~/Facility/Contacts.aspx")
        End If

        ' Require user to review facility user access annually.
        If FacilityAccess.AdminAccess AndAlso UserAccessReviewRequested(CurrentAirs) Then
            HttpContext.Current.Response.Redirect("~/Facility/Admin.aspx")
        End If
    End Sub

    Private Sub GetApplicationStatus()
        GetFeesStatus()
        GetEisStatus()
        GetEmissionStatementStatus()
        GetTestNotificationStatus()
        GetPermitAppStatus()
    End Sub

    Private Sub GetEisStatus()
        If Not FacilityAccess.EisAccess Then
            AppsEmissionInventory.Visible = False
            Return
        End If

        EisLink.NavigateUrl = $"~/EIS/?airs={CurrentAirs.ShortString}"

        ' This procedure obtains variable values from the EIS_Admin table and saves values in cookies
        ' Steps: 1 - read stored database values for EISStatusCode, EISStatusCode date, EISAccessCode, OptOut,
        '            Enrollment status, date finalized, last conf number
        '        2 - Saves EISAccessCode for use on entering the EIS home page
        '        3 - If facility is enrolled for current EI year, EISStatus, OptOut, date finalized and conf number cookies are created
        '            Based on values of above, EI status message is created and displayed on Facility Home page
        '        4 - If facility not enrolled - message indicating that the EI is not applicable is displayed

        Dim eiYear As Integer = Now.Year - 1
        Dim eiStatus As EisStatus = GetEiStatus(CurrentAirs)

        If eiStatus.AccessCode >= 3 Then
            AppsEmissionInventory.Visible = False
            Return
        End If

        ' enrollment status: 0 = not enrolled; 1 = enrolled for EI year
        If Not eiStatus.Enrolled Then
            litEmissionsInventory.Text = $"Not enrolled in {eiYear} EI."
            EisLink.Enabled = False
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
                litEmissionsInventory.Text = $"{eiYear} EI not applicable."
            Case Else
                litEmissionsInventory.Text = $"<em>Due: {GetEIDeadline(eiStatus.MaxYear).ToLongDate()}</em><br /><br />Enrolled in {eiYear} EI."
        End Select
    End Sub

    Private Sub GetFeesStatus()
        If Not FacilityAccess.FeeAccess Then
            AppsEmissionFees.Visible = False
            AppsFeesSummary.Visible = False
            Return
        End If

        PFLink.NavigateUrl = $"~/Fees/?airs={CurrentAirs.ShortString}"

        Dim dr As DataRow = GetFeeStatus(CurrentAirs)

        If dr Is Nothing Then
            litEmissionsFees.Text = "Not subject to fees."
            Return
        End If

        Dim submittal As Boolean = CBool(dr.Item("intsubmittal"))
        Dim year As String = dr.Item("numFeeYear").ToString()
        Dim dateSubmitted As Date? = GetNullableDateTime(dr.Item("datsubmittal"))
        Dim dateDue As Date? = GetNullableDateTime(dr.Item("datFeeDueDate"))

        If dateDue.HasValue Then
            litEmissionsFees.Text = $"<em>Due: <span class='no-wrap'>{dateDue.Value.ToLongDate}</span></em><br /><br />"
        End If

        litEmissionsFees.Text &= $"{GetNullableString(dr.Item("strGECODesc"))} {year}."

        If submittal AndAlso dateSubmitted.HasValue Then
            litEmissionsFees.Text &= $"<br /><br /><span class='no-wrap'>Status date: {dateSubmitted.Value.ToLongDate()}</span>."
        End If

    End Sub

    ' FUTURE: In 2025, all ES-related code can be deleted, including permissions.
    Private Sub GetEmissionStatementStatus()
        If Now.Year >= 2025 OrElse Not FacilityAccess.ESAccess Then
            AppsEmissionsStatement.Visible = False
        End If
    End Sub

    Private Sub GetTestNotificationStatus()
        TNLink.NavigateUrl = $"~/TN/?airs={CurrentAirs.ShortString}"

        Dim dr As DataRow = GetPendingTestNotifications(CurrentAirs)

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
                litTestNotifications.Text = $"{pendingTests} pending test notifications."
        End Select
    End Sub

    Private Sub GetPermitAppStatus()
        PALink.NavigateUrl = $"~/Permits/?airs={CurrentAirs.ShortString}"

        Dim dr As DataRow = GetPermitApplicationCounts(CurrentAirs)

        If dr Is Nothing Then
            Return
        End If

        Dim openCount As Integer = CInt(dr.Item("OpenApplications"))

        Select Case openCount
            Case 0
                litPermits.Text = "No open permit applications."
            Case 1
                litPermits.Text = "One open permit application."
            Case Else
                litPermits.Text = $"{openCount} open permit applications."
        End Select
    End Sub

End Class
