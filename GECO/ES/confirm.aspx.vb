Imports System.Data.SqlClient
Imports System.DateTime
Imports GECO.GecoModels

Partial Class es_confirm
    Inherits Page

    Private ConfNum As String
    Private Property CurrentAirs As ApbFacilityId

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim airs As String = GetSessionItem(Of String)("esAirsNumber")

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)

        If Not IsPostBack Then

            Dim ESExist As Boolean
            Dim AirsYear As String = GetSessionItem(Of String)("AirsYear")

            If GetSessionItem(Of String)("ESOptOut") = "YES" Then
                pnlTop.Visible = True
                pnlOptedOutYes.Visible = True
                pnlConfFinal.Visible = False
                GetConfirmNumber()
                lblConfNum1.Text = ConfNum
                lblESYear1.Text = GetSessionItem(Of String)("ESYear")
                lblDate1.Text = Now.ToString("d-MMM-yyyy")
                lblAirsNo1.Text = CurrentAirs.FormattedString
                lblFacility1.Text = GetFacilityName(CurrentAirs)
            End If

            If GetSessionItem(Of String)("ESOptOut") = "NO" Then
                pnlTop.Visible = True
                pnlOptedOutYes.Visible = False
                pnlConfFinal.Visible = True
                lblESYear3.Text = GetSessionItem(Of String)("ESYear")
                ESExist = CheckESExist(AirsYear)

                If Not ESExist Then
                    CreateConfNum()
                End If

                GetConfirmNumber()
                lblVOCAmt2.Text = GetEmissionValue("VOC").ToString
                lblNOXAmt2.Text = GetEmissionValue("NOX").ToString
                lblConfNumFinalize.Text = ConfNum
                lblDate2.Text = Now.ToString("d-MMM-yyyy")
                lblAirsNo2.Text = CurrentAirs.FormattedString
                lblFacility2.Text = GetFacilityName(CurrentAirs)
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

    Private Sub ShowSubmitHelp()

        Dim SubmitHelp = CType(Master.FindControl("pnlSubmitHelp"), Panel)

        If SubmitHelp IsNot Nothing Then
            SubmitHelp.Visible = True
        End If

    End Sub

#End Region

#Region " Confirmation Number Routines "

    Private Sub CreateConfNum()

        Dim esYear As String = GetSessionItem(Of String)("ESYear")
        Dim day As String = Now.ToString("d-MMM-yyyy")
        Dim hr As String = Now.Hour.ToString
        Dim min As String = Now.Minute.ToString
        If hr.Length() < 2 Then hr = "0" & hr
        If min.Length() < 2 Then min = "0" & min
        Dim TransDate As String = day.ToUpper
        Dim TransTime As String = hr & min
        Dim newConfNum As String

        TransDate = TransDate.Replace("-", "")
        newConfNum = CurrentAirs.ShortString & TransDate & TransTime

        Dim query = "Update esSchema Set strConfirmationNbr = @ConfNum Where intESYear = @esYear And strAirsNumber = @AirsNumber "

        Dim params As SqlParameter() = {
            New SqlParameter("@ConfNum", newConfNum),
            New SqlParameter("@esYear", esYear),
            New SqlParameter("@AirsNumber", CurrentAirs.DbFormattedString)
        }

        DB.RunCommand(query, params)

    End Sub

    Private Sub GetConfirmNumber()

        Dim AirsYear As String = GetSessionItem(Of String)("AirsYear")

        Dim query = "Select strConfirmationNbr FROM esSchema Where strAirsYear = @AirsYear "
        Dim param As New SqlParameter("@AirsYear", AirsYear)

        ConfNum = DB.GetString(query, param)

    End Sub

#End Region

#Region " Button Routines "

    Protected Sub btnOptOutChange1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOptOutChange1.Click, btnConfFinal.Click

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

        Dim param As New SqlParameter("@AirsYear", GetSessionItem(Of String)("AirsYear"))

        Return DB.GetSingleValue(Of Double)(query, param)

    End Function

End Class
