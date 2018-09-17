Imports System.Diagnostics
Imports System.Runtime.CompilerServices

Public Module ParseNullables

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ParseAsNullableDateTime(input As String) As DateTime?
        Dim result As DateTime

        If (DateTime.TryParse(input, result)) Then
            Return result
        End If

        Return Nothing
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ParseAsNullableDecimal(input As String) As Decimal?
        Dim result As Decimal

        If (Decimal.TryParse(input, result)) Then
            Return result
        End If

        Return Nothing
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ParseAsNullableDouble(input As String) As Double?
        Dim result As Double

        If (Double.TryParse(input, result)) Then
            Return result
        End If

        Return Nothing
    End Function

    <DebuggerStepThrough()>
    <Extension()>
    Public Function ParseAsNullableInteger(input As String) As Integer?
        Dim result As Integer

        If (Integer.TryParse(input, result)) Then
            Return result
        End If

        Return Nothing
    End Function

End Module
