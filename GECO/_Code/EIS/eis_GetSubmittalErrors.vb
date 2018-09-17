Imports System.Data
Imports System.Data.SqlClient

Public Module eis_GetSubmittalErrors
    Public Function GetEISSubmittalErrors(facilitySiteID As String, year As Integer) As DataSet
        Dim spName = "geco.EIS_Submittal_Errors"

        Dim params As SqlParameter() = {
            New SqlParameter("@FacilitySiteID", facilitySiteID),
            New SqlParameter("@year", year)
        }

        Return DB.SPGetDataSet(spName, params)
    End Function

End Module
