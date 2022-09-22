Imports System.Data.SqlClient

Public Module eis_NAICS

    Public Function GetNaicsCodeDesc(NaicsCode As String) As String
        Dim query As String = "select NAICS_DESC from LK_NAICS where NAICS_CODE = @code"
        Dim param As New SqlParameter("@code", NaicsCode)
        Return DB.GetString(query, param)
    End Function

    Public Function GetNaicsDataTable() As DataTable
        Dim query As String = "select NAICS_CODE as NAICSCode, NAICS_DESC as strDesc
            from LK_NAICS where ACTIVE = '1' order by NAICSCode"

        Dim dt As DataTable = DB.GetDataTable(query)

        dt.TableName = "NAICSDataTable"

        Return dt
    End Function

    Public Function DoesNaicsCodeExist(NaicsCode As String) As Boolean
        Dim query As String = "select CONVERT(bit, COUNT(*))
            from LK_NAICS where NAICS_CODE = @code and ACTIVE = '1'"

        Dim param As New SqlParameter("@code", NaicsCode)

        Return DB.GetBoolean(query, param)
    End Function

End Module
