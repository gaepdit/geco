Imports System.Data.SqlClient
Imports System.DateTime

Partial Class es_default
    Inherits Page

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim CountyName As String
        Dim intESYear As Integer = Now.Year - 1

        Session("ESYear") = intESYear.ToString
        Session("esAirsNumber") = "0413" & GetCookie(Cookie.AirsNumber)
        Session("AirsYear") = Session("esAirsNumber") & Session("ESYear")

        CountyName = GetCounty(Session("esAirsNumber"))

        Session("LongMin") = GetLongMin(CountyName)
        Session("LongMax") = GetLongMax(CountyName)
        Session("LatMin") = GetLatMin(CountyName)
        Session("LatMax") = GetLatMax(CountyName)

        If Not IsPostBack Then

            HideSubmitHelp()
            HideFacilityHelp()
            HideContactHelp()
            HideEmissionsHelp()
            LoadESYears()
            ShowInitial()

        End If

    End Sub

    Private Sub LoadESYears()

        cboESYear.Items.Clear()
        cboESYear.Items.Add(" -Select Year- ")

        Dim query = "Select intESYear FROM esSchema where strAirsNumber = @AirsNumber order by intESYear Desc"
        Dim param As New SqlParameter("@AirsNumber", Session("esAirsNumber"))
        Dim dt = DB.GetDataTable(query, param)

        For Each dr As DataRow In dt.Rows
            cboESYear.Items.Add(dr.Item("intESYear"))
        Next

    End Sub

#Region " Help Panel Routines "

    Private Sub HideFacilityHelp()

        Dim FacilityHelp = CType(Master.FindControl("pnlFacilityHelp"), Panel)

        If FacilityHelp IsNot Nothing Then
            FacilityHelp.Visible = False
        End If

    End Sub

    Private Sub HideContactHelp()

        Dim ContactHelp = CType(Master.FindControl("pnlContactHelp"), Panel)

        If ContactHelp IsNot Nothing Then
            ContactHelp.Visible = False
        End If

    End Sub

    Private Sub HideEmissionsHelp()

        Dim EmissionsHelp = CType(Master.FindControl("pnlEmissionsHelp"), Panel)

        If EmissionsHelp IsNot Nothing Then
            EmissionsHelp.Visible = False
        End If

    End Sub

    Private Sub HideSubmitHelp()

        Dim SubmitHelp = CType(Master.FindControl("pnlSubmitHelp"), Panel)

        If SubmitHelp IsNot Nothing Then
            SubmitHelp.Visible = False
        End If

    End Sub

#End Region

    Protected Sub cboESYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboESYear.SelectedIndexChanged

        Dim YearSelected As Integer
        Dim CurrentYear As Integer = Now.Year - 1
        Dim esYear As Integer
        Dim AirsNumber As String = Session("esAirsNumber")
        Dim esStatus As String = Session("esState")
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
                End If
                If Left(esStatus, 10) = "Applicable" Then
                    btnCurrentES.Text = "Begin " & Now.Year - 1 & " ES"
                End If
            Else
                ShowPast()
                Session.Add("PastESYear", CStr(YearSelected))
                lblPastYear1.Text = YearSelected
                lblPastYear2.Text = YearSelected
                lblAIRSNo.Text = AirsNumber
                PastAirsYear = AirsNumber & CStr(YearSelected)
                lblFacilityName.Text = GetFacilityName(AirsNumber)
                NOxAmt = GetEmissionValue("NOx", PastAirsYear)
                VOCAmt = GetEmissionValue("VOC", PastAirsYear)
                If NOxAmt = "-1" Then NOxAmt = "0"
                If VOCAmt = "-1" Then VOCAmt = "0"
                lblNOx.Text = NOxAmt
                lblVOC.Text = VOCAmt

                If (NOxAmt = "0") And (VOCAmt = "0") Then
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

        pnlInitial.Visible = True
        pnlCurrentES.Visible = False
        pnlPastES.Visible = False
        cboESYear.SelectedIndex = 0

    End Sub

    Private Sub ShowCurrent()

        pnlInitial.Visible = False
        pnlCurrentES.Visible = True
        pnlPastES.Visible = False
        lblCurrentYear.Text = Now.Year - 1
        lblCurrentYear2.Text = Now.Year - 1

    End Sub

    Private Sub ShowPast()

        pnlInitial.Visible = False
        pnlCurrentES.Visible = False
        pnlPastES.Visible = True

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ShowInitial()

    End Sub

    Protected Sub btnCurrentES_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCurrentES.Click

        Response.Redirect("esform.aspx")

    End Sub

    Private Function GetEmissionValue(ByVal emType As String, ByVal ay As String) As String

        Dim query As String

        If emType = "VOC" Then
            query = "Select dblVOCEmission FROM esSchema Where strAirsYear = @ay "
        Else
            query = "Select dblNOxEmission FROM esSchema Where strAirsYear = @ay "
        End If

        Dim param As New SqlParameter("@ay", ay)

        Return DB.GetString(query, param)

    End Function

    Protected Sub btnCancelPast_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelPast.Click

        ShowInitial()

    End Sub

    Protected Sub btnPrintPastES_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrintPastES.Click

        Dim fname As String = lblFacilityName.Text
        Dim vocnum As String = lblVOC.Text
        Dim noxnum As String = lblNOx.Text
        Dim payr As String = Session("esAirsNumber") & Session("PastESYear")

        Session("fname") = fname
        Session("voc") = vocnum
        Session("nox") = noxnum
        Session("pastayr") = payr

        Response.Redirect("espast.aspx")

    End Sub

End Class