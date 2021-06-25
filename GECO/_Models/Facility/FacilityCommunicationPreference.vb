Namespace GecoModels.Facility
    Public Class FacilityCommunicationPreference

        Public Property Id As Guid = Nothing
        Public Property CommunicationPreference As CommunicationPreference = CommunicationPreference.Mail
        Public Property IsConfirmed As Boolean = False
        Public Property InitialConfirmationDate As DateTimeOffset? = Nothing
        Public Property LatestConfirmationDate As DateTimeOffset? = Nothing

    End Class
End Namespace
