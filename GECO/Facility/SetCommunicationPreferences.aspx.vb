Imports GECO.GecoModels
Imports GECO.GecoModels.Facility
Imports GECO.DAL.Facility

Public Class SetCommunicationPreferences
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        If Not IsPostBack Then
            Dim currentAirs As ApbFacilityId = ApbFacilityId.IfValid(GetCookie(Cookie.AirsNumber))

            If currentAirs Is Nothing Then
                CompleteRedirect("~/Home/")
                Return
            End If

            Master.CurrentAirs = currentAirs
            Master.IsFacilitySet = True

            Dim facilityAccess As FacilityAccess = GetCurrentUser.GetFacilityAccess(currentAirs)

            If facilityAccess Is Nothing OrElse
              Not facilityAccess.HasCommunicationPermission(CommunicationCategory.PermitFees) Then
                CompleteRedirect("~/Home/")
                Return
            End If

            ' Check current pref setting. If already set, then redirect to facility home.
            Dim pref As FacilityCommunicationPreference = GetFacilityCommunicationPreference(currentAirs, CommunicationCategory.PermitFees)

            If pref.IsConfirmed Then
                CompleteRedirect("~/Home/")
                Return
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
            CompleteRedirect("~/Home/")
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

        CompleteRedirect("~/Facility/Contacts.aspx")
    End Sub

End Class
