Imports GECO.GecoModels

Public Class EIS
    Inherits MasterPage

    Public Property CurrentAirs As ApbFacilityId
    Public Property SelectedTab As EisTab = EisTab.Home

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If CurrentAirs Is Nothing Then
            Response.Redirect("~/EIS/")
        End If

        Master.CurrentAirs = CurrentAirs
        Master.IsFacilitySet = True

        If Not IsPostBack Then
            ShowFacilityInfo()
            SetTabs()
        End If
    End Sub

    Private Sub ShowFacilityInfo()
        Dim currentFacility As String = GetFacilityName(CurrentAirs) & ", " & GetFacilityCity(CurrentAirs)
        lblFacilityDisplay.Text = currentFacility
        lblAIRS.Text = CurrentAirs.FormattedString
    End Sub

    Private Sub SetTabs()
        Select Case SelectedTab
            Case EisTab.Home
                lnkHome.CssClass = "selected-menu-item"
            Case EisTab.Facility
                lnkFacility.CssClass = "selected-menu-item"
            Case EisTab.Users
                lnkUsers.CssClass = "selected-menu-item"
            Case EisTab.History
                lnkHistory.CssClass = "selected-menu-item"
        End Select
    End Sub

    Public Enum EisTab
        Home
        Facility
        Users
        History
    End Enum

End Class
