Public Module eis_getcodedescriptions

    Public Function DecodeOptOutReason(reason As String) As String
        Select Case reason
            Case "1"
                Return "Facility did not operate"
            Case "2"
                Return "Facility emissions below thresholds"
            Case Else
                Return String.Empty
        End Select
    End Function

    Public Function DecodeOptOutReason(reason As Integer?) As String
        If Not reason.HasValue Then
            Return String.Empty
        End If

        Return DecodeOptOutReason(reason.Value.ToString)
    End Function

End Module
