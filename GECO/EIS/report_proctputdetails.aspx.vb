Imports System.Data.SqlClient

Partial Class EIS_report_proctputdetails
    Inherits Page

    Private FacilitySiteID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If Not IsPostBack Then
            HideFacilityInventoryMenu()
            HideEmissionInventoryMenu()
            HideSubmitMenu()

            LoadYear()
            txtFacilitySiteID_Process.Text = FacilitySiteID
            txtFacilityName_Process.Text = GetFacilityName(FacilitySiteID)
        End If

    End Sub

    Private Sub LoadYear()
        'Load Years dropdown box

        Try
            ddlInventoryYear_Process.Items.Add("-Select Year-")

            Dim query = "Select distinct intInventoryYear FROM VW_EIS_RPEmissions " &
                    "where FacilitySiteID = @FacilitySiteID order by intInventoryYear desc"

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dt As DataTable = DB.GetDataTable(query, param)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    ddlInventoryYear_Process.Items.Add(dr("intInventoryYear").ToString)
                Next
            Else
                ddlInventoryYear_Process.Items.Add("No Data")
            End If

            ddlInventoryYear_Process.SelectedIndex = 0

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub loadReportgvw()

        Dim EIYear As Integer
        Dim dt As New DataTable

        If Integer.TryParse(ddlInventoryYear_Process.Text, EIYear) Then

            Dim query = "SELECT " &
            "FacilitySiteID, " &
            "INTINVENTORYYEAR, " &
            "EMISSIONSUNITID, " &
            "STRUNITDESCRIPTION, " &
            "PROCESSID, " &
            "STRPROCESSDESCRIPTION, " &
            "FLTCALCPARAMETERVALUE, " &
            "STRCPUOMDESC, " &
            "STRCPTYPEDESC, " &
            "STRMATERIAL, " &
            "STRREPORTINGPERIODCOMMENT, " &
            "INTACTUALHOURSPERPERIOD, " &
            "NUMAVERAGEDAYSPERWEEK, " &
            "NUMAVERAGEHOURSPERDAY, " &
            "NUMAVERAGEWEEKSPERPERIOD, " &
            "NUMPERCENTWINTERACTIVITY, " &
            "NUMPERCENTSPRINGACTIVITY, " &
            "NUMPERCENTSUMMERACTIVITY, " &
            "NUMPERCENTFALLACTIVITY, " &
            "HEATCONTENT, " &
            "     case " &
            "     when HCNUMER = 'E6BTU' " &
            "         then 'MILLION BTU' " &
            "     else HCNUMER " &
            "     end                                   as HCNUMER, " &
            "     c.STRDESC                             as HCDenom, " &
            "ASHCONTENT, " &
            "SULFURCONTENT " &
            " FROM VW_EIS_RPDETAILS d " &
            "     left join EISLK_SCPDENOMUOMCODE c " &
            "         on d.HCDENOM = c.SCPDENOMUOMCODE " &
            "where FacilitySiteID = @FacilitySiteID " &
            "and intInventoryYear = @EIYear " &
            "and d.EmissionUnitActive = '1' " &
            "and d.ProcessActive = '1' " &
            "order by EMISSIONSUNITID, PROCESSID"

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EIYear", EIYear)
            }

            dt = DB.GetDataTable(query, params)
        End If

        gvwProcessDetails.DataSource = dt
        gvwProcessDetails.DataBind()

    End Sub

    Protected Sub btnExportProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportProcess.Click

        Dim EIYear As String = ddlInventoryYear_Process.SelectedValue
        loadReportgvw()
        ExportAsExcel("EISProcessDetails_" & EIYear, gvwProcessDetails)

    End Sub

    Protected Sub btnReportsHome_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReportsHome.Click
        Response.Redirect("reports.aspx")
    End Sub

    Protected Sub btnGo_Process_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGo_Process.Click
        Dim gvwEmissionsSummarySize As Integer

        loadReportgvw()
        gvwEmissionsSummarySize = gvwProcessDetails.Rows.Count

        If gvwEmissionsSummarySize <= 0 Or ddlInventoryYear_Process.SelectedValue = "No Data" Then
            lblEmptygvwReportDetails.Text = "No data exists for the year selected or the facility has no data in the EI"
            lblEmptygvwReportDetails.Visible = True
            btnExportProcess.Visible = False
        Else
            lblEmptygvwReportDetails.Text = ""
            lblEmptygvwReportDetails.Visible = False
            btnExportProcess.Visible = True
        End If

    End Sub

#Region "  Menu Routines  "

    Private Sub HideFacilityInventoryMenu()

        Dim menu = CType(Master.FindControl("pnlFacilityInventory"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

    Private Sub HideEmissionInventoryMenu()

        Dim menu = CType(Master.FindControl("pnlEmissionInventory"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

    Private Sub HideSubmitMenu()

        Dim menu = CType(Master.FindControl("pnlSubmit"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

        menu = CType(Master.FindControl("pnlReset"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

#End Region

End Class