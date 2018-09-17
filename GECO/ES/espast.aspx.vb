Imports System.Data
Imports System.Data.SqlClient

Partial Class ES_espast
    Inherits Page

    Public conn, conn1, connsub As New SqlConnection(oradb)
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

        Dim SQL As String
        Dim ConfNum As String

        SQL = "Select strConfirmationNbr FROM esSchema Where strAirsYear = '" & ay & "' "

        Dim cmd As New SqlCommand(SQL, conn)
        If conn.State = ConnectionState.Closed Then
            conn.Open()
        End If

        Dim dr As SqlDataReader = cmd.ExecuteReader

        dr.Read()
        If IsDBNull(dr("strConfirmationNbr")) Then
            ConfNum = "No Data"
        Else
            ConfNum = dr.Item("strConfirmationNbr")
        End If

        If conn.State = ConnectionState.Open Then
            conn.Close()
        End If

        Return ConfNum

    End Function

    Protected Sub btnEiHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEiHome.Click

        Response.Redirect("default.aspx")

    End Sub

End Class