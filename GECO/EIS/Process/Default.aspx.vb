Imports GECO.GecoModels

Public Class EIS_Process_Default
    Inherits Page

    Public Property CurrentAirs As ApbFacilityId
    Public Property EiStatus As EisStatus
    Public Property SummerDayRequired As Boolean
    Public Property Participating As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If GetCookie(Cookie.EiProcess) Is Nothing Then
            Response.Redirect("~/EIS/")
        End If

        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.Users
        Master.IsBeginEisProcess = True

        EiStatus = GetEiStatus(CurrentAirs)
        SummerDayRequired = CheckSummerDayRequired(CurrentAirs)
    End Sub

    Private Sub LoadEmissionsThresholds()
        gThresholds.DataSource = GetEiThresholds(EiStatus.MaxYear, SummerDayRequired)
        gThresholds.DataBind()
    End Sub

    Private Sub rOperate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rOperate.SelectedIndexChanged
        If rOperate.SelectedValue = "Yes" Then
            pnlColocate.Visible = False
            pnlEmissions.Visible = True

            LoadEmissionsThresholds()
        Else
            pnlColocate.Visible = True
            pnlEmissions.Visible = False

            rColocated.SelectedIndex = -1
        End If

        pnlContinue.Visible = False
    End Sub

    Private Sub rColocated_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rColocated.SelectedIndexChanged
        pnlColocatedWith.Visible = rColocated.SelectedValue = "Yes"
        pnlContinue.Visible = True
    End Sub

    Private Sub rThresholds_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rThresholds.SelectedIndexChanged
        If rThresholds.SelectedValue = "Yes" Then
            ' "Yes" means emissions are below threshold, not subject
            pnlColocate.Visible = True
            pnlContinue.Visible = False

            rColocated.SelectedIndex = -1
        Else
            ' "No" means emissions are above threshold
            pnlColocate.Visible = False
            pnlContinue.Visible = True
        End If
    End Sub

    Private Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click
        pnlContinue.Visible = False
        pnlSubmit.Visible = True

        If rOperate.SelectedValue = "No" Then
            ' Facility opted out because it did not operate
            Participating = 1
        ElseIf rThresholds.SelectedValue = "Yes" Then
            ' Facility opted out because emissions are below threshold
            Participating = 2
        Else
            ' Facility opted in
            Participating = 3
        End If

        EnableDisableControls(False)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        pnlContinue.Visible = True
        pnlSubmit.Visible = False

        EnableDisableControls(True)
    End Sub

    Private Sub EnableDisableControls(enable As Boolean)
        rOperate.Enabled = enable
        txtComment.Enabled = enable
        rThresholds.Enabled = enable
        rColocated.Enabled = enable
        txtColocatedWith.Enabled = enable
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        SaveAndSubmit()
    End Sub

    Private Sub SaveAndSubmit()
        Dim currentUser As GecoUser = GetCurrentUser()
        Dim year As Integer = EiStatus.MaxYear

        Dim colocated As Boolean = (rColocated.SelectedValue = "Yes")
        Dim colocation As String = If(colocated, txtColocatedWith.Text, Nothing)

        ' Save FacilityStatus as OP even if not
        SaveEisFacilityStatus(CurrentAirs, FacilitySiteStatusCode.OP, currentUser.DbUpdateUser, year)
        SaveAdminComment(CurrentAirs, year, txtComment.Text)

        If rOperate.SelectedValue = "No" Then
            ' Facility opted out because it did not operate
            SaveEisOptOut(CurrentAirs, True, currentUser.DbUpdateUser, year, "1", colocated, colocation)
        ElseIf rThresholds.SelectedValue = "Yes" Then
            ' Facility opted out because emissions are below threshold
            SaveEisOptOut(CurrentAirs, True, currentUser.DbUpdateUser, year, "2", colocated, colocation)
        Else
            ' Facility opted in
            SaveEisOptOut(CurrentAirs, False, currentUser.DbUpdateUser, year)
        End If

        ClearCookie(Cookie.EiProcess)
        Response.Redirect("~/EIS/")
    End Sub

End Class
