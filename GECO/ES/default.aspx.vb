Imports System.DateTime
Imports GECO.GecoModels

Partial Class es_default
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

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/Home/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.IsFacilitySet = True

        Dim esYear As String = (Now.Year - 1).ToString
        Session("ESYear") = esYear
        Session("esAirsNumber") = CurrentAirs.DbFormattedString
        Session("AirsYear") = CurrentAirs.DbFormattedString & esYear

        Dim CountyBoundary = GetCountyBoundary(CurrentAirs.CountySubstring)

        ' Remove negative sign and switch max/min for longitude
        Session("LongMin") = -CountyBoundary.MaxLon
        Session("LongMax") = -CountyBoundary.MinLon
        Session("LatMin") = CountyBoundary.MinLat
        Session("LatMax") = CountyBoundary.MaxLat

        If Not IsPostBack Then
            LoadESYears()
            ShowInitial()
        End If

    End Sub

    Private Sub LoadESYears()

        cboESYear.Items.Clear()
        cboESYear.Items.Add(" -Select Year- ")

        Dim dt = GetEsYears(CurrentAirs)

        For Each dr As DataRow In dt.Rows
            cboESYear.Items.Add(dr.Item("intESYear").ToString)
        Next

    End Sub

    Protected Sub cboESYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboESYear.SelectedIndexChanged

        Dim YearSelected As Integer
        Dim CurrentYear As Integer = Now.Year - 1
        Dim esStatus As String = GetSessionItem(Of String)("esState")
        Dim PastAirsYear As String
        Dim NOxAmt As String
        Dim VOCAmt As String

        If cboESYear.SelectedIndex = 0 Then
            ShowInitial()
        Else
            YearSelected = CInt(cboESYear.SelectedValue)
            If YearSelected = CurrentYear Then
                ShowCurrent()
                lblCurrentStatus.Text = esStatus
                If Left(esStatus, 5) = "Opted" Then
                    btnCurrentES.Text = "Make changes to " & Now.Year - 1 & " ES Data"
                    btnCurrentES.CssClass = "button-large"
                    pnlCurrentESStatus.CssClass = "panel"

                    tblCurrentES.Visible = True
                    lblCurrentFacility.Text = GetFacilityName(CurrentAirs)
                    lblCurrentAirs.Text = CurrentAirs.FormattedString
                    Dim airsYear As String = CurrentAirs.DbFormattedString & CStr(YearSelected)
                    lblCurrentConfNum.Text = GetConfirmNumber(airsYear)

                    NOxAmt = GetEmissionValue("NOx", airsYear)
                    VOCAmt = GetEmissionValue("VOC", airsYear)
                    If NOxAmt = "-1" Then NOxAmt = "0"
                    If VOCAmt = "-1" Then VOCAmt = "0"

                    If NOxAmt <> "0" OrElse VOCAmt <> "0" Then
                        pnlCurrentOptedIn.Visible = True
                        lblCurrentNox.Text = NOxAmt
                        lblCurrentVOC.Text = VOCAmt
                    End If
                End If

                If Left(esStatus, 10) = "Applicable" Then
                    btnCurrentES.Text = "Begin " & Now.Year - 1 & " ES"
                    btnCurrentES.CssClass = "button-large button-proceed"
                    pnlCurrentESStatus.CssClass = "panel panel-inprogress"
                End If
            Else
                ShowPast()
                Session.Add("PastESYear", CStr(YearSelected))
                lblPastYear1.Text = YearSelected.ToString
                lblPastYear2.Text = YearSelected.ToString
                lblPastYear3.Text = YearSelected.ToString
                lblAIRSNo.Text = CurrentAirs.FormattedString
                PastAirsYear = CurrentAirs.DbFormattedString & CStr(YearSelected)
                lblFacilityName.Text = GetFacilityName(CurrentAirs)

                Dim confNum As String = GetConfirmNumber(PastAirsYear)
                If (String.IsNullOrEmpty(confNum)) Then confNum = "No Data"
                lblConfNo.Text = confNum

                NOxAmt = GetEmissionValue("NOx", PastAirsYear)
                VOCAmt = GetEmissionValue("VOC", PastAirsYear)
                If NOxAmt = "-1" Then NOxAmt = "0"
                If VOCAmt = "-1" Then VOCAmt = "0"
                lblNOx.Text = NOxAmt
                lblVOC.Text = VOCAmt

                If (NOxAmt = "0") AndAlso (VOCAmt = "0") Then
                    ShowOptedOut()
                Else
                    ShowOptedIn()
                End If
            End If
        End If
    End Sub

    Private Sub ShowOptedIn()

        pnlOptedIn.Visible = True
        pnlOptedOut.Visible = False

    End Sub

    Private Sub ShowOptedOut()

        pnlOptedIn.Visible = False
        pnlOptedOut.Visible = True

    End Sub

    Private Sub ShowInitial()

        pnlCurrentES.Visible = False
        pnlPastES.Visible = False
        cboESYear.SelectedIndex = 0

    End Sub

    Private Sub ShowCurrent()

        pnlCurrentES.Visible = True
        pnlPastES.Visible = False
        lblCurrentYear.Text = CStr(Now.Year - 1)
        lblCurrentYear2.Text = CStr(Now.Year - 1)

    End Sub

    Private Sub ShowPast()

        pnlCurrentES.Visible = False
        pnlPastES.Visible = True

    End Sub

    Protected Sub btnCurrentES_Click(sender As Object, e As EventArgs) Handles btnCurrentES.Click

        Response.Redirect("Form.aspx")

    End Sub

End Class
