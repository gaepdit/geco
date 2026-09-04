Namespace EmailAPI

    Public Class Email
        Public Property From As String
        Public Property FromName As String
        Public Property Recipients As List(Of String)
        Public Property Body As String
        Public Property Subject As String
        Public Property CopyRecipients As List(Of String)
        Public Property IsHtml As Boolean
    End Class

    Public Class EmailForBatchRequest
        Public Property BatchId As Guid
        Public Property Emails As List(Of Email)
    End Class

    Public Class BatchRequest
        Public Property BatchId As Guid
    End Class

End Namespace
