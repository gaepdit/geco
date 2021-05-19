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

    Public Function GetSessionItem(sessionItem As GecoSession) As Object
        Return GetSessionItem(sessionItem.ToString)
    End Function

    Private Function GetSessionItem(sessionItem As String) As Object
        If Not SessionItemExists(sessionItem) Then Return Nothing
        Return HttpContext.Current.Session(sessionItem)
    End Function

    Public Function GetSessionItem(Of T)(sessionItem As GecoSession) As T
        Return GetSessionItem(Of T)(sessionItem.ToString)
    End Function

    Public Function GetSessionItem(Of T)(sessionItem As String) As T
        Dim sessionObject As Object = GetSessionItem(sessionItem.ToString)
        If sessionObject Is Nothing Then Return Nothing
        Return DirectCast(sessionObject, T)
    End Function

    Public Function SessionItemExists(sessionItem As GecoSession) As Boolean
        Return HttpContext.Current.Session?.Item(sessionItem.ToString) IsNot Nothing
    End Function

    Private Function SessionItemExists(sessionItem As String) As Boolean
        Return HttpContext.Current.Session?.Item(sessionItem) IsNot Nothing
    End Function

End Module
