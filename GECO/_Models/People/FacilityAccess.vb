Imports GECO.GecoModels.Facility

Namespace GecoModels
    Public Class FacilityAccess
        Private _adminAccess As Boolean
        Private _feeAccess As Boolean
        Private _eisAccess As Boolean
        Private _esAccess As Boolean
        Private ReadOnly _communicationPermissions As List(Of String)

        Public Sub New(airsNumber As ApbFacilityId)
            Me.AirsNumber = airsNumber
            _communicationPermissions = New List(Of String)
        End Sub

        Public Sub New(airsNumber As ApbFacilityId,
                       admin As Boolean,
                       fee As Boolean,
                       eis As Boolean,
                       es As Boolean)
            Me.AirsNumber = airsNumber
            _communicationPermissions = New List(Of String)

            If admin Then
                AdminAccess = True
                _feeAccess = True
                _eisAccess = True
                _esAccess = True
            Else
                FeeAccess = fee
                EisAccess = eis
                ESAccess = es
            End If
        End Sub

        Public Property AirsNumber As ApbFacilityId

        Public Property AdminAccess As Boolean
            Get
                Return _adminAccess
            End Get
            Private Set
                _adminAccess = Value

                If Value Then
                    For Each category In CommunicationCategory.AllCategories
                        _communicationPermissions.Add(category.Name)
                    Next
                End If
            End Set
        End Property

        Public Property FeeAccess As Boolean
            Get
                Return _feeAccess
            End Get
            Private Set
                _feeAccess = Value

                If Value Then
                    _communicationPermissions.Add(CommunicationCategory.PermitFees.Name)
                End If
            End Set
        End Property

        Public Property EisAccess As Boolean
            Get
                Return _eisAccess
            End Get
            Private Set
                _eisAccess = Value

                If Value Then
                    _communicationPermissions.Add(CommunicationCategory.EmissionsInventory.Name)
                End If
            End Set
        End Property

        ' Existing Emissions Statement permissions are used only for displaying the notice on the facility home page.
        Public Property ESAccess As Boolean
            Get
                Return _esAccess
            End Get
            Private Set
                _esAccess = Value
                ' There is no communication pref for Emissions Statement.
            End Set
        End Property

        Public ReadOnly Property HasCommunicationPermission(category As CommunicationCategory) As Boolean
            Get
                Return _communicationPermissions.Contains(category.Name)
            End Get
        End Property
    End Class
End Namespace
