Imports Microsoft.Data.SqlClient

Friend Module DatabaseConnection

    Public Property DB As GaEpd.DBHelper = GetDBHelper()

    Private Function GetDBHelper() As GaEpd.DBHelper
        Dim connectionString As String = ConfigurationManager.ConnectionStrings("SqlConnectionString").ToString

        Dim options As New SqlRetryLogicOption() With {
            .NumberOfTries = 5,
            .DeltaTime = TimeSpan.FromSeconds(10),
            .MaxTimeInterval = TimeSpan.FromSeconds(15)
        }

        Dim connectionRetryProvider As SqlRetryLogicBaseProvider = SqlConfigurableRetryFactory.CreateFixedRetryProvider(options)

        Return New GaEpd.DBHelper(connectionString, connectionRetryProvider)
    End Function

End Module

Public Enum DbResult
    Success
    Failure
    DbError
End Enum
