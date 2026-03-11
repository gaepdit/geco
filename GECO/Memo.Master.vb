Public Class MemoLayout
    Inherits MasterPage

    Public ReadOnly Property CurrentEnvironment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")
    Public Property CurrentUserId As Integer = GetCurrentUserId()
    Public Property MemoPageCount As Integer

    Protected ReadOnly Property PageCountDisplay As String
        Get
            Return If(MemoPageCount > 1, $"({MemoPageCount} pages)", "")
        End Get
    End Property

End Class
