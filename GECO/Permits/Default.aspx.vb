Imports GECO.GecoModels

Public Class PermitDefault
    Inherits Page

    Private Property currentAirs As ApbFacilityId

    Private IsTerminating As Boolean = False
    Protected Overrides Sub Render(writer As HtmlTextWriter)
        If IsTerminating Then Return
        MyBase.Render(writer)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            currentAirs = New ApbFacilityId(GetCookie(Cookie.AirsNumber))
        Else
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.IsValidAirsNumberFormat(airsString) Then
                CompleteRedirect("~/Facility/", IsTerminating)
                Return
            End If

            currentAirs = New ApbFacilityId(airsString)
            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        Master.CurrentAirs = currentAirs
        Master.IsFacilitySet = True

        If Not AirsNumberExists(currentAirs) Then
            Throw New HttpException(404, "Not found")
        End If

        LoadFacilityInfo()
        LoadPermitApplications()
        LoadPermits()

        AddBreadcrumb("Permits/Application", "AIRS #", currentAirs.FormattedString, ID)
    End Sub

    Private Sub LoadFacilityInfo()
        Title = "Air Quality Permits - " & GetFacilityNameAndCity(currentAirs)
    End Sub

    Private Sub LoadPermitApplications()
        pNoOpenApps.Visible = True
        pNoClosedApps.Visible = True
        grdOpenApps.Visible = False
        grdClosedApps.Visible = False

        Dim dt As DataTable = DAL.GetPermitApplications(currentAirs)

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Return
        End If

        Dim openApps As DataView = New DataView(dt) With {
            .RowFilter = "Status<>'Closed Out'"
        }

        If openApps.Count > 0 Then
            grdOpenApps.DataSource = openApps
            grdOpenApps.DataBind()

            pNoOpenApps.Visible = False
            grdOpenApps.Visible = True
        End If

        Dim closedApps As DataView = New DataView(dt) With {
            .RowFilter = "Status='Closed Out'"
        }

        If closedApps.Count > 0 Then
            grdClosedApps.DataSource = closedApps
            grdClosedApps.DataBind()

            pNoClosedApps.Visible = False
            grdClosedApps.Visible = True
        End If
    End Sub

    Private Sub LoadPermits()
        pNoCurrentPermits.Visible = True
        pYesCurrentPermits.Visible = False
        grdCurrentPermits.Visible = False

        Dim dt As DataTable = DAL.GetPermits(currentAirs)

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Return
        End If

        grdCurrentPermits.DataSource = dt
        grdCurrentPermits.DataBind()

        pNoCurrentPermits.Visible = False
        pYesCurrentPermits.Visible = True
        hlPermitSearch.NavigateUrl = PermitApplication.GetPermitAirsSearchLink(currentAirs)
        grdCurrentPermits.Visible = True
    End Sub

End Class
