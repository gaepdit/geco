Imports System.Data.SqlClient

Partial Class eis_process_summary
    Inherits Page

    Private FacilitySiteID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FacilitySiteID = GetCookie(Cookie.AirsNumber)
        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            loadEmissionUnitID()
            loadReleasePointID()
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

    Private Sub loadEmissionUnitID()
        Try
            ddlExistEmissionUnitID.Items.Add("--Select Emission Unit ID--")

            Dim query = "select EmissionsUnitID FROM eis_EmissionsUnit " &
                  " where FacilitySiteID = @FacilitySiteID and Active = '1' " &
                  " and strUnitStatusCode = 'OP' order by EmissionsUnitID "

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dt = DB.GetDataTable(query, param)

            For Each dr In dt.Rows
                ddlExistEmissionUnitID.Items.Add(dr.Item("EmissionsUnitID").ToString)
            Next

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub loadReleasePointID()
        Try
            ddlexistReleasePointID.Items.Add("--Select Release Point ID--")

            Dim query = "select RELEASEPOINTID FROM EIS_RELEASEPOINT " &
                  "where FacilitySiteID = @FacilitySiteID and " &
                  "strRPStatusCode = 'OP' and " &
                  "Active = '1' " &
                  "Order by RELEASEPOINTID"

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dt = DB.GetDataTable(query, param)

            For Each dr In dt.Rows
                ddlexistReleasePointID.Items.Add(dr.Item("RELEASEPOINTID").ToString)
            Next

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub InsertProcess()

        'Code to insert a new Process
        'Reminder: insert only FacilitySiteID, Process ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim EmissionunitID As String = ddlExistEmissionUnitID.SelectedValue.ToUpper
        Dim ProcessID As String = Left(txtNewProcessID.Text.ToUpper, 6)
        Dim ReleasePointID As String = ddlexistReleasePointID.SelectedValue.ToUpper
        Dim ProcessDescription As String = Left(txtNewProcessDesc.Text, 200)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = Left(UpdateUserID & "-" & UpdateUserName, 250)

        Try
            Dim params As SqlParameter() = {
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionunitID", EmissionunitID),
                New SqlParameter("@ProcessID", ProcessID),
                New SqlParameter("@ProcessDescription", ProcessDescription),
                New SqlParameter("@ReleasePointID", ReleasePointID)
            }

            'insert new process into talbe EIS_PROCESS
            Dim query = "Insert into EIS_PROCESS (" &
                        " FacilitySiteID, " &
                        " EmissionsUnitID, " &
                        " PROCESSID, " &
                        " STRPROCESSDESCRIPTION, " &
                        " Active, " &
                        " UpdateUser, " &
                        " UpdateDateTime, " &
                        " CreateDateTime) " &
                        " Values (" &
                        " @FacilitySiteID, " &
                        " @EmissionunitID, " &
                        " @ProcessID, " &
                        " @ProcessDescription, " &
                        " '1', " &
                        " @UpdateUser, " &
                        " getdate(), " &
                        " getdate()) "

            DB.RunCommand(query, params)

            'insert new Release point apportionment into table EIS_RPAPPORTIONMENT
            query = "Insert into EIS_RPAPPORTIONMENT (" &
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
                       "@FacilitySiteID, " &
                       "@EmissionunitID, " &
                       "@ProcessID, " &
                       "@ReleasePointID, " &
                       "100, " &
                       "'1', " &
                       "(select " &
                       "case " &
                       "when max(RPAPPORTIONMENTID) is null then 1 " &
                       "else max(RPAPPORTIONMENTID) + 1 " &
                       "End RPAPPORTIONMENTID " &
                       "FROM EIS_RPAPPORTIONMENT), " &
                       "@UpdateUser, " &
                       "getdate(), " &
                       "getdate()) "

            DB.RunCommand(query, params)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub UpdateProcess()

        'Code to update a deleted Process that is being re-used
        'Reminder: insert only FacilitySiteID, Process ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim EmissionunitID As String = ddlExistEmissionUnitID.SelectedValue.ToUpper
        Dim ProcessID As String = txtNewProcessID.Text.ToUpper
        Dim ReleasePointID As String = ddlexistReleasePointID.SelectedValue.ToUpper
        Dim UpdateUser As String = GetCookie(GecoCookie.UserID) & "-" & GetCookie(GecoCookie.UserName)

        Try
            Dim params As SqlParameter() = {
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionunitID", EmissionunitID),
                New SqlParameter("@ProcessID", ProcessID),
                New SqlParameter("@ReleasePointID", ReleasePointID)
            }

            'Update process in table EIS_PROCESS, change Active = 1
            Dim query = "Update EIS_PROCESS " &
            " Set Active = 1, " &
            " UPDATEUSER = @UpdateUser, " &
            " UpdateDateTime = getdate() " &
            " where FACILITYSITEID = @FacilitySiteID and " &
            " EIS_PROCESS.EmissionsUnitID = @EmissionunitID and " &
            " EIS_PROCESS.PROCESSID = @ProcessID "

            DB.RunCommand(query, params)

            'insert new Release point apportionment into table EIS_RPAPPORTIONMENT
            query = " insert into EIS_RPAPPORTIONMENT ( " &
            "     FACILITYSITEID, " &
            "     EMISSIONSUNITID, " &
            "     PROCESSID, " &
            "     RELEASEPOINTID, " &
            "     INTAVERAGEPERCENTEMISSIONS, " &
            "     ACTIVE, " &
            "     RPAPPORTIONMENTID, " &
            "     UPDATEUSER, " &
            "     UPDATEDATETIME, " &
            "     CREATEDATETIME " &
            " ) " &
            "     select " &
            "         @FacilitySiteID, " &
            "         @EmissionunitID, " &
            "         @ProcessID, " &
            "         @ReleasePointID, " &
            "         100, " &
            "         '1', " &
            "         (select case " &
            "                 when max(RPAPPORTIONMENTID) is null " &
            "                     then 1 " &
            "                 else max(RPAPPORTIONMENTID) + 1 " &
            "                 End ID " &
            "          from EIS_RPAPPORTIONMENT), " &
            "         @UpdateUser, " &
            "         getdate(), " &
            "         getdate() " &
            "     where not exists " &
            "     (select * " &
            "      FROM EIS_RPAPPORTIONMENT " &
            "      where FACILITYSITEID = @FacilitySiteID " &
            "            and EMISSIONSUNITID = @EmissionunitID " &
            "            and PROCESSID = @ProcessID " &
            "            and RELEASEPOINTID = @ReleasePointID) "

            DB.RunCommand(query, params)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    'Custom Validator checks for valid Active Process ID

    Sub ProcessIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)

        Dim EmissionsUnitID As String = ddlExistEmissionUnitID.SelectedItem.Value
        Dim ProcessID As String = args.Value.ToUpper
        Dim ProcessActive = CheckProcessExist(FacilitySiteID, EmissionsUnitID, ProcessID)
        Dim targetpage As String = "Process_edit.aspx" & "?ep=" & ProcessID & "&eu=" & EmissionsUnitID

        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        Select Case ProcessActive
            Case UnitActiveStatus.Inactive
                args.IsValid = True
                UpdateProcess()
                Response.Redirect(targetpage)
            Case UnitActiveStatus.Active
                args.IsValid = False
                cusvProcessID.ErrorMessage = " Process " & ProcessID & " is already in use.  Please enter another."
                txtNewProcessID.Text = ""
                btnAdd_ModalPopupExtender.Show()
            Case UnitActiveStatus.DoesNotExist
                args.IsValid = True
                InsertProcess()
                Response.Redirect(targetpage)
        End Select
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ddlExistEmissionUnitID.SelectedIndex = 0
        ddlexistReleasePointID.SelectedIndex = 0
        txtNewProcessID.Text = ""
        txtNewProcessDesc.Text = ""
    End Sub

End Class