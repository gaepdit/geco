Imports System.Data.SqlClient

Public Module eis_getcodedescriptions

    Function GetHorCollMetDesc(ByVal HorCollMetCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM eislk_HorCollMetCode " &
            " Where HorCollMetCode = @HorCollMetCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@HorCollMetCode", HorCollMetCode)

        Return DB.GetString(query, param)
    End Function

    Function GetHorRefDatumDesc(ByVal HorRefDatumCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM eislk_HorRefDatumCode " &
            " Where HorRefDatumCode = @HorRefDatumCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@HorRefDatumCode", HorRefDatumCode)

        Return DB.GetString(query, param)
    End Function

    Function GetUnitStatusCodeDesc(ByVal UnitStatusCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM eislk_UnitStatusCode " &
            " Where UnitStatusCode = @UnitStatusCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@UnitStatusCode", UnitStatusCode)

        Return DB.GetString(query, param)
    End Function

    Function GetFacilityStatusCodeDesc(ByVal FacilityStatusCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM EISLK_FACILITYSITESTATUSCODE " &
            " Where FACILITYSITESTATUSCODE = @FacilityStatusCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@FacilityStatusCode", FacilityStatusCode)

        Return DB.GetString(query, param)
    End Function

    Function GetStackStatusCodeDesc(ByVal StackStatusCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM EISLK_RPSTATUSCODE " &
            " Where RPSTATUSCODE = @StackStatusCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@StackStatusCode", StackStatusCode)

        Return DB.GetString(query, param)
    End Function

    Function GetUnitTypeCodeDesc(ByVal UnitTypeCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM eislk_UnitTypeCode " &
            " Where UnitTypeCode = @UnitTypeCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@UnitTypeCode", UnitTypeCode)

        Return DB.GetString(query, param)
    End Function

    Function GetUnitDesCapUOMCodeDesc(ByVal UnitDesCapUOMCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM eislk_UnitDesCapacityUOMCode " &
            " Where UnitDesignCapacityUOMCode = @UnitDesCapUOMCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@UnitDesCapUOMCode", UnitDesCapUOMCode)

        Return DB.GetString(query, param)
    End Function

    Function GetStackTypeCodeDesc(ByVal StackTypeCode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM EISLK_RPTYPECODE " &
            " Where RPTYPECODE = @StackTypeCode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@StackTypeCode", StackTypeCode)

        Return DB.GetString(query, param)
    End Function

    Function GetEISStatusMessage(ByVal statuscode As String) As String
        Dim query As String = "Select strDesc " &
            " FROM eislk_EISStatusCode " &
            " Where EISStatusCode = @statuscode " &
            " and Active = '1' "

        Dim param As New SqlParameter("@statuscode", statuscode)

        Return DB.GetString(query, param)
    End Function

    Function GetOptOutReason(ByVal fsid As String, ByVal eiyr As Integer) As String
        Dim query As String = "Select stroptoutReason " &
            " FROM eis_Admin " &
            " Where FacilitySiteID = @fsid " &
            " and InventoryYear = @eiyr "

        Dim params As SqlParameter() = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@eiyr", eiyr)
        }

        Dim result = DB.GetString(query, params)

        Select Case result
            Case "1"
                Return "Facility did not operate"
            Case "2"
                Return "Facility emissions below thresholds"
            Case Else
                Return String.Empty
        End Select
    End Function

End Module