Imports System.Data.SqlClient

Public Module eis_getequipdescriptions

    Function GetEmissionUnitDesc(ByVal fsid As String, ByVal euid As String) As String
        Dim query As String = "Select strUnitDescription " &
            " FROM EIS_EmissionsUnit " &
            " Where FacilitySiteID = @fsid " &
            " and EmissionsUnitID = @euid " &
            " and Active = '1'"

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        Return DB.GetString(query, params)
    End Function

End Module