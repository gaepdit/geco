Imports System.Threading.Tasks

Namespace EmailTemplates
    Public Module UserAccountEmails

        Public Async Function SendConfirmEmailUpdateEmailAsync(email As String, token As String) As Task
            Dim partialUrl As String = $"~/Account.aspx?action=update&acct={Trim(email)}&token={token}"
            Dim confirmationUrl As String = FullyQualifiedUrl(partialUrl)

            Dim subject As String = "GECO: Confirm new email address"

            Dim body As String = "<p>Confirm and activate your new email address by clicking this link: <br /> " &
                "<a href='{0}' target='_blank'>Confirm your email address</a></p>" &
                "<p>The link expires after 2 hours.</p>" &
                "<p>If the above link doesn't work, copy and paste the following into your web browser:<br /> {0} </p>"

            Await SendEmailAsync(Trim(email), subject, String.Format(body, confirmationUrl),
                          "UserAccountEmails.SendConfirmEmailUpdateEmail")
        End Function

        Public Async Function SendConfirmAccountEmailAsync(email As String, token As String) As Task(Of Boolean)
            Dim partialUrl As String = $"~/Account.aspx?action=confirm&acct={Trim(email)}&token={token}"
            Dim confirmationUrl As String = FullyQualifiedUrl(partialUrl)

            Dim subject As String = "GECO: Confirm new account"

            Dim body As String = "<p>Thank you for creating an account at " &
                "Georgia Environmental Connections Online (GECO).</p> " &
                "<p>Confirm and activate your account by clicking this link: <br /> " &
                "<a href='{0}' target='_blank'>Confirm account</a></p>" &
                "<p>The link expires after 2 hours.</p>" &
                "<p>If the above link doesn't work, copy and paste the following into your web browser:<br /> {0} </p>"

            Return Await SendEmailAsync(Trim(email), subject, String.Format(body, confirmationUrl),
                                 "UserAccountEmails.SendConfirmAccountEmail")
        End Function

        Public Async Function SendPasswordResetEmailAsync(email As String, token As String) As Task
            Dim partialUrl As String = $"~/Account.aspx?action=reset&acct={Trim(email)}&token={token}"
            Dim confirmationUrl As String = FullyQualifiedUrl(partialUrl)

            Dim subject As String = "GECO: Password Reset"

            Dim body As String = "<p>A password reset was requested for this account at " &
                "Georgia Environmental Connections Online (GECO). You can reset your password " &
                "by clicking this link. The link expires after 2 hours.</p> " &
                "<p><a href='{0}' " &
                "style='display:inline-block;border:10px solid darkblue;background:darkblue;color:white;border-radius:3px;border-left-width:15px;border-right-width:15px;'>" &
                "Reset password</a></p>" &
                "<p>If you did not request a password reset, you can ignore this message, and your password will not change.</p>" &
                "<p>If the above link doesn't work, copy and paste the following into your web browser:<br /> {0} </p>"

            Await SendEmailAsync(Trim(email), subject, String.Format(body, confirmationUrl),
                          "UserAccountEmails.SendPasswordResetEmail")
        End Function

        Public Async Function SendPasswordChangeNotificationAsync(email As String) As Task
            Dim subject As String = "GECO: Password Changed"

            Dim body As String = "<p>The password for this account at " &
                "Georgia Environmental Connections Online (GECO) was recently changed.</p> " &
                "<p>Account: {0}</p>" &
                "<p>If you did not initiate this change, please contact the Air Protection Branch.</p>"

            Await SendEmailAsync(Trim(email), subject, String.Format(body, email),
                          "UserAccountEmails.SendPasswordChangeNotification")
        End Function

        Public Async Function SendEmailChangeNotificationAsync(oldEmail As String, newEmail As String) As Task
            Dim subject As String = "GECO: Email Address Changed"

            Dim body As String = "<p>The email address for this account at " &
                "Georgia Environmental Connections Online (GECO) was recently changed.</p> " &
                "<p>Old email: {0} <br /> " &
                "New email: {1} </p>" &
                "<p>If you did not initiate this change, please contact the Air Protection Branch.</p>"

            Dim emails As New List(Of String) From {Trim(newEmail), Trim(oldEmail)}

            Await SendEmailAsync(emails, subject, String.Format(body, oldEmail, newEmail),
                          "UserAccountEmails.SendEmailChangeNotification")
        End Function

    End Module
End Namespace
