Imports System.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_ReleasePoints
    Inherits Page

    Private Property FacilitySiteID As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(FacilitySiteID) Then
            Response.Redirect("~/")
        End If

        FacilitySiteID = New ApbFacilityId(FacilitySiteID)
        Master.CurrentAirs = FacilitySiteID
        Master.IsFacilitySet = True

        LoadFugitives()
        LoadStacks()
    End Sub

    Private Sub LoadFugitives()
        Dim query = "SELECT p.FacilitySiteID, p.strRPDescription, p.ReleasePointID, p.numRPFugitiveHeightMeasure,
                p.numRPFugitiveWidthMeasure, p.numRPFugitiveLengthMeasure, p.numRPFugitiveAngleMeasure,
                p.numRPFencelineDistMeasure, g.numLatitudeMeasure, g.numLongitudeMeasure, g.intHorAccuracyMeasure,
                lh.strDesc AS HorCollMetDesc, ld.strDesc AS HorRefDatumDesc, p.strRPComment,
                ls.strDesc AS strRPStatusCode, g.strGeographicComment, p.LastEISSubmitDate
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

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID.ShortString)
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
        Dim query = "SELECT p.FacilitySiteID, p.strRPDescription, p.ReleasePointID, p.numRPStackHeightMeasure, p.numRPStackDiameterMeasure,
                   p.numRPExitGasVelocityMeasure, p.numRPExitGasFlowRateMeasure, p.numRPExitGasTempMeasure,
                   p.numRPFenceLineDistMeasure, g.numLatitudeMeasure, g.numLongitudeMeasure, g.intHorAccuracyMeasure,
                   lh.strDesc AS HorCollMetDesc, ld.strDesc AS HorRefDatumDesc, lt.strDesc AS RPTypeDesc,
                   lr.strDesc AS RPStatusDesc, p.strRPComment, g.strGeographicComment, p.LastEISSubmitDate
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

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID.ShortString)
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
        ExportAsExcel($"{FacilitySiteID.ShortString}_Fugitive_Release_Points", Fugitives)
    End Sub

    Private Sub StacksExport_Click(sender As Object, e As EventArgs) Handles StacksExport.Click
        ExportAsExcel($"{FacilitySiteID.ShortString}_Stack_Release_Points", Stacks)
    End Sub

End Class
