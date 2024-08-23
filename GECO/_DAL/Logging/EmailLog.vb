Imports Microsoft.Data.SqlClient
Imports System.Net.Mail

Namespace DAL
    Public Module EmailLog

        Public Function LogEmail(msg As MailMessage, plainTextBody As String, htmlBody As String, origin As String) As Boolean
            NotNull(msg, NameOf(msg))

            Dim spName As String = "dbo.LogEmail"

            Dim params As SqlParameter() = {
                New SqlParameter("@From", msg.From.ToString),
                New SqlParameter("@To", msg.To.ToString),
                New SqlParameter("@Cc", msg.CC.ToString),
                New SqlParameter("@Bcc", msg.Bcc.ToString),
                New SqlParameter("@Subject", msg.Subject),
                New SqlParameter("@PlainTextBody", plainTextBody),
                New SqlParameter("@HtmlBody", htmlBody),
                New SqlParameter("@Origin", origin)
            }

            Return (DB.SPReturnValue(spName, params) = 0)
        End Function

    End Module
End Namespace