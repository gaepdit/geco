Imports System.Data.SqlClient
Imports GECO.MapHelper
Imports Reimers.Google.Map

Partial Class eis_stack_edit
    Inherits Page

    Private ReadOnly StackGCMessage As String = "Stack geographic coordinate data is missing one or more elements. Correct and save."
    Private StackEISSubmit As Boolean
    Private RPFlowRateInRange As Boolean
    Private RPGASRateAndFlowPresent As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim stackID As String = Request.QueryString("stk").ToUpper

        FIAccessCheck(EISAccessCode)

        If Not ReleasePointExists(FacilitySiteID, stackID) Then
            Throw New HttpException(404, "Not found")
        End If

        If Not IsPostBack Then
            loadStackStatusDDL()
            loadStackTypeDDL()
            LoadHorCollectDDL()
            LoadHorRefDatumDDL()
            LoadStackGCIValidation()
            loadStackInfo(FacilitySiteID, stackID) 'Loads Stack Info and sets StackEISSubmit based on EIS_ReleasePoint.strEISSubmit value
            LoadRPApportionment(FacilitySiteID, stackID)
            Dim StackUsedInRPA As Boolean = CheckRPApportionment(FacilitySiteID, stackID)
            Dim StackGCDataMissing As Boolean = CheckRPGCData(FacilitySiteID, stackID)

            If StackGCDataMissing Then
                lblStackGCDataMissing.Text = StackGCMessage
                lblStackMessage.Text = StackGCMessage
                TxtLatitudeMeasure.Focus()
            Else
                lblStackGCDataMissing.Text = ""
            End If

            If Not StackEISSubmit AndAlso Not StackUsedInRPA Then
                btnDelete.Visible = True
            Else
                btnDelete.Visible = False
            End If
            lblDeleteStack.Text = "Do you want to delete Stack " & stackID & "?"
            btnDeleteSummary.Visible = False

            txtMapLat.Attributes.Add("readonly", "readonly")
            txtMapLon.Attributes.Add("readonly", "readonly")

        End If

        HideTextBoxBorders(Me)
    End Sub

    Private Sub loadStackStatusDDL()
        ddlStackStatusCode.Items.Add("--Select Operating Status--")
        Try
            Dim query As String = "select strDesc, RPStatusCode FROM EISLK_RPSTATUSCODE where Active = '1'"

            Dim dt As DataTable = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("RPStatusCode")
                    }
                    ddlStackStatusCode.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub loadStackTypeDDL()
        ddlRPtypeCode.Items.Add("--Select Stack Type--")
        Try
            Dim query As String = "select strdesc, RPTypeCode FROM EISLK_RPTYPECODE " &
                "where EISLK_RPTYPECODE.active = '1' " &
                "and EISLK_RPTYPECODE.strdesc <> 'Fugitive' order by strdesc"

            Dim dt As DataTable = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("RPTypeCode")
                    }
                    ddlRPtypeCode.Items.Add(newListItem)
                Next
            End If
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

    Private Sub LoadStackGCIValidation()
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

    Private Sub loadStackInfo(ByVal fsid As String, ByVal RPid As String)

        Dim RPFenceLineDistanceMeasure As Decimal
        Dim RPExitGasVelocityMeasure As Decimal
        Dim RPExitGasFlowRateMeasure As Decimal
        Dim RPExitGasTemperatureMeasure As Decimal
        Dim RPStackHeightMeasure As Decimal
        Dim RPStackDiameterMeasure As Decimal
        Dim RPHorAccMeasure As Decimal
        Dim EISSubmit As String

        Try
            Dim query As String = "select ReleasePointID, " &
                "strRPDescription, " &
                "strRPTypeCode, " &
                "strRPStatusCode, " &
                "numRPStatusCodeYear, " &
                "numRPExitGasVelocityMeasure, " &
                "numRPExitGasFlowRateMeasure, " &
                "NUMRPEXITGASTEMPMEASURE, " &
                "NUMRPFENCELINEDISTMEASURE, " &
                "NUMRPSTACKHEIGHTMEASURE, " &
                "NUMRPSTACKDIAMETERMEASURE, " &
                "strRPComment, " &
                "strEISSubmit " &
                "FROM EIS_ReleasePoint where EIS_ReleasePoint.FACILITYSITEID = @fsid " &
                "and RELEASEPOINTID = @RPid "

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@RPid", RPid)
            }

            Dim dr As DataRow = DB.GetDataRow(query, params)

            If dr IsNot Nothing Then

                'Load Stack Release Point Details
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
                    ddlRPtypeCode.SelectedValue = ""
                Else
                    ddlRPtypeCode.SelectedValue = dr.Item("strRPTypeCode")
                End If

                If IsDBNull(dr("strRPStatusCode")) Then
                    ddlStackStatusCode.SelectedValue = ""
                Else
                    ddlStackStatusCode.SelectedValue = dr.Item("strRPStatusCode")
                    txtStackStatusCodeOnLoad.Text = ddlStackStatusCode.SelectedValue
                End If

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
                Else
                    txtRPComment.Text = dr.Item("strRPComment")
                End If

                If IsDBNull(dr.Item("strEISSubmit")) Then
                    StackEISSubmit = False
                Else
                    EISSubmit = dr.Item("strEISSubmit")
                    If EISSubmit = "0" Then
                        StackEISSubmit = False
                    Else
                        StackEISSubmit = True
                    End If
                End If
            End If

            'Load Stack GC Information
            query = "Select numLatitudeMeasure, " &
                "numLongitudeMeasure, " &
                "STRHORCOLLMETCode, " &
                "INTHORACCURACYMEASURE , " &
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
                    ddlHorCollectionMetCode.SelectedValue = ""
                Else
                    ddlHorCollectionMetCode.SelectedValue = dr3.Item("STRHORCOLLMETCode")
                End If

                If IsDBNull(dr3("INTHORACCURACYMEASURE")) Then
                    TxtHorizontalAccuracyMeasure.Text = ""
                Else
                    RPHorAccMeasure = dr3.Item("INTHORACCURACYMEASURE")
                    If RPHorAccMeasure = -1 Then
                        TxtHorizontalAccuracyMeasure.Text = ""
                    Else
                        TxtHorizontalAccuracyMeasure.Text = RPHorAccMeasure
                    End If
                End If

                If IsDBNull(dr3("STRHORREFDATUMCode")) Then
                    ddlHorReferenceDatCode.SelectedValue = ""
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

                If TxtLatitudeMeasure.Text <> "" AndAlso TxtLongitudeMeasure.Text <> "" Then
                    MapLatitude = TxtLatitudeMeasure.Text
                    MapLongitude = TxtLongitudeMeasure.Text
                Else
                    MapLatitude = GetFacilityLatitude(fsid)
                    MapLongitude = GetFacilityLongitude(fsid)
                End If

                imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(MapLatitude, MapLongitude))
                lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(New Coordinate(MapLatitude, MapLongitude))

            Else
                lblStackMessage.Text = "Release point geographic coordinate info incomplete."
                lblStackMessage.Visible = True
                lblStackGCDataMissing.Text = StackGCMessage
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
                "and eis_process.facilitysiteid= @FacilitySiteID " &
                "and eis_rpapportionment.releasepointid= @RPid " &
                " and EIS_RPAPPORTIONMENT.ACTIVE = '1' " &
                "and eis_process.Active = '1'"

        SqlDataSourceRPApp.SelectParameters.Add("FacilitySiteID", fsid)
        SqlDataSourceRPApp.SelectParameters.Add("RPid", RPid)

        gvwRPApportionment.DataBind()

        If gvwRPApportionment.Rows.Count = 0 Then
            lblReleasePointAppMessage.Text = "The Release Point is not used in any Process Release Point Apportionments and can be deleted on the Edit page."
            lblReleasePointAppMessage.Visible = True
            ddlStackStatusCode.Enabled = True
        Else
            lblReleasePointAppMessage.Text = "The Release Point cannot be deleted. Either delete the process or add another release point to the apportionment " &
                "before deleting the remaining release point. See Help for more details."
            lblReleasePointAppMessage.Visible = True
        End If

    End Sub

    Protected Sub StackStatusChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlStackStatusCode.SelectedIndexChanged
        'This sets the value of the textbox txtstackStatusChanged if the stack status is changed.
        'If the stack status is different than the value when the page loads, the status date must
        'be updated in the update subroutine

        txtStackStatusCodeChanged.Text = ddlStackStatusCode.SelectedItem.Value

    End Sub

    ' Custom validators

    Protected Sub FlowRateRangeAndGasVelocityCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs) Handles custRPExitGASVelocityMeasure.ServerValidate
        If txtRPExitGasVelocityMeasure.Text = "" AndAlso txtRPExitGasFlowRateMeasure.Text = "" Then
            RPGASRateAndFlowPresent = False
            args.IsValid = False
            custRPExitGASVelocityMeasure.ErrorMessage = "Either exit gas velocity or exit gas flow rate is required."
            custRPExitGASVelocityMeasure.Text = "Either exit gas velocity or exit gas flow rate is required."
            sumvStack.ShowSummary = True
        Else
            RPGASRateAndFlowPresent = True
            RPFlowRateInRange = True
        End If
    End Sub

    Protected Sub FlowRateRangeCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs) Handles cusvRPExitGasFlowRateMeasure.ServerValidate
        Dim StackDiameter As String = txtRPStackDiameterMeasure.Text
        Dim StackVelocity As String = txtRPExitGasVelocityMeasure.Text
        Dim StackFlowRate As Decimal = args.Value

        lblStackMessage.Visible = False

        If StackDiameter = "" OrElse StackVelocity = "" Then
            Exit Sub
        End If

        Dim StackFlowRateMinMax As MinMaxValues = GetRPMinMaxFlowRate(StackDiameter, StackVelocity)
        Dim CalculatedExitGasVelocity As Decimal = CalculateVelocity(StackDiameter, StackFlowRate)

        RPFlowRateInRange = False
        args.IsValid = False

        If StackFlowRate < StackFlowRateMinMax.MinValue OrElse StackFlowRate > StackFlowRateMinMax.MaxValue Then
            cusvRPExitGasFlowRateMeasure.ErrorMessage = "Stack flow rate is outside of expected range based on stack diameter and velocity: " & StackFlowRateMinMax.MinValue & " acfs to " & StackFlowRateMinMax.MaxValue & " acfs."
            cusvRPExitGasFlowRateMeasure.Text = "Stack flow rate is outside of expected range: " & StackFlowRateMinMax.MinValue & " to " & StackFlowRateMinMax.MaxValue & " acfs."
            sumvStack.ShowSummary = True
        ElseIf StackFlowRate < 0.00000001 OrElse StackFlowRate > 200000 Then
            cusvRPExitGasFlowRateMeasure.ErrorMessage = "Stack flow rate is outside of the allowed range of 0.00000001 to 200,000 acfs."
            cusvRPExitGasFlowRateMeasure.Text = "Stack flow rate is outside of allowed range of 0.1 to 200,000 acfs."
            sumvStack.ShowSummary = True
        ElseIf CalculatedExitGasVelocity < 0.001 OrElse CalculatedExitGasVelocity > 1500 Then
            cusvRPExitGasFlowRateMeasure.ErrorMessage = "Based on the stack diameter and flow rate entered, the calculated exit " &
                "gas velocity (" & CalculatedExitGasVelocity.ToString & ") is outside of the allowed range of 0.001 to 1500 FPS."
            cusvRPExitGasFlowRateMeasure.Text = "Calculated exit gas velocity is outside of the allowed range of 0.001 to 1500 FPS."
            sumvStack.ShowSummary = True
        Else
            RPFlowRateInRange = True
            args.IsValid = True
        End If

    End Sub

    Private Sub saveStackInfo()
        Dim query As String
        Dim RPDescription As String = txtRPDescription.Text
        Dim RPTypeCode As String = ddlRPtypeCode.SelectedItem.Value
        Dim RPStatusCode As String = ddlStackStatusCode.SelectedItem.Value
        Dim RPFenceLineDistanceMeasure As String = txtRPFenceLineDistanceMeasure.Text
        Dim RPStackHeightMeasure As String = txtRPStackHeightMeasure.Text
        Dim RPStackDiameterMeasure As String = txtRPStackDiameterMeasure.Text
        Dim RPExitGasVelocityMeasure As String = txtRPExitGasVelocityMeasure.Text
        Dim RPExitGasFlowRateMeasure As String = txtRPExitGasFlowRateMeasure.Text
        Dim RPExitGasTempMeasure As String = txtRPExitGasTemperatureMeasure.Text
        Dim RPComment As String = txtRPComment.Text
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim StackID As String = txtReleasePointID.Text
        Dim Active As Integer = 1
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        Dim StatusCodeOnLoad = txtStackStatusCodeOnLoad.Text
        Dim StatusCodeChanged = txtStackStatusCodeChanged.Text

        'Truncate strings
        RPComment = Left(RPComment, 400)
        RPDescription = Left(RPDescription, 100)
        StackID = Left(StackID, 6)

        Try
            If RPFlowRateInRange Then
                If StatusCodeOnLoad = StatusCodeChanged OrElse StatusCodeChanged = "" Then
                    'Does not update Stack Status and Stack Status Code year
                    query = "Update EIS_RELEASEPOINT Set " &
                        "STRRPTYPECODE = @RPTypeCode, " &
                        "STRRPDESCRIPTION = @RPDescription, " &
                        "NUMRPFENCELINEDISTMEASURE = @RPFenceLineDistanceMeasure, " &
                        "NUMRPEXITGASVELOCITYMEASURE = @RPExitGasVelocityMeasure, " &
                        "NUMRPEXITGASFLOWRATEMEASURE = @RPExitGasFlowRateMeasure, " &
                        "NUMRPEXITGASTEMPMEASURE = @RPExitGasTempMeasure, " &
                        "NUMRPSTACKHEIGHTMEASURE = @RPStackHeightMeasure, " &
                        "NUMRPSTACKDIAMETERMEASURE = @RPStackDiameterMeasure, " &
                        "STRRPCOMMENT = @RPComment, " &
                        "Active = @Active, " &
                        "UpdateUser = @UpdateUser, " &
                        "UpdateDateTime = getdate() " &
                        "where FACILITYSITEID = @FacilitySiteID " &
                        "and ReleasePointID = @StackID "
                Else
                    'update stack Status and Unit Status Code year
                    query = "Update EIS_RELEASEPOINT Set " &
                        "STRRPTYPECODE = @RPTypeCode, " &
                        "STRRPDESCRIPTION = @RPDescription, " &
                        "NUMRPFENCELINEDISTMEASURE = @RPFenceLineDistanceMeasure, " &
                        "NUMRPEXITGASVELOCITYMEASURE = @RPExitGasVelocityMeasure, " &
                        "NUMRPEXITGASFLOWRATEMEASURE = @RPExitGasFlowRateMeasure, " &
                        "NUMRPEXITGASTEMPMEASURE = @RPExitGasTempMeasure, " &
                        "NUMRPSTACKHEIGHTMEASURE = @RPStackHeightMeasure, " &
                        "NUMRPSTACKDIAMETERMEASURE = @RPStackDiameterMeasure, " &
                        "STRRPSTATUSCODE = @RPStatusCode, " &
                        "NUMRPSTATUSCODEYEAR = @StackStatusCodeYear, " &
                        "STRRPCOMMENT = @RPComment, " &
                        "ACTIVE = @Active, " &
                        "UpdateUser = @UpdateUser, " &
                        "UpdateDateTime = getdate() " &
                        "where FACILITYSITEID = @FacilitySiteID " &
                        "and ReleasePointID = @StackID "
                End If

                Dim params As SqlParameter() = {
                    New SqlParameter("@RPTypeCode", RPTypeCode),
                    New SqlParameter("@RPDescription", RPDescription),
                    New SqlParameter("@RPFenceLineDistanceMeasure", If(Not String.IsNullOrEmpty(RPFenceLineDistanceMeasure), RPFenceLineDistanceMeasure, Nothing)),
                    New SqlParameter("@RPExitGasVelocityMeasure", If(Not String.IsNullOrEmpty(RPExitGasVelocityMeasure), RPExitGasVelocityMeasure, Nothing)),
                    New SqlParameter("@RPExitGasFlowRateMeasure", If(Not String.IsNullOrEmpty(RPExitGasFlowRateMeasure), RPExitGasFlowRateMeasure, Nothing)),
                    New SqlParameter("@RPExitGasTempMeasure", If(Not String.IsNullOrEmpty(RPExitGasTempMeasure), RPExitGasTempMeasure, Nothing)),
                    New SqlParameter("@RPStackHeightMeasure", If(Not String.IsNullOrEmpty(RPStackHeightMeasure), RPStackHeightMeasure, Nothing)),
                    New SqlParameter("@RPStackDiameterMeasure", If(Not String.IsNullOrEmpty(RPStackDiameterMeasure), RPStackDiameterMeasure, Nothing)),
                    New SqlParameter("@RPStatusCode", RPStatusCode),
                    New SqlParameter("@StackStatusCodeYear", Now.Year),
                    New SqlParameter("@RPComment", RPComment),
                    New SqlParameter("@Active", Active),
                    New SqlParameter("@UpdateUser", UpdateUser),
                    New SqlParameter("@FacilitySiteID", FacilitySiteID),
                    New SqlParameter("@StackID", StackID)
                }

                DB.RunCommand(query, params)
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub SaveStackGCinfo()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        txtReleasePointID.Text = Left(txtReleasePointID.Text, 6)
        Dim StackID As String = txtReleasePointID.Text
        Dim HCD As String = ddlHorCollectionMetCode.SelectedValue
        Dim HorizontalAccuracyMeasure As String = TxtHorizontalAccuracyMeasure.Text
        Dim HRD As String = ddlHorReferenceDatCode.SelectedValue
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

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
                New SqlParameter("@HorizontalAccuracyMeasure", If(Not String.IsNullOrEmpty(HorizontalAccuracyMeasure), HorizontalAccuracyMeasure, Nothing)),
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
                          caller:="eis_stack_edit.SaveStackGCinfo")
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnSaveStack1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveStack1.Click, btnSaveStack2.Click
        If Not RPFlowRateInRange OrElse Not RPGASRateAndFlowPresent Then
            'Do Nothing
        Else
            lblStackMessage.Visible = False
            saveStackInfo()
            SaveStackGCinfo()
            lblStackMessage.Text = "Stack Information saved successfully."
            lblStackMessage.Visible = True
            lblStackGCDataMissing.Text = ""
        End If
    End Sub


    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click, btnCancel3.Click
        Dim StackId As String = txtReleasePointID.Text
        Dim targetpage As String = "Stack_details.aspx" & "?stk=" & StackId

        Response.Redirect(targetpage)
    End Sub

    Protected Sub btnDeleteOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteOK.Click
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim StackID As String = txtReleasePointID.Text.ToUpper
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        DeleteReleasePoint(FacilitySiteID, StackID, UpdateUser)
        lblDeleteStack.Text = "Stack " & StackID & " has been deleted."
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

            If TxtLatitudeMeasure.Text = "" OrElse TxtLongitudeMeasure.Text = "" Then
                'Get Co-ordinates from facility for showing Google map
                GetFacilityCoordinates()
                latitude = CDbl(hidLatitude.Value)
                longitude = CDbl(hidLongitude.Value)
            Else
                latitude = CDbl(TxtLatitudeMeasure.Text)
                longitude = CDbl(TxtLongitudeMeasure.Text)
            End If

            If latitude <> 0 AndAlso longitude <> 0 Then
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

        Try
            TxtLatitudeMeasure.Text = Left(txtMapLat.Text, 8)
            TxtLongitudeMeasure.Text = Left(txtMapLon.Text, 8)

            ddlHorCollectionMetCode.SelectedValue = "007"
            TxtHorizontalAccuracyMeasure.Text = "25"
            ddlHorReferenceDatCode.SelectedValue = "002"

            Dim MapLatitude As Decimal
            Dim MapLongitude As Decimal

            If TxtLatitudeMeasure.Text <> "" AndAlso TxtLongitudeMeasure.Text <> "" Then
                MapLatitude = TxtLatitudeMeasure.Text
                MapLongitude = TxtLongitudeMeasure.Text
            Else
                MapLatitude = GetFacilityLatitude(FacilitySiteID)
                MapLongitude = GetFacilityLongitude(FacilitySiteID)
            End If

            imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(MapLatitude, MapLongitude))
            lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(New Coordinate(MapLatitude, MapLongitude))

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

End Class