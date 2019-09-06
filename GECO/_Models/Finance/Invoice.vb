Imports System.ComponentModel

Namespace GecoModels
    Public Class Invoice

        <DisplayName("Invoice ID")>
        Public Property InvoiceID As Integer
        <Browsable(False)>
        Public Property InvoiceGuid As Guid
        <DisplayName("Facility ID")>
        Public Property FacilityID As ApbFacilityId
        <DisplayName("Facility Name")>
        Public Property FacilityName As String
        <Browsable(False)>
        Public Property FacilityAddress() As Address
        <Browsable(False)>
        Public Property InvoiceCategory As InvoiceCategory
        <Browsable(False)>
        Public Property InvoiceType As InvoiceType
        <DisplayName("Total Amount Due")>
        Public Property TotalAmountDue As Decimal
        <DisplayName("Payments Applied")>
        Public Property PaymentsApplied As Decimal
        <DisplayName("Current Balance")>
        Public ReadOnly Property CurrentBalance As Decimal
            Get
                Return TotalAmountDue - PaymentsApplied
            End Get
        End Property
        <DisplayName("Invoice Date")>
        Public Property InvoiceDate As Date
        <DisplayName("Due Date")>
        Public Property DueDate As Date
        <DisplayName("Comment")>
        Public Property Comment As String
        <DisplayName("Voided")>
        Public Property Voided As Boolean = False
        <DisplayName("Date Voided")>
        Public Property VoidedDate As Date?
        <DisplayName("Settlement Status")>
        Public Property SettlementStatus As String
        ' <DisplayName("Settlement Date")>
        ' Public Property FullSettlementDate As Date?
        <Browsable(False)>
        Public Property CeasedCollections As Boolean = False
        <DisplayName("Permit Application")>
        Public Property ApplicationID As Integer?
        <DisplayName("Emissions Fee Year")>
        Public Property FeeYear As Integer?
        Public Property FeeYearTotalEmissionFees As Decimal?
        Public Property FeeYearTotalAdminFee As Decimal?
        <Browsable(False)>
        Public Property InvoiceCategoryID As Char
            Get
                Select Case InvoiceCategory
                    Case InvoiceCategory.EmissionsFees
                        Return "E"c
                    Case InvoiceCategory.PermitApplicationFees
                        Return "P"c
                    Case Else
                        Return Nothing
                End Select
            End Get
            Set(value As Char)
                Select Case value
                    Case "E"c
                        InvoiceCategory = InvoiceCategory.EmissionsFees
                    Case "P"c
                        InvoiceCategory = InvoiceCategory.PermitApplicationFees
                    Case Else
                End Select
            End Set
        End Property

        Public ReadOnly Property InvoiceItems As New List(Of InvoiceItem)
        Public ReadOnly Property DepositsApplied As New List(Of DepositApplied)

        <DisplayName("Category")>
        Public ReadOnly Property InvoiceCategoryDisplay As String
            Get
                Return InvoiceCategory.GetDescription
            End Get
        End Property

    End Class

    Public Enum InvoiceCategory
        <Description("Emission Fees")>
        EmissionsFees
        <Description("Application Fees")>
        PermitApplicationFees
    End Enum

    Public Enum InvoiceItemStatus
        Pending = 0
        Canceled = 1
        Invoiced = 2
    End Enum

    Public Enum FeeRateCategory
        <Description("Permit Application Fee")>
        PermitApplication = 0
        <Description("Expedited Review Fee")>
        ExpeditedReview = 1
        <Description("Emission Fee")>
        Other = 2
    End Enum

End Namespace