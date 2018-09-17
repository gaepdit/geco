Public Module StringFunctions

    ''' <summary>
    ''' Implodes a String array to a single string, concatenating the items using the separator, and ignoring null or empty string items
    ''' </summary>
    ''' <param name="separator">The separator string to include between each item</param>
    ''' <param name="items">An array of strings to concatenate</param>
    ''' <returns>A concatenated string separated by the specified separator. Null or empty strings are ignored.</returns>
    Public Function ConcatNonEmptyStrings(separator As String, items As String()) As String
        Return String.Join(separator, Array.FindAll(items, Function(s) Not String.IsNullOrEmpty(s)))
    End Function

    ''' <summary>
    ''' Implodes a List of Strings to a single string, concatenating the items using the separator, and ignoring null or empty string items
    ''' </summary>
    ''' <param name="separator">The separator string to include between each item</param>
    ''' <param name="items">A List of Strings to concatenate</param>
    ''' <returns>A concatenated string separated by the specified separator. Null or empty strings are ignored.</returns>
    Public Function ConcatNonEmptyStrings(separator As String, items As List(Of String)) As String
        Return ConcatNonEmptyStrings(separator, items.ToArray())
    End Function

    Public Function RealStringOrNothing(s As String) As String
        If String.IsNullOrEmpty(s) Then
            Return Nothing
        Else
            Return s
        End If
    End Function

End Module