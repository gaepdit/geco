<Serializable()>
Public Class AnnualFeeCalc
    Public Property FeeRates As AnnualFeeRates
    Public Property Emissions As New AnnualFeeEmissions()

    Public Property CountyCode As String
    Public Property EntryDate As Date

    Public Property RulePart70Applies As Boolean
    Public Property RuleSmApplies As Boolean
    Public Property RuleNspsApplies As Boolean

    Private ReadOnly Property DaysDelayed As Integer
        Get
            If FeeRates Is Nothing OrElse FeeRates.AdminFeeDate = Nothing Then
                Return 0
            End If

            If EntryDate <= FeeRates.AdminFeeDate Then
                Return 0
            End If

            Return DateDiff(DateInterval.Day, FeeRates.AdminFeeDate, EntryDate)
        End Get
    End Property
    Public ReadOnly Property RuleNonattainmentAreaApplies As Boolean
        Get
            Dim nonattainmentCounties As New List(Of String) From {"057", "063", "067", "077", "089", "097", "113", "117", "121", "135", "151", "223", "247"}
            Return nonattainmentCounties.Contains(CountyCode)
        End Get
    End Property
    Public ReadOnly Property CalcVocFee As Decimal
        Get
            Return CalculatePollutantFee(Emissions.VocTons)
        End Get
    End Property
    Public ReadOnly Property CalcNoxFee As Decimal
        Get
            Return CalculatePollutantFee(Emissions.NoxTons)
        End Get
    End Property
    Public ReadOnly Property CalcPmFee As Decimal
        Get
            Return CalculatePollutantFee(Emissions.PmTons)
        End Get
    End Property
    Public ReadOnly Property CalcSo2Fee As Decimal
        Get
            Return CalculatePollutantFee(Emissions.So2Tons)
        End Get
    End Property

    Public ReadOnly Property CalcEmissionFee As Decimal
        Get
            Return CalcVocFee + CalcNoxFee + CalcPmFee + CalcSo2Fee
        End Get
    End Property
    Public ReadOnly Property CalcPart70Fee As Decimal
        Get
            If RulePart70Applies Then
                Return Math.Max(FeeRates.Part70MinFee, CalcEmissionFee)
            End If
            Return 0
        End Get
    End Property
    Public ReadOnly Property CalcMaintenanceFee As Decimal
        Get
            Return If(RulePart70Applies, FeeRates.Part70MaintenanceFee, 0)
        End Get
    End Property
    Public ReadOnly Property CalcSmFee As Decimal
        Get
            If RulePart70Applies Then
                Return 0
            End If
            Return If(RuleSmApplies, FeeRates.SmFeeRate, 0)
        End Get
    End Property
    Public ReadOnly Property CalcNspsFee As Decimal
        Get
            Return If(RuleNspsApplies, FeeRates.NspsFeeRate, 0)
        End Get
    End Property
    Public ReadOnly Property CalcFeeSubtotal As Decimal
        Get
            Return CalcPart70Fee + CalcSmFee + CalcNspsFee + CalcMaintenanceFee
        End Get
    End Property
    Public ReadOnly Property CalcAdminFee As Decimal
        Get
            If FeeRates Is Nothing OrElse FeeRates.AdminFeeRate = Nothing Then
                Return 0D
            End If

            Return DaysDelayed * CalcFeeSubtotal * FeeRates.AdminFeeRate / 100
        End Get
    End Property
    Public ReadOnly Property CalcTotalFee As Decimal
        Get
            Return CalcFeeSubtotal + CalcAdminFee
        End Get
    End Property

    Private Function CalculatePollutantFee(tons As Integer) As Decimal
        If FeeRates Is Nothing OrElse Emissions Is Nothing Then Return 0D
        If RuleNonattainmentAreaApplies AndAlso tons <= FeeRates.NonattainmentThreshold Then Return 0D
        If Not RuleNonattainmentAreaApplies AndAlso tons <= FeeRates.AttainmentThreshold Then Return 0D
        Return tons * FeeRates.PerTonRate
    End Function
End Class

<Serializable()>
Public Class AnnualFeeRates
    Public Property PerTonRate As Decimal
    Public Property Part70MinFee As Decimal
    Public Property Part70MaintenanceFee As Decimal
    Public Property SmFeeRate As Decimal
    Public Property NspsFeeRate As Decimal
    Public Property AdminFeeRate As Decimal
    Public Property AdminFeeDate As Date
    Public Property DueDate As Date
    Public Property FirstQuarterDueDate As Date
    Public Property SecondQuarterDueDate As Date
    Public Property ThirdQuarterDueDate As Date
    Public Property FourthQuarterDueDate As Date
    Public Property AttainmentThreshold As Integer
    Public Property NonattainmentThreshold As Integer
End Class

<Serializable()>
Public Class AnnualFeeEmissions
    Public Property VocTons As Integer = 0
    Public Property NoxTons As Integer = 0
    Public Property PmTons As Integer = 0
    Public Property So2Tons As Integer = 0
    Public ReadOnly Property Total As Integer
        Get
            Return VocTons + NoxTons + PmTons + So2Tons
        End Get
    End Property
End Class
