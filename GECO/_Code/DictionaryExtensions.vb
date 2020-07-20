Imports System.Runtime.CompilerServices

Public Module DictionaryExtensions

    ''' <summary>
    ''' Adds an element with the provided key and value to the System.Collections.IDictionary 
    ''' object if an element with that key does not yet exist.
    ''' </summary>
    ''' <param name="d">The IDictionary object to add the element to.</param>
    ''' <param name="key">The System.Object to use as the key of the element to add.</param>
    ''' <param name="value">The System.Object to use as the value of the element to add.</param>
    ''' <exception cref="ArgumentNullException">key is null.</exception>
    ''' <exception cref="NotSupportedException">The System.Collections.IDictionary is read-only. -or- The System.Collections.IDictionary
    ''' has a fixed size.</exception>
    <Extension>
    Public Sub AddIfNotExists(ByRef d As IDictionary, key As Object, value As Object)
        NotNull(key, NameOf(key))

        If Not d.Contains(key) Then
            d.Add(key, value)
        End If
    End Sub

    ''' <summary>
    ''' Adds an element with the provided key and String value to the System.Collections.IDictionary 
    ''' object if the String value is not null or empty.
    ''' </summary>
    ''' <param name="d">The IDictionary object to add the element to.</param>
    ''' <param name="key">The System.Object to use as the key of the element to add.</param>
    ''' <param name="value">The String to use as the value of the element to add.</param>
    ''' <exception cref="ArgumentNullException">key is null.</exception>
    ''' <exception cref="NotSupportedException">The System.Collections.IDictionary is read-only. -or- The System.Collections.IDictionary
    ''' has a fixed size.</exception>
    <Extension>
    Public Sub AddIfNotNullOrEmpty(ByRef d As IDictionary, key As Object, value As String)
        If Not String.IsNullOrEmpty(value) Then
            d.Add(key, value)
        End If
    End Sub

End Module
