Imports System.ComponentModel

Namespace GecoModels
    Public Class GecoUser
        Inherits Person

        ' Constructors 

        Public Sub New()
        End Sub

        Public Sub New(userId As Integer)
            Me.UserId = userId
        End Sub

        ' Properties 

        Public Property UserId As Integer

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

        ' User Type 

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

        ' Facility Access 

        Public Function GetFacilityAccess(airsNumber As ApbFacilityId) As FacilityAccess
            Dim dr As DataRow = FacilityAccessTable.Rows.Find(airsNumber)

            If dr Is Nothing Then
                Return New FacilityAccess(airsNumber, False)
            End If

            Return New FacilityAccess(airsNumber) With {
                .AdminAccess = dr.Item("AdminAccess"),
                .FeeAccess = dr.Item("FeeAccess"),
                .EisAccess = dr.Item("EIAccess"),
                .ESAccess = dr.Item("ESAccess")
            }
        End Function

    End Class

    Public Enum GecoUserType
        <Description("Work for a facility")> Facility
        <Description("Public")> [Public]
        <Description("Environmental Consultant")> EnvironmentalConsultant
        <Description("Work for Environmental Group")> EnvironmentalGroup
        <Description("Government Agency")> GovernmentAgency
    End Enum

End Namespace
