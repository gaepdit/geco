Namespace GecoModels.Facility
    Public Structure CommunicationCategory

        Private Shared ReadOnly prefs As CommunicationPrefsConfigSection =
            CType(ConfigurationManager.GetSection("communicationPrefs"),
            CommunicationPrefsConfigSection)

        ' Instances
        Public Shared Property Fees As New CommunicationCategory("Fees", "Permit Fees", prefs.FeesEnabled)
        Public Shared Property PermitApplications As New CommunicationCategory("Permits", "Permit Applications", prefs.PermitApplicationsEnabled)
        Public Shared Property EmissionsInventory As New CommunicationCategory("EI", "Emissions Inventory", prefs.EmissionsInventoryEnabled)
        Public Shared Property EmissionsStatement As New CommunicationCategory("ES", "Emissions Statement", prefs.EmissionsStatementEnabled)
        Public Shared Property TestingMonitoring As New CommunicationCategory("Testing", "Testing and Monitoring", prefs.TestingMonitoringEnabled)

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

    Class CommunicationPrefsConfigSection
        Inherits ConfigurationSection

        <ConfigurationProperty(NameOf(FeesEnabled), IsRequired:=True)>
        Public ReadOnly Property FeesEnabled As Boolean
            Get
                Return CType(Me(NameOf(FeesEnabled)), Boolean)
            End Get
        End Property

        <ConfigurationProperty(NameOf(PermitApplicationsEnabled), IsRequired:=True)>
        Public ReadOnly Property PermitApplicationsEnabled As Boolean
            Get
                Return CType(Me(NameOf(PermitApplicationsEnabled)), Boolean)
            End Get
        End Property

        <ConfigurationProperty(NameOf(EmissionsInventoryEnabled), IsRequired:=True)>
        Public ReadOnly Property EmissionsInventoryEnabled As Boolean
            Get
                Return CType(Me(NameOf(EmissionsInventoryEnabled)), Boolean)
            End Get
        End Property

        <ConfigurationProperty(NameOf(EmissionsStatementEnabled), IsRequired:=True)>
        Public ReadOnly Property EmissionsStatementEnabled As Boolean
            Get
                Return CType(Me(NameOf(EmissionsStatementEnabled)), Boolean)
            End Get
        End Property

        <ConfigurationProperty(NameOf(TestingMonitoringEnabled), IsRequired:=True)>
        Public ReadOnly Property TestingMonitoringEnabled As Boolean
            Get
                Return CType(Me(NameOf(TestingMonitoringEnabled)), Boolean)
            End Get
        End Property
    End Class

End Namespace
