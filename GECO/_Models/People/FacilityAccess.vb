Imports GECO.GecoModels.Facility

Namespace GecoModels
    Public Class FacilityAccess
        Private _AdminAccess As Boolean
        Private _FeeAccess As Boolean
        Private _EisAccess As Boolean
        Private _ESAccess As Boolean
        Private ReadOnly _CommunicationPermissions As List(Of String)

        Public Sub New(airsNumber As ApbFacilityId)
            Me.AirsNumber = airsNumber
            _CommunicationPermissions = New List(Of String)
        End Sub

        Public Sub New(airsNumber As ApbFacilityId,
                       admin As Boolean,
                       fee As Boolean,
                       eis As Boolean,
                       es As Boolean)
            Me.AirsNumber = airsNumber
            _CommunicationPermissions = New List(Of String)

            If admin Then
                AdminAccess = True
                _FeeAccess = True
                _EisAccess = True
                _ESAccess = True
            Else
                FeeAccess = fee
                EisAccess = eis
                ESAccess = es
            End If
        End Sub

        Public Property AirsNumber As ApbFacilityId

        Public Property AdminAccess As Boolean
            Get
                Return _AdminAccess
            End Get
            Private Set
                _AdminAccess = Value

                If Value Then
                    For Each category In CommunicationCategory.AllCategories
                        _CommunicationPermissions.Add(category.Name)
                    Next
                End If
            End Set
        End Property

        Public Property FeeAccess As Boolean
            Get
                Return _FeeAccess
            End Get
            Private Set
                _FeeAccess = Value

                If Value Then
                    _CommunicationPermissions.Add(CommunicationCategory.Fees.Name)
                End If
            End Set
        End Property

        Public Property EisAccess As Boolean
            Get
                Return _EisAccess
            End Get
            Private Set
                _EisAccess = Value

                If Value Then
                    _CommunicationPermissions.Add(CommunicationCategory.EmissionsInventory.Name)
                End If
            End Set
        End Property

        Public Property ESAccess As Boolean
            Get
                Return _ESAccess
            End Get
            Private Set
                _ESAccess = Value

                If Value Then
                    _CommunicationPermissions.Add(CommunicationCategory.EmissionsStatement.Name)
                End If
            End Set
        End Property

        Public ReadOnly Property HasCommunicationPermission(category As CommunicationCategory) As Boolean
            Get
                Return _CommunicationPermissions.Contains(category.Name)
            End Get
        End Property

    End Class
End Namespace
