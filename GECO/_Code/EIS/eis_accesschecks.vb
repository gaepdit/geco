Public Module eis_accesschecks

    'Checks Facility Inventory access status (only for Facility Inventory pages) and redirects to EIS default page if no access
    Public Sub FIAccessCheck(ByVal access As String)
        If Not (access = "0" OrElse access = "1") Then
            HttpContext.Current.Server.Transfer("Default.aspx")
        End If
    End Sub

    Public Sub EIEntryAccessCheck(ByVal access As String, ByVal opt As String)
        If Not (access = "1" AndAlso opt = "NULL") Then
            HttpContext.Current.Server.Transfer("Default.aspx")
        End If
    End Sub

End Module