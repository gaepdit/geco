Imports Microsoft.Data.SqlClient
Imports GaEpd.DBUtilities
Imports GECO.GecoModels

Public Module UserAccounts

    Public Function LogInUser(email As String, password As String, remember As Boolean, ipAddress As String, ByRef user As GecoUser, ByRef userSession As UserSession) As LoginResult
        Dim params As SqlParameter() = {
            New SqlParameter("@Email", email.Trim()),
            New SqlParameter("@Password", GetMd5Hash(password)),
            New SqlParameter("@CreateSession", remember),
            New SqlParameter("@IPAddress", ipAddress)
        }
        Dim result As Integer
        Dim ds As DataSet = DB.SPGetDataSet("geco.LogInUser", params, result)

        Select Case result
            Case 0
                If ds IsNot Nothing AndAlso ds.Tables.Count > 1 Then
                    user = ParseUserFromDataRow(ds.Tables(0).Rows(0))

                    ds.Tables(1).PrimaryKey = {ds.Tables(1).Columns(0)}
                    user.FacilityAccessTable = ds.Tables(1)

                    If remember AndAlso ds.Tables.Count = 3 Then
                        Dim dr As DataRow = ds.Tables(2).Rows(0)
                        userSession = New UserSession(dr(0).ToString(), dr(1).ToString())
                    End If
                End If

                Return LoginResult.Success
            Case 1
                Return LoginResult.Invalid
            Case 2
                Return LoginResult.AccountUnconfirmed
            Case 3
                Return LoginResult.LoginThrottled
            Case Else
                Return LoginResult.DbError
        End Select
    End Function

    Public Enum LoginResult
        Success
        Invalid
        AccountUnconfirmed
        LoginThrottled
        DbError
    End Enum

    Public Function GetCurrentUser() As GecoUser
        Return GetSessionItem(Of GecoUser)(GecoSession.CurrentUser)
    End Function

    Private Function ParseUserFromDataRow(dr As DataRow) As GecoUser
        Dim userId As Integer = GetNullable(Of Integer)(dr("UserId"))

        If userId = 0 Then
            Return Nothing
        End If

        Dim user As New GecoUser(userId) With {
            .Email = GetNullableString(dr("Email")).Trim(),
            .FirstName = GetNullableString(dr("FirstName")),
            .LastName = GetNullableString(dr("LastName")),
            .Title = GetNullableString(dr("Title")),
            .Company = GetNullableString(dr("Company")),
            .PhoneNumber = Person.ResolvePhoneNumbers(GetNullableString(dr("Phone")), GetNullableString(dr("UnformattedPhone"))),
            .GecoUserType = GetNullableString(dr("UserType")),
            .Address = New Address() With {
                .Street = GetNullableString(dr("Street")),
                .City = GetNullableString(dr("City")),
                .State = GetNullableString(dr("State")),
                .PostalCode = GetNullableString(dr("PostalCode"))
            },
            .ProfileUpdateRequired = CBool(dr("UpdateRequired"))
        }


        Return user
    End Function

    Public Function GetGecoUser(userId As Integer) As GecoUser
        Dim dr As DataRow = DB.SPGetDataRow("geco.GetGecoUser", New SqlParameter("@UserId", userId))

        If dr Is Nothing Then
            Return Nothing
        End If

        Return ParseUserFromDataRow(dr)
    End Function

    Public Function GecoUserExists(email As String) As Boolean
        Dim query = "select geco.UserExists(@email)"

        Dim param As New SqlParameter("@email", email.Trim())

        Return DB.GetBoolean(query, param)
    End Function

    Public Function CreateGecoAccount(email As String, password As String, ByRef token As String) As DbResult
        Dim params As SqlParameter() = {
                New SqlParameter("@UserEmail", email),
                New SqlParameter("@Password", GetMd5Hash(password))
            }

        Dim returnValue As Integer
        token = DB.SPGetString("geco.CreateUser", params, returnValue)

        Select Case returnValue
            Case 0
                Return DbResult.Success
            Case 1
                Return DbResult.Failure
        End Select

        Return DbResult.DbError
    End Function

    Public Function RenewAccountToken(email As String, ByRef token As String) As DbResult
        Dim param As New SqlParameter("@Email", email)

        Dim returnValue As Integer
        token = DB.SPGetString("geco.RenewAccountToken", param, returnValue)

        Select Case returnValue
            Case 0
                Return DbResult.Success
            Case 1
                Return DbResult.Failure
        End Select

        Return DbResult.DbError
    End Function

    Public Function ConfirmAccount(email As String, token As String) As DbResult
        Dim params As SqlParameter() = {
            New SqlParameter("@Email", email.Trim()),
            New SqlParameter("@Token", token)
        }

        Dim result As Integer = DB.SPReturnValue("geco.ConfirmAccount", params)

        Select Case result
            Case 0
                Return DbResult.Success
            Case 1
                Return DbResult.Failure
        End Select

        Return DbResult.DbError
    End Function

    Public Function ConfirmEmailChange(email As String, token As String, ByRef oldEmail As String) As DbResult
        Dim params As SqlParameter() = {
            New SqlParameter("@NewEmail", email.Trim()),
            New SqlParameter("@Token", token)
        }

        Dim returnValue As Integer
        oldEmail = DB.SPGetString("geco.ConfirmEmailChange", params, returnValue)

        Select Case returnValue
            Case 0
                Return DbResult.Success
            Case 1
                Return DbResult.Failure
        End Select

        Return DbResult.DbError
    End Function

    Public Function SetAccountPassword(email As String, password As String) As DbResult
        Dim params As SqlParameter() = {
            New SqlParameter("@Email", email.Trim()),
            New SqlParameter("@Password", GetMd5Hash(password))
        }

        Dim result As Integer = DB.SPReturnValue("geco.SetAccountPassword", params)

        Select Case result
            Case 0
                Return DbResult.Success
            Case 1
                Return DbResult.Failure
        End Select

        Return DbResult.DbError
    End Function

    Public Function UpdatePassword(email As String, oldPassword As String, newPassword As String) As UpdatePasswordResult
        Dim params As SqlParameter() = {
            New SqlParameter("@Email", email.Trim()),
            New SqlParameter("@OldPassword", GetMd5Hash(oldPassword)),
            New SqlParameter("@NewPassword", GetMd5Hash(newPassword))
        }

        Dim result As Integer = DB.SPReturnValue("geco.UpdatePassword", params)

        Select Case result
            Case 0
                Return UpdatePasswordResult.Success
            Case 1
                Return UpdatePasswordResult.InvalidEmail
            Case 2
                Return UpdatePasswordResult.InvalidPassword
            Case Else
                Return UpdatePasswordResult.DbError
        End Select
    End Function

    '  0 - Successfully reset user password
    '  1 - Account does Not exist
    '  2 - Old password Is invalid
    ' -1 - Error
    Public Enum UpdatePasswordResult
        Success
        InvalidEmail
        InvalidPassword
        DbError
    End Enum

    Public Function UpdateUserEmail(email As String, newEmail As String, ByRef token As String) As UpdateUserEmailResult
        Dim params As SqlParameter() = {
            New SqlParameter("@OldEmail", email.Trim()),
            New SqlParameter("@NewEmail", newEmail.Trim())
        }

        Dim result As Integer
        token = DB.SPGetString("geco.UpdateUserEmail", params, result)

        Select Case result
            Case 0
                Return UpdateUserEmailResult.Success
            Case 1
                Return UpdateUserEmailResult.InvalidEmail
            Case 2
                Return UpdateUserEmailResult.NewEmailExists
        End Select

        Return UpdateUserEmailResult.DbError
    End Function

    '  0 - Successfully added new email
    '  1 - Account does not exist
    '  2 - New email address already registered
    ' -1 - Error
    Public Enum UpdateUserEmailResult
        Success
        InvalidEmail
        NewEmailExists
        DbError
    End Enum

    Public Function GetSavedUserSession(ByRef userSession As UserSession, ByRef user As GecoUser) As Boolean
        NotNull(userSession, NameOf(userSession))

        Dim params As SqlParameter() = {
            New SqlParameter("@Series", userSession.Series),
            New SqlParameter("@Token", userSession.Token)
        }

        Dim result As Integer
        Dim ds As DataSet = DB.SPGetDataSet("geco.LogInViaSession", params, result)

        ' Future enhancement: If result = 2, token theft assumed. Inform user?
        If result = 0 AndAlso ds IsNot Nothing AndAlso ds.Tables.Count = 3 Then
            user = ParseUserFromDataRow(ds.Tables(0).Rows(0))

            ds.Tables(1).PrimaryKey = {ds.Tables(1).Columns(0)}
            user.FacilityAccessTable = ds.Tables(1)

            Dim dr As DataRow = ds.Tables(2).Rows(0)
            userSession = New UserSession(dr(0).ToString(), dr(1).ToString())

            Return True
        End If

        Return False
    End Function

    Public Function UpdateUserProfile(user As GecoUser) As DbResult
        NotNull(user, NameOf(user))

        Dim params As SqlParameter() = {
            New SqlParameter("@GecoUserID", user.UserId),
            New SqlParameter("@FName", user.FirstName),
            New SqlParameter("@LName", user.LastName),
            New SqlParameter("@Title", user.Title),
            New SqlParameter("@CoName", user.Company),
            New SqlParameter("@Street", user.Address.Street),
            New SqlParameter("@City", user.Address.City),
            New SqlParameter("@State", user.Address.State),
            New SqlParameter("@Zip", user.Address.PostalCode),
            New SqlParameter("@Phone", user.PhoneNumber),
            New SqlParameter("@UserType", user.GecoUserType),
            New SqlParameter("@UserID", user.UserId)
        }

        Select Case DB.SPReturnValue("geco.UpdateProfile", params)
            Case 0
                Return DbResult.Success
            Case 1
                Return DbResult.Failure
            Case Else
                Return DbResult.DbError
        End Select
    End Function

End Module
