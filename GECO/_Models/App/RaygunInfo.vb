Imports GECO.GecoModels
Imports Mindscape.Raygun4Net.Messages

Public Module RaygunInfo

    Public Function GetRaygunIdentifier() As RaygunIdentifierMessage
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

End Module
