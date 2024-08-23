Imports Microsoft.Data.SqlClient

Public Module ZipCode

    Public Function GetCityStateFromZip(zip As String) As DataRow
        Return GetCityStateFromZip(CInt(zip))
    End Function

    Public Function GetCityStateFromZip(zip As Integer) As DataRow
        Dim query As String = "Select city, state FROM ZipCityState where zip = @zip"
        Dim param As New SqlParameter("@zip", zip)
        Return DB.GetDataRow(query, param)
    End Function

End Module
