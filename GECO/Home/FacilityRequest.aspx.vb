Imports GECO.GecoModels

Partial Class Home_FacilityRequest
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property LookingUp As Boolean = False

    Private Enum LookupWhat
        Airs
        Facility
    End Enum

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        currentUser = GetCurrentUser()

        If Not IsPostBack AndAlso currentUser.ProfileUpdateRequired Then
            pUpdateRequired.Visible = True
            pnlRequestAccess.Visible = False
        End If
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        lblSuccess.Visible = False
        lblError.Visible = False

        If String.IsNullOrWhiteSpace(ltlMessage.Text) Then
            lblError.Visible = True
            lblError.Text = "Error: No message sent. Select a facility first."
            Return
        End If

        Dim ccList As New List(Of String) From {currentUser.Email}
        Dim recipientList As New List(Of String)

        If Not chkAssistanceNeeded.Checked Then
            Dim dt As DataTable = GetFacilityAdminUsers(New ApbFacilityId(txtAirsNo.Text))

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows
                    recipientList.Add(dr(0).ToString)
                Next
            End If
        End If

        If recipientList.Count = 0 Then
            ' No Admin User in DB or assistance needed
            recipientList.Add(GecoContactEmail)
        Else
            ccList.Add(GecoContactEmail)
        End If

        Dim subject As String = "GECO Facility Access Request"

        Dim htmlBody As String = ltlMessage.Text & ltlMessagePart2.Text & ltlMessagePart3.Text

        If Not String.IsNullOrWhiteSpace(txtComments.Text) Then
            htmlBody &= "<p><b>Additional comments from the requesting user:</b></p>" &
                "<blockquote>" & Server.HtmlEncode(txtComments.Text) & "</blockquote>"
        End If

        If SendEmail(ConcatNonEmptyStrings(",", recipientList), subject, Nothing, htmlBody, ConcatNonEmptyStrings(",", ccList),
                     caller:="Home_FacilityRequest.btnSend_Click") Then
            lblSuccess.Visible = True
            lblApbInstructions.Visible = False
            lblAdminInstructions.Visible = False
            btnSend.Enabled = False
        Else
            lblError.Visible = True
            lblApbInstructions.Visible = False
            lblAdminInstructions.Visible = False
        End If

    End Sub

    Private Sub LookUpFacility(what As LookupWhat)
        LookingUp = True

        HideNextSteps()

        Try
            Dim facilityTable As DataTable = GetCachedFacilityTable()
            If facilityTable Is Nothing Then Return

            Dim query As String

            If what = LookupWhat.Airs Then
                txtFacility.Text = ""
                query = $"[airsnumber] = '{txtAirsNo.Text}'"
            Else
                txtAirsNo.Text = ""
                query = $"[facilityname] = '{txtFacility.Text.Replace("'", "''")}'"
            End If

            Dim rows As DataRow() = facilityTable.Select(query)
            If rows Is Nothing OrElse rows.Length = 0 Then Return

            Dim dr As DataRow = rows(0)
            If dr Is Nothing Then Return

            txtAirsNo.Text = dr(0).ToString()
            txtFacility.Text = dr(1).ToString()

            Dim hasAdminUsers As Boolean = DisplayNextSteps()
            ComposeEmailMessage(hasAdminUsers)

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            LookingUp = False
        End Try
    End Sub

    Private Sub HideNextSteps()
        lblSuccess.Visible = False
        lblError.Visible = False
        btnSend.Enabled = False
        ltlMessage.Text = ""
        bqMessage.Visible = False
        lblSuccess.Visible = False
        lblError.Visible = False
        pnlHasAdmin.Visible = False
        lblAdminInstructions.Visible = False
        pNoAdmin.Visible = False
        pContactEpdWarning.Visible = False
        lblApbInstructions.Visible = False
        pnlNextSteps.Visible = False
        chkAssistanceNeeded.Checked = False
    End Sub

    Private Function DisplayNextSteps() As Boolean
        pnlNextSteps.Visible = True

        Dim dt As DataTable = GetFacilityAdminUsers(New ApbFacilityId(txtAirsNo.Text))

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            pNoAdmin.Visible = True
            pContactEpdWarning.Visible = True
            lblAdminInstructions.Visible = False
            lblApbInstructions.Visible = True

            Return False
        End If

        lstAdminUsers.DataSource = dt
        lstAdminUsers.DataTextField = "Name"
        lstAdminUsers.DataBind()
        pnlHasAdmin.Visible = True
        lblAdminInstructions.Visible = True
        lblApbInstructions.Visible = False

        Return True
    End Function

    Private Sub ComposeEmailMessage(hasAdminUsers As Boolean)
        If String.IsNullOrEmpty(txtAirsNo.Text) OrElse String.IsNullOrEmpty(txtFacility.Text) OrElse
          Not ApbFacilityId.IsValidAirsNumberFormat(txtAirsNo.Text) Then
            HideNextSteps()
            Return
        End If

        lblSuccess.Visible = False
        lblError.Visible = False

        Dim message As New StringBuilder()

        If hasAdminUsers Then
            message.AppendLine("<p>Dear Facility Administrator,</p>")
            message.AppendLine(
                "<p>You are receiving this email because you are currently an assigned administrator for " &
                "the following facility in GECO.</p>")
        Else
            message.AppendLine("<p>Dear GECO Administrator,</p>")
            message.AppendLine("<p>A user is requesting assistance from the Air Protection Branch related to the following facility access request.</p>")
        End If

        ltlMessage.Text = message.ToString()

        ComposeEmailMessagePart2()

        Dim message3 As New StringBuilder()

        If hasAdminUsers Then
            message3.AppendLine(
                "<p>To provide access to the requested facility in GECO, please follow these instructions:</p>" &
                "<p>- Sign into your GECO Account at: https://geco.gaepd.org <br />" &
                "- On the Home page, select the requested facility.<br />" &
                "- On the Facility Home page, select User Access.<br />" &
                "- Enter the requesting user's email address: <b>" & Server.HtmlEncode(currentUser.Email) & "</b><br />" &
                "- Select ""Add New User"".</p>" &
                "<p>The email address should now appear in the list of current users. " &
                "To adjust which GECO applications the user has access to:</p>" &
                "- Select ""Edit"" next to the user.<br />" &
                "- Select which applications the user can access by checking the appropriate boxes.<br />" &
                "- Select ""Update"" to save the new application permissions.</p>" &
                "<p>The user will have access to the facility the next time they sign into GECO. Thank you.</p>")
        Else
            message3.AppendLine("<p>Thank you.</p>")
        End If

        ltlMessagePart3.Text = message3.ToString()

        bqMessage.Visible = True
        btnSend.Enabled = True
    End Sub

    Private Sub ComposeEmailMessagePart2()
        Dim message2 As New StringBuilder()

        message2.AppendLine(
            "<p><b>Facility:</b> " & Server.HtmlEncode(txtFacility.Text) & "<br />" &
            "<b>AIRS Number:</b> " & Server.HtmlEncode(New ApbFacilityId(txtAirsNo.Text).FormattedString) & "</p>")
        message2.AppendLine("<p>GECO user <b>" & Server.HtmlEncode(currentUser.FullName) & "</b> " &
                            "&lt;" & Server.HtmlEncode(currentUser.Email) & "&gt; is requesting the following:")

        For Each item As ListItem In lstbAccess.Items
            If item.Selected Then message2.AppendLine("<br />- " & item.Text)
        Next

        message2.AppendLine("</p>")

        ltlMessagePart2.Text = message2.ToString()
    End Sub

    Private Sub chkAssistanceNeeded_CheckedChanged(sender As Object, e As EventArgs) Handles chkAssistanceNeeded.CheckedChanged
        If chkAssistanceNeeded.Checked Then
            lblAdminInstructions.Visible = False
            lblApbInstructions.Visible = True
            pContactEpdWarning.Visible = True
            ComposeEmailMessage(False)
        Else
            lblAdminInstructions.Visible = True
            lblApbInstructions.Visible = False
            pContactEpdWarning.Visible = False
            ComposeEmailMessage(True)
        End If
    End Sub

    Protected Sub txtAirsNo_TextChanged(sender As Object, e As EventArgs) Handles txtAirsNo.TextChanged
        If Not LookingUp Then
            LookUpFacility(LookupWhat.Airs)
        End If
    End Sub

    Protected Sub txtFacility_TextChanged(sender As Object, e As EventArgs) Handles txtFacility.TextChanged
        If Not LookingUp Then
            LookUpFacility(LookupWhat.Facility)
        End If
    End Sub

    Protected Sub lstbAccess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstbAccess.SelectedIndexChanged
        If pnlNextSteps.Visible Then ComposeEmailMessagePart2()
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="count")>
    <CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", MessageId:="count")>
    <Services.WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Shared Function AutoCompleteAirs(prefixText As String, count As Integer) As String()
        Dim dt As DataTable = GetCachedFacilityTable()
        Dim filteredList As New List(Of String)

        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("airsnumber").ToString().StartsWith(prefixText) Then
                filteredList.Add(dt.Rows(i)("airsnumber").ToString())
            End If
        Next

        Return filteredList.ToArray
    End Function

    <CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="count")>
    <CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", MessageId:="count")>
    <Services.WebMethod()>
    <Script.Services.ScriptMethod()>
    Public Shared Function AutoCompleteFacility(prefixText As String, count As Integer) As String()
        Dim dt As DataTable = GetCachedFacilityTable()
        Dim filteredList As New List(Of String)

        For i As Integer = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("facilityname").ToString().StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) Then
                filteredList.Add(dt.Rows(i)("facilityname").ToString())
            End If
        Next

        Return filteredList.ToArray
    End Function

End Class
