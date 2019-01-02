Imports GECO.GecoModels

Public Class PermitDefault
    Inherits Page

    Private Property currentAirs As ApbFacilityId

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
                HttpContext.Current.Response.Redirect("~/Facility/")
            End If

            currentAirs = New ApbFacilityId(airsString)
            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        LoadFacilityInfo()
        LoadPermitApplications()
        LoadPermits()
    End Sub

    Private Sub LoadFacilityInfo()
        lblFacilityDisplay.Text = GetFacilityName(currentAirs) & ", " & GetFacilityCity(currentAirs)
        Title = "Air Quality Permits - " & lblFacilityDisplay.Text
        lblAIRS.Text = currentAirs.FormattedString
    End Sub

    Private Sub LoadPermitApplications()
        pNoOpenApps.Visible = True
        pNoClosedApps.Visible = True
        grdOpenApps.Visible = False
        grdClosedApps.Visible = False

        Dim dt As DataTable = DAL.GetPermitApplications(currentAirs)

        If dt Is Nothing OrElse dt.Rows.Count = 0 Then
            Exit Sub
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
            Exit Sub
        End If

        grdCurrentPermits.DataSource = dt
        grdCurrentPermits.DataBind()

        pNoCurrentPermits.Visible = False
        pYesCurrentPermits.Visible = True
        hlPermitSearch.NavigateUrl = PermitApplication.GetPermitAirsSearchLink(currentAirs)
        grdCurrentPermits.Visible = True
    End Sub

End Class