Imports GECO.GecoModels

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

    Protected Sub btnSend_Click(sender As Object, e As EventArgs)
        lblError.Visible = False
        lblSuccess.Visible = False

        Dim Subject As String = "GECO Contact Form - " & ddlSubject.Text

        Dim Body As String = "From: " & txtName.Text & " (" & txtEmail.Text & ") " &
            vbNewLine & vbNewLine &
            txtMessage.Text

        If SendEmail(GecoContactEmail, Subject, Body, caller:="ContactUs.btnSend_Click") Then
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
