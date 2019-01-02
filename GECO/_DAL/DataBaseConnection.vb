Public Module DatabaseConnection

    ' Connection string
    Public oradb As String = ConfigurationManager.ConnectionStrings("SqlConnectionString").ToString

    ' DB Helper
    Public DB As EpdIt.DBHelper = New EpdIt.DBHelper(oradb)

    'Function to set provider name for gridviews
    Function setProviderName() As String
        Return "System.Data.SqlClient"
    End Function

End Module

Public Enum DbResult
    Success
    Failure
    DbError
End Enum
