Namespace GecoModels
    Public Class Address
        ' Address is simply all the elements of a postal address

        Public Property Street As String
        Public Property Street2 As String
        Public Property City As String
        Public Property State As String
        Public Property PostalCode As String

        Public Overrides Function ToString() As String
            Return CompileAddressString(False)
        End Function

        Public Function ToLinearString() As String
            Return CompileAddressString(True)
        End Function

        Private Function CompileAddressString(linear As Boolean) As String
            Dim cityState As String = ConcatNonEmptyStrings(", ", {City, State})
            Dim zip As String = FormatPostalCode(PostalCode)
            If linear Then
                Return ConcatNonEmptyStrings(", ", {Street, Street2, cityState & " " & zip})
            Else
                Return ConcatNonEmptyStrings(vbNewLine, {Street, Street2, cityState & " " & zip})
            End If
        End Function

        Public Shared Function FormatPostalCode(postalCode As String) As String
            If postalCode IsNot Nothing AndAlso postalCode.Length = 9 AndAlso IsNumeric(postalCode) Then
                Return postalCode.Insert(5, "-")
            Else
                Return postalCode
            End If
        End Function

    End Class
End Namespace
