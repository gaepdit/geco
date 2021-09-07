Namespace GecoModels.Facility
    Public Class CommunicationUpdateResponse

        Public Sub New()
            CategoryUpdates = New Dictionary(Of CommunicationCategory, CategoryUpdateStatus)
        End Sub

        Public Property RoutineConfirmationRequired As Boolean
        Public Property UpdateRequired As Boolean

        Public ReadOnly Property ResponseRequired As Boolean
            Get
                Return RoutineConfirmationRequired OrElse UpdateRequired
            End Get
        End Property

        Public ReadOnly Property CategoryUpdates As New Dictionary(Of CommunicationCategory, CategoryUpdateStatus)

        Public Enum CategoryUpdateStatus
            NoUpdateRequired = 0
            RoutineConfirmationRequired = 1
            AddressIncomplete = 2
            EmailMissing = 3
            AddressIncompleteAndEmailMissing = 4
        End Enum

        Public Sub Add(category As CommunicationCategory, status As CategoryUpdateStatus)
            If status = 1 Then
                RoutineConfirmationRequired = True
            ElseIf status > 1 Then
                UpdateRequired = True
            End If

            CategoryUpdates.Add(category, status)
        End Sub

    End Class
End Namespace
