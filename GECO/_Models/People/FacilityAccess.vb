Namespace GecoModels
    Public Class FacilityAccess
        Public Sub New(airsNumber As ApbFacilityId)
            Me.AirsNumber = airsNumber
        End Sub

        Public Sub New(airsNumber As ApbFacilityId, accessGranted As Boolean)
            Me.AirsNumber = airsNumber
            AdminAccess = accessGranted
            FeeAccess = accessGranted
            EisAccess = accessGranted
            ESAccess = accessGranted
        End Sub

        Public Property AirsNumber As ApbFacilityId

        Public Property AdminAccess As Boolean
        Public Property FeeAccess As Boolean
        Public Property EisAccess As Boolean
        Public Property ESAccess As Boolean
    End Class
End Namespace
