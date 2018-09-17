Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail

Partial Class AirsRequestAccess
    Inherits Page

    Private Property LookingUp As Boolean = False

    Private Enum LookupWhat
        Airs
        Facility
    End Enum

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        btnClose.Attributes.Add("onclick", "window.close();")

        If Not IsPostBack Then
            txtEmail.Text = GetCookie(GecoCookie.UserEmail)
            txtName.Text = GetCookie(GecoCookie.UserName)
        End If
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs)
        lblSuccess.Visible = False
        lblError.Visible = False

        If ltlMessage.Text Is Nothing OrElse ltlMessage.Text = "" Then
            lblError.Visible = True
            lblError.Text = "Error: No message sent. Select a facility first."
            Exit Sub
        End If

        Dim access As StringBuilder = New StringBuilder()

        Try
            For i = 0 To lstbAccess.Items.Count - 1
                If lstbAccess.Items(i).Selected Then
                    access.Append(lstbAccess.Items(i).Text & "; ")
                End If
            Next

            Dim access1 As String = Left(access.ToString(), Len(access.ToString()) - 2)

            If txtEmail.Text = "" Then
                lblError.Visible = True
                lblError.Text = "Please enter your registered email address."
                Exit Sub
            End If

            Dim ccEmails As String = GecoContactEmail

            Try
                If Not String.IsNullOrWhiteSpace(txtEmail.Text) Then
                    Dim originator As New MailAddress(txtEmail.Text)
                    ccEmails = String.Concat(originator.Address, ", ", GecoContactEmail)
                End If
            Catch ex As FormatException
                lblError.Visible = True
                lblError.Text = "Please enter a valid email address."
                Exit Sub
            End Try

            Dim query As String = "Select l.struseremail as email " &
                " FROM  OlapUserAccess a " &
                " INNER JOIN OlapUserLogin l " &
                " ON a.numuserid = l.numuserid " &
                " WHERE a.strairsnumber = @airs " &
                " AND a.intadminaccess = 1 "

            Dim param As New SqlParameter("@airs", "0413" & txtAirsNo.Text)

            Dim recipient As String = DB.GetString(query, param)

            If String.IsNullOrEmpty(recipient) Then
                'No Admin User
                recipient = GecoContactEmail
            End If

            Dim subject As String = "GECO - Request Facility Access"

            Dim htmlBody As String = ltlMessage.Text &
                "<p><b>Requested Access:</b> " & access1 & "</p>"

            If Not String.IsNullOrWhiteSpace(txtComments.Text) Then
                htmlBody &= "<p><b>Additional Comments:</b> <br />" & txtComments.Text & "</p>"
            End If

            If SendEmail(recipient, subject, Nothing, htmlBody, ccEmails) Then
                lblSuccess.Visible = True
                lblSuccess.Text = "Success! Your message has been sent."
                btnSend.Enabled = False
            Else
                lblError.Visible = True
                lblError.Text = "There was an error sending the email."
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnClose_Click(sender As Object, e As EventArgs)
        Response.Write("<script language='javascript'> { window.close();}</script>")
    End Sub

    Private Sub ShowEmailMessage(what As LookupWhat)
        LookingUp = True
        Try
            Dim queryformat As String = " [{0}] = '{1}'"
            Dim query As String = ""

            lblSuccess.Visible = False
            lblError.Visible = False
            btnSend.Enabled = True
            ltlMessage.Text = ""

            If what = LookupWhat.Airs Then
                txtFacility.Text = ""
                query = String.Format(queryformat, "strairsnumber", txtAirsNo.Text)
            Else
                txtAirsNo.Text = ""
                query = String.Format(queryformat, "facilityname", Replace(txtFacility.Text.ToUpper, "'", "''"))
            End If

            Dim dt As DataTable = GetCachedFacilityTable()

            If dt IsNot Nothing Then
                Dim rows As DataRow() = dt.Select(query)

                If rows IsNot Nothing AndAlso rows.Length > 0 Then
                    Dim dr As DataRow = rows(0)

                    If dr IsNot Nothing Then
                        If what = LookupWhat.Airs Then
                            txtFacility.Text = dr(1).ToString()
                        Else
                            txtAirsNo.Text = dr(0).ToString()
                        End If

                        ltlMessage.Text = "<p>Dear GECO Administrator, <br /><br />" &
                        "You are receiving this email because you are the currently assigned GECO Administrator for " &
                        "" & Server.HtmlEncode(txtFacility.Text) & " (AIRS Number: " & Server.HtmlEncode(txtAirsNo.Text) & "), and " &
                        "" & Server.HtmlEncode(txtName.Text) & " is requesting GECO access to the facility. <br /><br />" &
                        "To provide the user access to the facility in GECO, please follow the following instructions.<br /><br />" &
                        "- Sign in to your GECO Account at <a href='https://geco.gaepd.org'>geco.gaepd.org</a> <br />" &
                        "- At the User Home Page, select on the AIRS Number: " & Server.HtmlEncode(txtAirsNo.Text) & "<br />" &
                        "- At the Facility Home Page, go to the Admin/User Tools <br />" &
                        "- At the bottom,  add the requesting user's email address: " & Server.HtmlEncode(txtEmail.Text) & "<br />" &
                        "- Select Add New User - The email should now appear in the current users table. <br /><br />" &
                        "To adjust the users access to the GECO applications <br />" &
                        "- Select Edit next to the appropriate user email address. <br />" &
                        "- Check the various application boxes that the user needs access. <br />" &
                        "- Select Update to complete the process of adding application permissions. <br /><br />" &
                        "The requesting user will now have access to your facility the next time they sign into GECO. <br /><br />" &
                        "Thank you. <br /><br /></p>"
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
            If dt.Rows(i)("strairsnumber").ToString().ToUpper.StartsWith(prefixText.ToString.ToUpper) Then
                filteredList.Add(dt.Rows(i)("strairsnumber").ToString().ToUpper)
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
            If dt.Rows(i)("facilityname").ToString().ToUpper.StartsWith(prefixText.ToString.ToUpper) Then
                filteredList.Add(dt.Rows(i)("facilityname").ToString().ToUpper)
            End If
        Next

        Return filteredList.ToArray

    End Function

    Private Shared Function GetCachedFacilityTable() As DataTable
        Dim dt As DataTable

        If HttpContext.Current.Cache("RequestAccess") Is Nothing Then
            Dim query = "Select DISTINCT substring (strairsnumber, 5, LEN(strairsnumber)) as strairsnumber, " &
                " Upper(strfacilityname) as facilityname " &
                " FROM  APBFacilityInformation " &
                " order by strairsnumber"

            dt = DB.GetDataTable(query)
            dt.TableName = "facilityInfo"
            HttpContext.Current.Cache.Insert("RequestAccess", dt, Nothing)
        Else
            dt = DirectCast(HttpContext.Current.Cache("RequestAccess"), DataTable)
        End If

        Return dt
    End Function

End Class