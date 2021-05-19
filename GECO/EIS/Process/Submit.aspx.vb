Imports GECO.GecoModels
Imports GECO.GecoModels.EIS
Imports GECO.DAL.EIS

Public Class EIS_Process_Submit
    Inherits Page

    Public Property CurrentAirs As ApbFacilityId
    Public Property Participating As Integer
    Public Property InventoryYear As Integer

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("EisProcessStarted") Is Nothing OrElse Session("EisProcess") Is Nothing Then
            Response.Redirect("~/EIS/")
        End If

        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.IsBeginEisProcess = True

        LoadCurrentData()
    End Sub

    Private Sub LoadCurrentData()
        Dim process As EisProcess = GetSessionItem(Of EisProcess)("EisProcess")

        If process Is Nothing Then
            Response.Redirect("~/EIS/")
        End If

        InventoryYear = process.InventoryYear

        Select Case process.Opted
            Case OptedInOut.DidNotOperate
                Participating = 1
            Case OptedInOut.BelowThresholds
                Participating = 2
            Case OptedInOut.OptedIn
                Participating = 3
        End Select
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim process As EisProcess = GetSessionItem(Of EisProcess)("EisProcess")
        SaveEisProcess(process)
        Session("EisProcess") = Nothing
        Response.Redirect("~/EIS/")
    End Sub
End Class
