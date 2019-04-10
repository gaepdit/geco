Imports System.IO
Imports System.Net.Mail
Imports System.Net.Mime

Public Module EmailSender

    Public GecoEmailSender As String = ConfigurationManager.AppSettings("GecoEmailSender")
    Public GecoContactEmail As String = ConfigurationManager.AppSettings("GecoContactEmail")
    Public GecoContactName As String = ConfigurationManager.AppSettings("GecoContactName")
    Public SaveAllEmails As Boolean = ConfigurationManager.AppSettings("SaveAllEmails")
    Public EnableSendingEmail As Boolean = ConfigurationManager.AppSettings("EnableSendingEmail")

    ''' <summary>
    ''' Sends an email and returns true if successful; otherwise false.
    ''' </summary>
    ''' <param name="toAddresses">The email recipients, separated by commas.</param>
    ''' <param name="mailSubject">The email subject line.</param>
    ''' <param name="plainTextBody">The plaintext body of the email.</param>
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

        Dim msg As MailMessage = New MailMessage With {
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
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(htmlBody, New ContentType(MediaTypeNames.Text.Html)))
        End If

        Dim origin As String = ConcatNonEmptyStrings(" | User ID: ", {ConcatNonEmptyStrings(".", {"GECO", caller}), GetCurrentUser()?.UserId.ToString})

        DAL.LogEmail(msg, plainTextBody, htmlBody, origin)

        Dim environment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")

        If HttpContext.Current.Request.IsLocal OrElse Not EnableSendingEmail Then
            ' If running locally, just save file (there is probably no SMTP server)
            SaveLocalEmail(msg)
        Else
            If environment <> "Production" Then
                ' If not production, replace recipients with local contacts
                msg.To.Clear()
                msg.To.Add(GecoContactEmail)
                msg.CC.Clear()
            End If

            Try
                Using smtpClient As New SmtpClient("smtp.gets.ga.gov")
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
        End If

        Return True
    End Function

    Private Sub SaveLocalEmail(msg As MailMessage)
        Dim folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings("UndeliverableEmailFolder"))

        Using smtpClient As New SmtpClient()
            Directory.CreateDirectory(folder)
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory
            smtpClient.PickupDirectoryLocation = folder
            smtpClient.Send(msg)
        End Using
    End Sub

    <CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId:="testEmail")>
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
