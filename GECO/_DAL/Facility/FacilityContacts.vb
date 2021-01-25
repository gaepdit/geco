Imports System.Data.SqlClient
Imports GECO.GecoModels

Namespace DAL.Facility
    Module FacilityContacts

        Public Function SaveApbContactInformation(airs As ApbFacilityId, key As String, prefix As String,
                firstName As String, lastName As String, title As String, email As String, address1 As String,
                address2 As String, city As String, state As String, zipCode As String, phoneNo As String,
                mobileNo As String, fax As String, comment As String,
                                                  companyName As String, updateUserId As String
                                                  ) As Boolean

            Dim query As String = "select convert(bit, count(*)) " &
                " FROM APBCONTACTINFORMATION " &
                " where STRAIRSNUMBER = @FacilitySiteID " &
                " and STRKEY = convert(varchar(2), @Key)"

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteID", airs.DbFormattedString),
                New SqlParameter("@Key", key)
            }

            If DB.GetBoolean(query, params) Then
                query = "Update APBCONTACTINFORMATION Set " &
                        " STRCONTACTPREFIX = @ContactPrefix, " &
                        " STRCONTACTFIRSTNAME = @ContactFirstName, " &
                        " STRCONTACTLASTNAME = @ContactLastName, " &
                        " STRCONTACTTITLE = @ContactTitle, " &
                        " STRCONTACTEMAIL = @ContactEmail, " &
                        " STRCONTACTADDRESS1 = @ContactAddress1, " &
                        " STRCONTACTADDRESS2 = @ContactAddress2, " &
                        " STRCONTACTCITY = @ContactCity, " &
                        " STRCONTACTSTATE = @ContactState, " &
                        " STRCONTACTZIPCODE = @ContactZipCode, " &
                        " STRCONTACTPHONENUMBER1 = @contactphoneNo, " &
                        " STRCONTACTPHONENUMBER2 = @contactMobileNo, " &
                        " STRCONTACTFAXNUMBER = @contactFax, " &
                        " strcontactcompanyname = @CompanyName, " &
                        " STRCONTACTDESCRIPTION = @contactComment, " &
                        " STRMODIFINGPERSON = @IAIPUserID," &
                        " DATMODIFINGDATE = getdate() " &
                        " where strAIRSNumber = @FacilitySiteID " &
                        " and strkey = @Key "
            Else
                query = " Insert into APBCONTACTINFORMATION ( " &
                "     STRCONTACTKEY, " &
                "     STRAIRSNUMBER, " &
                "     STRKEY, " &
                "     STRCONTACTFIRSTNAME, " &
                "     STRCONTACTLASTNAME, " &
                "     STRCONTACTPREFIX, " &
                "     STRCONTACTTITLE, " &
                "     STRCONTACTCOMPANYNAME, " &
                "     STRCONTACTPHONENUMBER1, " &
                "     STRCONTACTPHONENUMBER2, " &
                "     STRCONTACTFAXNUMBER, " &
                "     STRCONTACTEMAIL, " &
                "     STRCONTACTADDRESS1, " &
                "     STRCONTACTADDRESS2, " &
                "     STRCONTACTCITY, " &
                "     STRCONTACTSTATE, " &
                "     STRCONTACTZIPCODE, " &
                "     STRMODIFINGPERSON, " &
                "     DATMODIFINGDATE, " &
                "     STRCONTACTDESCRIPTION " &
                " ) Values ( " &
                "     @ContactKey, " &
                "     @FacilitySiteID, " &
                "     @Key, " &
                "     @ContactFirstName, " &
                "     @ContactLastName, " &
                "     @ContactPrefix, " &
                "     @ContactTitle, " &
                "     @CompanyName, " &
                "     @contactphoneNo, " &
                "     @contactMobileNo, " &
                "     @contactFax, " &
                "     @ContactEmail, " &
                "     @ContactAddress1, " &
                "     @ContactAddress2, " &
                "     @ContactCity, " &
                "     @ContactState, " &
                "     @ContactZipCode, " &
                "     @IAIPUserID, " &
                "     getdate(), " &
                "     @contactComment " &
                " ) "
            End If

            params = {
                New SqlParameter("@ContactKey", airs.DbFormattedString & key),
                New SqlParameter("@FacilitySiteID", airs.DbFormattedString),
                New SqlParameter("@Key", key),
                New SqlParameter("@ContactFirstName", firstName),
                New SqlParameter("@ContactLastName", lastName),
                New SqlParameter("@ContactPrefix", prefix),
                New SqlParameter("@ContactTitle", title),
                New SqlParameter("@contactphoneNo", phoneNo),
                New SqlParameter("@contactMobileNo", mobileNo),
                New SqlParameter("@contactFax", fax),
                New SqlParameter("@ContactEmail", email),
                New SqlParameter("@ContactAddress1", address1),
                New SqlParameter("@ContactAddress2", address2),
                New SqlParameter("@ContactCity", city),
                New SqlParameter("@ContactState", state),
                New SqlParameter("@ContactZipCode", Replace(zipCode, "-", "")),
                New SqlParameter("@CompanyName", companyName),
                New SqlParameter("@IAIPUserID", updateUserId),
                New SqlParameter("@contactComment", Left(comment, 400))
            }

            Return DB.RunCommand(query, params)
        End Function

    End Module
End Namespace
