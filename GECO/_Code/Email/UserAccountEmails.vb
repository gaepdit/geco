Namespace EmailTemplates
    Public Module UserAccountEmails

        Public Sub SendConfirmEmailUpdateEmail(email As String, token As String)
            Dim partialUrl As String = $"~/Account.aspx?action=update&acct={Trim(email)}&token={token}"
            Dim confirmationUrl As String = FullyQualifiedUrl(partialUrl)

            Dim subject As String = "GECO: Confirm new email address"

            Dim plainBody As String =
                "Confirm and activate your new email address using this link: " &
                vbNewLine & "{0}" &
                vbNewLine & vbNewLine &
                "The link expires after 2 hours."

            Dim htmlBody As String = "<p>Confirm and activate your new email address using this link: <br /> " &
                "<a href='{0}' target='_blank'>Confirm your email address</a></p>" &
                "<p>The link expires after 2 hours.</p>"

            SendEmail(Trim(email), subject,
                      String.Format(plainBody, confirmationUrl),
                      String.Format(htmlBody, confirmationUrl),
                      caller:="UserAccountEmails.SendConfirmEmailUpdateEmail")
        End Sub

        Public Function SendConfirmAccountEmail(email As String, token As String) As Boolean
            Dim partialUrl As String = $"~/Account.aspx?action=confirm&acct={Trim(email)}&token={token}"
            Dim confirmationUrl As String = FullyQualifiedUrl(partialUrl)

            Dim subject As String = "GECO: Confirm new account"

            Dim plainBody As String = "Thank you for creating an account at " &
                "Georgia Environmental Connections Online (GECO). " &
                vbNewLine & vbNewLine &
                "Confirm and activate your account using this link: " &
                vbNewLine & "{0}" &
                vbNewLine & vbNewLine &
                "The link expires after 2 hours."

            Dim htmlBody As String = "<p>Thank you for creating an account at " &
                "Georgia Environmental Connections Online (GECO).</p> " &
                "<p>Confirm and activate your account using this link: <br /> " &
                "<a href='{0}' target='_blank'>Confirm account</a></p>" &
                "<p>The link expires after 2 hours.</p>"

            Return SendEmail(Trim(email), subject,
                      String.Format(plainBody, confirmationUrl),
                      String.Format(htmlBody, confirmationUrl),
                      caller:="UserAccountEmails.SendConfirmAccountEmail")
        End Function

        Public Sub SendPasswordResetEmail(email As String, token As String)
            Dim partialUrl As String = $"~/Account.aspx?action=reset&acct={Trim(email)}&token={token}"
            Dim confirmationUrl As String = FullyQualifiedUrl(partialUrl)

            Dim subject As String = "GECO: Password Reset"

            Dim plainBody As String = "A password reset was requested for this account at " &
                "Georgia Environmental Connections Online (GECO). This link expires after 2 hours." &
                vbNewLine & vbNewLine &
                "{0}" &
                vbNewLine & vbNewLine &
                "If you did not request a password reset, you can ignore this message and your password will not change."

            Dim htmlBody As String = "<p>A password reset was requested for this account at " &
                "Georgia Environmental Connections Online (GECO). This link expires after 2 hours.</p> " &
                "<p><a href='{0}' " &
                "style='display:inline-block;border:10px solid darkblue;background:darkblue;color:white;border-radius:3px;border-left-width:15px;border-right-width:15px;'>" &
                "Reset password</a></p>" &
                "<p>If you did not request a password reset, you can ignore this message and your password will not change.</p>" &
                "<p>If the above link doesn't work, copy and paste the following into your web browser:<br /> {0} </p>"

            SendEmail(Trim(email), subject,
                      String.Format(plainBody, confirmationUrl),
                      String.Format(htmlBody, confirmationUrl),
                      caller:="UserAccountEmails.SendPasswordResetEmail")
        End Sub

        Public Sub SendPasswordChangeNotification(email As String)
            Dim subject As String = "GECO: Password Changed"

            Dim plainBody As String = "The password for this account at " &
                "Georgia Environmental Connections Online (GECO) was recently changed. " &
                vbNewLine & vbNewLine &
                "Account: {0}" &
                vbNewLine & vbNewLine &
                "If you did not initiate this change, please contact the Air Protection Branch."

            Dim htmlBody As String = "<p>The password for this account at " &
                "Georgia Environmental Connections Online (GECO) was recently changed.</p> " &
                "<p>Account: {0}</p>" &
                "<p>If you did not initiate this change, please contact the Air Protection Branch.</p>"

            SendEmail(Trim(email), subject,
                      String.Format(plainBody, email),
                      String.Format(htmlBody, email),
                      caller:="UserAccountEmails.SendPasswordChangeNotification")
        End Sub

        Public Sub SendEmailChangeNotification(oldEmail As String, newEmail As String)
            Dim subject As String = "GECO: Email Address Changed"

            Dim plainBody As String = "The email address for this account at " &
                "Georgia Environmental Connections Online (GECO) was recently changed. " &
                vbNewLine & vbNewLine &
                "Old email: {0} " & vbNewLine &
                "New email: {1} " &
                vbNewLine & vbNewLine &
                "If you did not initiate this change, please contact the Air Protection Branch."

            Dim htmlBody As String = "<p>The email address for this account at " &
                "Georgia Environmental Connections Online (GECO) was recently changed.</p> " &
                "<p>Old email: {0} <br /> " &
                "New email: {1} </p>" &
                "<p>If you did not initiate this change, please contact the Air Protection Branch.</p>"

            SendEmail(ConcatNonEmptyStrings(",", {Trim(newEmail), Trim(oldEmail)}), subject,
                      String.Format(plainBody, oldEmail, newEmail),
                      String.Format(htmlBody, oldEmail, newEmail),
                      caller:="UserAccountEmails.SendEmailChangeNotification")
        End Sub

    End Module
End Namespace
