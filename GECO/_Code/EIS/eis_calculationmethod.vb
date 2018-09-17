Public Module eis_calculationmethod

    Public Function CalcMethodIsEmissionFactor(methodCode As String) As Boolean
        Select Case methodCode
            Case "0", "1", "2", "3", "4", "5", "6", "7", "24"
                Return False
            Case Else
                Return True
        End Select
    End Function

    Public Function CalcMethodIsVague(methodCode As String) As Boolean
        Select Case methodCode
            Case "2", "13", "33"
                Return True
            Case Else
                Return False
        End Select
    End Function

End Module