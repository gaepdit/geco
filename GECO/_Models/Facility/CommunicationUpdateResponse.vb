Namespace GecoModels.Facility
    Public Class CommunicationUpdateResponse

        Public Sub New()
            CategoryUpdates = New Dictionary(Of CommunicationCategory, CategoryUpdateStatus)
        End Sub

        Public Property UpdateRequired As Boolean ' Indicates contact information is incomplete and must be updated
        Public Property ResponseRequired As Boolean ' Indicates facility response is required (either routine review or mandatory update)
        Public ReadOnly Property CategoryUpdates As New Dictionary(Of CommunicationCategory, CategoryUpdateStatus)

        Public Enum CategoryUpdateStatus
            NoUpdateRequired = 0
            RoutineConfirmationRequired = 1
            AddressIncomplete = 2
            EmailMissing = 3
            AddressIncompleteAndEmailMissing = 4
        End Enum

        Public Sub AddCategoryUpdate(category As CommunicationCategory, status As CategoryUpdateStatus)
            If status >= 1 Then ResponseRequired = True
            If status > 1 Then UpdateRequired = True
            CategoryUpdates.Add(category, status)
        End Sub

    End Class
End Namespace
