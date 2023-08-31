Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module UserAccess
    Public Function GetUserAccess(airs As ApbFacilityId) As DataTable
        NotNull(airs, NameOf(airs))

        Const query As String =
                  "select a.NUMUSERID,
               p.STRFIRSTNAME as [FirstName],
               p.STRLASTNAME as [LastName],
               l.STRUSEREMAIL as [Email],
               a.INTADMINACCESS,
               a.INTFEEACCESS,
               a.INTEIACCESS
        from OLAPUSERACCESS a
            inner join OLAPUSERLOGIN l
            on a.NUMUSERID = l.NUMUSERID
            inner join OLAPUSERPROFILE p
            on a.NUMUSERID = p.NUMUSERID
        where a.STRAIRSNUMBER = @airs"

        Dim param As New SqlParameter("@airs", airs.DbFormattedString)

        Return DB.GetDataTable(query, param)
    End Function

    Public Sub UpdateUserAccess(adminAccess As Boolean, feeAccess As Boolean, eiAccess As Boolean,
                                userID As Integer, airs As ApbFacilityId)
        NotNull(airs, NameOf(airs))

        Const query As String =
                  "UPDATE OlapUserAccess SET 
                   INTADMINACCESS = @admin, 
                   intFeeAccess = @fee, 
                   intEIAccess = @ei 
                   WHERE numUserID = @userID 
                   and strAirsNumber = @airs "

        Dim params As SqlParameter() = { _
                                           New SqlParameter("@admin", adminAccess),
                                           New SqlParameter("@fee", feeAccess),
                                           New SqlParameter("@ei", eiAccess),
                                           New SqlParameter("@userID", userID),
                                           New SqlParameter("@airs", airs.DbFormattedString)
                                       }

        DB.RunCommand(query, params)
    End Sub

    Public Function DeleteUserAccess(userId As Integer, airs As ApbFacilityId) As Boolean
        NotNull(airs, NameOf(airs))

        Const query As String =
                  "DELETE OlapUserAccess 
                   WHERE numUserID = @numUserID 
                   and strAirsNumber = @strAirsNumber "

        Dim params As SqlParameter() = { _
                                           New SqlParameter("@numUserID", userId),
                                           New SqlParameter("@strAirsNumber", airs.DbFormattedString)
                                       }

        Return DB.RunCommand(query, params)
    End Function

    Public Function InsertUserAccess(email As String, airs As ApbFacilityId) As Integer
        NotNull(airs, NameOf(airs))

        If Not GecoUserExists(email) Then
            'email address not registered
            Return -1
        End If

        If GecoUserAccessExists(email, airs) Then
            'user access already exists
            Return -2
        End If

        Const query As String =
                  "INSERT INTO OLAPUSERACCESS 
                 (NUMUSERID, STRAIRSNUMBER) 
                     SELECT 
                         NUMUSERID, 
                         @airs 
                     FROM OLAPUSERLOGIN 
                     WHERE STRUSEREMAIL = @userEmail "

        Dim param As SqlParameter() = { _
                                          New SqlParameter("@userEmail", email),
                                          New SqlParameter("@airs", airs.DbFormattedString)
                                      }

        If DB.RunCommand(query, param) Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private Function GecoUserAccessExists(email As String, airs As ApbFacilityId) As Boolean
        NotNull(airs, NameOf(airs))

        Const query As String =
                  " SELECT convert(BIT, count(*)) 
                 FROM OLAPUSERACCESS a 
                     inner join OLAPUSERLOGIN l 
                         on a.NUMUSERID = l.NUMUSERID 
                 WHERE STRUSEREMAIL = @email 
                       and STRAIRSNUMBER = @airs "

        Dim params As SqlParameter() = { _
                                           New SqlParameter("@email", email),
                                           New SqlParameter("@airs", airs.DbFormattedString)
                                       }

        Return DB.GetBoolean(query, params)
    End Function

    Public Function UserAccessReviewRequested(airs As ApbFacilityId) As Boolean
        NotNull(airs, NameOf(airs))

        Const query As String =
                  "select UserAccessLastReviewed
            from dbo.Geco_FacilityInformation
            where FacilityId = @facilityId"

        Dim param As New SqlParameter("@facilityId", airs.ShortString)

        Dim dateLastReviewed As DateTimeOffset? = DB.GetSingleValue(Of DateTimeOffset)(query, param)

        If Not dateLastReviewed.HasValue Then
            Return True
        End If

        Return (DateTimeOffset.Now.Date - dateLastReviewed.Value.Date).TotalDays > 365
    End Function

    Public Sub UpdateUserAccessAsReviewed(airs As ApbFacilityId, userId As Integer)
        NotNull(airs, NameOf(airs))

        Dim params As SqlParameter() = { _
                                           New SqlParameter("@facilityId", airs.ShortString),
                                           New SqlParameter("@userId", userId)
                                       }

        DB.SPRunCommand("geco.FacilityUserAccessReviewed", params)
    End Sub
End Module
