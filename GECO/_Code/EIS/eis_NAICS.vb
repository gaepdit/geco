Imports System.Data
Imports System.Data.SqlClient

Public Module eis_NAICS

    Public Function GetNaicsCodeDesc(ByVal NaicsCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM EISLK_NAICSCODE " &
            " Where NAICSCode = @code " &
            " and Active = '1' "

        Dim param As New SqlParameter("@code", NaicsCode)

        Return DB.GetString(query, param)
    End Function

    Public Function GetNaicsDataTable() As DataTable
        Dim query As String = "SELECT NAICSCode, strDesc " &
            " FROM EISLK_NAICSCODE " &
            " where Active = '1' " &
            " and len(NAICSCode) = 6 " &
            " ORDER BY NAICSCode"

        Dim dt As DataTable = DB.GetDataTable(query)

        dt.TableName = "NAICSDataTable"

        Return dt
    End Function

    Public Function DoesNaicsCodeExist(NaicsCode As String) As Boolean
        Dim query As String = "Select CONVERT( bit, COUNT(*)) " &
                " FROM EISLK_NAICSCODE " &
                " where NAICSCode = @code " &
                " and Active = '1' "

        Dim param As New SqlParameter("@code", NaicsCode)

        Return DB.GetBoolean(query, param)
    End Function

End Module
