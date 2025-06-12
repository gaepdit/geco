Imports GECO.GecoModels
Imports GECO.GecoModels.Facility
Imports GECO.DAL.Facility

Public Class SetCommunicationPreferences
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim currentAirs As ApbFacilityId = ApbFacilityId.IfValid(GetCookie(Cookie.AirsNumber))

            If currentAirs Is Nothing Then
                HttpContext.Current.Response.Redirect("~/Home/")
                Return
            End If

            Master.CurrentAirs = currentAirs
            Master.IsFacilitySet = True
            MainLoginCheck(Page.ResolveUrl("~/Facility/?airs=" & currentAirs.ShortString))

            Dim facilityAccess As FacilityAccess = GetCurrentUser.GetFacilityAccess(currentAirs)

            If facilityAccess Is Nothing OrElse
              Not facilityAccess.HasCommunicationPermission(CommunicationCategory.PermitFees) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            ' Check current pref setting. If already set, then redirect to facility home.
            Dim pref As FacilityCommunicationPreference = GetFacilityCommunicationPreference(currentAirs, CommunicationCategory.PermitFees)

            If pref.IsConfirmed Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            ' AIRS number cookie gets cleared so user can't manually navigate to other 
            ' facility pages without submitting the form. Upon submittal, the cookie
            ' gets set again from the hidden form value.
            hidAirs.Value = currentAirs
            ClearCookie(Cookie.AirsNumber)
        End If
    End Sub

    Private Sub btnSavePref_Click(sender As Object, e As EventArgs) Handles btnSavePref.Click
        If rbCommPref.SelectedIndex = -1 OrElse
          Not {"Electronic", "Mail", "Both"}.Contains(rbCommPref.SelectedValue) Then
            pNotSelected.Visible = True
            Return
        End If

        Dim airs As ApbFacilityId = ApbFacilityId.IfValid(hidAirs.Value)

        If airs Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
            Return
        End If

        Dim result As Boolean = SaveCommunicationPreference(airs,
                                    CommunicationCategory.PermitFees,
                                    CommunicationPreference.FromName(rbCommPref.SelectedValue),
                                    GetCurrentUser.UserId)

        If Not result Then
            pPrefSaveError.Visible = True
            Return
        End If

        SetCookie(Cookie.AirsNumber, airs.ShortString)
        HttpContext.Current.Response.Redirect($"~/Facility/Contacts.aspx")
    End Sub

End Class
