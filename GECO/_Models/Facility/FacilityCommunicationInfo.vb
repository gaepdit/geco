Imports GECO.GecoModels.Facility

Public Class FacilityCommunicationInfo

    Public Property Preference As FacilityCommunicationPreference
    Public Property Mail As FacilityMailContact
    Public Property Emails As List(Of FacilityEmailContact)

    Public ReadOnly Property AnyVerifiedEmails As Boolean
        Get
            Return Emails.Any(Function(e) e.Verified)
        End Get
    End Property

End Class
