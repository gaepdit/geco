Public Module SessionHelper

    Public Enum GecoSession
        CurrentUser
        EventPasscode
        TempEmail
        TestNotifications
    End Enum

    Public Sub SessionAdd(sessionItem As GecoSession, value As Object)
        HttpContext.Current.Session.Add(sessionItem.ToString, value)
    End Sub

    Public Sub SessionRemove(sessionItem As GecoSession)
        HttpContext.Current.Session.Remove(sessionItem.ToString)
    End Sub

    Public Function GetSessionItem(sessionItem As GecoSession) As Object
        If SessionItemExists(sessionItem) Then
            Return HttpContext.Current.Session(sessionItem.ToString)
        End If
        Return Nothing
    End Function

    Public Function SessionItemExists(sessionItem As GecoSession) As Boolean
        Return HttpContext.Current.Session(sessionItem.ToString) IsNot Nothing
    End Function

End Module
