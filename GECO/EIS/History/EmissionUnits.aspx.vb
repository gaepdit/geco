Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_EmissionUnits
    Inherits Page

    Private Property FacilitySiteID As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(FacilitySiteID) Then
            Response.Redirect("~/")
        End If

        FacilitySiteID = New ApbFacilityId(FacilitySiteID)
        Master.CurrentAirs = FacilitySiteID
        Master.IsFacilitySet = True

        LoadEmissionUnits()
    End Sub


    Private Sub LoadEmissionUnits()
        Dim query = "select u.EmissionsUnitID, u.strUnitDescription,
                   lu.STRDESC as strUnitType, u.fltUnitDesignCapacity,
                   u.STRUNITDESIGNCAPACITYUOMCODE, u.NUMMAXIMUMNAMEPLATECAPACITY,
                   ls.strDesc as strUnitStatusCode, u.DATUNITOPERATIONDATE,
                   STRUNITCOMMENT, LASTEISSUBMITDATE
            from EIS_EMISSIONSUNIT u
                left join EISLK_UNITTYPECODE lu
                on u.STRUNITTYPECODE = lu.UNITTYPECODE
                    and lu.ACTIVE = '1'
                left join EISLK_UNITSTATUSCODE ls
                on u.STRUNITSTATUSCODE = ls.UNITSTATUSCODE
                    and ls.ACTIVE = '1'
            where FacilitySiteID = @FacilitySiteID
              and u.Active = '1'"

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID.ShortString)
        Dim dt As DataTable = DB.GetDataTable(query, param)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            EmissionUnits.DataSource = dt
            EmissionUnits.DataBind()
        Else
            EmissionUnitsEmptyNotice.Visible = True
            EmissionUnits.Visible = False
            EmissionUnitsExport.Visible = False
        End If
    End Sub

    Private Sub EmissionUnitsExport_Click(sender As Object, e As EventArgs) Handles EmissionUnitsExport.Click
        ExportAsExcel($"{FacilitySiteID.ShortString}_Emission_Units", EmissionUnits)
    End Sub

End Class
