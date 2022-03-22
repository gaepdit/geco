Imports System.Data.SqlClient
Imports System.DateTime
Imports GECO.GecoModels

Partial Class es_default
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
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

        Dim query = "Select intESYear FROM esSchema where strAirsNumber = @AirsNumber order by intESYear Desc"
        Dim param As New SqlParameter("@AirsNumber", CurrentAirs.DbFormattedString)
        Dim dt = DB.GetDataTable(query, param)

        For Each dr As DataRow In dt.Rows
            cboESYear.Items.Add(dr.Item("intESYear").ToString)
        Next

    End Sub

    Protected Sub cboESYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboESYear.SelectedIndexChanged

        Dim YearSelected As Integer
        Dim CurrentYear As Integer = Now.Year - 1
        Dim esYear As Integer
        Dim esStatus As String = GetSessionItem(Of String)("esState")
        Dim PastAirsYear As String
        Dim NOxAmt As String
        Dim VOCAmt As String

        If cboESYear.SelectedIndex = 0 Then
            ShowInitial()
            Session("esprintsource") = ""
        Else
            YearSelected = CInt(cboESYear.SelectedValue)
            esYear = YearSelected
            If esYear = CurrentYear Then
                ShowCurrent()
                lblCurrentStatus.Text = esStatus
                If Left(esStatus, 5) = "Opted" Then
                    btnCurrentES.Text = "Make changes to " & Now.Year - 1 & " ES Data"
                    btnCurrentES.CssClass = "button-large"
                    pnlCurrentESStatus.CssClass = "panel panel-complete text-centered"
                End If
                If Left(esStatus, 10) = "Applicable" Then
                    btnCurrentES.Text = "Begin " & Now.Year - 1 & " ES"
                    btnCurrentES.CssClass = "button-large button-proceed"
                    pnlCurrentESStatus.CssClass = "panel panel-inprogress text-centered"
                End If
            Else
                ShowPast()
                Session.Add("PastESYear", CStr(YearSelected))
                lblPastYear1.Text = YearSelected.ToString
                lblPastYear2.Text = YearSelected.ToString
                lblAIRSNo.Text = CurrentAirs.FormattedString
                PastAirsYear = CurrentAirs.DbFormattedString & CStr(YearSelected)
                lblFacilityName.Text = GetFacilityName(CurrentAirs)
                lblConfNo.Text = GetConfirmNumber(PastAirsYear)

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

    Private Shared Function GetConfirmNumber(ay As String) As String

        Dim query As String = "Select strConfirmationNbr FROM esSchema Where strAirsYear = @ay "
        Dim ConfNum As String = DB.GetString(query, New SqlParameter("@ay", ay))
        Return If(String.IsNullOrEmpty(ConfNum), "No Data", ConfNum)

    End Function

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

    Protected Sub btnCurrentES_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCurrentES.Click

        Response.Redirect("Form.aspx")

    End Sub

    Private Shared Function GetEmissionValue(ByVal emType As String, ByVal ay As String) As String

        Dim query As String

        If emType = "VOC" Then
            query = "Select dblVOCEmission FROM esSchema Where strAirsYear = @ay "
        Else
            query = "Select dblNOxEmission FROM esSchema Where strAirsYear = @ay "
        End If

        Dim param As New SqlParameter("@ay", ay)

        Return DB.GetString(query, param)

    End Function

End Class
