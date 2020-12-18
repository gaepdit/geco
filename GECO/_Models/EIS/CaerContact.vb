Namespace GecoModels.EIS

    Public Class CaerContact
        Public Property Id As Guid
        Public Property Contact As Person ' includes address, email, company, etc.
        Public Property CaerRole As CaerRole
        Public Property FacilitySiteId As ApbFacilityId
        Public Property FacilityName As String
        Public Property Active As Boolean
    End Class

    Public Enum CaerRole
        Preparer
        Certifier
    End Enum

End Namespace
