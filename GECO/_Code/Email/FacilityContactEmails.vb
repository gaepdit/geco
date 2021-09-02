Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Namespace EmailTemplates
    Public Module FacilityContactEmails

        Public Function SendEmailContactNotificationEmail(facilityId As ApbFacilityId,
                                                          email As String,
                                                          category As CommunicationCategory
                                                          ) As Boolean

            Dim subject As String = "GECO: Email added as facility contact"

            Dim plainBody As String = "Your email address has been added at " &
                "Georgia Environmental Connections Online (GECO) to receive " &
                "electronic communication for the following facility. " &
                vbNewLine & vbNewLine &
                " * Email: " & email & vbNewLine &
                " * AIRS Number: " & facilityId.FormattedString & vbNewLine &
                " * Facility: " & GetFacilityNameAndCity(facilityId) & vbNewLine &
                " * Category: " & category.Description &
                vbNewLine & vbNewLine &
                "Please contact the Georgia Air Protection Branch if you need assistance."

            Dim htmlBody As String = "<p>Your email address has been added at " &
                "Georgia Environmental Connections Online (GECO) to receive " &
                "electronic communication for the following facility.</p> " &
                "<ul><li>Email: " & email &
                "</li><li>AIRS Number: " & facilityId.FormattedString &
                "</li><li>Facility: " & GetFacilityNameAndCity(facilityId) &
                "</li><li>Category: " & category.Description & "</li></ul>" &
                "<p>Please contact the Georgia Air Protection Branch if you need assistance.</p>"

            Return SendEmail(Trim(email), subject, plainBody, htmlBody,
                      caller:="FacilityContactEmails.SendEmailContactConfirmationEmail")
        End Function

    End Module
End Namespace
