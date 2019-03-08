Imports System.Data.SqlClient
Imports System.DateTime

Partial Class es_confirm
    Inherits Page

    Public ConfNum As String

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim Source As String = Session("ConfirmOption")
            Dim ESExist As Boolean
            Dim AirsYear As String = Session("AirsYear")

            If Session("ESOptOut") = "YES" Then
                pnlTop.Visible = True
                pnlOptedOutYes.Visible = True
                pnlConfFinal.Visible = False
                GetConfirmNumber()
                lblConfNum1.Text = ConfNum
                lblESYear1.Text = Session("ESYear")
                lblDate1.Text = Now.ToString("d-MMM-yyyy")
                lblAirsNo1.Text = Session("esAirsNumber")
                lblFacility1.Text = GetFacilityName(Session("esAirsNumber"))
            End If

            If Session("ESOptOut") = "NO" Then
                pnlTop.Visible = True
                pnlOptedOutYes.Visible = False
                pnlConfFinal.Visible = True
                lblESYear3.Text = Session("ESYear")
                ESExist = CheckESExist(AirsYear)

                If ESExist = False Then
                    CreateConfNum()
                End If

                GetConfirmNumber()
                lblVOCAmt2.Text = GetEmissionValue("VOC")
                lblNOXAmt2.Text = GetEmissionValue("NOX")
                lblConfNumFinalize.Text = ConfNum
                lblDate2.Text = Now.ToString("d-MMM-yyyy")
                lblAirsNo2.Text = Session("esAirsNumber")
                lblFacility2.Text = GetFacilityName(Session("esAirsNumber"))
            End If

            ShowSubmitHelp()
            HideFacilityHelp()
            HideContactHelp()
            HideEmissionsHelp()

        End If

    End Sub

#Region " Help Panel Routines "

    Private Sub HideFacilityHelp()

        Dim FacilityHelp = CType(Master.FindControl("pnlFacilityHelp"), Panel)

        If Not FacilityHelp Is Nothing Then
            FacilityHelp.Visible = False
        End If

    End Sub

    Private Sub HideContactHelp()

        Dim ContactHelp = CType(Master.FindControl("pnlContactHelp"), Panel)

        If Not ContactHelp Is Nothing Then
            ContactHelp.Visible = False
        End If

    End Sub

    Private Sub HideEmissionsHelp()

        Dim EmissionsHelp = CType(Master.FindControl("pnlEmissionsHelp"), Panel)

        If Not EmissionsHelp Is Nothing Then
            EmissionsHelp.Visible = False
        End If

    End Sub

    Private Sub ShowSubmitHelp()

        Dim SubmitHelp = CType(Master.FindControl("pnlSubmitHelp"), Panel)

        If Not SubmitHelp Is Nothing Then
            SubmitHelp.Visible = True
        End If

    End Sub

    Private Sub HideSubmitHelp()

        Dim SubmitHelp = CType(Master.FindControl("pnlSubmitHelp"), Panel)

        If Not SubmitHelp Is Nothing Then
            SubmitHelp.Visible = False
        End If

    End Sub

#End Region

#Region " Confirmation Number Routines "

    Private Sub CreateConfNum()

        Dim esYear As String = Session("ESYear")
        Dim AirsNumber As String = Session("esAirsNumber")
        Dim day As String = Now.ToString("d-MMM-yyyy")
        Dim hr As String = Now.Hour
        Dim min As String = Now.Minute
        Dim sec As String = Now.Second
        If Len(hr) < 2 Then hr = "0" & hr
        If Len(min) < 2 Then min = "0" & min
        If Len(sec) < 2 Then sec = "0" & sec
        Dim TransDate As String = day.ToUpper
        Dim TransTime As String = hr & min
        Dim ConfNum As String

        TransDate = TransDate.Replace("-", "")
        ConfNum = AirsNumber & TransDate & TransTime

        Dim query = "Update esSchema Set strConfirmationNbr = @ConfNum Where intESYear = @esYear And strAirsNumber = @AirsNumber "

        Dim params As SqlParameter() = {
            New SqlParameter("@ConfNum", ConfNum),
            New SqlParameter("@esYear", esYear),
            New SqlParameter("@AirsNumber", AirsNumber)
        }

        DB.RunCommand(query, params)

    End Sub

    Private Sub GetConfirmNumber()

        Dim AirsYear As String = Session("AirsYear")

        Dim query = "Select strConfirmationNbr FROM esSchema Where strAirsYear = @AirsYear "
        Dim param As New SqlParameter("@AirsYear", AirsYear)

        ConfNum = DB.GetString(query, param)

    End Sub

#End Region

#Region " Button Routines "

    Protected Sub btnOptOutChange1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOptOutChange1.Click

        Session("eschanges") = "TRUE"

        Response.Redirect("esform.aspx")

    End Sub

    Protected Sub btnConfFinal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfFinal.Click

        Session("eschanges") = "TRUE"

        Response.Redirect("esform.aspx")

    End Sub

#End Region

    Private Function GetEmissionValue(ByVal emType As String) As Double

        Dim query As String

        If emType = "VOC" Then
            query = "Select dblVOCEmission FROM esSchema Where strAirsYear = @AirsYear "
        Else
            query = "Select dblNOxEmission FROM esSchema Where strAirsYear = @AirsYear "
        End If

        Dim param As New SqlParameter("@AirsYear", Session("AirsYear"))

        Return DB.GetString(query, param)

    End Function

End Class