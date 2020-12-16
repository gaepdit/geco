Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_ReportingPeriodPollutants
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

        If Not IsPostBack Then
            LoadYears()
        End If

        LoadDetails()
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
        Else
            Years.Items.Add("No Data")
            YearButton.Visible = False
        End If
    End Sub

    Private Sub LoadDetails()
        Dim query = "SELECT FacilitySiteID, INTINVENTORYYEAR, EMISSIONSUNITID, STRUNITDESCRIPTION, PROCESSID, STRPROCESSDESCRIPTION,
                   STRPOLLUTANT, IIF(RPTPeriodTypeCode = 'O3D', 'Summer Day', 'Annual') AS RPTPeriodType,
                   IIF(RPTPeriodTypeCode = 'O3D', 'TPD *', 'TPY')                       AS PollutantUnit,
                   FLTTOTALEMISSIONS, FLTEMISSIONFACTOR, STREMCALCMETHOD, EFUNITS, EFNUMDESC, EFDENDESC, STREMISSIONFACTORTEXT,
                   STREMISSIONSCOMMENT, UPDATEUSER, UPDATEDATETIME, LASTEISSUBMITDATE
            FROM VW_EIS_RPEMISSIONS
            where FacilitySiteID = @FacilitySiteID
              and intInventoryYear = @EIYear
            order by EMISSIONSUNITID, PROCESSID, STRPOLLUTANT"

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
        ExportAsExcel($"{CurrentAirs.ShortString}_{Years.SelectedValue}_Reporting_Period_Pollutants", ReportingPeriod)
    End Sub

End Class
