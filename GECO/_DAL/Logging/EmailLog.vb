Imports GECO.EmailApi
Imports Microsoft.Data.SqlClient

Namespace DAL
    Public Module EmailLog

        Public Function LogEmail(msg As Email, plainTextBody As String, htmlBody As String, origin As String) As Boolean
            NotNull(msg, NameOf(msg))

            Dim params As SqlParameter() = {
                New SqlParameter("@From", ConcatNonEmptyStrings(", ", {msg.From, msg.FromName})),
                New SqlParameter("@To", ConcatNonEmptyStrings(",", msg.Recipients)),
                New SqlParameter("@Cc", ConcatNonEmptyStrings(",", msg.CopyRecipients)),
                New SqlParameter("@Bcc", Nothing),
                New SqlParameter("@Subject", msg.Subject),
                New SqlParameter("@PlainTextBody", plainTextBody),
                New SqlParameter("@HtmlBody", htmlBody),
                New SqlParameter("@Origin", origin)
            }

            Return (DB.SPReturnValue("dbo.LogEmail", params) = 0)
        End Function

    End Module
End Namespace