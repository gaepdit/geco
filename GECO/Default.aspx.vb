Imports System.Threading.Tasks
Imports GECO.GecoModels

Partial Class _Default
    Inherits Page

    Private Async Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            Throw New HttpException(405, "Method Not Allowed")
        End If

        If Request.QueryString("do") = "SignOut" Then
            ClearCurrentLogin()
            Await DisplayNotificationsAsync()
            Return
        End If

        If UserIsLoggedIn() OrElse CheckForSavedSession() Then
            Response.Redirect("~/Home/", False)
        Else
            ClearCurrentLogin()
            Await DisplayNotificationsAsync()
        End If
    End Sub

    Private Async Function DisplayNotificationsAsync() As Task
        Dim notifications As List(Of OrgNotification) = Await GetNotificationsAsync()

        If notifications.Count > 0 Then
            Dim div As HtmlGenericControl = FormatNotificationsDiv(notifications)
            OrgNotifications.Controls.Add(div)
        End If
    End Function

    Private Sub ClearCurrentLogin()
        Session.Clear()
        ClearAllCookies()
    End Sub


    Private Shared Function CheckForSavedSession() As Boolean

        Dim series As String = GetCookie(UserSessionCookie.Series)
        Dim token As String = GetCookie(UserSessionCookie.Token)

        If String.IsNullOrEmpty(series) OrElse String.IsNullOrEmpty(token) Then
            Return False
        End If

        Dim userSession As New UserSession(series, token)
        Dim gecoUser As New GecoUser

        If GetSavedUserSession(userSession, gecoUser) Then
            SessionAdd(GecoSession.CurrentUser, gecoUser)
            CreateSessionCookie(userSession)

            Return True
        End If

        Return False
    End Function

End Class
