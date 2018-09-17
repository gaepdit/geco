Imports System.ComponentModel
Imports System.Data

Namespace GecoModels
    Public Class GecoUser

#Region " Constructors "

        Public Sub New()
        End Sub

        Public Sub New(userId As Integer)
            Me.UserId = userId
        End Sub

#End Region

#Region " Properties "

        Public Property UserId As Integer
        Private _email As String
        Public Property Email As String
            Get
                Return _email
            End Get
            Set(value As String)
                _email = Trim(value)
            End Set
        End Property

        Public Property Salutation As String
        Public Property FirstName As String
        Public Property LastName As String

        Public Property Title As String
        Public Property Company As String
        Public Property Address As Address
        Public Property PhoneNumber As String
        Public Property FaxNumber As String

        Public Property GecoUserType As GecoUserType
        Public Property GecoUserTypeString As String
            Get
                Return GecoUserType.GetDescription()
            End Get
            Set(value As String)
                GecoUserType = ParseGecoUserType(value)
            End Set
        End Property

        Public Property FacilityAccessTable As DataTable

#End Region

#Region " Read-only properties "

        Public ReadOnly Property FullName As String
            Get
                Return ConcatNonEmptyStrings(" ", {FirstName, LastName})
            End Get
        End Property

        Public ReadOnly Property PhoneFormatted As String
            Get
                Return FormatPhoneNumber(PhoneMain)
            End Get
        End Property

        Public ReadOnly Property PhoneMain As String
            Get
                Return Mid(PhoneNumber, 1, 10)
            End Get
        End Property

        Public ReadOnly Property PhoneExt As String
            Get
                Return Mid(PhoneNumber, 11)
            End Get
        End Property

#End Region

#Region " User Type "

        Private Function ParseGecoUserType(userType As String) As GecoUserType
            Select Case userType
                Case "Work for a facility"
                    Return GecoUserType.Facility
                Case "Public"
                    Return GecoUserType.Public
                Case "Environmental Consultant"
                    Return GecoUserType.EnvironmentalConsultant
                Case "Work for Environmental Group"
                    Return GecoUserType.EnvironmentalGroup
                Case "Government Agency"
                    Return GecoUserType.GovernmentAgency
                Case Else
                    Throw New InvalidOperationException(String.Format("{0} is not a valid GECO user type", userType))
            End Select
        End Function

#End Region

#Region " Facility Access "

        Public Function GetFacilityAccess(airsNumber As ApbFacilityId) As FacilityAccess
            Dim dr As DataRow = FacilityAccessTable.Rows.Find(airsNumber)

            If dr Is Nothing Then
                Return Nothing
            End If

            Return New FacilityAccess(airsNumber) With {
                .AdminAccess = dr.Item("AdminAccess"),
                .FeeAccess = dr.Item("FeeAccess"),
                .EisAccess = dr.Item("EIAccess"),
                .ESAccess = dr.Item("ESAccess")
            }
        End Function

#End Region

        Public Shared Function FormatPhoneNumber(phone As String) As String
            If String.IsNullOrEmpty(phone) OrElse phone.Length <> 10 OrElse Not IsNumeric(phone) Then
                Return phone
            End If

            Return phone.Insert(6, "-").Insert(3, "-")
        End Function

    End Class

    Public Enum GecoUserType
        <Description("Work for a facility")> Facility
        <Description("Public")> [Public]
        <Description("Environmental Consultant")> EnvironmentalConsultant
        <Description("Work for Environmental Group")> EnvironmentalGroup
        <Description("Government Agency")> GovernmentAgency
    End Enum

    Public Class FacilityAccess
        Public Sub New(airsNumber As ApbFacilityId)
            Me.AirsNumber = airsNumber
        End Sub
        Public Property AirsNumber As ApbFacilityId

        Public Property AdminAccess As Boolean
        Public Property FeeAccess As Boolean
        Public Property EisAccess As Boolean
        Public Property ESAccess As Boolean
    End Class

End Namespace
