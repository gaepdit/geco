Imports System.Net.Mail
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

        If Not IsPostBack Then
            If currentUser.ProfileUpdateRequired Then
                pUpdateRequired.Visible = True
                upRequestAccess.Visible = False
            End If

            txtEmail.Text = currentUser.Email
            txtName.Text = currentUser.FullName
        End If
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs)
        lblSuccess.Visible = False
        lblError.Visible = False

        If String.IsNullOrWhiteSpace(ltlMessage.Text) Then
            lblError.Visible = True
            lblError.Text = "Error: No message sent. Select a facility first."
            Exit Sub
        End If

        Dim access As New List(Of String)

        For i = 0 To lstbAccess.Items.Count - 1
            If lstbAccess.Items(i).Selected Then
                access.Add(lstbAccess.Items(i).Text)
            End If
        Next

        If txtEmail.Text = "" Then
            lblError.Visible = True
            lblError.Text = "Please enter your registered email address."
            Exit Sub
        End If

        Dim ccList As New List(Of String)

        Dim originator As MailAddress

        Try
            If Not String.IsNullOrWhiteSpace(txtEmail.Text) Then
                originator = New MailAddress(txtEmail.Text)
                ccList.Add(originator.Address)
            End If
        Catch ex As FormatException
            lblError.Visible = True
            lblError.Text = "Please enter a valid email address."
            Exit Sub
        End Try

        Dim recipientList As New List(Of String)

        Dim dt As DataTable = GetFacilityAdminUsers(New ApbFacilityId(txtAirsNo.Text))

        If dt IsNot Nothing Then
            For Each dr As DataRow In dt.Rows
                recipientList.Add(dr(0))
            Next
        End If

        If recipientList.Count = 0 Then
            'No Admin User in DB
            recipientList.Add(GecoContactEmail)
        Else
            ccList.Add(GecoContactEmail)
        End If

        Dim subject As String = "GECO - Facility Access Request"

        Dim htmlBody As String = ltlMessage.Text

        htmlBody &= "<p><b>Requested access:</b> " & ConcatNonEmptyStrings("; ", access) & "</p>"

        If Not String.IsNullOrWhiteSpace(txtComments.Text) Then
            htmlBody &= "<p><b>Additional comments from user:</b> <br />" & txtComments.Text & "</p>"
        End If

        If SendEmail(ConcatNonEmptyStrings(",", recipientList), subject, Nothing, htmlBody, ConcatNonEmptyStrings(",", ccList)) Then
            lblSuccess.Visible = True
            lblSuccess.Text = "Success! Your message has been sent."
            btnSend.Enabled = False
        Else
            lblError.Visible = True
            lblError.Text = "There was an error sending the email."
        End If


    End Sub

    Private Sub ShowEmailMessage(what As LookupWhat)
        LookingUp = True

        Try
            Dim queryformat As String = " [{0}] = '{1}'"
            Dim query As String = ""

            lblSuccess.Visible = False
            lblError.Visible = False
            btnSend.Enabled = False
            ltlMessage.Text = ""
            bqMessage.Visible = False

            If what = LookupWhat.Airs Then
                txtFacility.Text = ""
                query = String.Format(queryformat, "airsnumber", txtAirsNo.Text)
            Else
                txtAirsNo.Text = ""
                query = String.Format(queryformat, "facilityname", Replace(txtFacility.Text, "'", "''"))
            End If

            Dim dt As DataTable = GetCachedFacilityTable()

            If dt IsNot Nothing Then
                Dim rows As DataRow() = dt.Select(query)

                If rows IsNot Nothing AndAlso rows.Length > 0 Then
                    Dim dr As DataRow = rows(0)

                    If dr IsNot Nothing Then
                        txtFacility.Text = dr(1).ToString()
                        txtAirsNo.Text = dr(0).ToString()

                        ltlMessage.Text = "<p>Dear GECO Administrator, <br /><br />" &
                        "You are receiving this email because you are currently the assigned GECO Administrator for " &
                        "the following facility, and " & Server.HtmlEncode(txtName.Text) & " is requesting access: <br /><br />" &
                        Server.HtmlEncode(txtFacility.Text) & " <br />" &
                        "AIRS Number: " & Server.HtmlEncode(New ApbFacilityId(txtAirsNo.Text).FormattedString) & " <br /><br /> " &
                        "To provide access to the requested facility in GECO, please follow these instructions: <br /><br />" &
                        "- Sign into your GECO Account at: https://geco.gaepd.org<br />" &
                        "- On the Home page, select the requested facility. <br />" &
                        "- On the Facility Home Page, select User Access. <br />" &
                        "- Enter the requesting user's email address: " & Server.HtmlEncode(txtEmail.Text) & "<br />" &
                        "- Select Add New User. <br /><br />" &
                        "The email address should now appear in the list of current users. " &
                        "To adjust which GECO applications the user has access to: <br /><br />" &
                        "- Select Edit next to the email address. <br />" &
                        "- Select which applications the user can access by checking the appropriate boxes. <br />" &
                        "- Select Update to save the new application permissions. <br /><br />" &
                        "The user will have access to the facility the next time they sign into GECO. <br /><br />" &
                        "Thank you.</p>"

                        bqMessage.Visible = True

                        btnSend.Enabled = True
                    End If
                End If
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

        LookingUp = False
    End Sub

    Protected Sub txtAirsNo_TextChanged(sender As Object, e As EventArgs) Handles txtAirsNo.TextChanged
        If Not LookingUp Then
            ShowEmailMessage(LookupWhat.Airs)
        End If
    End Sub

    Protected Sub txtFacility_TextChanged(sender As Object, e As EventArgs) Handles txtFacility.TextChanged
        If Not LookingUp Then
            ShowEmailMessage(LookupWhat.Facility)
        End If
    End Sub

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