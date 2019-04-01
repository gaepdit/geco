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
End Namespace
