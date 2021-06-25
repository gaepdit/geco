Namespace GecoModels.Facility
    Public Structure CommunicationPreference

        ' "Values"
        Public Shared Property Mail As New CommunicationPreference("Mail", "By mail only.")
        Public Shared Property Electronic As New CommunicationPreference("Electronic", "Electronic communication only.")
        Public Shared Property Both As New CommunicationPreference("Both", "Both electronic and mail.")

        ' Implementation
        Public ReadOnly Name As String
        Public ReadOnly Description As String

        Private Sub New(category As String, description As String)
            Name = category
            Me.Description = description
        End Sub

        Public Shared Property FromName As New Dictionary(Of String, CommunicationPreference) From
        {
            {Mail.Name, Mail},
            {Electronic.Name, Electronic},
            {Both.Name, Both}
        }

        Public Shared Function IsValidPreference(preference As String) As Boolean
            Return Not String.IsNullOrEmpty(preference) AndAlso FromName.ContainsKey(preference)
        End Function

        Public ReadOnly Property IncludesElectronic As Boolean
            Get
                Return Not Name.Equals(Mail.Name)
            End Get
        End Property

    End Structure
End Namespace
