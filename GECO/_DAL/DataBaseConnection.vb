Public Module DatabaseConnection

    ' Connection string
    Public DBConnectionString As String = ConfigurationManager.ConnectionStrings("SqlConnectionString").ToString

    ' DB Helper
    Public DB As EpdIt.DBHelper = New EpdIt.DBHelper(DBConnectionString)

End Module

Public Enum DbResult
    Success
    Failure
    DbError
End Enum
