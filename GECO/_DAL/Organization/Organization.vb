Namespace DAL
    Public Module Organization

        Public Function GetEpdManagersAsDataTable() As DataTable
            Dim query As String = "SELECT STRKEY AS Role, STRMANAGEMENTNAME AS Name FROM LOOKUPAPBMANAGEMENTTYPE WHERE STRCURRENTCONTACT = 'C'"
            Dim dt As DataTable = DB.GetDataTable(query)
            dt.PrimaryKey = New DataColumn() {dt.Columns("Role")}
            Return dt
        End Function

        Public Function GetManagerName(role As String) As String
            Dim dt As DataTable = GetEpdManagersAsDataTable()

            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Return Nothing
            End If

            Dim dr As DataRow = dt.Rows.Find(role)

            If dr Is Nothing Then
                Return Nothing
            End If

            Return dr("Name")
        End Function

    End Module
End Namespace