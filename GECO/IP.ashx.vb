Public Class IP1
    Implements IHttpHandler

    Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        NotNull(context, NameOf(context))

        context.Response.ContentType = "text/plain"
        Dim IpAddress As String = context.Request.ServerVariables("REMOTE_ADDR")
        context.Response.Write(IpAddress)
    End Sub

    ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class