Imports GECO.DAL

Public Class Memo
    Inherits MasterPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblDirector.Text = GetManagerName("EpdDirector")
    End Sub

End Class