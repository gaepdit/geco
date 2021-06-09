﻿Imports GECO.GecoModels

Public Class FacilityContacts
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property facilityAccess As FacilityAccess
    Private Property currentAirs As ApbFacilityId
    Public Property Reconfirm As Boolean

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' TODO: REMOVE ==
        Reconfirm = True
        Return
        ' ==

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

        MainLoginCheck(Page.ResolveUrl("~/Facility/Contacts.aspx?airs=" & currentAirs.ShortString))

        ' Current user and facility access
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Facility/")
        End If

        If Not IsPostBack Then
            Title = "GECO Facility Contacts - " & GetFacilityNameAndCity(currentAirs)
            Reconfirm = IsItTimeToReconfirm()
        End If
    End Sub

    Private Function IsItTimeToReconfirm() As Boolean
        Return True
    End Function
End Class
