Imports System.Data.SqlClient

Partial Class ES_espast
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            lblPastESYear.Text = Session("PastESYear")
            lblESPastYear2.Text = Session("PastESYear")
            lblConfNo.Text = GetConfirmNumber(Session("pastayr"))
            lblFacilityName.Text = Session("fname")
            lblAIRSNo.Text = Session("esAirsNumber")
            lblNOx.Text = Session("nox")
            lblVOC.Text = Session("voc")

            If (lblNOx.Text = "0") And (lblVOC.Text = "0") Then
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

    Protected Sub btnEiHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEiHome.Click

        Response.Redirect("default.aspx")

    End Sub

End Class