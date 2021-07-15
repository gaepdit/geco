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

            Dim returnValue As Integer
            Dim dr As DataRow = DB.SPGetDataRow("geco.AddEmailContact", params, returnValue)

            Dim result As New AddEmailContactResult()

            Select Case returnValue
                Case 0
                    result.Status = AddEmailContactResultStatus.Success
                    result.Seq = CStr(dr("Seq"))
                    result.Token = CStr(dr("Token"))
                Case 1
                    result.Status = AddEmailContactResultStatus.EmailExists
                Case Else
                    result.Status = AddEmailContactResultStatus.DbError
            End Select

            Return result
        End Function

        Public Class AddEmailContactResult
            Public Property Status As AddEmailContactResultStatus
            Public Property Seq As String
            Public Property Token As String
        End Class

        Public Enum AddEmailContactResultStatus
            DbError
            Success
            EmailExists
        End Enum

        Public Function RemoveEmailContact(facilityId As ApbFacilityId,
                                           category As CommunicationCategory,
                                           email As String) As Boolean

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", facilityId.DbFormattedString),
                New SqlParameter("@category", category.Name),
                New SqlParameter("@email", email)
            }

            Return 0 = DB.SPReturnValue("geco.RemoveEmailContact", params)
        End Function

        Public Function RefreshEmailContactToken(facilityId As ApbFacilityId,
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

            Dim returnValue As Integer
            Dim dr As DataRow = DB.SPGetDataRow("geco.RefreshEmailContactToken", params, returnValue)

            Dim result As New ResendEmailVerificationResult

            Select Case returnValue
                Case 0
                    result.Status = ResendEmailVerificationResultStatus.Success
                    result.Seq = CStr(dr("Seq"))
                    result.Token = CStr(dr("Token"))
                Case 1
                    result.Status = ResendEmailVerificationResultStatus.EmailDoesNotExist
                Case Else
                    result.Status = ResendEmailVerificationResultStatus.DbError
            End Select

            Return result
        End Function

        Public Class ResendEmailVerificationResult
            Public Property Status As ResendEmailVerificationResultStatus
            Public Property Seq As String
            Public Property Token As String
        End Class

        Public Enum ResendEmailVerificationResultStatus
            DbError
            Success
            EmailDoesNotExist
        End Enum

        Public Function InitialCommunicationPreferenceSettingRequired(facilityId As ApbFacilityId, category As CommunicationCategory) As Boolean
            Return Not GetFacilityCommunicationPreference(facilityId, category).IsConfirmed
        End Function

        Public Function RoutineConfirmationRequired(facilityId As ApbFacilityId, access As FacilityAccess) As Boolean
            For Each category In CommunicationCategory.AllCategories
                If access.HasCommunicationPermission(category) Then
                    Dim params As SqlParameter() = {
                        New SqlParameter("@category", category.Name),
                        New SqlParameter("@facilityId", facilityId.DbFormattedString)
                    }

                    Dim confirmationDate As DateTimeOffset? = DB.SPGetSingleValue(Of DateTimeOffset?)("geco.GetFacilityConfirmationDate", params)

                    If Not confirmationDate.HasValue OrElse (DateTimeOffset.Now.Date - confirmationDate.Value.Date).Days > 275 Then
                        Return True
                    End If
                End If
            Next

            Return False
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

        Public Function ConfirmContactEmail(seq As String, token As String) As ConfirmContactEmailResult
            Dim params As SqlParameter() = {
                New SqlParameter("@seq", seq),
                New SqlParameter("@token", token)
            }

            Dim result As New ConfirmContactEmailResult
            Dim returnValue As Integer
            Dim dr As DataRow = DB.SPGetDataRow("geco.ConfirmContactEmail", params, returnValue)

            If dr Is Nothing Then
                result.Success = False
            Else
                Select Case returnValue
                    Case 0
                        result.Success = True
                        result.FacilityId = New ApbFacilityId(dr("FacilityId").ToString)
                        result.FacilityName = GetNullableString(dr("FacilityName"))
                        result.FacilityCity = GetNullableString(dr("FacilityCity"))
                        result.CategoryDesc = CommunicationCategory.FromName(CStr(dr("Category"))).Description
                    Case -1
                        result.Success = False
                End Select
            End If

            Return result
        End Function

        Public Class ConfirmContactEmailResult
            Public Property Success As Boolean
            Public Property FacilityId As ApbFacilityId
            Public Property FacilityName As String
            Public Property FacilityCity As String
            Public Property CategoryDesc As String
        End Class

    End Module
End Namespace
