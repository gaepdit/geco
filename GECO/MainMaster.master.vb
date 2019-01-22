Partial Class MainMaster
    Inherits MasterPage

    Public Property IncludeSignInLink As Boolean = True
    Public Property IsFacilitySubpage As Boolean = False

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

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
                EasyMenu1.MenuRoot.AddSubMenuItem("Register", "/UserRegistration.aspx")

                If IncludeSignInLink Then
                    Dim path = Replace(Request.Url.PathAndQuery, "default.aspx", "")

                    If String.IsNullOrEmpty(path) Then
                        EasyMenu1.MenuRoot.AddSubMenuItem("Sign In", "/")
                    Else
                        EasyMenu1.MenuRoot.AddSubMenuItem("Sign In", "/?ReturnUrl=" & path)
                    End If
                End If
            End If

        End If
    End Sub

End Class