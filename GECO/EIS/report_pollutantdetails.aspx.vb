Imports System.Data.SqlClient

Partial Class EIS_report_pollutantdetails
    Inherits Page

    Private FacilitySiteID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If Not IsPostBack Then
            txtFacilitySiteID_Pollutant.Text = FacilitySiteID
            txtFacilityName_Pollutant.Text = getFacilityName(FacilitySiteID)
            LoadYear()
        End If

    End Sub

    Private Sub LoadYear()
        'Load Years dropdown box

        Try
            ddlInventoryYear_Pollutant.Items.Add("-Select Year-")

            Dim query = "Select distinct intInventoryYear FROM VW_EIS_RPEmissions " &
                    "where FacilitySiteID = @FacilitySiteID order by intInventoryYear desc"

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dt As DataTable = DB.GetDataTable(query, param)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    ddlInventoryYear_Pollutant.Items.Add(dr("intInventoryYear").ToString)
                Next
            Else
                ddlInventoryYear_Pollutant.Items.Add("No Data")
            End If

            ddlInventoryYear_Pollutant.SelectedIndex = 0

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Private Sub loadReportgvw()
        Dim EIYear As Integer
        Dim dt As New DataTable

        If Integer.TryParse(ddlInventoryYear_Pollutant.Text, EIYear) Then

            Dim query = "SELECT " &
            "FacilitySiteID, " &
            "INTINVENTORYYEAR, " &
            "EMISSIONSUNITID, " &
            "STRUNITDESCRIPTION, " &
            "PROCESSID, " &
            "STRPROCESSDESCRIPTION, " &
            "STRPOLLUTANT, " &
            "Case " &
                "When vw_eis_rpemissions.RPTPeriodTypeCode = 'O3D' Then 'Summer Day' " &
                "Else 'Annual' " &
                "END RPTPeriodType, " &
            "Case " &
                "When vw_eis_rpemissions.RPTPeriodTypeCode = 'O3D' Then 'TPD *' " &
                "Else 'TPY' " &
                "END PollutantUnit, " &
            "FLTTOTALEMISSIONS, " &
            "FLTEMISSIONFACTOR, " &
            "STREMCALCMETHOD, " &
            "EFUNITS, " &
            "EFNUMDESC, " &
            "EFDENDESC, " &
            "STREMISSIONFACTORTEXT, " &
            "STREMISSIONSCOMMENT, " &
            "UPDATEUSER, " &
            "UPDATEDATETIME, " &
            "LASTEISSUBMITDATE " &
            "FROM VW_EIS_RPEmissions " &
            "where FacilitySiteID = @FacilitySiteID " &
            "and intInventoryYear = @EIYear " &
            "order by EMISSIONSUNITID, PROCESSID, STRPOLLUTANT"

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EIYear", EIYear)
            }

            dt = DB.GetDataTable(query, params)

        End If

        gvwPollutantDetails.DataSource = dt
        gvwPollutantDetails.DataBind()

    End Sub

    Protected Sub btnGO_Pollutant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGO_Pollutant.Click

        Dim gvwPollutantDetailsSize As Integer

        loadReportgvw()
        gvwPollutantDetailsSize = gvwPollutantDetails.Rows.Count

        If gvwPollutantDetailsSize <= 0 OrElse ddlInventoryYear_Pollutant.SelectedValue = "No Data" Then
            lblEmptygvwPollutantDetails.Text = "No data exists for the year selected or the facility has no data in the EI"
            lblEmptygvwPollutantDetails.Visible = True
            lblPollutantDetails.Text = ""
            lblPollutantDetails.Visible = False
            btnExport_Pollutant.Visible = False
        Else
            lblEmptygvwPollutantDetails.Text = ""
            lblEmptygvwPollutantDetails.Visible = False
            lblPollutantDetails.Text = "*Summer Day emissions = emissions on an average summer day May&nbsp;1 through Sep&nbsp;30. Units are in tons per day (TPD)"
            lblPollutantDetails.Visible = True
            btnExport_Pollutant.Visible = True
        End If

    End Sub

    Protected Sub btnReportsHome_Pollutant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReportsHome_Pollutant.Click

        Response.Redirect("reports.aspx")

    End Sub

    Protected Sub btnExport_Pollutant_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport_Pollutant.Click

        Dim EIYear As String = ddlInventoryYear_Pollutant.SelectedValue
        loadReportgvw()
        ExportAsExcel("EISPollutantDetails_" & EIYear, gvwPollutantDetails)

    End Sub

End Class