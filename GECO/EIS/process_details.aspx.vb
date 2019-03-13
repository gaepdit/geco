Imports System.Data
Imports System.Data.SqlClient

Partial Class eis_process_details
    Inherits Page
    Public conn, conn1 As New SqlConnection(DBConnectionString)
    Public IDExists As Boolean
    Public EmissionUnitStatus As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EUCtrlApproachExist As Boolean = "False"
        Dim ProcCtrlApproachExist As Boolean = "False"
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EIYear As Integer = CInt(GetCookie(EisCookie.EISMaxYear))
        Dim EmissionsUnitID As String = Request.QueryString("eu")
        Dim ProcessID As String = Request.QueryString("ep")

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            LoadReleasePointDDL(FacilitySiteID)
            LoadProcessDetails(FacilitySiteID, EmissionsUnitID, ProcessID)
            LoadRPApportionment(FacilitySiteID, EmissionsUnitID, ProcessID)
            LoadReportingPeriodGVW(EIYear, FacilitySiteID, EmissionsUnitID, ProcessID)

            'Hide and Display the Release Point Apportionment Information
            If gvwRPApportionment.Rows.Count = 0 Then
                btnEditRPApportion.Text = "Add"
                btnEditRPApportion.Visible = True
            Else
                lblRPApportionInfoWarning.Visible = False
                btnEditRPApportion.Text = "Edit"
                btnEditRPApportion.Visible = True
            End If

            'Hide and Display the Process Control Approach, Load Process Control Approach if needed.
            EUCtrlApproachExist = CheckEUControlApproachAny(FacilitySiteID, EmissionsUnitID)
            ProcCtrlApproachExist = CheckProcCtrlApproachSpec(FacilitySiteID, EmissionsUnitID, ProcessID)

            If EUCtrlApproachExist Then
                lblProcessControlApproachWarning.Text = "An Emission Unit Control Approach exists, no Process Control Approach can be added."
                lblProcessControlApproachWarning.ForeColor = Drawing.Color.ForestGreen
                pnlProcessControlApproach.Visible = False
                btnAddControlApproach.Visible = False
                btnEditControlApproach.Visible = False
            Else
                If ProcCtrlApproachExist Then
                    LoadProcessControlApproach(FacilitySiteID, EmissionsUnitID, ProcessID)
                    pnlProcessControlApproach.Visible = True
                    btnAddControlApproach.Visible = False
                    btnEditControlApproach.Visible = True
                    LoadProcessControlMeasure(FacilitySiteID, EmissionsUnitID, ProcessID)
                    LoadProcessControlPollutant(FacilitySiteID, EmissionsUnitID, ProcessID)
                    If gvwProcessControlMeasure.Rows.Count = 0 Then
                        lblProcessControlMeasureWarning.Text = "At least one Process Control Measure must be added to the Process Control Approach."
                    End If
                    If gvwProcessControlPollutant.Rows.Count = 0 Then
                        lblProcessControlPollutantWarning.Text = "At least one Process Control Pollutant must be added to the Process Control Approach."
                    End If

                Else
                    lblProcessControlApproachWarning.Text = "No Process Control Approach Exists. Click Add to create one for this Process."
                    lblProcessControlApproachWarning.ForeColor = Drawing.Color.ForestGreen
                    pnlProcessControlApproach.Visible = False
                    btnAddControlApproach.Visible = True
                    btnEditControlApproach.Visible = False
                End If
            End If

            'Hide all buttons if Emission Unit is shutdown
            If EmissionUnitStatus = "Operating" Then
                'Do nothing
                lblEmissionUnitStatusWarning.Text = ""
            Else
                btnEdit.Visible = False
                btnEditControlApproach.Visible = False
                btnAddProcess.Visible = False
                btnAddControlApproach.Visible = False
                btnEditRPApportion.Visible = False
                lblEmissionUnitStatusWarning.Text = "Emission Unit is in a shutdown state. All actions for this unit are disabled."
            End If

            ShowFacilityInventoryMenu()
            ShowEISHelpMenu()
            If EISStatus = "2" Then
                ShowEmissionInventoryMenu()
            Else
                HideEmissionInventoryMenu()
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

    'Load the Add Process Release Point dropdown list.
    Private Sub LoadReleasePointDDL(ByVal fsid As String)
        Dim sql As String
        Dim rpID As String

        ddlexistReleasePointID.Items.Add("--Select Release Point ID--")
        Try
            sql = "select RELEASEPOINTID FROM EIS_RELEASEPOINT " &
                  "where FacilitySiteID = '" & fsid & "' and " &
                  "strRPStatusCode = 'OP' and " &
                  "Active = '1' " &
                  "Order by RELEASEPOINTID"

            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader

            While dr.Read
                rpID = dr.Item("RELEASEPOINTID")
                ddlexistReleasePointID.Items.Add(rpID)
            End While

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    'Load the Process details
    Private Sub LoadProcessDetails(ByVal fsid As String, ByVal euid As String, ByVal epid As String)

        Dim UpdateUser As String = ""
        Dim UpdateDateTime As String = ""
        Dim EmissionsUnitID As String = ""

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
                    txtExistProcessID.Text = dr.Item("PROCESSID")
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
                    txtExistEmissionUnitID.Text = EmissionsUnitID
                    txtExistEmissionUnitID2.Text = EmissionsUnitID
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
                Else
                    txtSourceClassCode.Text = dr.Item("SOURCECLASSCODE")
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
        Dim sql As String = ""
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = ""
        Dim UpdateDateTime As String = ""
        Dim PctCtrlApproachCapEffic As Decimal
        Dim PCTCTRLAPPROACHEFFECT As Decimal

        Try
            sql = "select EIS_PROCESSCONTROLAPPROACH.STRCONTROLAPPROACHDESC, " &
                        "EIS_PROCESSCONTROLAPPROACH.NUMPCTCTRLAPPROACHCAPEFFIC, " &
                        "EIS_PROCESSCONTROLAPPROACH.NUMPCTCTRLAPPROACHEFFECT, " &
                        "EIS_PROCESSCONTROLAPPROACH.INTFIRSTINVENTORYYEAR, " &
                        "EIS_PROCESSCONTROLAPPROACH.INTLASTINVENTORYYEAR, " &
                        "convert(char, UpdateDateTime, 20) As UpdateDateTime,  " &
                        "convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                        "UpdateUser, " &
                        "EIS_PROCESSCONTROLAPPROACH.STRCONTROLAPPROACHCOMMENT " &
                        "FROM EIS_PROCESSCONTROLAPPROACH " &
                        "where EIS_PROCESSCONTROLAPPROACH.FACILITYSITEID = '" & fsid & "' " &
                        "and EIS_PROCESSCONTROLAPPROACH.EMISSIONSUNITID = '" & euid & "' and EIS_PROCESSCONTROLAPPROACH.ACTIVE = '1'" &
                        "and EIS_PROCESSCONTROLAPPROACH.PROCESSID = '" & epid & "' "

            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader

            dr.Read()

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

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim ep As String = txtProcessID.Text
        Dim eu As String = txtEmissionUnitID.Text
        Dim targetpage As String = "process_edit.aspx" & "?ep=" & ep & "&eu=" & eu
        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnEditControlApproach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditControlApproach.Click
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim eu As String = txtEmissionUnitID.Text.ToUpper
        Dim targetpage As String = "processcontrolapproach_edit.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)
    End Sub

    Private Sub InsertProcess(ByVal fsid As String, ByVal euid As String, ByVal pid As String)

        'Code to insert a new Process
        'Reminder: insert only FacilitySiteID, Process ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim sql As String = ""
        Dim sql2 As String = ""
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim NewEmissionUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim NewProcessID As String = txtNewProcessID.Text.ToUpper
        Dim ReleasePointID As String = ddlexistReleasePointID.SelectedValue.ToUpper
        Dim ProcessDescription As String = txtNewProcessDesc.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim Active As String = "1"

        Try
            'Insert new process into talbe EIS_PROCESS
            sql = "Insert into EIS_PROCESS (" &
                        "FacilitySiteID, " &
                        "EmissionsUnitID, " &
                        "PROCESSID, " &
                        "STRPROCESSDESCRIPTION, " &
                        "Active, " &
                        "UpdateUser, " &
                        "UpdateDateTime, " &
                        "CreateDateTime) " &
                "Values (" &
                        "'" & FacilitySiteID & "', " &
                        "'" & NewEmissionUnitID & "', " &
                        "'" & NewProcessID & "', " &
                        "'" & ProcessDescription & "', " &
                        "'" & Active & "', " &
                        "'" & Replace(UpdateUser, "'", "''") & "', " &
                        "getdate(), " &
                        "getdate()) "

            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader
            dr.Close()

            'Insert new Release point apportionment into table EIS_RPAPPORTIONMENT
            sql2 = "Insert into EIS_RPAPPORTIONMENT (" &
                       "FacilitySiteID, " &
                       "EmissionsUnitID, " &
                       "PROCESSID, " &
                       "RELEASEPOINTID, " &
                       "INTAVERAGEPERCENTEMISSIONS, " &
                       "Active, " &
                       "RPAPPORTIONMENTID, " &
                       "UpdateUser, " &
                       "UpdateDateTime, " &
                       "CreateDateTime) " &
               "Values (" &
                       "'" & FacilitySiteID & "', " &
                       "'" & NewEmissionUnitID & "', " &
                       "'" & NewProcessID & "', " &
                       "'" & ReleasePointID & "', " &
                       "100, " &
                       "'" & Active & "', " &
                       "(select " &
                       "case " &
                       "when max(RPAPPORTIONMENTID) is null then 1 " &
                       "else max(RPAPPORTIONMENTID) + 1 " &
                       "End RPAPPORTIONMENTID " &
                       "FROM EIS_RPAPPORTIONMENT), " &
                       "'" & Replace(UpdateUser, "'", "''") & "', " &
                       "getdate(), " &
                       "getdate()) "

            Dim cmd2 As New SqlCommand(sql2, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr2 As SqlDataReader = cmd2.ExecuteReader

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Private Sub UpdateProcess(ByVal fsid As String, ByVal euid As String, ByVal pid As String)

        'Code to update a deleted Process that is being re-used
        'Reminder: insert only FacilitySiteID, Process ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim sql As String = ""
        Dim sql2 As String = ""
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionunitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtNewProcessID.Text.ToUpper
        Dim ReleasePointID As String = ddlexistReleasePointID.SelectedValue.ToUpper
        Dim ProcessDescription As String = txtNewProcessDesc.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = GetCookie(GecoCookie.UserID) & "-" & GetCookie(GecoCookie.UserName)
        Dim Active As String = "1"

        Try
            'Update process in table EIS_PROCESS, change Active = 1
            sql = "Update EIS_PROCESS " &
                  " Set Active = '" & Active & "', " &
                  " UPDATEUSER = '" & Replace(UpdateUser, "'", "''") & "', " &
                  " UpdateDateTime = getdate() " &
                  " where EIS_PROCESS.FACILITYSITEID = '" & FacilitySiteID & "'and " &
                  " EIS_PROCESS.EmissionsUnitID = '" & EmissionunitID & "' and " &
                  " EIS_PROCESS.PROCESSID = '" & ProcessID & "' "

            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader
            dr.Close()

            'insert new Release point apportionment into table EIS_RPAPPORTIONMENT
            sql2 = "Insert into EIS_RPAPPORTIONMENT (" &
                       "FacilitySiteID, " &
                       "EmissionsUnitID, " &
                       "PROCESSID, " &
                       "RELEASEPOINTID, " &
                       "INTAVERAGEPERCENTEMISSIONS, " &
                       "Active, " &
                       "RPAPPORTIONMENTID, " &
                       "UpdateUser, " &
                       "UpdateDateTime, " &
                       "CreateDateTime) " &
               "Values (" &
                       "'" & FacilitySiteID & "', " &
                       "'" & EmissionunitID & "', " &
                       "'" & ProcessID & "', " &
                       "'" & ReleasePointID & "', " &
                       "100, " &
                       "'" & Active & "', " &
                       "(select " &
                       "case " &
                       "when max(RPAPPORTIONMENTID) is null then 1 " &
                       "else max(RPAPPORTIONMENTID) + 1 " &
                       "End RPAPPORTIONMENTID " &
                       "FROM EIS_RPAPPORTIONMENT), " &
                       "'" & Replace(UpdateUser, "'", "''") & "', " &
                       "getdate(), " &
                       "getdate()) "

            Dim cmd2 As New SqlCommand(sql2, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr2 As SqlDataReader = cmd2.ExecuteReader

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    'Custom Validator checks for valid Active Process ID

    Sub ProcessIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtExistEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = args.Value.ToUpper
        Dim targetpage As String = "Process_edit.aspx" & "?ep=" & ProcessID & "&eu=" & EmissionsUnitID
        Dim ProcessActive = CheckProcessExist(FacilitySiteID, EmissionsUnitID, ProcessID)

        Select Case ProcessActive
            Case UnitActiveStatus.Inactive
                args.IsValid = True
                UpdateProcess(FacilitySiteID, EmissionsUnitID, ProcessID)
                Response.Redirect(targetpage)
            Case UnitActiveStatus.Active
                args.IsValid = False
                cusvProcessID.ErrorMessage = " Process " + ProcessID + " is already in use.  Please enter another."
                txtNewProcessID.Text = ""
                btnAddProcess_ModalPopupExtender.Show()
            Case UnitActiveStatus.DoesNotExist
                args.IsValid = True
                InsertProcess(FacilitySiteID, EmissionsUnitID, ProcessID)
                Response.Redirect(targetpage)
        End Select

    End Sub

    Private Sub InsertProcessControlApproach()

        'Code to insert a new process control approach
        'Reminder: insert only FacilitySiteID, Control approach description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim sql As String = ""
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim eu As String = txtEmissionUnitID.Text.ToUpper
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim ProcessControlApproachDesc As String = txtNewProcessControlApproachDesc.Text
        Dim ProcCtrlApproachCapEffic As String = txtProcessCACaptureEffic.Text
        Dim ProcCtrlApproachEffect As String = txtProcessCAControlEffect.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim Active As String = "1"

        Try
            sql = "Insert into EIS_PROCESSCONTROLAPPROACH (" &
                        "FacilitySiteID, " &
                        "EmissionsUnitID, " &
                        "PROCESSID, " &
                        "STRCONTROLAPPROACHDESC, " &
                        "NUMPCTCTRLAPPROACHCAPEFFIC, " &
                        "NUMPCTCTRLAPPROACHEFFECT, " &
                        "Active, " &
                        "UpdateUser, " &
                        "UpdateDateTime, " &
                        "CreateDateTime) " &
                "Values (" &
                        "'" & FacilitySiteID & "', " &
                        "'" & eu & "', " &
                        "'" & ep & "', " &
                        "'" & ProcessControlApproachDesc & "', " &
                        DbStringDecimalOrNull(ProcCtrlApproachCapEffic) & ", " &
                        DbStringDecimalOrNull(ProcCtrlApproachEffect) & ", " &
                        "'" & Active & "', " &
                        "'" & Replace(UpdateUser, "'", "''") & "', " &
                        "getdate(), " &
                        "getdate()) "

            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Sub ProcessControlApproachCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)

        Dim sql As String = ""
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim ProcessControlApproach As String = args.Value

        ProcessControlApproach = ProcessControlApproach.ToUpper
        Try
            sql = "Select STRCONTROLAPPROACHDESC FROM EIS_PROCESSCONTROLAPPROACH " &
                    "where FacilitySiteID = '" & FacilitySiteID & "' " &
                    "and STRCONTROLAPPROACHDESC = '" & ProcessControlApproach & "'"

            Dim cmd1 As New SqlCommand(sql, conn1)

            If conn1.State = ConnectionState.Open Then
            Else
                conn1.Open()
            End If

            Dim dr1 As SqlDataReader = cmd1.ExecuteReader
            Dim recExist As Boolean = dr1.Read

            If recExist Then
                args.IsValid = False
                btnAddControlApproach_ModalPopupExtender.Show()
                IDExists = True
            Else
                args.IsValid = True
                IDExists = False
            End If

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Protected Sub btnInsertProcessControlApproach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsertProcessControlApproach.Click
        Dim Processid As String = txtExistProcessID.Text.ToUpper
        Dim EmissionUnitId As String = txtEmissionUnitID.Text.ToUpper
        Dim targetpage As String = "Processcontrolapproach_edit.aspx" & "?eu=" & EmissionUnitId & "&ep=" & Processid
        If IDExists Then
            btnInsertProcessControlApproach.Visible = False
            'Nothing - we do this to make sure the modalpopup shows if there is an existing emission unit id
        Else
            InsertProcessControlApproach()
            Response.Redirect(targetpage)
        End If
    End Sub

    Protected Sub btnEditRPApportion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditRPApportion.Click
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim eu As String = txtEmissionUnitID.Text.ToUpper
        Dim targetpage As String = "rpapportionment_edit.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)
    End Sub

#Region "  Menu Routines  "

    Private Sub ShowFacilityInventoryMenu()
        Dim menuFacilityInventory As Panel
        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)
        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = True
        End If

    End Sub

    Private Sub HideFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = False
        End If

    End Sub

    Private Sub ShowEmissionInventoryMenu()

        Dim menuEmissionInventory As Panel

        menuEmissionInventory = CType(Master.FindControl("pnlEmissionInventory"), Panel)

        If Not menuEmissionInventory Is Nothing Then
            menuEmissionInventory.Visible = True
        End If

    End Sub

    Private Sub HideEmissionInventoryMenu()

        Dim menuEmissionInventory As Panel

        menuEmissionInventory = CType(Master.FindControl("pnlEmissionInventory"), Panel)

        If Not menuEmissionInventory Is Nothing Then
            menuEmissionInventory.Visible = False
        End If

    End Sub

    Private Sub ShowEISHelpMenu()

        Dim menuEISHelp As Panel

        menuEISHelp = CType(Master.FindControl("pnlEISHelp"), Panel)

        If Not menuEISHelp Is Nothing Then
            menuEISHelp.Visible = True
        End If

    End Sub

    Private Sub HideEISHelpMenu()

        Dim menuEISHelp As Panel

        menuEISHelp = CType(Master.FindControl("pnlEISHelp"), Panel)

        If Not menuEISHelp Is Nothing Then
            menuEISHelp.Visible = False
        End If

    End Sub

#End Region

End Class