Namespace GecoModels.Facility
    Public Class CommunicationCategory

        ' Instances
        Public Shared Property Fees As New CommunicationCategory("Fees")
        Public Shared Property PermitApplications As New CommunicationCategory("Permit Applications")
        Public Shared Property EmissionsInventory As New CommunicationCategory("Emissions Inventory")
        Public Shared Property EmissionsStatement As New CommunicationCategory("Emissions Statement")
        Public Shared Property TestingMonitoring As New CommunicationCategory("Testing and Monitoring")

        ' Implementation
        Public ReadOnly Name As String

        Private Sub New(category As String)
            Name = category
        End Sub

        Public Shared Property FromName As New Dictionary(Of String, CommunicationCategory) From
        {
            {Fees.Name, Fees},
            {PermitApplications.Name, PermitApplications},
            {EmissionsInventory.Name, EmissionsInventory},
            {EmissionsStatement.Name, EmissionsStatement},
            {TestingMonitoring.Name, TestingMonitoring}
        }

        Public Shared AllCategories As New List(Of CommunicationCategory) From
            {Fees, PermitApplications, EmissionsInventory, EmissionsStatement, TestingMonitoring}

    End Class
End Namespace
