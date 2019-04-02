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

        Dim query = "SELECT p.EmissionsUnitID,
                   s.strDesc AS strUnitDesc,
                   p.ProcessID,
                   p.strProcessDescription,
                   p.SourceClassCode,
                   CONCAT(c.[scc level one], '; ', c.[scc level two], '; ',
                          c.[scc level three], '; ', c.[scc level four])
                             AS strSCCDesc,
                   p.strProcessComment,
                   p.LastEISSubmitDate
            FROM EIS_PROCESS p
                 LEFT JOIN EISLK_SCC c
                           ON p.SourceClassCode = c.SCC
                 LEFT JOIN EIS_EMISSIONSUNIT u
                           ON p.FacilitySiteID = u.FacilitySiteID
                               AND p.EmissionsUnitID = u.EmissionsUnitID
                 INNER JOIN EISLK_UNITSTATUSCODE s
                            ON u.strUnitStatusCode = s.UnitStatusCode
            WHERE p.facilitysiteid = @FacilitySiteID
              AND p.active = '1'
            ORDER BY p.EmissionsUnitID, p.ProcessID "

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