Imports System.DateTime
Imports GECO.GecoModels

Partial Class es_confirm
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        AirsSelectedCheck()

        'Check if the user has access to the Application
        Dim facilityAccess = GetCurrentUser().GetFacilityAccess(New ApbFacilityId(GetCookie(Cookie.AirsNumber).ToString))

        If Not facilityAccess.ESAccess Then
            Response.Redirect("~/NoAccess.aspx")
        End If

        Dim airs As String = GetSessionItem(Of String)("esAirsNumber")

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/Home/")
        End If

        Dim esOptOut As String = GetSessionItem(Of String)("ESOptOut")

        If String.IsNullOrEmpty(esOptOut) Then
            Response.Redirect("~/ES")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.IsFacilitySet = True

        If IsPostBack Then
            Return
        End If

        Dim esYear As String = GetSessionItem(Of String)("ESYear")
        Dim airsYear As String = GetSessionItem(Of String)("AirsYear")

        If esOptOut = "YES" Then
            pnlOptedOutYes.Visible = True
            pnlConfFinal.Visible = False

            lblConfNum1.Text = GetConfirmNumber(airsYear)
            lblESYear1.Text = esYear
            lblDate1.Text = Now.ToShortDate()
            lblAirsNo1.Text = CurrentAirs.FormattedString
            lblFacility1.Text = GetFacilityName(CurrentAirs)
        Else
            pnlOptedOutYes.Visible = False
            pnlConfFinal.Visible = True

            lblESYear3.Text = esYear

            If Not CheckESExist(airsYear) Then
                CreateConfNum(esYear, CurrentAirs)
            End If

            lblVOCAmt2.Text = GetEmissionValue("VOC", airsYear)
            lblNOXAmt2.Text = GetEmissionValue("NOX", airsYear)
            lblConfNumFinalize.Text = GetConfirmNumber(airsYear)
            lblDate2.Text = Now.ToShortDate()
            lblAirsNo2.Text = CurrentAirs.FormattedString
            lblFacility2.Text = GetFacilityName(CurrentAirs)
        End If

    End Sub

    Protected Sub btnMakeChange_Click(sender As Object, e As EventArgs) Handles btnMakeChange.Click
        Response.Redirect("Form.aspx")
    End Sub

    Protected Sub btnEsHome_Click(sender As Object, e As EventArgs) Handles btnEsHome.Click
        Response.Redirect("Default.aspx")
    End Sub

End Class
