Imports System.Data.SqlClient
Imports System.Data
Imports GECO.MapHelper

Partial Class eis_stack_details
    Inherits Page
    Public RPStatusCode As String
    Public conn, conn1, conn2, conn3 As New SqlConnection(DBConnectionString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim StackID As String = Request.QueryString("stk")

        FIAccessCheck(EISAccessCode)

        If Not ReleasePointExists(FacilitySiteID, StackID) Then
            Throw New HttpException(404, "Not found")
        End If

        If Not IsPostBack Then
            LoadStackDetails(FacilitySiteID, StackID)
            LoadRPApportionment(FacilitySiteID, StackID)
            loadStackTypeDDL()
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

    Private Sub loadStackTypeDDL()
        Dim sql As String
        Dim desc As String
        Dim code As String

        ddlRPTypeCode.Items.Add("--Select Stack Type--")
        Try
            sql = "select strdesc, RPTypeCode FROM EISLK_RPTYPECODE " &
                "where EISLK_RPTYPECODE.active = '1' " &
                "and EISLK_RPTYPECODE.strdesc <> 'Fugitive' order by strdesc"
            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader

            While dr.Read
                Dim newListItem As New ListItem()
                desc = dr.Item("strdesc")
                code = dr.Item("RPTypeCode")
                newListItem.Text = desc
                newListItem.Value = code
                ddlRPTypeCode.Items.Add(newListItem)

            End While

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try
    End Sub

    Private Sub LoadStackDetails(ByVal fsid As String, ByVal Stkid As String)

        Dim sql As String = ""
        Dim sql2 As String = ""
        Dim sql3 As String = ""
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim HCCcode As String
        Dim HCCdesc As String
        Dim HRCcode As String
        Dim HRCdesc As String
        Dim UpdateUser As String = ""
        Dim UpdateDateTime As String = ""
        Dim RPExitGasVelocityMeasure As Decimal
        Dim RPExitGasFlowRateMeasure As Decimal
        Dim RPExitGasTemperatureMeasure As Decimal
        Dim RPStackHeightMeasure As Decimal
        Dim RPStackDiameterMeasure As Decimal
        Dim RPFenceLineDistanceMeasure As Decimal
        Dim FacilityLongitude As String
        Dim FacilityLatitude As String
        Dim RPTypeCode As String = ""
        Dim RPTypeCodedesc As String = ""
        Dim RPStatusCodeDesc As String = ""
        Dim RPStatusCodeyear As String = ""

        Try
            sql = "select ReleasePointID, " &
                        "strRPDescription, " &
                        "strRPTypeCode , " &
                        "strRPStatusCode, " &
                        "numRPStatusCodeYear, " &
                        "numRPStackHeightMeasure , " &
                        "numRPStackDiameterMeasure, " &
                        "numRPExitGasVelocityMeasure, " &
                        "numRPExitGasFlowRateMeasure, " &
                        "numRPExitGasTempMeasure, " &
                        "numRPFencelineDistMeasure, " &
                        "convert(char, UpdateDateTime, 20) As UpdateDateTime,  " &
                        "convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                        "UpdateUser, " &
                        "strRPComment " &
                        "FROM EIS_ReleasePoint " &
                        "where EIS_ReleasePoint.FACILITYSITEID = '" & FacilitySiteID & "' and " &
                        "RELEASEPOINTID = '" & Stkid & "'"
            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader

            dr.Read()

            'Facility Name and Location
            If IsDBNull(dr("ReleasePointID")) Then
                txtReleasePointID.Text = ""
            Else
                txtReleasePointID.Text = dr.Item("ReleasePointID")
            End If

            If IsDBNull(dr("strRPDescription")) Then
                txtRPDescription.Text = ""
            Else
                txtRPDescription.Text = dr.Item("strRPDescription")
            End If

            If IsDBNull(dr("strRPTypeCode")) Then
                txtRPTypeCode.Text = ""
                RPTypeCode = ""
            Else
                RPTypeCode = dr.Item("strRPTypeCode")
                RPTypeCodedesc = GetStackTypeCodeDesc(RPTypeCode)
                txtRPTypeCode.Text = RPTypeCodedesc
            End If

            If IsDBNull(dr("strRPStatusCode")) Then
                txtRPStatusCode.Text = ""
                RPStatusCode = ""
            Else
                RPStatusCode = dr.Item("strRPStatusCode")
                RPStatusCodeDesc = GetStackStatusCodeDesc(RPStatusCode)
            End If

            If IsDBNull(dr("NumRPStatusCodeYear")) Then
                RPStatusCodeyear = ""
            Else
                RPStatusCodeyear = dr.Item("NumRPStatusCodeYear")
            End If
            txtRPStatusCode.Text = RPStatusCodeDesc & " as reported in " & RPStatusCodeyear

            If IsDBNull(dr("numRPExitGasVelocityMeasure")) Then
                txtRPExitGasVelocityMeasure.Text = ""
            Else
                RPExitGasVelocityMeasure = dr.Item("numRPExitGasVelocityMeasure")
                If RPExitGasVelocityMeasure = -1 Then
                    txtRPExitGasVelocityMeasure.Text = ""
                Else
                    txtRPExitGasVelocityMeasure.Text = FormatNumber(RPExitGasVelocityMeasure, 1)
                End If
            End If

            If IsDBNull(dr("numRPExitGasFlowRateMeasure")) Then
                txtRPExitGasFlowRateMeasure.Text = ""
            Else
                RPExitGasFlowRateMeasure = dr.Item("numRPExitGasFlowRateMeasure")
                If RPExitGasFlowRateMeasure = -1 Then
                    txtRPExitGasFlowRateMeasure.Text = ""
                Else
                    txtRPExitGasFlowRateMeasure.Text = FormatNumber(RPExitGasFlowRateMeasure, 1)
                End If
            End If

            If IsDBNull(dr("NUMRPEXITGASTEMPMEASURE")) Then
                txtRPExitGasTemperatureMeasure.Text = ""
            Else
                RPExitGasTemperatureMeasure = dr.Item("NUMRPEXITGASTEMPMEASURE")
                If RPExitGasTemperatureMeasure = -1 Then
                    txtRPExitGasTemperatureMeasure.Text = ""
                Else
                    txtRPExitGasTemperatureMeasure.Text = FormatNumber(RPExitGasTemperatureMeasure, 0)
                End If
            End If

            If IsDBNull(dr("NUMRPFENCELINEDISTMEASURE")) Then
                txtRPFenceLineDistanceMeasure.Text = ""
            Else
                RPFenceLineDistanceMeasure = dr.Item("NUMRPFENCELINEDISTMEASURE")
                If RPFenceLineDistanceMeasure = -1 Then
                    txtRPFenceLineDistanceMeasure.Text = ""
                Else
                    txtRPFenceLineDistanceMeasure.Text = FormatNumber(RPFenceLineDistanceMeasure, 0)
                End If
            End If
            If IsDBNull(dr("NUMRPSTACKHEIGHTMEASURE")) Then
                txtRPStackHeightMeasure.Text = ""
            Else
                RPStackHeightMeasure = dr.Item("NUMRPSTACKHEIGHTMEASURE")
                If RPStackHeightMeasure = -1 Then
                    txtRPStackHeightMeasure.Text = ""
                Else
                    txtRPStackHeightMeasure.Text = FormatNumber(RPStackHeightMeasure, 1)
                End If
            End If

            If IsDBNull(dr("NUMRPSTACKDIAMETERMEASURE")) Then
                txtRPStackDiameterMeasure.Text = ""
            Else
                RPStackDiameterMeasure = dr.Item("NUMRPSTACKDIAMETERMEASURE")
                If RPStackDiameterMeasure = -1 Then
                    txtRPStackDiameterMeasure.Text = ""
                Else
                    txtRPStackDiameterMeasure.Text = FormatNumber(RPStackDiameterMeasure, 1)
                End If
            End If

            If IsDBNull(dr("strRPComment")) Then
                txtRPComment.Text = ""
                txtRPComment.Visible = False
            Else
                txtRPComment.Text = dr.Item("strRPComment")
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
            dr.Close()

            'Check if RP GeoCoord data exists before loading data
            sql2 = "select numLatitudeMeasure " &
                        "FROM EIS_RPGeoCoordinates where " &
                        "EIS_RPGeoCoordinates.FACILITYSITEID = '" & FacilitySiteID & "' and " &
                        "RELEASEPOINTID = '" & Stkid & "'"

            Dim cmd2 As New SqlCommand(sql2, conn2)

            If conn2.State = ConnectionState.Open Then
            Else
                conn2.Open()
            End If

            Dim dr2 As SqlDataReader = cmd2.ExecuteReader
            Dim recExist As Boolean = dr2.Read
            dr2.Close()

            If recExist Then
                'Load Stack GC Information
                sql3 = "select numLatitudeMeasure, " &
                            "numLongitudeMeasure, " &
                            "STRHORCOLLMETCode, " &
                            "INTHORACCURACYMEASURE , " &
                            "STRHORREFDATUMCode, " &
                            "convert(char, UpdateDateTime, 20) As UpdateDateTime,  " &
                            "convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                            "UpdateUser, " &
                            "strGeographicComment " &
                            "FROM EIS_RPGeoCoordinates " &
                            "where EIS_RPGeoCoordinates.FACILITYSITEID = '" & FacilitySiteID & "' " &
                            "and RELEASEPOINTID = '" & Stkid & "'"

                Dim cmd3 As New SqlCommand(sql3, conn3)

                If conn3.State = ConnectionState.Open Then
                Else
                    conn3.Open()
                End If

                Dim dr3 As SqlDataReader = cmd3.ExecuteReader

                dr3.Read()

                If IsDBNull(dr3("numLatitudeMeasure")) Then
                    TxtLatitudeMeasure.Text = ""
                Else
                    TxtLatitudeMeasure.Text = dr3.Item("numLatitudeMeasure")
                End If
                If IsDBNull(dr3("numLongitudeMeasure")) Then
                    TxtLongitudeMeasure.Text = ""
                Else
                    TxtLongitudeMeasure.Text = dr3.Item("numLongitudeMeasure")
                End If

                'Render Google map
                If TxtLatitudeMeasure.Text <> "" And TxtLongitudeMeasure.Text <> "" Then
                    FacilityLatitude = TxtLatitudeMeasure.Text
                    FacilityLongitude = TxtLongitudeMeasure.Text
                Else
                    FacilityLatitude = GetFacilityLatitude(FacilitySiteID)
                    FacilityLongitude = GetFacilityLongitude(FacilitySiteID)
                End If
                imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(FacilityLatitude, FacilityLongitude))

                If IsDBNull(dr3("STRHORCOLLMETCode")) Then
                    HCCcode = ""
                    TxtHorCollectionMetCode.Text = ""
                Else
                    HCCcode = dr3.Item("STRHORCOLLMETCode")
                    HCCdesc = GetHorCollMetDesc(HCCcode)
                    TxtHorCollectionMetCode.Text = HCCdesc
                End If

                If IsDBNull(dr3("INTHORACCURACYMEASURE")) Then
                    TxtHorizontalAccuracyMeasure.Text = ""
                Else
                    TxtHorizontalAccuracyMeasure.Text = dr3.Item("INTHORACCURACYMEASURE")
                End If

                If IsDBNull(dr3("STRHORREFDATUMCode")) Then
                    HRCcode = ""
                    TxtHorReferenceDatCode.Text = ""
                Else
                    HRCcode = dr3.Item("STRHORREFDATUMCode")
                    HRCdesc = GetHorRefDatumDesc(HRCcode)
                    TxtHorReferenceDatCode.Text = HRCdesc
                End If

                If IsDBNull(dr3("strGeographicComment")) Then
                    TxtGeographicComment.Text = ""
                    TxtGeographicComment.Visible = False
                Else
                    TxtGeographicComment.Text = dr3.Item("strGeographicComment")
                End If

                If IsDBNull(dr3("LastEISSubmitDate")) Then
                    txtlastEISSubmit_SGC.Text = "Never submitted"
                Else
                    txtlastEISSubmit_SGC.Text = dr3.Item("LastEISSubmitDate")
                End If

                If IsDBNull(dr3("UpdateUser")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr3.Item("UpdateUser")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If

                If IsDBNull(dr3("UpdateDateTime")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr3.Item("UpdateDateTime")
                End If
                txtLastUpdate_SGC.Text = UpdateDateTime & " by " & UpdateUser
                dr3.Close()

                If CheckRPGCData(FacilitySiteID, Stkid) Then
                    lblNoRPGeoCoordInfo.Text = "Stack geographic coordinate info need to be provided. Click the Edit button above."
                End If

            End If

            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
            If conn2.State = ConnectionState.Open Then
                conn2.Close()
            End If
            If conn3.State = ConnectionState.Open Then
                conn3.Close()
            End If

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Sub LoadRPApportionment(ByVal fsid As String, ByVal Stkid As String)
        SqlDataSourceRPApp.ConnectionString = DBConnectionString

        SqlDataSourceRPApp.SelectCommand = "select eis_process.emissionsunitid, " &
                "eis_process.processid, " &
                "eis_process.strprocessdescription, " &
                "eis_rpapportionment.releasepointid, " &
                "concat(eis_rpapportionment.intaveragepercentemissions, '%') as intaveragepercentemissions " &
                "FROM eis_rpapportionment, eis_process " &
                "where eis_process.facilitysiteid = eis_rpapportionment.facilitysiteid " &
                "and eis_process.emissionsunitid = eis_rpapportionment.emissionsunitid " &
                "and eis_process.processid = eis_rpapportionment.processid " &
                "and eis_process.facilitysiteid= @FacilitySiteID " &
                "and eis_rpapportionment.releasepointid= @Stkid " &
                "and eis_process.Active = '1'"

        SqlDataSourceRPApp.SelectParameters.Add("FacilitySiteID", fsid)
        SqlDataSourceRPApp.SelectParameters.Add("Stkid", Stkid)

        gvwRPApportionment.DataBind()

        If gvwRPApportionment.Rows.Count = 0 Then
            lblReleasePointAppMessage.Text = "Release Point not used in Process Release Point Apportionments. It can be deleted on the Edit page."
            lblReleasePointAppMessage.Visible = True
        Else
            lblReleasePointAppMessage.Text = "Release Point cannot be deleted. Delete process or add another release point to apportionment before deleting."
            lblReleasePointAppMessage.Visible = True
        End If
        If RPStatusCode <> "OP" Then
            lblRPShutdownMessage.Text = "Release point is shutdown; it cannot be added to new release point apportionments."
            lblRPShutdownMessage.Visible = True
        Else
            lblRPShutdownMessage.Text = ""
            lblRPShutdownMessage.Visible = False
        End If

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim stackID = txtReleasePointID.Text
        Dim targetpage As String = "stack_edit.aspx" & "?stk=" & stackID
        Response.Redirect(targetpage)
    End Sub

    Sub StackIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim stackID As String = args.Value.ToUpper
        Dim stackActive As String = CheckReleasePointIDexist(FacilitySiteID, stackID)
        Dim targetpage As String = "stack_edit.aspx" & "?stk=" & stackID

        Select Case stackActive
            Case "0"
                args.IsValid = True
                Response.Redirect(targetpage)
            Case "1"
                args.IsValid = False
                cusvStackID.ErrorMessage = "Stack " + stackID + " is already in use.  Please enter another."
                txtNewStackID.Text = ""
                btnAddStack_ModalPopupExtender.Show()
            Case "n"
                args.IsValid = True
                InsertStack()
                Response.Redirect(targetpage)
        End Select

    End Sub

    Sub StackDupIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)
        'Checks Release Point ID when duplicating release point

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim RPID As String = args.Value.ToUpper
        Dim targetpage As String = "stack_edit.aspx" & "?stk=" & RPID
        Dim RPIDActive As String = CheckRPIDExist_Dup(FacilitySiteID, RPID)

        Select Case RPIDActive
            Case "DFUG"
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Release Point " + RPID + " already exists and is a deleted fugitive release point. Enter another ID."
                txtDupStackID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case "AFUG"
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Release Point " + RPID + " already exists and is a fugitive release point. Enter another ID."
                txtDupStackID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case "DSTK"
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Stack " + RPID + " is already in use by a deleted stack.  Please enter another ID."
                txtDupStackID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case "ASTK"
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Stack " + RPID + " is already in use.  Please enter another ID."
                txtDupStackID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case "DNE"
                args.IsValid = True
                DuplicateStack(FacilitySiteID, txtDupStackID.Text.ToUpper)
                Response.Redirect(targetpage)
        End Select

    End Sub

    Private Sub InsertStack()

        InsertReleasePoint(GetCookie(Cookie.AirsNumber), txtNewStackID.Text.ToUpper, txtNewStackDesc.Text, ddlRPTypeCode.SelectedValue)

    End Sub

    Private Sub DuplicateStack(ByVal fsid As String, ByVal stkid As String)

        Dim sqldup As String = ""
        Dim sqlsource As String = ""
        Dim SourceStackID As String = txtReleasePointID.Text.ToUpper
        Dim DupStackID As String = stkid.ToUpper
        Dim RPDescription As String = txtDupStackDescription.Text
        Dim RPTypeCode As String = ""
        Dim RPStackHeightMeasure As String = ""
        Dim RPStackDiameterMeasure As String = ""
        Dim RPExitGasVelocityMeasure As String = ""
        Dim RPExitGasFlowRateMeasure As String = ""
        Dim RPExitGasTempMeasure As String = ""
        Dim RPFenceLineDistMeasure As String = ""
        Dim RPStatusCode As String = "OP"
        Dim RPStatusCodeYear As Integer = Now.Year
        Dim Active As String = "1"
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        'Truncate description if > 100
        If Len(RPDescription) > 100 Then
            RPDescription = Left(RPDescription, 100)
        End If

        Try
            'Get data for source unit
            sqlsource = "select * FROM eis_ReleasePoint " &
                        "where " &
                        "FacilitySiteID = '" & fsid & "' and " &
                        "ReleasePointID = '" & SourceStackID.ToUpper & "'"

            Dim cmd1 As New SqlCommand(sqlsource, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr1 As SqlDataReader = cmd1.ExecuteReader

            dr1.Read()

            If IsDBNull(dr1("strRPTypeCode")) Then
                RPTypeCode = ""
            Else
                RPTypeCode = dr1.Item("strRPTypeCode")
            End If
            If IsDBNull(dr1("numRPStackHeightMeasure")) Then
                RPStackHeightMeasure = ""
            Else
                RPStackHeightMeasure = dr1.Item("numRPStackHeightMeasure")
            End If
            If IsDBNull(dr1("numRPStackDiameterMeasure")) Then
                RPStackDiameterMeasure = ""
            Else
                RPStackDiameterMeasure = dr1.Item("numRPStackDiameterMeasure")
            End If
            If IsDBNull(dr1("numRPExitGasVelocityMeasure")) Then
                RPExitGasVelocityMeasure = ""
            Else
                RPExitGasVelocityMeasure = dr1.Item("numRPExitGasVelocityMeasure")
            End If
            If IsDBNull(dr1("numRPExitGasFlowRateMeasure")) Then
                RPExitGasFlowRateMeasure = ""
            Else
                RPExitGasFlowRateMeasure = dr1.Item("numRPExitGasFlowRateMeasure")
            End If
            If IsDBNull(dr1("numRPExitGasTempMeasure")) Then
                RPExitGasTempMeasure = ""
            Else
                RPExitGasTempMeasure = dr1.Item("numRPExitGasTempMeasure")
            End If
            If IsDBNull(dr1("numRPFenceLineDistMeasure")) Then
                RPFenceLineDistMeasure = ""
            Else
                RPFenceLineDistMeasure = dr1.Item("numRPFenceLineDistMeasure")
            End If

            sqldup = "Insert into eis_ReleasePoint (" &
                        "FacilitySiteID, " &
                        "ReleasePointID, " &
                        "strRPTypeCode, " &
                        "strRPDescription, " &
                        "numRPStackHeightMeasure, " &
                        "numRPStackDiameterMeasure, " &
                        "numRPExitGasVelocityMeasure, " &
                        "numRPExitGasFlowRateMeasure, " &
                        "numRPExitGasTempMeasure, " &
                        "numRPFenceLineDistMeasure, " &
                        "strRPStatusCode, " &
                        "numRPStatusCodeYear, " &
                        "Active, " &
                        "UpdateUser, " &
                        "UpdateDateTime, " &
                        "CreateDateTime) " &
                "Values (" &
                        "'" & fsid & "', " &
                        "'" & DupStackID & "', " &
                        "'" & RPTypeCode & "', " &
                        "'" & Replace(RPDescription, "'", "''") & "', " &
                        DbStringDecimalOrNull(RPStackHeightMeasure) & ", " &
                        DbStringDecimalOrNull(RPStackDiameterMeasure) & ", " &
                        DbStringDecimalOrNull(RPExitGasVelocityMeasure) & ", " &
                        DbStringDecimalOrNull(RPExitGasFlowRateMeasure) & ", " &
                        DbStringIntOrNull(RPExitGasTempMeasure) & ", " &
                        DbStringIntOrNull(RPFenceLineDistMeasure) & ", " &
                        "'" & RPStatusCode & "', " &
                        "'" & RPStatusCodeYear & "', " &
                        "'" & Active & "', " &
                        "'" & Replace(UpdateUser, "'", "''") & "', " &
                        "getdate(), " &
                        "getdate()) "

            Dim cmd2 As New SqlCommand(sqldup, conn)

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