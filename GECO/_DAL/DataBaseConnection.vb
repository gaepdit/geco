Public Module DatabaseConnection

    ' Connection string
    Public ReadOnly DBConnectionString As String = ConfigurationManager.ConnectionStrings("SqlConnectionString").ToString

    ' DB Helper
    Friend DB As EpdIt.DBHelper = New EpdIt.DBHelper(DBConnectionString)

End Module

Public Enum DbResult
    Success
    Failure
    DbError
End Enum
