Imports GECO.GecoModels

Public Class EIS_History_Default
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        Master.CurrentAirs = New ApbFacilityId(airs)
        Master.SelectedTab = EIS.EisTab.History
    End Sub

End Class
