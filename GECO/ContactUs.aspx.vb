Imports System.Data.SqlClient
Imports GECO.GecoModels

Partial Class ContactUs
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim user As GecoUser = GetCurrentUser()

            If user IsNot Nothing Then
                txtEmail.Text = user.Email
                txtName.Text = user.FullName
            End If
        End If
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs)
        lblError.Visible = False
        lblSuccess.Visible = False

        Dim query As String = "Insert into OlapContactUs " &
            "(strIPAddress, strEmail, strName, DateTimeStamp, strMessage, strSubject) " &
            "values " &
            "(@strIPAddress, @strEmail, @strName, getdate(), @strMessage, @strSubject) "

        Dim params As SqlParameter() = {
            New SqlParameter("@strIPAddress", Request.ServerVariables("REMOTE_ADDR")),
            New SqlParameter("@strEmail", txtEmail.Text),
            New SqlParameter("@strName", txtName.Text),
            New SqlParameter("@strMessage", txtMessage.Text),
            New SqlParameter("@strSubject", ddlSubject.Text)
        }

        Try
            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex, False)
        End Try

        Dim Subject As String = "GECO Contact Form - " & ddlSubject.Text

        Dim Body As String = "From: " & txtName.Text & " (" & txtEmail.Text & ") " &
            vbNewLine & vbNewLine &
            txtMessage.Text

        If SendEmail(GecoContactEmail, Subject, Body) Then
            lblSuccess.Visible = True
        Else
            lblError.Visible = True
        End If

        txtName.Enabled = False
        txtEmail.Enabled = False
        ddlSubject.Enabled = False
        txtMessage.Enabled = False
        btnSend.Enabled = False
    End Sub

End Class