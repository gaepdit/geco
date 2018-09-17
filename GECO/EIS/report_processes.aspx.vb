Imports System.Data.SqlClient

Partial Class EIS_report_processes
    Inherits Page

    Public FacilitySiteID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim gvwProcessesSize As Integer

        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If Not IsPostBack Then
            HideFacilityInventoryMenu()
            HideEmissionInventoryMenu()
            HideSubmitMenu()

            txtFacilityName_Processes.Text = GetFacilityName(FacilitySiteID)
            txtFacilitySiteID_Processes.Text = FacilitySiteID
            loadProcessSummarygvw()
            gvwProcessesSize = gvwProcesses.Rows.Count

            If gvwProcessesSize <= 0 Then
                lblEmptygvwProcesses.Text = "No emission units exist for this facility in the EIS"
                lblEmptygvwProcesses.Visible = True
                btnExport_Processes.Visible = False
            Else
                lblEmptygvwProcesses.Text = ""
                lblEmptygvwProcesses.Visible = False
                btnExport_Processes.Visible = True
            End If

        End If

    End Sub

    Private Sub loadProcessSummarygvw()

        Dim query = "SELECT eis_Process.EmissionsUnitID " &
            ", eislk_UnitStatusCode.strDesc AS strUnitDesc " &
            ", eis_Process.ProcessID " &
            ", eis_Process.strProcessDescription " &
            ", eis_Process.SourceClassCode " &
            ", CONCAT(strDesc1, '; ', strDesc2, '; ', strDesc3, '; ', strDesc4) AS strSCCDesc " &
            ", eis_Process.strProcessComment " &
            ", eis_Process.LastEISSubmitDate " &
            "FROM EIS_Process " &
            "LEFT JOIN EISLK_SourceClassCode " &
            "ON EIS_Process.SourceClassCode = EISLK_SourceClassCode.sourceclasscode " &
            "LEFT JOIN EIS_EmissionsUnit " &
            "ON EIS_Process.FacilitySiteID = EIS_EmissionsUnit.FacilitySiteID " &
            "AND EIS_Process.EmissionsUnitID = EIS_EmissionsUnit.EmissionsUnitID " &
            "INNER JOIN EISLK_UnitStatusCode " &
            "ON EIS_EmissionsUnit.strUnitStatusCode = EISLK_UnitStatusCode.UnitStatusCode " &
            "WHERE EIS_Process.facilitysiteid = @FacilitySiteID " &
            "AND EIS_Process.active = '1' " &
            "ORDER BY eis_Process.EmissionsUnitID " &
            ", eis_Process.ProcessID "

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

        gvwProcesses.DataSource = DB.GetDataTable(query, param)
        gvwProcesses.DataBind()

    End Sub

    Protected Sub btnExport_Processes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport_Processes.Click

        loadProcessSummarygvw()
        ExportAsExcel("EISProcesses", gvwProcesses)

    End Sub

    Protected Sub btnReportsHome_Processes_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReportsHome_Processes.Click

        Response.Redirect("reports.aspx")

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