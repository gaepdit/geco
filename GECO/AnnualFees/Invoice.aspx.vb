Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Partial Class AnnualFees_Invoice
    Inherits Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ConfigureCrystalReports()
    End Sub

    Private Sub ConfigureCrystalReports()
        Dim CrReportDocument As New ReportDocument()

        Dim crConnectionInfo As ConnectionInfo

        Dim reportPath As String

        Dim p As New ParameterFields
        Dim p1 As New ParameterField
        Dim p2 As New ParameterField
        Dim p3 As New ParameterDiscreteValue
        Dim p4 As New ParameterDiscreteValue

        Dim year As String = Request.QueryString("FeeYear")

        Dim yearInt As Integer

        If String.IsNullOrEmpty(year) OrElse Not Integer.TryParse(year, yearInt) Then
            Throw New HttpException(404, "Invoice fee year is not valid.")
        End If

        If yearInt < 1992 Or yearInt >= Today.Year Then
            Throw New HttpException(404, "Invoice fee year is not valid.")
        End If

        Try
            reportPath = Server.MapPath("~/AnnualFees/FeeInvoice.rpt")
            p1.ParameterFieldName = "AirsNumber"
            p3.Value = "0413" & GetCookie(Cookie.AirsNumber)
            p1.CurrentValues.Add(p3)
            p.Add(p1)
            myCrystalReportViewer.ParameterFieldInfo = p

            p2.ParameterFieldName = "Year"
            p4.Value = yearInt
            p2.CurrentValues.Add(p4)
            p.Add(p2)
            myCrystalReportViewer.ParameterFieldInfo = p

            'Create an instance of the strongly-typed report object
            CrReportDocument.Load(reportPath)

            'Create the Conection Info object to hold the logon information for the report
            crConnectionInfo = New ConnectionInfo()
            Dim database_service As String = ConfigurationManager.AppSettings("database_service")
            Dim database_uid As String = ConfigurationManager.AppSettings("database_uid")
            Dim database_pwd As String = ConfigurationManager.AppSettings("database_pwd")

            ' setup the connection
            With crConnectionInfo
                .ServerName = database_service ' ODBC DSN in quotes, not Oracle server or database name
                .DatabaseName = "airbranch" ' leave empty string here
                .UserID = database_uid ' database user ID in quotes
                .Password = database_pwd  'database password in quotes
            End With

            ' apply logon information

            For Each tbl As CrystalDecisions.CrystalReports.Engine.Table In CrReportDocument.Database.Tables
                Dim repTblLogonInfo As TableLogOnInfo = tbl.LogOnInfo
                repTblLogonInfo.ConnectionInfo = crConnectionInfo
                tbl.ApplyLogOnInfo(repTblLogonInfo)
            Next

            'Set the ReportSource of the CrystalReportViewer to the strongly typed Report included in the project
            myCrystalReportViewer.ReportSource = CrReportDocument
            myCrystalReportViewer.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
        Catch ex As Exception
            ErrorReport(ex)
        Finally
            CrReportDocument.Dispose()
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sm As ScriptManager = ScriptManager.GetCurrent(Me) '= CType(Master.FindControl("ScriptManager1"), ScriptManager)
        sm.RegisterPostBackControl(Me.myCrystalReportViewer)

        'sm.RegisterAsyncPostBackControl(myCrystalReportViewer)
        If Not IsPostBack Then
            Dim easymenu = CType(Master.FindControl("EasyMenu1"), Sequentum.EasyMenu)
            easymenu.MenuRoot.AddSubMenuItem("Back to Fees Form", "Default.aspx")
        End If

    End Sub

End Class