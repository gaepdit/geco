Imports GECO.GecoModels

Public Class EIS
    Inherits MasterPage

    Public Property CurrentAirs As ApbFacilityId
    Public Property SelectedTab As EisTab = EisTab.Home

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If CurrentAirs Is Nothing Then
            CompleteRedirect("~/")
            Return
        End If

        Master.CurrentAirs = CurrentAirs
        Master.IsFacilitySet = True

        If Not IsPostBack Then
            SetTabs()
        End If
    End Sub

    Private Sub SetTabs()
        Select Case SelectedTab
            Case EisTab.Home
                lnkHome.CssClass = "selected-menu-item"
            Case EisTab.History
                lnkHistory.CssClass = "selected-menu-item"
            Case EisTab.None
                eisDefaultHeader.Visible = False
        End Select
    End Sub

    Public Enum EisTab
        Home
        History
        None
    End Enum

End Class
