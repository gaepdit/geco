Imports GECO.DAL.Facility
Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Public Class FacilityContacts
    Inherits Page

    Private Property currentAirs As ApbFacilityId
    Public Property FacilityAccess As FacilityAccess
    Public Property CommunicationInfo As New Dictionary(Of CommunicationCategory, FacilityCommunicationInfo)
    Public Property CommunicationUpdate As CommunicationUpdateResponse

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
            If Not ApbFacilityId.TryParse(GetCookie(Cookie.AirsNumber), currentAirs) Then
                CompleteRedirect("~/Home/", IsTerminating)
                Return
            End If
        Else
            ' AIRS number
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.TryParse(airsString, currentAirs) Then
                CompleteRedirect("~/Home/", IsTerminating)
                Return
            End If

            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        Master.CurrentAirs = currentAirs
        Master.IsFacilitySet = True

        FacilityAccess = GetCurrentUser().GetFacilityAccess(currentAirs)

        If FacilityAccess Is Nothing Then
            CompleteRedirect("~/Facility/", IsTerminating)
            Return
        End If

        Title = "GECO Facility Contacts - " & GetFacilityNameAndCity(currentAirs)
        CommunicationInfo = GetFacilityCommunicationInfo(currentAirs)

        If Not IsPostBack Then
            CommunicationUpdate = GetCommunicationUpdate(currentAirs, FacilityAccess)
        End If
    End Sub

    Private Sub btnLooksGood_Click(sender As Object, e As EventArgs) Handles btnLooksGood.Click
        ConfirmCommunicationSettings(currentAirs, GetCurrentUser.UserId, FacilityAccess)
        CompleteRedirect("~/Facility/", IsTerminating)
        Return
    End Sub

End Class
