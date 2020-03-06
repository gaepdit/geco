Imports System.Data.SqlClient

Partial Class eis_emissionunit_summary
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FIAccessCheck(EISAccessCode)

        pnlDeletedEmissionUnits.Visible = CheckDeletedEUExist(FacilitySiteID)

        LoadGVWEmissionUnitSummary(FacilitySiteID)

    End Sub

    Private Sub LoadGVWEmissionUnitSummary(ByVal fsid As String)
        Dim query = "SELECT eis_EmissionsUnit.EmissionsUnitID " &
            ", strUnitDescription " &
            ", (SELECT strDesc " &
            "FROM eislk_unittypecode " &
            "WHERE eislk_unittypecode.unittypecode = eis_emissionsunit.strunittypecode " &
            "AND eislk_unittypecode.Active = '1' " &
            ") AS unittypecode " &
            ", CASE " &
            "WHEN eis_emissionsunit.strUnitStatusCode = 'OP' " &
            "THEN 'Operating' " &
            "WHEN eis_emissionsunit.strUnitStatusCode = 'PS' " &
            "THEN 'Perm Shutdown' " &
            "WHEN eis_emissionsunit.strUnitStatusCode = 'TS' " &
            "THEN 'Temp Shutdown' " &
            "ELSE '' " &
            "END AS strUnitStatusCode " &
            ", eis_EmissionsUnit.LastEISSubmitDate " &
            ", CASE " &
            "WHEN eis_UnitControlApproach.EmissionsUnitID IS NULL " &
            "THEN 'No' " &
            "ELSE 'Yes' " &
            "END AS ControlApproach " &
            "FROM EIS_EmissionsUnit " &
            "LEFT JOIN eis_UnitControlApproach " &
            "ON eis_EmissionsUnit.FacilitySiteID = EIS_UnitControlApproach.FacilitySiteID " &
            "AND EIS_EmissionsUnit.FacilitySiteID = EIS_UnitControlApproach.FacilitySiteID " &
            "AND eis_EmissionsUnit.EmissionsUnitID = eis_UnitControlApproach.EmissionsUnitID " &
            " AND eis_UnitControlApproach.ACTIVE = '1' " &
            "WHERE eis_EmissionsUnit.ACTIVE = '1' " &
            "AND eis_EmissionsUnit.FacilitySiteID = @fsid " &
            "ORDER BY eis_EmissionsUnit.EmissionsUnitID "

        Dim param As New SqlParameter("@fsid", fsid)

        gvwEmissionUnitSummary.DataSource = DB.GetDataTable(query, param)

        gvwEmissionUnitSummary.DataBind()
    End Sub

    Protected Sub btnShowDeletedEU_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDeletedEU.Click

        Dim btnText As String = Left(btnShowDeletedEU.Text, 4)

        If btnText = "Show" Then
            LoadGVWDeletedEU()
            btnShowDeletedEU.Text = "Hide Deleted Emission Units"
        Else
            UnloadGVWDeletedEU()
            btnShowDeletedEU.Text = "Show Deleted Emission Units"
        End If

    End Sub

    Private Sub LoadGVWDeletedEU()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        Dim query = "select EmissionsUnitID, strUnitDescription " &
            "FROM eis_EmissionsUnit " &
            "where " &
            "FacilitySiteID = @FacilitySiteID " &
            "and Active = '0' " &
            "order by EmissionsUnitID"

        gvwDeletedEU.DataSource = DB.GetDataTable(query, New SqlParameter("@FacilitySiteID", FacilitySiteID))

        gvwDeletedEU.DataBind()
    End Sub

    Private Sub UnloadGVWDeletedEU()
        Dim query = "select EmissionsUnitID, strUnitDescription " &
            "FROM eis_EmissionsUnit " &
            "where 1 = 0"

        gvwDeletedEU.DataSource = DB.GetDataTable(query)

        gvwDeletedEU.DataBind()
    End Sub

End Class