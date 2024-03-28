Imports GECO.EmailTemplates
Imports GECO.GecoModels

Partial Class Account_Password
    Inherits Page

    Private Property currentUser As GecoUser

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck("Account/Password.aspx")

        currentUser = GetCurrentUser()

        Title = "GECO Account - " & currentUser.FullName

        If Not IsPostBack Then
            LoadProfile()
        End If
    End Sub

    Protected Sub LoadProfile()
        lblDisplayName.Text = currentUser.FullName & ", " & currentUser.Email
    End Sub

    Private Sub HideMessages()
        lblPasswordMessage.Visible = False
    End Sub

    Protected Sub btnPwdUpdate_Click(sender As Object, e As EventArgs) Handles btnPwdUpdate.Click
        HideMessages()

        If IsValid Then
            Dim result As UpdatePasswordResult = UpdatePassword(currentUser.Email, txtOldPassword.Text, txtNewPassword.Text)

            Select Case result
                Case UpdatePasswordResult.Success
                    SendPasswordChangeNotification(currentUser.Email)
                    lblPasswordMessage.Text = "Password successfully updated."

                Case UpdatePasswordResult.InvalidPassword
                    lblPasswordMessage.Text = "The old password is incorrect. The password has not been changed."

                Case Else
                    lblPasswordMessage.Text = "An error occurred. The password has not been changed."

            End Select

            lblPasswordMessage.Visible = True
        End If
    End Sub

    Protected Sub passwordRequirement_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles passwordRequirements.ServerValidate
        If checkPasswordValid() Then
            passwordRequirements.ErrorMessage = "The password cannot contain segments of the URL, app name, or email."
            args.IsValid = False
        End If
    End Sub

    ''' <summary>
    ''' Helper function for the passwordRequirement_ServerValidate()
    ''' </summary>
    ''' <returns>True if the password Is valid, false otherwise.</returns>
    Private Function checkPasswordValid() As Boolean
        Dim email As String = lblDisplayName.Text.ToLower
        Dim password As String = txtNewPassword.Text.ToLower

        ' check if these passwords matches the email Or website
        Dim validPassEmail As Integer = FindIntersection(email, password)
        Dim validPassWebsite As Integer = FindIntersection("geco", password)
        Dim validPassDepartment As Integer = FindIntersection("gaepd", password)

        ' declare an arbitrary length
        Dim maxSequenceLength As Integer = 3
        Return validPassEmail <= maxSequenceLength AndAlso
                validPassWebsite <= maxSequenceLength AndAlso
                validPassDepartment <= maxSequenceLength
    End Function

    ''' <summary>
    ''' Find where the sequence starts And its length between 2 strings
    ''' </summary>
    ''' <param name="a">First string</param>
    ''' <param name="b">Second string</param>
    ''' <returns>0 if there are no sequence, else return the length</returns>
    Private Function FindIntersection(a As String, b As String) As Integer
        Dim bestResult As Integer = 0
        For i As Integer = 0 To a.Length - 2
            Dim result As Integer = FindIntersectionFromStart(a.Substring(i), b)
            If result <> 0 Then
                If bestResult = 0 Then
                    bestResult = result
                Else
                    If result > bestResult Then
                        bestResult = result
                    End If
                End If
            End If
            If bestResult >= a.Length - i Then
                Exit For
            End If
        Next
        Return bestResult
    End Function

    ''' <summary>
    ''' Helper method for FindIntersection()
    ''' </summary>
    ''' <param name="a">First string</param>
    ''' <param name="b">Second string</param>
    ''' <returns>0 if there are no sequence, else return the length</returns>
    Private Function FindIntersectionFromStart(a As String, b As String) As Integer
        For i As Integer = a.Length To 1 Step -1
            Dim d As String = a.Substring(0, i)
            Dim j As Integer = b.IndexOf(d)
            If j >= 0 Then
                Return i ' return the length
            End If
        Next
        Return 0
    End Function

End Class
