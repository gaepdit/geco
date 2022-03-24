Imports System.Data.SqlClient
Imports System.Math
Imports GECO.GecoModels

Public Module EmissionsStatement

    Public Function StatusES(ayr As String) As String
        Dim esStatus As String

        If CheckESExist(ayr) Then
            Dim OptOut As String = CheckESOptOut(ayr)
            If OptOut = "YES" OrElse OptOut = "NO" Then
                Dim esSubmitted As String = GetESSubmit(ayr)

                If OptOut = "YES" Then
                    esStatus = "Opted out & submitted on " & esSubmitted & "."
                Else
                    esStatus = "Opted in & submitted on " & esSubmitted & "."
                End If
            Else
                esStatus = "Applicable; needs to be done."
            End If
        Else
            esStatus = "N/A"
        End If

        Return esStatus
    End Function

    Public Function CheckESExist(ay As String) As Boolean
        Dim query As String = "Select convert(bit, count(*)) FROM esSchema where strAirsYear = @ay "

        Dim param As New SqlParameter("@ay", ay)

        Return DB.GetBoolean(query, param)
    End Function

    Public Function CheckESOptOut(ay As String) As String
        Dim query As String = "Select strOptOut FROM esSchema where strAirsYear = @ay "

        Dim param As New SqlParameter("@ay", ay)

        Dim result = DB.GetString(query, param)

        If String.IsNullOrEmpty(result) Then
            Return "NULL"
        Else
            Return result.ToUpper
        End If
    End Function

    Public Function GetESSubmit(ay As String) As String
        Dim query As String = "Select convert(char, datTransaction, 106) As ShortDate FROM esSchema where strAirsYear = @ay "

        Dim param As New SqlParameter("@ay", ay)

        Dim result As String = DB.GetString(query, param).Trim()

        If String.IsNullOrEmpty(result) Then
            Return "NULL"
        Else
            Return result
        End If
    End Function

    Public Function GetDecDegree(dd As String, mm As String, ss As String) As Decimal
        Return Round(CDec(dd) + CDec(mm) / 60 + CDec(ss) / 3600, 6)
    End Function

    Public Function GetHorizCollDesc(code As String) As String
        Dim query As String = "Select strHorizCollectionMethodDesc " &
            "FROM EILookupHorizColMethod Where strHorizCollectionMethodCode = @code "
        Dim param As New SqlParameter("@code", code)

        Return DB.GetString(query, param)
    End Function

    Public Function GetHorizRefDesc(code As String) As String
        Dim query As String = "Select strHorizontalReferenceDesc " &
            "FROM EILookupHorizRefDatum Where strHorizontalReferenceDatum = @code "

        Dim desc As String = DB.GetString(query, New SqlParameter("@code", code))

        Return If(String.IsNullOrEmpty(desc), "", desc)
    End Function

    Public Function CheckESEntry(ay As String) As Boolean
        Dim query As String = "Select strFacilityAddress FROM esSchema where strAirsYear = @ay "
        Dim param As New SqlParameter("@ay", ay)
        Return DB.ValueExists(query, param)
    End Function

    Public Function CheckFirstConfirm(ByVal ay As String) As Boolean
        Dim query = "SELECT convert(bit,count(*)) FROM ESSCHEMA
            where STRDATEFIRSTCONFIRM is not null
            and STRAIRSYEAR = @ay"

        Dim param As New SqlParameter("@ay", ay)

        Return DB.GetBoolean(query, param)
    End Function

    Public Function GetEsYears(airs As ApbFacilityId) As DataTable

        Dim query = "Select intESYear FROM esSchema where strAirsNumber = @AirsNumber order by intESYear Desc"
        Dim param As New SqlParameter("@AirsNumber", airs.DbFormattedString)
        Return DB.GetDataTable(query, param)

    End Function

    Public Function GetConfirmNumber(airsYear As String) As String

        Dim query = "Select strConfirmationNbr FROM esSchema Where strAirsYear = @AirsYear "
        Dim param As New SqlParameter("@AirsYear", airsYear)

        Return DB.GetString(query, param)

    End Function

    Public Sub CreateConfNum(esYear As String, airs As ApbFacilityId)

        Dim day As String = Now.ToString("d-MMM-yyyy")
        Dim hr As String = Now.Hour.ToString
        Dim min As String = Now.Minute.ToString
        If hr.Length() < 2 Then hr = "0" & hr
        If min.Length() < 2 Then min = "0" & min
        Dim TransDate As String = day.ToUpper
        Dim TransTime As String = hr & min
        Dim newConfNum As String

        TransDate = TransDate.Replace("-", "")
        newConfNum = airs.ShortString & TransDate & TransTime

        Dim query = "Update esSchema Set strConfirmationNbr = @ConfNum Where intESYear = @esYear And strAirsNumber = @AirsNumber "

        Dim params As SqlParameter() = {
            New SqlParameter("@ConfNum", newConfNum),
            New SqlParameter("@esYear", esYear),
            New SqlParameter("@AirsNumber", airs.DbFormattedString)
        }

        DB.RunCommand(query, params)

    End Sub

    Public Function GetEmissionValue(emType As String, ay As String) As String

        Dim query As String

        If emType = "VOC" Then
            query = "Select dblVOCEmission FROM esSchema Where strAirsYear = @ay "
        Else
            query = "Select dblNOxEmission FROM esSchema Where strAirsYear = @ay "
        End If

        Dim param As New SqlParameter("@ay", ay)

        Return DB.GetSingleValue(Of Double)(query, param).ToString

    End Function

    Public Function GetHorizontalCollectionMethods() As DataTable

        Dim query = "Select strHorizCollectionMethodCode, strHorizCollectionMethodDesc " &
            "FROM eiLookupHorizColMethod order by strHorizCollectionMethodDesc"

        Return DB.GetDataTable(query)

    End Function

    Public Function GetHorizontalDatumReferenceCodes() As DataTable

        Dim query = "Select strHorizontalReferenceDatum, strHorizontalReferenceDesc " &
            "FROM eiLookupHorizRefDatum order by strHorizontalReferenceDesc"

        Return DB.GetDataTable(query)

    End Function

    Public Function GetEsStates() As DataTable

        Return DB.GetDataTable("Select Abbreviation FROM tblState order by Abbreviation")

    End Function

    Public Function GetFacilityEsSchema(airsYear As String) As DataRow

        Dim query = "Select strFacilityName, " &
            "strFacilityAddress, " &
            "strFacilityCity, " &
            "strFacilityZip, " &
            "strCounty, " &
            "strHorizontalCollectionCode, " &
            "strHorizontalAccuracyMeasure, " &
            "strHorizontalReferenceCode, " &
            "dblXCoordinate, " &
            "dblYCoordinate, " &
            "strContactPrefix, " &
            "strContactFirstName, " &
            "strContactLastName, " &
            "strContactTitle, " &
            "strContactCompany, " &
            "strContactPhoneNumber, " &
            "strContactFaxNumber, " &
            "strContactEmail, " &
            "strContactAddress1, " &
            "strContactCity, " &
            "strContactState, " &
            "strContactZip, " &
            "dblVOCEmission, " &
            "dblNOXEmission, " &
            "strOptOut " &
            "FROM esSchema Where strAirsYear = @AirsYear "

        Dim param As New SqlParameter("@AirsYear", airsYear)

        Return DB.GetDataRow(query, param)

    End Function

    Public Function GetEsContactInfo(airs As ApbFacilityId, year As Integer) As DataRow

        Dim query = "select dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.Prefix,
                               c.STRCONTACTPREFIX))          as strContactPrefix,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.FirstName,
                               c.STRCONTACTFIRSTNAME))       as strContactFirstName,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.LastName,
                               c.STRCONTACTLASTNAME))        as strContactLastName,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, left(m.Title, 50),
                               left(c.STRCONTACTTITLE, 50))) as strContactTitle,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.Organization,
                               c.STRCONTACTCOMPANYNAME))     as strContactCompanyName,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.Telephone,
                               c.STRCONTACTPHONENUMBER1))    as strContactPhoneNumber1,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, null,
                               c.STRCONTACTFAXNUMBER))       as strContactFaxNumber,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.Email,
                               left(c.STRCONTACTEMAIL, 50))) as strContactEmail,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.Address1,
                               c.STRCONTACTADDRESS1))        as strContactAddress1,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.City,
                               c.STRCONTACTCITY))            as strContactCity,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.State,
                               c.STRCONTACTSTATE))           as strContactState,
                   dbo.NullIfNaOrEmpty(
                           IIF(m.Id is not null, m.PostalCode,
                               c.STRCONTACTZIPCODE))         as strContactZipCode
            from dbo.ESSCHEMA e
                left join dbo.Geco_MailContact m
                on e.STRAIRSNUMBER = m.FacilityId
                    and m.Category = 'ES'
                    and m.Confirmed = 1
                left join dbo.APBCONTACTINFORMATION c
                on e.STRAIRSNUMBER = c.STRAIRSNUMBER
                    and c.STRKEY = '42'
            where e.INTESYEAR = @year
              and e.STRAIRSNUMBER = @airs"

        Dim params As SqlParameter() = {
            New SqlParameter("@year", year),
            New SqlParameter("@airs", airs.DbFormattedString)
        }

        Return DB.GetDataRow(query, params)
    End Function

End Module
