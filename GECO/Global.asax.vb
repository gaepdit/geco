Public Class Global_asax
    Inherits HttpApplication

    'Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Fires when the application is started
    'End Sub

    'Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Fires when the session is started
    'End Sub

    'Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Fires at the beginning of each request
    'End Sub

    'Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Fires upon attempting to authenticate the user
    'End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an unhandled error occurs

        ' Get exception
        Dim exc As Exception = Server.GetLastError()

        ' Get HTTP code if available
        Dim httpCode As Integer = -1
        Dim httpException As HttpException = TryCast(exc, HttpException)
        If httpException IsNot Nothing Then httpCode = httpException.GetHttpCode()

        ' HTTP 404s are handled differently from everything else
        If httpCode = 404 Then
            Server.ClearError()
            Server.Transfer("/404.aspx")
        Else
            ' Add data to exception and send to error logger
            exc.Data.AddIfNotExists("Unhandled", True)
            If httpCode > 0 Then exc.Data.AddIfNotExists("HTTP Code", httpCode)
            ErrorReport(exc, unhandled:=True)
        End If
    End Sub

    'Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Fires when the session ends
    'End Sub

    'Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Fires when the application ends
    'End Sub

End Class
