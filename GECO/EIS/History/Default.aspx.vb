Imports Microsoft.Data.SqlClient
Imports GECO.GecoModels

Public Class EIS_History_Default
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            CompleteRedirect("~/")
            Return
        End If

        Dim airsNum As New ApbFacilityId(airs)
        Master.CurrentAirs = airsNum
        Master.SelectedTab = EIS.EisTab.History

        If Not HistoricalDataExists(airsNum) Then
            dNoDataExists.Visible = True
            dDataExists.Visible = False
        End If
    End Sub

    Private Shared Function HistoricalDataExists(airs As ApbFacilityId) As Boolean
        Dim query As String = "select convert(bit, count(*))
            from EIS_REPORTINGPERIODEMISSIONS
            where FACILITYSITEID = @FACILITYSITEID"

        Dim param As New SqlParameter("@FACILITYSITEID", airs.ShortString)

        Return DB.GetBoolean(query, param)
    End Function

End Class
