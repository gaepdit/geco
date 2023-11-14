Imports System.Data.SqlClient

Namespace DAL.EIS
    Public Module EisNaics

        Public Function GetNaicsCodeDesc(NaicsCode As String) As String
            Dim query As String = "select NAICS_DESC from LK_NAICS where NAICS_CODE = @code"
            Dim param As New SqlParameter("@code", NaicsCode)
            Return DB.GetString(query, param)
        End Function

    End Module
End Namespace
