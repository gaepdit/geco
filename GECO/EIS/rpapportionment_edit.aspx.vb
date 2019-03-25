Imports System.Data.SqlClient

Partial Class eis_rpapportionment_edit
    Inherits Page
    Public conn, conn1 As New SqlConnection(DBConnectionString)
    Public RPApportionmentExists As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EmissionsUnitID As String = Request.QueryString("eu").ToUpper
        Dim ProcessID As String = Request.QueryString("ep").ToUpper

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then

            RPApportionmentExists = CheckAnyRPApportionment(FacilitySiteID, EmissionsUnitID, ProcessID)
            LoadReleasePoints(FacilitySiteID, EmissionsUnitID)
            LoadRPApportionDetails(FacilitySiteID, EmissionsUnitID, ProcessID)

            If RPApportionmentExists Then
                LoadRPApportionmentGridView(FacilitySiteID, EmissionsUnitID, ProcessID)
                CheckRPApportionmentTotal(FacilitySiteID, EmissionsUnitID, ProcessID)
            Else
                lblRPApportionmentWarning.Text = "No release points added for this process as yet. Please add a release point before continuing."
                lblRPApportionmentTotal.Visible = False
                txtRPApportionmentTotal.Visible = False
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
    End Sub

    Private Sub LoadReleasePoints(ByVal fsid As String, ByVal euid As String)
        Dim sql As String
        Dim RPID As String
        Dim ProcessID As String = Request.QueryString("ep")

        ddlReleasePointID.Items.Clear()
        ddlReleasePointID.Items.Add("--Select a Release Point --")

        'sql statement to select Release Point IDs that are in eis_ReleasePoint and not in eis_RPApprotionment
        Try
            sql = "select ReleasePointID FROM eis_ReleasePoint " &
                            "where " &
                            "eis_ReleasePoint.FacilitySiteID = '" & fsid & "' and " &
                            "eis_ReleasePoint.Active = '1' and " &
                            "eis_ReleasePoint.strRPStatusCode = 'OP' and " &
                            "not exists " &
                            "(select ReleasePointID FROM eis_RPApportionment " &
                            "where " &
                            "eis_RPApportionment.FacilitySiteID = '" & fsid & "' and " &
                            "eis_RPApportionment.EmissionsUnitID = '" & euid & "' and " &
                            "eis_RPApportionment.ProcessID = '" & ProcessID & "' and " &
                            " EIS_RPAPPORTIONMENT.ACTIVE = '1' and " &
                            "eis_RPApportionment.ReleasePointID = eis_ReleasePoint.ReleasePointID) " &
                            "Order by ReleasePointID"

            Dim cmd As New SqlCommand(sql, conn)
            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If
            Dim dr As SqlDataReader = cmd.ExecuteReader
            While dr.Read
                RPID = dr.Item("ReleasePointID")
                ddlReleasePointID.Items.Add(RPID)
            End While
        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub LoadRPApportionDetails(ByVal fsid As String, ByVal euid As String, ByVal prid As String)

        Dim sql1 As String = ""
        Dim sql2 As String = ""

        Try
            sql1 = "select FacilitySiteID, EmissionsUnitID, " &
                        "(Select strUnitDescription FROM eis_EmissionsUnit where " &
                        "eis_EmissionsUnit.FacilitySiteID = '" & fsid & "' and " &
                        "eis_EmissionsUnit.EmissionsUnitID = '" & euid & "') As strUnitDescription, " &
                        "ProcessID, " &
                        "(Select strProcessDescription FROM eis_Process where " &
                        "eis_Process.FacilitySiteID = '" & fsid & "' and " &
                        "eis_Process.EmissionsUnitID = '" & euid & "' and " &
                        "eis_Process.ProcessID = '" & prid & "') As strProcessDescription, " &
                        "ReleasePointID, " &
                        "intAveragepercentEmissions, " &
                        "strRPApportionmentComment " &
                        "FROM eis_RPApportionment " &
                        "where " &
                        "FacilitySiteID = '" & fsid & "' " &
                        "and EmissionsUnitID = '" & euid & "' " &
                        "and ProcessID = '" & prid & "' " &
                        "and Active = '1'"

            Dim cmd1 As New SqlCommand(sql1, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr1 As SqlDataReader = cmd1.ExecuteReader
            Dim recExist As Boolean = dr1.Read

            If recExist Then
                If IsDBNull(dr1("EmissionsUnitID")) Then
                    txtEmissionsUnitID.Text = ""
                Else
                    txtEmissionsUnitID.Text = dr1.Item("EmissionsUnitID")
                End If
                If IsDBNull(dr1("strUnitDescription")) Then
                    txtEmissionsUnitDesc.Text = ""
                Else
                    txtEmissionsUnitDesc.Text = dr1.Item("strUnitDescription")
                End If
                If IsDBNull(dr1("ProcessID")) Then
                    txtProcessID.Text = ""
                Else
                    txtProcessID.Text = dr1.Item("ProcessID")
                End If
                If IsDBNull(dr1("strProcessDescription")) Then
                    txtProcessDesc.Text = ""
                Else
                    txtProcessDesc.Text = dr1.Item("strProcessDescription")
                End If
                If IsDBNull(dr1("strRPApportionmentComment")) Then
                    txtRPApportionmentComment.Text = ""
                Else
                    txtRPApportionmentComment.Text = dr1.Item("strRPApportionmentComment")
                End If
            Else
                txtEmissionsUnitID.Text = euid.ToUpper
                txtEmissionsUnitDesc.Text = GetEmissionUnitDesc(fsid, euid)
                txtProcessID.Text = prid.ToUpper
                txtProcessDesc.Text = GetProcessDesc(fsid, euid, prid)
            End If

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub LoadRPApportionmentGridView(ByVal fsid As String, ByVal euid As String, ByVal prid As String)

        Dim sql As String = ""

        Try
            sql = "SELECT eis_RPApportionment.FacilitySiteID " &
                ", eis_RPApportionment.EmissionsUnitID " &
                ", eis_RPApportionment.ProcessID " &
                ", eis_RPApportionment.ReleasePointID " &
                ", eis_RPApportionment.RPApportionmentID " &
                ", eis_ReleasePoint.strRPDescription " &
                ", eislk_RPTypeCode.strDesc AS RPType " &
                ", eis_RPApportionment.intAveragePercentEmissions AS AvgPctEmissions " &
                ", eis_RPApportionment.strRPApportionmentComment AS RPApportionmentComment " &
                ", CONVERT( char, eis_RPApportionment.LastEISSubmitDate, 101) AS LastEISSubmitDate " &
                "FROM eis_RPApportionment " &
                "INNER JOIN eis_ReleasePoint " &
                "ON eis_RPApportionment.ReleasePointID = eis_ReleasePoint.ReleasePointID " &
                "AND eis_RPApportionment.FacilitySiteID = eis_ReleasePoint.FacilitySiteID " &
                "LEFT JOIN eislk_RPTypeCode " &
                "ON eis_ReleasePoint.strRPTypeCode = eislk_RPTypeCode.RPTypeCode " &
                "WHERE eis_RPApportionment.FacilitySiteID = '" & fsid & "' " &
                "AND eis_RPApportionment.EmissionsUnitID = '" & euid & "' " &
                "AND eis_RPApportionment.ProcessID = '" & prid & "' " &
                "AND eis_RPApportionment.Active = '1' " &
                "AND eislk_RPTypeCode.Active = '1' "

            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            gvwRPApportionment.DataSource = cmd.ExecuteReader
            gvwRPApportionment.DataBind()

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

    Protected Sub btnCancel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcessSummary.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim eu As String = txtEmissionsUnitID.Text.ToUpper
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim targetpage As String = "process_details.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)

    End Sub

    Protected Sub btnCancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel2.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim eu As String = txtEmissionsUnitID.Text.ToUpper
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim targetpage As String = "process_details.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)

    End Sub

    Protected Sub btnAddRPApportionment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddRPApportionment.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionsUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper

        lblRPApportionmentDeleteWarning.Text = ""
        lblRPApportionmentDeleteWarning.Visible = True

        InsertRPApportionment(FacilitySiteID, EmissionsUnitID, ProcessID)
        LoadReleasePoints(FacilitySiteID, EmissionsUnitID)
        CheckRPApportionmentTotal(FacilitySiteID, EmissionsUnitID, ProcessID)
        ClearRPApportionmentDetails()

    End Sub

    Private Sub InsertRPApportionment(ByVal fsid As String, ByVal euid As String, ByVal prid As String)

        'Code to insert a new RPApportion
        'Reminder: insert only FacilitySiteID, process ID, description, UpdateUser, CreateDateTime, UpdateDateTime (same as Create)
        'and any other fields that are required for the insert before going to edit page for more details.
        'Edit pages perform only updates.

        Dim sql1 As String = ""
        Dim sql2 As String = ""
        Dim ReleasePointID As String = ddlReleasePointID.SelectedValue
        Dim AveragePctEmissions As Integer = CInt(txtAvgPctEmissions.Text)
        Dim RPApportioncomment As String = txtRPApportionmentComment.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim Active As String = "1"

        'Truncate comment if >400 chars
        RPApportioncomment = Left(RPApportioncomment, 400)

        Try

            sql2 = "Insert into eis_RPApportionment (" &
                        "FacilitySiteID, " &
                        "EmissionsUnitID, " &
                        "ProcessID, " &
                        "ReleasePointID, " &
                        "RPApportionmentID, " &
                        "intAveragePercentEmissions, " &
                        "strRPApportionmentComment, " &
                        "Active, " &
                        "UpdateUser, " &
                        "UpdateDateTime, " &
                        "CreateDateTime) " &
                "Values (" &
                        "'" & fsid & "', " &
                        "'" & euid & "', " &
                        "'" & prid & "', " &
                        "'" & ReleasePointID & "', " &
                        "Next Value for EIS_SEQ_RPAPPID, " &
                        " " & AveragePctEmissions & ", " &
                        "'" & Replace(RPApportioncomment, "'", "''") & "', " &
                        "'" & Active & "', " &
                        "'" & Replace(UpdateUser, "'", "''") & "', " &
                        "getdate(), " &
                        "getdate()) "

            Dim cmd As New SqlCommand(sql2, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            'Populate gridview
            LoadRPApportionmentGridView(fsid, euid, prid)

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Private Function RPApportiomentTotal(ByVal fsid As String, ByVal euid As String, ByVal prid As String) As Integer

        Dim sql As String = ""
        Dim Result As Integer

        euid = euid.ToUpper
        prid = prid.ToUpper

        Try
            sql = "select sum(intAveragePercentEmissions) As RPApportionmentTotal FROM eis_RPApportionment " &
                  "where " &
                  "FacilitySiteID = '" & fsid & "' and " &
                  "EmissionsUnitID = '" & euid & "' and " &
                  "ProcessID = '" & prid & "' and " &
                  "Active = '1'"

            Dim cmd1 As New SqlCommand(sql, conn1)

            If conn1.State = ConnectionState.Open Then
            Else
                conn1.Open()
            End If

            Dim dr1 As SqlDataReader = cmd1.ExecuteReader
            dr1.Read()
            Result = dr1.Item("RPApportionmentTotal")

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

        Return Result

    End Function

    Private Sub RPApportionmentCheck(ByVal rptotal As Integer)

        If rptotal <> 100 Then
            lblRPApportionmentWarning.Text = "Apportionment total must be 100%. Correct this by editing the values for " &
                                "the average percent emissions or adding another release point and its percentage. The Emissions " &
                                "Inventory submittal check will validate this total before allowing you to submit."
            lblRPApportionmentWarning.BackColor = Drawing.Color.White
            lblRPApportionmentWarning.ForeColor = Drawing.Color.Red
            lblRPApportionmentWarning.Visible = True
        Else
            lblRPApportionmentWarning.Text = ""
            lblRPApportionmentWarning.Visible = False
        End If

    End Sub

    Protected Sub gvwRPApportionment_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvwRPApportionment.RowDeleting

        Try
            If gvwRPApportionment.Rows.Count = 1 Then
                lblRPApportionmentDeleteWarning.Text = "At least one apportionment must exist. Add another before deleting the only one existing."
                lblRPApportionmentDeleteWarning.Visible = True
                Exit Sub
            Else
                Dim FacilitySiteID As String = gvwRPApportionment.DataKeys(e.RowIndex).Values(0).ToString
                Dim EmissionsUnitID As String = gvwRPApportionment.DataKeys(e.RowIndex).Values(1).ToString
                Dim ProcessID As String = gvwRPApportionment.DataKeys(e.RowIndex).Values(2).ToString
                Dim ReleasePointID As String = gvwRPApportionment.DataKeys(e.RowIndex).Values(3).ToString

                lblRPApportionmentDeleteWarning.Text = ""
                lblRPApportionmentDeleteWarning.Visible = True

                Dim sql = "Delete FROM  eis_RPApportionment " &
                "where eis_RPApportionment.FacilitySiteID = '" & FacilitySiteID & "' and " &
                "eis_RPApportionment.EmissionsUnitID = '" & EmissionsUnitID & "' and " &
                "eis_RPApportionment.ProcessID = '" & ProcessID & "' and " &
                "eis_RPApportionment.ReleasePointID = '" & ReleasePointID & "'"

                Dim cmd As New SqlCommand(sql, conn)
                If conn.State = ConnectionState.Open Then
                Else
                    conn.Open()
                End If

                cmd.ExecuteNonQuery()

                If conn.State = ConnectionState.Open Then
                    conn.Close()
                End If

                LoadRPApportionmentGridView(FacilitySiteID, EmissionsUnitID, ProcessID)
                LoadReleasePoints(FacilitySiteID, EmissionsUnitID)
                CheckRPApportionmentTotal(FacilitySiteID, EmissionsUnitID, ProcessID)
                ClearRPApportionmentDetails()

            End If
        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Protected Sub gvwRPApportionment_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvwRPApportionment.RowCancelingEdit

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionsUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        gvwRPApportionment.EditIndex = -1
        LoadRPApportionmentGridView(FacilitySiteID, EmissionsUnitID, ProcessID)

    End Sub

    Protected Sub gvwRPApportionment_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvwRPApportionment.RowEditing

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EmissionsUnitID As String = txtEmissionsUnitID.Text.ToUpper
        Dim ProcessID As String = txtProcessID.Text.ToUpper
        gvwRPApportionment.EditIndex = e.NewEditIndex
        LoadRPApportionmentGridView(FacilitySiteID, EmissionsUnitID, ProcessID)

    End Sub

    Protected Sub gvwRPApportionment_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvwRPApportionment.RowUpdating

        Try
            Dim AvgPctEmissions As String = DirectCast(gvwRPApportionment.Rows(e.RowIndex).FindControl("txtAvgPctEmissions"), TextBox).Text
            Dim RPApportionmentComment As String = DirectCast(gvwRPApportionment.Rows(e.RowIndex).FindControl("txtRPApportionmentComment"), TextBox).Text
            Dim FacilitySiteID As String = gvwRPApportionment.DataKeys(e.RowIndex).Values(0).ToString
            Dim EmissionsUnitID As String = gvwRPApportionment.DataKeys(e.RowIndex).Values(1).ToString
            Dim ProcessID As String = gvwRPApportionment.DataKeys(e.RowIndex).Values(2).ToString
            Dim ReleasePointID As String = gvwRPApportionment.DataKeys(e.RowIndex).Values(3).ToString

            RPApportionmentComment = Left(RPApportionmentComment, 400)

            Dim sql = "Update eis_RPApportionment Set " &
                    "eis_RPApportionment.intAveragepercentEmissions = " & DbStringIntOrNull(AvgPctEmissions) & ", " &
                    "eis_RPApportionment.strRPApportionmentComment = '" & Replace(RPApportionmentComment, "'", "''") & "' " &
                    "where " &
                    "eis_RPApportionment.FacilitySiteID = '" & FacilitySiteID & "' and " &
                    "eis_RPApportionment.EmissionsUnitID = '" & EmissionsUnitID & "' and " &
                    "eis_RPApportionment.ProcessID = '" & ProcessID & "' and " &
                    "eis_RPApportionment.ReleasePointID = '" & ReleasePointID & "' "

            Dim cmd As New SqlCommand(sql, conn)
            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If
            cmd.ExecuteNonQuery()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            gvwRPApportionment.EditIndex = -1
            LoadRPApportionmentGridView(FacilitySiteID, EmissionsUnitID, ProcessID)
            CheckRPApportionmentTotal(FacilitySiteID, EmissionsUnitID, ProcessID)
            ClearRPApportionmentDetails()

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub CheckRPApportionmentTotal(ByVal fsid As String, ByVal euid As String, ByVal prid As String)

        Dim RPApportionmentSum As Integer

        RPApportionmentSum = RPApportiomentTotal(fsid, euid, prid)
        txtRPApportionmentTotal.Text = RPApportionmentSum & "%"
        RPApportionmentCheck(RPApportionmentSum)

    End Sub

    Private Sub ClearRPApportionmentDetails()

        ddlReleasePointID.SelectedIndex = 0
        txtAvgPctEmissions.Text = ""
        txtRPApportionmentComment.Text = ""

    End Sub

    Protected Sub btnClearRPApportionment_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearRPApportionment.Click
        ClearRPApportionmentDetails()
    End Sub

    Protected Sub gvwRPApportionment_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvwRPApportionment.RowCommand

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