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

        Public Function GetPermitApplication(appNumber As Integer) As PermitApplication
            If appNumber = 0 Then Return Nothing

            Dim query As String = "select m.STRAIRSNUMBER as AirsNumber, " &
            "       m.STRSTAFFRESPONSIBLE as IaipID, " &
            "       u.STRFIRSTNAME as FirstName, " &
            "       u.STRLASTNAME as LastName, " &
            "       u.STREMAILADDRESS as Email, " &
            "       u.STRPHONE as Phone, " &
            "       convert(bit, u.NUMEMPLOYEESTATUS) as ActiveEmployee, " &
            "       d.STRFACILITYNAME as FacilityName, " &
            "       d.STRFACILITYSTREET1 as FacilityStreet, " &
            "       case " &
            "           when d.STRFACILITYSTREET2 = 'N/A' then null " &
            "           else d.STRFACILITYSTREET2 " &
            "       end as FacilityStreet2, " &
            "       d.STRFACILITYCITY as FacilityCity, " &
            "       d.STRFACILITYSTATE as FacilityState, " &
            "       d.STRFACILITYZIPCODE as FacilityPostalCode, " &
            "       lp.STRPERMITTYPEDESCRIPTION as ApplicationResult, " &
            "       la.STRAPPLICATIONTYPEDESC as ApplicationType, " &
            "       case " &
            "           when m.STRPERMITTYPE = 11 then convert(bit, 1) " &
            "           else convert(bit, 0) " &
            "       end as ApplicationWithdrawn, " &
            "       convert(date, t.DATACKNOWLEDGEMENTLETTERSENT) as DateAcknowledgementLetterSent, " &
            "       convert(date, m.DATFINALIZEDDATE) as DateApplicationFinalized, " &
            "       convert(date, t.DATWITHDRAWN) as DateApplicationWithdrawn, " &
            "       convert(date, t.DATASSIGNEDTOENGINEER) as DateAssignedToStaff, " &
            "       convert(date, t.DATDRAFTISSUED) as DateDraftIssued, " &
            "       convert(date, t.DATEPAENDS) as DateEpaCommentPeriodEnds, " &
            "       convert(date, t.DATEPAWAIVED) as DateEpaCommentPeriodWaived, " &
            "       convert(date, t.DATPAEXPIRES) as DatePAExpires, " &
            "       convert(date, t.DATPERMITISSUED) as DatePermitIssued, " &
            "       convert(date, t.DATPNEXPIRES) as DatePNExpires, " &
            "       convert(date, t.DATREASSIGNEDTOENGINEER) as DateReassignedToStaff, " &
            "       convert(date, t.DATRECEIVEDDATE) as DateReceivedByApb, " &
            "       convert(date, t.DATSENTBYFACILITY) as DateSentByFacility, " &
            "       convert(date, t.DATTOBRANCHCHEIF) as DateToAdministrativeReview, " &
            "       convert(date, t.DATTOPMII) as DateToProgramManager, " &
            "       convert(date, t.DATTOPMI) as DateToUnitManager, " &
            "       lc.STRCOUNTYNAME as FacilityCounty, " &
            "       d.STRPLANTDESCRIPTION as FacilityDescription, " &
            "       ld.STRDISTRICTNAME as FacilityDistrict, " &
            "       d.STRNAICSCODE as FacilityNaicsCode, " &
            "       d.STRFACILITYNAME as FacilityName, " &
            "       d.STRSICCODE as FacilitySicCode, " &
            "       d.STRPERMITNUMBER as PermitNumber, " &
            "       CASE " &
            "           WHEN p.PDFPERMITDATA IS NOT NULL " &
            "                 THEN 'PDF-' + p.STRFILENAME " &
            "           WHEN p.DOCPERMITDATA IS NOT NULL " &
            "                 THEN 'DOC-' + p.STRFILENAME " &
            "           else null " &
            "       END AS PermitFileName, " &
            "       case " &
            "           when d.STRPUBLICINVOLVEMENT = '1' then 'PA Needed' " &
            "           when d.STRPUBLICINVOLVEMENT = '2' then 'PA Not Needed' " &
            "           else 'Not Decided' " &
            "       end as PublicAdvisoryNeeded, " &
            "       d.STRAPPLICATIONNOTES as ReasonForApplication, " &
            "       CASE " &
            "           WHEN t.DATPERMITISSUED IS NOT NULL OR m.DATFINALIZEDDATE IS NOT NULL THEN 'Closed Out' " &
            "           WHEN t.DATTODIRECTOR IS NOT NULL AND m.DATFINALIZEDDATE IS NULL AND " &
            "                (t.DATDRAFTISSUED IS NULL OR t.DATDRAFTISSUED < t.DATTODIRECTOR) THEN 'Administrative Review' " &
            "           WHEN t.DATTOBRANCHCHEIF IS NOT NULL AND m.DATFINALIZEDDATE IS NULL AND t.DATTODIRECTOR IS NULL AND " &
            "                (t.DATDRAFTISSUED IS NULL OR t.DATDRAFTISSUED < t.DATTOBRANCHCHEIF) THEN 'Administrative Review' " &
            "           WHEN t.DATEPAENDS IS NOT NULL THEN 'EPA 45-day Review' " &
            "           WHEN t.DATPNEXPIRES IS NOT NULL AND t.DATPNEXPIRES < GETDATE() THEN 'Public Notice Expired' " &
            "           WHEN t.DATPNEXPIRES IS NOT NULL AND t.DATPNEXPIRES >= GETDATE() THEN 'Public Notice' " &
            "           WHEN t.DATDRAFTISSUED IS NOT NULL AND t.DATPNEXPIRES IS NULL THEN 'Draft Issued' " &
            "           WHEN t.DATTOPMII IS NOT NULL THEN 'At PM' " &
            "           WHEN t.DATTOPMI IS NOT NULL THEN 'At UC' " &
            "           WHEN t.DATREVIEWSUBMITTED IS NOT NULL AND (d.STRSSCPUNIT <> '0' OR d.STRISMPUNIT <> '0') " &
            "                 THEN 'Internal Review' " &
            "           WHEN m.STRSTAFFRESPONSIBLE IS NULL OR m.STRSTAFFRESPONSIBLE = '0' THEN 'Unassigned' " &
            "           ELSE 'At Engineer' " &
            "       END as Status, " &
            "       CASE " &
            "           WHEN t.DATPERMITISSUED IS NOT NULL THEN CONVERT(date, t.DATPERMITISSUED) " &
            "           WHEN m.DATFINALIZEDDATE IS NOT NULL THEN CONVERT(date, m.DATFINALIZEDDATE) " &
            "           WHEN t.DATTODIRECTOR IS NOT NULL AND m.DATFINALIZEDDATE IS NULL AND " &
            "                (t.DATDRAFTISSUED IS NULL OR t.DATDRAFTISSUED < t.DATTODIRECTOR) THEN CONVERT(date, t.DATTODIRECTOR) " &
            "           WHEN t.DATTOBRANCHCHEIF IS NOT NULL AND m.DATFINALIZEDDATE IS NULL AND t.DATTODIRECTOR IS NULL AND " &
            "                (t.DATDRAFTISSUED IS NULL OR t.DATDRAFTISSUED < t.DATTOBRANCHCHEIF) " &
            "                 THEN CONVERT(date, t.DATTOBRANCHCHEIF) " &
            "           WHEN t.DATEPAENDS IS NOT NULL THEN CONVERT(date, t.DATEPAENDS) " &
            "           WHEN t.DATPNEXPIRES IS NOT NULL AND t.DATPNEXPIRES < GETDATE() THEN CONVERT(date, t.DATPNEXPIRES) " &
            "           WHEN t.DATPNEXPIRES IS NOT NULL AND t.DATPNEXPIRES >= GETDATE() THEN CONVERT(date, t.DATPNEXPIRES) " &
            "           WHEN t.DATDRAFTISSUED IS NOT NULL AND t.DATPNEXPIRES IS NULL THEN CONVERT(date, t.DATDRAFTISSUED) " &
            "           WHEN t.DATTOPMII IS NOT NULL THEN CONVERT(date, t.DATTOPMII) " &
            "           WHEN t.DATTOPMI IS NOT NULL THEN CONVERT(date, t.DATTOPMI) " &
            "           WHEN t.DATREVIEWSUBMITTED IS NOT NULL AND (d.STRSSCPUNIT <> '0' OR d.STRISMPUNIT <> '0') " &
            "                 THEN CONVERT(date, t.DATREVIEWSUBMITTED) " &
            "           WHEN m.STRSTAFFRESPONSIBLE IS NULL OR m.STRSTAFFRESPONSIBLE = '000' THEN 'Unknown' " &
            "           ELSE CONVERT(date, t.DATASSIGNEDTOENGINEER) " &
            "       END AS StatusDate, " &
            "       lu.STRUNITDESC as UnitResponsible " &
            "from SSPPAPPLICATIONMASTER m " &
            "     inner join SSPPAPPLICATIONDATA d " &
            "             on m.STRAPPLICATIONNUMBER = d.STRAPPLICATIONNUMBER " &
            "     inner join SSPPAPPLICATIONTRACKING t " &
            "             on m.STRAPPLICATIONNUMBER = t.STRAPPLICATIONNUMBER " &
            "     left join EPDUSERPROFILES u " &
            "             on convert(int, m.STRSTAFFRESPONSIBLE) = convert(int, u.NUMUSERID) " &
            "     left join LOOKUPPERMITTYPES lp " &
            "             on lp.STRPERMITTYPECODE = m.STRPERMITTYPE " &
            "     left join LOOKUPAPPLICATIONTYPES la " &
            "             on la.STRAPPLICATIONTYPECODE = m.STRAPPLICATIONTYPE " &
            "     left join LOOKUPEPDUNITS lu " &
            "             on convert(int, lu.NUMUNITCODE) = convert(int, m.APBUNIT) " &
            "     left join LOOKUPCOUNTYINFORMATION lc " &
            "             on lc.STRCOUNTYCODE = substring(m.STRAIRSNUMBER, 5, 3) " &
            "     left join LOOKUPDISTRICTINFORMATION li " &
            "             on li.STRDISTRICTCODE = lc.STRCOUNTYCODE " &
            "     left join LOOKUPDISTRICTS ld " &
            "             on ld.STRDISTRICTCODE = li.STRDISTRICTCODE " &
            "     left join APBPERMITS p " &
            "             on SUBSTRING(p.STRFILENAME, 4, 15) = t.STRAPPLICATIONNUMBER " &
            "                    and SUBSTRING(p.STRFILENAME, 1, 2) in ('VF', 'PI', 'OP') " &
            "                    and t.DATFINALONWEB is not null " &
            "where m.STRAPPLICATIONNUMBER = @appNumber "

            Dim dr As DataRow = DB.GetDataRow(query, New SqlParameter("@appNumber", appNumber))

            If dr Is Nothing Then Return Nothing

            Dim staffResponsible As New ApbStaff With {
                .IaipID = CInt(dr.Item("IaipID")),
                .FirstName = GetNullableString(dr.Item("FirstName")),
                .LastName = GetNullableString(dr.Item("LastName")),
                .Email = GetNullableString(dr.Item("Email")),
                .PhoneNumber = GetNullableString(dr.Item("Phone")),
                .ActiveEmployee = CBool(dr.Item("ActiveEmployee"))
            }

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
                .ApplicationFeeInfo = GetApplicationFeesInfo(appNumber),
                .ApplicationInvoices = GetApplicationInvoices(appNumber),
                .ApplicationPayments = GetApplicationPayments(appNumber),
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
                .IsInvoiceGenerated = IsInvoiceGeneratedForApplication(appNumber),
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