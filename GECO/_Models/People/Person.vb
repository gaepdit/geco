Namespace GecoModels
    Public Class Person

        ' Name
        Public Property Honorific As String
        Public Property FirstName As String
        Public Property LastName As String

        ' Org
        Public Property Title As String
        Public Property Company As String

        ' Contact
        Public Property Address As Address
        Public Property Email As String

        ' (Phone numbers)
        Public Property PhoneNumber As String

        ' Static functions

        Public Shared Function ResolvePhoneNumbers(formattedPhone As String, unformattedPhone As String) As String
            If Not String.IsNullOrEmpty(formattedPhone) Then
                Return formattedPhone
            End If

            Return FormatPhoneNumber(unformattedPhone)
        End Function

        Public Shared Function FormatPhoneNumber(unformattedPhone As String) As String
            If String.IsNullOrEmpty(unformattedPhone) OrElse unformattedPhone.Length < 10 OrElse Not IsNumeric(unformattedPhone) Then
                Return unformattedPhone
            End If

            Dim PhoneMain As String = unformattedPhone.Substring(0, 10).Insert(6, "-").Insert(3, "-")
            Dim PhoneExt As String = unformattedPhone.Substring(10)

            Return ConcatNonEmptyStrings(" x ", {PhoneMain, PhoneExt})
        End Function

        ' Read-only properties

        Public ReadOnly Property FullName As String
            Get
                Return ConcatNonEmptyStrings(" ", {FirstName, LastName})
            End Get
        End Property

        Public ReadOnly Property AlphaName As String
            Get
                Return ConcatNonEmptyStrings(", ", {LastName, FirstName})
            End Get
        End Property

        Public Overrides Function ToString() As String
            Return AlphaName
        End Function

    End Class
End Namespace
