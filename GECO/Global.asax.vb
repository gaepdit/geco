Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

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
    '    ' Fires upon attempting to authenticate the use
    'End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
        ' Code that runs when an unhandled error occurs

        ' Get exception
        Dim lastErrorWrapper As HttpException = CType(Server.GetLastError(), HttpException)
        Dim httpCode As Integer = lastErrorWrapper.GetHttpCode()

        ' 404's handled differently from everything else
        If httpCode = 404 Then
            Server.ClearError()
            Server.Transfer("/404.aspx")
        Else
            Dim exc As Exception = lastErrorWrapper

            If lastErrorWrapper.InnerException IsNot Nothing Then
                exc = lastErrorWrapper.InnerException
            End If

            ' Redirect to error page
            If Not exc.Data.Contains("Unhandled") Then
                exc.Data.Add("Unhandled", True)
            End If

            If Not exc.Data.Contains("HTTP Code") Then
                exc.Data.Add("HTTP Code", httpCode)
            End If

            ErrorReport(exc)
        End If
    End Sub

    'Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Fires when the session ends
    'End Sub

    'Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Fires when the application ends
    'End Sub

End Class