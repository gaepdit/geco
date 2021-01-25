Imports GECO.DAL
Imports GECO.GecoModels

Public Class Fees_Invoices
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
            LoadInvoices()
        End If
    End Sub

    Private Sub LoadFacilityInfo()
        lblFacilityDisplay.Text = GetFacilityName(currentAirs) & ", " & GetFacilityCity(currentAirs)
        Title = "Air Quality Permits - " & lblFacilityDisplay.Text
        lblAIRS.Text = currentAirs.FormattedString
    End Sub

    Private Sub LoadInvoices()
        Dim ds As DataSet = GetCombinedFeesInvoices(currentAirs)

        If ds IsNot Nothing Then
            If ds.Tables("AnnualFees").Rows.Count = 0 Then
                pAnnualInvoices.Visible = True
                grdAnnualInvoices.Visible = False
            Else
                grdAnnualInvoices.DataSource = ds.Tables("AnnualFees")
                grdAnnualInvoices.DataBind()
            End If

            If ds.Tables("ApplicationFees").Rows.Count = 0 Then
                pApplicationInvoices.Visible = True
                grdApplicationInvoices.Visible = False
            Else
                grdApplicationInvoices.DataSource = ds.Tables("ApplicationFees")
                grdApplicationInvoices.DataBind()
            End If
        End If
    End Sub
End Class
