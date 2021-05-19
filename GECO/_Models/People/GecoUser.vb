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
        Public Property GecoUserType As String
        Public Property FacilityAccessTable As DataTable
        Public Property ProfileUpdateRequired As Boolean

        ' Generated properties
        Public ReadOnly Property DbUpdateUser As String
            Get
                Return UserId & "-" & FullName
            End Get
        End Property

        ' Facility Access
        Public Function GetFacilityAccess(airsNumber As ApbFacilityId) As FacilityAccess
            Dim dr As DataRow = FacilityAccessTable.Rows.Find(airsNumber)

            If dr Is Nothing Then
                Return New FacilityAccess(airsNumber, False)
            End If

            Return New FacilityAccess(airsNumber) With {
                .AdminAccess = CBool(dr.Item("AdminAccess")),
                .FeeAccess = CBool(dr.Item("FeeAccess")),
                .EisAccess = CBool(dr.Item("EIAccess")),
                .ESAccess = CBool(dr.Item("ESAccess"))
            }
        End Function

    End Class
End Namespace
