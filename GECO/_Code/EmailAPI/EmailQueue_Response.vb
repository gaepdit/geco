Imports System.ComponentModel

Namespace EmailApi
    Public Class EmailQueueResponseBody
        Public Property Status As String
        Public Property Count As Integer
        Public Property BatchId As String
    End Class

    Public Class EmailQueueApiResponse
        Public Property Status As String
        Public Property Body As EmailQueueResponseBody

        Public Shared ReadOnly Property Failed As EmailQueueApiResponse = New EmailQueueApiResponse With {
            .Status = "Failed",
            .Body = Nothing
        }

        Public Shared Function Ok(body As EmailQueueResponseBody) As EmailQueueApiResponse
            Return New EmailQueueApiResponse With {
                .Status = body.Status,
                .Body = body
            }
        End Function
    End Class

    Public Class EmailTask
        Public Property Counter As Integer
        Public Property Status As String
        Public Property AttemptedAt As Date
        Public Property From As String
        Public Property Subject As String
        Public Property Recipients As List(Of String)
        Public Property FailureReason As String
    End Class

    Public Class EmailBatchDetails
        Public Property Status As String
        Public Property Emails As List(Of EmailTask)

        Public Shared ReadOnly Property Failed As EmailBatchDetails = New EmailBatchDetails With {
            .Status = "Failed",
            .Emails = Nothing
        }

        Public Shared Function Ok(emails As List(Of EmailTask)) As EmailBatchDetails
            Return New EmailBatchDetails With {
                .Status = "Success",
                .Emails = emails
            }
        End Function
    End Class

    <Serializable>
    Public Class EmailTaskViewModel
        Public Sub New(email As EmailTask)
            Id = email.Counter
            Status = email.Status
            Sent = If(email.AttemptedAt = Nothing, email.AttemptedAt.ToLocalTime(), CType(Nothing, Date?))
            Subject = email.Subject
            Recipients = String.Join(", ", email.Recipients)
            FailureReason = IfEmpty(email.FailureReason, "N/A")
        End Sub

        Public Property Id As Integer
        Public Property Status As String
        Public Property Sent As Date?
        Public Property Subject As String
        Public Property Recipients As String

        <DisplayName("Failure Reason")>
        Public Property FailureReason As String

        Private Shared Function IfEmpty(value As String, defaultValue As String) As String
            Return If(String.IsNullOrEmpty(value), defaultValue, value)
        End Function
    End Class
End Namespace
