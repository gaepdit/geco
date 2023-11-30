Imports System.Data.SqlClient

Namespace DAL.EIS
    Public Module EisDates

        Public Function GetEIDeadline(eiyr As Integer) As Date
            Dim query = "Select datDeadline FROM EIThresholdYears Where strYear = @eiyr"
            Dim p As New SqlParameter("@eiyr", eiyr)
            Return DB.GetSingleValue(Of Date)(query, p)
        End Function

        Public Function GetCurrentEiYear() As Integer
            Return DB.GetInteger("select max(INVENTORYYEAR) FROM dbo.EIS_ADMIN")
        End Function

    End Module
End Namespace
