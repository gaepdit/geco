Imports GECO.GecoModels

Partial Class MainLayout
    Inherits MasterPage

    Public ReadOnly Property raygunInfo As New RaygunInfo()
    Public Property IsFacilitySet As Boolean = False
    Public Property IsLoggedIn As Boolean
    Public Property CurrentAirs As ApbFacilityId
    Public ReadOnly Property Environment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' TODO: REMOVE
        LoadTempData()
        Return

        CurrentAirs = If(CurrentAirs, ApbFacilityId.IfValid(GetCookie(Cookie.AirsNumber)))
        IsLoggedIn = UserIsLoggedIn()

        SetFacility()
    End Sub

    Private Sub LoadTempData()
        CurrentAirs = New ApbFacilityId("999-00001")
        lblFacilityDisplay.Text = "ABC Company, Atlanta"
        lblAIRS.Text = CurrentAirs.FormattedString
        IsFacilitySet = True
    End Sub

    Public Sub SetFacility()
        If Not IsPostBack AndAlso IsFacilitySet AndAlso IsLoggedIn AndAlso CurrentAirs IsNot Nothing Then
            lblFacilityDisplay.Text = GetFacilityNameAndCity(CurrentAirs)
            lblAIRS.Text = CurrentAirs.FormattedString()
        End If
    End Sub

    Public Sub SetDefaultButton(button As Button)
        NotNull(button, NameOf(button))

        MainForm.DefaultButton = button.UniqueID
    End Sub

    Public Sub ClearDefaultButton()
        MainForm.DefaultButton = ""
    End Sub

End Class
