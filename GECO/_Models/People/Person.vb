Namespace GecoModels
    Public Class Person

        ' Name
        Public Property Salutation As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property Suffix As String

        ' Org
        Public Property Title As String
        Public Property Company As String

        ' Contact
        Public Property Address As Address
        Public Property Email As String

        ' (Unformatted DB phone numbers)
        Public Property PhoneNumber As String
        Public Property FaxNumber As String

        ' Read-only properties
        Public ReadOnly Property PhoneMain As String
            Get
                Return Mid(PhoneNumber, 1, 10)
            End Get
        End Property

        Public ReadOnly Property PhoneExt As String
            Get
                Return Mid(PhoneNumber, 11)
            End Get
        End Property

        Public ReadOnly Property PhoneFormatted As String
            Get
                Return FormatPhoneNumber(PhoneMain)
            End Get
        End Property

        Public ReadOnly Property FaxFormatted As String
            Get
                Return FormatPhoneNumber(FaxNumber)
            End Get
        End Property

        Public ReadOnly Property FullName As String
            Get
                Return ConcatNonEmptyStrings(" ", {FirstName, LastNameWithSuffix})
            End Get
        End Property

        Public ReadOnly Property AlphaName As String
            Get
                Return ConcatNonEmptyStrings(", ", {LastNameWithSuffix, FirstName})
            End Get
        End Property

        Private ReadOnly Property LastNameWithSuffix As String
            Get
                Return ConcatNonEmptyStrings(", ", {LastName, Suffix})
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return AlphaName
        End Function

        ' Shared functions
        Public Shared Function FormatPhoneNumber(phone As String) As String
            If String.IsNullOrEmpty(phone) OrElse phone.Length <> 10 OrElse Not IsNumeric(phone) Then
                Return phone
            End If

            Return phone.Insert(6, "-").Insert(3, "-")
        End Function

    End Class
End Namespace