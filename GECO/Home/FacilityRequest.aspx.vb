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

        Dim dt As DataTable = GetFacilityAdminUsers(New ApbFacilityId(txtAirsNo.Text))

        If dt IsNot Nothing Then
            For Each dr As DataRow In dt.Rows
                recipientList.Add(dr(0).ToString)
            Next
        End If

        If recipientList.Count = 0 Then
            'No Admin User in DB
            recipientList.Add(GecoContactEmail)
        Else
            ccList.Add(GecoContactEmail)
        End If

        Dim subject As String = "GECO Facility Access Request"

        Dim htmlBody As String = ltlMessage.Text

        If Not String.IsNullOrWhiteSpace(txtComments.Text) Then
            htmlBody &= "<p><b>Additional comments from the requesting user:</b></p>" &
                "<blockquote>" & Server.HtmlEncode(txtComments.Text) & "</blockquote>"
        End If

        If SendEmail(ConcatNonEmptyStrings(",", recipientList), subject, Nothing, htmlBody, ConcatNonEmptyStrings(",", ccList),
                     caller:="Home_FacilityRequest.btnSend_Click") Then
            lblSuccess.Visible = True
            lblSuccess.Text = "Success! Your message has been sent."
            btnSend.Enabled = False
        Else
            lblError.Visible = True
            lblError.Text = "There was an error sending the email."
        End If

    End Sub

    Private Sub LookUpFacility(what As LookupWhat)
        LookingUp = True

        HideMessage()

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

            ComposeEmailMessage()

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            LookingUp = False
        End Try
    End Sub

    Private Sub HideMessage()
        lblSuccess.Visible = False
        lblError.Visible = False
        btnSend.Enabled = False
        ltlMessage.Text = ""
        bqMessage.Visible = False
        lblSuccess.Visible = False
        lblError.Visible = False
    End Sub

    Private Sub ComposeEmailMessage()
        If String.IsNullOrEmpty(txtAirsNo.Text) OrElse String.IsNullOrEmpty(txtFacility.Text) OrElse
          Not ApbFacilityId.IsValidAirsNumberFormat(txtAirsNo.Text) Then
            HideMessage()
            Return
        End If

        lblSuccess.Visible = False
        lblError.Visible = False

        Dim adminUsersTable As DataTable = GetFacilityAdminUsers(New ApbFacilityId(txtAirsNo.Text))
        Dim hasAdminUsers As Boolean = adminUsersTable IsNot Nothing AndAlso adminUsersTable.Rows.Count > 0

        Dim message As New StringBuilder()

        If hasAdminUsers Then
            lblMessageLabel.Text = "This message will be sent to the facility administrator(s):"
            message.AppendLine("<p>Dear Facility Administrator,</p>")
            message.AppendLine(
                    "<p>You are receiving this email because you are currently an assigned administrator for " &
                    "the following facility in GECO.</p>")
        Else
            lblMessageLabel.Text = "This message will be sent to the EPD GECO administrator:"
            message.AppendLine("<p>Dear GECO Administrator,</p>")
            message.AppendLine("<p>The following facility does not have an assigned administrator in GECO.</p>")
        End If

        message.AppendLine(
                "<p><b>Facility:</b> " & Server.HtmlEncode(txtFacility.Text) & "<br />" &
                "<b>AIRS Number:</b> " & Server.HtmlEncode(New ApbFacilityId(txtAirsNo.Text).FormattedString) & "</p>")
        message.AppendLine("<p>GECO user <b>" & Server.HtmlEncode(currentUser.FullName) & "</b> is requesting the following:")

        For Each item As ListItem In lstbAccess.Items
            If item.Selected Then message.AppendLine("<br />- " & item.Text)
        Next

        message.AppendLine(
                "</p>" &
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

        ltlMessage.Text = message.ToString()
        bqMessage.Visible = True
        btnSend.Enabled = True

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

    Protected Sub txtComments_TextChanged(sender As Object, e As EventArgs) Handles txtComments.TextChanged
        ComposeEmailMessage()
    End Sub

    Protected Sub lstbAccess_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstbAccess.SelectedIndexChanged
        ComposeEmailMessage()
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
