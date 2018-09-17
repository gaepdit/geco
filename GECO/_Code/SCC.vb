Imports System.Data.SqlClient

Public Module SCC

    Public Function GetSCC(ByVal strDesc1 As String, ByVal strDesc2 As String, ByVal strDesc3 As String, ByVal strDesc4 As String) As String
        Try
            Dim query As String = "SELECT SourceClassCode FROM EISLK_SourceClassCode " &
                " Where Active = '1' and STRDESC1 = @strDesc1 and STRDESC2 = @strDesc2 " &
                " and STRDESC3 = @strDesc3 and STRDESC4 = @strDesc4 "

            Dim params As SqlParameter() = {
                New SqlParameter("@strDesc1", strDesc1),
                New SqlParameter("@strDesc2", strDesc2),
                New SqlParameter("@strDesc3", strDesc3),
                New SqlParameter("@strDesc4", strDesc4)
            }

            Return DB.GetString(query, params)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Module