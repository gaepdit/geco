Imports System.Data.SqlClient
Imports EpdIt.DBUtilities

Partial Class eis_rpapportionment_edit
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EmissionsUnitID As String = Request.QueryString("eu").ToUpper
        Dim ProcessID As String = Request.QueryString("ep").ToUpper

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then

            Dim RPApportionmentExists As Boolean = CheckAnyRPApportionment(FacilitySiteID, EmissionsUnitID, ProcessID)
            LoadRPApportionDetails(FacilitySiteID, EmissionsUnitID, ProcessID)

            If RPApportionmentExists Then
                LoadRPApportionmentGridView(FacilitySiteID, EmissionsUnitID, ProcessID)
                CheckRPApportionmentTotal(FacilitySiteID, EmissionsUnitID, ProcessID)
            Else
                lblRPApportionmentWarning.Text = "No release points added for this process."
                lblRPApportionmentTotal.Visible = False
                txtRPApportionmentTotal.Visible = False
            End If

        End If

        HideTextBoxBorders(Me)
    End Sub

    Private Sub LoadRPApportionDetails(ByVal fsid As String, ByVal euid As String, ByVal prid As String)
        Try
            Dim query As String = "select e.EMISSIONSUNITID,
                       e.STRUNITDESCRIPTION,
                       PROCESSID,
                       STRPROCESSDESCRIPTION
                from  EIS_PROCESS p
                inner join EIS_EMISSIONSUNIT e
                on e.EMISSIONSUNITID = p.EMISSIONSUNITID
                       and e.FACILITYSITEID = p.FACILITYSITEID
                where p.FACILITYSITEID = @fsid
                  and p.EMISSIONSUNITID = @euid
                  and p.PROCESSID = @prid"

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@prid", prid)
            }

            Dim dr1 As DataRow = DB.GetDataRow(query, params)

            If dr1 IsNot Nothing Then
                txtEmissionsUnitID.Text = GetNullableString(dr1.Item("EMISSIONSUNITID"))
                txtEmissionsUnitDesc.Text = GetNullableString(dr1.Item("strUnitDescription"))
                txtProcessID.Text = GetNullableString(dr1.Item("ProcessID"))
                txtProcessDesc.Text = GetNullableString(dr1.Item("strProcessDescription"))
            Else
                Throw New HttpException(404, "Not found")
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadRPApportionmentGridView(ByVal fsid As String, ByVal euid As String, ByVal prid As String)
        Try
            Dim query As String = "SELECT eis_RPApportionment.FacilitySiteID " &
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
                "WHERE eis_RPApportionment.FacilitySiteID = @fsid " &
                "AND eis_RPApportionment.EmissionsUnitID = @euid " &
                "AND eis_RPApportionment.ProcessID = @prid " &
                "AND eis_RPApportionment.Active = '1' " &
                "AND eislk_RPTypeCode.Active = '1' "

            Dim params As SqlParameter() = {
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@euid", euid),
                New SqlParameter("@prid", prid)
            }

            gvwRPApportionment.DataSource = DB.GetDataTable(query, params)
            gvwRPApportionment.DataBind()
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnCancel1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcessSummary.Click, btnCancel2.Click

        Dim eu As String = txtEmissionsUnitID.Text.ToUpper
        Dim ep As String = txtProcessID.Text.ToUpper
        Dim targetpage As String = "process_details.aspx" & "?eu=" & eu & "&ep=" & ep
        Response.Redirect(targetpage)

    End Sub

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

    Private Sub CheckRPApportionmentTotal(ByVal fsid As String, ByVal euid As String, ByVal prid As String)

        Dim RPApportionmentSum As Integer

        RPApportionmentSum = GetRPApportionmentTotal(fsid, euid, prid)
        txtRPApportionmentTotal.Text = RPApportionmentSum & "%"
        RPApportionmentCheck(RPApportionmentSum)

    End Sub

End Class