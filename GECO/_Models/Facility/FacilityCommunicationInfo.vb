Imports GECO.GecoModels.Facility

Public Class FacilityCommunicationInfo

    Public Property Preference As FacilityCommunicationPreference
    Public Property Mail As MailContact
    Public Property Emails As List(Of EmailContact)

    Public ReadOnly Property AnyVerifiedEmails As Boolean
        Get
            Return Emails.Any(Function(e) e.Verified)
        End Get
    End Property

    Public ReadOnly Property AnyUnverifiedEmails As Boolean
        Get
            Return Emails.Any(Function(e) Not e.Verified)
        End Get
    End Property

    Public ReadOnly Property VerifiedEmails As List(Of String)
        Get
            Return Emails.Where(Function(e) e.Verified).Select(Function(e) e.Email).ToList()
        End Get
    End Property

    Public ReadOnly Property UnverifiedEmails As List(Of String)
        Get
            Return Emails.Where(Function(e) Not e.Verified).Select(Function(e) e.Email).ToList()
        End Get
    End Property

    Public Function ContainsEmail(email As String) As Boolean
        Return Emails.Any(Function(e) e.Email = email)
    End Function

    Public Sub RemoveEmail(email As String)
        Emails.RemoveAll(Function(e) e.Email = email)
    End Sub

End Class
