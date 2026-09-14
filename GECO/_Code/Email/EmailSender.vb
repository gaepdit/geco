Imports GECO.EmailAPI
Imports System.Net.Mail

Public Module EmailSender

    ' If running locally, an SMTP server is probably not available. An exception will occur at `smtpClient.Send(msg)`,
    ' and the email will be saved in the undeliverable email folder. To avoid the exception, either set 
    ' `EnableSendingEmail` to false, or run a dev SMTP server such as https://github.com/rnwood/smtp4dev

    Public ReadOnly Property GecoContactEmail As String = ConfigurationManager.AppSettings("GecoContactEmail")
    Private ReadOnly GecoEmailSender As String = ConfigurationManager.AppSettings("GecoEmailSender")
    Private ReadOnly GecoContactName As String = ConfigurationManager.AppSettings("GecoContactName")

    ''' <summary>
    ''' Sends an HTML-formatted email and returns true if successful; otherwise false.
    ''' </summary>
    ''' <param name="recipient">Email recipient.</param>
    ''' <param name="subject">The email subject line.</param>
    ''' <param name="body">The HTML-formatted body of the email.</param>
    ''' <param name="caller">The code location calling this method (for logging purposes).</param>
    ''' <returns>True if email is sent successfully; otherwise false.</returns>
    Public Function SendEmail(recipient As String, subject As String, body As String, caller As String) As Boolean
        Dim emails As New List(Of String) From {recipient}
        Return SendEmailInternal(emails, subject, body, isHtml:=True, copyRecipients:=New List(Of String), caller)
    End Function

    ''' <summary>
    ''' Sends an HTML-formatted email and returns true if successful; otherwise false.
    ''' </summary>
    ''' <param name="recipients">List of email recipients.</param>
    ''' <param name="subject">The email subject line.</param>
    ''' <param name="body">The HTML-formatted body of the email.</param>
    ''' <param name="caller">The code location calling this method (for logging purposes).</param>
    ''' <returns>True if email is sent successfully; otherwise false.</returns>
    Public Function SendEmail(recipients As List(Of String), subject As String, body As String, caller As String) As Boolean
        Return SendEmailInternal(recipients, subject, body, isHtml:=True, copyRecipients:=New List(Of String), caller)
    End Function

    ''' <summary>
    ''' Sends an HTML-formatted email and returns true if successful; otherwise false.
    ''' </summary>
    ''' <param name="recipients">List of email recipients.</param>
    ''' <param name="subject">The email subject line.</param>
    ''' <param name="body">The HTML-formatted body of the email.</param>
    ''' <param name="copyRecipients">List of email CC recipients.</param>
    ''' <param name="caller">The code location calling this method (for logging purposes).</param>
    ''' <returns>True if email is sent successfully; otherwise false.</returns>
    Public Function SendEmail(recipients As List(Of String), subject As String, body As String, copyRecipients As List(Of String), caller As String) As Boolean
        Return SendEmailInternal(recipients, subject, body, isHtml:=True, copyRecipients, caller)
    End Function

    ''' <summary>
    ''' Sends an email and returns true if successful; otherwise false.
    ''' </summary>
    ''' <param name="recipients">List of email recipients.</param>
    ''' <param name="subject">The email subject line.</param>
    ''' <param name="body">The body of the email.</param>
    ''' <param name="isHtml">A flag indicating whether the email is HTML-formatted or not.</param>
    ''' <param name="copyRecipients">List of email CC recipients.</param>
    ''' <param name="caller">The code location calling this method (for logging purposes).</param>
    ''' <returns>True if email is sent successfully; otherwise false.</returns>
    Private Function SendEmailInternal(recipients As List(Of String),
                                       subject As String,
                                       body As String,
                                       isHtml As Boolean,
                                       copyRecipients As List(Of String),
                                       caller As String
                                       ) As Boolean

        If String.IsNullOrWhiteSpace(body) Then
            Throw New ArgumentException("Message body required.")
        End If

        If recipients.Count = 0 OrElse recipients.Any(Function(r) r.Length = 0) Then
            Throw New ArgumentNullException(NameOf(recipients), "Non-empty recipient addresses required.")
        End If

        If String.IsNullOrWhiteSpace(subject) Then
            Throw New ArgumentException("Message subject required.", NameOf(subject))
        End If

        LabelSubject(subject)

        Dim email As New Email With {
            .Recipients = recipients,
            .Subject = subject,
            .From = GecoEmailSender,
            .FromName = GecoContactName,
            .Body = body,
            .CopyRecipients = copyRecipients,
            .IsHtml = isHtml
        }

        Dim origin As String = ConcatNonEmptyStrings(" | User ID: ", {ConcatNonEmptyStrings(".", {"GECO", caller}), GetCurrentUser()?.UserId.ToString})

        If isHtml Then
            DAL.LogEmail(email, Nothing, body, origin)
        Else
            DAL.LogEmail(email, body, Nothing, origin)
        End If

        EmailQueueApi.QueueEmail(email)
        Return True
    End Function

    Private Sub LabelSubject(ByRef subject As String)
        Dim environment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")
        If environment = "Development" Then subject = "[GECO DEV] " & subject
        If environment = "Staging" Then subject = "[GECO UAT] " & subject
    End Sub

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
