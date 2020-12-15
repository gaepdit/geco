Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_Processes
    Inherits Page

    Private Property FacilitySiteID As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(FacilitySiteID) Then
            Response.Redirect("~/")
        End If

        FacilitySiteID = New ApbFacilityId(FacilitySiteID)
        Master.CurrentAirs = FacilitySiteID
        Master.IsFacilitySet = True

        LoadProcesses()
    End Sub


    Private Sub LoadProcesses()
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
            ORDER BY p.EmissionsUnitID, p.ProcessID"

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID.ShortString)
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
        ExportAsExcel($"{FacilitySiteID.ShortString}_Processes", Processes)
    End Sub

End Class
