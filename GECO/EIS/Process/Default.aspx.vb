Imports GECO.GecoModels
Imports GECO.GecoModels.EIS

Public Class EIS_Process_Default
    Inherits Page

    Public Property CurrentAirs As ApbFacilityId
    Public Property EiStatus As EisStatus
    Public Property SummerDayRequired As Boolean
    Public Property Participating As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("EisProcessStarted") Is Nothing Then
            Response.Redirect("~/EIS/")
        End If

        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.IsBeginEisProcess = True

        EiStatus = GetEiStatus(CurrentAirs)
        SummerDayRequired = CheckSummerDayRequired(CurrentAirs)

        If Not IsPostBack AndAlso Session("EisProcess") IsNot Nothing Then
            LoadCurrentData()
        End If
    End Sub

    Private Sub LoadCurrentData()
        Dim process As EisProcess = GetSessionItem(Of EisProcess)("EisProcess")

        If process IsNot Nothing Then
            txtComment.Text = process.AdminComment

            Select Case process.Opted
                Case OptedInOut.DidNotOperate
                    rOperate.SelectedIndex = 1
                    rThresholds.SelectedIndex = -1
                    pnlColocate.Visible = True
                    pnlEmissions.Visible = False
                    Select Case process.Colocated
                        Case True
                            rColocated.SelectedIndex = 0
                            pnlColocatedWith.Visible = True
                            txtColocatedWith.Text = process.Colocation
                        Case False
                            rColocated.SelectedIndex = 1
                            pnlColocatedWith.Visible = False
                        Case Else
                            rColocated.SelectedIndex = -1
                            pnlColocatedWith.Visible = False
                    End Select
                Case OptedInOut.BelowThresholds
                    rOperate.SelectedIndex = 0
                    rThresholds.SelectedIndex = 0
                    pnlColocate.Visible = True
                    pnlEmissions.Visible = True
                    LoadEmissionsThresholds()
                    Select Case process.Colocated
                        Case True
                            rColocated.SelectedIndex = 0
                            txtColocatedWith.Text = process.Colocation
                            pnlColocatedWith.Visible = True
                        Case False
                            rColocated.SelectedIndex = 1
                            pnlColocatedWith.Visible = False
                        Case Else
                            rColocated.SelectedIndex = -1
                            pnlColocatedWith.Visible = False
                    End Select
                Case OptedInOut.OptedIn
                    rOperate.SelectedIndex = 0
                    rThresholds.SelectedIndex = 1
                    pnlColocate.Visible = False
                    pnlEmissions.Visible = True
                    LoadEmissionsThresholds()
            End Select

            pnlContinue.Visible = True
        End If
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
        SaveAndContinue()
    End Sub

    Private Sub SaveAndContinue()
        Dim opted As OptedInOut
        Dim colocated As Boolean?
        Dim colocation As String

        If rOperate.SelectedValue = "No" Then
            ' Facility opted out because it did not operate
            opted = OptedInOut.DidNotOperate
            colocated = (rColocated.SelectedValue = "Yes")
            colocation = If(colocated, txtColocatedWith.Text, Nothing)
        ElseIf rThresholds.SelectedValue = "Yes" Then
            ' Facility opted out because emissions are below threshold
            opted = OptedInOut.BelowThresholds
            colocated = (rColocated.SelectedValue = "Yes")
            colocation = If(colocated, txtColocatedWith.Text, Nothing)
        Else
            ' Facility opted in
            opted = OptedInOut.OptedIn
            colocated = Nothing
            colocation = Nothing
        End If

        Dim process = New EisProcess With {
            .FacilitySiteId = CurrentAirs,
            .InventoryYear = EiStatus.MaxYear,
            .AdminComment = Left(txtComment.Text, 4000),
            .Opted = opted,
            .Colocated = colocated,
            .Colocation = colocation,
            .UpdateUser = GetCurrentUser().DbUpdateUser
        }

        Session("EisProcess") = process

        If opted = OptedInOut.OptedIn Then
            Response.Redirect("~/EIS/Users/")
        Else
            Response.Redirect("~/EIS/Process/Submit.aspx")
        End If
    End Sub

End Class
