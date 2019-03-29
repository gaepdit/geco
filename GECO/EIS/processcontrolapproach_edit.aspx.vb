Imports System.Data.SqlClient

Partial Class eis_processcontrolapproach_edit
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim processid As String = Request.QueryString("ep").ToUpper
        Dim emissionunitID As String = Request.QueryString("eu").ToUpper
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            LoadcontrolMeasureDDL()
            LoadControlPollutantDDL()
            LoadProcessControlApproach(FacilitySiteID, emissionunitID, processid)
            LoadProcessControlMeasureDGV(FacilitySiteID, emissionunitID, processid)
            LoadProcessControlPollutantsDGV(FacilitySiteID, emissionunitID, processid)

            If gvwProcessControlMeasure.Rows.Count = 0 Then
                lblProcessControlMeasureWarning.Text = "Click Add to include at least one Control Measure."
                lblProcessControlMeasureWarning.Visible = True
                btnAddControlMeasure.Visible = True
            Else
                lblProcessControlMeasureWarning.Visible = False
            End If

            If gvwProcessCtrlPollutant.Rows.Count = 0 Then
                lblProcessControlPollutantWarning.Text = "Click Add to include at least one Control Pollutant."
                lblProcessControlPollutantWarning.Visible = True
                btnAddControlPollutant.Visible = True
            Else
                lblProcessControlPollutantWarning.Visible = False
            End If
        End If
        btnDeleteDetails.Visible = False

        ShowFacilityInventoryMenu()
        ShowEISHelpMenu()
        If EISStatus = "2" Then
            ShowEmissionInventoryMenu()
        Else
            HideEmissionInventoryMenu()
        End If

    End Sub
    'Process Control Measure DataGridView

    Private Sub LoadProcessControlMeasureDGV(ByVal fsid As String, ByVal euid As String, ByVal epid As String)
        Try
            Dim query As String = "select " &
                "EIS_PROCESSCONTROLMEASURE.FACILITYSITEID, " &
                "EIS_PROCESSCONTROLMEASURE.EMISSIONSUNITID, " &
                "EIS_PROCESSCONTROLMEASURE.PROCESSID, " &
                "EIS_PROCESSCONTROLMEASURE.STRCONTROLMEASURECODE as MeasureCode, " &
                "(Select strDesc FROM EISLK_CONTROLMEASURECODE where " &
                "EIS_PROCESSCONTROLMEASURE.STRCONTROLMEASURECODE = EISLK_CONTROLMEASURECODE.STRCONTROLMEASURECODE " &
                "and EISLK_CONTROLMEASURECODE.Active = '1') as CDType, " &
                "EIS_PROCESSCONTROLMEASURE.LastEISSubmitDate from " &
                "EIS_PROCESSCONTROLMEASURE " &
                "where EIS_PROCESSCONTROLMEASURE.FACILITYSITEID = @fsid " &
                "and EIS_PROCESSCONTROLMEASURE.EMISSIONSUNITID = @euid " &
                "and EIS_PROCESSCONTROLMEASURE.PROCESSID = @epid " &
                "and EIS_PROCESSCONTROLMEASURE.ACTIVE = '1' " &
                "order by CDType"

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid)
            }

            gvwProcessControlMeasure.DataSource = DB.GetDataTable(query, params)
            gvwProcessControlMeasure.DataBind()

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub
    'Process Control Pollutant Datagridview

    Private Sub LoadProcessControlPollutantsDGV(ByVal fsid As String, ByVal euid As String, ByVal epid As String)
        Try
            Dim query As String = "SELECT p.FACILITYSITEID " &
                " , p.EMISSIONSUNITID " &
                " , p.PROCESSID " &
                " , p.POLLUTANTCODE " &
                " , l.STRDESC AS PollutantType " &
                " , CONVERT(decimal(4, 1), p.NUMPCTCTRLMEASURESREDEFFIC) AS ReductionEfficiency " &
                " , p.LASTEISSUBMITDATE " &
                " , c.NUMPCTCTRLAPPROACHCAPEFFIC " &
                " , c.NUMPCTCTRLAPPROACHEFFECT " &
                " , CONVERT(decimal(5, 2), c.NUMPCTCTRLAPPROACHCAPEFFIC * c.NUMPCTCTRLAPPROACHEFFECT * p.NUMPCTCTRLMEASURESREDEFFIC / 10000) AS CalculatedReduction " &
                "FROM EIS_PROCESSCONTROLPOLLUTANT AS p " &
                "LEFT JOIN dbo.EISLK_POLLUTANTCODE AS l " &
                " ON p.POLLUTANTCODE = l.POLLUTANTCODE " &
                " AND l.ACTIVE = '1' " &
                "LEFT JOIN dbo.EIS_PROCESSCONTROLAPPROACH AS c " &
                " ON c.FACILITYSITEID = p.FACILITYSITEID " &
                " AND c.EMISSIONSUNITID = p.EMISSIONSUNITID " &
                " AND c.PROCESSID = p.PROCESSID " &
                " and c.ACTIVE = '1' " &
                "WHERE p.FACILITYSITEID = @fsid " &
                " AND p.EMISSIONSUNITID = @euid " &
                " AND p.PROCESSID = @epid " &
                " AND p.ACTIVE = '1' " &
                "ORDER BY PollutantType "

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid)
            }

            gvwProcessCtrlPollutant.DataSource = DB.GetDataTable(query, params)
            gvwProcessCtrlPollutant.DataBind()

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadcontrolMeasureDDL()
        ddlControlMeasure.Items.Add("--Select a Control Measure --")
        Try
            Dim query As String = "select STRCONTROLMEASURECODE, strdesc FROM EISLK_CONTROLMEASURECODE where Active = '1' order by strDesc"

            Dim dt As DataTable = DB.GetDataTable(query)

            For Each dr As DataRow In dt.Rows
                Dim newListItem As New ListItem With {
                    .Text = dr.Item("strdesc"),
                    .Value = dr.Item("STRCONTROLMEASURECODE")
                }
                ddlControlMeasure.Items.Add(newListItem)
            Next
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadControlPollutantDDL()
        ddlControlPollutants.Items.Add("--Select a Pollutant --")
        Try
            Dim query As String = " select POLLUTANTCODE, STRDESC " &
                "FROM EISLK_POLLUTANTCODE " &
                "where EISLK_POLLUTANTCODE.STRPOLLUTANTTYPE = 'CAP' and EISLK_POLLUTANTCODE.ACTIVE ='1' order by STRDESC"

            Dim dt As DataTable = DB.GetDataTable(query)

            For Each dr As DataRow In dt.Rows
                Dim newListItem As New ListItem With {
                    .Text = dr.Item("strdesc"),
                    .Value = dr.Item("POLLUTANTCODE")
                }
                ddlControlPollutants.Items.Add(newListItem)
            Next
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadProcessControlApproach(ByVal fsid As String, ByVal euid As String, ByVal epid As String)
        Try
            Dim query As String = "select EMISSIONSUNITID, " &
                         "PROCESSID, " &
                        "STRCONTROLAPPROACHDESC, " &
                        "NUMPCTCTRLAPPROACHCAPEFFIC, " &
                        "NUMPCTCTRLAPPROACHEFFECT, " &
                        "INTFIRSTINVENTORYYEAR, " &
                        "INTLASTINVENTORYYEAR, " &
                        "STRCONTROLAPPROACHCOMMENT " &
                        "FROM EIS_PROCESSCONTROLAPPROACH where FACILITYSITEID = @fsid " &
                        "and EMISSIONSUNITID = @euid and ACTIVE = '1'" &
                        "and PROCESSID = @epid "

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@epid", epid)
            }

            Dim dr As DataRow = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                If IsDBNull(dr("EMISSIONSUNITID")) Then
                    txtEmissionUnitID.Text = ""
                Else
                    txtEmissionUnitID.Text = dr.Item("EMISSIONSUNITID")

                End If
                If IsDBNull(dr("PROCESSID")) Then
                    txtProcessID.Text = ""
                Else
                    txtProcessID.Text = dr.Item("PROCESSID")
                End If

                If IsDBNull(dr("STRCONTROLAPPROACHDESC")) Then
                    txtControlApproachDescription.Text = ""
                Else
                    txtControlApproachDescription.Text = dr.Item("STRCONTROLAPPROACHDESC")
                End If
                If IsDBNull(dr("NUMPCTCTRLAPPROACHCAPEFFIC")) Then
                    txtPctCtrlApproachCapEffic.Text = ""
                Else
                    txtPctCtrlApproachCapEffic.Text = dr.Item("NUMPCTCTRLAPPROACHCAPEFFIC")
                End If
                If IsDBNull(dr("NUMPCTCTRLAPPROACHEFFECT")) Then
                    txtPctCtrlApproachEffect.Text = ""
                Else
                    txtPctCtrlApproachEffect.Text = dr.Item("NUMPCTCTRLAPPROACHEFFECT")
                End If

                If IsDBNull(dr("STRCONTROLAPPROACHCOMMENT")) Then
                    txtControlApproachComment.Text = ""
                Else
                    txtControlApproachComment.Text = dr.Item("STRCONTROLAPPROACHCOMMENT")
                End If

            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        saveControlApproach()
        lblMessageTop.Text = "Process control approach info saved succesfully"
        lblMessageTop.Visible = True

    End Sub

    Private Sub saveControlApproach()
        Dim eisAirsNumber As String = GetCookie(Cookie.AirsNumber)
        Dim emissionunitid As String = txtEmissionUnitID.Text
        Dim processID As String = txtProcessID.Text
        Dim ControlApproachDescription As String = txtControlApproachDescription.Text
        Dim ConAppCaptureEff As String = txtPctCtrlApproachCapEffic.Text
        Dim conAppEffective As String = txtPctCtrlApproachEffect.Text
        Dim conAppComment As String = txtControlApproachComment.Text

        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        'Truncate comment if >400 chars
        If Len(conAppComment) > 400 Then
            conAppComment = Left(conAppComment, 400)
        End If

        Try
            Dim query As String = "Update EIS_PROCESSCONTROLAPPROACH " &
                " Set EIS_PROCESSCONTROLAPPROACH.STRCONTROLAPPROACHDESC = @ControlApproachDescription, " &
                " EIS_PROCESSCONTROLAPPROACH.NUMPCTCTRLAPPROACHCAPEFFIC = @ConAppCaptureEff, " &
                " EIS_PROCESSCONTROLAPPROACH.NUMPCTCTRLAPPROACHEFFECT = @conAppEffective, " &
                " EIS_PROCESSCONTROLAPPROACH.updateuser = @UpdateUser, " &
                " EIS_PROCESSCONTROLAPPROACH.STRCONTROLAPPROACHCOMMENT = @conAppComment, " &
                " UpdateDateTime = getdate() " &
                " where EIS_PROCESSCONTROLAPPROACH.FACILITYSITEID = @eisAirsNumber " &
                " and EIS_PROCESSCONTROLAPPROACH.EMISSIONSUNITID = @emissionunitid " &
                " and EIS_PROCESSCONTROLAPPROACH.ACTIVE = '1' " &
                " and EIS_PROCESSCONTROLAPPROACH.PROCESSID = @processID "

            Dim params As SqlParameter() = {
                New SqlParameter("@ControlApproachDescription", ControlApproachDescription),
                New SqlParameter("@ConAppCaptureEff", If(Not String.IsNullOrEmpty(ConAppCaptureEff), ConAppCaptureEff, Nothing)),
                New SqlParameter("@conAppEffective", If(Not String.IsNullOrEmpty(conAppEffective), conAppEffective, Nothing)),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@conAppComment", conAppComment),
                New SqlParameter("@eisAirsNumber", eisAirsNumber),
                New SqlParameter("@emissionunitid", emissionunitid),
                New SqlParameter("@processID", processID)
            }

            DB.RunCommand(query, params)

            LoadProcessControlPollutantsDGV(eisAirsNumber, emissionunitid, processID)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnAddControlMeasure_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddControlMeasure.Click
        lblProcessControlMeasureWarning.Visible = False
        lblMessageTop.Visible = False
        insertControlMeasure()
    End Sub

    Private Sub insertControlMeasure()

        Dim Active As Integer = "1"
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim emissionunitid As String = txtEmissionUnitID.Text
        Dim processID As String = txtProcessID.Text
        Dim CMcode As String = ddlControlMeasure.SelectedValue

        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        lblProcessControlMeasureWarning.Visible = "False"

        Try
            Dim query As String = "Select STRCONTROLMEASURECODE FROM EIS_PROCESSCONTROLMEASURE " &
                " where EIS_PROCESSCONTROLMEASURE.FACILITYSITEID = @FacilitySiteID " &
                " and EIS_PROCESSCONTROLMEASURE.EMISSIONSUNITID = @emissionunitid " &
                " and EIS_PROCESSCONTROLMEASURE.PROCESSID = @processID " &
                " and EIS_PROCESSCONTROLMEASURE.STRCONTROLMEASURECODE = @CMcode " &
                " and EIS_PROCESSCONTROLMEASURE.ACTIVE = '1' "

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@emissionunitid", emissionunitid),
                New SqlParameter("@processID", processID),
                New SqlParameter("@CMcode", If(Not String.IsNullOrEmpty(CMcode), CMcode, Nothing))
            }

            If DB.ValueExists(query, params) Then
                lblProcessControlMeasureWarning.Text = "Please select a new control measure. This one already exists in the Control Approach."
                lblProcessControlMeasureWarning.Visible = "True"
            Else
                query = "Insert Into EIS_PROCESSCONTROLMEASURE (FACILITYSITEID, " &
                            "EMISSIONSUNITID, " &
                            "PROCESSID, " &
                            "STRCONTROLMEASURECODE, " &
                            "ACTIVE, " &
                            "UPDATEUSER, " &
                            "UPDATEDATETIME, " &
                            "CreateDateTime) " &
                        " Values (" &
                            " @FacilitySiteID, " &
                            " @emissionunitid, " &
                            " @processID, " &
                            " @CMcode, " &
                            " @Active, " &
                            " @UpdateUser, " &
                            "getdate() " & ", " &
                            "getdate()) "

                Dim params2 As SqlParameter() = {
                    New SqlParameter("@FacilitySiteID", FacilitySiteID),
                    New SqlParameter("@emissionunitid", emissionunitid),
                    New SqlParameter("@processID", processID),
                    New SqlParameter("@CMcode", If(Not String.IsNullOrEmpty(CMcode), CMcode, Nothing)),
                    New SqlParameter("@Active", Active),
                    New SqlParameter("@UpdateUser", UpdateUser)
                }

                DB.RunCommand(query, params2)

                ddlControlMeasure.SelectedIndex = "0"
            End If

            LoadProcessControlMeasureDGV(FacilitySiteID, emissionunitid, processID)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnAddControlPollutant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddControlPollutant.Click
        lblProcessControlPollutantWarning.Visible = False
        lblMessageTop.Visible = False
        InsertControlPollutant()
    End Sub

    Private Sub InsertControlPollutant()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim emissionunitid As String = txtEmissionUnitID.Text
        Dim processID As String = txtProcessID.Text
        Dim CPcode As String = ddlControlPollutants.SelectedValue
        Dim CMReductEff As Decimal = Decimal.Round(CDec(txtCMReductionEff.Text), 1)

        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        lblProcessControlPollutantWarning.Visible = False

        Try
            Dim _iReturnCode As Integer = InsertUpdateProcessControlPollutant(FacilitySiteID, emissionunitid, CPcode, processID, CMReductEff, UpdateUser, DbChangeMode.Insert)

            If _iReturnCode = 0 Then
                LoadProcessControlPollutantsDGV(FacilitySiteID, emissionunitid, processID)
                ddlControlPollutants.SelectedIndex = 0
                txtCMReductionEff.Text = ""
                lblProcessControlPollutantWarning.Text = "Process Control Pollutant saved successfully."
                lblProcessControlPollutantWarning.Visible = True
            ElseIf _iReturnCode = -20 Then
                lblProcessControlPollutantWarning.Text = "The PM2.5 Control Measure Reduction Efficiency can not be larger than the PM10 Control Measure Reduction Efficiency."
                lblProcessControlPollutantWarning.Visible = True
            ElseIf _iReturnCode = -30 Then
                lblProcessControlPollutantWarning.Text = "Please enter a new pollutant. This one already exists in the Control Approach."
                lblProcessControlPollutantWarning.Visible = True
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    'Delete Process Control Pollutant
    Protected Sub gvwProcessControlMeasure_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwProcessControlMeasure.RowDeleting
        Try
            If gvwProcessControlMeasure.Rows.Count = 1 Then
                lblProcessControlMeasureWarning.Text = "Please add an additional control measure before the only control measure in the control approach can be deleted."
                lblProcessControlMeasureWarning.Visible = True
                Exit Sub
            Else
                Dim FacilitySiteID As String = gvwProcessControlMeasure.DataKeys(e.RowIndex).Values(0).ToString
                Dim EmissionsUnitID As String = gvwProcessControlMeasure.DataKeys(e.RowIndex).Values(1).ToString
                Dim ProcessID As String = gvwProcessControlMeasure.DataKeys(e.RowIndex).Values(2).ToString
                Dim MeasureCode As String = gvwProcessControlMeasure.DataKeys(e.RowIndex).Values(3).ToString

                Dim query As String = "Delete FROM  EIS_PROCESSCONTROLMEASURE " &
                    "where " &
                    "FACILITYSITEID = @FacilitySiteID and " &
                    "EMISSIONSUNITID = @EmissionsUnitID and " &
                    "PROCESSID = @ProcessID and " &
                    "STRCONTROLMEASURECODE = @MeasureCode "

                Dim params As SqlParameter() = {
                    New SqlParameter("@FacilitySiteID", FacilitySiteID),
                    New SqlParameter("@EmissionsUnitID", EmissionsUnitID),
                    New SqlParameter("@ProcessID", ProcessID),
                    New SqlParameter("@MeasureCode", MeasureCode)
                }

                DB.RunCommand(query, params)

                LoadProcessControlMeasureDGV(FacilitySiteID, EmissionsUnitID, ProcessID)
            End If
            ddlControlMeasure.SelectedIndex = 0
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub
    'Reload gridview after delete of single control measure
    Protected Sub gvwProcessControlMeasure_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles gvwProcessControlMeasure.RowDeleted
        Dim FacilitySiteID As String = e.Keys(0).ToString
        Dim emissionunitid As String = e.Keys(1).ToString
        Dim processID As String = e.Keys(2).ToString
        LoadProcessControlMeasureDGV(FacilitySiteID, emissionunitid, processID)
        ddlControlMeasure.SelectedIndex = 0
    End Sub

    'Delete Process Control Pollutant
    Protected Sub gvwProcessCtrlPollutant_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwProcessCtrlPollutant.RowDeleting
        Try
            If gvwProcessCtrlPollutant.Rows.Count = 1 Then
                lblProcessControlPollutantWarning.Text = "Please add an additional control pollutant before the only control pollutant in the control approach can be deleted."
                lblProcessControlPollutantWarning.Visible = True
                Exit Sub
            Else
                Dim FacilitySiteID As String = gvwProcessCtrlPollutant.DataKeys(e.RowIndex).Values(0).ToString
                Dim EmissionsUnitID As String = gvwProcessCtrlPollutant.DataKeys(e.RowIndex).Values(1).ToString
                Dim ProcessID As String = gvwProcessCtrlPollutant.DataKeys(e.RowIndex).Values(2).ToString
                Dim PollutantCode As String = gvwProcessCtrlPollutant.DataKeys(e.RowIndex).Values(3).ToString

                Dim query As String = "Delete FROM  EIS_PROCESSCONTROLPOLLUTANT " &
                    "where " &
                    "FACILITYSITEID = @FacilitySiteID and " &
                    "EMISSIONSUNITID = @EmissionsUnitID and " &
                    "PROCESSID = @ProcessID and " &
                    "pollutantcode = @PollutantCode "

                Dim params As SqlParameter() = {
                    New SqlParameter("@FacilitySiteID", FacilitySiteID),
                    New SqlParameter("@EmissionsUnitID", EmissionsUnitID),
                    New SqlParameter("@ProcessID", ProcessID),
                    New SqlParameter("@PollutantCode", PollutantCode)
                }

                DB.RunCommand(Query, params)

                LoadProcessControlPollutantsDGV(FacilitySiteID, EmissionsUnitID, ProcessID)
                ClearProcessCtrlDetails()
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    'Re-direct to Process Details
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim eu As String = txtEmissionUnitID.Text
        Dim ep As String = txtProcessID.Text
        Dim targetpage As String = "process_details.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)
    End Sub
    'Re-direct to Process Details
    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click
        Dim eu As String = txtEmissionUnitID.Text
        Dim ep As String = txtProcessID.Text
        Dim targetpage As String = "process_details.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)
    End Sub
    'Delete Entire Control Approach
    Protected Sub btnDeleteOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteOK.Click
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper

        DeleteAllProcessControlMeasures(FacilitySiteID, EmissionsUnitID, ProcessID)
        DeleteAllProcessControlPollutants(FacilitySiteID, EmissionsUnitID, ProcessID)
        DeleteProcessControlApproach(FacilitySiteID, EmissionsUnitID, ProcessID)
        lblDeleteProcessCA.Text = "The Process Control Approach, Pollutants and Measures have been deleted."
        btnDeleteCancel.Visible = False
        btnDeleteOK.Visible = False
        btnDeleteDetails.Visible = True
        mpeDelete.Show()
    End Sub

    Protected Sub btnDeleteDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteDetails.Click
        Dim eu As String = txtEmissionUnitID.Text
        Dim ep As String = txtProcessID.Text
        Dim targetpage As String = "process_details.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)
    End Sub

    'Edit the Control Pollutant Efficiency
    Protected Sub gvwProcessCtrlPollutant_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwProcessCtrlPollutant.RowEditing
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        gvwProcessCtrlPollutant.EditIndex = e.NewEditIndex
        LoadProcessControlPollutantsDGV(FacilitySiteID, EmissionUnitID, ProcessID)
    End Sub
    'Update the Control Pollutant Efficiency
    Protected Sub gvwProcessCtrlPollutant_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvwProcessCtrlPollutant.RowUpdating
        Try
            Dim ReductionEfficiency As Decimal = Decimal.Round(CDec(DirectCast(gvwProcessCtrlPollutant.Rows(e.RowIndex).FindControl("txtReductionEfficiency"), TextBox).Text), 1)
            Dim FacilitySiteID As String = gvwProcessCtrlPollutant.DataKeys(e.RowIndex).Values(0).ToString
            Dim EmissionsUnitID As String = gvwProcessCtrlPollutant.DataKeys(e.RowIndex).Values(1).ToString
            Dim ProcessID As String = gvwProcessCtrlPollutant.DataKeys(e.RowIndex).Values(2).ToString
            Dim PollutantCode As String = gvwProcessCtrlPollutant.DataKeys(e.RowIndex).Values(3).ToString

            Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
            Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
            Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

            lblProcessControlPollutantWarning.Visible = False

            Dim _iReturnCode As Integer = InsertUpdateProcessControlPollutant(FacilitySiteID, EmissionsUnitID, PollutantCode, ProcessID, ReductionEfficiency, UpdateUser, DbChangeMode.Update)

            If _iReturnCode = 0 Then
                gvwProcessCtrlPollutant.EditIndex = -1
                LoadProcessControlPollutantsDGV(FacilitySiteID, EmissionsUnitID, ProcessID)
                ClearProcessCtrlDetails()
            ElseIf _iReturnCode = -20 Then
                Dim row As GridViewRow = gvwProcessCtrlPollutant.Rows(e.RowIndex)
                Dim label As Label = TryCast(row.FindControl("lblError"), Label)
                label.Text = "*"
                label.Visible = True
                lblProcessControlPollutantWarning.Text = "The PM2.5 Control Measure Reduction Efficiency can not be larger than the PM10 Control Measure Reduction Efficiency."
                lblProcessControlPollutantWarning.Visible = True
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub
    'Cancels the Edit without updating the database
    Protected Sub gvwProcessCtrlPollutant_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvwProcessCtrlPollutant.RowCancelingEdit

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        gvwProcessCtrlPollutant.EditIndex = -1
        LoadProcessControlPollutantsDGV(FacilitySiteID, EmissionsUnitID, ProcessID)

    End Sub

    Private Sub ClearProcessCtrlDetails()

        ddlControlPollutants.SelectedIndex = 0
        txtCMReductionEff.Text = ""

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