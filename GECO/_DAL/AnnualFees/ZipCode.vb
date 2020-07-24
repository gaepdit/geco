Imports System.Data.SqlClient

Public Module ZipCode

    Public Function GetCityStateFromZip(zip As String) As DataTable
        Return GetCityStateFromZip(CInt(zip))
    End Function

    Public Function GetCityStateFromZip(zip As Integer) As DataTable
        Dim query As String = "Select city, state FROM ZipCityState where zip = @zip"
        Dim param As New SqlParameter("@zip", zip)
        Return DB.GetDataTable(query, param)
    End Function

End Module