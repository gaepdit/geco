Imports System.Data.SqlClient

Partial Class ES_espast
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            lblPastESYear.Text = GetSessionItem(Of String)("PastESYear")
            lblESPastYear2.Text = GetSessionItem(Of String)("PastESYear")
            lblConfNo.Text = GetConfirmNumber(GetSessionItem(Of String)("pastayr"))
            lblFacilityName.Text = GetSessionItem(Of String)("fname")
            lblAIRSNo.Text = GetSessionItem(Of String)("esAirsNumber")
            lblNOx.Text = GetSessionItem(Of String)("nox")
            lblVOC.Text = GetSessionItem(Of String)("voc")

            If (lblNOx.Text = "0") AndAlso (lblVOC.Text = "0") Then
                ShowOptedOut()
            Else
                ShowOptedIn()
            End If

        End If
    End Sub

    Private Sub ShowOptedOut()

        pnlOptedOut.Visible = True
        pnlOptedIn.Visible = False

    End Sub

    Private Sub ShowOptedIn()

        pnlOptedOut.Visible = False
        pnlOptedIn.Visible = True

    End Sub

    Private Function GetConfirmNumber(ByVal ay As String) As String

        Dim query As String = "Select strConfirmationNbr FROM esSchema Where strAirsYear = @ay "
        Dim ConfNum As String = DB.GetString(query, New SqlParameter("@ay", ay))
        Return If(String.IsNullOrEmpty(ConfNum), "No Data", ConfNum)

    End Function

    Protected Sub btnEiHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEiHome.Click

        Response.Redirect("default.aspx")

    End Sub

End Class
