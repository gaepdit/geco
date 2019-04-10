﻿Imports System.Data.SqlClient

Public Module eis_FacilityData

    Public Function EisFacSiteAffExists(FacilitySiteID As String) As Boolean
        Dim query = "select convert(bit, count(*)) " &
        " from EIS_FACILITYSITEAFFINDIV " &
        " where FACILITYSITEID = @FacilitySiteID "

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

        Return DB.GetBoolean(query, param)
    End Function

    'Saves Contact information to the EIS tables
    Public Sub SaveFacilityContact(
         ContactPrefix As String,
         ContactFirstName As String,
         ContactLastName As String,
         ContactTitle As String,
         ContactEmail As String,
         ContactAddress1 As String,
         ContactAddress2 As String,
         ContactCity As String,
         ContactState As String,
         ContactZipCode As String,
         ContactComment As String,
         UpdateUser As String,
         FacilitySiteID As String
        )
        Dim query As String

        If EisFacSiteAffExists(FacilitySiteID) Then
            query = "Update EIS_FACILITYSITEAFFINDIV " &
            " Set STRNAMEPREFIXTEXT = @ContactPrefix, " &
            " STRFIRSTNAME = @ContactFirstName, " &
            " STRLASTNAME = @ContactLastName, " &
            " STRINDIVIDUALTITLETEXT = @ContactTitle, " &
            " STRELECTRONICADDRESSTEXT = @ContactEmail, " &
            " STRMAILINGADDRESSTEXT = @ContactAddress1, " &
            " STRSUPPLEMENTALADDRESSTEXT = @ContactAddress2, " &
            " STRMAILINGADDRESSCITYNAME = @ContactCity, " &
            " STRMAILINGADDRESSSTATECODE = @ContactState, " &
            " STRMAILINGADDRESSPOSTALCODE = @ContactZipCode, " &
            " STRADDRESSCOMMENT = @contactComment, " &
            " UpdateUser = @UpdateUser, " &
            " UpdateDateTime = getdate() " &
            " where FACILITYSITEID = @FacilitySiteID "
        Else
            query = " INSERT INTO dbo.EIS_FACILITYSITEAFFINDIV ( " &
            "     FACILITYSITEID, " &
            "     STRINDIVIDUALTITLETEXT, " &
            "     STRNAMEPREFIXTEXT, " &
            "     STRFIRSTNAME, " &
            "     STRLASTNAME, " &
            "     STRELECTRONICADDRESSTEXT, " &
            "     STRMAILINGADDRESSTEXT, " &
            "     STRSUPPLEMENTALADDRESSTEXT, " &
            "     STRMAILINGADDRESSCITYNAME, " &
            "     STRMAILINGADDRESSSTATECODE, " &
            "     STRMAILINGADDRESSPOSTALCODE, " &
            "     STRADDRESSCOMMENT, " &
            "     ACTIVE, " &
            "     UPDATEUSER, " &
            "     UPDATEDATETIME, " &
            "     CREATEDATETIME, " &
            "     LASTEISSUBMITDATE " &
            " ) values ( " &
            "     @FacilitySiteID, " &
            "     @ContactTitle, " &
            "     @ContactPrefix, " &
            "     @ContactFirstName, " &
            "     @ContactLastName, " &
            "     @ContactEmail, " &
            "     @ContactAddress1, " &
            "     @ContactAddress2, " &
            "     @ContactCity, " &
            "     @ContactState, " &
            "     @ContactZipCode, " &
            "     @contactComment, " &
            "     1, " &
            "     @UpdateUser, " &
            "     getdate(), " &
            "     getdate(), " &
            "     null " &
            " ) "
        End If

        Dim params As SqlParameter() = {
            New SqlParameter("@ContactPrefix", Left(ContactPrefix, 15)),
            New SqlParameter("@ContactFirstName", Left(ContactFirstName, 35)),
            New SqlParameter("@ContactLastName", Left(ContactLastName, 35)),
            New SqlParameter("@ContactTitle", Left(ContactTitle, 100)),
            New SqlParameter("@ContactEmail", Left(ContactEmail, 50)),
            New SqlParameter("@ContactAddress1", Left(ContactAddress1, 100)),
            New SqlParameter("@ContactAddress2", Left(ContactAddress2, 50)),
            New SqlParameter("@ContactCity", Left(ContactCity, 60)),
            New SqlParameter("@ContactState", Left(ContactState, 2)),
            New SqlParameter("@ContactZipCode", Left(ContactZipCode, 10)),
            New SqlParameter("@contactComment", Left(ContactComment, 400)),
            New SqlParameter("@UpdateUser", Left(UpdateUser, 250)),
            New SqlParameter("@FacilitySiteID", FacilitySiteID)
        }

        DB.RunCommand(query, params)
    End Sub


End Module