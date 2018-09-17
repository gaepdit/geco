Imports CrystalDecisions.Shared
Imports CrystalDecisions.CrystalReports.Engine

Partial Class Fees_FeesReport
    Inherits Page
    Dim CrReportDocument As New ReportDocument()

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ConfigureCrystalReports()
    End Sub

    Private Sub ConfigureCrystalReports()
        Dim crConnectionInfo As ConnectionInfo

        Dim reportPath As String

        Dim p As New ParameterFields
        Dim p1 As New ParameterField
        Dim p2 As New ParameterField
        Dim p3 As New ParameterDiscreteValue
        Dim p4 As New ParameterDiscreteValue

        Dim report As String = Request.QueryString("Report")
        Dim year As String = Request.QueryString("FeeYear")
        Try
            If report = "Invoice" Then
                reportPath = Server.MapPath("~/Fees/FeeInvoice.rpt")
                p1.ParameterFieldName = "AirsNumber"
                p3.Value = "0413" & GetCookie(Cookie.AirsNumber)
                p1.CurrentValues.Add(p3)
                p.Add(p1)
                myCrystalReportViewer.ParameterFieldInfo = p

                p2.ParameterFieldName = "Year"
                p4.Value = CInt(year)
                p2.CurrentValues.Add(p4)
                p.Add(p2)
                myCrystalReportViewer.ParameterFieldInfo = p
            Else
                reportPath = Server.MapPath("~/Fees/FeeReport.rpt")
                p1.ParameterFieldName = "AirsNumber"
                p3.Value = "0413" & GetCookie(Cookie.AirsNumber)
                p1.CurrentValues.Add(p3)
                p.Add(p1)
                myCrystalReportViewer.ParameterFieldInfo = p
            End If
            'Create an instance of the strongly-typed report object
            CrReportDocument.Load(reportPath)

            'Create the Conection Info object to hold the logon information for the report
            crConnectionInfo = New ConnectionInfo()
            Dim database_service As String = System.Configuration.ConfigurationManager.AppSettings("database_service")
            Dim database_uid As String = System.Configuration.ConfigurationManager.AppSettings("database_uid")
            Dim database_pwd As String = System.Configuration.ConfigurationManager.AppSettings("database_pwd")

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
        End Try

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sm As ScriptManager = ScriptManager.GetCurrent(Me) '= CType(Master.FindControl("ScriptManager1"), ScriptManager)
        sm.RegisterPostBackControl(Me.myCrystalReportViewer)

        'sm.RegisterAsyncPostBackControl(myCrystalReportViewer)
        If Not IsPostBack Then
            Dim easymenu = CType(Master.FindControl("EasyMenu1"), Sequentum.EasyMenu)
            easymenu.MenuRoot.AddSubMenuItem("Back to Emission Fees", "Default.aspx")
        End If

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        CrReportDocument.Close()
        CrReportDocument.Dispose()
    End Sub
End Class