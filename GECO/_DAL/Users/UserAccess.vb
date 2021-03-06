﻿Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module UserAccess

    Public Function GetUserAccess(airs As ApbFacilityId) As DataTable
        NotNull(airs, NameOf(airs))

        Dim query = "select a.NUMUSERID,
               p.STRFIRSTNAME as [FirstName],
               p.STRLASTNAME as [LastName],
               l.STRUSEREMAIL as [Email],
               a.INTADMINACCESS,
               a.INTFEEACCESS,
               a.INTEIACCESS,
               a.INTESACCESS
        from OLAPUSERACCESS a
            inner join OLAPUSERLOGIN l
            on a.NUMUSERID = l.NUMUSERID
            inner join OLAPUSERPROFILE p
            on a.NUMUSERID = p.NUMUSERID
        where a.STRAIRSNUMBER = @airs"

        Dim param As New SqlParameter("@airs", airs.DbFormattedString)

        Return DB.GetDataTable(query, param)
    End Function

    Public Function UpdateUserAccess(adminAccess As Boolean, feeAccess As Boolean,
                                     eiAccess As Boolean, esAccess As Boolean,
                                     userID As Integer, airs As ApbFacilityId) As Boolean
        NotNull(airs, NameOf(airs))

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
            New SqlParameter("@airs", airs.DbFormattedString)
        }

        Return DB.RunCommand(query, params)
    End Function

    Public Function DeleteUserAccess(userId As Integer, airs As ApbFacilityId) As Boolean
        NotNull(airs, NameOf(airs))

        Dim query As String = "DELETE OlapUserAccess " &
            " WHERE numUserID = @numUserID " &
            " and strAirsNumber = @strAirsNumber "

        Dim params As SqlParameter() = {
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

        Dim query As String = "INSERT INTO OLAPUSERACCESS " &
        " (NUMUSERID, STRAIRSNUMBER) " &
        "     SELECT " &
        "         NUMUSERID, " &
        "         @airs " &
        "     FROM OLAPUSERLOGIN " &
        "     WHERE STRUSEREMAIL = @userEmail "

        Dim param As SqlParameter() = {
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
