Imports System.ComponentModel

Namespace GecoModels
    Public Class InvoiceItem

        <Browsable(False)>
        Public Property InvoiceItemID As Integer
        <Browsable(False)>
        Public Property InvoiceID As Integer?
        <Browsable(False)>
        Public Property InvoiceCategory As InvoiceCategory
        <Browsable(False)>
        Public Property FacilityID As ApbFacilityId
        Public Property Amount As Decimal
        <Browsable(False)>
        Public Property ItemStatus As InvoiceItemStatus
        <Browsable(False)>
        Public Property RateCategory As FeeRateCategory
        <DisplayName("Category")>
        Public ReadOnly Property RateCategoryDisplay As String
            Get
                Return RateCategory.GetDescription
            End Get
        End Property
        <DisplayName("Application #")>
        Public Property ApplicationID As Integer?
        <DisplayName("Fee Year")>
        Public Property FeeYear As Integer?

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

    End Class
End Namespace