Public Module eis_emissionunits

    Public Function UnitIsFuelBurning(UnitTypeCode As String) As Boolean
        Select Case UnitTypeCode
            Case "100", "120", "140", "160", "180"
                Return True
            Case Else
                Return False
        End Select
    End Function

End Module