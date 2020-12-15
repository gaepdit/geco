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
        Master.IsFacilitySet = True

        If Not IsPostBack Then
            ShowFacilityInfo()
        End If
    End Sub

    Private Sub ShowFacilityInfo()
        Dim currentFacility As String = GetFacilityName(Master.CurrentAirs) & ", " & GetFacilityCity(Master.CurrentAirs)
        lblFacilityDisplay.Text = currentFacility
        lblAIRS.Text = Master.CurrentAirs.FormattedString
    End Sub

End Class
