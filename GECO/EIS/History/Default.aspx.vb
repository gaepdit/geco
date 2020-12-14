Imports GECO.GecoModels

Public Class EIS_History_Default
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim currentAirs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(currentAirs) Then
            Response.Redirect("~/")
        End If

        Master.CurrentAirs = New ApbFacilityId(currentAirs)
        Master.IsFacilitySet = True
    End Sub

End Class
