Public Module UserAccountEmails

    Public Sub SendConfirmEmailUpdateEmail(email As String, token As String)
        Dim partialUrl As String = String.Format("~/Account.aspx?action=update&acct={0}&token={1}", Trim(email), token)
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
        Dim partialUrl As String = String.Format("~/Account.aspx?action=confirm&acct={0}&token={1}", Trim(email), token)
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
        Dim partialUrl As String = String.Format("~/Account.aspx?action=reset&acct={0}&token={1}", Trim(email), token)
        Dim confirmationUrl As String = FullyQualifiedUrl(partialUrl)

        Dim subject As String = "GECO: Password Reset"

        Dim plainBody As String = "A password reset was requested for this account at " &
            "Georgia Environmental Connections Online (GECO). " &
            vbNewLine & vbNewLine &
            "Reset your password using this link: " &
            vbNewLine & "{0}" &
            vbNewLine & vbNewLine &
            "The link expires after 2 hours."

        Dim htmlBody As String = "<p>A password reset was requested for this account at " &
            "Georgia Environmental Connections Online (GECO).</p> " &
            "<p>Reset your password using this link: <br /> " &
            "<a href='{0}' target='_blank'>Reset password</a></p>" &
            "<p>The link expires after 2 hours.</p>"

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
            "If you did not initiate this change, please contact the Air Protection Branch."

        Dim htmlBody As String = "<p>The password for this account at " &
            "Georgia Environmental Connections Online (GECO) was recently changed.</p> " &
            "<p>If you did not initiate this change, please contact the Air Protection Branch.</p>"

        SendEmail(Trim(email), subject, plainBody, htmlBody,
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
                  String.Format(htmlBody, oldEmail, newEmail))
    End Sub

End Module
