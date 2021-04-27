Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_Processes
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
        Dim query = "SELECT p.EmissionsUnitID       as [Emission Unit ID],
               s.strDesc               as [Unit Status],
               p.ProcessID             as [Process ID],
               p.strProcessDescription as [Process Description],
               p.SourceClassCode       as [SCC],
               CONCAT(c.[scc level one], '; ', c.[scc level two], '; ',
                      c.[scc level three], '; ', c.[scc level four])
                                       as [SCC Description],
               p.LastEISSubmitDate     as [Last EPA Submittal],
               p.strProcessComment     as [Comment]
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
        ORDER BY p.EmissionsUnitID, p.ProcessID"

        Dim param As New SqlParameter("@FacilitySiteID", CurrentAirs.ShortString)
        Dim dt As DataTable = DB.GetDataTable(query, param)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Processes.DataSource = dt
            Processes.DataBind()
        Else
            ProcessesEmptyNotice.Visible = True
            Processes.Visible = False
            ProcessesExport.Visible = False
        End If
    End Sub

    Private Sub ProcessesExport_Click(sender As Object, e As EventArgs) Handles ProcessesExport.Click
        ExportAsExcel($"{CurrentAirs.ShortString}_Processes", Processes)
    End Sub

End Class
