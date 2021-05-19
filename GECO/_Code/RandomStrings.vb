Public Module RandomStrings

    Private Const UnambiguousCharacters As String = "ACDEFGHJKMNPQRSTUVWXY345679"

    Public Function RandomString(intLength As Integer, Optional strAllowedCharacters As String = UnambiguousCharacters) As String
        NotNull(strAllowedCharacters, NameOf(strAllowedCharacters))

        Randomize()
        Dim chrChars As Char() = strAllowedCharacters.ToCharArray
        Dim strReturn As New StringBuilder

        Do Until Len(strReturn.ToString) = intLength
            Dim x As Integer = CInt(Rnd() * (chrChars.Length - 1))
            strReturn.Append(chrChars(x))
        Loop

        Return strReturn.ToString
    End Function

End Module
