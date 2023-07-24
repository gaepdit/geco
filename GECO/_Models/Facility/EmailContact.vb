Namespace GecoModels.Facility
    Public Class EmailContact
        Public Sub New(id As Guid, email As String)
            Me.Id = id
            Me.Email = email
        End Sub

        Public Property Id As Guid = Guid.Empty
        Public Property Email As String = Nothing

    End Class
End Namespace
