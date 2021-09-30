Imports System.IO
Imports System.Net.Mail

Public Module EmailSender

    ' If running locally, an SMTP server is probably not available. An exception will occur at `smtpClient.Send(msg)`,
    ' and the email will be saved in the undeliverable email folder. To avoid the exception, either set 
    ' `EnableSendingEmail` to false, or run a dev SMTP server such as https://github.com/rnwood/smtp4dev

    Public ReadOnly Property GecoContactEmail As String = ConfigurationManager.AppSettings("GecoContactEmail")

    Private ReadOnly GecoEmailSender As String = ConfigurationManager.AppSettings("GecoEmailSender")
    Private ReadOnly GecoContactName As String = ConfigurationManager.AppSettings("GecoContactName")
    Private ReadOnly SaveAllEmails As Boolean = CBool(ConfigurationManager.AppSettings("SaveAllEmails")) ' Useful for debugging but excessive for normal use.
    Private ReadOnly EnableSendingEmail As Boolean = CBool(ConfigurationManager.AppSettings("EnableSendingEmail"))
    Private ReadOnly SmtpHost As String = ConfigurationManager.AppSettings("SmtpHost")
    Private ReadOnly SmtpPort As Integer = CInt(ConfigurationManager.AppSettings("SmtpPort"))
    Private ReadOnly EmailAllowList As String() = ConfigurationManager.AppSettings("EmailAllowList").Split(";"c)

    ''' <summary>
    ''' Sends an email and returns true if successful; otherwise false.
    ''' </summary>
    ''' <param name="toAddresses">The email recipients, separated by commas.</param>
    ''' <param name="mailSubject">The email subject line.</param>
    ''' <param name="plainTextBody">The plain text body of the email.</param>
    ''' <param name="htmlBody">The optional HTML formatted body of the email.</param>
    ''' <param name="ccAddresses">The email cc recipients, separated by commas.</param>
    ''' <param name="mailPriority">MailPriority of the email.</param>
    ''' <returns>True if email is sent successfully; otherwise false.</returns>
    ''' <remarks>See https://stackoverflow.com/q/9736176/212978 for details on adding 
    ''' multiple recipients.</remarks>
    Public Function SendEmail(toAddresses As String,
                              mailSubject As String,
                              Optional plainTextBody As String = Nothing,
                              Optional htmlBody As String = Nothing,
                              Optional ccAddresses As String = Nothing,
                              Optional mailPriority As MailPriority = MailPriority.Normal,
                              Optional caller As String = NameOf(EmailSender)
                              ) As Boolean

        If String.IsNullOrWhiteSpace(plainTextBody) AndAlso String.IsNullOrWhiteSpace(htmlBody) Then
            Throw New ArgumentException("Message body required.")
        End If

        If String.IsNullOrEmpty(toAddresses) Then
            Throw New ArgumentNullException(NameOf(toAddresses), "Recipient address required.")
        End If

        If String.IsNullOrWhiteSpace(mailSubject) Then
            mailSubject = "Message from " & GecoContactName
        End If

        Dim environment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")

        If environment = "Development" Then
            mailSubject = "[GECO DEV] " & mailSubject
        End If

        If environment = "Staging" Then
            mailSubject = "[GECO UAT] " & mailSubject
        End If

        Dim msg As New MailMessage With {
            .From = New MailAddress(GecoEmailSender, GecoContactName),
            .Subject = mailSubject,
            .SubjectEncoding = Encoding.UTF8,
            .Priority = mailPriority
        }

        msg.To.Add(toAddresses)

        If Not String.IsNullOrWhiteSpace(ccAddresses) Then
            msg.CC.Add(ccAddresses)
        End If

        If plainTextBody IsNot Nothing Then
            msg.Body = plainTextBody
            msg.BodyEncoding = Encoding.UTF8
        End If

        If htmlBody IsNot Nothing Then
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlBody, Encoding.UTF8, "text/html"))
        End If

        Dim origin As String = ConcatNonEmptyStrings(" | User ID: ", {ConcatNonEmptyStrings(".", {"GECO", caller}), GetCurrentUser()?.UserId.ToString})

        DAL.LogEmail(msg, plainTextBody, htmlBody, origin)


        ' If email sending is disabled, save to file and return.
        If Not EnableSendingEmail Then
            Return SaveLocalEmail(msg)
        End If

        ' If not production, limit recipients to the "EmailAllowList".
        If environment <> "Production" Then
            Dim originalRecipients As MailAddress() = msg.To.ToArray

            For Each recipient As MailAddress In originalRecipients
                If Not EmailAllowList.Contains(recipient.Address) Then
                    msg.To.Remove(recipient)
                End If
            Next

            ' If no recipients are in allowlist, then add original recipients back and save to file.
            If msg.To.Count = 0 Then
                For Each recipient As MailAddress In originalRecipients
                    msg.To.Add(recipient)
                Next

                Return SaveLocalEmail(msg)
            End If

            ' Also clear CC list for non-production server before sending.
            msg.CC.Clear()
        End If

        Try
            Using smtpClient As New SmtpClient(SmtpHost, SmtpPort)
                smtpClient.Send(msg)
            End Using

            If SaveAllEmails Then
                SaveLocalEmail(msg)
            End If
        Catch ex As Exception
            ' If message send failure, save file, log & return
            SaveLocalEmail(msg)
            ErrorReport(ex, False)
            Return False
        End Try

        Return True
    End Function

    Private Function SaveLocalEmail(msg As MailMessage) As Boolean
        Try
            Dim folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings("UndeliverableEmailFolder"))

            Using smtpClient As New SmtpClient()
                Directory.CreateDirectory(folder)
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory
                smtpClient.PickupDirectoryLocation = folder
                smtpClient.Send(msg)
            End Using

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    <CodeAnalysis.SuppressMessage("Minor Code Smell", "S1481:Unused local variables should be removed", Justification:="MailAddress created to test valid format")>
    Public Function IsValidEmailAddress(emailAddress As String) As Boolean
        If String.IsNullOrEmpty(emailAddress) Then Return False

        Try
            Dim testEmail As New MailAddress(emailAddress)
        Catch ex As Exception
            Return False
        End Try

        Return True
    End Function

End Module
