Imports System.Data
Imports System.Data.SqlClient

Partial Class EIS_report_fw_emsummary
    Inherits Page

    Public FacilitySiteID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If Not IsPostBack Then
            HideFacilityInventoryMenu()
            HideEmissionInventoryMenu()
            HideSubmitMenu()

            txtFacilityName_fwemSummary.Text = GetFacilityName(FacilitySiteID)
            txtFacilitySiteID_fwemSummary.Text = FacilitySiteID
            LoadYear()
        End If

    End Sub

#Region "Load Routines"

    Private Sub LoadYear()
        'Load Years dropdown box

        Try
            ddlInventoryYear_fwemSummary.Items.Add("-Select Year-")

            Dim query = "Select distinct intInventoryYear FROM VW_EIS_RPEmissions " &
                "where FacilitySiteID = @FacilitySiteID order by intInventoryYear desc"

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dt As DataTable = DB.GetDataTable(query, param)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    ddlInventoryYear_fwemSummary.Items.Add(dr("intInventoryYear").ToString)
                Next
            Else
                ddlInventoryYear_fwemSummary.Items.Add("No Data")
            End If

            ddlInventoryYear_fwemSummary.SelectedIndex = 0

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

#Region "Grid View Routines"

    Protected Sub btnGO_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGO_fwemSummary.Click

        Dim gvwEmissionsSummarySize As Integer

        loadEmissionsSummarygvw()
        gvwEmissionsSummarySize = gvwEmissionsSummary.Rows.Count

        If gvwEmissionsSummarySize <= 0 Or ddlInventoryYear_fwemSummary.SelectedValue = "No Data" Then
            lblEmptygvwEmissionsSummary.Text = "No data exists for the year selected or the facility has no data in the EI"
            lblEmptygvwEmissionsSummary.Visible = True
            lblFWSummary.Text = ""
            lblFWSummary.Visible = False
            btnExport_fwemSummary.Visible = False
        Else
            lblEmptygvwEmissionsSummary.Text = ""
            lblEmptygvwEmissionsSummary.Visible = False
            lblFWSummary.Text = "Emissions shown are ANNUAL emissions quantities."
            lblFWSummary.Visible = True
            btnExport_fwemSummary.Visible = True
        End If

    End Sub

    Private Sub loadEmissionsSummarygvw()

        Dim EIYear As Integer
        Dim dt As New DataTable

        If Integer.TryParse(ddlInventoryYear_fwemSummary.SelectedValue, EIYear) Then
            Dim query = "SELECT " &
            " vw_eis_RPEmissions.strPollutant as strPollutant, " &
            " sum(vw_eis_RPEmissions.fltTotalEmissions) as fltTotalEmissions " &
            " FROM VW_EIS_RPEmissions " &
            " where " &
            " vw_eis_RPEmissions.FacilitySiteID = @FacilitySiteID and " &
            " intInventoryYear = @EIYear and " &
            " RPTPeriodTypeCode = 'A' " &
            " GROUP BY vw_eis_RPEmissions.strPollutant, " &
            " vw_eis_RPEmissions.intInventoryYear, " &
            " vw_eis_RPEmissions.FacilitySiteID"

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EIYear", EIYear)
            }

            dt = DB.GetDataTable(query, params)
        End If

        gvwEmissionsSummary.DataSource = dt
        gvwEmissionsSummary.DataBind()

    End Sub

#End Region

    Protected Sub btnReportsHome_fwemSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReportsHome_fwemSummary.Click

        Response.Redirect("reports.aspx")

    End Sub

    Protected Sub btnExport_fwemSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport_fwemSummary.Click

        Dim EIYear As String = ddlInventoryYear_fwemSummary.SelectedValue
        loadEmissionsSummarygvw()
        ExportAsExcel("EISFacWideEmisSummary_" & EIYear, gvwEmissionsSummary)

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