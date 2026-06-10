Public Module ErrorReporting

    Public Sub ErrorReport(exc As Exception, Optional redirectToErrorPage As Boolean = True)

        If exc Is Nothing Then
            CompleteRedirect("~/ErrorPage.aspx")
            Return
        End If

        LogExceptionToTextFile(exc)

        If redirectToErrorPage Then
            CompleteRedirect("~/ErrorPage.aspx")
        End If
    End Sub

    Private Sub LogExceptionToTextFile(exc As Exception)
        Dim sb As New StringBuilder("--- Begin Exception ---")
        sb.AppendLine()
        Dim inner As Boolean = False

        While exc IsNot Nothing
            If inner Then
                sb.AppendLine("--- Inner Exception: ---")
            End If
            sb.Append("Message: ")
            sb.AppendLine(exc.Message)
            sb.AppendLine()
            sb.AppendLine("Stack Trace:")
            sb.AppendLine(exc.StackTrace)
            sb.AppendLine()
            sb.Append("Source: ")
            sb.AppendLine(exc.Source)
            sb.AppendLine()
            exc = exc.InnerException
            inner = True
        End While

        LogToTextFile(sb.ToString)
    End Sub

End Module
