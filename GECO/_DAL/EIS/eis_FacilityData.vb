Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module eis_FacilityData

    Private Function EisFacSiteAffExists(FacilitySiteID As String) As Boolean
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
         FacilitySiteID As String)

        NotNull(ContactPrefix, NameOf(ContactPrefix))
        NotNull(ContactFirstName, NameOf(ContactFirstName))
        NotNull(ContactLastName, NameOf(ContactLastName))
        NotNull(ContactTitle, NameOf(ContactTitle))
        NotNull(ContactEmail, NameOf(ContactEmail))
        NotNull(ContactAddress1, NameOf(ContactAddress1))
        NotNull(ContactAddress2, NameOf(ContactAddress2))
        NotNull(ContactCity, NameOf(ContactCity))
        NotNull(ContactState, NameOf(ContactState))
        NotNull(ContactZipCode, NameOf(ContactZipCode))
        NotNull(ContactComment, NameOf(ContactComment))
        NotNull(UpdateUser, NameOf(UpdateUser))

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
            New SqlParameter("@ContactPrefix", ContactPrefix.Left(15)),
            New SqlParameter("@ContactFirstName", ContactFirstName.Left(35)),
            New SqlParameter("@ContactLastName", ContactLastName.Left(35)),
            New SqlParameter("@ContactTitle", ContactTitle.Left(100)),
            New SqlParameter("@ContactEmail", ContactEmail.Left(50)),
            New SqlParameter("@ContactAddress1", ContactAddress1.Left(100)),
            New SqlParameter("@ContactAddress2", ContactAddress2.Left(50)),
            New SqlParameter("@ContactCity", ContactCity.Left(60)),
            New SqlParameter("@ContactState", ContactState.Left(2)),
            New SqlParameter("@ContactZipCode", ContactZipCode.Left(10)),
            New SqlParameter("@contactComment", ContactComment.Left(400)),
            New SqlParameter("@UpdateUser", UpdateUser.Left(250)),
            New SqlParameter("@FacilitySiteID", FacilitySiteID)
        }

        DB.RunCommand(query, params)
    End Sub

    Public Function IsFacilityLatLonLocked(facilityId As ApbFacilityId) As Boolean
        NotNull(facilityId, NameOf(facilityId))

        Dim query = "select convert(bit, iif(CoordinatesProtected = 'Yes', 1, 0))
            from EIS_EpaFacilityGeoCoord
            where FACILITYSITEID = @FacilitySiteID "

        Dim param As New SqlParameter("@FacilitySiteID", facilityId.ShortString)

        Return DB.GetBoolean(query, param)
    End Function

    Public Enum FacilitySiteStatusCode
        OP 'Operating
        PS 'Permanently Shutdown
        TS 'Temporarily Shutdown
        UNK 'Unknown
    End Enum

    Public Sub SaveEisFacilityStatus(fsid As ApbFacilityId, facstatus As FacilitySiteStatusCode, UpdUser As String, eiyr As Integer)
        NotNull(fsid, NameOf(fsid))

        Dim query = "update eis_FacilitySite " &
            " set strFacilitySiteStatusCode = @facstatus, " &
            " intFacilitySiteStatusCodeYear = @eiyr, " &
            " UpdateUser = @UpdUser, " &
            " UpdateDateTime = getdate() " &
            " where " &
            " FacilitySiteID = @fsid "

        Dim params = {
            New SqlParameter("@facstatus", facstatus.ToString),
            New SqlParameter("@eiyr", eiyr),
            New SqlParameter("@UpdUser", UpdUser),
            New SqlParameter("@fsid", fsid.ShortString)
        }

        DB.RunCommand(query, params)
    End Sub


End Module
