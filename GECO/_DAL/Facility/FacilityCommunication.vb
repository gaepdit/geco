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
                    .IsConfirmed = True
                    .CommunicationPreference = CommunicationPreference.FromName(CStr(dr1("CommunicationPreference")))
                End With
            End If

            If ds.Tables(1).Rows.Count = 1 Then
                Dim dr2 As DataRow = ds.Tables(1).Rows(0)
                info.Mail = New MailContact() With {
                    .Id = CType(dr2("Id"), Guid),
                    .Address1 = CStr(dr2("Address1")),
                    .Address2 = GetNullableString(dr2("Address2")),
                    .City = CStr(dr2("City")),
                    .FirstName = GetNullableString(dr2("FirstName")),
                    .LastName = GetNullableString(dr2("LastName")),
                    .Prefix = GetNullableString(dr2("Prefix")),
                    .Organization = GetNullableString(dr2("Organization")),
                    .PostalCode = CStr(dr2("PostalCode")),
                    .State = CStr(dr2("State")),
                    .Telephone = GetNullableString(dr2("Telephone")),
                    .Email = GetNullableString(dr2("Email")),
                    .Title = GetNullableString(dr2("Title"))
                }
            End If

            For Each row As DataRow In ds.Tables(2).Rows
                info.Emails.Add(New EmailContact(CType(row("Id"), Guid), CStr(row("Email"))))
            Next

            Return info
        End Function

        Public Function GetFacilityCommunicationPreference(facilityId As ApbFacilityId,
                                                           category As CommunicationCategory
                                                           ) As FacilityCommunicationPreference

            Dim pref As New FacilityCommunicationPreference()

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name)
            }

            Dim dr As DataRow = DB.SPGetDataRow("geco.GetFacilityCommunicationPreference", params)

            If dr IsNot Nothing Then
                With pref
                    .Id = CType(dr("Id"), Guid)
                    .IsConfirmed = True
                    .CommunicationPreference = CommunicationPreference.FromName(CStr(dr("CommunicationPreference")))
                End With
            End If

            Return pref
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
                New SqlParameter("@firstName", contact.FirstName),
                New SqlParameter("@lastName", contact.LastName),
                New SqlParameter("@prefix", contact.Prefix),
                New SqlParameter("@title", contact.Title),
                New SqlParameter("@organization", contact.Organization),
                New SqlParameter("@address1", contact.Address1),
                New SqlParameter("@address2", contact.Address2),
                New SqlParameter("@city", contact.City),
                New SqlParameter("@state", contact.State),
                New SqlParameter("@postalcode", contact.PostalCode),
                New SqlParameter("@telephone", contact.Telephone),
                New SqlParameter("@email", contact.Email),
                New SqlParameter("@userId", userId)
            }

            Return 0 = DB.SPReturnValue("geco.SaveMailContact", params)
        End Function

        Public Function AddEmailContact(facilityId As ApbFacilityId,
                                        category As CommunicationCategory,
                                        email As String,
                                        userId As Integer
                                        ) As AddEmailContactResultStatus

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name),
                New SqlParameter("@email", email),
                New SqlParameter("@userId", userId)
            }

            Select Case DB.SPReturnValue("geco.AddEmailContact", params)
                Case 0
                    Return AddEmailContactResultStatus.Success
                Case 1
                    Return AddEmailContactResultStatus.EmailExists
                Case Else
                    Return AddEmailContactResultStatus.DbError
            End Select

        End Function

        Public Enum AddEmailContactResultStatus
            DbError
            Success
            EmailExists
        End Enum

        Public Function RemoveEmailContact(facilityId As ApbFacilityId,
                                           category As CommunicationCategory,
                                           email As String,
                                           userId As Integer) As Boolean

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name),
                New SqlParameter("@email", email),
                New SqlParameter("@userId", userId)
            }

            Return 0 = DB.SPReturnValue("geco.RemoveEmailContact", params)
        End Function

        Public Function InitialCommunicationPreferenceSettingRequired(facilityId As ApbFacilityId,
                                                                      access As FacilityAccess,
                                                                      category As CommunicationCategory) As Boolean
            Return category.CommunicationPreferenceEnabled AndAlso
                access.HasCommunicationPermission(category) AndAlso
                Not GetFacilityCommunicationPreference(facilityId, category).IsConfirmed
        End Function

        Public Function CommunicationUpdateResponseRequired(facilityId As ApbFacilityId, access As FacilityAccess) As Boolean
            Return GetCommunicationUpdate(facilityId, access).ResponseRequired
        End Function

        Public Function GetCommunicationUpdate(facilityId As ApbFacilityId, access As FacilityAccess) As CommunicationUpdateResponse
            Dim response As New CommunicationUpdateResponse()

            For Each category In CommunicationCategory.AllCategories
                If category.CommunicationPreferenceEnabled AndAlso access.HasCommunicationPermission(category) Then
                    Dim params As SqlParameter() = {
                        New SqlParameter("@category", category.Name),
                        New SqlParameter("@facilityId", facilityId.DbFormattedString)
                    }

                    Dim status As Integer = DB.SPReturnValue("geco.IsCommunicationUpdateRequired", params)

                    response.AddCategoryUpdate(category, CType(status, CommunicationUpdateResponse.CategoryUpdateStatus))
                End If
            Next

            Return response
        End Function

        Public Sub ConfirmCommunicationSettings(facilityId As ApbFacilityId, userId As Integer, access As FacilityAccess)
            For Each category In CommunicationCategory.AllCategories
                If access.HasCommunicationPermission(category) Then
                    Dim params As SqlParameter() = {
                        New SqlParameter("@facilityId", facilityId.DbFormattedString),
                        New SqlParameter("@userId", userId),
                        New SqlParameter("@category", category.Name)
                    }

                    DB.SPRunCommand("geco.ConfirmCommunicationPreference", params)
                End If
            Next
        End Sub

    End Module
End Namespace
