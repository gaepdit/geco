Imports System.Runtime.CompilerServices

Public Module DbStringUtils

    Public Function DbStringDecimalOrNull(tryDecimal As String) As String
        If Decimal.TryParse(tryDecimal, Nothing) Then
            Return tryDecimal
        Else
            Return "NULL"
        End If
    End Function

    Public Function DbStringDoubleOrNull(tryDouble As String) As String
        If Double.TryParse(tryDouble, Nothing) Then
            Return tryDouble
        Else
            Return "NULL"
        End If
    End Function

    Public Function DbStringIntOrNull(tryInteger As String) As String
        If Integer.TryParse(tryInteger, Nothing) Then
            Return tryInteger
        Else
            Return "NULL"
        End If
    End Function

    Public Function DbStringShortDateOrNull(tryDate As String) As String
        If Date.TryParse(tryDate, Nothing) Then
            Return "'" & CDate(tryDate).ToShortDateString & "'"
        Else
            Return "NULL"
        End If
    End Function

    Public Function DbStringNonEmptyOrNull(tryString As String) As String
        If Not String.IsNullOrEmpty(tryString) Then
            Return "'" & tryString & "'"
        Else
            Return "NULL"
        End If
    End Function

    <Extension()>
    Public Function RealStringOrNothing(s As String) As String
        If String.IsNullOrEmpty(s) Then
            Return Nothing
        Else
            Return s
        End If
    End Function

End Module
