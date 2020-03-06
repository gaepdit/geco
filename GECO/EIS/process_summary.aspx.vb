Partial Class eis_process_summary
    Inherits Page

    Private FacilitySiteID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FacilitySiteID = GetCookie(Cookie.AirsNumber)
        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            loadProcessSummaryGVW()
        End If
    End Sub

    Private Sub loadProcessSummaryGVW()

        SqlDataSourceID1.ConnectionString = DBConnectionString

        SqlDataSourceID1.SelectCommand = "SELECT EIS_PROCESS.EMISSIONSUNITID " &
            ", (SELECT eislk_UnitStatusCode.strDesc " &
            "FROM eislk_UnitStatusCode " &
            ", eis_EmissionsUnit " &
            "WHERE eis_EmissionsUnit.strUnitStatusCode = eislk_UnitStatusCode.UnitStatusCode " &
            "AND eis_EmissionsUnit.FacilitySiteID = @FacilitySiteID " &
            "AND eis_EmissionsUnit.EmissionsUnitID = eis_Process.EmissionsUnitID " &
            "AND eis_EmissionsUnit.Active = '1' " &
            ") AS strEmissionsUnitStatus " &
            ", (SELECT eis_EmissionsUnit.strUnitDescription " &
            "FROM eis_EmissionsUnit " &
            "WHERE eis_EmissionsUnit.FacilitySiteID = @FacilitySiteID " &
            "AND eis_EmissionsUnit.EmissionsUnitID = eis_Process.EmissionsUnitID " &
            "AND eis_EmissionsUnit.Active = '1' " &
            ") AS strEmissionsUnitDesc " &
            ", EIS_PROCESS.PROCESSID " &
            ", EIS_PROCESS.STRPROCESSDESCRIPTION " &
            ", EIS_PROCESS.SOURCECLASSCODE " &
            ", EIS_PROCESS.LastEISSubmitDate " &
            ", CASE " &
            "WHEN EIS_PROCESSCONTROLAPPROACH.FacilitySiteID IS NULL " &
            "THEN 'No' " &
            "ELSE 'Yes' " &
            "END AS ControlApproach " &
            "FROM EIS_PROCESS " &
            "LEFT JOIN EIS_PROCESSCONTROLAPPROACH " &
            "ON EIS_PROCESS.FacilitySiteID = EIS_PROCESSCONTROLAPPROACH.FacilitySiteID " &
            "AND EIS_PROCESS.EmissionsUnitID = EIS_PROCESSCONTROLAPPROACH.EmissionsUnitID " &
            "AND EIS_PROCESS.PROCESSID = EIS_PROCESSCONTROLAPPROACH.PROCESSID " &
            "WHERE EIS_PROCESS.ACTIVE = '1' " &
            "AND EIS_PROCESS.FacilitySiteID = @FacilitySiteID " &
            "ORDER BY EIS_PROCESS.EmissionsUnitID " &
            ", EIS_PROCESS.PROCESSID "

        SqlDataSourceID1.SelectParameters.Add("FacilitySiteID", FacilitySiteID)

        gvwProcessSummary.DataBind()

    End Sub

End Class