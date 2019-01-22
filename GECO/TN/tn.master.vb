Imports GECO.GecoModels

Partial Class APB_tn
    Inherits MasterPage

    Private Property currentUser As GecoUser
    Private Property currentAirs As ApbFacilityId

    Private Sub APB_tn_Init(sender As Object, e As EventArgs) Handles Me.Init
        MainLoginCheck()
        AirsSelectedCheck()

        currentUser = GetCurrentUser()
        currentAirs = GetCookie(Cookie.AirsNumber)
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            lblUserName.Text = currentUser.FullName
            lblAirsNo.Text = currentAirs.FormattedString
            lblFacilityName.Text = GetFacilityName(currentAirs)

            EasyMenu1.MenuStyle.BorderStyle = "0px solid"
            EasyMenu1.MenuStyle.MenuItemStyle = "color:#0000ff;font-family:Verdana;font-size:small;margin: 0px 0px 0px 1px;"
            EasyMenu1.MenuStyle.BackgroundActiveColor = "#ffffff"
            EasyMenu1.MenuStyle.BackgroundColor = "#9bd7ff;"

            EasyMenu1.MenuRoot.AddSubMenuItem("Facility Home", "../Facility/")
            EasyMenu1.MenuRoot.AddSubMenuItem("Home", "../Home/")
            EasyMenu1.MenuRoot.AddSubMenuItem("Contact Us", "javascript:var w=window.open('../ContactUs.aspx','', 'width=600,height=600,scrollbars=yes,resizeable=yes');")
            EasyMenu1.MenuRoot.AddSubMenuItem("Account", "../Account/")
        End If
    End Sub

End Class