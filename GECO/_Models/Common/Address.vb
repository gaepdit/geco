Namespace GecoModels
    Public Class Address
        ' Address is simply all the elements of a postal address

        Public Property Street As String
        Public Property Street2 As String
        Public Property City As String
        Public Property State As String
        Public Property PostalCode As String

        Public Overrides Function ToString() As String
            Return CompileAddressString(CompileStringType.Linear)
        End Function

        Public Function ToNewLineString() As String
            Return CompileAddressString(CompileStringType.NewLine)
        End Function

        Public Function ToLinearString() As String
            Return CompileAddressString(CompileStringType.Linear)
        End Function

        Public Function ToHtmlString() As String
            Return CompileAddressString(CompileStringType.Html)
        End Function

        Private Enum CompileStringType
            NewLine
            Linear
            Html
        End Enum

        Private Function CompileAddressString(type As CompileStringType) As String
            Dim cityState As String = ConcatNonEmptyStrings(", ", {City, State})
            Dim zip As String = FormatPostalCode(PostalCode)
            Dim separator As String = ""

            Select Case type
                Case CompileStringType.Html
                    separator = "<br />"
                Case CompileStringType.Linear
                    separator = ", "
                Case CompileStringType.NewLine
                    separator = vbNewLine
            End Select

            Return ConcatNonEmptyStrings(separator, {Street, Street2, cityState & " " & zip})
        End Function

        Public Shared Function FormatPostalCode(postalCode As String) As String
            If postalCode IsNot Nothing AndAlso postalCode.Length = 9 AndAlso IsNumeric(postalCode) Then
                Return postalCode.Insert(5, "-")
            Else
                Return postalCode
            End If
        End Function

        ' Try to get a 2-character state code
        Public Shared Function ProbableStateCode(state As String) As String
            If String.IsNullOrWhiteSpace(state) Then
                Return Nothing
            End If

            If USStateCodes.ContainsValue(state) Then
                Return state
            End If

            If USStateCodes.ContainsKey(state) Then
                Return USStateCodes(state)
            End If

            Return Nothing
        End Function

        Private Shared ReadOnly USStateCodes As Dictionary(Of String, String) = New Dictionary(Of String, String) From {
            {"Alabama", "AL"},
            {"Alaska", "AK"},
            {"Arizona", "AZ"},
            {"Arkansas", "AR"},
            {"California", "CA"},
            {"Colorado", "CO"},
            {"Connecticut", "CT"},
            {"Delaware", "DE"},
            {"District of Columbia", "DC"},
            {"Washington DC", "DC"},
            {"Florida", "FL"},
            {"Georgia", "GA"},
            {"Hawaii", "HI"},
            {"Idaho", "ID"},
            {"Illinois", "IL"},
            {"Indiana", "IN"},
            {"Iowa", "IA"},
            {"Kansas", "KS"},
            {"Kentucky", "KY"},
            {"Louisiana", "LA"},
            {"Maine", "ME"},
            {"Maryland", "MD"},
            {"Massachusetts", "MA"},
            {"Michigan", "MI"},
            {"Minnesota", "MN"},
            {"Mississippi", "MS"},
            {"Missouri", "MO"},
            {"Montana", "MT"},
            {"Nebraska", "NE"},
            {"Nevada", "NV"},
            {"New Hampshire", "NH"},
            {"New Jersey", "NJ"},
            {"New Mexico", "NM"},
            {"New York", "NY"},
            {"North Carolina", "NC"},
            {"North Dakota", "ND"},
            {"Ohio", "OH"},
            {"Oklahoma", "OK"},
            {"Oregon", "OR"},
            {"Pennsylvania", "PA"},
            {"Rhode Island", "RI"},
            {"South Carolina", "SC"},
            {"South Dakota", "SD"},
            {"Tennessee", "TN"},
            {"Texas", "TX"},
            {"Utah", "UT"},
            {"Vermont", "VT"},
            {"Virginia", "VA"},
            {"Washington", "WA"},
            {"Washington State", "WA"},
            {"West Virginia", "WV"},
            {"Wisconsin", "WI"},
            {"Wyoming", "WY"},
            {"Puerto Rico", "PR"}
        }

    End Class
End Namespace
