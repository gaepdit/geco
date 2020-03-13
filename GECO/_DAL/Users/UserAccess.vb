Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module UserAccess

    Public Function GetUserAccess() As DataTable
        Dim query = "select " &
        "    a.NUMUSERID, " &
        "    STRUSEREMAIL, " &
        "    STRAIRSNUMBER, " &
        "    INTADMINACCESS, " &
        "    INTFEEACCESS, " &
        "    INTEIACCESS, " &
        "    INTESACCESS " &
        " FROM OLAPUSERACCESS a " &
        "    inner join OLAPUSERLOGIN l " &
        "        on a.NUMUSERID = l.NUMUSERID " &
        " where a.STRAIRSNUMBER = @airs "

        Dim param As New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber))

        Return DB.GetDataTable(query, param)
    End Function

    Public Function UpdateUserAccess(adminAccess As Boolean, feeAccess As Boolean, eiAccess As Boolean, esAccess As Boolean, userID As Decimal, airs As String) As Boolean
        Dim query As String = "UPDATE OlapUserAccess SET " &
        " INTADMINACCESS = @admin, " &
        " intFeeAccess = @fee, " &
        " intEIAccess = @ei, " &
        " intESAccess = @es " &
        " WHERE numUserID = @userID " &
        " and strAirsNumber = @airs "

        Dim params As SqlParameter() = {
            New SqlParameter("@admin", adminAccess),
            New SqlParameter("@fee", feeAccess),
            New SqlParameter("@ei", eiAccess),
            New SqlParameter("@es", esAccess),
            New SqlParameter("@userID", userID),
            New SqlParameter("@airs", airs)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteUserAccess(numUserID As Decimal, strAirsNumber As String) As Boolean
        Dim query As String = "DELETE OlapUserAccess " &
            " WHERE numUserID = @numUserID " &
            " and strAirsNumber = @strAirsNumber "

        Dim params As SqlParameter() = {
            New SqlParameter("@numUserID", numUserID),
            New SqlParameter("@strAirsNumber", strAirsNumber)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function InsertUserAccess(userEmail As String, airs As ApbFacilityId) As Integer
        NotNull(airs, NameOf(airs))

        If Not GecoUserExists(userEmail) Then
            'email address not registered
            Return -1
        End If

        If GecoUserAccessExists(userEmail, airs) Then
            'user access already exists
            Return -2
        End If

        Dim query As String = "INSERT INTO OLAPUSERACCESS " &
        " (NUMUSERID, STRAIRSNUMBER) " &
        "     SELECT " &
        "         NUMUSERID, " &
        "         @airs " &
        "     FROM OLAPUSERLOGIN " &
        "     WHERE STRUSEREMAIL = @userEmail "

        Dim param As SqlParameter() = {
            New SqlParameter("@userEmail", userEmail),
            New SqlParameter("@airs", airs.DbFormattedString)
        }

        If DB.RunCommand(query, param) Then
            Return 1
        Else
            Return 0
        End If
    End Function

    Private Function GecoUserAccessExists(email As String, airs As ApbFacilityId) As Boolean
        Dim query As String = " SELECT convert(BIT, count(*)) " &
        " FROM OLAPUSERACCESS a " &
        "     inner join OLAPUSERLOGIN l " &
        "         on a.NUMUSERID = l.NUMUSERID " &
        " WHERE STRUSEREMAIL = @email " &
        "       and STRAIRSNUMBER = @airs "

        Dim params As SqlParameter() = {
            New SqlParameter("@email", email),
            New SqlParameter("@airs", airs.DbFormattedString)
        }

        Return DB.GetBoolean(query, params)
    End Function

End Module
