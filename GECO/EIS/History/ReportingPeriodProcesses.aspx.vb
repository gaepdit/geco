Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_ReportingPeriodProcesses
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
        Dim query = "SELECT INTINVENTORYYEAR          as [Inventory Year],
               EMISSIONSUNITID           as [Emissions Unit ID],
               STRUNITDESCRIPTION        as [Emissions Unit Desc],
               PROCESSID                 as [Process ID],
               STRPROCESSDESCRIPTION     as [Process Desc],
               FLTCALCPARAMETERVALUE     as [Calculation Parameter Value],
               STRCPUOMDESC              as [Calculation Parameter Unit of Measure],
               STRCPTYPEDESC             as [Calculation Parameter Type Desc],
               STRMATERIAL               as [Material Desc],
               STRREPORTINGPERIODCOMMENT as [Reporting Period Comment],
               INTACTUALHOURSPERPERIOD   as [Actual Hours Per Period],
               NUMAVERAGEDAYSPERWEEK     as [Average Days Per Week],
               NUMAVERAGEHOURSPERDAY     as [Average Hours per Day],
               NUMAVERAGEWEEKSPERPERIOD  as [Average Weeks per Period],
               NUMPERCENTWINTERACTIVITY  as [Winter Activity %],
               NUMPERCENTSPRINGACTIVITY  as [Spring Activity %],
               NUMPERCENTSUMMERACTIVITY  as [Summer Activity %],
               NUMPERCENTFALLACTIVITY    as [Fall Activity %],
               HEATCONTENT               as [Heat Content],
               IIF(HCNUMER = 'E6BTU', 'MILLION BTU', HCNUMER)
                                         as [Heat Content Numerator],
               c.STRDESC                 as [Heat Content Denominator],
               ASHCONTENT                as [Ash Content],
               SULFURCONTENT             as [Sulfur Content]
        FROM VW_EIS_RPDETAILS d
            left join EISLK_SCPDENOMUOMCODE c
            on d.HCDENOM = c.SCPDENOMUOMCODE
        where FacilitySiteID = @FacilitySiteID
          and intInventoryYear = @EIYear
          and d.EmissionUnitActive = '1'
          and d.ProcessActive = '1'
        order by EMISSIONSUNITID, PROCESSID"

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
        ExportAsExcel($"{CurrentAirs.ShortString}_{Years.SelectedValue}_Reporting_Period_Processes", ReportingPeriod)
    End Sub

End Class
