﻿Namespace GecoModels
    Public Class Person

        ' Name
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

        Public WriteOnly Property UnformattedPhoneNumber As String
            Set(value As String)
                If Not String.IsNullOrEmpty(value) AndAlso Not String.IsNullOrEmpty(PhoneNumber) Then
                    PhoneNumber = FormatPhoneNumber(value)
                End If
            End Set
        End Property

        Public Shared Function FormatPhoneNumber(unformattedPhone As String) As String
            If String.IsNullOrEmpty(unformattedPhone) OrElse unformattedPhone.Length < 10 OrElse Not IsNumeric(unformattedPhone) Then
                Return unformattedPhone
            End If

            Dim PhoneMain As String = Mid(unformattedPhone, 1, 10).Insert(6, "-").Insert(3, "-")
            Dim PhoneExt As String = Mid(unformattedPhone, 11)

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