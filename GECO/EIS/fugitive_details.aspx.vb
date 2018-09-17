Imports System.Data.SqlClient
Imports System.Data
Imports GECO.MapHelper

Partial Class eis_fugitive_details
    Inherits Page
    Public RPStatusCode As String
    Public conn, conn1, conn2, conn3 As New SqlConnection(oradb)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FugitiveID As String = Request.QueryString("fug")
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FIAccessCheck(EISAccessCode)

        If Not ReleasePointExists(FacilitySiteID, FugitiveID) Then
            Throw New HttpException(404, "Not found")
        End If

        If Not IsPostBack Then
            LoadFugitiveDetails(FacilitySiteID, FugitiveID)
            LoadRPApportionment(FacilitySiteID, FugitiveID)
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

    Private Sub LoadFugitiveDetails(ByVal fsid As String, ByVal RPid As String)

        Dim sql As String = ""
        Dim sql2 As String = ""
        Dim sql3 As String = ""
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim UpdateUser As String = ""
        Dim UpdateDateTime As String = ""
        Dim HCCcode As String = ""
        Dim HCCdesc As String = ""
        Dim HRCcode As String = ""
        Dim HRCdesc As String = ""
        Dim RPFencelineDistanceMeasure As Decimal
        Dim RPFugitiveHeightMeasure As Decimal
        Dim RPFugitiveWidthMeasure As Decimal
        Dim RPFugitiveLengthMeasure As Decimal
        Dim RPFugitiveAngleMeasure As Decimal
        Dim HORACCURACYMEASURE As Decimal
        Dim FacilityLongitude As String
        Dim FacilityLatitude As String
        Dim RPTypeCode As String = ""
        Dim RPTypeCodeDesc As String = ""
        Dim RPStatusCodeDesc As String = ""
        Dim RPStatusCodeyear As String = ""

        Try
            sql = "select RELEASEPOINTID, " &
                        "strRPDescription, " &
                        "strRPStatusCode, " &
                        "strRPTypeCode , " &
                        "NumRPStatusCodeYear, " &
                        "NUMRPFENCELINEDISTMEASURE, " &
                        "numRPFugitiveHeightMeasure , " &
                        "numRPFugitiveWidthMeasure, " &
                        "numRPFugitiveLengthMeasure, " &
                        "numRPFugitiveAngleMeasure, " &
                        "convert(char, UpdateDateTime, 20) As UpdateDateTime,  " &
                        "convert(char, LASTEISSUBMITDATE, 101) As LastEISSubmitDate, " &
                        "UpdateUser, " &
                        "strRPComment " &
                        "FROM EIS_ReleasePoint where EIS_ReleasePoint.FACILITYSITEID = '" & FacilitySiteID & "' and " &
                        "RELEASEPOINTID = '" & RPid & "' and ACTIVE = '1' order by RELEASEPOINTID"

            Dim cmd As New SqlCommand(sql, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr As SqlDataReader = cmd.ExecuteReader

            dr.Read()

            'Facility Name and Location
            If IsDBNull(dr("RELEASEPOINTID")) Then
                txtReleasePointID.Text = ""
            Else
                txtReleasePointID.Text = dr.Item("RELEASEPOINTID")
            End If

            If IsDBNull(dr("strRPDescription")) Then
                txtRPDescription.Text = ""
            Else
                txtRPDescription.Text = dr.Item("strRPDescription")
            End If

            If IsDBNull(dr("strRPStatusCode")) Then
                txtRPStatusCode.Text = 0
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

            If IsDBNull(dr("NUMRPFENCELINEDISTMEASURE")) Then
                txtRPFenceLineDistanceMeasure.Text = ""
            Else
                RPFencelineDistanceMeasure = dr.Item("NUMRPFENCELINEDISTMEASURE")
                If RPFencelineDistanceMeasure = -1 Then
                    txtRPFenceLineDistanceMeasure.Text = ""
                Else
                    txtRPFenceLineDistanceMeasure.Text = RPFencelineDistanceMeasure
                End If

            End If
            If IsDBNull(dr("numRPFugitiveHeightMeasure")) Then
                txtRPFugitiveHeightMeasure.Text = ""
            Else
                RPFugitiveHeightMeasure = dr.Item("numRPFugitiveHeightMeasure")
                If RPFugitiveHeightMeasure = -1 Then
                    txtRPFugitiveHeightMeasure.Text = ""
                Else
                    txtRPFugitiveHeightMeasure.Text = RPFugitiveHeightMeasure
                End If
            End If

            If IsDBNull(dr("numRPFugitiveWidthMeasure")) Then
                txtRPFugitiveWidthMeasure.Text = ""
            Else
                RPFugitiveWidthMeasure = dr.Item("numRPFugitiveWidthMeasure")
                If RPFugitiveWidthMeasure = -1 Then
                    txtRPFugitiveWidthMeasure.Text = ""
                Else
                    txtRPFugitiveWidthMeasure.Text = RPFugitiveWidthMeasure
                End If
            End If

            'Description
            If IsDBNull(dr("numRPFugitiveLengthMeasure")) Then
                txtRPFugitiveLengthMeasure.Text = ""
            Else
                RPFugitiveLengthMeasure = dr.Item("numRPFugitiveLengthMeasure")
                If RPFugitiveLengthMeasure = -1 Then
                    txtRPFugitiveLengthMeasure.Text = ""
                Else
                    txtRPFugitiveLengthMeasure.Text = RPFugitiveLengthMeasure
                End If
            End If
            If IsDBNull(dr("numRPFugitiveAngleMeasure")) Then
                txtRPFugitiveAngleMeasure.Text = ""
            Else
                RPFugitiveAngleMeasure = dr.Item("numRPFugitiveAngleMeasure")
                If RPFugitiveAngleMeasure = -1 Then
                    txtRPFugitiveAngleMeasure.Text = ""
                Else
                    txtRPFugitiveAngleMeasure.Text = RPFugitiveAngleMeasure
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
                        "RELEASEPOINTID = '" & RPid & "'"

            Dim cmd2 As New SqlCommand(sql2, conn2)

            If conn2.State = ConnectionState.Open Then
            Else
                conn2.Open()
            End If

            Dim dr2 As SqlDataReader = cmd2.ExecuteReader
            Dim recExist As Boolean = dr2.Read
            dr2.Close()

            If recExist Then
                'Load Fugitive geographic coordinate information
                sql3 = "select numLatitudeMeasure, " &
                            " numLongitudeMeasure, " &
                            " STRHORCOLLMETCode, " &
                            " INTHORACCURACYMEASURE, " &
                            " STRHORREFDATUMCode, " &
                            " convert(char, UpdateDateTime, 20) As UpdateDateTime,  " &
                            " convert(char, LastEISSubmitDate, 101) As LastEISSubmitDate, " &
                            " UpdateUser, " &
                            " strGeographicComment " &
                            " FROM EIS_RPGeoCoordinates where EIS_RPGeoCoordinates.FACILITYSITEID = '" & FacilitySiteID & "' and " &
                            " RELEASEPOINTID = '" & RPid & "'"
                Dim cmd3 As New SqlCommand(sql3, conn3)

                If conn3.State = ConnectionState.Open Then
                Else
                    conn3.Open()
                End If

                Dim dr3 As SqlDataReader = cmd3.ExecuteReader

                dr3.Read()

                'G.C. info
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

                If IsDBNull(dr3("INTHORACCURACYMEASURE")) Then
                    TxtHorizontalAccuracyMeasure.Text = ""
                Else
                    HORACCURACYMEASURE = dr3.Item("INTHORACCURACYMEASURE")
                    If HORACCURACYMEASURE = -1 Then
                        TxtHorizontalAccuracyMeasure.Text = ""
                    Else
                        TxtHorizontalAccuracyMeasure.Text = HORACCURACYMEASURE
                    End If
                End If
                If IsDBNull(dr3("STRHORCOLLMETCode")) Then
                    HCCcode = ""
                    TxtHorCollectionMetCode.Text = ""
                Else
                    HCCcode = dr3.Item("STRHORCOLLMETCode")
                    HCCdesc = GetHorCollMetDesc(HCCcode)
                    TxtHorCollectionMetCode.Text = HCCdesc
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
                    txtLastEISSubmit_FGC.Text = "Never submitted"
                Else
                    txtLastEISSubmit_FGC.Text = dr3.Item("LastEISSubmitDate")
                End If
                If IsDBNull(dr3("UpdateUser")) Then
                    UpdateUser = "Unknown"
                Else
                    UpdateUser = dr3.Item("UpdateUser")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr3("UpdateDateTime")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr3.Item("UpdateDateTime")
                End If
                txtLastUpdate_FGC.Text = UpdateDateTime & " by " & UpdateUser
                dr3.Close()
            End If

            If CheckRPGCData(FacilitySiteID, RPid) Then
                lblNoRPGeoCoordInfo.Text = "Release point geographic coordinate info need to be provided. Click the Edit button above."
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
            If conn2.State = ConnectionState.Open Then
                conn2.Close()
            End If
            If conn3.State = ConnectionState.Open Then
                conn3.Close()
            End If
        End Try
    End Sub

    Sub LoadRPApportionment(ByVal fsid As String, ByVal RPid As String)
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        SqlDataSourceRPApp.ConnectionString = oradb
        SqlDataSourceRPApp.ProviderName = setProviderName()

        SqlDataSourceRPApp.SelectCommand = "select eis_process.emissionsunitid, " &
                "eis_process.processid, " &
                "eis_process.strprocessdescription, " &
                "eis_rpapportionment.releasepointid, " &
                "concat(eis_rpapportionment.intaveragepercentemissions, '%') as intaveragepercentemissions " &
                "FROM eis_rpapportionment, eis_process " &
                "where eis_process.facilitysiteid = eis_rpapportionment.facilitysiteid " &
                "and eis_process.emissionsunitid = eis_rpapportionment.emissionsunitid " &
                "and eis_process.processid = eis_rpapportionment.processid " &
                "and eis_process.facilitysiteid='" & FacilitySiteID & "' " &
                "and eis_rpapportionment.releasepointid='" & RPid & "' " &
                "and eis_process.Active = '1'"

        gvwRPApportionment.DataBind()

        If gvwRPApportionment.Rows.Count = 0 Then
            lblReleasePointAppMessage.Text = "The Release Point is not used in any Process Release Point Apportionments and can be deleted on the Edit page."
            lblReleasePointAppMessage.Visible = True
        Else
            lblReleasePointAppMessage.Text = "The Release Point cannot be deleted. Either delete the process or add another release point to the apportionment " &
                                    "before deleting the remaining release point. See Help for more details."
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
        Dim FugitiveId As String = txtReleasePointID.Text
        Dim targetpage As String = "fugitive_edit.aspx" & "?fug=" & FugitiveId
        Response.Redirect(targetpage)

    End Sub
    Sub FugitiveRPIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim Fugitiveid As String = args.Value.ToUpper
        Dim targetpage As String = "fugitive_edit.aspx" & "?fug=" & Fugitiveid
        Dim FugitiveActive As String = CheckReleasePointIDexist(FacilitySiteID, Fugitiveid)
        Select Case FugitiveActive
            Case "0"
                args.IsValid = True
                Response.Redirect(targetpage)
            Case "1"
                args.IsValid = False
                cusvFugitiveID.ErrorMessage = " Release Point " + Fugitiveid + " is already in use.  Please enter another."
                txtNewFugitiveRP.Text = ""
                btnAddFugitiveRP_ModalPopupExtender.Show()
            Case "n"
                args.IsValid = True
                InsertFugitiveRP()
                Response.Redirect(targetpage)
        End Select

    End Sub

    Sub FugitiveDupIDCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)
        'Checks Release Point ID when duplicating release point

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim RPID As String = args.Value.ToUpper
        Dim targetpage As String = "fugitive_edit.aspx" & "?fug=" & RPID
        Dim RPIDActive As String = CheckRPIDExist_Dup(FacilitySiteID, RPID)

        Select Case RPIDActive
            Case "DFUG"
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Release Point " + RPID + " already exists and is a deleted fugitive release point. Enter another ID."
                txtDupFugitiveID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case "AFUG"
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Release Point " + RPID + " already exists and is a fugitive release point. Enter another ID."
                txtDupFugitiveID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case "DSTK"
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Stack " + RPID + " is already in use by a deleted stack.  Please enter another ID."
                txtDupFugitiveID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case "ASTK"
                args.IsValid = False
                cusvDuplicate.ErrorMessage = " Stack " + RPID + " is already in use.  Please enter another ID."
                txtDupFugitiveID.Text = ""
                btnDuplicate_ModalPopupExtender.Show()
            Case "DNE"
                args.IsValid = True
                DuplicateFugitive(FacilitySiteID, txtDupFugitiveID.Text.ToUpper)
                Response.Redirect(targetpage)
        End Select

    End Sub

    Private Sub InsertFugitiveRP()

        InsertReleasePoint(GetCookie(Cookie.AirsNumber), txtNewFugitiveRP.Text.ToUpper, txtNewFugitiveRPDesc.Text, "1")

    End Sub

    Private Sub DuplicateFugitive(ByVal fsid As String, ByVal stkid As String)

        Dim sqldup As String = ""
        Dim sqlsource As String = ""
        Dim SourceFugitiveID As String = txtReleasePointID.Text.ToUpper
        Dim DupFugitiveID As String = stkid.ToUpper
        Dim RPDescription As String = txtDupFugitiveDescription.Text
        Dim RPTypeCode As String = ""
        Dim RPFenceLineDistMeasure As String = ""
        Dim RPFugitiveHeightMeasure As String = ""
        Dim RPFugitiveWidthMeasure As String = ""
        Dim RPFugitiveLengthMeasure As String = ""
        Dim RPExitGasTempMeasure As String = ""
        Dim RPFugitiveAngleMeasure As String = ""
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
                        "ReleasePointID = '" & SourceFugitiveID.ToUpper & "'"

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
            If IsDBNull(dr1("numRPFugitiveHeightMeasure")) Then
                RPFugitiveHeightMeasure = ""
            Else
                RPFugitiveHeightMeasure = dr1.Item("numRPFugitiveHeightMeasure")
            End If
            If IsDBNull(dr1("numRPFugitiveWidthMeasure")) Then
                RPFugitiveWidthMeasure = ""
            Else
                RPFugitiveWidthMeasure = dr1.Item("numRPFugitiveWidthMeasure")
            End If
            If IsDBNull(dr1("numRPFugitiveLengthMeasure")) Then
                RPFugitiveLengthMeasure = ""
            Else
                RPFugitiveLengthMeasure = dr1.Item("numRPFugitiveLengthMeasure")
            End If
            If IsDBNull(dr1("numRPFugitiveAngleMeasure")) Then
                RPFugitiveAngleMeasure = ""
            Else
                RPFugitiveAngleMeasure = dr1.Item("numRPFugitiveAngleMeasure")
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
                        "numRPFugitiveHeightMeasure, " &
                        "numRPFugitiveWidthMeasure, " &
                        "numRPFugitiveLengthMeasure, " &
                        "numRPFugitiveAngleMeasure, " &
                        "numRPFenceLineDistMeasure, " &
                        "strRPStatusCode, " &
                        "numRPStatusCodeYear, " &
                        "Active, " &
                        "UpdateUser, " &
                        "UpdateDateTime, " &
                        "CreateDateTime) " &
                "Values (" &
                        "'" & fsid & "', " &
                        "'" & DupFugitiveID & "', " &
                        "'" & RPTypeCode & "', " &
                        "'" & Replace(RPDescription, "'", "''") & "', " &
                        DbStringIntOrNull(RPFugitiveHeightMeasure) & ", " &
                        DbStringIntOrNull(RPFugitiveWidthMeasure) & ", " &
                        DbStringIntOrNull(RPFugitiveLengthMeasure) & ", " &
                        DbStringIntOrNull(RPFugitiveAngleMeasure) & ", " &
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