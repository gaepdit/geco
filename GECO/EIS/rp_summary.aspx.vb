Imports System.Data.SqlClient

Partial Class EIS_rp_summary
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim InventoryYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        EIAccessCheck(EISAccessCode, EISStatus)

        If Not IsPostBack Then
            If InventoryYear <> "" Then
                LoadRPSummary_RP(FacilitySiteID, InventoryYear)
                LoadRPSummary_NoRP(FacilitySiteID, InventoryYear)
                lblRPSummary.Text = "Processes in the " & InventoryYear & " Reporting Period"
            End If
        End If

    End Sub

    Private Sub LoadRPSummary_NoRP(ByVal fsid As String, ByVal eiyear As String)
        Dim query = "select " &
            " vw_eis_RPSummary_NoRP.FacilitySiteID, " &
            " vw_eis_RPSummary_NoRP.EmissionsUnitID, " &
            " vw_eis_RPSummary_NoRP.ProcessID, " &
            " vw_eis_RPSummary_NoRP.strProcessDescription, " &
            " vw_eis_RPSummary_NoRP.ControlApproach, " &
            " vw_eis_RPSummary_NoRP.LastEISSubmitDate  " &
            " from " &
            " vw_eis_RPSummary_NoRP " &
            " where " &
            " vw_eis_RPSummary_NoRP.FacilitySiteID = @fsid and " &
            " vw_eis_RPSummary_NoRP.EmissionUnitActive = '1' " &
            " Order by vw_eis_RPSummary_NoRP.EmissionsUnitID, vw_eis_RPSummary_NoRP.ProcessID"

        Dim param As New SqlParameter("@fsid", fsid)

        gvwRPSummary_NoRP.DataSource = DB.GetDataTable(query, param)
        gvwRPSummary_NoRP.DataBind()

        If gvwRPSummary_NoRP.Rows.Count = 0 Then
            lblRPSummaryNoRP.Visible = False
        Else
            lblRPSummaryNoRP.Visible = True
            lblRPSummaryNoRP.Text = "Processes NOT in the " & eiyear & " Reporting Period"
        End If

    End Sub

    Private Sub LoadRPSummary_RP(ByVal fsid As String, ByVal eiyr As Integer)
        Dim query = "select vw_eis_rpsummary.emissionsunitid, " &
            " vw_eis_rpsummary.processid, " &
            " vw_eis_rpsummary.strprocessdescription, " &
            " vw_eis_rpsummary.controlapproach, " &
            " vw_eis_rpsummary.lasteissubmitdate " &
            " FROM  vw_eis_rpsummary " &
            " where vw_eis_rpsummary.facilitysiteid = @fsid " &
            " and vw_eis_rpsummary.intinventoryyear = @eiyr " &
            " and vw_eis_rpsummary.RPTPeriodTypeCode = 'A' " &
            " and vw_eis_rpsummary.strUnitStatusCode = 'OP' " &
            " order by vw_eis_rpsummary.emissionsunitid, vw_eis_rpsummary.processid "

        Dim params = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@eiyr", eiyr)
        }

        gvwReportingPeriodSummary.DataSource = DB.GetDataTable(query, params)
        gvwReportingPeriodSummary.DataBind()
    End Sub

    Protected Sub gvwRPSummary_NoRP_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwRPSummary_NoRP.PageIndexChanging

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim InventoryYear As String = GetCookie(EisCookie.EISMaxYear)

        gvwRPSummary_NoRP.PageIndex = e.NewPageIndex
        LoadRPSummary_NoRP(FacilitySiteID, InventoryYear)

    End Sub

    Protected Sub gvwRPSummary_NoRP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvwRPSummary_NoRP.RowDataBound
        'Hide command button in gridview
        Try
            If e.Row.RowType = DataControlRowType.DataRow AndAlso
                (e.Row.Cells(4).Text.ToString = "Not Submitted" OrElse e.Row.Cells(4).Text.ToString = "") Then
                e.Row.Cells(6).Enabled = False
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

End Class