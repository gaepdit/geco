Imports System.Data.SqlClient
Imports GECO.GecoModels
Imports GECO.GecoModels.ApbFacilityId

Public Module FacilityInfo

    ''' <summary>
    ''' Returns whether an AIRS number already exists in the database
    ''' </summary>
    ''' <param name="airsNumber">The AIRS number to test.</param>
    ''' <returns>True if the AIRS number exists; otherwise false.</returns>
    ''' <remarks>Looks for value in APBMASTERAIRS table. Does not make any judgments about state of facility otherwise.</remarks>
    Public Function AirsNumberExists(airsNumber As ApbFacilityId) As Boolean
        NotNull(airsNumber, NameOf(airsNumber))

        Dim spName As String = "iaip_facility.AirsNumberExists"
        Dim parameter As New SqlParameter("@AirsNumber", airsNumber.DbFormattedString)
        Return DB.SPGetBoolean(spName, parameter)
    End Function

    Public Function GetFacilityName(airs As ApbFacilityId) As String
        NotNull(airs, NameOf(airs))

        Dim query As String = "Select strFacilityName " &
            " FROM dbo.APBFacilityInformation " &
            " Where strAirsNumber = @airs "

        Dim param As New SqlParameter("@airs", airs.DbFormattedString)

        Return DB.GetString(query, param)
    End Function

    Public Function GetFacilityNameAndCity(airs As ApbFacilityId) As String
        NotNull(airs, NameOf(airs))

        Dim query As String = "select concat(STRFACILITYNAME, ', ', STRFACILITYCITY)
            from dbo.APBFACILITYINFORMATION
            where STRAIRSNUMBER = @airs"

        Return DB.GetString(query, New SqlParameter("@airs", airs.DbFormattedString))
    End Function

    Public Function CheckSummerDayRequired(airs As ApbFacilityId) As Boolean
        NotNull(airs, NameOf(airs))

        Select Case airs.CountySubstring
            Case "015", "057", "063", "067", "077", "089", "097", "113", "117", "121", "135", "151", "217", "223", "247"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Function CheckFacilityEmissionStatement(airs As ApbFacilityId) As Boolean
        NotNull(airs, NameOf(airs))

        Select Case airs.CountySubstring
            Case "013", "015", "045", "057", "063", "067", "077", "089", "097", "113", "117", "121", "135", "139", "151", "217", "223", "247", "255", "297"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Function GetFacilityAdminUsers(airs As ApbFacilityId) As DataTable
        NotNull(airs, NameOf(airs))

        Dim query As String = "SELECT l.STRUSEREMAIL as email
            FROM dbo.OLAPUSERACCESS a
                 INNER JOIN dbo.OLAPUSERLOGIN l
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
                from dbo.APBFACILITYINFORMATION
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
