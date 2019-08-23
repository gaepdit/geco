Imports GECO.DAL

Public Class Memo
    Inherits MasterPage

    Public Property EpdDirector As String
    Public Property MemoPageCount As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            EpdDirector = GetManagerName("EpdDirector")
        End If
    End Sub

    Protected Function PageCountDisplay() As String
        If MemoPageCount > 1 Then
            Return "(" & MemoPageCount & " pages)"
        End If

        Return ""
    End Function

End Class