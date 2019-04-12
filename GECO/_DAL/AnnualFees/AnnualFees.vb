Imports System.Data.SqlClient
Imports GECO.GecoModels

Module AnnualFees

    Public Sub SaveTempFacInfo(airs As ApbFacilityId, FacName As String, FacStr As String, FacCity As String)
        Dim query As String = "select convert(bit, count(*)) FROM APBFACILITYINFOTEMP where STRAIRSNUMBER = @Airs"

        Dim param As New SqlParameter("@Airs", airs.DbFormattedString)

        If DB.GetBoolean(query, param) Then
            query = "Update apbfacilityinfotemp set " &
                "strfacilityname = @FacName, " &
                "strfacilitystreet1 = @FacStr, " &
                "strfacilitycity = @FacCity " &
                "where strairsnumber = @Airs"
        Else
            query = "Insert into apbfacilityinfotemp( " &
                "strairsnumber, strfacilityname, strfacilitystreet1, strfacilitycity) " &
                "values(@Airs , @FacName, @FacStr, @FacCity)"
        End If

        Dim param2 As SqlParameter() = {
            New SqlParameter("@FacName", FacName),
            New SqlParameter("@FacStr", FacStr),
            New SqlParameter("@FacCity", FacCity),
            param
        }

        DB.RunCommand(query, param2)
    End Sub

    Public Sub RemoveTempFacInfo(airs As ApbFacilityId)
        Dim SQL As String = "Delete FROM apbfacilityinfotemp where strairsnumber = @airs"
        Dim param As SqlParameter = New SqlParameter("@airs", airs.DbFormattedString)
        DB.RunCommand(SQL, param)
    End Sub

End Module
