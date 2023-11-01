Public Module DatabaseConnection

    ' Connection string
    Private ReadOnly DBConnectionString As String = ConfigurationManager.ConnectionStrings("SqlConnectionString").ToString

    ' DB Helper
    Friend DB As New GaEpd.DBHelper(DBConnectionString)

End Module

Public Enum DbResult
    Success
    Failure
    DbError
End Enum
