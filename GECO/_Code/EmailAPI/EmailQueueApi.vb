Imports System.Net.Http
Imports System.Text.Json
Imports System.Threading.Tasks

Namespace EmailAPI
    Public Module EmailQueueApi

        Private Const SendEndpoint As String = "add"
        Private Const SendForBatchEndpoint As String = "add-to-batch"
        Private Const BatchDetailsEndpoint As String = "batch-details"
        'Private Const BatchStatusEndpoint As String = "batch-status"

        Private Async Function CallEmailApiAsync(Of T)(payload As Object, endpoint As String) As Task(Of T)
            Dim _baseUri As String = ConfigurationManager.AppSettings("EmailQueueApiUrl")
            Dim _clientID As String = ConfigurationManager.AppSettings("EmailQueueClientId")
            Dim _apiKey As String = ConfigurationManager.AppSettings("EmailQueueApiKey")

            Dim requestContent As New StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json")
            Dim responseBody As String

            Using client As New HttpClient()
                client.DefaultRequestHeaders.Add("X-API-Key", _apiKey)
                client.DefaultRequestHeaders.Add("X-Client-ID", _clientID)

                Dim response As HttpResponseMessage = Await client.PostAsync(New Uri(_baseUri & endpoint), requestContent)
                response.EnsureSuccessStatusCode()
                responseBody = Await response.Content.ReadAsStringAsync()

                Return JsonSerializer.Deserialize(Of T)(responseBody)
            End Using
        End Function

        Public Function QueueEmail(email As Email) As Task(Of EmailQueueApiResponse)
            Return QueueEmail(New List(Of Email) From {email})
        End Function

        Public Async Function QueueEmail(emails As List(Of Email)) As Task(Of EmailQueueApiResponse)
            Try
                Dim response As EmailQueueResponseBody = Await CallEmailApiAsync(Of EmailQueueResponseBody)(emails, SendEndpoint)
                Return EmailQueueApiResponse.Ok(response)
            Catch e As Exception
                Return EmailQueueApiResponse.Failed
            End Try
        End Function

        Public Async Function QueueEmailWithBatchID(emails As List(Of Email), batchId As Guid) As Task(Of EmailQueueApiResponse)
            Dim emailForBatchRequest As New EmailForBatchRequest() With {
                .Emails = emails,
                .BatchId = batchId
            }

            Try
                Dim response As EmailQueueResponseBody = Await CallEmailApiAsync(Of EmailQueueResponseBody)(emailForBatchRequest, SendForBatchEndpoint)
                Return EmailQueueApiResponse.Ok(response)
            Catch e As Exception
                Return EmailQueueApiResponse.Failed
            End Try
        End Function

        Public Async Function GetBatchDetails(batchId As Guid) As Task(Of EmailBatchDetails)
            Dim batchRequest As New BatchRequest() With {.BatchId = batchId}

            Try
                Dim response As List(Of EmailTask) = Await CallEmailApiAsync(Of List(Of EmailTask))(batchRequest, BatchDetailsEndpoint)
                Return EmailBatchDetails.Ok(response)
            Catch e As Exception
                Return EmailBatchDetails.Failed
            End Try
        End Function

    End Module
End Namespace
