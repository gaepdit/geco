Imports System.Data.SqlClient
Imports System.Math

Public Module ES_MiscCode

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

        Dim result As String = DB.GetString(query, param)

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

End Module