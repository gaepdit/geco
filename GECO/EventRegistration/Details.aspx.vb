Imports GECO.GecoModels
Imports EpdIt.DBUtilities

Partial Class EventRegistration_EventDetails
    Inherits Page

    Private Property eventId As Integer
    Private Property currentUser As GecoUser
    Private Property passcodeRequired As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString("eventid") Is Nothing OrElse
            Not Integer.TryParse(Request.QueryString("eventid"), eventId) Then

            Response.Redirect(".", False)
            Exit Sub
        End If

        If Not EventExists(eventId) Then
            Throw New HttpException(404, "Event not found")
        End If

        currentUser = GetCurrentUser()

        If Not IsPostBack Then
            'Menu setup
            Dim EasyMenu1 As Sequentum.EasyMenu = CType(Master.FindControl("EasyMenu1"), Sequentum.EasyMenu)
            EasyMenu1.MenuRoot.AddSubMenuItem("Event List", ".")

            lblMessage.Text = ""

            DisplayEventDetails()

            If UserIsLoggedIn() Then
                pnlNotLoggedIn.Visible = False
                pnlLoggedIn.Visible = True
                lblEmail.Text = currentUser.Email

                CheckRegistration()
            Else
                pnlNotLoggedIn.Visible = True
                pnlLoggedIn.Visible = False
                lblEmail.Text = ""
            End If
        End If
    End Sub

#Region " Display event "

    Private Sub DisplayEventDetails()
        Dim dr = GetEventDetails(eventId)

        If dr Is Nothing Then
            Throw New HttpException(404, "Event not found")
        End If

        Dim address As New Address() With {
                    .Street = dr.Item("strAddress").ToString,
                    .City = dr.Item("strCity").ToString,
                    .State = dr.Item("strState").ToString,
                    .PostalCode = dr.Item("numZipCode").ToString
                }

        lblTitle.Text = dr.Item("StrTitle").ToString

        hlLogin.NavigateUrl = "~/?ReturnUrl=" & Page.ResolveUrl("~/EventRegistration/Details.aspx?eventid=") & eventId.ToString

        litEventDetails.Text &= dr.Item("strDescription").ToString

        If Not String.IsNullOrEmpty(dr.Item("strWebURL").ToString) Then
            litEventDetails.Text &= "<br /><br /><strong>Event Website: </strong><a href='" & dr.Item("strWebURL").ToString
            litEventDetails.Text &= "' target='_blank'>" & dr.Item("strWebURL").ToString & "</a>"
        End If

        litEventDetails.Text &= "<p><strong>Date & Time: </strong><br />" & String.Format("{0: MM/dd/yyyy}", CDate(dr.Item("datStartDate").ToString))
        litEventDetails.Text &= ", " & dr.Item("strEventStartTime").ToString
        litEventDetails.Text &= "<br />to "
        If dr.Item("datEndDate").ToString <> "" Then
            litEventDetails.Text &= String.Format("{0: MM/dd/yyyy}", CDate(dr.Item("datEndDate").ToString)) & ", "
        End If
        litEventDetails.Text &= dr.Item("strEventEndTime").ToString

        litEventDetails.Text &= "<br /><br /><strong>Location: </strong><br />" & dr.Item("strVenue").ToString
        litEventDetails.Text &= "<br /><a title='Click to open Google Map' href='https://maps.google.com/?q=" & address.ToLinearString
        litEventDetails.Text &= "' target='_blank'>" & address.ToString
        litEventDetails.Text &= "</a></p>"

        litEventDetails.Text &= "<p><strong>Contact:</strong>"
        litEventDetails.Text &= "<br />" & dr.Item("strFirstName").ToString
        litEventDetails.Text &= " " & dr.Item("strLastName").ToString
        litEventDetails.Text &= "<br />" & dr.Item("strPhone").ToString
        litEventDetails.Text &= "<br /><a href='mailto:" & dr.Item("strEmailAddress").ToString
        litEventDetails.Text &= "'>" & dr.Item("strEmailAddress").ToString & "</a></p>"

        If dr.Item("strNotes").ToString <> "" Then
            litEventDetails.Text &= "<strong>Additional Notes: </strong><br />" & dr.Item("strNotes").ToString
        End If

        Dim total As Integer = dr.Item("numCapacity")
        Dim confirmed As Integer = dr.Item("NumConfirmed")
        Dim waiting As Integer = dr.Item("NumWaitingList")

        DisplayCapacity(total, confirmed, waiting)

        Dim passcode As String = GetNullable(Of String)(dr.Item("strPasscode"))

        If String.IsNullOrEmpty(passcode) OrElse passcode = "1" Then ' No Passcode Required
            passcodeRequired = False
        Else
            SessionAdd(GecoSession.EventPasscode, passcode)
            passcodeRequired = True
        End If
    End Sub

    Private Sub CheckCapacity()
        Dim total As Integer = 0
        Dim confirmed As Integer = 0
        Dim waiting As Integer = 0

        ' Returns Total, Confirmed, WaitingList
        Dim dr = GetEventAvailability(eventId)

        If dr IsNot Nothing Then
            total = dr.Item("Total")
            confirmed = dr.Item("Confirmed")
            waiting = dr.Item("WaitingList")
        End If

        DisplayCapacity(total, confirmed, waiting)
    End Sub

    Private Sub DisplayCapacity(total As Integer, confirmed As Integer, waiting As Integer)
        litCapacity.Text = "<p><strong>Class Capacity:</strong>" &
            "<br />Total capacity: <em>" & total & "</em>" &
            "<br />Available seats: <em>" & (total - confirmed) & "</em>"

        If waiting > 0 Then
            litCapacity.Text &= "<br />Waiting List: <em>" & waiting & "</em>"
        End If

        litCapacity.Text &= "</p>"
    End Sub

#End Region

#Region " Display Registration Status "

    Private Sub CheckRegistration()
        If UserIsRegisteredForEvent(eventId, currentUser.UserId) Then
            pnlPasscode.Visible = False
            pnlRegister.Visible = True
            btnRegister.Visible = False
            btnCancelRegistration.Visible = True

            DisplayRegistration()
        Else
            litConfirmation.Text = "<b>Register for this event here.</b>"
            pnlPasscode.Visible = passcodeRequired
            pnlRegister.Visible = Not passcodeRequired
            btnRegister.Visible = True
            btnCancelRegistration.Visible = False
        End If
    End Sub

    Private Sub DisplayRegistration()
        Dim dr = GetRegistrationStatus(eventId, currentUser.UserId)
        Dim status As Integer = dr.Item("StatusCode")

        If dr IsNot Nothing Then
            litConfirmation.Text = "<b>Status:</b> " & If(status = 1, "REGISTERED", "WAITING LIST")
            litConfirmation.Text &= "<br />You registered for this event on "
            litConfirmation.Text &= String.Format("{0:M/d/yyyy}", dr.Item("RegistrationDate")) & "."

            If status = 2 Then
                litConfirmation.Text &= "<br /><br /><em>The event is currently full, but you have been placed on the waiting list.</em>"
            End If

            litConfirmation.Text &= "<br /><br /><b>Confirmation code:</b> " & dr.Item("ConfirmationCode").ToString

            litConfirmation.Text &= "<br /><br /><b>You may cancel your registration here.</b>"

            txtComments.Text = dr.Item("Comments").ToString
        End If
    End Sub

#End Region

#Region " Registration "

    Protected Sub btnPasscode_Click(sender As Object, e As EventArgs) Handles btnPasscode.Click
        If txtPasscode.Text = GetSessionItem(GecoSession.EventPasscode).ToString Then
            pnlPasscode.Visible = False
            pnlRegister.Visible = True
        Else
            lblPasscodeWrong.Visible = True
        End If
    End Sub

    Protected Sub btnRegister_Click(sender As Object, e As EventArgs) Handles btnRegister.Click
        Dim confirmationNumber As String = RandomString(10)

        Dim status As Integer = 0
        Dim result As DbResult = RegisterUserForEvent(currentUser.UserId, eventId, confirmationNumber, txtComments.Text, status)

        Select Case result
            Case DbResult.Success
                lblMessage.Text = "You have been successfully registered."
                SendRegistrationEmail(confirmationNumber, status)
            Case Else
                lblMessage.Text = "There was a problem registering you. Please try again or contact us."
        End Select

        CheckRegistration()
        CheckCapacity()
    End Sub

    Private Sub SendRegistrationEmail(confirmationNumber As String, status As Integer)
        Dim subject As String = "GA EPD Event Registration"

        Dim linkPath As String = Page.ResolveUrl("~/EventRegistration/Details.aspx") & "?eventid=" & eventId.ToString
        Dim linkUri As Uri = New Uri(New Uri(Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath), linkPath)

        Dim htmlBody As String = "<p>Dear " & currentUser.FullName & ", </p>" &
            "<p>Thank you for registering for the following event. "

        If status = 2 Then
            htmlBody &= "<em>The event is currently full, but you have been placed on the waiting list.</em></p>"
        End If

        htmlBody &= "</p><p>To view your registration status or make changes, visit: <br />" & linkUri.ToString & " </p>" &
            "<p><b>Event Details:</b></p>" &
            litEventDetails.Text

        SendEmail(currentUser.Email, subject, Nothing, htmlBody)
    End Sub

#End Region

#Region " Cancellation "

    Protected Sub btnCancelRegistration_Click(sender As Object, e As EventArgs) Handles btnCancelRegistration.Click
        Dim result As DbResult = CancelEventRegistration(currentUser.UserId, eventId, txtComments.Text)

        Select Case result
            Case DbResult.Success
                lblMessage.Text = "Your registration has been canceled."
                SendCancellationEmail()
            Case Else
                lblMessage.Text = "There was an error. Please try again or contact us."
        End Select

        CheckRegistration()
        CheckCapacity()
    End Sub

    Private Sub SendCancellationEmail()
        Dim subject As String = "GA EPD Event Registration"

        Dim linkPath As String = Page.ResolveUrl("~/EventRegistration/Details.aspx") & "?eventid=" & eventId.ToString
        Dim linkUri As Uri = New Uri(New Uri(Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath), linkPath)

        Dim htmlBody As String = "<p>Dear " & currentUser.FullName & ", </p>" &
            "<p>Your registration for the following event has been <b>canceled.</b></p>" &
            "<p>To view the event or renew your registration, please visit: <br />" & linkUri.ToString & " </p>" &
            "<p><b>Event Details:</b></p>" &
            litEventDetails.Text

        SendEmail(currentUser.Email, subject, Nothing, htmlBody)
    End Sub

#End Region

End Class
