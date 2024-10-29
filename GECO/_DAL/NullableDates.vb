Imports GaEpd

Module NullableDates
    Public Function GetNullableDateTime(obj As Object) As DateTime?
        Dim newDate As Date? = DBUtilities.GetNullableDateTime(obj)
        If newDate IsNot Nothing AndAlso newDate = New Date(1776, 7, 4) Then
            Return Nothing
        End If
        Return newDate
    End Function

End Module
