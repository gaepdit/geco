Imports Mindscape.Raygun4Net

Public Module ErrorReporting

    Public Sub ErrorReport(exc As Exception,
                           Optional redirectToErrorPage As Boolean = True,
                           Optional unhandled As Boolean = False)

        If exc Is Nothing Then
            HttpContext.Current.Response.Redirect("~/ErrorPage.aspx", False)
            HttpContext.Current.ApplicationInstance.CompleteRequest()
            Return
        End If

        Dim LogExceptionsToFile As Boolean = CBool(ConfigurationManager.AppSettings("LogExceptionsToFile"))
        If LogExceptionsToFile Then
            LogExceptionToTextFile(exc)
        End If

        Try
            HttpContext.Current.Server.ClearError()

            Dim environment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")
            If environment = "Development" Then
                Return
            End If

            ' Data for error logger
            Dim tags As New List(Of String) From {environment}
            If unhandled Then
                tags.Add("Unhandled")
            End If

            Dim customData As New Dictionary(Of String, Object)
            customData.AddIfNotNullOrEmpty("Absolute URI", HttpContext.Current.Request.Url.AbsoluteUri)
            customData.AddIfNotNullOrEmpty("Current AIRS", GetCookie(Cookie.AirsNumber))

            ' Log the exception
            Try

                Dim raygunClient As New RaygunClient With {
                    .ApplicationVersion = ConfigurationManager.AppSettings("GECO_VERSION"),
                    .UserInfo = RaygunInfo.GetRaygunIdentifier()
                }
                raygunClient.IgnoreFormFieldNames("txtPassword", "txtOldPassword", "txtNewPassword", "txtPwdConfirm")

                If unhandled Then
                    raygunClient.Send(exc, tags, customData)
                Else
                    raygunClient.SendInBackground(exc, tags, customData)
                End If

            Catch ex As Exception
                ' If logging fails, log to text file instead
                LogToTextFile("Exception logging error:")
                LogExceptionToTextFile(ex)
            End Try

        Catch ex As Exception
            ' Do nothing if error logger fails
        Finally
            If redirectToErrorPage Then
                HttpContext.Current.Response.Redirect("~/ErrorPage.aspx", False)
                HttpContext.Current.ApplicationInstance.CompleteRequest()
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
