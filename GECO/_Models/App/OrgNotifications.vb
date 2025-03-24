Imports System.Net.Http
Imports System.Runtime.Caching
Imports System.Text.Json
Imports System.Text.Json.Serialization
Imports System.Threading.Tasks

Friend Module OrgNotifications


    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification:="Instantiated by API deserializer")>
    Public Class OrgNotification
        <JsonPropertyName("message")>
        Public Property Message As String
    End Class

    Public Async Function GetNotificationsAsync() As Task(Of List(Of OrgNotification))
        Const cacheKey As String = "OrgNotifications"
        Dim orgNotificationsApiUrl As String = ConfigurationManager.AppSettings("OrgNotificationsApiUrl")

        If String.IsNullOrEmpty(orgNotificationsApiUrl) Then
            Return New List(Of OrgNotification)
        End If

        Dim cache As ObjectCache = MemoryCache.Default
        Dim notifications As List(Of OrgNotification) = CType(cache.Get(cacheKey), List(Of OrgNotification))

        If notifications Is Nothing Then
            notifications = Await FetchNotificationsFromApi(orgNotificationsApiUrl)
            cache.Add(cacheKey, notifications, DateTimeOffset.Now.AddHours(1))
        End If

        Return notifications
    End Function

    Private Async Function FetchNotificationsFromApi(orgNotificationsApiUrl As String) As Task(Of List(Of OrgNotification))
        Try
            Using client As New HttpClient()
                Dim response As HttpResponseMessage = Await client.GetAsync(orgNotificationsApiUrl)
                response.EnsureSuccessStatusCode()
                Return JsonSerializer.Deserialize(Of List(Of OrgNotification))(Await response.Content.ReadAsStringAsync())
            End Using
        Catch
            ' Intentionally left empty. If the API is unresponsive or other error occurs, no notifications will be displayed.
            Return New List(Of OrgNotification)
        End Try
    End Function

    Public Function FormatNotificationsDiv(notifications As List(Of OrgNotification)) As HtmlGenericControl
        Dim div As New HtmlGenericControl("div")
        div.Attributes("class") = "announcement announcement-severe"
        div.InnerHtml = "<h2>Notice</h2>"
        For Each notification As OrgNotification In notifications
            div.InnerHtml += $"<p>{notification.Message}</p>"
        Next
        div.InnerHtml += "<p>Please refer to the <a href=""https://status.gaepd.org/"">EPD-IT status page</a> for updates.</p>"
        Return div
    End Function

End Module
