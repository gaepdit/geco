Imports System.Data.SqlClient

Public Module eis_releasePoints

    Public Function ReleasePointExists(fsid As String, rpid As String) As Boolean
        Dim query As String = "select convert(bit, count(*)) " &
            " From EIS_RELEASEPOINT " &
            " where FACILITYSITEID = @fsid " &
            " and RELEASEPOINTID = @rpid "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@rpid", rpid)
        }

        Return DB.GetBoolean(query, params)
    End Function

    Public Function GetRPApportionmentTotal(ByVal fsid As String, ByVal euid As String, ByVal prid As String) As Integer
        Dim query As String = "select sum(intAveragePercentEmissions) As RPApportionmentTotal FROM eis_RPApportionment " &
            "where " &
            "FacilitySiteID = @fsid and " &
            "EmissionsUnitID = @euid and " &
            "ProcessID = @prid and " &
            "Active = '1'"

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid),
            New SqlParameter("@prid", prid)
        }

        Return DB.GetInteger(query, params)
    End Function

End Module