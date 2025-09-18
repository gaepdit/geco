Imports Microsoft.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_ReleasePoints
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId

    Private IsTerminating As Boolean = False
    Protected Overrides Sub OnLoad(e As EventArgs)
        IsTerminating = MainLoginCheck()
        If IsTerminating Then Return
        MyBase.OnLoad(e)
    End Sub
    Protected Overrides Sub Render(writer As HtmlTextWriter)
        If IsTerminating Then Return
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            CompleteRedirect("~/", IsTerminating)
            Return
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.History

        LoadFugitives()
        LoadStacks()
    End Sub

    Private Sub LoadFugitives()
        Dim query = "SELECT p.ReleasePointID             as [Release Point ID],
                   p.strRPDescription           as [Description],
                   p.numRPFugitiveHeightMeasure as [Fugitive Height (ft)],
                   p.numRPFugitiveWidthMeasure  as [Fugitive Width (ft)],
                   p.numRPFugitiveLengthMeasure as [Fugitive Length (ft)],
                   p.numRPFugitiveAngleMeasure  as [Fugitive Angle (0° to 89°)],
                   p.numRPFenceLineDistMeasure  as [Fenceline Distance (ft)],
                   ls.strDesc                   as [Operating Status],
                   g.numLatitudeMeasure         as [Latitude],
                   g.numLongitudeMeasure        as [Longitude],
                   g.intHorAccuracyMeasure      as [Horiz Accuracy Measure (m)],
                   lh.strDesc                   as [Horiz Collection Method],
                   ld.strDesc                   as [Horiz Reference Datum],
                   p.LastEISSubmitDate          as [Last EPA Submittal],
                   p.strRPComment               as [Fugitive Comment],
                   g.strGeographicComment       as [Geo Coord Comment]
            FROM EIS_RELEASEPOINT p
                LEFT JOIN EIS_RPGEOCOORDINATES g
                ON p.FacilitySiteID = g.FacilitySiteID
                    AND p.ReleasePointId = g.ReleasePointID
                LEFT JOIN EISLK_HORCOLLMETCODE lh
                ON g.strHorcollMetCode = lh.HorCollMetCode
                LEFT JOIN EISLK_HORREFDATUMCODE ld
                ON g.strHorRefDatumCode = ld.HorRefDatumCode
                LEFT JOIN EISLK_RPSTATUSCODE ls
                ON p.strRPStatusCode = ls.RPStatusCode
            WHERE p.FacilitySiteID = @FacilitySiteID
              AND p.strRPtypeCode = '1'
              AND p.Active = '1'
            ORDER BY p.ReleasePointID"

        Dim param As New SqlParameter("@FacilitySiteID", CurrentAirs.ShortString)
        Dim dt As DataTable = DB.GetDataTable(query, param)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Fugitives.DataSource = dt
            Fugitives.DataBind()
        Else
            FugitivesEmptyNotice.Visible = True
            Fugitives.Visible = False
            FugitivesExport.Visible = False
        End If

    End Sub

    Private Sub LoadStacks()
        Dim query = "SELECT p.ReleasePointID              as [Release Point ID],
                   p.strRPDescription            as [Description],
                   lt.strDesc                    as [Stack Type],
                   p.numRPStackHeightMeasure     as [Stack Height (ft)],
                   p.numRPStackDiameterMeasure   as [Stack Diameter (ft)],
                   p.numRPExitGasVelocityMeasure as [Exit Gas Velocity (fps)],
                   p.numRPExitGasFlowRateMeasure as [Exit Gas Flow Rate (acfs)],
                   p.numRPExitGasTempMeasure     as [Exit Gas Temp (°F)],
                   p.numRPFenceLineDistMeasure   as [Fenceline Distance (ft)],
                   lr.strDesc                    as [Operating Status],
                   g.numLatitudeMeasure          as [Latitude],
                   g.numLongitudeMeasure         as [Longitude],
                   g.intHorAccuracyMeasure       as [Horiz Accuracy Measure (m)],
                   lh.strDesc                    as [Horiz Collection Method],
                   ld.strDesc                    as [Horiz Reference Datum],
                   p.LastEISSubmitDate           as [Last EPA Submittal],
                   p.strRPComment                as [Stack Comment],
                   g.strGeographicComment        as [Geo Coord Comment]
            FROM EIS_RELEASEPOINT p
                LEFT JOIN EIS_RPGEOCOORDINATES g
                ON p.FacilitySiteID = g.FacilitySiteID
                    AND p.ReleasePointID = g.ReleasePointID
                LEFT JOIN EISLK_RPTYPECODE lt
                ON p.strRPTypeCode = lt.RPTypeCode
                LEFT JOIN EISLK_HORCOLLMETCODE lh
                ON g.strHorcollMetCode = lh.HorCollMetCode
                LEFT JOIN EISLK_HORREFDATUMCODE ld
                ON g.strHorRefDatumCode = ld.HorRefDatumCode
                LEFT JOIN EISLK_RPSTATUSCODE lr
                ON p.strRPStatusCode = lr.RPStatusCode
            WHERE p.FacilitySiteID = @FacilitySiteID
              AND p.strRPTypeCode <> '1'
              AND p.Active = '1'
            ORDER BY p.ReleasePointID"

        Dim param As New SqlParameter("@FacilitySiteID", CurrentAirs.ShortString)
        Dim dt As DataTable = DB.GetDataTable(query, param)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Stacks.DataSource = dt
            Stacks.DataBind()
        Else
            StacksEmptyNotice.Visible = True
            Stacks.Visible = False
            StacksExport.Visible = False
        End If
    End Sub

    Private Sub FugitivesExport_Click(sender As Object, e As EventArgs) Handles FugitivesExport.Click
        ExportAsExcel($"{CurrentAirs.ShortString}_Fugitive_Release_Points", Fugitives)
    End Sub

    Private Sub StacksExport_Click(sender As Object, e As EventArgs) Handles StacksExport.Click
        ExportAsExcel($"{CurrentAirs.ShortString}_Stack_Release_Points", Stacks)
    End Sub

End Class
