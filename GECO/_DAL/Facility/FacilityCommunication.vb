Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Namespace DAL.Facility
    Module FacilityCommunication

        Public Function GetFacilityCommunicationInfo(airs As ApbFacilityId) As Dictionary(Of CommunicationCategory, FacilityCommunicationInfo)
            Dim info As New Dictionary(Of CommunicationCategory, FacilityCommunicationInfo)

            For Each category In CommunicationCategory.AllCategories
                info.Add(category, GetFacilityCommunicationInfoPerCategory(airs, category))
            Next

            Return info
        End Function

        Private Function GetFacilityCommunicationInfoPerCategory(airs As ApbFacilityId, category As CommunicationCategory) As FacilityCommunicationInfo
            Dim params As SqlParameter() = {
                New SqlParameter("@facilityId", airs.DbFormattedString),
                New SqlParameter("@category", category.Name)
            }

            Dim ds As DataSet = DB.SPGetDataSet("geco.GetFacilityCommunicationInfo", params)

            If ds Is Nothing Then Return Nothing

            Dim info As New FacilityCommunicationInfo() With {
                .Preference = New FacilityCommunicationPreference(),
                .Emails = New List(Of FacilityEmailContact)
            }

            If ds.Tables(0).Rows.Count = 1 Then
                Dim dr1 As DataRow = ds.Tables(0).Rows(0)

                With info.Preference
                    .Id = CType(dr1("Id"), Guid)
                    .CommunicationPreference = CommunicationPreference.FromName(CStr(dr1("CommunicationPreference")))
                    .InitialConfirmationDate = CType(dr1("InitialConfirmationDate"), DateTimeOffset)
                End With
            End If

            If ds.Tables(1).Rows.Count = 1 Then
                Dim dr2 As DataRow = ds.Tables(1).Rows(0)
                info.Mail = New FacilityMailContact() With {
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
                Dim email As New FacilityEmailContact() With {
                    .Id = CType(row("Id"), Guid),
                    .Verified = CBool(row("Verified")),
                    .Email = CStr(row("Email"))
                }

                info.Emails.Add(email)
            Next

            Return info
        End Function

    End Module
End Namespace
