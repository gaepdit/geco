Imports GECO.DAL.Facility
Imports GECO.GecoModels
Imports GECO.GecoModels.Facility

Public Class FacilityContacts
    Inherits Page

    Private Property currentAirs As ApbFacilityId
    Public Property FacilityAccess As FacilityAccess
    Public Property Reconfirm As Boolean
    Public Property CommunicationInfo As New Dictionary(Of CommunicationCategory, FacilityCommunicationInfo)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If IsPostBack Then
            currentAirs = New ApbFacilityId(GetCookie(Cookie.AirsNumber))
        Else
            ' AIRS number
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.TryParse(airsString, currentAirs) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        Master.CurrentAirs = currentAirs
        Master.IsFacilitySet = True

        MainLoginCheck(MyBase.Page.ResolveUrl("~/Facility/Contacts.aspx?airs=" & currentAirs.ShortString))

        FacilityAccess = GetCurrentUser().GetFacilityAccess(currentAirs)

        If FacilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Facility/")
        End If

        Title = "GECO Facility Contacts - " & GetFacilityNameAndCity(currentAirs)
        CommunicationInfo = GetFacilityCommunicationInfo(currentAirs)
    End Sub

End Class
