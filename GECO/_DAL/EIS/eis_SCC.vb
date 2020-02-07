Imports System.Data.SqlClient

Public Module eis_SCC

    Public Function IsValidScc(SCC As String) As Boolean
        Dim query As String = "select convert(bit, count(*))
            FROM EISLK_SCC
            where [data category] = 'Point'
              and [status] = 'Active'
              and ([last inventory year] = ''
                  or [last inventory year] is null)
              and SCC = @SCC"

        Return DB.GetBoolean(query, New SqlParameter("@SCC", SCC))
    End Function

    Public Function GetSccLevel1() As DataTable
        Dim query As String = "select distinct [scc level one]
                from EISLK_SCC
                where [data category] = 'Point'
                  and [status] = 'Active'
                  and ([last inventory year] = ''
                      or [last inventory year] is null)
                  order by [scc level one] "

        Return DB.GetDataTable(query)
    End Function

    Public Function GetSccLevel2(desc1 As String) As DataTable
        Dim query As String = "select distinct [scc level two]
                from EISLK_SCC
                where [data category] = 'Point'
                  and [status] = 'Active'
                  and ([last inventory year] = ''
                      or [last inventory year] is null)
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
                  and ([last inventory year] = ''
                      or [last inventory year] is null)
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
                  and ([last inventory year] = ''
                      or [last inventory year] is null)
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

    Public Function GetSccDetails(desc1 As String, desc2 As String, desc3 As String, desc4 As String) As SccDetails
        Dim query As String = "select SCC,
               [scc level one],
               [scc level two],
               [scc level three],
               [scc level four],
               status,
               [map to],
               [last inventory year],
               [data category],
               [code description],
               [short name],
               sector,
               [usage notes],
               [last updated date],
               [tier 1 description],
               [tier 2 description],
               [tier 3 description]
        from EISLK_SCC
        where [data category] = 'Point'
          and [status] = 'Active'
          and ([last inventory year] = ''
              or [last inventory year] is null)
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

        Dim dr As DataRow = DB.GetDataRow(query, params)

        Return SccDetailsFromDataRow(dr)
    End Function

    Public Function GetSccDetails(scc As String) As SccDetails
        Dim query As String = "select SCC,
               [scc level one],
               [scc level two],
               [scc level three],
               [scc level four],
               status,
               [map to],
               [last inventory year],
               [data category],
               [code description],
               [short name],
               sector,
               [usage notes],
               [last updated date],
               [tier 1 description],
               [tier 2 description],
               [tier 3 description]
        from EISLK_SCC
        where [data category] = 'Point'
          and [status] = 'Active'
          and ([last inventory year] = ''
              or [last inventory year] is null)
          and [scc] = @scc"

        Dim dr As DataRow = DB.GetDataRow(query, New SqlParameter("@scc", scc))

        Return SccDetailsFromDataRow(dr)
    End Function

    Private Function SccDetailsFromDataRow(dr As DataRow) As SccDetails
        If dr Is Nothing Then
            Return Nothing
        End If

        Return New SccDetails With {
            .SCC = dr("SCC").ToString(),
            .Level1 = dr("scc level one").ToString(),
            .Level2 = dr("scc level two").ToString(),
            .Level3 = dr("scc level three").ToString(),
            .Level4 = dr("scc level four").ToString(),
            .Status = dr("status").ToString(),
            .MapTo = dr("map to").ToString(),
            .LastInventoryYear = dr("last inventory year").ToString(),
            .Category = dr("data category").ToString(),
            .CodeDescription = dr("code description").ToString(),
            .ShortName = dr("short name").ToString(),
            .Sector = dr("sector").ToString(),
            .UsageNotes = dr("usage notes").ToString(),
            .LastUpdated = dr("last updated date").ToString(),
            .Tier1 = dr("tier 1 description").ToString(),
            .Tier2 = dr("tier 2 description").ToString(),
            .Tier3 = dr("tier 3 description").ToString()
        }
    End Function
End Module

Public Class SccDetails
    Public Property SCC As String
    Public Property Level1 As String
    Public Property Level2 As String
    Public Property Level3 As String
    Public Property Level4 As String
    Public Property Status As String
    Public Property MapTo As String
    Public Property LastInventoryYear As String
    Public Property Category As String
    Public Property CodeDescription As String
    Public Property ShortName As String
    Public Property Sector As String
    Public Property UsageNotes As String
    Public Property LastUpdated As String
    Public Property Tier1 As String
    Public Property Tier2 As String
    Public Property Tier3 As String

    Public ReadOnly Property Description As String
        Get
            Return ConcatNonEmptyStrings("; ", {Level1, Level2, Level3, Level4})
        End Get
    End Property
End Class
