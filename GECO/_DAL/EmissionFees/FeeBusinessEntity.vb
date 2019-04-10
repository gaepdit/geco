Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module FeeBusinessEntity

    Public Function CheckFinalSubmit(airsno As ApbFacilityId) As DataTable
        Dim SQL As String = "select numfeeyear as intyear, intsubmittal from " &
            "fs_admin where " &
            "strairsnumber = @airsno and " &
            "strenrolled = '1' and " &
            "(numcurrentstatus between '3' and '10') and " &
            "active = '1' " &
            "order by intyear desc"

        Dim param As SqlParameter = New SqlParameter("@airsno", airsno.DbFormattedString)

        Return DB.GetDataTable(SQL, param)
    End Function

    Public Function GetFeeRates(ByVal feeyear As Integer) As DataRow
        'In the database table fs_feerate, all the rates for a particular year are saved
        Dim SQL = "Select numsmfee, " &
            " numpertonrate, numnspsfee, " &
            " numpart70fee, numadminfeerate, " &
            " datfeeduedate, datadminapplicable, " &
            " DATFIRSTQRTDUE, DATSECONDQRTDUE, DATTHIRDQRTDUE, DATFOURTHQRTDUE, NUMAATHRES, NUMNATHRES " &
            " FROM fs_feerate " &
            " where numfeeyear = @feeyear "

        Dim param As SqlParameter = New SqlParameter("@feeyear", feeyear)

        Return DB.GetDataRow(SQL, param)
    End Function

    Public Function GetNSPSExemptList(ByVal feeyear As Integer) As DataTable
        Dim SQL = "select fslk_NSPSReasonYear.NSPSReasonCode as ReasonID, fslk_NSPSReason.Description as Reason " &
            " FROM fslk_NSPSReasonYear " &
            " join fslk_NSPSReason on fslk_NSPSReasonYear.NSPSReasonCode = fslk_NSPSReason.NSPSReasonCode " &
            " where numFeeYear = @feeyear " &
            " order by fslk_NSPSReasonYear.DisplayOrder "

        Dim param As SqlParameter = New SqlParameter("@feeyear", feeyear)

        Return DB.GetDataTable(SQL, param)
    End Function

    Public Function GetClassInfo(ByVal feeyear As Integer) As DataRow
        Dim SQL = "Select strclass, strnsps, " &
            " strpart70 FROM fs_mailout " &
            " where strairsnumber = @airs and numfeeyear = @feeyear "

        Dim params As SqlParameter() = {
                New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@feeyear", feeyear)
            }

        Return DB.GetDataRow(SQL, params)
    End Function

    Public Function GetFS_ContactInfo(ByVal feeyear As Integer) As DataRow
        'In the database table fs_contactinfo, the contact for
        'Fees has a contact key of airsnumber and fee year

        Dim query = "Select strcontactfirstname, strcontactlastname, " &
            " strcontacttitle, strcontactcompanyname, " &
            " strcontactphonenumber, strcontactfaxnumber, strcontactemail, " &
            " strcontactaddress, strcontactcity, " &
            " strcontactstate, strcontactzipcode " &
            " FROM fs_contactinfo " &
            " where strairsnumber = @airs and numfeeyear = @feeyear "

        Dim params As SqlParameter() = {
            New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber)),
            New SqlParameter("@feeyear", feeyear)
        }

        Return DB.GetDataRow(query, params)
    End Function

    Public Function GetAPBContactInformation(key As Integer) As DataRow
        'In the database table fs_contactinfo, the contact for
        'Fees has a contact key of airsnumber and two digits (40)

        Dim query = "Select strcontactfirstname, strcontactlastname, " &
            " strcontacttitle, strcontactcompanyname, " &
            " strcontactphonenumber1 as strcontactphonenumber, strcontactfaxnumber, strcontactemail, " &
            " strcontactaddress1 as strcontactaddress, strcontactcity, " &
            " strcontactstate, strcontactzipcode " &
            " FROM APBContactInformation " &
            " where strairsnumber = @airs and strkey = @key "

        Dim params As SqlParameter() = {
            New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber)),
            New SqlParameter("@key", key)
        }

        Return DB.GetDataRow(query, params)
    End Function

    Public Function GetFacilityInfo(ByVal feeyear As Integer) As DataRow
        'In the database table apbcontactinformation, the contact for
        'Fees has a contact key of airsnumber and two digits (40)
        Dim SQL = " Select strfacilityname, strfacilityaddress1, " &
            " strfacilitycity " &
            " FROM fs_mailout " &
            " where strairsnumber = @airs and numfeeyear = @feeyear "

        Dim params As SqlParameter() = {
                New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@feeyear", feeyear)
            }

        Return DB.GetDataRow(SQL, params)
    End Function

    Public Function GetFacilityInfoTemp() As DataRow
        'In the database table apbcontactinformation, the contact for
        'Fees has a contact key of airsnumber and two digits (40)
        Dim SQL = "Select strfacilityname, strfacilitystreet1, " &
            " strfacilitycity " &
            " FROM apbfacilityinfotemp " &
            " where strairsnumber = @airs "

        Dim param As SqlParameter = New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber))

        Return DB.GetDataRow(SQL, param)
    End Function

    Public Function GetCalculationInfo(ByVal feeyear As Integer) As DataRow
        Dim SQL = "Select intvoctons, intpmtons, intso2tons, intnoxtons, " &
            " numpart70fee, numsmfee, numnspsfee, numtotalfee, " &
            " strnspsexempt, strnspsexemptreason as strnspsreason, " &
            " strnsps, strpart70, strsyntheticminor, numcalculatedfee, " &
            " strclass, numfeerate FROM fs_feeauditeddata " &
            " where strairsnumber = @airs " &
            " and numfeeyear = @feeyear "

        Dim params As SqlParameter() = {
                New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@feeyear", feeyear)
            }

        Return DB.GetDataRow(SQL, params)
    End Function

    Public Function GetPaySubmitInfo(ByVal feeyear As Integer) As DataRow
        Dim SQL = "SELECT aud.STRPAYMENTPLAN, " &
            " aud.strofficialname, " &
            " aud.strofficialtitle, " &
            " dat.strcomment " &
            " FROM fs_feeauditeddata aud " &
            " JOIN fs_feedata dat ON aud.strairsnumber = dat.strairsnumber " &
            " WHERE aud.strairsnumber = @airs " &
            " And aud.numfeeyear = dat.numfeeyear " &
            " And aud.numfeeyear = @feeyear"

        Dim params As SqlParameter() = {
                New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@feeyear", feeyear)
            }

        Return DB.GetDataRow(SQL, params)
    End Function

    Public Function GetFeeStatus(airs As ApbFacilityId) As DataRow
        Dim query As String = " select " &
        "     a.STRAIRSNUMBER, " &
        "     a.NUMFEEYEAR, " &
        "     convert(bit, INTSUBMITTAL) as INTSUBMITTAL, " &
        "     DATSUBMITTAL, " &
        "     STRGECODESC, " &
        "     DATFEEDUEDATE " &
        " FROM FS_ADMIN a " &
        "     inner join FSLK_ADMIN_STATUS s " &
        "         on a.NUMCURRENTSTATUS = s.ID " &
        "     inner join FS_FEERATE r " &
        "         on a.NUMFEEYEAR = r.NUMFEEYEAR " &
        " where a.STRAIRSNUMBER = @airs " &
        "       and a.NUMFEEYEAR = " &
        "           (select max(NUMFEEYEAR) " &
        "            from FS_ADMIN " &
        "            where STRAIRSNUMBER = @airs) "

        Dim param As New SqlParameter("@airs", airs.DbFormattedString)

        Return DB.GetDataRow(query, param)
    End Function

    Public Function GetAnnualFeeHistory(airs As ApbFacilityId) As DataTable
        Dim spName As String = "iaip_facility.GetAnnualFeesHistory"

        Return DB.SPGetDataTable(spName, New SqlParameter("@FacilityID", airs.DbFormattedString))
    End Function

End Module
