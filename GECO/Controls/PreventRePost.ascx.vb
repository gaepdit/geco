Partial Class PreventRePost
    Inherits UserControl
    ' https://www.codeproject.com/tips/319955/how-to-prevent-re-post-action-caused-by-pressing-b
    ' Probably shouldn't be used on a page with an UpdatePanel

    Private Sub PreventRePost_Load(sender As Object, e As EventArgs) Handles Me.Load
        CancelUnexpectedRePost()
    End Sub

    Private Sub CancelUnexpectedRePost()
        Dim clientCode, serverCode As String

        clientCode = _repostCheckCode.Value

        If Session.Item("_repostCheckCode") Is Nothing Then
            serverCode = String.Empty
        Else
            serverCode = Session("_repostCheckCode").ToString
        End If

        If Not IsPostBack OrElse clientCode.Equals(serverCode) Then
            ' If not a POST or if the codes are equal, then the action was initiated by the user
            ' Generate a new code
            Dim code As String = Guid.NewGuid.ToString
            _repostCheckCode.Value = code
            Session.Item("_repostCheckCode") = code
        Else
            Response.Redirect(Request.Url.AbsoluteUri)
        End If
    End Sub

End Class
