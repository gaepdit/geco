Imports System.Data.SqlClient
Imports GECO.GecoModels


Namespace DAL.EIS
    Module Facility

        Public Function GetEisFacilityDetails(airs As ApbFacilityId) As DataRow
            NotNull(airs, NameOf(airs))

            Dim query = "select strFacilitySiteName, strMailingAddressText, strSupplementalAddressText, strMailingAddressCityName,
                   strMailingAddressStateCode, strMailingAddressPostalCode, strLocationAddressText, strSupplementalLocationText,
                   strLocalityName, strLocationAddressPostalCode, strAddressComment, strFacilitySiteStatusDesc,
                   intFacilitySiteStatusCodeYear, strFacilitySiteDescription, strNAICSCode, strFacilitySiteComment,
                   numLatitudeMeasure, numLongitudeMeasure, " &
                   "HorCollMetCode, STRHORCOLLMETDesc, INTHORACCURACYMEASURE, HorRefDatumCode, STRHORREFDATUMDesc,
                   strGeographicComment, strNamePrefixText, strFirstName, strLastName, strIndividualTitleText, strFSAIMAddressText,
                   strFSAISAddressText, strFSAIMAddressCityName, strFSAIMAddressStateCode, strFSAIMAddressPostalCode,
                   StrElectronicAddressText, UpdateUser_mailingAddress, UpdateDateTime_mailingAddress, LastEPASubmitDate_MAddress,
                   UpdateUser_FacilitySite, UpdateDateTime_FacilitySite, LastEPASubmitDate_FacilitySite, UpdateUser_GeoCoord,
                   UpdateDateTime_GeoCoord, LastEPASubmitDate_GeoCoord, UpdateUser_AffIndiv, UpdateDateTime_AffIndiv,
                   LastEPASubmitDate_AffIndiv, strFSAIAddressComment
            FROM VW_EIS_FACILITY
            where FACILITYSITEID = @FacilitySiteID"

            Dim param As New SqlParameter("@FacilitySiteID", airs.ShortString)

            Return DB.GetDataRow(query, param)
        End Function

        Public Function GetEisContactPhoneNumbers(airs As ApbFacilityId) As DataTable
            NotNull(airs, NameOf(airs))

            Dim query = "select TELEPHONENUMBERTYPECODE, STRTELEPHONENUMBERTEXT
            from EIS_TELEPHONECOMM where FACILITYSITEID = @FacilitySiteID and ACTIVE='1'"

            Dim param As New SqlParameter("@FacilitySiteID", airs.ShortString)

            Return DB.GetDataTable(query, param)
        End Function

        Public Function SaveEisFacilityMailingAddress(
                airs As ApbFacilityId,
                address As Address,
                comment As String,
                user As GecoUser) As Boolean

            NotNull(airs, NameOf(airs))
            NotNull(user, NameOf(user))
            NotNull(address, NameOf(address))

            Dim query As String = "Update EIS_FACILITYSITEADDRESS " &
                " Set STRMAILINGADDRESSTEXT = @FacilityMailingAddress, " &
                " STRSUPPLEMENTALADDRESSTEXT = @FacilityMailingAddress2, " &
                " STRMAILINGADDRESSCITYNAME = @FacilityMailingAddressCity, " &
                " STRMAILINGADDRESSSTATECODE = @FacilityMailingAddressState, " &
                " STRMAILINGADDRESSPOSTALCODE = @FacilityMailingAddressZip, " &
                " STRADDRESSCOMMENT = @FacilityMailingAddressComment, " &
                " UPDATEUSER = @UpdateUser, " &
                " UpdateDateTime = getdate() " &
                " where EIS_FACILITYSITEADDRESS.FACILITYSITEID = @FacilitySiteID "

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilityMailingAddress", address.Street),
                New SqlParameter("@FacilityMailingAddress2", address.Street2),
                New SqlParameter("@FacilityMailingAddressCity", address.City),
                New SqlParameter("@FacilityMailingAddressState", address.State),
                New SqlParameter("@FacilityMailingAddressZip", address.PostalCode),
                New SqlParameter("@FacilityMailingAddressComment", comment.Left(400)),
                New SqlParameter("@UpdateUser", user.DbUpdateUser),
                New SqlParameter("@FacilitySiteID", airs.ShortString)
            }

            Return DB.RunCommand(query, params)
        End Function

        Public Function SaveEisFacilitySiteInfo(airs As ApbFacilityId, description As String, naics As String,
                comment As String, user As GecoUser) As Boolean
            NotNull(airs, NameOf(airs))
            NotNull(user, NameOf(user))

            Dim query As String = "Update EIS_FACILITYSITE " &
               "Set STRFACILITYSITEDESCRIPTION = @facilityDescription, " &
               "STRNAICSCODE = @NAICScode, " &
               "STRFACILITYSITECOMMENT = @facilityComment, " &
               "UPDATEUSER = @UpdateUser, " &
               "UpdateDateTime = getdate() " &
               "where FACILITYSITEID = @FacilitySiteID "

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityDescription", description),
                New SqlParameter("@NAICScode", naics),
                New SqlParameter("@facilityComment", comment.Left(400)),
                New SqlParameter("@UpdateUser", user.DbUpdateUser),
                New SqlParameter("@FacilitySiteID", airs.ShortString)
            }

            Return DB.RunCommand(query, params)
        End Function

        Public Function UpdateEisGeographicComment(airs As ApbFacilityId, comment As String, user As GecoUser) As Boolean
            NotNull(airs, NameOf(airs))
            NotNull(user, NameOf(user))

            Dim query = "Update EIS_FACILITYGEOCOORD " &
                    " Set STRGEOGRAPHICCOMMENT = @GeographicComment, " &
                    " UPDATEUSER = @UpdateUser, " &
                    " UpdateDateTime = getdate() " &
                    " where FACILITYSITEID = @FacilitySiteID "

            Dim params = {
                    New SqlParameter("@GeographicComment", comment.Left(200)),
                    New SqlParameter("@UpdateUser", user.DbUpdateUser),
                    New SqlParameter("@FacilitySiteID", airs.ShortString)
                }

            Return DB.RunCommand(query, params)
        End Function

        Public Sub SaveEisContactPhoneNumbers(airs As ApbFacilityId, phone As String, mobile As String, fax As String, user As GecoUser)
            Dim query As String

            Dim facParam As New SqlParameter("@FacilitySiteID", airs.ShortString)
            Dim params As SqlParameter()

            If Not String.IsNullOrEmpty(phone) Then
                query = "select convert(bit, count(*)) " &
                    " FROM EIS_TELEPHONECOMM " &
                    " where EIS_TELEPHONECOMM.FACILITYSITEID = @FacilitySiteID " &
                    " and TelephoneNumberTypeCode = 'W' " &
                    " and active = '1' "

                If DB.GetBoolean(query, facParam) Then
                    query = "Update EIS_TELEPHONECOMM Set " &
                        " STRTELEPHONENUMBERTEXT = @phoneNumber, " &
                        " updateUser = @UpdateUser, " &
                        " UPDATEDATETIME = getdate() " &
                        " where FACILITYSITEID = @FacilitySiteID " &
                        " and TELEPHONENUMBERTYPECODE= 'W' "
                Else
                    query = "Insert Into EIS_TELEPHONECOMM " &
                        " (FACILITYSITEID, " &
                        " TELEPHONENUMBERTYPECODE, " &
                        " STRTELEPHONENUMBERTEXT, " &
                        " ACTIVE, " &
                        " UPDATEUSER, " &
                        " UPDATEDATETIME, " &
                        " CREATEDATETIME) " &
                        " Values " &
                        " (@FACILITYSITEID, " &
                        " 'W', " &
                        " @phoneNumber, " &
                        " '1', " &
                        " @UpdateUser, " &
                        " getdate(), " &
                        " getdate()) "
                End If

                params = {
                    facParam,
                    New SqlParameter("@phoneNumber", phone),
                    New SqlParameter("@UpdateUser", user.DbUpdateUser)
                }

                DB.RunCommand(query, params)
            End If

            If Not String.IsNullOrEmpty(mobile) Then
                query = "select convert(bit, count(*)) " &
                        " FROM EIS_TELEPHONECOMM " &
                        " where EIS_TELEPHONECOMM.FACILITYSITEID = @FacilitySiteID " &
                        " and TelephoneNumberTypeCode = 'M' " &
                        " and active = '1' "

                If DB.GetBoolean(query, facParam) Then
                    query = "Update EIS_TELEPHONECOMM Set " &
                            " STRTELEPHONENUMBERTEXT = @mobilePhone, " &
                            " UPDATEUSER = @UpdateUser, " &
                            " UPDATEDATETIME = getdate() " &
                            " where FACILITYSITEID = @FacilitySiteID " &
                            " and TELEPHONENUMBERTYPECODE= 'M' "
                Else
                    query = "Insert Into EIS_TELEPHONECOMM " &
                            " (FACILITYSITEID, " &
                            " TELEPHONENUMBERTYPECODE, " &
                            " STRTELEPHONENUMBERTEXT, " &
                            " ACTIVE, " &
                            " UPDATEUSER, " &
                            " UPDATEDATETIME, " &
                            " CREATEDATETIME) " &
                            " Values " &
                            " (@FACILITYSITEID, " &
                            " 'M', " &
                            " @mobilePhone, " &
                            " '1', " &
                            " @UpdateUser, " &
                            " getdate(), " &
                            " getdate()) "
                End If

                params = {
                    facParam,
                    New SqlParameter("@mobilePhone", mobile),
                    New SqlParameter("@UpdateUser", user.DbUpdateUser)
                }

                DB.RunCommand(query, params)
            End If

            If Not String.IsNullOrEmpty(fax) Then
                query = "select convert(bit, count(*)) " &
                        " FROM EIS_TELEPHONECOMM " &
                        " where EIS_TELEPHONECOMM.FACILITYSITEID = @FacilitySiteID " &
                        " and TelephoneNumberTypeCode = 'F' " &
                        " and active = '1' "

                If DB.GetBoolean(query, facParam) Then
                    query = "Update EIS_TELEPHONECOMM Set " &
                            " STRTELEPHONENUMBERTEXT = @FaxNumber, " &
                            " UPDATEUSER = @UpdateUser, " &
                            " UPDATEDATETIME = getdate() " &
                            " where FACILITYSITEID = @FacilitySiteID " &
                            " and TELEPHONENUMBERTYPECODE= 'F' "
                Else
                    query = "Insert Into EIS_TELEPHONECOMM " &
                            " (FACILITYSITEID, " &
                            " TELEPHONENUMBERTYPECODE, " &
                            " STRTELEPHONENUMBERTEXT, " &
                            " ACTIVE, " &
                            " UPDATEUSER, " &
                            " UPDATEDATETIME, " &
                            " CREATEDATETIME) " &
                            " Values " &
                            " (@FACILITYSITEID, " &
                            " 'F', " &
                            " @FaxNumber, " &
                            " '1', " &
                            " @UpdateUser, " &
                            " getdate(), " &
                            " getdate()) "
                End If

                params = {
                    facParam,
                    New SqlParameter("@FaxNumber", fax),
                    New SqlParameter("@UpdateUser", user.DbUpdateUser)
                }

                DB.RunCommand(query, params)
            End If
        End Sub

    End Module
End Namespace
