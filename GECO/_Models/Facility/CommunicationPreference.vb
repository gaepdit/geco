Namespace GecoModels.Facility
    Public Class CommunicationPreference

        ' "Values"
        Public Shared Property Mail As New CommunicationPreference("Mail", "By mail only.")
        Public Shared Property Electronic As New CommunicationPreference("Electronic", "Electronic communication only.")
        Public Shared Property Both As New CommunicationPreference("Both", "Both electronic and mail.")

        ' Implementation
        Public ReadOnly Name As String
        Public ReadOnly Display As String

        Private Sub New(category As String, display As String)
            Name = category
            Me.Display = display
        End Sub

        Public Shared Property FromName As New Dictionary(Of String, CommunicationPreference) From
        {
            {Mail.Name, Mail},
            {Electronic.Name, Electronic},
            {Both.Name, Both}
        }

    End Class
End Namespace
