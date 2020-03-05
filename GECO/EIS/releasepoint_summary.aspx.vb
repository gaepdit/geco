Partial Class eis_releasepoint_summary
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then

            loadFugitivesgvw()
            loadStacksgvw()

            pnlDeletedRP.Visible = CheckDeletedRPExist(FacilitySiteID)

            HideTextBoxBorders(Me)
        End If

    End Sub

    Private Sub loadFugitivesgvw()

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        'Fugitive Release Point Gridview
        SqlDataSourceID1.ConnectionString = DBConnectionString

        SqlDataSourceID1.SelectCommand = "select " &
                "ReleasepointID, " &
                "strRPDescription, " &
                "(select strDesc FROM eislk_RPStatusCode where " &
                "eis_ReleasePoint.strRPStatusCode = eislk_RPStatusCode.RPStatusCode and " &
                "eislk_RPStatusCode.Active = '1') as RPStatus, " &
                "LastEISSubmitDate " &
                "from " &
                "eis_ReleasePoint " &
                "where " &
                "FacilitySiteID = @FacilitySiteID and " &
                "Active = '1' and " &
                "eis_ReleasePoint.strRPTypeCode = '1' " &
                "order by ReleasePointID"

        SqlDataSourceID1.SelectParameters.Add("FacilitySiteID", FacilitySiteID)

        gvwFugRPSummary.DataBind()

    End Sub

    Private Sub loadStacksgvw()

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        'Stack Release Point Gridview
        SqlDataSourceID2.ConnectionString = DBConnectionString

        SqlDataSourceID2.SelectCommand = "select " &
                    "ReleasePointID, strRPDescription, " &
                    "(select eislk_RPTypeCode.strDesc FROM eislk_RPTypeCode where " &
                    "eislk_RPTypeCode.RPTypeCode = eis_ReleasePoint.strRPTypeCode and eislk_RPTypeCode.Active = '1') as RPTypeCode, " &
                    "numRPStackHeightMeasure, " &
                    "numRPExitGasFlowRateMeasure, " &
                    "(select eislk_RPStatusCode.strDesc FROM eislk_RPStatusCode where " &
                    "eislk_RPStatusCode.RPStatusCode = eis_ReleasePoint.strRPStatusCode and eislk_RPStatusCode.Active = '1') as RPStatusCode, " &
                    "LastEISSubmitDate " &
                    "FROM eis_ReleasePoint " &
                    "where " &
                    "FacilitySiteID = @FacilitySiteID and " &
                    "Active = '1' and " &
                    "eis_ReleasePoint.strRPTypeCode <> '1' " &
                    "order by ReleasePointID "

        SqlDataSourceID2.SelectParameters.Add("FacilitySiteID", FacilitySiteID)

        gvwRPSummary.DataBind()

    End Sub

    Protected Sub btnShowDeletedRP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowDeletedRP.Click

        Dim btnText As String = Left(btnShowDeletedRP.Text, 4)

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        sqldsDeletedRP.ConnectionString = DBConnectionString

        If sqldsDeletedRP.SelectParameters.Count = 0 Then
            sqldsDeletedRP.SelectParameters.Add("FacilitySiteID", FacilitySiteID)
        End If

        If btnText = "Show" Then
            sqldsDeletedRP.SelectCommand = "select eis_ReleasePoint.ReleasePointID, eis_ReleasePoint.strRPDescription, " &
                "(select eislk_RPTypeCode.strDesc FROM eislk_RPTypeCode where eislk_RPTypeCode.RPTypeCode = eis_ReleasePoint.strRPTypeCode) as strRPType " &
                "FROM eis_ReleasePoint " &
                "where " &
                "FacilitySiteID = @FacilitySiteID " &
                "and Active = '0' " &
                "order by eis_ReleasePoint.ReleasePointID"

            btnShowDeletedRP.Text = "Hide Deleted Release Points"
        Else
            sqldsDeletedRP.SelectCommand = "select eis_ReleasePoint.ReleasePointID, eis_ReleasePoint.strRPDescription, " &
                "(select eislk_RPTypeCode.strDesc FROM eislk_RPTypeCode where eislk_RPTypeCode.RPTypeCode = eis_ReleasePoint.strRPTypeCode) as strRPType " &
                "FROM eis_ReleasePoint " &
                "where " &
                "FacilitySiteID = @FacilitySiteID " &
                "and Active = '999' " &
                "order by eis_ReleasePoint.ReleasePointID"

            btnShowDeletedRP.Text = "Show Deleted Release Points"
        End If

        gvwDeletedRP.DataBind()

    End Sub

End Class