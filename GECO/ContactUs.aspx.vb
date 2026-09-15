Imports GECO.GecoModels
Imports System.Threading.Tasks

Partial Class ContactUs
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim gecoUser As GecoUser = GetCurrentUser()

            If gecoUser IsNot Nothing Then
                txtEmail.Text = gecoUser.Email
                txtName.Text = gecoUser.FullName
            End If
        End If
    End Sub

    Protected Async Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        lblError.Visible = False
        lblSuccess.Visible = False

        Dim Subject As String = "GECO Contact Form - " & ddlSubject.Text

        Dim Body As String = "From: " & txtName.Text & " (" & txtEmail.Text & ") " &
            NewLine & NewLine &
            txtMessage.Text

        If Await SendEmailAsync(GecoContactEmail, Subject, Body, caller:="ContactUs.btnSend_Click") Then
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
