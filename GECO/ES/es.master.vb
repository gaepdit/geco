Imports GECO.GecoModels

Partial Class APB_es
    Inherits MasterPage

    Public ReadOnly Property raygunInfo As New RaygunInfo()

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        AirsSelectedCheck()

        'Check if the user has access to the Application
        Dim facilityAccess = GetCurrentUser().GetFacilityAccess(New ApbFacilityId(GetCookie(Cookie.AirsNumber).ToString))

        If Not facilityAccess.ESAccess Then
            Response.Redirect("~/NoAccess.aspx")
        End If

        If Not IsPostBack Then
            Master.IsFacilitySet = True
        End If
    End Sub

End Class
