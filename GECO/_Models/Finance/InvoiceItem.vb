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
        <DisplayName("Description")>
        Public ReadOnly Property InvoiceItemDescription As String
            Get
                Select Case InvoiceCategory
                    Case InvoiceCategory.EmissionsFees
                        Return FeeYear.ToString & " " & InvoiceCategory.GetDescription & ", " & InvoiceType.Description

                    Case InvoiceCategory.PermitApplicationFees
                        Return RateCategory.GetDescription

                    Case Else
                        Return "Unknown"
                End Select
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
                    End Select
            End Set
        End Property
        Public Property InvoiceType As InvoiceType

    End Class
End Namespace
