Imports System.Data.SqlClient

Partial Class EIS_report_emissionunits
    Inherits Page

    Private FacilitySiteID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim gvwEmissionsUnitSize As Integer

        FacilitySiteID = GetCookie(Cookie.AirsNumber)

        If Not IsPostBack Then
            HideFacilityInventoryMenu()
            HideEmissionInventoryMenu()
            HideSubmitMenu()

            txtFacilityName_EmissionsUnit.Text = GetFacilityName(FacilitySiteID)
            txtFacilitySiteID_EmissionsUnit.Text = FacilitySiteID
            loadEmissionsSummarygvw()
            gvwEmissionsUnitSize = gvwEmissionsUnit.Rows.Count

            If gvwEmissionsUnitSize <= 0 Then
                lblEmptygvwEmissionsUnit.Text = "No emission units exist for this facility in the EIS"
                lblEmptygvwEmissionsUnit.Visible = True
                btnExport_EmissionsUnit.Visible = False
            Else
                lblEmptygvwEmissionsUnit.Text = ""
                lblEmptygvwEmissionsUnit.Visible = False
                btnExport_EmissionsUnit.Visible = True
            End If

        End If

    End Sub

    Private Sub loadEmissionsSummarygvw()

        Dim query = "select " &
            "EmissionsUnitID, strUnitDescription, " &
            "(select strDesc FROM eislk_UnittypeCode where " &
            "eis_EmissionsUnit.strUnitTypeCode = eislk_UnitTypeCode.UnitTypeCode and eislk_UnitTypeCode.Active = '1') as strUnitType, " &
            "fltUnitDesignCapacity, " &
            "STRUNITDESIGNCAPACITYUOMCODE, " &
            "NUMMAXIMUMNAMEPLATECAPACITY, " &
            "(select strDesc FROM eislk_UnitStatusCode where " &
            "eis_EmissionsUnit.strUnitStatusCode = eislk_UnitStatusCode.UnitStatusCode and eislk_UnitStatusCode.Active = '1') as strUnitStatusCode, " &
            "DATUNITOPERATIONDATE, " &
            "STRUNITCOMMENT, " &
            "LASTEISSUBMITDATE " &
            "from " &
            "eis_EmissionsUnit " &
            "where " &
            "FacilitySiteID = @FacilitySiteID and " &
            "eis_EmissionsUnit.Active = '1'"

        Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

        gvwEmissionsUnit.DataSource = DB.GetDataTable(query, param)

        gvwEmissionsUnit.DataBind()

    End Sub

    Protected Sub btnExport_EmissionsUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExport_EmissionsUnit.Click

        loadEmissionsSummarygvw()
        ExportAsExcel("EISEmissionUnits", gvwEmissionsUnit)

    End Sub

    Protected Sub btnReportsHome_EmissionsUnit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReportsHome_EmissionsUnit.Click

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