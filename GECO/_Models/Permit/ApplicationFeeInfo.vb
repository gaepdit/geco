Namespace GecoModels
    Public Class ApplicationFeeInfo

        ' Permit application
        Public Property ApplicationID As Integer
        Public Property ApplicationWithdrawn As Boolean

        ' Permit application fees
        Public Property ApplicationFeeApplies As Boolean
        Public Property ApplicationFeeDescription As String
        Public Property ApplicationFeeAmount As Decimal

        ' Expedited review fees
        Public Property ExpeditedFeeApplies As Boolean
        Public Property ExpeditedFeeDescription As String
        Public Property ExpeditedFeeAmount As Decimal

        ' Invoicing
        Public Property FeeDataFinalized As Boolean
        Public Property DateFeeDataFinalized As Date?
        Public Property DateFacilityNotifiedOfFees As Date?

        Public ReadOnly Property TotalFeeAmount As Decimal
            Get
                Return ApplicationFeeAmount + ExpeditedFeeAmount
            End Get
        End Property

    End Class
End Namespace