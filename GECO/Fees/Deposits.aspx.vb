Imports GECO.DAL
Imports GECO.GecoModels

Public Class Fees_Deposits
    Inherits Page

    Private Property currentAirs As ApbFacilityId
    Private Property facilityAccess As FacilityAccess
    Private Property currentUser As GecoUser

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

        Master.CurrentAirs = currentAirs
        Master.IsFacilitySet = True

        MainLoginCheck(Page.ResolveUrl("~/Fees/?airs=" & currentAirs.ShortString))

        ' Current user
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
        End If

        If Not facilityAccess.FeeAccess Then
            HttpContext.Current.Response.Redirect("~/Facility/")
        End If

        If Not IsPostBack Then
            LoadFacilityInfo()
            LoadDeposits()
        End If
    End Sub

    Private Sub LoadFacilityInfo()
        lblFacilityDisplay.Text = GetFacilityName(currentAirs) & ", " & GetFacilityCity(currentAirs)
        Title = "Air Quality Permits - " & lblFacilityDisplay.Text
        lblAIRS.Text = currentAirs.FormattedString
    End Sub

    Private Sub LoadDeposits()
        Dim ds As DataSet = GetCombinedFeesDeposits(currentAirs)

        If ds IsNot Nothing Then
            If ds.Tables("AnnualFeesTransactions").Rows.Count = 0 Then
                pAnnualTransactions.Visible = True
                grdAnnualTransactions.Visible = False
            Else
                grdAnnualTransactions.DataSource = ds.Tables("AnnualFeesTransactions")
                grdAnnualTransactions.DataBind()
            End If
            If ds.Tables("ApplicationFeesDeposits").Rows.Count = 0 Then
                pApplicationDeposits.Visible = True
                grdApplicationDeposits.Visible = False
            Else
                grdApplicationDeposits.DataSource = ds.Tables("ApplicationFeesDeposits")
                grdApplicationDeposits.DataBind()
            End If
            If ds.Tables("ApplicationFeesRefunds").Rows.Count = 0 Then
                pApplicationRefunds.Visible = True
                grdApplicationRefunds.Visible = False
            Else
                grdApplicationRefunds.DataSource = ds.Tables("ApplicationFeesRefunds")
                grdApplicationRefunds.DataBind()
            End If
        End If
    End Sub
End Class
