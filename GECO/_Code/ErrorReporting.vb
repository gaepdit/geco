Imports GECO.GecoModels
Imports SharpRaven
Imports SharpRaven.Data

Public Module ErrorReporting

    Public ReadOnly LogExceptionsToFile As Boolean = ConfigurationManager.AppSettings("LogExceptionsToFile")

    Public Sub ErrorReport(exc As Exception, Optional redirectToErrorPage As Boolean = True)
        If exc Is Nothing Then
            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx", False)
            Return
        End If

        If LogExceptionsToFile Then
            LogExceptionToTextFile(exc)
        End If

        Try
            Dim context = HttpContext.Current
            context.Server.ClearError()

            Dim environment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")
            If environment = "Development" Then
                Return
            End If

            ' Add additional data to exception
            Dim URL = context.Request.Url.AbsoluteUri
            If Not String.IsNullOrEmpty(URL) Then
                exc.Data.Add("URL", URL)
            End If

            Dim userEmail As String = GetCurrentUser()?.Email
            If userEmail IsNot Nothing Then
                exc.Data.Add("User Email", userEmail)
            End If

            Dim currentUser As GecoUser = GetCurrentUser()
            If currentUser IsNot Nothing Then
                exc.Data.Add("Current User ID", currentUser.DbUpdateUser)
            End If

            Dim airsCookie As String = GetCookie(Cookie.AirsNumber)
            If airsCookie IsNot Nothing Then
                exc.Data.Add("Current AIRS", airsCookie)
            End If

            If exc.StackTrace IsNot Nothing Then
                exc.Data.Add("Stack Trace", exc.StackTrace)
            End If

            If exc.TargetSite IsNot Nothing AndAlso exc.TargetSite.Name IsNot Nothing Then
                exc.Data.Add("Target Method", exc.TargetSite.Name)
            End If

            ' Log the exception
            Try
                Dim ExceptionLogger As New RavenClient(ConfigurationManager.AppSettings("SENTRY_DSN")) With {
                    .Environment = environment,
                    .Release = ConfigurationManager.AppSettings("GECO_VERSION")
                }
                ExceptionLogger.Capture(New SentryEvent(exc))
            Catch ex As Exception
                ' If Sentry logging fails, log to text file
                LogToTextFile("Sentry exception:")
                LogExceptionToTextFile(ex)
            End Try

        Catch ex As Exception
            ' Do nothing if procedure fails
        Finally
            If redirectToErrorPage Then
                HttpContext.Current.Response.Redirect("~/ErrorPage.aspx", False)
            End If
        End Try
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
