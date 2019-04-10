Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels

Namespace DAL
    Public Module PermitApplications

        Public Function ApplicationExists(appNumber As Integer) As Boolean
            If appNumber = 0 Then Return False
            Dim query As String = "SELECT CONVERT( bit, COUNT(*)) FROM SSPPAPPLICATIONMASTER WHERE STRAPPLICATIONNUMBER = @id "
            Return DB.GetBoolean(query, New SqlParameter("@id", appNumber))
        End Function

        Public Function GetPermitApplications(airsNumber As ApbFacilityId) As DataTable
            Dim spName As String = "iaip_facility.PermitApplications"
            Return DB.SPGetDataTable(spName, New SqlParameter("@AirsNumber", airsNumber.DbFormattedString))
        End Function

        Public Function GetPermitApplicationCounts(airsNumber As ApbFacilityId) As DataRow
            Dim spName As String = "iaip_facility.GetApplicationCounts"
            Return DB.SPGetDataRow(spName, New SqlParameter("@AirsNumber", airsNumber.DbFormattedString))
        End Function

        Public Function GetPermitApplication(appNumber As Integer) As PermitApplication
            If appNumber = 0 Then Return Nothing

            Dim ds As DataSet = DB.SPGetDataSet("iaip_facility.GetPermitApplication", New SqlParameter("@AppNumber", appNumber))

            If ds Is Nothing OrElse ds.Tables.Count <> 4 OrElse ds.Tables(0).Rows.Count <> 1 OrElse ds.Tables(1).Rows.Count <> 1 Then Return Nothing

            Dim permitApplication As PermitApplication = PermitApplicationFromDataRow(appNumber, ds.Tables(0).Rows(0))

            With permitApplication
                .ApplicationFeeInfo = ApplicationFeeInfoFromDataRow(ds.Tables(1).Rows(0))
                .ApplicationInvoices = ds.Tables(2)
                .ApplicationPayments = ds.Tables(3)
            End With

            Return permitApplication
        End Function

        Private Function PermitApplicationFromDataRow(appNumber As Integer, dr As DataRow) As PermitApplication
            Dim staffResponsible As New ApbStaff With {
                .IaipID = CInt(dr.Item("IaipID")),
                .FirstName = GetNullableString(dr.Item("FirstName")),
                .LastName = GetNullableString(dr.Item("LastName")),
                .Email = GetNullableString(dr.Item("Email")),
                .ActiveEmployee = CBool(dr.Item("ActiveEmployee"))
            }

            staffResponsible.SetUnformattedPhoneNumber(GetNullableString(dr.Item("Phone")))

            Dim facilityAddress As New Address With {
                .Street = GetNullableString(dr.Item("FacilityStreet")),
                .Street2 = GetNullableString(dr.Item("FacilityStreet2")),
                .City = GetNullableString(dr.Item("FacilityCity")),
                .State = GetNullableString(dr.Item("FacilityState")),
                .PostalCode = GetNullableString(dr.Item("FacilityPostalCode"))
            }

            Dim airs As ApbFacilityId = ApbFacilityId.IfValid(GetNullableString(dr.Item("AirsNumber")))

            Dim permitApplication As New PermitApplication With {
                .AppNumber = appNumber,
                .FacilityAddress = facilityAddress,
                .StaffResponsible = staffResponsible,
                .FacilityID = airs,
                .ApplicationResult = GetNullableString(dr.Item("ApplicationResult")),
                .ApplicationType = GetNullableString(dr.Item("ApplicationType")),
                .ApplicationWithdrawn = CBool(dr.Item("ApplicationWithdrawn")),
                .DateAcknowledgementLetterSent = GetNullableDateTime(dr.Item("DateAcknowledgementLetterSent")),
                .DateApplicationFinalized = GetNullableDateTime(dr.Item("DateApplicationFinalized")),
                .DateApplicationWithdrawn = GetNullableDateTime(dr.Item("DateApplicationWithdrawn")),
                .DateAssignedToStaff = GetNullableDateTime(dr.Item("DateAssignedToStaff")),
                .DateDraftIssued = GetNullableDateTime(dr.Item("DateDraftIssued")),
                .DateEpaCommentPeriodEnds = GetNullableDateTime(dr.Item("DateEpaCommentPeriodEnds")),
                .DateEpaCommentPeriodWaived = GetNullableDateTime(dr.Item("DateEpaCommentPeriodWaived")),
                .DatePAExpires = GetNullableDateTime(dr.Item("DatePAExpires")),
                .DatePermitIssued = GetNullableDateTime(dr.Item("DatePermitIssued")),
                .DatePNExpires = GetNullableDateTime(dr.Item("DatePNExpires")),
                .DateReassignedToStaff = GetNullableDateTime(dr.Item("DateReassignedToStaff")),
                .DateReceivedByApb = GetNullableDateTime(dr.Item("DateReceivedByApb")),
                .DateSentByFacility = GetNullableDateTime(dr.Item("DateSentByFacility")),
                .DateToAdministrativeReview = GetNullableDateTime(dr.Item("DateToAdministrativeReview")),
                .DateToProgramManager = GetNullableDateTime(dr.Item("DateToProgramManager")),
                .DateToUnitManager = GetNullableDateTime(dr.Item("DateToUnitManager")),
                .FacilityCounty = GetNullableString(dr.Item("FacilityCounty")),
                .FacilityDescription = GetNullableString(dr.Item("FacilityDescription")),
                .FacilityDistrict = GetNullableString(dr.Item("FacilityDistrict")),
                .FacilityNaicsCode = GetNullableString(dr.Item("FacilityNaicsCode")),
                .FacilityName = GetNullableString(dr.Item("FacilityName")),
                .FacilitySicCode = GetNullableString(dr.Item("FacilitySicCode")),
                .IsInvoiceGenerated = CBool(dr.Item("IsInvoiceGeneratedForApplication")),
                .PermitNumberInDB = GetNullableString(dr.Item("PermitNumber")),
                .PermitFileName = GetNullableString(dr.Item("PermitFileName")),
                .PublicAdvisoryNeeded = GetNullableString(dr.Item("PublicAdvisoryNeeded")),
                .ReasonForApplication = GetNullableString(dr.Item("ReasonForApplication")),
                .Status = GetNullableString(dr.Item("Status")),
                .StatusDate = GetNullableDateTime(dr.Item("StatusDate")),
                .UnitResponsible = GetNullableString(dr.Item("UnitResponsible"))
            }

            Return permitApplication
        End Function
    End Module
End Namespace