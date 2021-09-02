Imports GECO.GecoModels.Facility

Public Class FacilityCommunicationInfo

    Public Property Preference As FacilityCommunicationPreference
    Public Property Mail As MailContact
    Public Property Emails As List(Of EmailContact)

    Public ReadOnly Property EmailList As List(Of String)
        Get
            Return Emails.Select(Function(e) e.Email).ToList()
        End Get
    End Property

    Public Sub RemoveEmail(email As String)
        Emails.RemoveAll(Function(e) e.Email = email)
    End Sub

End Class
