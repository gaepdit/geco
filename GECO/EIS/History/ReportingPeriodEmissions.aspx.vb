﻿Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_ReportingPeriodEmissions
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
        Master.IsFacilitySet = True

        If Not IsPostBack Then
            ShowFacilityInfo()
            LoadYears()
        End If

        LoadDetails()
    End Sub

    Private Sub ShowFacilityInfo()
        Dim currentFacility As String = GetFacilityName(CurrentAirs) & ", " & GetFacilityCity(CurrentAirs)
        lblFacilityDisplay.Text = currentFacility
        lblAIRS.Text = CurrentAirs.FormattedString
    End Sub

    Private Sub LoadYears()
        Dim query = "select distinct INTINVENTORYYEAR
            FROM VW_EIS_RPEMISSIONS
            where FACILITYSITEID = @FacilitySiteID
            order by INTINVENTORYYEAR desc"

        Dim param As New SqlParameter("@FacilitySiteID", CurrentAirs.ShortString)

        Dim dt As DataTable = DB.GetDataTable(query, param)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                Years.Items.Add(dr("INTINVENTORYYEAR").ToString)
            Next
            Years.SelectedIndex = 0
        Else
            Years.Items.Add("No Data")
            YearButton.Visible = False
        End If
    End Sub

    Private Sub LoadDetails()
        Dim query = "SELECT STRPOLLUTANT as [Pollutant],
                   convert(decimal(10, 2), sum(FLTTOTALEMISSIONS)) as [Emissions (tons)]
            FROM VW_EIS_RPEMISSIONS
            where FacilitySiteID = @FacilitySiteID
              and INTINVENTORYYEAR = @EIYear
              and RPTPERIODTYPECODE = 'A'
            GROUP BY STRPOLLUTANT"

        Dim params As SqlParameter() = {
            New SqlParameter("@FacilitySiteID", CurrentAirs.ShortString),
            New SqlParameter("@EIYear", Integer.Parse(Years.SelectedValue))
        }

        Dim dt As DataTable = DB.GetDataTable(query, params)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            ReportingPeriodEmptyNotice.Visible = False
            ReportingPeriod.Visible = True
            ReportingPeriodExport.Visible = True
            ReportingPeriod.DataSource = dt
            ReportingPeriod.DataBind()
        Else
            ReportingPeriodEmptyNotice.Visible = True
            ReportingPeriod.Visible = False
            ReportingPeriodExport.Visible = False
        End If
    End Sub

    Private Sub ReportingPeriodExport_Click(sender As Object, e As EventArgs) Handles ReportingPeriodExport.Click
        ExportAsExcel($"{CurrentAirs.ShortString}_{Years.SelectedValue}_Reporting_Period_Emissions", ReportingPeriod)
    End Sub

End Class
