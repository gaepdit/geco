Imports GECO.GecoModels

Partial Class MainMaster
    Inherits MasterPage

    Public ReadOnly Property raygunInfo As New RaygunInfo()
    Public Property IsFacilitySet As Boolean = False
    Public Property IsLoggedIn As Boolean
    Public Property CurrentAirs As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        CurrentAirs = If(CurrentAirs, ApbFacilityId.IfValid(GetCookie(Cookie.AirsNumber)))
        IsLoggedIn = UserIsLoggedIn()

        SetFacility()
    End Sub

    Public Sub SetFacility()
        If Not IsPostBack AndAlso IsFacilitySet AndAlso IsLoggedIn AndAlso CurrentAirs IsNot Nothing Then
            lblFacility.Text =
                ConcatNonEmptyStrings(", ", {GetFacilityName(CurrentAirs), GetFacilityCity(CurrentAirs)}) &
                " (" & CurrentAirs.FormattedString() & ")"
        End If
    End Sub

End Class
