Imports GECO.GecoModels
Imports SharpRaven
Imports SharpRaven.Data

Public Module ErrorReporting

    Public Sub ErrorReport(exc As Exception, Optional redirectToErrorPage As Boolean = True)
        Try
            Dim context = HttpContext.Current
            context.Server.ClearError()

            Dim environment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")
            If environment = "Development" Then
                Exit Sub
            End If

            ' Add additional data to exception
            Dim URL = context.Request.Url.AbsoluteUri
            If Not String.IsNullOrEmpty(URL) Then
                exc.Data.Add("URL", URL)
            End If

            Dim userEmail As String = GetCookie(GecoCookie.UserEmail)
            If userEmail IsNot Nothing Then
                exc.Data.Add("User Email", userEmail)
            End If

            Dim currentUser As GecoUser = GetCurrentUser()
            If currentUser IsNot Nothing Then
                exc.Data.Add("Current User", currentUser)
            End If

            Dim airsCookie As String = GetCookie(Cookie.AirsNumber)
            If airsCookie IsNot Nothing Then
                exc.Data.Add("Current AIRS", airsCookie)
            End If

            If exc.StackTrace IsNot Nothing Then
                exc.Data.Add("Stack Trace", exc.StackTrace)
            End If

            If exc.TargetSite.Name IsNot Nothing Then
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
                ' Do nothing if error logging fails
            End Try

        Catch ex As Exception
            ' Do nothing if procedure fails
        Finally
            If redirectToErrorPage Then
                HttpContext.Current.Response.Redirect("~/ErrorPage.aspx", False)
            End If
        End Try
    End Sub

End Module