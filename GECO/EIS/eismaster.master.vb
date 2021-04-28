Imports GECO.GecoModels

Partial Class APB_eismaster
    Inherits MasterPage

    Public ReadOnly Property raygunInfo As New RaygunInfo()
    Private Property currentUser As GecoUser
    Private Property currentAirs As ApbFacilityId

    Private Sub APB_eismaster_Init(sender As Object, e As EventArgs) Handles Me.Init
        Response.Redirect("~/EIS/")
        MainLoginCheck()
        EisLoginCheck()

        currentUser = GetCurrentUser()
        currentAirs = GetCookie(Cookie.AirsNumber)

        'Check if the user has access to the Application
        Dim facilityAccess = currentUser.GetFacilityAccess(currentAirs)
        If Not facilityAccess.EisAccess Then
            Response.Redirect("~/NoAccess.aspx")
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblUserName.Text = currentUser.FullName
            lblEISAirsNumber.Text = currentAirs.FormattedString
            lblFacilityName.Text = GetFacilityName(currentAirs)

            'EasyMenu1.MenuStyle.BorderStyle = "0px solid"
            'EasyMenu1.MenuStyle.MenuItemStyle = "color:#0000ff;font-family:Verdana;font-size:small;margin: 0px 0px 0px 1px;"
            'EasyMenu1.MenuStyle.BackgroundActiveColor = "#ffffff"
            'EasyMenu1.MenuStyle.BackgroundColor = "#9bd7ff;"

            'EasyMenu1.MenuRoot.AddSubMenuItem("Facility Home", "../Facility/")
            'EasyMenu1.MenuRoot.AddSubMenuItem("Home", "../Home/")
            'EasyMenu1.MenuRoot.AddSubMenuItem("Contact Us", "../ContactUs.aspx")
            'EasyMenu1.MenuRoot.AddSubMenuItem("Account", "../Account/")
        End If
    End Sub

End Class
