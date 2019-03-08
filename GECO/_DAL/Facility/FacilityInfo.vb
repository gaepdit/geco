Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Module FacilityInfo

    Public Function GetFacilityName(airs As String) As String
        If airs Is Nothing OrElse Not ApbFacilityId.IsValidAirsNumberFormat(airs) Then
            Return Nothing
        End If

        Return GetFacilityName(New ApbFacilityId(airs))
    End Function

    Public Function GetFacilityName(airs As ApbFacilityId) As String
        Dim query As String = "Select strFacilityName " &
            " FROM APBFacilityInformation " &
            " Where strAirsNumber = @airs "

        Dim param As New SqlParameter("@airs", airs.DbFormattedString)

        Return DB.GetString(query, param)
    End Function

    Public Function GetFacilityCity(airs As ApbFacilityId) As String
        Dim query As String = "Select STRFACILITYCITY" &
            " FROM APBFacilityInformation " &
            " Where strAirsNumber = @airs "

        Dim param As New SqlParameter("@airs", airs.DbFormattedString)

        Return DB.GetString(query, param)
    End Function

    Public Function CheckSummerDayRequired(airsNumber As String) As Boolean
        If airsNumber.Length <> 8 Then
            Throw New FormatException("AIRS Number must be eight characters long.")
        End If

        Select Case Left(airsNumber, 3)
            Case "015", "057", "063", "067", "077", "089", "097", "113", "117", "121", "135", "151", "217", "223", "247"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Function CheckFacilityEmissionStatement(airs As ApbFacilityId) As Boolean
        Select Case airs.CountySubstring
            Case "013", "015", "045", "057", "063", "067", "077", "089", "097", "113", "117", "121", "135", "139", "151", "217", "223", "247", "255", "297"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Function GetFacilityLatitude(fsid As String) As Decimal
        Dim query As String = "select numLatitudeMeasure FROM eis_FacilityGeoCoord where FacilitySiteID = @fsid "

        Dim param As New SqlParameter("@fsid", fsid)

        Return DB.GetSingleValue(Of Decimal)(query, param)
    End Function

    Public Function GetFacilityLongitude(fsid As String) As Decimal
        Dim query As String = "select numLongitudeMeasure FROM eis_FacilityGeoCoord where FacilitySiteID = @fsid "

        Dim param As New SqlParameter("@fsid", fsid)

        Return DB.GetSingleValue(Of Decimal)(query, param)
    End Function

    Public Function GetFacilityAdminUsers(airs As ApbFacilityId) As DataTable
        Dim query As String = "SELECT l.STRUSEREMAIL as email
            FROM OLAPUSERACCESS a
                 INNER JOIN OLAPUSERLOGIN l
                            ON a.NUMUSERID = l.NUMUSERID
            WHERE a.STRAIRSNUMBER = @airs
              AND a.INTADMINACCESS = 1"

        Dim param As New SqlParameter("@airs", airs.DbFormattedString)

        Return DB.GetDataTable(query, param)
    End Function

    Public Function GetCachedFacilityTable() As DataTable
        Dim dt As DataTable

        If HttpContext.Current.Cache("RequestAccess") Is Nothing Then
            Dim query = "select right(STRAIRSNUMBER, 8) as airsnumber,
                STRFACILITYNAME as facilityname
                from APBFACILITYINFORMATION
                order by STRAIRSNUMBER"

            dt = DB.GetDataTable(query)
            dt.TableName = "facilityInfo"
            HttpContext.Current.Cache.Insert("RequestAccess", dt, Nothing)
        Else
            dt = DirectCast(HttpContext.Current.Cache("RequestAccess"), DataTable)
        End If

        Return dt
    End Function

End Module