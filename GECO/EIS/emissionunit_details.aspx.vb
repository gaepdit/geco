Imports System.Data.SqlClient

Partial Class eis_emissionunit_details
    Inherits Page

    Private ElecGen As Boolean
    Private FuelBurning As Boolean
    Private UnitStatusCode As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EmissionsUnitID As String = Request.QueryString("eu").ToUpper
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EUCtrlApproachExist As Boolean
        Dim ProcCtrlApproachExist As Boolean

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            LoadEmissionUnitDetails(FacilitySiteID, EmissionsUnitID)
            LoadProcessGVW(FacilitySiteID, EmissionsUnitID)
            EUCtrlApproachExist = CheckEUControlApproachAny(FacilitySiteID, EmissionsUnitID)
            ProcCtrlApproachExist = CheckProcCtrlApproachAny(FacilitySiteID, EmissionsUnitID)

            If EUCtrlApproachExist Then
                pnlUnitControlApproach.Visible = True
                lblProcessWarning.Text = "Note: Emission Unit Control Approach exists."
                lblProcessWarning.ForeColor = Drawing.Color.ForestGreen
                LoadUnitCAMeasuresGVW(FacilitySiteID, EmissionsUnitID)
                LoadUnitCAPollutantsGVW(FacilitySiteID, EmissionsUnitID)
                LoadUnitCtrlApprDetails(FacilitySiteID, EmissionsUnitID)
                If gvwUnitControlMeasure.Rows.Count = 0 Then
                    lblUnitControlMeasureWarning.Text = "No Process Control Measure exists."
                End If
                If gvwUnitControlPollutant.Rows.Count = 0 Then
                    lblUnitControlPollutantWarning.Text = "No Process Control Pollutant exists."
                End If
            Else
                If ProcCtrlApproachExist Then
                    lblUnitCtrlApprWarning.Text = "Note: Process Control Approach exists."
                    lblUnitCtrlApprWarning.ForeColor = Drawing.Color.ForestGreen
                    pnlUnitControlApproach.Visible = False
                Else
                    lblUnitCtrlApprWarning.Text = "No Emission Unit Control Approach Exists."
                    lblUnitCtrlApprWarning.ForeColor = Drawing.Color.ForestGreen
                    pnlUnitControlApproach.Visible = False
                End If
            End If

            HideTextBoxBorders(Me)
        End If

    End Sub

    Private Sub LoadEmissionUnitDetails(ByVal fsid As String, ByVal euid As String)
        Dim UpdateUser As String
        Dim UpdateDateTime As String
        Dim UnitDesignCapacity As Decimal
        Dim MaxNameplateCapacity As Decimal
        Dim UnitTypeCode As String = ""
        Dim unitDesignCapacityUOMCode As String

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
                    If UnitDesignCapacity = -1 OrElse UnitDesignCapacity = 0 Then
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
                    If MaxNameplateCapacity = -1 OrElse MaxNameplateCapacity = 0 Then
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
                pnlElecGenerating.Visible = True
            Else
                lblElecGenerating1.Text = "This unit has been identified as non electrical generating."
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

        Dim UpdateUser As String
        Dim UpdateDateTime As String

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

End Class