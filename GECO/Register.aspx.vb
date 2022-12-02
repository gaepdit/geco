Imports System.Reflection
Imports GECO.EmailTemplates

Partial Class Register
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If UserIsLoggedIn() Then
                Response.Redirect("~/Home/")
            End If

            Session.Clear()
            ClearAllCookies()
        End If
    End Sub

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Page.Validate()

        If IsValid Then
            Dim email As String = txtEmail.Text.Trim()

            Dim token As String = Nothing
            Dim returnvalue As DbResult = CreateGecoAccount(email, txtPwd.Text, token)

            Select Case returnvalue
                Case DbResult.Success
                    SendConfirmAccountEmail(email, token)
                    Response.Redirect("~/Account.aspx?result=Success", False)

                Case DbResult.Failure
                    '  User already exists
                    Response.Redirect("~/Account.aspx?result=Exists", False)

                Case Else
                    Dim ex As New Exception("GECO Registration Error")
                    ex.Data.Add("Email", email)
                    ex.Data.Add("Method", MethodBase.GetCurrentMethod.Name)
                    ErrorReport(ex, False)
                    Response.Redirect("~/Account.aspx?result=Error", False)
            End Select
        End If
    End Sub

    Protected Sub lbtnRefreshCaptcha_Click(sender As Object, e As EventArgs) Handles lbtnRefreshCaptcha.Click
        captchaControl.ValidateCaptcha(String.Empty)
        txtCaptcha.Text = ""
    End Sub

    Protected Sub cvEmailExists_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvEmailExists.ServerValidate
        args.IsValid = Not GecoUserExists(args.Value)
    End Sub

    Protected Sub cvCaptcha_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles cvCaptcha.ServerValidate
        captchaControl.ValidateCaptcha(txtCaptcha.Text)
        txtCaptcha.Text = ""
        args.IsValid = captchaControl.UserValidated
    End Sub

    ''' <summary>
    ''' Helper function for the validatePassword()
    ''' </summary>
    ''' <returns>True if the password Is valid, false otherwise.</returns>
    Private Function checkPasswordValid() As Boolean
        Dim email As String = txtEmail.Text.ToLower
        Dim password As String = txtPwd.Text.ToLower

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
