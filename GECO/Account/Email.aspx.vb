Imports GECO.EmailTemplates
Imports GECO.GecoModels

Partial Class Account_Email
    Inherits Page

    Private Property currentUser As GecoUser

    Private IsTerminating As Boolean = False
    Protected Overrides Sub OnLoad(e As EventArgs)
        IsTerminating = MainLoginCheck()
        If IsTerminating Then Return
        MyBase.OnLoad(e)
    End Sub
    Protected Overrides Sub Render(writer As HtmlTextWriter)
        If IsTerminating Then Return
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        currentUser = GetCurrentUser()

        Title = "GECO Account - " & currentUser.FullName

        If Not IsPostBack Then LoadProfile()
    End Sub

    Protected Sub LoadProfile()
        lblDisplayName.Text = currentUser.FullName & ", " & currentUser.Email

        txtEmail.Text = currentUser.Email
    End Sub

    Private Sub HideMessages()
        lblEmailMessage.Visible = False
    End Sub

    Private Sub btnSaveEmail_Click(sender As Object, e As EventArgs) Handles btnSaveEmail.Click
        If currentUser Is Nothing Then
            CompleteRedirect("~/", IsTerminating)
            Return
        End If

        HideMessages()

        If Not IsValid Then Return

        Dim token As String = Nothing
        Dim result As UpdateUserEmailResult = UpdateUserEmail(currentUser.Email, txtEmail.Text, token)

        Select Case result
            Case UpdateUserEmailResult.Success
                SendConfirmEmailUpdateEmail(txtEmail.Text, token)
                lblEmailMessage.Text = "An email has been sent to the address you provided with an activation link to confirm your new address."

            Case UpdateUserEmailResult.NewEmailExists
                lblEmailMessage.Text = "An account already exists for that email address."

            Case Else
                lblEmailMessage.Text = "An error occurred. The email address has not been changed."

        End Select

        lblEmailMessage.Visible = True
    End Sub

End Class
