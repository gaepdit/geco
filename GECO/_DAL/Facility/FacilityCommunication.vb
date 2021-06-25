Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Namespace DAL.Facility
    Module FacilityCommunication

        Public Function GetFacilityCommunicationInfo(facilityId As ApbFacilityId
                                                     ) As Dictionary(Of CommunicationCategory, FacilityCommunicationInfo)

            Dim info As New Dictionary(Of CommunicationCategory, FacilityCommunicationInfo)

            For Each category In CommunicationCategory.AllCategories
                info.Add(category, GetFacilityCommunicationInfo(facilityId, category))
            Next

            Return info
        End Function

        Public Function GetFacilityCommunicationInfo(facilityId As ApbFacilityId,
                                                     category As CommunicationCategory
                                                     ) As FacilityCommunicationInfo

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name)
            }

            Dim ds As DataSet = DB.SPGetDataSet("geco.GetFacilityCommunicationInfo", params)

            If ds Is Nothing Then Return Nothing

            Dim info As New FacilityCommunicationInfo() With {
                .Preference = New FacilityCommunicationPreference(),
                .Emails = New List(Of EmailContact)
            }

            If ds.Tables(0).Rows.Count = 1 Then
                Dim dr1 As DataRow = ds.Tables(0).Rows(0)

                With info.Preference
                    .Id = CType(dr1("Id"), Guid)
                    .CommunicationPreference = CommunicationPreference.FromName(CStr(dr1("CommunicationPreference")))
                    .InitialConfirmationDate = GetNullable(Of DateTimeOffset)(dr1("InitialConfirmationDate"))
                    .LatestConfirmationDate = GetNullable(Of DateTimeOffset)(dr1("LatestConfirmationDate"))
                End With
            End If

            If ds.Tables(1).Rows.Count = 1 Then
                Dim dr2 As DataRow = ds.Tables(1).Rows(0)
                info.Mail = New MailContact() With {
                    .Id = CType(dr2("Id"), Guid),
                    .Address1 = CStr(dr2("Address1")),
                    .Address2 = GetNullableString(dr2("Address2")),
                    .City = CStr(dr2("City")),
                    .Name = CStr(dr2("Name")),
                    .Organization = GetNullableString(dr2("Organization")),
                    .PostalCode = CStr(dr2("PostalCode")),
                    .State = CStr(dr2("State")),
                    .Telephone = GetNullableString(dr2("Telephone")),
                    .Title = GetNullableString(dr2("Title"))
                }
            End If

            For Each row As DataRow In ds.Tables(2).Rows
                Dim email As New EmailContact() With {
                    .Id = CType(row("Id"), Guid),
                    .Verified = CBool(row("Verified")),
                    .Email = CStr(row("Email"))
                }

                info.Emails.Add(email)
            Next

            Return info
        End Function

        Public Function SaveCommunicationPreference(facilityId As ApbFacilityId,
                                                    category As CommunicationCategory,
                                                    preference As CommunicationPreference,
                                                    userId As Integer
                                                    ) As Boolean
            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name),
                New SqlParameter("@preference", preference.Name),
                New SqlParameter("@userId", userId)
            }

            Return 0 = DB.SPReturnValue("geco.SaveCommunicationPreference", params)
        End Function

        Public Function SaveMailContact(facilityId As ApbFacilityId,
                                        category As CommunicationCategory,
                                        contact As MailContact,
                                        userId As Integer
                                        ) As Boolean
            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name),
                New SqlParameter("@name", contact.Name),
                New SqlParameter("@title", contact.Title),
                New SqlParameter("@organization", contact.Organization),
                New SqlParameter("@address1", contact.Address1),
                New SqlParameter("@address2", contact.Address2),
                New SqlParameter("@city", contact.City),
                New SqlParameter("@state", contact.State),
                New SqlParameter("@postalcode", contact.PostalCode),
                New SqlParameter("@telephone", contact.Telephone),
                New SqlParameter("@userId", userId)
            }

            Return 0 = DB.SPReturnValue("geco.SaveMailContact", params)
        End Function

        Public Function AddEmailContact(facilityId As ApbFacilityId,
                                        category As CommunicationCategory,
                                        email As String,
                                        userId As Integer
                                        ) As AddEmailContactResult

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name),
                New SqlParameter("@email", email),
                New SqlParameter("@userId", userId)
            }

            Dim result As Integer = DB.SPReturnValue("geco.AddEmailContact", params)

            Select Case result
                Case 0
                    Return AddEmailContactResult.Success
                Case 1
                    Return AddEmailContactResult.EmailExists
                Case Else
                    Return AddEmailContactResult.DbError
            End Select
        End Function

        Public Enum AddEmailContactResult
            DbError
            Success
            EmailExists
        End Enum

        Public Function RemoveEmailContact(facilityId As ApbFacilityId,
                                           category As CommunicationCategory,
                                           email As String,
                                           userId As Integer
                                           ) As Boolean
            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name),
                New SqlParameter("@email", email),
                New SqlParameter("@userId", userId)
            }

            Return 0 = DB.SPReturnValue("geco.RemoveEmailContact", params)
        End Function

        Public Function ResendEmailVerification(facilityId As ApbFacilityId,
                                                category As CommunicationCategory,
                                                email As String,
                                                userId As Integer
                                                ) As ResendEmailVerificationResult
            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name),
                New SqlParameter("@email", email),
                New SqlParameter("@userId", userId)
            }

            Dim result As Integer = DB.SPReturnValue("geco.ResendEmailVerification", params)

            Select Case result
                Case 0
                    Return ResendEmailVerificationResult.Success
                Case 1
                    Return ResendEmailVerificationResult.EmailDoesNotExist
                Case Else
                    Return ResendEmailVerificationResult.DbError
            End Select
        End Function

        Public Enum ResendEmailVerificationResult
            DbError
            Success
            EmailDoesNotExist
        End Enum

    End Module
End Namespace
