Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Namespace EpdEvents
    Partial Class Events_Default
        Inherits Page

        Private ReadOnly client As New HttpClient()

        Private Async Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

            If IsPostBack Then Return
            Dim orgEvents As EventsList = Await GetActiveEvents()
            lvEventList.DataSource = orgEvents.events
            lvEventList.DataBind()

        End Sub



        ''' <summary>
        ''' Uses the Eventbrite API to retrieve future events for the organization.
        ''' Eventbrite Documentation: https://www.eventbrite.com/platform/docs/events
        ''' API Reference: https://www.eventbrite.com/platform/api
        ''' </summary>
        ''' <returns></returns>
        <CodeAnalysis.SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification:="External API")>
        Public Async Function GetActiveEvents() As Task(Of EventsList)

            Dim key As String = ConfigurationManager.AppSettings("EventbritePrivateToken")
            Dim orgId As String = ConfigurationManager.AppSettings("EventbriteOrgId")
            Dim apiBase As New Uri("https://www.eventbriteapi.com/v3")
            Dim apiCall As String = $"{apiBase}/organizations/{orgId}/events/?expand=ticket_availability&status=live&time_filter=current_future&token={key}"
            Dim jsonResponse As String = Await client.GetStringAsync(apiCall)

            Return JsonConvert.DeserializeObject(Of EventsList)(jsonResponse)

        End Function

    End Class
End Namespace
