Public Module Guard

    Public Function NotNull(Of T)(ByVal value As T, ByVal parameterName As String) As T
        If value Is Nothing Then
            Throw New ArgumentNullException(parameterName)
        End If

        Return value
    End Function

    Public Function NotNullOrEmpty(ByVal value As String, ByVal parameterName As String) As String
        If value Is Nothing Then
            Throw New ArgumentNullException(parameterName)
        End If

        If String.IsNullOrEmpty(value) Then
            Throw New ArgumentException($"{parameterName} can not be null or empty.", parameterName)
        End If

        Return value
    End Function

    'Public Function NotNullOrWhiteSpace(ByVal value As String, ByVal parameterName As String) As String
    '    If value Is Nothing Then
    '        Throw New ArgumentNullException(parameterName)
    '    End If

    '    If String.IsNullOrWhiteSpace(value) Then
    '        Throw New ArgumentException($"{parameterName} can not be null, empty, or white space.", parameterName)
    '    End If

    '    Return value
    'End Function

    'Public Function NotNullOrEmpty(Of T)(ByVal value As ICollection(Of T), ByVal parameterName As String) As ICollection(Of T)
    '    If value Is Nothing Then
    '        Throw New ArgumentNullException(parameterName)
    '    End If

    '    If value.IsNullOrEmpty() Then
    '        Throw New ArgumentException($"{parameterName} can not be null or empty.", parameterName)
    '    End If

    '    Return value
    'End Function

    'Public Function NotNegative(ByVal value As Integer, ByVal parameterName As String) As Integer
    '    If value < 0 Then
    '        Throw New ArgumentException($"{parameterName} can not be negative.", parameterName)
    '    End If

    '    Return value
    'End Function

    'Public Function Positive(ByVal value As Integer, ByVal parameterName As String) As Integer
    '    If value <= 0 Then
    '        Throw New ArgumentException($"{parameterName} must be positive (greater than zero).", parameterName)
    '    End If

    '    Return value
    'End Function

End Module
