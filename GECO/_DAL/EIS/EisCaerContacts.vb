Imports System.Data.SqlClient
Imports GaEpd.DBUtilities
Imports GECO.GecoModels
Imports GECO.GecoModels.EIS

Namespace DAL.EIS
    Module EisCaerContacts

        Public Function SaveCaerContact(caerContact As CaerContact) As DbResult
            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteId", caerContact.FacilitySiteId.ShortString),
                New SqlParameter("@CaerRole", caerContact.CaerRole.GetDescription),
                New SqlParameter("@Honorific", caerContact.Contact.Honorific),
                New SqlParameter("@FirstName", caerContact.Contact.FirstName),
                New SqlParameter("@LastName", caerContact.Contact.LastName),
                New SqlParameter("@Title", caerContact.Contact.Title),
                New SqlParameter("@Company", caerContact.Contact.Company),
                New SqlParameter("@Email", caerContact.Contact.Email),
                New SqlParameter("@PhoneNumber", caerContact.Contact.PhoneNumber),
                New SqlParameter("@Street", caerContact.Contact.Address.Street),
                New SqlParameter("@Street2", caerContact.Contact.Address.Street2),
                New SqlParameter("@City", caerContact.Contact.Address.City),
                New SqlParameter("@State", caerContact.Contact.Address.State),
                New SqlParameter("@PostalCode", caerContact.Contact.Address.PostalCode),
                New SqlParameter("@GecoUser", GetCurrentUser.UserId)
            }

            Dim result As Integer = DB.SPReturnValue("geco.Caer_CreateContact", params)

            Select Case result
                Case 0
                    Return DbResult.Success
                Case Else
                    Return DbResult.DbError
            End Select
        End Function

        Public Function UpdateCaerContact(caerContact As CaerContact, id As Guid) As DbResult
            Dim params As SqlParameter() = {
                New SqlParameter("@Id", id),
                New SqlParameter("@CaerRole", caerContact.CaerRole.ToString()),
                New SqlParameter("@Honorific", caerContact.Contact.Honorific),
                New SqlParameter("@FirstName", caerContact.Contact.FirstName),
                New SqlParameter("@LastName", caerContact.Contact.LastName),
                New SqlParameter("@Title", caerContact.Contact.Title),
                New SqlParameter("@Company", caerContact.Contact.Company),
                New SqlParameter("@Email", caerContact.Contact.Email),
                New SqlParameter("@PhoneNumber", caerContact.Contact.PhoneNumber),
                New SqlParameter("@Street", caerContact.Contact.Address.Street),
                New SqlParameter("@Street2", caerContact.Contact.Address.Street2),
                New SqlParameter("@City", caerContact.Contact.Address.City),
                New SqlParameter("@State", caerContact.Contact.Address.State),
                New SqlParameter("@PostalCode", caerContact.Contact.Address.PostalCode),
                New SqlParameter("@GecoUser", GetCurrentUser.UserId)
            }

            Dim result As Integer = DB.SPReturnValue("geco.Caer_UpdateContact", params, result)

            Select Case result
                Case 0
                    Return DbResult.Success
                Case Else
                    Return DbResult.DbError
            End Select
        End Function

        Public Function CaerPreparerExists(email As String, facilityId As ApbFacilityId, Optional ignoreId As Guid = Nothing) As Boolean
            Dim params As SqlParameter() = {
                New SqlParameter("@Email", email),
                New SqlParameter("@FacilityId", facilityId.ShortString),
                New SqlParameter("@IgnoreId", ignoreId)
            }

            Return DB.SPGetBoolean("geco.Caer_PreparerExists", params)
        End Function

        Public Function DeleteCaerContact(id As Guid) As Boolean
            Dim query As String = "update EIS_CaerContacts
                set Active         = 0,
                    UpdateGecoUser = @GecoUser,
                    UpdateDateTime = sysdatetimeoffset()
                where Id = @Id"

            Dim param As SqlParameter() = {
                New SqlParameter("@Id", id),
                New SqlParameter("@GecoUser", GetCurrentUser.UserId)
            }

            Return DB.RunCommand(query, param)
        End Function

        Public Function GetFacilityCaerContacts(facilityId As ApbFacilityId) As DataTable
            Return DB.SPGetDataTable("geco.Caer_GetFacilityContacts", New SqlParameter("@FacilitySiteId", facilityId.ShortString))
        End Function

        Public Function GetCaerContact(id As Guid) As CaerContact
            Dim dr As DataRow = DB.SPGetDataRow("geco.Caer_GetContact", New SqlParameter("@Id", id))

            If dr Is Nothing Then
                Return Nothing
            End If

            Dim address As New Address With {
                .Street = GetNullableString(dr.Item("Street")),
                .Street2 = GetNullableString(dr.Item("Street2")),
                .City = GetNullableString(dr.Item("City")),
                .State = GetNullableString(dr.Item("State")),
                .PostalCode = GetNullableString(dr.Item("PostalCode"))
            }

            Dim contact As New Person With {
                .Address = address,
                .Company = GetNullableString(dr.Item("Company")),
                .Email = GetNullableString(dr.Item("Email")),
                .Honorific = GetNullableString(dr.Item("Honorific")),
                .FirstName = GetNullableString(dr.Item("FirstName")),
                .LastName = GetNullableString(dr.Item("LastName")),
                .PhoneNumber = GetNullableString(dr.Item("PhoneNumber")),
                .Title = GetNullableString(dr.Item("Title"))
            }

            Dim role As CaerRole = CType([Enum].Parse(GetType(CaerRole), dr.Item("CaerRole").ToString), CaerRole)

            Dim caerContact As New CaerContact With {
                .Active = CBool(dr.Item("Active")),
                .CaerRole = role,
                .Contact = contact,
                .FacilityName = GetNullableString(dr.Item("FacilityName")),
                .FacilitySiteId = New ApbFacilityId(dr.Item("FacilitySiteId").ToString),
                .Id = id
            }

            Return caerContact
        End Function

    End Module
End Namespace
