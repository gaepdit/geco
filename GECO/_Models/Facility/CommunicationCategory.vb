Namespace GecoModels.Facility
    Public Structure CommunicationCategory

        Private Shared ReadOnly Prefs As CommunicationPrefsConfigSection = CType(ConfigurationManager.GetSection("communicationPrefs"), CommunicationPrefsConfigSection)

        ' Instances
        Public Shared Property PermitFees As New CommunicationCategory("Fees", "Permit Fees", Prefs.PermitFees)
        Public Shared Property PermitApplications As New CommunicationCategory("Permits", "Permit Applications", Prefs.PermitApplications)
        Public Shared Property EmissionsInventory As New CommunicationCategory("EI", "Emissions Inventory", Prefs.EmissionsInventory)
        Public Shared Property TestingMonitoring As New CommunicationCategory("Testing", "Testing and Monitoring", Prefs.TestingMonitoring)

        ' Implementation
        Public ReadOnly Name As String
        Public ReadOnly Description As String
        Public ReadOnly CommunicationOption As CommunicationOptionType

        Private Sub New(category As String, description As String, preferenceEnabled As CommunicationOptionType)
            Name = category
            Me.Description = description
            CommunicationOption = preferenceEnabled
        End Sub

        Public Shared ReadOnly Property FromName As New Dictionary(Of String, CommunicationCategory) From
        {
            {PermitFees.Name, PermitFees},
            {PermitApplications.Name, PermitApplications},
            {EmissionsInventory.Name, EmissionsInventory},
            {TestingMonitoring.Name, TestingMonitoring}
        }

        Public Shared ReadOnly AllCategories As New List(Of CommunicationCategory) From
            {PermitFees, PermitApplications, EmissionsInventory, TestingMonitoring}

        Public Shared Function IsValidCategory(category As String) As Boolean
            Return Not String.IsNullOrEmpty(category) AndAlso FromName.ContainsKey(category)
        End Function

    End Structure

    Class CommunicationPrefsConfigSection
        Inherits ConfigurationSection

        <ConfigurationProperty(NameOf(PermitFees), IsRequired:=True)>
        Public ReadOnly Property PermitFees As CommunicationOptionType
            Get
                Return CType(Me(NameOf(PermitFees)), CommunicationOptionType)
            End Get
        End Property

        <ConfigurationProperty(NameOf(PermitApplications), IsRequired:=True)>
        Public ReadOnly Property PermitApplications As CommunicationOptionType
            Get
                Return CType(Me(NameOf(PermitApplications)), CommunicationOptionType)
            End Get
        End Property

        <ConfigurationProperty(NameOf(EmissionsInventory), IsRequired:=True)>
        Public ReadOnly Property EmissionsInventory As CommunicationOptionType
            Get
                Return CType(Me(NameOf(EmissionsInventory)), CommunicationOptionType)
            End Get
        End Property

        <ConfigurationProperty(NameOf(TestingMonitoring), IsRequired:=True)>
        Public ReadOnly Property TestingMonitoring As CommunicationOptionType
            Get
                Return CType(Me(NameOf(TestingMonitoring)), CommunicationOptionType)
            End Get
        End Property
    End Class

    Public Enum CommunicationOptionType
        Mail
        Electronic
        FacilityChoice
    End Enum

End Namespace
