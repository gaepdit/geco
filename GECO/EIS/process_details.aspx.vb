Imports System.Data.SqlClient

Partial Class eis_process_details
    Inherits Page

    Private EmissionUnitStatus As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EUCtrlApproachExist As Boolean
        Dim ProcCtrlApproachExist As Boolean
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EIYear As Integer = CInt(GetCookie(EisCookie.EISMaxYear))
        Dim EmissionsUnitID As String = Request.QueryString("eu")
        Dim ProcessID As String = Request.QueryString("ep")

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            LoadProcessDetails(FacilitySiteID, EmissionsUnitID, ProcessID)
            LoadRPApportionment(FacilitySiteID, EmissionsUnitID, ProcessID)
            LoadReportingPeriodGVW(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID)

            'Hide and Display the Release Point Apportionment Information
            If gvwRPApportionment.Rows.Count = 0 Then
                lblRPApportionInfoWarning.Visible = True
            Else
                lblRPApportionInfoWarning.Visible = False
            End If

            'Hide and Display the Process Control Approach, Load Process Control Approach if needed.
            EUCtrlApproachExist = CheckEUControlApproachAny(FacilitySiteID, EmissionsUnitID)
            ProcCtrlApproachExist = CheckProcCtrlApproachSpec(FacilitySiteID, EmissionsUnitID, ProcessID)

            If EUCtrlApproachExist Then
                lblProcessControlApproachWarning.Text = "An Emission Unit Control Approach exists."
                lblProcessControlApproachWarning.ForeColor = Drawing.Color.ForestGreen
                pnlProcessControlApproach.Visible = False
            Else
                If ProcCtrlApproachExist Then
                    LoadProcessControlApproach(FacilitySiteID, EmissionsUnitID, ProcessID)
                    pnlProcessControlApproach.Visible = True
                    LoadProcessControlMeasure(FacilitySiteID, EmissionsUnitID, ProcessID)
                    LoadProcessControlPollutant(FacilitySiteID, EmissionsUnitID, ProcessID)

                    If gvwProcessControlMeasure.Rows.Count = 0 Then
                        lblProcessControlMeasureWarning.Text = "No Process Control Measures."
                    End If

                    If gvwProcessControlPollutant.Rows.Count = 0 Then
                        lblProcessControlPollutantWarning.Text = "No Process Control Pollutants."
                    End If
                Else
                    lblProcessControlApproachWarning.Text = "No Process Control Approach Exists ."
                    lblProcessControlApproachWarning.ForeColor = Drawing.Color.ForestGreen
                    pnlProcessControlApproach.Visible = False
                End If
            End If

            'Hide all buttons if Emission Unit is shutdown
            If EmissionUnitStatus = "Operating" Then
                lblEmissionUnitStatusWarning.Text = ""
            Else
                lblEmissionUnitStatusWarning.Text = "Emission Unit is in a shutdown state."
            End If

            HideTextBoxBorders(Me)
        End If
    End Sub

    'Load Release Point Apportionment Datagridview
    Private Sub LoadRPApportionment(ByVal fsid As String, ByVal euid As String, ByVal epid As String)
        sqldsRPApportionment.ConnectionString = DBConnectionString

        sqldsRPApportionment.SelectCommand = "SELECT EIS_RPAPPORTIONMENT.RELEASEPOINTID " &
            ", EIS_RELEASEPOINT.STRRPDESCRIPTION " &
            ", EISLK_RPTYPECODE.STRDESC AS RPType " &
            ", CONCAT(EIS_RPAPPORTIONMENT.INTAVERAGEPERCENTEMISSIONS, '%') AS Apportionment " &
            ", EIS_RPAPPORTIONMENT.STRRPAPPORTIONMENTCOMMENT " &
            ", CONVERT( char, EIS_RPAPPORTIONMENT.LASTEISSUBMITDATE, 101) AS LASTEISSUBMITDATE " &
            "FROM EIS_RPAPPORTIONMENT " &
            "INNER JOIN EIS_RELEASEPOINT " &
            "ON EIS_RPAPPORTIONMENT.RELEASEPOINTID = EIS_RELEASEPOINT.RELEASEPOINTID " &
            "AND EIS_RPAPPORTIONMENT.FACILITYSITEID = EIS_RELEASEPOINT.FacilitySiteID " &
            "LEFT JOIN EISLK_RPTYPECODE " &
            "ON EIS_RELEASEPOINT.STRRPTYPECODE = EISLK_RPTYPECODE.RPTYPECODE " &
            "WHERE EIS_RPAPPORTIONMENT.FACILITYSITEID = @fsid " &
            "AND EIS_RPAPPORTIONMENT.EMISSIONSUNITID = @euid " &
            "AND EIS_RPAPPORTIONMENT.PROCESSID = @epid " &
            "AND EIS_RPAPPORTIONMENT.ACTIVE = '1' " &
            "AND EISLK_RPTYPECODE.Active = '1' " &
            "ORDER BY 1 "

        sqldsRPApportionment.SelectParameters.Add("fsid", fsid)
        sqldsRPApportionment.SelectParameters.Add("euid", euid)
        sqldsRPApportionment.SelectParameters.Add("epid", epid)

        gvwRPApportionment.DataBind()
    End Sub

    'Load Process Control Measure DataGridView
    Private Sub LoadProcessControlMeasure(ByVal fsid As String, ByVal euid As String, ByVal epid As String)
        SqlDataSourceID1.ConnectionString = DBConnectionString

        SqlDataSourceID1.SelectCommand = "select EISLK_CONTROLMEASURECODE.STRDESC as CDType, " &
            "convert(char, EIS_PROCESSCONTROLMEASURE.LastEISSubmitDate, 101) as LastEISSubmitDate " &
            "FROM EIS_PROCESSCONTROLMEASURE, EISLK_CONTROLMEASURECODE " &
            "where EIS_PROCESSCONTROLMEASURE.FACILITYSITEID = @fsid " &
            "and EIS_PROCESSCONTROLMEASURE.EMISSIONSUNITID = @euid " &
            "and EIS_PROCESSCONTROLMEASURE.PROCESSID = @epid " &
            "and EIS_PROCESSCONTROLMEASURE.ACTIVE = '1' " &
            "and EISLK_CONTROLMEASURECODE.Active = '1' " &
            "and EIS_PROCESSCONTROLMEASURE.STRCONTROLMEASURECODE=EISLK_CONTROLMEASURECODE.STRCONTROLMEASURECODE " &
            "order by 1"

        SqlDataSourceID1.SelectParameters.Add("fsid", fsid)
        SqlDataSourceID1.SelectParameters.Add("euid", euid)
        SqlDataSourceID1.SelectParameters.Add("epid", epid)

        gvwProcessControlMeasure.DataBind()
    End Sub

    'Load Process Control Pollutant DataGridView
    Private Sub LoadProcessControlPollutant(ByVal fsid As String, ByVal euid As String, ByVal epid As String)
        SqlDataSourceID2.ConnectionString = DBConnectionString

        SqlDataSourceID2.SelectCommand = "SELECT l.STRDESC AS PollutantType " &
            " , CASE " &
            " WHEN p.NUMPCTCTRLMEASURESREDEFFIC IS NOT NULL " &
            " THEN concat(CONVERT(decimal(4, 1), p.NUMPCTCTRLMEASURESREDEFFIC), '%') " &
            " ELSE NULL " &
            " END AS CtrlEfficiency " &
            " , CONVERT(char, p.LASTEISSUBMITDATE, 101) AS LASTEISSUBMITDATE " &
            " , CASE " &
            " WHEN c.NUMPCTCTRLAPPROACHCAPEFFIC IS NOT NULL " &
            " AND c.NUMPCTCTRLAPPROACHEFFECT IS NOT NULL " &
            " AND p.NUMPCTCTRLMEASURESREDEFFIC IS NOT NULL " &
            " THEN concat(CONVERT(decimal(5, 2), c.NUMPCTCTRLAPPROACHCAPEFFIC * c.NUMPCTCTRLAPPROACHEFFECT * p.NUMPCTCTRLMEASURESREDEFFIC / 10000), '%') " &
            " ELSE NULL " &
            " END AS CalculatedReduction " &
            "FROM EIS_PROCESSCONTROLPOLLUTANT AS p " &
            "LEFT JOIN EISLK_POLLUTANTCODE AS l " &
            " ON p.POLLUTANTCODE = l.POLLUTANTCODE " &
            " AND l.Active = '1' " &
            "LEFT JOIN dbo.EIS_PROCESSCONTROLAPPROACH AS c " &
            " ON c.FACILITYSITEID = p.FACILITYSITEID " &
            " AND c.EMISSIONSUNITID = p.EMISSIONSUNITID " &
            " AND c.PROCESSID = p.PROCESSID " &
            " AND c.ACTIVE = '1' " &
            "WHERE p.FACILITYSITEID = @fsid " &
            " AND p.EMISSIONSUNITID = @euid " &
            " AND p.PROCESSID = @epid " &
            " AND p.ACTIVE = '1' " &
            "ORDER BY PollutantType "

        SqlDataSourceID2.SelectParameters.Add("fsid", fsid)
        SqlDataSourceID2.SelectParameters.Add("euid", euid)
        SqlDataSourceID2.SelectParameters.Add("epid", epid)

        gvwProcessControlPollutant.DataBind()
    End Sub

    'Load Process Reporting Period Emissions Table
    Private Sub LoadReportingPeriodGVW(ByVal MaxYear As Integer, ByVal fsid As String, ByVal euid As String, ByVal epid As String)

        Dim gvwReportingPeriodSize As Integer

        sqldsReportingPeriod.ConnectionString = DBConnectionString

        sqldsReportingPeriod.SelectCommand = "select strPollutantDescription, " &
            "Case " &
                "When vw_eis_yearly_poll_emissions.RptPeriodTypeCode = 'O3D' Then 'Summer Day' " &
                "Else 'Annual' " &
                "END RptPeriodType, " &
            "curr_fltTotalEmissions, " &
            "prev1_fltTotalEmissions, " &
            "prev2_fltTotalEmissions, " &
            "prev3_fltTotalEmissions, " &
            "prev4_fltTotalEmissions " &
            "FROM vw_eis_yearly_poll_emissions " &
            "where FacilitySiteID = @fsid " &
            "and EmissionsUnitID = @euid " &
            "and ProcessID = @epid " &
            "order by strPollutantDescription, RptPeriodTypeCode"

        sqldsReportingPeriod.SelectParameters.Add("fsid", fsid)
        sqldsReportingPeriod.SelectParameters.Add("euid", euid)
        sqldsReportingPeriod.SelectParameters.Add("epid", epid)

        gvwReportingPeriods.Columns(2).HeaderText = MaxYear
        gvwReportingPeriods.Columns(3).HeaderText = MaxYear - 1
        gvwReportingPeriods.Columns(4).HeaderText = MaxYear - 2
        gvwReportingPeriods.Columns(5).HeaderText = MaxYear - 3
        gvwReportingPeriods.Columns(6).HeaderText = MaxYear - 4
        gvwReportingPeriods.DataBind()

        gvwReportingPeriodSize = gvwReportingPeriods.Rows.Count

        If gvwReportingPeriodSize = 0 Then
            lblGVWReportingPeriodEmpty.Text = "This process has not participated in a previous reporting period."
        End If

    End Sub

    'Load the Process details
    Private Sub LoadProcessDetails(ByVal fsid As String, ByVal euid As String, ByVal epid As String)

        Dim UpdateUser As String
        Dim UpdateDateTime As String
        Dim EmissionsUnitID As String

        Try
            Dim query = " select " &
            "     p.PROCESSID, " &
            "     p.EMISSIONSUNITID, " &
            "     c.STRDESC                               as strEmissionsUnitStatus, " &
            "     p.STRPROCESSDESCRIPTION, " &
            "     p.SOURCECLASSCODE, " &
            "     convert(char, p.UpdateDateTime, 20)     As UpdateDateTime, " &
            "     convert(char, p.LastEISSubmitDate, 101) As LastEISSubmitDate, " &
            "     p.UPDATEUSER, " &
            "     p.STRPROCESSCOMMENT " &
            " FROM EIS_PROCESS p " &
            "     inner join EIS_EMISSIONSUNIT u " &
            "         on p.EMISSIONSUNITID = u.EMISSIONSUNITID " &
            "            and p.FACILITYSITEID = u.FACILITYSITEID " &
            "     left join EISLK_UNITSTATUSCODE c " &
            "         on c.UNITSTATUSCODE = u.STRUNITSTATUSCODE " &
            "            and u.ACTIVE = 1 " &
            " where p.FACILITYSITEID = @fsid " &
            "       and p.EMISSIONSUNITID = @euid " &
            "       and p.PROCESSID = @epid "

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid)
            }

            Dim dr As DataRow = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then
                If IsDBNull(dr("PROCESSID")) Then
                    txtProcessID.Text = ""
                Else
                    txtProcessID.Text = dr.Item("PROCESSID")
                End If

                If IsDBNull(dr("strEmissionsUnitStatus")) Then
                    EmissionUnitStatus = ""
                    txtEmissionUnitStatus.Text = EmissionUnitStatus
                Else
                    EmissionUnitStatus = dr.Item("strEmissionsUnitStatus")
                    txtEmissionUnitStatus.Text = EmissionUnitStatus
                End If

                If IsDBNull(dr("EMISSIONSUNITID")) Then
                    EmissionsUnitID = ""
                Else
                    EmissionsUnitID = dr.Item("EMISSIONSUNITID")
                    txtEmissionUnitID.Text = EmissionsUnitID
                    hlinkEmissionUnitID.NavigateUrl = "emissionunit_details.aspx?eu=" & EmissionsUnitID
                    hlinkEmissionUnitID.Text = EmissionsUnitID
                    txtEmissionUnitDesc.Text = GetEmissionUnitDesc(fsid, euid)
                End If
                If IsDBNull(dr("STRPROCESSDESCRIPTION")) Then
                    txtProcessDescription.Text = ""
                Else
                    txtProcessDescription.Text = dr.Item("STRPROCESSDESCRIPTION")
                End If
                If IsDBNull(dr("STRPROCESSCOMMENT")) Then
                    txtProcessComment.Text = ""
                    txtProcessComment.Visible = False
                Else
                    txtProcessComment.Text = dr.Item("STRPROCESSCOMMENT")
                End If

                If IsDBNull(dr("SOURCECLASSCODE")) Then
                    txtSourceClassCode.Text = ""
                    lblSccDesc.Text = ""
                Else
                    txtSourceClassCode.Text = dr.Item("SOURCECLASSCODE")

                    If IsValidScc(dr.Item("SOURCECLASSCODE")) Then
                        lblSccDesc.Text = GetSccDetails(dr.Item("SOURCECLASSCODE"))?.Description
                    End If
                End If

                If IsDBNull(dr("LastEISSubmitDate")) Then
                    txtLastEISSubmit.Text = "Never submitted"
                Else
                    txtLastEISSubmit.Text = dr.Item("LastEISSubmitDate")
                End If
                If IsDBNull(dr("UpdateUser")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr.Item("UpdateUser")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr("UpdateDateTime")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr.Item("UpdateDateTime")
                End If

                txtLastUpdate.Text = UpdateDateTime & " by " & UpdateUser
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    'Load the Process Control Approach
    Private Sub LoadProcessControlApproach(ByVal fsid As String, ByVal euid As String, ByVal epid As String)
        Dim UpdateUser As String
        Dim UpdateDateTime As String
        Dim PctCtrlApproachCapEffic As Decimal
        Dim PCTCTRLAPPROACHEFFECT As Decimal

        Try
            Dim query As String = "select EIS_PROCESSCONTROLAPPROACH.STRCONTROLAPPROACHDESC, " &
                "EIS_PROCESSCONTROLAPPROACH.NUMPCTCTRLAPPROACHCAPEFFIC, " &
                "EIS_PROCESSCONTROLAPPROACH.NUMPCTCTRLAPPROACHEFFECT, " &
                "EIS_PROCESSCONTROLAPPROACH.INTFIRSTINVENTORYYEAR, " &
                "EIS_PROCESSCONTROLAPPROACH.INTLASTINVENTORYYEAR, " &
                "convert(char, UpdateDateTime, 20) As UpdateDateTime,  " &
                "convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                "UpdateUser, " &
                "EIS_PROCESSCONTROLAPPROACH.STRCONTROLAPPROACHCOMMENT " &
                "FROM EIS_PROCESSCONTROLAPPROACH " &
                "where EIS_PROCESSCONTROLAPPROACH.FACILITYSITEID = @fsid " &
                "and EIS_PROCESSCONTROLAPPROACH.EMISSIONSUNITID = @euid and EIS_PROCESSCONTROLAPPROACH.ACTIVE = '1'" &
                "and EIS_PROCESSCONTROLAPPROACH.PROCESSID = @epid "

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid)
            }

            Dim dr As DataRow = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                If IsDBNull(dr("STRCONTROLAPPROACHDESC")) Then
                    txtControlApproachDescription.Text = ""
                Else
                    txtControlApproachDescription.Text = dr.Item("STRCONTROLAPPROACHDESC")
                End If
                If IsDBNull(dr("NUMPCTCTRLAPPROACHCAPEFFIC")) Then
                    txtPctCtrlApproachCapEffic.Text = ""
                Else
                    PctCtrlApproachCapEffic = dr.Item("NUMPCTCTRLAPPROACHCAPEFFIC")
                    If PctCtrlApproachCapEffic = -1 Then
                        txtPctCtrlApproachCapEffic.Text = ""
                    Else
                        txtPctCtrlApproachCapEffic.Text = PctCtrlApproachCapEffic
                    End If
                End If
                If IsDBNull(dr("NUMPCTCTRLAPPROACHEFFECT")) Then
                    txtPctCtrlApproachEffect.Text = ""
                Else
                    PCTCTRLAPPROACHEFFECT = dr.Item("NUMPCTCTRLAPPROACHEFFECT")
                    If PCTCTRLAPPROACHEFFECT = -1 Then
                        txtPctCtrlApproachEffect.Text = ""
                    Else
                        txtPctCtrlApproachEffect.Text = PCTCTRLAPPROACHEFFECT
                    End If
                End If
                If IsDBNull(dr("INTFIRSTINVENTORYYEAR")) Then
                    txtFirstInventoryYear.Text = ""
                Else
                    txtFirstInventoryYear.Text = dr.Item("INTFIRSTINVENTORYYEAR")
                End If
                If IsDBNull(dr("INTLASTINVENTORYYEAR")) Then
                    txtLastInventoryYear.Text = ""
                Else
                    txtLastInventoryYear.Text = dr.Item("INTLASTINVENTORYYEAR")
                End If
                If IsDBNull(dr("STRCONTROLAPPROACHCOMMENT")) Then
                    txtControlApproachComment.Text = ""
                Else
                    txtControlApproachComment.Text = dr.Item("STRCONTROLAPPROACHCOMMENT")
                End If
                If IsDBNull(dr("LastEISSubmitDate")) Then
                    txtLastSubmitEPA_CP.Text = "Never submitted"
                Else
                    txtLastSubmitEPA_CP.Text = dr.Item("LastEISSubmitDate")
                End If
                If IsDBNull(dr("UpdateUser")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr.Item("UpdateUser")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr("UpdateDateTime")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr.Item("UpdateDateTime")
                End If

                txtLastUpdate_CP.Text = UpdateDateTime & " by " & UpdateUser

            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

End Class