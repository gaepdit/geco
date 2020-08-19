Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module AnnualFees

    Public Function CheckFinalSubmit(airs As ApbFacilityId) As DataTable
        NotNull(airs, NameOf(airs))

        Dim SQL As String = "select numfeeyear as intyear, intsubmittal from " &
            "fs_admin where " &
            "strairsnumber = @airsno and " &
            "strenrolled = '1' and " &
            "(numcurrentstatus between '3' and '10') and " &
            "active = '1' " &
            "order by intyear desc"

        Dim param As SqlParameter = New SqlParameter("@airsno", airs.DbFormattedString)

        Return DB.GetDataTable(SQL, param)
    End Function

    Public Function GetFeeRates(feeyear As Integer) As AnnualFeeRates
        'In the database table fs_feerate, all the rates for a particular year are saved
        Dim SQL = "select NUMPART70FEE, NUMSMFEE, NUMPERTONRATE, NUMNSPSFEE, DATFEEDUEDATE, NUMADMINFEERATE, DATADMINAPPLICABLE,
            DATFIRSTQRTDUE, DATSECONDQRTDUE, DATTHIRDQRTDUE, DATFOURTHQRTDUE, NUMAATHRES, NUMNATHRES, MaintenanceFeeRate
            from FS_FEERATE where NUMFEEYEAR = @FeeYear "

        Dim param As SqlParameter = New SqlParameter("@FeeYear", feeyear)

        Dim dr As DataRow = DB.GetDataRow(SQL, param)

        If dr Is Nothing Then
            Return Nothing
        End If

        Return New AnnualFeeRates With {
            .PerTonRate = CDec(dr.Item("NUMPERTONRATE")),
            .SmFeeRate = CDec(dr.Item("NUMSMFEE")),
            .NspsFeeRate = CDec(dr.Item("NUMNSPSFEE")),
            .Part70MinFee = CDec(dr.Item("NUMPART70FEE")),
            .Part70MaintenanceFee = CDec(dr.Item("MaintenanceFeeRate")),
            .AdminFeeRate = CDec(dr.Item("NUMADMINFEERATE")),
            .AdminFeeDate = CDate(dr.Item("DATADMINAPPLICABLE")),
            .DueDate = CDate(dr.Item("DATFEEDUEDATE")),
            .FirstQuarterDueDate = CDate(dr.Item("DATFIRSTQRTDUE")),
            .SecondQuarterDueDate = CDate(dr.Item("DATSECONDQRTDUE")),
            .ThirdQuarterDueDate = CDate(dr.Item("DATTHIRDQRTDUE")),
            .FourthQuarterDueDate = CDate(dr.Item("DATFOURTHQRTDUE")),
            .AttainmentThreshold = CInt(dr.Item("NUMAATHRES")),
            .NonattainmentThreshold = CInt(dr.Item("NUMNATHRES"))
        }
    End Function

    Public Function GetNSPSExemptList(feeyear As Integer) As DataTable
        Dim SQL = "select fslk_NSPSReasonYear.NSPSReasonCode as ReasonID, fslk_NSPSReason.Description as Reason " &
            " FROM fslk_NSPSReasonYear " &
            " join fslk_NSPSReason on fslk_NSPSReasonYear.NSPSReasonCode = fslk_NSPSReason.NSPSReasonCode " &
            " where numFeeYear = @feeyear " &
            " order by fslk_NSPSReasonYear.DisplayOrder "

        Dim param As SqlParameter = New SqlParameter("@feeyear", feeyear)

        Return DB.GetDataTable(SQL, param)
    End Function

    Public Function GetClassInfo(feeyear As Integer) As DataRow
        Dim SQL = "Select strclass, strnsps, " &
            " strpart70 FROM fs_mailout " &
            " where strairsnumber = @airs and numfeeyear = @feeyear "

        Dim params As SqlParameter() = {
                New SqlParameter("@airs", "0413" & GetCookie(Cookie.AirsNumber)),
                New SqlParameter("@feeyear", feeyear)
            }

        Return DB.GetDataRow(SQL, params)
    End Function

    Public Function GetFS_ContactInfo(feeyear As Integer) As DataRow
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

    Public Function GetFacilityInfo(feeyear As Integer) As DataRow
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

    Public Function GetExistingFeeData(feeyear As Integer) As DataRow
        Dim SQL = "Select intvoctons, intpmtons, intso2tons, intnoxtons, " &
            " numpart70fee, numsmfee, numnspsfee, numtotalfee, " &
            " strnspsexempt, strnspsexemptreason, " &
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

    Public Function GetPaySubmitInfo(feeyear As Integer) As DataRow
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
        NotNull(airs, NameOf(airs))

        Dim query As String = " select " &
        "     a.STRAIRSNUMBER, " &
        "     a.NUMFEEYEAR, " &
        "     convert(bit, isnull(INTSUBMITTAL, 0)) as INTSUBMITTAL, " &
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

    Public Function FeeDataExists(airs As ApbFacilityId, feeyear As Integer) As Boolean
        NotNull(airs, NameOf(airs))

        Dim SQL As String = "select convert(bit, count(*)) from FS_FEEDATA
            where NUMFEEYEAR = @feeyear and STRAIRSNUMBER = @airsno"

        Dim params As SqlParameter() = {
            New SqlParameter("@airsno", airs.DbFormattedString),
            New SqlParameter("@feeyear", feeyear)
        }

        Return DB.GetBoolean(SQL, params)
    End Function

    Public Function GetAnnualFeeHistory(airs As ApbFacilityId) As DataTable
        NotNull(airs, NameOf(airs))

        Dim spName As String = "iaip_facility.GetAnnualFeesHistory"
        Return DB.SPGetDataTable(spName, New SqlParameter("@FacilityID", airs.DbFormattedString))
    End Function

    Public Function ActiveInvoiceExists(airs As ApbFacilityId, feeYear As Integer) As Boolean
        NotNull(airs, NameOf(airs))

        Dim SQL As String = "select convert(bit, count(*)) from FS_FEEINVOICE
            where STRAIRSNUMBER = @airsno and NUMFEEYEAR = @feeyear and ACTIVE = 1"

        Dim params As SqlParameter() = {
            New SqlParameter("@airsno", airs.DbFormattedString),
            New SqlParameter("@feeyear", feeYear)
        }

        Return DB.GetBoolean(SQL, params)
    End Function

End Module
