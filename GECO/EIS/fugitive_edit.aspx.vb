Imports System.Data.SqlClient
Imports GECO.MapHelper
Imports Reimers.Google.Map

Partial Class eis_fugitive_edit
    Inherits Page

    Private ReadOnly SaveFugitive As String = "Fugitive information saved successfully."
    Private FugitiveGCDataMissing As Boolean
    Private ReadOnly FugitiveGCMessage As String = "Fugitive geographic coordinate data incomplete. Correct and save."
    Private FugitiveEISSubmit As Boolean
    Private FugitiveUsedInRPA As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim FugitiveID As String = Request.QueryString("fug")

        FIAccessCheck(EISAccessCode)

        If Not ReleasePointExists(FacilitySiteID, FugitiveID) Then
            Throw New HttpException(404, "Not found")
        End If

        If Not IsPostBack Then
            loadFugiveStatusCode()
            LoadHorCollectDDL()
            LoadHorRefDatumDDL()
            LoadFugitiveGCIValidation()
            LoadFugitiveDetails(FacilitySiteID, FugitiveID) 'Loads Fugitive Info and sets StackEISSubmit based on EIS_ReleasePoint.strEISSubmit value
            LoadRPApportionment(FacilitySiteID, FugitiveID)
            FugitiveUsedInRPA = CheckRPApportionment(FacilitySiteID, FugitiveID)

            'NOTE: If CheckRPGCData returns TRUE then GC data is missing
            FugitiveGCDataMissing = CheckRPGCData(FacilitySiteID, FugitiveID)
            If FugitiveGCDataMissing Then
                lblFugitiveGCDataMissing.Text = FugitiveGCMessage
                lblFugitiveGCDataMissing.Visible = True
                lblFugitiveMessage.Text = FugitiveGCMessage
                TxtLatitudeMeasure.Focus()
            Else
                lblFugitiveGCDataMissing.Text = ""
            End If

            If Not FugitiveEISSubmit AndAlso Not FugitiveUsedInRPA Then
                btnDelete.Visible = True
            Else
                btnDelete.Visible = False
            End If
            lblDeleteFugitive.Text = "Do you want to delete Fugitive Release Point " & FugitiveID & "?"
            btnDeleteSummary.Visible = False

            txtMapLat.Attributes.Add("readonly", "readonly")
            txtMapLon.Attributes.Add("readonly", "readonly")
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

    Private Sub loadFugiveStatusCode()
        ddlFugitiveStatusCode.Items.Add("--Select Operating Status--")
        Try
            Dim query As String = " select strDesc, RPStatusCode FROM EISLK_RPSTATUSCODE where Active = '1' order by strDesc"

            Dim dt As DataTable = DB.GetDataTable(query)

            For Each dr As DataRow In dt.Rows
                Dim newListItem As New ListItem With {
                    .Text = dr.Item("strdesc").ToString,
                    .Value = dr.Item("RPStatusCode").ToString
                }
                ddlFugitiveStatusCode.Items.Add(newListItem)
            Next

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadHorCollectDDL()
        ddlHorCollectionMetCode.Items.Add("--Select Horizontal Collection Method--")
        Try
            Dim query = "select strDesc, HorCollMetCode FROM EISLK_HORCOLLMETCODE where Active = '1' order by strDesc "

            Dim dt As DataTable = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("HorCollMetCode")
                    }
                    ddlHorCollectionMetCode.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadHorRefDatumDDL()
        ddlHorReferenceDatCode.Items.Add("--Select Horizontal Reference Datum--")
        Try
            Dim query = " select strDesc, HorRefDatumCode FROM EISLK_HORREFDATUMCODE where ACTIVE = '1' order by strDesc "

            Dim dt As DataTable = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("HorRefDatumCode")
                    }
                    ddlHorReferenceDatCode.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadFugitiveGCIValidation()
        Dim CountyCode As String = GetCookie(Cookie.AirsNumber).Substring(0, 3)
        Dim mm As MinMaxLatLon = GetCountyLatLong(CountyCode)

        rngvLatitudeMeasure.MaximumValue = mm.MaxLat
        rngvLatitudeMeasure.MinimumValue = mm.MinLat
        rngvLatitudeMeasure.ErrorMessage = "The Latitude must be between " & mm.MinLat.ToString & " and " & mm.MaxLat.ToString & "."
        rngvLatitudeMeasure.Text = "Must be between " & mm.MinLat.ToString & " and " & mm.MaxLat.ToString

        rngvLongitudeMeasure.MaximumValue = mm.MaxLon
        rngvLongitudeMeasure.MinimumValue = mm.MinLon
        rngvLongitudeMeasure.ErrorMessage = "The Latitude must be between " & mm.MinLon.ToString & " and " & mm.MaxLon.ToString & "."
        rngvLongitudeMeasure.Text = "Must be between " & mm.MinLon.ToString & " and " & mm.MaxLon.ToString
    End Sub

    Private Sub LoadFugitiveDetails(ByVal fsid As String, ByVal RPid As String)

        Dim RPFencelineDistanceMeasure As Decimal
        Dim RPFugitiveHeightMeasure As Decimal
        Dim RPFugitiveWidthMeasure As Decimal
        Dim RPFugitiveLengthMeasure As Decimal
        Dim RPFugitiveAngleMeasure As Decimal
        Dim EISSubmit As String

        Try
            Dim query As String = "select ReleasePointID, " &
                "STRRPDESCRIPTION, " &
                "strRPStatusCode, " &
                "NumRPStatusCodeYear, " &
                "NUMRPFENCELINEDISTMEASURE, " &
                "numRPFugitiveHeightMeasure , " &
                "numRPFugitiveWidthMeasure, " &
                "numRPFugitiveLengthMeasure, " &
                "numRPFugitiveAngleMeasure, " &
                "strRPComment, " &
                "Active, " &
                "strEISSubmit " &
                "FROM EIS_ReleasePoint " &
                "where EIS_ReleasePoint.FACILITYSITEID = @fsid " &
                "and ReleasePointID = @RPid"

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@RPid", RPid)
            }

            Dim dr As DataRow = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                'Load Fugitive Release Point Details

                If IsDBNull(dr("Active")) Then
                    btnCancel.Visible = False
                    btnCancel3.Visible = False
                Else
                    If dr.Item("Active") = "0" Then
                        btnCancel.Visible = False
                        btnCancel3.Visible = False
                    Else
                        btnCancel.Visible = True
                        btnCancel3.Visible = True
                    End If
                End If

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

                If IsDBNull(dr("strRPStatusCode")) Then
                    ddlFugitiveStatusCode.SelectedValue = ""
                Else
                    ddlFugitiveStatusCode.SelectedValue = dr.Item("strRPStatusCode")
                    txtFugitiveStatusCodeOnLoad.Text = ddlFugitiveStatusCode.SelectedValue
                End If

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
                Else
                    txtRPComment.Text = dr.Item("strRPComment")
                End If

                If IsDBNull(dr.Item("strEISSubmit")) Then
                    FugitiveEISSubmit = False
                Else
                    EISSubmit = dr.Item("strEISSubmit")
                    If EISSubmit = "0" Then
                        FugitiveEISSubmit = False
                    Else
                        FugitiveEISSubmit = True
                    End If
                End If

            End If

            'Load Fugitive GC Information
            query = "select numLatitudeMeasure, " &
                            "numLongitudeMeasure, " &
                            "STRHORCOLLMETCode, " &
                            "INTHORACCURACYMEASURE, " &
                            "STRHORREFDATUMCode, " &
                            "strGeographicComment " &
                            "FROM EIS_RPGeoCoordinates " &
                            "where EIS_RPGeoCoordinates.FACILITYSITEID = @fsid " &
                            "and ReleasePointID = @RPid "

            Dim dr3 As DataRow = DB.GetDataRow(query, params)

            If dr3 IsNot Nothing Then

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

                If IsDBNull(dr3("STRHORCOLLMETCode")) Then
                    ddlHorCollectionMetCode.SelectedIndex = 0
                Else
                    ddlHorCollectionMetCode.SelectedValue = dr3.Item("STRHORCOLLMETCode")
                End If

                If IsDBNull(dr3("INTHORACCURACYMEASURE")) Then
                    TxtHorizontalAccuracyMeasure.Text = ""
                Else
                    TxtHorizontalAccuracyMeasure.Text = dr3.Item("INTHORACCURACYMEASURE")
                End If

                If IsDBNull(dr3("STRHORREFDATUMCode")) Then
                    ddlHorReferenceDatCode.SelectedIndex = 0
                Else
                    ddlHorReferenceDatCode.SelectedValue = dr3.Item("STRHORREFDATUMCode")
                End If

                If IsDBNull(dr3("strGeographicComment")) Then
                    TxtGeographicComment.Text = ""
                Else
                    TxtGeographicComment.Text = dr3.Item("strGeographicComment")
                End If

                'Populate Google map
                Dim MapLatitude As Decimal
                Dim MapLongitude As Decimal

                If TxtLatitudeMeasure.Text <> "" And TxtLongitudeMeasure.Text <> "" Then
                    MapLatitude = TxtLatitudeMeasure.Text
                    MapLongitude = TxtLongitudeMeasure.Text
                Else
                    MapLatitude = GetFacilityLatitude(fsid)
                    MapLongitude = GetFacilityLongitude(fsid)
                End If

                imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(MapLatitude, MapLongitude))
                lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(New Coordinate(MapLatitude, MapLongitude))

            Else
                lblFugitiveMessage.Text = FugitiveGCMessage
                lblFugitiveGCDataMissing.Text = FugitiveGCMessage
            End If

            ' Store previous Geographic Data for comparison on submit
            hidLatitude.Value = TxtLatitudeMeasure.Text
            hidLongitude.Value = TxtLongitudeMeasure.Text
            hidHorCollectionMetCode.Value = ddlHorCollectionMetCode.SelectedValue
            hidHorCollectionMetDesc.Value = ddlHorCollectionMetCode.SelectedItem.Text
            hidHorizontalAccuracyMeasure.Value = TxtHorizontalAccuracyMeasure.Text
            hidHorReferenceDatCode.Value = ddlHorReferenceDatCode.SelectedValue
            hidHorReferenceDatDesc.Value = ddlHorReferenceDatCode.SelectedItem.Text
            hidGeographicComment.Value = TxtGeographicComment.Text

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Sub LoadRPApportionment(ByVal fsid As String, ByVal RPid As String)

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
                "and eis_process.facilitysiteid= @fsid " &
                "and eis_rpapportionment.releasepointid= @RPid " &
                " and EIS_RPAPPORTIONMENT.ACTIVE = '1' " &
                "and eis_process.Active = '1'"

        SqlDataSourceRPApp.SelectParameters.Add("fsid", fsid)
        SqlDataSourceRPApp.SelectParameters.Add("RPid", RPid)

        gvwRPApportionment.DataBind()

        If gvwRPApportionment.Rows.Count = 0 Then
            lblReleasePointAppMessage.Text = "The Release Point is not used in any Process Release Point Apportionments and can be deleted on the Edit page."
            lblReleasePointAppMessage.Visible = True
            ddlFugitiveStatusCode.Enabled = True
        Else
            lblReleasePointAppMessage.Text = "The Release Point cannot be deleted. Either delete the process or add another release point to the apportionment " &
                                    "before deleting the remaining release point. See Help for more details."
            lblReleasePointAppMessage.Visible = True
            If ddlFugitiveStatusCode.SelectedValue <> "OP" Then
                ddlFugitiveStatusCode.Enabled = True
            Else
                ddlFugitiveStatusCode.Enabled = False
                lblRPShutdownMessage.Text = "Status cannot be changed as release point is in a release point apportionment."
            End If
        End If

    End Sub

    Protected Sub FugitiveStatusChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlFugitiveStatusCode.SelectedIndexChanged
        'This sets the value of the textbox txtstackStatusChanged if the stack status is changed.
        'If the stack status is different than the value when the page loads, the status date must
        'be updated in the update subroutine

        txtFugitiveStatusCodeChanged.Text = ddlFugitiveStatusCode.SelectedValue

    End Sub

    Private Sub SaveFugitiveStackInfo()
        Dim SQL As String
        Dim FugitiveID As String = Left(txtReleasePointID.Text, 6)
        Dim RPDescription As String = Left(txtRPDescription.Text, 100)
        Dim RPStatusCode As String = ddlFugitiveStatusCode.SelectedValue
        Dim RPFenceLineDistanceMeasure = ParseAsNullableInteger(txtRPFenceLineDistanceMeasure.Text)
        Dim RPFugitiveHeightMeasure = ParseAsNullableInteger(txtRPFugitiveHeightMeasure.Text)
        Dim RPFugitiveWidthMeasure = ParseAsNullableInteger(txtRPFugitiveWidthMeasure.Text)
        Dim RPFugitiveLengthMeasure = ParseAsNullableInteger(txtRPFugitiveLengthMeasure.Text)
        Dim RPFugitiveAngleMeasure = ParseAsNullableInteger(txtRPFugitiveAngleMeasure.Text)
        Dim RPComment As String = Left(txtRPComment.Text, 400)
        Dim FugitiveStatusCodeYear = Now.Year
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Dim StatusCodeOnLoad = txtFugitiveStatusCodeOnLoad.Text
        Dim StatusCodeChanged = txtFugitiveStatusCodeChanged.Text
        Dim Active As String = "1"
        FugitiveID = FugitiveID.ToUpper

        Try
            If StatusCodeOnLoad = StatusCodeChanged Or StatusCodeChanged = "" Then
                'Does not update Fugitive Status and Unit Status Code year
                SQL = "Update EIS_RELEASEPOINT Set STRRPDESCRIPTION = @RPDescription , " &
                                     "NUMRPFENCELINEDISTMEASURE = @RPFenceLineDistanceMeasure , " &
                                     "NUMRPFUGITIVEHEIGHTMEASURE = @RPFugitiveHeightMeasure, " &
                                     "NUMRPFUGITIVEWIDTHMEASURE = @RPFugitiveWidthMeasure , " &
                                     "NUMRPFUGITIVELENGTHMEASURE = @RPFugitiveLengthMeasure, " &
                                     "NUMRPFUGITIVEANGLEMEASURE = @RPFugitiveAngleMeasure ,  " &
                                     "STRRPTYPECODE = '1', " &
                                     "STRRPCOMMENT = @RPComment, " &
                                     "ACTIVE = @Active, " &
                                     "UPDATEUSER = @UpdateUser, " &
                                     "UpdateDateTime = getdate() " &
                                     "where EIS_RELEASEPOINT.FACILITYSITEID = @FacilitySiteID " &
                                     "and ReleasePointID = @FugitiveID "


            Else
                'Updates Fugitive Status and Unit Status Code year
                SQL = "Update EIS_RELEASEPOINT Set STRRPDESCRIPTION =  @RPDescription, " &
                                         "NUMRPFENCELINEDISTMEASURE = @RPFenceLineDistanceMeasure , " &
                                         "NUMRPFUGITIVEHEIGHTMEASURE = @RPFugitiveHeightMeasure , " &
                                         "NUMRPFUGITIVEWIDTHMEASURE = @RPFugitiveWidthMeasure , " &
                                         "NUMRPFUGITIVELENGTHMEASURE = @RPFugitiveLengthMeasure , " &
                                         "NUMRPFUGITIVEANGLEMEASURE = @RPFugitiveAngleMeasure ,  " &
                                         "STRRPSTATUSCODE = @RPStatusCode , " &
                                         "STRRPTYPECODE = 1 , " &
                                         "NUMRPSTATUSCODEYEAR = @FugitiveStatusCodeYear , " &
                                         "STRRPCOMMENT = @RPComment , " &
                                         "ACTIVE = @Active , " &
                                         "UPDATEUSER = @UpdateUser, " &
                                         "UpdateDateTime = getdate() " &
                                         "where EIS_RELEASEPOINT.FACILITYSITEID = @FacilitySiteID " &
                                         "and ReleasePointID = @FugitiveID  "
            End If

            Dim params As SqlParameter() = {
                New SqlParameter("@RPDescription", RPDescription),
                New SqlParameter("@RPFenceLineDistanceMeasure", RPFenceLineDistanceMeasure),
                New SqlParameter("@RPFugitiveHeightMeasure", RPFugitiveHeightMeasure),
                New SqlParameter("@RPFugitiveWidthMeasure", RPFugitiveWidthMeasure),
                New SqlParameter("@RPFugitiveLengthMeasure", RPFugitiveLengthMeasure),
                New SqlParameter("@RPFugitiveAngleMeasure", RPFugitiveAngleMeasure),
                New SqlParameter("@RPComment", RPComment),
                New SqlParameter("@Active", Active),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@FugitiveID", FugitiveID),
                New SqlParameter("@RPStatusCode", RPStatusCode),
                New SqlParameter("@FugitiveStatusCodeYear", FugitiveStatusCodeYear)
            }

            DB.RunCommand(SQL, params)

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub SaveFugitiveStackGCinfo()
        Dim HorizontalAccuracyMeasure = ParseAsNullableInteger(TxtHorizontalAccuracyMeasure.Text)
        Dim HCD As String = ddlHorCollectionMetCode.SelectedItem.Value
        Dim HRD As String = ddlHorReferenceDatCode.SelectedItem.Value
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim StackID As String = Left(txtReleasePointID.Text.ToUpper, 6)

        Try
            Dim query = "SELECT convert(BIT, count(*)) " &
                " FROM eis_RPGeocoordinates " &
                " WHERE FacilitySiteID = @FacilitySiteID " &
                "      AND ReleasePointID = @StackID "

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@StackID", StackID),
                New SqlParameter("@numLatitude", TxtLatitudeMeasure.Text),
                New SqlParameter("@numLongitude", TxtLongitudeMeasure.Text),
                New SqlParameter("@HorizontalAccuracyMeasure", HorizontalAccuracyMeasure),
                New SqlParameter("@HCD", HCD),
                New SqlParameter("@HRD", HRD),
                New SqlParameter("@GeographicComment", Left(TxtGeographicComment.Text, 200)),
                New SqlParameter("@UpdateUser", UpdateUser)
            }

            If Not DB.GetBoolean(query, params) AndAlso TxtLatitudeMeasure.Text <> "" AndAlso TxtLongitudeMeasure.Text <> "" Then
                query = "Insert into eis_RPGeocoordinates (" &
                " FacilitySiteID, " &
                " ReleasePointID, " &
                " numLatitudeMeasure, " &
                " numLongitudeMeasure, " &
                " intHorAccuracyMeasure, " &
                " strHorCollMetCode, " &
                " strHorRefDatumCode, " &
                " strGeographicComment, " &
                " Active, " &
                " UpdateUser, " &
                " UpdateDateTime, " &
                " CreateDateTime) " &
                " values (" &
                " @FacilitySiteID, " &
                " @StackID, " &
                " @numLatitude, " &
                " @numLongitude, " &
                " @HorizontalAccuracyMeasure, " &
                " @HCD, " &
                " @HRD, " &
                " @GeographicComment, " &
                " '1', " &
                " @UpdateUser, " &
                " getdate(), " &
                " getdate())"

                DB.RunCommand(query, params)

                Exit Sub
            End If

            Dim gcUpdated As Boolean =
                hidLatitude.Value <> TxtLatitudeMeasure.Text OrElse
                hidLongitude.Value <> TxtLongitudeMeasure.Text OrElse
                hidHorCollectionMetCode.Value <> ddlHorCollectionMetCode.SelectedValue OrElse
                hidHorizontalAccuracyMeasure.Value <> TxtHorizontalAccuracyMeasure.Text OrElse
                hidHorReferenceDatCode.Value <> ddlHorReferenceDatCode.SelectedValue OrElse
                hidGeographicComment.Value <> TxtGeographicComment.Text

            If gcUpdated Then

                ' Update database

                query = "Update EIS_RPGEOCOORDINATES Set NUMLATITUDEMEASURE = @numLatitude, " &
                    "NUMLONGITUDEMEASURE = @numLongitude, " &
                    "INTHORACCURACYMEASURE = @HorizontalAccuracyMeasure, " &
                    "STRHORCOLLMETCODE = @HCD, " &
                    "STRHORREFDATUMCODE = @HRD, " &
                    "STRGEOGRAPHICCOMMENT = @GeographicComment, " &
                    "ACTIVE = 1, " &
                    "UpdateUser = @UpdateUser, " &
                    "UpdateDateTime = getdate() " &
                    "where FACILITYSITEID = @FacilitySiteID " &
                    "and ReleasePointID = @StackID "

                DB.RunCommand(query, params)

                ' Email APB if any Geographic Coordinate Information changed

                Dim curGoogleMapLink As String = "none"
                If Decimal.TryParse(hidLatitude.Value, Nothing) AndAlso Decimal.TryParse(hidLongitude.Value, Nothing) Then
                    curGoogleMapLink = GoogleMaps.GetMapLinkUrl(New Coordinate(hidLatitude.Value, hidLongitude.Value))
                End If

                Dim newGoogleMapLink As String = "none"
                If Decimal.TryParse(TxtLatitudeMeasure.Text, Nothing) AndAlso Decimal.TryParse(TxtLongitudeMeasure.Text, Nothing) Then
                    newGoogleMapLink = GoogleMaps.GetMapLinkUrl(New Coordinate(TxtLatitudeMeasure.Text, TxtLongitudeMeasure.Text))
                End If

                Dim plainBody As String = "An update of EIS Geographic Coordinate Information has been submitted for " &
                    "the following release point. The updated information HAS BEEN SAVED in the database. APB review of " &
                    "the change is still recommended. " & vbNewLine &
                    vbNewLine &
                    "Facility Site ID: " & FacilitySiteID & vbNewLine &
                    vbNewLine &
                    "Release Point ID: " & StackID & vbNewLine &
                    vbNewLine &
                    "Update User: " & UpdateUserName & " (" & UpdateUserID & ")" & vbNewLine &
                    vbNewLine &
                    "Previous Geographic Coordinate Information: " & vbNewLine &
                    vbNewLine &
                    "    Latitude: " & hidLatitude.Value & vbNewLine &
                    "    Longitude: " & hidLongitude.Value & vbNewLine &
                    "    Horizontal Collection Method: " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & vbNewLine &
                    "    Accuracy Measure: " & hidHorizontalAccuracyMeasure.Value & vbNewLine &
                    "    Horizontal Reference Datum: " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & vbNewLine &
                    "    Google Map: " & curGoogleMapLink & vbNewLine &
                    vbNewLine &
                    "Updated Geographic Coordinate Information submitted by user: " & vbNewLine &
                    vbNewLine &
                    "    Latitude: " & TxtLatitudeMeasure.Text & vbNewLine &
                    "    Longitude: " & TxtLongitudeMeasure.Text & vbNewLine &
                    "    Horizontal Collection Method: " & ddlHorCollectionMetCode.SelectedValue & " - " & ddlHorCollectionMetCode.SelectedItem.Text & vbNewLine &
                    "    Accuracy Measure: " & TxtHorizontalAccuracyMeasure.Text & vbNewLine &
                    "    Horizontal Reference Datum: " & ddlHorReferenceDatCode.SelectedValue & " - " & ddlHorReferenceDatCode.SelectedItem.Text & vbNewLine &
                    "    Google Map: " & newGoogleMapLink & vbNewLine &
                    vbNewLine &
                    "Comment submitted by user: " & vbNewLine &
                    vbNewLine &
                    TxtGeographicComment.Text & vbNewLine

                Dim htmlBody As String = "<p>An update of EIS Geographic Coordinate Information has been submitted for " &
                    "the following release point. The updated information <em>HAS BEEN SAVED</em> in the database. APB review of " &
                    "the change is still recommended.</p>" &
                    "<p><b>Facility Site ID:</b> " & FacilitySiteID & "</p>" &
                    "<p><b>Release Point ID:</b> " & StackID & "</p>" &
                    "<p><b>Update User:</b> " & UpdateUserName & " (" & UpdateUserID & ")" & "</p>" &
                    "<p><b>Previous Geographic Coordinate Information:</b> " & "</p>" &
                    "<ul>" &
                    "<li><b>Latitude:</b> " & hidLatitude.Value & "</li>" &
                    "<li><b>Longitude:</b> " & hidLongitude.Value & "</li>" &
                    "<li><b>Horizontal Collection Method:</b> " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & "</li>" &
                    "<li><b>Accuracy Measure:</b> " & hidHorizontalAccuracyMeasure.Value & "</li>" &
                    "<li><b>Horizontal Reference Datum:</b> " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & "</li>" &
                    "<li><b>Google Map:</b> " & curGoogleMapLink & "</li>" &
                    "</ul>" &
                    "<p><b>Updated Geographic Coordinate Information submitted by user:</b> " & "</p>" &
                    "<ul>" &
                    "<li><b>Latitude:</b> " & TxtLatitudeMeasure.Text & "</li>" &
                    "<li><b>Longitude:</b> " & TxtLongitudeMeasure.Text & "</li>" &
                    "<li><b>Horizontal Collection Method:</b> " & ddlHorCollectionMetCode.SelectedValue & " - " & ddlHorCollectionMetCode.SelectedItem.Text & "</li>" &
                    "<li><b>Accuracy Measure:</b> " & TxtHorizontalAccuracyMeasure.Text & "</li>" &
                    "<li><b>Horizontal Reference Datum:</b> " & ddlHorReferenceDatCode.SelectedValue & " - " & ddlHorReferenceDatCode.SelectedItem.Text & "</li>" &
                    "<li><b>Google Map:</b> " & newGoogleMapLink & "</li>" &
                    "</ul>" &
                    "<p><b>Comment submitted by user:</b> " & "</p>" &
                    "<blockquote><pre>" & TxtGeographicComment.Text & "</pre></blockquote>"

                SendEmail(GecoContactEmail, "GECO Emission Inventory - Release Point Geographic Info Updated", plainBody, htmlBody,
                          caller:="eis_fugitive_edit.SaveFugitiveStackGCinfo")

            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnSaveFugitive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveFugitive1.Click, btnSaveFugitive2.Click

        lblFugitiveMessage.Visible = False
        SaveFugitiveStackInfo()
        SaveFugitiveStackGCinfo()
        lblFugitiveMessage.Text = SaveFugitive
        lblFugitiveMessage.Visible = True
        lblFugitiveMessage2.Text = SaveFugitive
        lblFugitiveMessage2.Visible = True
        lblFugitiveGCDataMissing.Text = ""

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim FugitiveId As String = txtReleasePointID.Text
        Dim targetpage As String = "fugitive_details.aspx" & "?fug=" & FugitiveId

        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnCancel3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel3.Click
        Dim FugitiveId As String = txtReleasePointID.Text
        Dim targetpage As String = "fugitive_details.aspx" & "?fug=" & FugitiveId

        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnReturnToSummary_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturnToSummary.Click
        Dim targetpage As String = "releasepoint_summary.aspx"

        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnReturnSummary2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturnSummary2.Click
        Dim targetpage As String = "releasepoint_summary.aspx"

        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnDeleteOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteOK.Click
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim FugitiveID As String = txtReleasePointID.Text.ToUpper
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        DeleteReleasePoint(FacilitySiteID, FugitiveID, UpdateUser)
        lblDeleteFugitive.Text = "Fugitive " & FugitiveID & " has been deleted."
        btnDeleteCancel.Visible = False
        btnDeleteOK.Visible = False
        btnDeleteSummary.Visible = True
        mpeDelete.Show()
    End Sub

    'Added by Mahesh for google maps

    Protected Sub GetFacilityCoordinates()
        Try
            Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

            'Load Facility GC Information
            Dim query As String = "Select numLatitudeMeasure, " &
                "numLongitudeMeasure, " &
                "STRHORCOLLMETCode, " &
                "INTHORACCURACYMEASURE , " &
                "STRHORREFDATUMCode, " &
                "strGeographicComment " &
                "FROM EIS_FacilityGeoCoord " &
                "where EIS_FacilityGeoCoord.FACILITYSITEID = @FacilitySiteID "

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then

                If Not IsDBNull(dr("numLatitudeMeasure")) Then
                    hidLatitude.Value = dr.Item("numLatitudeMeasure")
                End If
                If Not IsDBNull(dr("numLongitudeMeasure")) Then
                    hidLongitude.Value = dr.Item("numLongitudeMeasure")
                End If

            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub lbtnGetLatLon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnGetLatLon.Click
        Try
            lbtnGetLatLon_ModalPopupExtender.Show()
            GMap.ApiKey = ConfigurationManager.AppSettings("GoogleMapsAPIKey")
            Dim latitude, longitude As Double

            If TxtLatitudeMeasure.Text = "" Or TxtLongitudeMeasure.Text = "" Then
                'Get Co-ordinates from facility for showing Google map
                GetFacilityCoordinates()
                latitude = CDbl(hidLatitude.Value)
                longitude = CDbl(hidLongitude.Value)
            Else
                latitude = CDbl(TxtLatitudeMeasure.Text)
                longitude = CDbl(TxtLongitudeMeasure.Text)
            End If

            If latitude <> 0 And longitude <> 0 Then
                GMap.Center = LatLng.Create(latitude, longitude)
                txtMapLat.Text = Left(latitude.ToString(), 8)
                txtMapLon.Text = Left(longitude.ToString(), 9)
            End If
            GMap.MapControls.Add(New Controls.ScaleControl())
            GMap.MapControls.Add(New Controls.ZoomControl())
            GMap.MapControls.Add(New Controls.MapTypeControl())
            GMap.Zoom = 15

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub GMap_Click(ByVal sender As Object, ByVal e As CoordinatesEventArgs) Handles GMap.Click
        GMap.Overlays.Clear()

        Dim myOverlay As New Marker(New Guid(), e.Coordinates.Latitude, e.Coordinates.Longitude)
        GMap.Overlays.Add(myOverlay)

        Dim Mapcommand As String = GMap.UpdateOverlays()
        Mapcommand &= String.Format("document.getElementById('{0}').value = " & e.Coordinates.Latitude & ";", txtMapLat.ClientID)
        Mapcommand &= String.Format("document.getElementById('{0}').value = " & e.Coordinates.Longitude & ";", txtMapLon.ClientID)

        e.MapCommand = Mapcommand
    End Sub

    Protected Sub btnUseLatLon_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUseLatLon.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim LatitudeText As String = txtMapLat.Text
        Dim LongitudeText As String = txtMapLon.Text
        Dim FacilityLatitude As String
        Dim FacilityLongitude As String

        Try
            If Len(LatitudeText) > 8 Then
                LatitudeText = Left(LatitudeText, 8)
                TxtLatitudeMeasure.Text = LatitudeText
            Else
                TxtLatitudeMeasure.Text = LatitudeText
            End If

            If Len(LongitudeText) > 9 Then
                LongitudeText = Left(LongitudeText, 9)
                TxtLongitudeMeasure.Text = LongitudeText
            Else
                TxtLongitudeMeasure.Text = LongitudeText
            End If

            ddlHorCollectionMetCode.SelectedValue = "007"
            TxtHorizontalAccuracyMeasure.Text = "25"
            ddlHorReferenceDatCode.SelectedValue = "002"

            If TxtLatitudeMeasure.Text <> "" And TxtLongitudeMeasure.Text <> "" Then
                FacilityLatitude = TxtLatitudeMeasure.Text
                FacilityLongitude = TxtLongitudeMeasure.Text
            Else
                FacilityLatitude = GetFacilityLatitude(FacilitySiteID)
                FacilityLongitude = GetFacilityLongitude(FacilitySiteID)
            End If
            imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(FacilityLatitude, FacilityLongitude))

        Catch ex As Exception
            ErrorReport(ex)
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

#End Region

End Class