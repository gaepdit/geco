﻿Imports System.Runtime.CompilerServices

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
        NotNull(items, NameOf(items))

        Return ConcatNonEmptyStrings(separator, items.ToArray())
    End Function

    <Extension>
    Public Function NonEmptyStringOrNothing(s As String) As String
        If String.IsNullOrEmpty(s) Then
            Return Nothing
        End If

        Return s
    End Function

    <Extension>
    Public Function EmptyStringIfNothing(s As String) As String
        If s Is Nothing Then
            Return ""
        End If

        Return s
    End Function

    Public Function Left(s As String, length As Integer) As String
        If s Is Nothing Then
            Return String.Empty
        End If

        If s.Length > length Then
            Return s.Substring(0, length)
        End If

        Return s
    End Function

End Module
