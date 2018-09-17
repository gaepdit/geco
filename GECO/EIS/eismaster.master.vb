Imports GECO.GecoModels

Partial Class APB_eismaster
    Inherits MasterPage

    Private Property currentUser As GecoUser
    Private Property currentAirs As ApbFacilityId

    Private Sub APB_eismaster_Init(sender As Object, e As EventArgs) Handles Me.Init
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

            EasyMenu1.MenuStyle.BorderStyle = "0px solid"
            EasyMenu1.MenuStyle.MenuItemStyle = "color:#0000ff;font-family:Verdana;font-size:small;margin: 0px 0px 0px 1px;"
            EasyMenu1.MenuStyle.BackgroundActiveColor = "#ffffff"
            EasyMenu1.MenuStyle.BackgroundColor = "#9bd7ff;"

            EasyMenu1.MenuRoot.AddSubMenuItem("Facility Home", "../FacilityHome.aspx")
            EasyMenu1.MenuRoot.AddSubMenuItem("My Home", "../UserHome.aspx")
            EasyMenu1.MenuRoot.AddSubMenuItem("Contact Us", "javascript:var w=window.open('../ContactUs.aspx','', 'width=600,height=600,scrollbars=yes,resizeable=yes');")
            EasyMenu1.MenuRoot.AddSubMenuItem("Sign Out", "../Default.aspx?do=SignOut")
        End If
    End Sub

End Class