Imports GECO.GecoModels

Partial Class MainMaster
    Inherits MasterPage

    Public Property IncludeSignInLink As Boolean = True
    Public Property IncludeRegisterLink As Boolean = True
    Public Property IsFacilitySubpage As Boolean = False
    Private Property currentUser As GecoUser

    Public WriteOnly Property UserName As String
        Set(value As String)
            If Not String.IsNullOrWhiteSpace(value) Then
                lblUserName.Text = String.Concat("Welcome: ", value)
            End If
        End Set
    End Property

    Public WriteOnly Property Facility As String
        Set(value As String)
            If Not String.IsNullOrWhiteSpace(value) Then
                lblFacility.Text = String.Concat("Current Facility: ", value)
            End If
        End Set
    End Property

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetUpEasyMenu()

        If Not IsPostBack AndAlso UserIsLoggedIn() Then
            currentUser = GetCurrentUser()

            If currentUser IsNot Nothing Then
                UserName = If(String.IsNullOrWhiteSpace(currentUser.FullName), currentUser.Email, currentUser.FullName)
            End If
        End If
    End Sub

    Private Sub SetUpEasyMenu()
        If Not IsPostBack Then
            Dim contactLink As String = "javascript:var w=window.open('/ContactUs.aspx','', 'width=600,height=600,scrollbars=yes,resizeable=yes');"

            EasyMenu1.MenuStyle.BorderStyle = "0px solid"
            EasyMenu1.MenuStyle.MenuItemStyle = "color:#33d;font-family:Verdana;font-size:small;margin: 0px 0px 0px 1px;"
            EasyMenu1.MenuStyle.MenuItemActiveStyle = "color:#000099;"
            EasyMenu1.MenuStyle.BackgroundColor = "#b2dffd;"
            EasyMenu1.MenuStyle.BackgroundActiveColor = "#ffffff"

            If UserIsLoggedIn() Then
                ' Logged in

                If IsFacilitySubpage Then
                    EasyMenu1.MenuRoot.AddSubMenuItem("Facility Home", "/Facility/")
                End If

                EasyMenu1.MenuRoot.AddSubMenuItem("Home", "/Home/")
                EasyMenu1.MenuRoot.AddSubMenuItem("Contact Us", contactLink)
                EasyMenu1.MenuRoot.AddSubMenuItem("Account", "/Account/")
            Else
                ' Not logged in
                EasyMenu1.MenuRoot.AddSubMenuItem("Contact Us", contactLink)

                If IncludeRegisterLink Then
                    EasyMenu1.MenuRoot.AddSubMenuItem("Register", "/Register.aspx")
                End If

                Dim path = Replace(Request.Url.PathAndQuery, "default.aspx", "")
                path = Replace(path, "Default.aspx", "")
                path = Replace(path, "register.aspx", "")
                path = Replace(path, "Register.aspx", "")
                path = Replace(path, "/?do=SignOut", "")

                If String.IsNullOrEmpty(path) OrElse path = "/" Then
                    EasyMenu1.MenuRoot.AddSubMenuItem("Sign In", "/Login.aspx")
                Else
                    EasyMenu1.MenuRoot.AddSubMenuItem("Sign In", "/Login.aspx?ReturnUrl=" & path)
                End If
            End If
        End If
    End Sub

End Class