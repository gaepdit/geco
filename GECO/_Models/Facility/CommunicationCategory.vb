Namespace GecoModels.Facility
    Public Structure CommunicationCategory

        ' Instances
        Public Shared Property Fees As New CommunicationCategory("Fees", "Permit Fees", True)
        Public Shared Property PermitApplications As New CommunicationCategory("Permits", "Permit Applications", False)
        Public Shared Property EmissionsInventory As New CommunicationCategory("EI", "Emissions Inventory", False)
        Public Shared Property EmissionsStatement As New CommunicationCategory("ES", "Emissions Statement", False)
        Public Shared Property TestingMonitoring As New CommunicationCategory("Testing", "Testing and Monitoring", False)

        ' Implementation
        Public ReadOnly Name As String
        Public ReadOnly Description As String
        Public ReadOnly CommunicationPreferenceEnabled As Boolean

        Private Sub New(category As String, description As String, preferenceEnabled As Boolean)
            Name = category
            Me.Description = description
            CommunicationPreferenceEnabled = preferenceEnabled
        End Sub

        Public Shared ReadOnly Property FromName As New Dictionary(Of String, CommunicationCategory) From
        {
            {Fees.Name, Fees},
            {PermitApplications.Name, PermitApplications},
            {EmissionsInventory.Name, EmissionsInventory},
            {EmissionsStatement.Name, EmissionsStatement},
            {TestingMonitoring.Name, TestingMonitoring}
        }

        Public Shared ReadOnly AllCategories As New List(Of CommunicationCategory) From
            {Fees, PermitApplications, EmissionsInventory, EmissionsStatement, TestingMonitoring}

        Public Shared Function IsValidCategory(category As String) As Boolean
            Return Not String.IsNullOrEmpty(category) AndAlso FromName.ContainsKey(category)
        End Function

    End Structure
End Namespace
