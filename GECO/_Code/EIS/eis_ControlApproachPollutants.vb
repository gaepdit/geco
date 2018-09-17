Imports System.Data.SqlClient

Public Module eis_ControlApproachPollutants

    Public Function InsertUpdateProcessControlPollutant(facilitySiteID As String,
                                                        emissionsUnitID As String,
                                                        pollutantCode As String,
                                                        processID As String,
                                                        reductionEfficiency As Decimal,
                                                        updateUser As String,
                                                        mode As DbChangeMode) As Integer

        Dim spName = "geco.EIS_Process_Control_Pollutant"

        Dim Params = {
            New SqlParameter("@FacilitySiteID", facilitySiteID),
            New SqlParameter("@EmissionUnitID", emissionsUnitID),
            New SqlParameter("@PollutantCode", pollutantCode),
            New SqlParameter("@ProcessID", processID),
            New SqlParameter("@ReductionEfficiency", reductionEfficiency),
            New SqlParameter("@UpdateUser", updateUser),
            New SqlParameter("@mode", mode)
        }

        Return DB.SPReturnValue(spName, Params)
    End Function

    Public Function InsertUpdateEUControlPollutant(facilitySiteID As String,
                                                   emissionsUnitID As String,
                                                   pollutantCode As String,
                                                   reductionEfficiency As Decimal,
                                                   updateUser As String,
                                                   mode As DbChangeMode) As Integer

        Dim spName = "geco.EIS_EU_Control_Pollutant"

        Dim Params = {
            New SqlParameter("@FacilitySiteID", facilitySiteID),
            New SqlParameter("@EmissionUnitID", emissionsUnitID),
            New SqlParameter("@PollutantCode", pollutantCode),
            New SqlParameter("@ReductionEfficiency", reductionEfficiency),
            New SqlParameter("@UpdateUser", updateUser),
            New SqlParameter("@mode", mode)
        }

        Return DB.SPReturnValue(spName, Params)
    End Function

    Public Enum DbChangeMode
        Insert = 1
        Update = 2
    End Enum

End Module
