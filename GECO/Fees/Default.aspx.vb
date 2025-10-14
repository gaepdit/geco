Imports GECO.DAL
Imports GECO.GecoModels

Public Class Fees_Default
    Inherits Page

    Private Property currentAirs As ApbFacilityId
    Private Property facilityAccess As FacilityAccess
    Private Property currentUser As GecoUser

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

        ' Current user
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            CompleteRedirect("~/Home/", IsTerminating)
            Return
        End If

        If Not facilityAccess.FeeAccess Then
            CompleteRedirect("~/Facility/", IsTerminating)
            Return
        End If

        If Not IsPostBack Then
            LoadFacilityInfo()
            LoadFeesSummary()
        End If

        AddBreadcrumb("Fees", "AIRS #", currentAirs.FormattedString, Me)
    End Sub

    Private Sub LoadFacilityInfo()
        Title = "Air Quality Permits - " & GetFacilityNameAndCity(currentAirs)
    End Sub

    Private Sub LoadFeesSummary()
        Dim ds As DataSet = GetCombinedFeesSummary(currentAirs)

        If ds IsNot Nothing Then
            If ds.Tables("AnnualFees").Rows.Count = 0 Then
                pAnnualFees.Visible = True
                grdAnnualFees.Visible = False
            Else
                grdAnnualFees.DataSource = ds.Tables("AnnualFees")
                grdAnnualFees.DataBind()
            End If

            If ds.Tables("ApplicationFees").Rows.Count = 0 Then
                pApplicationDeposits.Visible = True
                grdApplicationDeposits.Visible = False
            Else
                grdApplicationDeposits.DataSource = ds.Tables("ApplicationFees")
                grdApplicationDeposits.DataBind()
            End If
        End If
    End Sub
End Class
