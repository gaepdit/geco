Imports System.Data.SqlClient

Partial Class eis_emissionunit_details
    Inherits Page

    Public IDExists As Boolean
    Public ElecGen As Boolean
    Public FuelBurning As Boolean
    Public UnitStatusCode As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EmissionsUnitID As String = Request.QueryString("eu").ToUpper
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EUCtrlApproachExist As Boolean
        Dim ProcCtrlApproachExist As Boolean

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            LoadEmissionUnitDetails(FacilitySiteID, EmissionsUnitID)
            LoadProcessGVW(FacilitySiteID, EmissionsUnitID)
            LoadReleasePointDDL(FacilitySiteID)
            EUCtrlApproachExist = CheckEUControlApproachAny(FacilitySiteID, EmissionsUnitID)
            ProcCtrlApproachExist = CheckProcCtrlApproachAny(FacilitySiteID, EmissionsUnitID)
            txtNewProcessIDEmissionsUnit.Text = txtEmissionUnitID.Text

            If UnitStatusCode <> "OP" Then
                btnAddProcess.Visible = False
                btnAddControlApproach.Visible = False
                btnEditControlApproach.Visible = False
            Else
                btnAddProcess.Visible = True
                btnAddControlApproach.Visible = True
                btnEditControlApproach.Visible = True
            End If

            If EUCtrlApproachExist Then
                pnlUnitControlApproach.Visible = True
                btnAddControlApproach.Visible = False
                btnEditControlApproach.Visible = True
                lblProcessWarning.Text = "Note: Emission Unit Control Approach exists; No Process Control Approaches can be added."
                lblProcessWarning.ForeColor = Drawing.Color.ForestGreen
                LoadUnitCAMeasuresGVW(FacilitySiteID, EmissionsUnitID)
                LoadUnitCAPollutantsGVW(FacilitySiteID, EmissionsUnitID)
                LoadUnitCtrlApprDetails(FacilitySiteID, EmissionsUnitID)
                If gvwUnitControlMeasure.Rows.Count = 0 Then
                    lblUnitControlMeasureWarning.Text = "At least one Process Control Measure must be added to the Process Control Approach."
                End If
                If gvwUnitControlPollutant.Rows.Count = 0 Then
                    lblUnitControlPollutantWarning.Text = "At least one Process Control Pollutant must be added to the Process Control Approach."
                End If
            Else
                If ProcCtrlApproachExist Then
                    lblUnitCtrlApprWarning.Text = "Note: Process Control Approach exists; cannot add Emission Unit Control Approach."
                    lblUnitCtrlApprWarning.ForeColor = Drawing.Color.ForestGreen
                    pnlUnitControlApproach.Visible = False
                    btnAddControlApproach.Visible = False
                    btnEditControlApproach.Visible = False
                Else
                    lblUnitCtrlApprWarning.Text = "No Emission Unit Control Approach Exists. Click Add to create one for this Emission Unit."
                    txtCtrlApprEUID.Text = txtEmissionUnitID.Text
                    lblUnitCtrlApprWarning.ForeColor = Drawing.Color.ForestGreen
                    pnlUnitControlApproach.Visible = False
                    btnAddControlApproach.Visible = True
                    btnEditControlApproach.Visible = False
                End If
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

#Region "  Page Procedures  "

    'Load Release Point ddl
    Private Sub LoadReleasePointDDL(ByVal fsid As String)

        ddlReleasePointID.Items.Add("--Select Release Point--")

        Try
            Dim query = " select ReleasePointID, strRPDescription, strRPStatusCode " &
                " from " &
                " eis_ReleasePoint " &
                " where " &
                " Active = '1' and " &
                " FacilitySiteID = @fsid " &
                " and strRPStatusCode = 'OP' " &
                " Order by strRPDescription"

            Dim param As New SqlParameter("@fsid", fsid)

            Dim dt = DB.GetDataTable(query, param)

            If dt IsNot Nothing Then
                For Each dr In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("ReleasePointID") & " - " & dr.Item("strRPDescription"),
                        .Value = dr.Item("ReleasePointID")
                    }
                    ddlReleasePointID.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadEmissionUnitDetails(ByVal fsid As String, ByVal euid As String)
        Dim UpdateUser As String = ""
        Dim UpdateDateTime As String = ""
        Dim UnitDesignCapacity As Decimal
        Dim MaxNameplateCapacity As Decimal
        Dim UnitTypeCode As String = ""
        Dim unitDesignCapacityUOMCode As String = ""

        Try
            Dim query = "select " &
                " EmissionsUnitID, " &
                " strUnitDescription, " &
                " strUnitTypeCode, " &
                " strUnitStatusCode, " &
                " fltUnitDesignCapacity, " &
                " strUnitDesignCapacityUOMCode, " &
                " convert(char, datUnitOperationDate, 101) As datUnitOperationDate, " &
                " strUnitComment, " &
                " numMaximumNameplateCapacity, " &
                " convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                " UpdateUser, " &
                " convert(char, UpdateDateTime, 20) As UpdateDateTime " &
                " from " &
                " EIS_EmissionsUnit " &
                " where FacilitySiteID = @fsid and " &
                " EmissionsUnitID = @euid "

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid)
            }

            Dim dr = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                If IsDBNull(dr("EmissionsUnitID")) Then
                    txtEmissionUnitID.Text = ""
                Else
                    txtEmissionUnitID.Text = dr.Item("EmissionsUnitID")
                End If
                If IsDBNull(dr("strUnitDescription")) Then
                    txtUnitDescription.Text = ""
                Else
                    txtUnitDescription.Text = dr.Item("strUnitDescription")
                End If
                If IsDBNull(dr("strUnitTypeCode")) Then
                    txtUnitTypeCode.Text = ""
                Else
                    UnitTypeCode = dr.Item("strUnitTypeCode")
                    txtUnitTypeCode.Text = GetUnitTypeCodeDesc(UnitTypeCode)
                End If
                If IsDBNull(dr("strUnitStatusCode")) Then
                    txtUnitStatusDesc.Text = ""
                Else
                    UnitStatusCode = dr.Item("strUnitStatusCode")
                    txtUnitStatusDesc.Text = GetUnitStatusCodeDesc(UnitStatusCode)
                End If
                If IsDBNull(dr("datUnitOperationDate")) Then
                    txtUnitOperationDate.Text = ""
                Else
                    txtUnitOperationDate.Text = dr.Item("datUnitOperationDate")
                End If
                If IsDBNull(dr("fltUnitDesignCapacity")) Then
                    txtUnitDesignCapacity.Text = ""
                Else
                    UnitDesignCapacity = dr.Item("fltUnitDesignCapacity")
                    If UnitDesignCapacity = -1 Or UnitDesignCapacity = 0 Then
                        txtUnitDesignCapacity.Text = ""
                    Else
                        txtUnitDesignCapacity.Text = UnitDesignCapacity
                    End If
                End If
                If IsDBNull(dr("strUnitDesignCapacityUOMCode")) Then
                    txtUnitDesignCapacity.Text = ""
                Else
                    unitDesignCapacityUOMCode = dr.Item("strUnitDesignCapacityUOMCode")
                    If txtUnitDesignCapacity.Text = "" Then
                        txtUnitDesignCapacity.Text = ""
                    Else
                        txtUnitDesignCapacity.Text = txtUnitDesignCapacity.Text & " " & GetUnitDesCapUOMCodeDesc(unitDesignCapacityUOMCode)
                    End If
                End If

                If IsDBNull(dr("numMaximumNameplateCapacity")) Then
                    txtMaxNameplateCapacity.Text = ""
                    ElecGen = False
                Else
                    MaxNameplateCapacity = dr.Item("numMaximumNameplateCapacity")
                    If MaxNameplateCapacity = -1 Or MaxNameplateCapacity = 0 Then
                        txtMaxNameplateCapacity.Text = ""
                        ElecGen = False
                    Else
                        txtMaxNameplateCapacity.Text = MaxNameplateCapacity & " MW"
                        ElecGen = True
                    End If
                End If

                If IsDBNull(dr("strUnitComment")) Then
                    txtUnitComment.Text = ""
                    txtUnitComment.Visible = False
                Else
                    txtUnitComment.Text = dr.Item("strUnitComment")
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
                    UpdateDateTime = CType(dr.Item("UpdateDateTime"), DateTime).ToShortDateString
                End If

                SetFuelandElec(UnitTypeCode)
                txtEULastUpdated.Text = UpdateDateTime & " by " & UpdateUser
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub SetFuelandElec(ByVal UnitTypeCode As String)

        FuelBurning = UnitIsFuelBurning(UnitTypeCode)

        If FuelBurning Then
            pnlFuelBurning.Visible = True
            If ElecGen Then
                lblElecGenerating1.Text = "This unit has been identified as an electrical generating unit."
                lblElecGenerating2.Text = "To change click the Edit button."
                pnlElecGenerating.Visible = True
            Else
                lblElecGenerating1.Text = "This unit has been identified as non electrical generating."
                lblElecGenerating2.Text = "To change click the Edit button."
                pnlElecGenerating.Visible = False
            End If
        Else
            pnlFuelBurning.Visible = False
            pnlElecGenerating.Visible = False
            lblElecGenerating1.Text = ""
            lblElecGenerating2.Text = ""
        End If

    End Sub

    Private Sub LoadUnitCtrlApprDetails(ByVal fsid As String, ByVal euid As String)

        Dim UpdateUser As String = ""
        Dim UpdateDateTime As String = ""

        Try
            Dim query = "select " &
                "EmissionsUnitID, " &
                "strControlApproachDescription, " &
                "numPctCtrlApproachCapEffic, " &
                "numPctCtrlApproachEffect, " &
                "intFirstInventoryYear, " &
                "intLastInventoryYear, " &
                "strControlApproachComment, " &
                "convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                "UpdateUser, " &
                "convert(char, UpdateDateTime, 20) As UpdateDateTime " &
                "from " &
                "EIS_UnitControlApproach " &
                "where FacilitySiteID = @fsid and " &
                "EmissionsUnitID = @euid and " &
                "Active = '1'"

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid)
            }

            Dim dr = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then
                If IsDBNull(dr("EmissionsUnitID")) Then
                    txtEmissionUnitID.Text = ""
                Else
                    txtEmissionUnitID.Text = dr.Item("EmissionsUnitID")
                End If
                If IsDBNull(dr("strControlApproachDescription")) Then
                    txtControlApproachDescription.Text = ""
                Else
                    txtControlApproachDescription.Text = dr.Item("strControlApproachDescription")
                End If
                If IsDBNull(dr("numPctCtrlApproachCapEffic")) Then
                    txtPctCtrlApproachCapEffic.Text = ""
                Else
                    txtPctCtrlApproachCapEffic.Text = dr.Item("numPctCtrlApproachCapEffic")
                End If
                If IsDBNull(dr("numPctCtrlApproachEffect")) Then
                    txtPctCtrlApproachEffect.Text = ""
                Else
                    txtPctCtrlApproachEffect.Text = dr.Item("numPctCtrlApproachEffect")
                End If
                If IsDBNull(dr("intFirstInventoryYear")) Then
                    txtFirstInventoryYear.Text = ""
                Else
                    txtFirstInventoryYear.Text = dr.Item("intFirstInventoryYear")
                End If
                If IsDBNull(dr("intLastInventoryYear")) Then
                    txtLastInventoryYear.Text = ""
                Else
                    txtLastInventoryYear.Text = dr.Item("intLastInventoryYear")
                End If
                If IsDBNull(dr("strControlApproachComment")) Then
                    txtControlApproachComment.Text = ""
                    txtControlApproachComment.Visible = False
                Else
                    txtControlApproachComment.Text = dr.Item("strControlApproachComment")
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
                    UpdateDateTime = CType(dr.Item("UpdateDateTime"), DateTime).ToShortDateString()
                End If

                txtEUCtrlApprLastUpdated.Text = UpdateDateTime & " by " & UpdateUser
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub LoadUnitCAMeasuresGVW(ByVal fsid As String, ByVal euid As String)
        Dim query = "select " &
            " EIS_UNITCONTROLMEASURE.FACILITYSITEID, " &
            " EIS_UNITCONTROLMEASURE.EMISSIONSUNITID, " &
            " EIS_UNITCONTROLMEASURE.STRCONTROLMEASURECODE as MeasureCode, " &
            " EISLK_CONTROLMEASURECODE.STRDESC as CDType, " &
            " EIS_UNITCONTROLMEASURE.LastEISSubmitDate " &
            " FROM EIS_UNITCONTROLMEASURE, EISLK_CONTROLMEASURECODE " &
            " where EIS_UNITCONTROLMEASURE.FACILITYSITEID = @fsid " &
            " and EIS_UNITCONTROLMEASURE.EMISSIONSUNITID = @euid " &
            " and EIS_UNITCONTROLMEASURE.ACTIVE = '1' " &
            " and EISLK_CONTROLMEASURECODE.Active = '1' " &
            " and EIS_UNITCONTROLMEASURE.STRCONTROLMEASURECODE = EISLK_CONTROLMEASURECODE.STRCONTROLMEASURECODE " &
            " order by CDType "

        Dim params = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        gvwUnitControlMeasure.DataSource = DB.GetDataTable(query, params)
        gvwUnitControlMeasure.DataBind()
    End Sub

    Private Sub LoadUnitCAPollutantsGVW(ByVal fsid As String, ByVal euid As String)
        Dim query = "SELECT p.FACILITYSITEID " &
            " , p.EMISSIONSUNITID " &
            " , p.POLLUTANTCODE " &
            " , l.STRDESC AS PollutantType " &
            " , CASE " &
            " WHEN p.NUMPCTCTRLMEASURESREDEFFIC IS NOT NULL " &
            " THEN concat(CONVERT(decimal(4, 1), p.NUMPCTCTRLMEASURESREDEFFIC), '%') " &
            " ELSE NULL " &
            " END AS MeasureEfficiency " &
            " , CONVERT(char, p.LASTEISSUBMITDATE, 101) AS LASTEISSUBMITDATE " &
            " , CASE " &
            " WHEN c.NUMPCTCTRLAPPROACHCAPEFFIC IS NOT NULL " &
            " AND c.NUMPCTCTRLAPPROACHEFFECT IS NOT NULL " &
            " AND p.NUMPCTCTRLMEASURESREDEFFIC IS NOT NULL " &
            " THEN concat(CONVERT(decimal(5, 2), c.NUMPCTCTRLAPPROACHCAPEFFIC * c.NUMPCTCTRLAPPROACHEFFECT * p.NUMPCTCTRLMEASURESREDEFFIC / 10000), '%') " &
            " ELSE NULL " &
            " END AS CalculatedReduction " &
            "FROM EIS_UNITCONTROLPOLLUTANT AS p " &
            "LEFT JOIN EISLK_POLLUTANTCODE AS l " &
            " ON p.POLLUTANTCODE = l.POLLUTANTCODE " &
            " AND l.ACTIVE = '1' " &
            "LEFT JOIN dbo.EIS_UNITCONTROLAPPROACH AS c " &
            " ON c.FACILITYSITEID = p.FACILITYSITEID " &
            " AND c.EMISSIONSUNITID = p.EMISSIONSUNITID " &
            " AND c.ACTIVE = '1' " &
            "WHERE p.FACILITYSITEID = @fsid " &
            " AND p.EMISSIONSUNITID = @euid " &
            " AND p.ACTIVE = '1' " &
            "ORDER BY PollutantType "

        Dim params = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        gvwUnitControlPollutant.DataSource = DB.GetDataTable(query, params)
        gvwUnitControlPollutant.DataBind()
    End Sub

    Private Sub LoadProcessGVW(ByVal fsid As String, ByVal euid As String)
        Dim query = "SELECT p.EmissionsUnitID, p.ProcessID, " &
            " p.strProcessDescription, p.SourceClassCode, p.LastEISSubmitDate, " &
            " CASE WHEN a.EmissionsUnitID IS NULL THEN 'No' ELSE 'Yes' END AS ControlApproach " &
            " FROM EIS_Process AS p " &
            " LEFT JOIN eis_ProcessControlApproach AS a  " &
            " ON p.FacilitySiteID = a.FacilitySiteID  " &
            " AND p.FacilitySiteID = a.FacilitySiteID  " &
            " AND p.EmissionsUnitID = a.EmissionsUnitID  " &
            " AND p.ProcessID = a.ProcessID " &
            " AND a.ACTIVE = '1' " &
            " WHERE p.Active = '1' AND p.FacilitySiteID = @fsid " &
            " AND p.EmissionsUnitID = @euid " &
            " ORDER BY p.EmissionsUnitID"

        Dim params = {
            New SqlParameter("@fsid", fsid),
            New SqlParameter("@euid", euid)
        }

        gvwProcesses.DataSource = DB.GetDataTable(query, params)
        gvwProcesses.DataBind()
    End Sub

    Protected Sub btnEditEmissionUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditEmissionUnit.Click

        Dim emissionuintid As String = txtEmissionUnitID.Text
        Dim targetpage As String = "~/EIS/emissionunit_edit.aspx" & "?eu=" & emissionuintid

        Response.Redirect(targetpage)

    End Sub

    Protected Sub btnEditControlApproach_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEditControlApproach.Click

        Dim emissionuintid As String = txtEmissionUnitID.Text.ToUpper
        Dim targetpage As String = "~/EIS/eucontrolapproach_edit.aspx" & "?eu=" & emissionuintid

        Response.Redirect(targetpage)

    End Sub

#End Region

#Region "  Add Or Duplicate New Emission Unit Panel  "

    Protected Sub EmissionsUnitIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)
        'Checks Emission Unit ID when adding new emission unit in panel

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = args.Value.ToUpper
        Dim targetpage As String = "~/EIS/emissionunit_edit.aspx" & "?eu=" & EmissionsUnitID
        Dim EUIDActive = CheckEUIDExist(FacilitySiteID, EmissionsUnitID)

        Select Case EUIDActive
            Case UnitActiveStatus.Inactive
                args.IsValid = True
                Response.Redirect(targetpage)
            Case UnitActiveStatus.Active
                args.IsValid = False
                cusvEmissionUnitID.ErrorMessage = " Emission Unit " + EmissionsUnitID + " is already in use.  Please enter another."
                txtNewEmissionsUnitID.Text = ""
                btnAddEmissionUnit_ModalPopupExtender.Show()
            Case UnitActiveStatus.DoesNotExist
                args.IsValid = True
                InsertEmissionUnit(FacilitySiteID, EmissionsUnitID)
                Response.Redirect(targetpage)
        End Select

    End Sub

    Protected Sub EmissionsUnitDupIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)
        'Checks Emission Unit ID when adding new emission unit in panel

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = args.Value.ToUpper
        Dim targetpage As String = "~/EIS/emissionunit_edit.aspx" & "?eu=" & EmissionsUnitID
        Dim EUIDActive = CheckEUIDExist(FacilitySiteID, EmissionsUnitID)

        Select Case EUIDActive
            Case UnitActiveStatus.Inactive
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Emission Unit " + EmissionsUnitID + " already exists but is in a deleted state. Enter another ID or go to the Summary page to undelete."
                txtDupEmissionsUnitID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case UnitActiveStatus.Active
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Emission Unit " + EmissionsUnitID + " is already in use.  Please enter another."
                txtDupEmissionsUnitID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case UnitActiveStatus.DoesNotExist
                args.IsValid = True
                DuplicateEmissionUnit(FacilitySiteID, txtDupEmissionsUnitID.Text.ToUpper)
                Response.Redirect(targetpage)
        End Select

    End Sub

    Private Sub InsertEmissionUnit(ByVal fsid As String, ByVal euid As String)

        'Code to insert a new emission unit
        'Reminder: insert only FacilitySiteID, Unit/Stack/Etc ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim UnitDescription As String = Left(txtNewEmissionUnitDesc.Text, 100)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Try
            Dim query = "Insert into eis_EmissionsUnit (" &
                " FacilitySiteID, " &
                " EmissionsUnitID, " &
                " strUnitDescription, " &
                " strUnitStatusCode, " &
                " fltUnitDesignCapacity, " &
                " Active, " &
                " UpdateUser, " &
                " UpdateDateTime, " &
                " CreateDateTime) " &
                " Values (" &
                " @fsid, " &
                " @euid, " &
                " @UnitDescription, " &
                " 'OP', " &
                " null, " &
                " '1', " &
                " @UpdateUser, " &
                " getdate(), " &
                " getdate() ) "

            Dim params = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@UnitDescription", UnitDescription),
                New SqlParameter("@UpdateUser", UpdateUser)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub DuplicateEmissionUnit(ByVal fsid As String, ByVal euid As String)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Try
            'Get data for source unit
            Dim query = "INSERT INTO EIS_EMISSIONSUNIT " &
                " (FacilitySiteID, " &
                " EmissionsUnitID, " &
                " strUnitDescription, " &
                " strUnitTypeCode, " &
                " strUnitStatusCode, " &
                " fltUnitDesignCapacity, " &
                " strUnitDesignCapacityUOMCode, " &
                " numMaximumNameplateCapacity, " &
                " intUnitStatusCodeYear, " &
                " datUnitOperationDate, " &
                " strUnitComment, " &
                " Active, " &
                " UpdateUser, " &
                " UpdateDateTime, " &
                " CreateDateTime) " &
                "    SELECT " &
                "        FACILITYSITEID, " &
                "        @DupEmissionsUnitID, " &
                "        @UnitDescription, " &
                "        STRUNITTYPECODE, " &
                "        'OP', " &
                "        FLTUNITDESIGNCAPACITY, " &
                "        STRUNITDESIGNCAPACITYUOMCODE, " &
                "        NUMMAXIMUMNAMEPLATECAPACITY, " &
                "        @UnitStatusCodeYear, " &
                "        DATUNITOPERATIONDATE, " &
                "        STRUNITCOMMENT, " &
                "        1, " &
                "        @UpdateUser, " &
                "        getdate(), " &
                "        getdate() " &
                "    FROM EIS_EMISSIONSUNIT " &
                "    WHERE FACILITYSITEID = @fsid " &
                "          AND EMISSIONSUNITID = @SourceEmissionsUnitID "

            Dim params = {
                New SqlParameter("@DupEmissionsUnitID", euid.ToUpper),
                New SqlParameter("@UnitDescription", Left(txtDupEUDescription.Text, 100)),
                New SqlParameter("@UnitStatusCodeYear", Now.Year),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@SourceEmissionsUnitID", txtEmissionUnitID.Text)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnCancelEmissionUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelEmissionUnit.Click

        txtNewEmissionsUnitID.Text = ""
        txtNewEmissionUnitDesc.Text = ""
        btnAddEmissionUnit_ModalPopupExtender.Hide()

    End Sub

#End Region

#Region "  Add Unit Approach Control Panel  "

    Protected Sub btnCancelUnitApproachControl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelUnitApproachControl.Click

        txtCtrlApproachDesc.Text = ""
        txtCtrlApprCapEffic.Text = ""
        txtCtrlApprEffect.Text = ""
        btnAddControlApproach_ModalPopupExtender.Hide()

    End Sub

    Protected Sub btnInsertUnitApproachControl_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnInsertUnitApproachControl.Click

        Dim emissionuintid As String = txtCtrlApprEUID.Text
        Dim targetpage As String = "~/EIS/eucontrolapproach_edit.aspx" & "?eu=" & emissionuintid

        InsertControlApproach()
        Response.Redirect(targetpage)

    End Sub

    Private Sub InsertControlApproach()

        'Code to insert a control approach for an emission unit
        'Inserts only FacilitySiteID, Emission Unit ID, EU Control Approach Description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtCtrlApprEUID.Text.ToUpper
        Dim CtrlApproachDesc As String = txtCtrlApproachDesc.Text
        Dim CtrlApprCapEffic As Decimal = txtCtrlApprCapEffic.Text
        Dim CtrlApprEffect As Decimal = txtCtrlApprEffect.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        'Truncate description if >200 chars
        CtrlApproachDesc = Left(CtrlApproachDesc, 200)

        Try
            Dim query = "Insert into eis_UnitControlApproach (" &
                " FacilitySiteID, " &
                " EmissionsUnitID, " &
                " strControlApproachDescription, " &
                " numPctCtrlApproachCapEffic, " &
                " numPctCtrlApproachEffect, " &
                " Active, " &
                " UpdateUser, " &
                " UpdateDateTime, " &
                " CreateDateTime) " &
                " Values (" &
                " @FacilitySiteID, " &
                " @EmissionsUnitID, " &
                " @CtrlApproachDesc, " &
                " @CtrlApprCapEffic, " &
                " @CtrlApprEffect, " &
                " '1', " &
                " @UpdateUser, " &
                " getdate(), " &
                " getdate() ) "

            Dim params = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionsUnitID", EmissionsUnitID),
                New SqlParameter("@CtrlApproachDesc", CtrlApproachDesc),
                New SqlParameter("@CtrlApprCapEffic", CtrlApprCapEffic),
                New SqlParameter("@CtrlApprEffect", CtrlApprEffect),
                New SqlParameter("@UpdateUser", UpdateUser)
            }

            DB.RunCommand(query, params)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

#Region "  Add Process Panel  "

    Private Sub UpdateProcess()

        'Code to update a deleted Process that is being re-used
        'Reminder: insert only FacilitySiteID, Process ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionunitID As String = txtEmissionUnitID.Text.ToUpper
        Dim ProcessID As String = txtNewProcessID.Text.ToUpper
        Dim ReleasePointID As String = ddlReleasePointID.SelectedValue.ToUpper
        Dim ProcessDescription As String = txtNewProcessDesc.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Try
            Dim queryList As New List(Of String)
            Dim paramsList As New List(Of SqlParameter())

            Dim params = {
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EmissionunitID", EmissionunitID),
                New SqlParameter("@ProcessID", ProcessID),
                New SqlParameter("@ReleasePointID", ReleasePointID)
            }

            'Update process in table EIS_PROCESS, change Active = 1
            queryList.Add("Update EIS_PROCESS " &
                " Set Active = 1, " &
                " UPDATEUSER = @UpdateUser, " &
                " UpdateDateTime = getdate() " &
                " where FACILITYSITEID = @FacilitySiteID and " &
                " EmissionsUnitID = @EmissionunitID and " &
                " PROCESSID = @ProcessID ")

            paramsList.Add(params)

            'insert new Release point apportionment into table EIS_RPAPPORTIONMENT
            queryList.Add("Insert into EIS_RPAPPORTIONMENT (" &
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
                "@FacilitySiteID , " &
                "@EmissionunitID , " &
                "@ProcessID      , " &
                "@ReleasePointID , " &
                "'100', " &
                "1, " &
                "(select " &
                "case " &
                "when max(RPAPPORTIONMENTID) is null then 1 " &
                "else max(RPAPPORTIONMENTID) + 1 " &
                "End RPAPPORTIONMENTID " &
                "FROM EIS_RPAPPORTIONMENT), " &
                "@UpdateUser, " &
                "getdate(), " &
                "getdate()) ")

            paramsList.Add(params)

            DB.RunCommand(queryList, paramsList)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub ProcessIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim ProcessID As String = args.Value.ToUpper
        Dim EmissionsUnitID As String = txtNewProcessIDEmissionsUnit.Text.ToUpper
        Dim Targetpage As String = "~/EIS/process_edit.aspx" & "?ep=" & ProcessID & "&eu=" & EmissionsUnitID
        Dim ProcessIDActive = CheckProcessExist(FacilitySiteID, EmissionsUnitID, ProcessID)

        Select Case ProcessIDActive
            Case UnitActiveStatus.Inactive
                args.IsValid = True
                UpdateProcess()
                Response.Redirect(Targetpage)
            Case UnitActiveStatus.Active
                args.IsValid = False
                cusvNewProcessID.ErrorMessage = " Process ID " + ProcessID + " is already in use.  Please enter another."
                txtNewProcessID.Text = ""
                btnAddprocess_ModalPopupExtender.Show()
            Case UnitActiveStatus.DoesNotExist
                args.IsValid = True
                InsertProcess()
                Response.Redirect(Targetpage)
        End Select

    End Sub

    Protected Sub btnAddprocess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddProcess.Click

        'REMINDER - add dropdown for Stack and save to rpapportionment with 100%
        btnAddprocess_ModalPopupExtender.Show()

    End Sub

    Protected Sub btnCancelProcess_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelProcess.Click

        txtNewProcessID.Text = ""
        txtNewProcessDesc.Text = ""
        btnAddprocess_ModalPopupExtender.Hide()

    End Sub

    Private Sub InsertProcess()

        'Code to insert a new Process
        'Reminder: insert only FacilitySiteID, Process ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim NewEmissionUnitID As String = txtEmissionUnitID.Text.ToUpper
        Dim NewProcessID As String = txtNewProcessID.Text.ToUpper
        Dim ReleasePointID As String = ddlReleasePointID.SelectedValue.ToUpper
        Dim ProcessDescription As String = txtNewProcessDesc.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Try
            Dim queryList As New List(Of String)
            Dim paramsList As New List(Of SqlParameter())

            Dim params = {
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@NewEmissionUnitID", NewEmissionUnitID),
                New SqlParameter("@NewProcessID", NewProcessID),
                New SqlParameter("@ProcessDescription", ProcessDescription),
                New SqlParameter("@ReleasePointID", ReleasePointID)
            }

            'Insert new process into talbe EIS_PROCESS
            queryList.Add("Insert into EIS_PROCESS (" &
                "FacilitySiteID, " &
                "EmissionsUnitID, " &
                "PROCESSID, " &
                "STRPROCESSDESCRIPTION, " &
                "Active, " &
                "UpdateUser, " &
                "UpdateDateTime, " &
                "CreateDateTime) " &
                "Values (" &
                "@FacilitySiteID, " &
                "@NewEmissionUnitID, " &
                "@NewProcessID, " &
                "@ProcessDescription, " &
                "1, " &
                "@UpdateUser, " &
                "getdate(), " &
                "getdate()) ")

            paramsList.Add(params)

            'Insert new Release point apportionment into table EIS_RPAPPORTIONMENT
            queryList.Add("Insert into EIS_RPAPPORTIONMENT (" &
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
                "@NewEmissionUnitID, " &
                "@NewProcessID, " &
                "@ReleasePointID, " &
                "100, " &
                "1, " &
                "(select " &
                "case " &
                "when max(RPAPPORTIONMENTID) is null then 1 " &
                "else max(RPAPPORTIONMENTID) + 1 " &
                "End RPAPPORTIONMENTID " &
                "FROM EIS_RPAPPORTIONMENT), " &
                "@UpdateUser, " &
                "getdate(), " &
                "getdate()) ")

            paramsList.Add(params)

            DB.RunCommand(queryList, paramsList)
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

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