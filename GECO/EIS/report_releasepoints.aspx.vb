Imports System.Data.SqlClient

Partial Class EIS_report_releasepoints
    Inherits Page

    Public FacilitySiteID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim gvwStacksSize As Integer
        Dim gvwFugitivesSize As Integer

        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If Not IsPostBack Then

            HideFacilityInventoryMenu()
            HideEmissionInventoryMenu()
            HideSubmitMenu()

            txtFacilityName_ReleasePoints.Text = GetFacilityName(FacilitySiteID)
            txtFacilitySiteID_ReleasePoints.Text = FacilitySiteID

            loadFugitivesgvw()
            loadStacksgvw()

            gvwStacksSize = gvwStacks.Rows.Count
            gvwFugitivesSize = gvwFugitives.Rows.Count

            'Load/hide message and Export button if stacks gridview empty
            If gvwStacksSize <= 0 Then
                lblEmptygvwStacks.Text = "No stacks exist for this facility in the EIS"
                lblEmptygvwStacks.Visible = True
                btnExport_Stacks.Visible = False
            Else
                lblEmptygvwStacks.Text = ""
                lblEmptygvwStacks.Visible = False
                btnExport_Stacks.Visible = True
            End If

            'Load/hide message and Export button if fugitive gridview empty
            If gvwFugitivesSize <= 0 Then
                lblEmptygvwFugitives.Text = "No fugitive release points exist for this facility in the EIS"
                lblEmptygvwFugitives.Visible = True
                btExport_Fugitives.Visible = False
            Else
                lblEmptygvwFugitives.Text = ""
                lblEmptygvwFugitives.Visible = False
                btExport_Fugitives.Visible = True
            End If

        End If

    End Sub

    Private Sub loadFugitivesgvw()

        Dim query = "SELECT DISTINCT " &
            "eis_ReleasePoint.FacilitySiteID " &
            ", strRPDescription " &
            ", eis_ReleasePoint.ReleasePointID " &
            ", numRPFugitiveHeightMeasure " &
            ", numRPFugitiveWidthMeasure " &
            ", numRPFugitiveLengthMeasure " &
            ", numRPFugitiveAngleMeasure " &
            ", numRPFencelineDistMeasure " &
            ", numLatitudeMeasure " &
            ", numLongitudeMeasure " &
            ", intHorAccuracyMeasure " &
            ", eislk_HorCollMetCode.strDesc AS  HorCollMetDesc " &
            ", eislk_HorRefDatumCode.strDesc AS HorRefDatumDesc " &
            ", strRPComment " &
            ", eislk_RPStatusCode.strDesc AS    strRPStatusCode " &
            ", strGeographicComment " &
            ", eis_ReleasePoint.LastEISSubmitDate " &
            "FROM   eis_ReleasePoint " &
            "INNER JOIN EIS_RPGeoCoordinates " &
            "ON eis_ReleasePoint.FacilitySiteID = EIS_RPGeoCoordinates.FacilitySiteID " &
            "AND eis_ReleasePoint.ReleasePointId = EIS_RPGeoCoordinates.ReleasePointID " &
            "LEFT JOIN eislk_HorCollmetCode " &
            "ON EIS_RPGeoCoordinates.strHorcollMetCode = eislk_HorCollMetCode.HorCollMetCode " &
            "LEFT JOIN eislk_HorRefDatumCode " &
            "ON EIS_RPGeoCoordinates.strHorRefDatumCode = eislk_HorRefDatumCode.HorRefDatumCode " &
            "JOIN eislk_RPStatusCode " &
            "ON EIS_ReleasePoint.strRPStatusCode = eislk_RPStatusCode.RPStatusCode " &
            "WHERE  eis_ReleasePoint.FacilitySiteID = @FacilitySiteID " &
            "AND eis_ReleasePoint.strRPtypeCode = '1' " &
            "AND eis_ReleasePoint.Active = '1' " &
            "ORDER BY eis_ReleasePoint.ReleasePointID "

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

        gvwFugitives.DataSource = DB.GetDataTable(query, param)
        gvwFugitives.DataBind()

    End Sub

    Private Sub loadStacksgvw()

        Dim query = "SELECT DISTINCT " &
            "eis_ReleasePoint.FacilitySiteID " &
            ", eis_ReleasePoint.strRPDescription " &
            ", eis_ReleasePoint.ReleasePointID " &
            ", eis_ReleasePoint.numRPStackHeightMeasure " &
            ", eis_ReleasePoint.numRPStackDiameterMeasure " &
            ", eis_ReleasePoint.numRPExitGasVelocityMeasure " &
            ", eis_ReleasePoint.numRPExitGasFlowRateMeasure " &
            ", eis_ReleasePoint.numRPExitGasTempMeasure " &
            ", eis_ReleasePoint.numRPFenceLineDistMeasure " &
            ", eis_RPGeoCoordinates.numLatitudeMeasure " &
            ", eis_RPGeoCoordinates.numLongitudeMeasure " &
            ", eis_RPGeoCoordinates.intHorAccuracyMeasure " &
            ", eislk_HorCollMetCode.strDesc AS HorCollMetDesc " &
            ", eislk_HorRefDatumCode.strDesc AS HorRefDatumDesc " &
            ", eislk_RPTypeCode.strDesc AS RPTypeDesc " &
            ", eislk_RPStatusCode.strDesc AS RPStatusDesc " &
            ", eis_ReleasePoint.strRPComment " &
            ", eis_RPGeoCoordinates.strGeographicComment " &
            ", eis_ReleasePoint.LastEISSubmitDate " &
            "FROM eis_ReleasePoint " &
            "INNER JOIN EIS_RPGeoCoordinates " &
            "ON eis_ReleasePoint.FacilitySiteID = EIS_RPGeoCoordinates.FacilitySiteID " &
            "AND eis_ReleasePoint.ReleasePointID = EIS_RPGeoCoordinates.ReleasePointID " &
            "LEFT JOIN eislk_RPTypeCode " &
            "ON EIS_ReleasePoint.strRPTypeCode = eislk_RPTypeCode.RPTypeCode " &
            "LEFT JOIN eislk_HorCollmetCode " &
            "ON EIS_RPGeoCoordinates.strHorcollMetCode = eislk_HorCollMetCode.HorCollMetCode " &
            "LEFT JOIN eislk_HorRefDatumCode " &
            "ON EIS_RPGeoCoordinates.strHorRefDatumCode = eislk_HorRefDatumCode.HorRefDatumCode " &
            "LEFT JOIN eislk_RPStatusCode " &
            "ON EIS_ReleasePoint.strRPStatusCode = eislk_RPStatusCode.RPStatusCode " &
            "WHERE eis_ReleasePoint.FacilitySiteID = @FacilitySiteID " &
            "AND eis_ReleasePoint.strRPTypeCode <> '1' " &
            "AND eis_ReleasePoint.Active = '1' " &
            "ORDER BY eis_ReleasePoint.ReleasePointID "

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

        gvwStacks.DataSource = DB.GetDataTable(query, param)
        gvwStacks.DataBind()

    End Sub

    Protected Sub btnExport_Stacks_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport_Stacks.Click

        loadStacksgvw()
        ExportAsExcel("EISStacks", gvwStacks)

    End Sub

    Protected Sub btExport_Fugitives_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btExport_Fugitives.Click

        loadFugitivesgvw()
        ExportAsExcel("EISFugitives", gvwFugitives)

    End Sub

    Protected Sub btnReportsHome_ReleasePoints_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReportsHome_ReleasePoints.Click

        Response.Redirect("reports.aspx")

    End Sub

#Region "  Menu Routines  "

    Private Sub HideFacilityInventoryMenu()

        Dim menu = CType(Master.FindControl("pnlFacilityInventory"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

    Private Sub HideEmissionInventoryMenu()

        Dim menu = CType(Master.FindControl("pnlEmissionInventory"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

    Private Sub HideSubmitMenu()

        Dim menu = CType(Master.FindControl("pnlSubmit"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

        menu = CType(Master.FindControl("pnlReset"), Panel)
        If menu IsNot Nothing Then
            menu.Visible = False
        End If

    End Sub

#End Region

End Class