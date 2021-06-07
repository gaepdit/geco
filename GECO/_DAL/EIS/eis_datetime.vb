Imports System.Data.SqlClient

Public Module eis_datetime

    Public Function GetEIDeadline(eiyr As Integer) As Date
        Dim query = "Select datDeadline FROM EIThresholdYears Where strYear = @eiyr"
        Dim p As New SqlParameter("@eiyr", eiyr)
        Return DB.GetSingleValue(Of Date)(query, p)
    End Function

End Module
