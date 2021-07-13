Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Namespace EmailTemplates
    Public Module FacilityContactEmails

        Public Function SendEmailContactConfirmationEmail(facilityId As ApbFacilityId,
                                                          email As String,
                                                          category As CommunicationCategory,
                                                          seq As String,
                                                          token As String) As Boolean

            Dim partialUrl As String = String.Format($"~/Facility/ConfirmEmail.aspx?seq={seq}&token={token}")
            Dim confirmationUrl As String = FullyQualifiedUrl(partialUrl)

            Dim subject As String = "GECO: Confirm your email"

            Dim plainBody As String = "Your email address has been added at " &
                "Georgia Environmental Connections Online (GECO) to receive " &
                "electronic communication for the following facility. " &
                vbNewLine & vbNewLine &
                " * Email: " & email & vbNewLine &
                " * AIRS Number: " & facilityId.FormattedString & vbNewLine &
                " * Facility: " & GetFacilityNameAndCity(facilityId) & vbNewLine &
                " * Category: " & category.Description &
                vbNewLine & vbNewLine &
                "Confirm and approve your email address using this link: " &
                vbNewLine & "{0}" &
                vbNewLine & vbNewLine &
                "The link expires after 2 hours."

            Dim htmlBody As String = "<p>Your email address has been added at " &
                "Georgia Environmental Connections Online (GECO) to receive " &
                "electronic communication for the following facility.</p> " &
                "<ul><li>Email: " & email &
                "</li><li>AIRS Number: " & facilityId.FormattedString &
                "</li><li>Facility: " & GetFacilityNameAndCity(facilityId) &
                "</li><li>Category: " & category.Description & "</li></ul>" &
                "<p>Confirm and approve your email address using this link: <br /> " &
                "<a href='{0}' target='_blank'>Confirm email</a></p>" &
                "<p>The link expires after 2 hours.</p>"

            Return SendEmail(Trim(email), subject,
                      String.Format(plainBody, confirmationUrl),
                      String.Format(htmlBody, confirmationUrl),
                      caller:="FacilityContactEmails.SendEmailContactConfirmationEmail")
        End Function

    End Module
End Namespace
