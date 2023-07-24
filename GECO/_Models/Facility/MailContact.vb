Namespace GecoModels.Facility
    Public Class MailContact

        Public Property Id As Guid
        Public Property FirstName As String
        Public Property LastName As String
        Public Property Prefix As String
        Public Property Title As String
        Public Property Organization As String
        Public Property Address1 As String
        Public Property Address2 As String
        Public Property City As String
        Public Property State As String
        Public Property PostalCode As String
        Public Property Telephone As String
        Public Property Email As String

        Public ReadOnly Property Name As String
            Get
                Return ConcatNonEmptyStrings(" ", {Prefix, FirstName, LastName})
            End Get
        End Property

    End Class
End Namespace
