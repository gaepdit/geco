Imports System.ComponentModel
Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module EnumHelpers

    Private ReadOnly enumDescriptions As New Dictionary(Of String, String)

    ''' <summary>
    ''' If a Description attribute is present for an enum value, returns the description.
    ''' Otherwise, returns the normal ToString() representation of the enum value.
    ''' </summary>
    ''' <param name="e">The enum value to describe.</param>
    ''' <returns>The value of the Description attribute if present, else
    ''' the normal ToString() representation of the enum value.</returns>
    ''' <remarks>http://stackoverflow.com/a/14772005/212978</remarks>
    <Extension>
    Public Function GetDescription(e As [Enum]) As String
        Dim enumType As Type = e.GetType()
        Dim name As String = e.ToString()

        ' Construct a full name for this enum value
        Dim fullName As String = enumType.FullName + "." + name

        ' See if we have looked it up earlier
        Dim enumDescription As String = Nothing
        If enumDescriptions.TryGetValue(fullName, enumDescription) Then
            ' Yes we have - return previous value
            Return enumDescription
        End If

        ' Find the value of the Description attribute on this enum value
        Dim members As MemberInfo() = enumType.GetMember(name)
        If members IsNot Nothing AndAlso members.Length > 0 Then
            Dim descriptions() As Object = members(0).GetCustomAttributes(GetType(DescriptionAttribute), False)
            If descriptions IsNot Nothing AndAlso descriptions.Length > 0 Then
                ' Set name to description found
                name = DirectCast(descriptions(0), DescriptionAttribute).Description
            End If
        End If

        ' Save the name in the dictionary:
        enumDescriptions.Add(fullName, name)

        Return name
    End Function

End Module
