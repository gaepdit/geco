Imports GECO.GecoModels

Partial Class Fees_Fees
    Inherits MasterPage

    Private Property currentUser As GecoUser
    Private Property currentAirs As ApbFacilityId

    Private Sub Fees_Fees_Init(sender As Object, e As EventArgs) Handles Me.Init
        MainLoginCheck()
        AirsSelectedCheck()

        currentUser = GetCurrentUser()
        currentAirs = GetCookie(Cookie.AirsNumber)

        'Check if the user has access to the Application
        Dim facilityAccess = currentUser.GetFacilityAccess(currentAirs)
        If Not facilityAccess.FeeAccess Then
            Response.Redirect("~/NoAccess.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            EasyMenu1.MenuStyle.BorderStyle = "0px solid"
            EasyMenu1.MenuStyle.MenuItemStyle = "color:#0000ff;font-family:Verdana;font-size:small;margin: 0px 0px 0px 1px;"
            EasyMenu1.MenuStyle.BackgroundActiveColor = "#ffffff"
            EasyMenu1.MenuStyle.BackgroundColor = "#9bd7ff;"

            Dim menu As Sequentum.EasyMenuItem
            menu = EasyMenu1.MenuRoot.AddSubMenuItem("Facility Home", "../FacilityHome.aspx")
            menu = EasyMenu1.MenuRoot.AddSubMenuItem("My Home", "../UserHome.aspx")
            menu = EasyMenu1.MenuRoot.AddSubMenuItem("Contact Us", "javascript:var w=window.open('../ContactUs.aspx','', 'width=600,height=600,scrollbars=yes,resizeable=yes');")
            menu = EasyMenu1.MenuRoot.AddSubMenuItem("Sign Out", "../Default.aspx?do=SignOut")
        End If
    End Sub
End Class