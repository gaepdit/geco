Namespace GecoModels.EIS

    Public Class EisProcess
        Public Property FacilitySiteId As ApbFacilityId
        Public Property InventoryYear As Integer
        Public Property UpdateUser As String
        Public Property AdminComment As String
        Public Property Opted As OptedInOut
        Public Property Colocated As Boolean?
        Public Property Colocation As String

        Public Function GetOptOutCode() As String
            Return If(Opted = OptedInOut.OptedIn, "0", "1")
        End Function
        Public Function GetOptOutReasonCode() As String
            Select Case Opted
                Case OptedInOut.DidNotOperate
                    Return "1"
                Case OptedInOut.BelowThresholds
                    Return "2"
                Case Else
                    Return Nothing
            End Select
        End Function
        Public Function GetOptOutReason() As String
            Select Case Opted
                Case OptedInOut.DidNotOperate
                    Return "Facility did not operate"
                Case OptedInOut.BelowThresholds
                    Return "Facility emissions below thresholds"
                Case Else
                    Return String.Empty
            End Select
        End Function
    End Class

    Public Enum OptedInOut
        OptedIn
        DidNotOperate
        BelowThresholds
    End Enum

End Namespace
