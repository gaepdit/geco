Imports Microsoft.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_ReportingPeriodPollutants
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            CompleteRedirect("~/")
            Return
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.History

        If Not IsPostBack Then
            If LoadYears() Then
                LoadDetails()
            Else
                dNoDataExists.Visible = True
                dDataExists.Visible = False
            End If
        Else
            LoadDetails()
        End If
    End Sub

    Private Function LoadYears() As Boolean
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
            Return True
        End If

        Years.Items.Add("No Data")
        Return False
    End Function

    Private Sub LoadDetails()
        Dim query = "SELECT INTINVENTORYYEAR                 as [Inventory Year],
               EMISSIONSUNITID                  as [Emissions Unit ID],
               STRUNITDESCRIPTION               as [Emissions Unit Desc],
               PROCESSID                        as [Process ID],
               STRPROCESSDESCRIPTION            as [Process Desc],
               STRPOLLUTANT                     as [Pollutant],
               IIF(RPTPeriodTypeCode = 'O3D',
                   'Summer Day',
                   'Annual')                    as [Pollutant Period],
               IIF(RPTPeriodTypeCode = 'O3D',
                   'TPD *',
                   'TPY')                       as [Total Emissions Units],
               FLTTOTALEMISSIONS                as [Total Emissions],
               FLTEMISSIONFACTOR                as [Emission Factor],
               EFUNITS                          as [Emissions Factor Units],
               STREMCALCMETHOD                  as [Emission Calculation Method],
               EFNUMDESC                        as [Emission Factor Numerator],
               EFDENDESC                        as [Emission Factor Denominator],
               STREMISSIONFACTORTEXT            as [Emission Factor Text],
               STREMISSIONSCOMMENT              as [Emissions Comment],
               convert(date, LASTEISSUBMITDATE) as [Last EPA Submittal]
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
