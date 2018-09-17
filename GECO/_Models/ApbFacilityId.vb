Imports System.Diagnostics

Namespace GecoModels
    Public Class ApbFacilityId
        Implements IEquatable(Of ApbFacilityId)

#Region " Constants "

        Private Const GA_STATE_CODE As String = "GA"
        Private Const GA_STATE_NUMERIC_CODE As String = "13"
        Private Const GA_EPA_REGION_CODE As String = "04"
        Private Const AirsNumberPattern As String = "^(04-?13-?)?\d{3}-?\d{5}$"

#End Region

#Region " Constructor "

        Private ReadOnly airsNumber As String

        <DebuggerStepThrough>
        Public Sub New(airsNumber As String)
            If Not IsValidAirsNumberFormat(airsNumber) Then
                Throw New ArgumentException(String.Format("{0} is not a valid AIRS number.", airsNumber))
            End If

            Me.airsNumber = GetNormalizedAirsNumber(airsNumber)
        End Sub

#End Region

#Region " Read-only properties "

        ''' <summary>
        ''' Returns APB facility ID as an eight-character string in the form "00000000"
        ''' </summary>
        Public Overrides Function ToString() As String
            Return airsNumber
        End Function

        ''' <summary>
        ''' APB facility ID as an eight-character string in the form "00000000"
        ''' </summary>
        Public ReadOnly Property ShortString() As String
            Get
                Return airsNumber
            End Get
        End Property

        ''' <summary>
        ''' Returns APB facility ID as a nine-character string in the form "000-00000"
        ''' </summary>
        Public ReadOnly Property FormattedString() As String
            Get
                Return Mid(airsNumber, 1, 3) & "-" & Mid(airsNumber, 4, 5)
            End Get
        End Property

        ''' <summary>
        ''' Displays APB facility ID as a 12-character string in the form "041300000000"
        ''' </summary>
        Public ReadOnly Property DbFormattedString() As String
            Get
                Return GA_EPA_REGION_CODE & GA_STATE_NUMERIC_CODE & airsNumber
            End Get
        End Property

        ''' <summary>
        ''' Returns county substring as a 3-character string 
        ''' </summary>
        Public ReadOnly Property CountySubstring() As String
            Get
                Return Mid(airsNumber, 1, 3)
            End Get
        End Property

        ''' <summary>
        ''' Displays the facility identifier used by EPA's ICIS-Air system
        ''' </summary>
        Public ReadOnly Property EpaFacilityIdentifier() As String
            Get
                Return GA_STATE_CODE & "000000" & GA_STATE_NUMERIC_CODE & airsNumber
            End Get
        End Property

#End Region

#Region " Operators "

        Public Shared Narrowing Operator CType(airsNumber As String) As ApbFacilityId
            Return New ApbFacilityId(airsNumber)
        End Operator

        Public Shared Widening Operator CType(airsNumber As ApbFacilityId) As String
            Return airsNumber.ToString
        End Operator

#End Region

#Region " IEquatable Interface implementation "

        Public Overloads Function Equals(other As ApbFacilityId) As Boolean _
        Implements IEquatable(Of ApbFacilityId).Equals
            If other Is Nothing Then Return False
            Return ToString().Equals(other.ToString())
        End Function

        Public Overrides Function Equals(obj As Object) As Boolean
            If obj Is Nothing Then Return False
            If TypeOf obj Is ApbFacilityId Then Return Equals(DirectCast(obj, ApbFacilityId))
            Return False
        End Function

        Public Overrides Function GetHashCode() As Integer
            Return ToString().GetHashCode()
        End Function

#End Region

#Region " Validate/normalize AIRS number "

        ''' <summary>
        ''' Determines whether a string is in the format of a valid AIRS number.
        ''' </summary>
        ''' <param name="airsNumber">The string to test</param>
        ''' <returns>True if airsNumber is valid; otherwise, False.</returns>
        ''' <remarks>Valid AIRS numbers are in the form 000-00000 or 04-13-000-0000 (with or without the dashes)</remarks>
        <DebuggerStepThrough()>
        Public Shared Function IsValidAirsNumberFormat(airsNumber As String) As Boolean
            If airsNumber Is Nothing Then Return False

            Return Regex.IsMatch(airsNumber, AirsNumberPattern)
        End Function

        ''' <summary>
        ''' Converts a string representation of an AIRS number to the "00000000" form.
        ''' </summary>
        ''' <param name="airsNumber">The AIRS number to convert.</param>
        ''' <returns>A string representation of an AIRS number in the form "00000000".</returns>
        <DebuggerStepThrough()>
        Private Shared Function GetNormalizedAirsNumber(airsNumber As String) As String
            ' Remove dashes and leading '0413'
            airsNumber = airsNumber.Replace("-", "")
            If airsNumber.Length = 12 Then airsNumber = airsNumber.Remove(0, 4)

            Return airsNumber
        End Function

#End Region

    End Class
End Namespace
