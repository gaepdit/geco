Namespace GecoModels
    Public Class UserSession

        Public Sub New()
        End Sub

        Public Sub New(series As String, token As String)
            Me.Series = series
            Me.Token = token
        End Sub

        Public Property Series As String
        Public Property Token As String

    End Class
End Namespace
