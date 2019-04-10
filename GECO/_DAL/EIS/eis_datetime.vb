Imports System.Data.SqlClient

Public Module eis_datetime

    Public Function GetEIDeadline(ByVal eiyr As Integer) As String
        Dim query = "Select datDeadline FROM EIThresholdYears Where strYear = @eiyr"
        Dim p As New SqlParameter("@eiyr", eiyr)
        Return DB.GetSingleValue(Of Date)(query, p).ToShortDateString
    End Function

End Module