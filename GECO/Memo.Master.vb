Public Class MemoLayout
    Inherits MasterPage

    Protected ReadOnly Property raygunInfo As New RaygunInfo()
    Public ReadOnly Property CurrentEnvironment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")
    Public Property MemoPageCount As Integer

    Protected ReadOnly Property PageCountDisplay As String
        Get
            Return If(MemoPageCount > 1, $"({MemoPageCount} pages)", "")
        End Get
    End Property

End Class
