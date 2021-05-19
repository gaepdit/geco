Imports GECO.GecoModels
Imports Mindscape.Raygun4Net
Imports Mindscape.Raygun4Net.Messages

Public Class RaygunInfo
    Public ReadOnly Property User As RaygunIdentifierMessage = GetRaygunIdentifier()
    Public ReadOnly Property ApiKey As String = CType(ConfigurationManager.GetSection("RaygunSettings"), RaygunSettings).ApiKey
    Public ReadOnly Property Environment As String = ConfigurationManager.AppSettings("GECO_ENVIRONMENT")
    Public ReadOnly Property Version As String = ConfigurationManager.AppSettings("GECO_VERSION")
    Public ReadOnly Property IsAnonymous As String = User.IsAnonymous.ToString.ToLower()

    Public Shared Function GetRaygunIdentifier() As RaygunIdentifierMessage
        Dim user As GecoUser = GetCurrentUser()

        If user Is Nothing Then
            Return New RaygunIdentifierMessage("") With {
                .IsAnonymous = True
            }
        Else
            Return New RaygunIdentifierMessage(user.UserId.ToString) With {
                .Email = user.Email,
                .FirstName = user.FirstName,
                .FullName = user.FullName,
                .IsAnonymous = False
            }
        End If
    End Function

End Class
