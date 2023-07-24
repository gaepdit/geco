Namespace GecoModels.Facility
    Public Class FacilityCommunicationPreference

        Public Property Id As Guid
        Public Property CommunicationPreference As CommunicationPreference = CommunicationPreference.Mail
        Public Property IsConfirmed As Boolean = False

    End Class
End Namespace
