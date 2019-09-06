Imports System.ComponentModel

Namespace GecoModels
    Public Class DepositApplied

        <Browsable(False)>
        Public Property DepositAppliedID As Integer
        <Browsable(False)>
        Public Property DepositID As Integer
        <DisplayName("Invoice ID")>
        Public Property InvoiceID As Integer
        <DisplayName("Facility ID")>
        Public Property FacilityID As ApbFacilityId
        <DisplayName("Amount Applied")>
        Public Property AmountApplied As Decimal
        <Browsable(False)>
        Public Property DepositDate As Date
        <DisplayName("Deposit #")>
        Public Property DepositNumber As String
        <DisplayName("Check #")>
        Public Property CheckNumber As String
        <DisplayName("Batch #")>
        Public Property BatchNumber As String
        <DisplayName("Credit Card Confirmation #")>
        Public Property CreditCardConf As String
        <Browsable(False)>
        Public Property InvoiceVoided As Boolean
        <Browsable(False)>
        Public Property DepositDeleted As Boolean

        Public ReadOnly Property Description As String
            Get
                Return ConcatNonEmptyStrings(
                    ": ",
                    {
                        DepositDate.ToString(ShortishDateFormat),
                        ConcatNonEmptyStrings(
                            ", ",
                            {
                                If(String.IsNullOrEmpty(DepositNumber), Nothing, "Deposit #" & DepositNumber),
                                If(String.IsNullOrEmpty(CheckNumber), Nothing, "Check #" & CheckNumber),
                                If(String.IsNullOrEmpty(CreditCardConf), Nothing, "Credit Card Conf #" & CreditCardConf),
                                If(String.IsNullOrEmpty(InvoiceID), Nothing, "Invoice #" & InvoiceID)
                            })
                    })
            End Get
        End Property

    End Class
End Namespace
