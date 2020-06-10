Imports GECO.GecoModels

Partial Class MainMaster
    Inherits MasterPage

    Public ReadOnly Property raygunInfo As New RaygunInfo()
    Public Property IncludeSignInLink As Boolean = True
    Public Property IncludeRegisterLink As Boolean = True
    Public Property IsFacilitySubpage As Boolean = False

    Private Property currentUser As GecoUser
    Public Property currentAirs As ApbFacilityId

    Private Sub SetUserName(value As String)
        If Not String.IsNullOrWhiteSpace(value) Then
            lblUserName.Text = String.Concat("Welcome: ", value)
        End If
    End Sub

    Public Sub SetFacility(value As String)
        If Not String.IsNullOrWhiteSpace(value) Then
            lblFacility.Text = String.Concat("Current Facility: ", value)
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        currentUser = GetCurrentUser()
        currentAirs = If(currentAirs, ApbFacilityId.IfValid(GetCookie(Cookie.AirsNumber)))

        If Not IsPostBack AndAlso UserIsLoggedIn() Then
            If currentUser IsNot Nothing Then
                If String.IsNullOrWhiteSpace(currentUser.FullName) Then
                    SetUserName(currentUser.Email)
                Else
                    SetUserName(currentUser.FullName)
                End If
            End If

            If IsFacilitySubpage AndAlso currentAirs IsNot Nothing Then
                SetFacility(ConcatNonEmptyStrings(", ", {currentAirs.FormattedString(), GetFacilityName(currentAirs), GetFacilityCity(currentAirs)}))
            End If

            SetUpEasyMenu()
        End If
    End Sub

    Private Sub SetUpEasyMenu()
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
            EasyMenu1.MenuRoot.AddSubMenuItem("Contact Us", "/ContactUs.aspx")
            EasyMenu1.MenuRoot.AddSubMenuItem("Account", "/Account/")
        Else
            ' Not logged in
            EasyMenu1.MenuRoot.AddSubMenuItem("Contact Us", "/ContactUs.aspx")

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
    End Sub

End Class
