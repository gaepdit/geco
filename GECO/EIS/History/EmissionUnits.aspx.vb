﻿Imports Microsoft.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_EmissionUnits
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.History

        LoadDetails()
    End Sub


    Private Sub LoadDetails()
        Dim query = "select u.EmissionsUnitID              as [Emission Unit ID],
               u.strUnitDescription           as [Description],
               lu.STRDESC                     as [Unit Type],
               u.fltUnitDesignCapacity        as [Design Capacity],
               u.STRUNITDESIGNCAPACITYUOMCODE as [Design Capacity Units],
               u.NUMMAXIMUMNAMEPLATECAPACITY  as [Max Nameplate Capacity (MW)],
               u.DATUNITOPERATIONDATE         as [Placed in Operation],
               ls.strDesc                     as [Operating Status],
               LASTEISSUBMITDATE              as [Last EPA Submittal],
               STRUNITCOMMENT                 as [Comment]
        from EIS_EMISSIONSUNIT u
            left join EISLK_UNITTYPECODE lu
            on u.STRUNITTYPECODE = lu.UNITTYPECODE
                and lu.ACTIVE = '1'
            left join EISLK_UNITSTATUSCODE ls
            on u.STRUNITSTATUSCODE = ls.UNITSTATUSCODE
                and ls.ACTIVE = '1'
        where FacilitySiteID = @FacilitySiteID
          and u.Active = '1'"

        Dim param As New SqlParameter("@FacilitySiteID", CurrentAirs.ShortString)
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
        ExportAsExcel($"{CurrentAirs.ShortString}_Emission_Units", EmissionUnits)
    End Sub

End Class
