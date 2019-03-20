Imports System.Data.SqlClient

Public Module SCC

    Public Function SccIsValid(SCC As String) As Boolean
        Dim query As String = "select convert(bit, count(*))
            FROM EISLK_SCC
            where [data category] = 'Point'
              and [status] = 'Active'
              and SCC = @SCC"

        Return DB.GetBoolean(query, New SqlParameter("@SCC", SCC))
    End Function

    Public Function GetSccLevel1() As DataTable
        Dim query As String = "select distinct [scc level one]
                from EISLK_SCC
                where [data category] = 'Point'
                  and [status] = 'Active'
                  order by [scc level one] "

        Return DB.GetDataTable(query)
    End Function

    Public Function GetSccLevel2(desc1 As String) As DataTable
        Dim query As String = "select distinct [scc level two]
                from EISLK_SCC
                where [data category] = 'Point'
                  and [status] = 'Active'
                  and [scc level one] = @desc1
                  order by [scc level two] "

        Dim params As SqlParameter() = {
                New SqlParameter("@desc1", desc1)
            }

        Return DB.GetDataTable(query, params)
    End Function

    Public Function GetSccLevel3(desc1 As String, desc2 As String) As DataTable
        Dim query As String = "select distinct [scc level three]
                from EISLK_SCC
                where [data category] = 'Point'
                  and [status] = 'Active'
                  and [scc level one] = @desc1
                  and [scc level two] = @desc2
                  order by [scc level three] "

        Dim params As SqlParameter() = {
                New SqlParameter("@desc1", desc1),
                New SqlParameter("@desc2", desc2)
            }

        Return DB.GetDataTable(query, params)
    End Function

    Public Function GetSccLevel4(desc1 As String, desc2 As String, desc3 As String) As DataTable
        Dim query As String = "select distinct [scc level four]
                from EISLK_SCC
                where [data category] = 'Point'
                  and [status] = 'Active'
                  and [scc level one] = @desc1
                  and [scc level two] = @desc2
                  and [scc level three] = @desc3
                  order by [scc level four] "

        Dim params As SqlParameter() = {
                New SqlParameter("@desc1", desc1),
                New SqlParameter("@desc2", desc2),
                New SqlParameter("@desc3", desc3)
            }

        Return DB.GetDataTable(query, params)
    End Function

    Public Function GetSccValue(desc1 As String, desc2 As String, desc3 As String, desc4 As String) As String
        Dim query As String = "select [SCC]
                from EISLK_SCC
                where [data category] = 'Point'
                  and [status] = 'Active'
                  and [scc level one] = @desc1
                  and [scc level two] = @desc2
                  and [scc level three] = @desc3
                  and [scc level four] = @desc4"

        Dim params As SqlParameter() = {
                New SqlParameter("@desc1", desc1),
                New SqlParameter("@desc2", desc2),
                New SqlParameter("@desc3", desc3),
                New SqlParameter("@desc4", desc4)
            }

        Return DB.GetString(query, params)
    End Function

End Module