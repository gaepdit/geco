Namespace GecoModels.Facility
    Public Class MailContact

        Public Property Id As Guid = Nothing
        Public Property FirstName As String = Nothing
        Public Property LastName As String = Nothing
        Public Property Prefix As String = Nothing
        Public Property Title As String = Nothing
        Public Property Organization As String = Nothing
        Public Property Address1 As String = Nothing
        Public Property Address2 As String = Nothing
        Public Property City As String = Nothing
        Public Property State As String = Nothing
        Public Property PostalCode As String = Nothing
        Public Property Telephone As String = Nothing
        Public Property Email As String = Nothing

        Public ReadOnly Property Name As String
            Get
                Return ConcatNonEmptyStrings(" ", {Prefix, FirstName, LastName})
            End Get
        End Property

    End Class
End Namespace
