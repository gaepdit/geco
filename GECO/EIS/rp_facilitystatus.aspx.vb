Imports GECO.GecoModels

Partial Class EIS_rp_facilitystatus
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim eiYear As String = GetCookie(EisCookie.EISMaxYear)

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            txtComment.Text = GetAdminComment(FacilitySiteID, eiYear)
            lblEIYear1.Text = eiYear
            lblEIYear2.Text = eiYear
        End If

    End Sub

    Protected Sub rblOperate_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblOperate.SelectedIndexChanged

        pnlShutdownStatus.Visible = (rblOperate.SelectedValue = "No")

    End Sub

    Private Sub rblIsColocated_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblIsColocated.SelectedIndexChanged

        pnlColocation.Visible = (rblIsColocated.SelectedValue = "Yes")

    End Sub

    Protected Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim airs As New ApbFacilityId(FacilitySiteID)
        Dim eiYear As Integer = CInt(GetCookie(EisCookie.EISMaxYear))
        Dim currentUser = GetCurrentUser()

        'Saving FacilityStatus as OP even if not during
        SaveEisFacilityStatus(airs, FacilitySiteStatusCode.OP, currentUser.DbUpdateUser, eiYear)

        If Len(txtComment.Text) > 0 Then
            SaveAdminComment(airs, eiYear, txtComment.Text)
        End If

        If rblOperate.SelectedValue = "No" Then
            Dim colocated As Boolean = (rblIsColocated.SelectedValue = "Yes")
            Dim colocation As String = Nothing

            If rblIsColocated.SelectedValue = "Yes" Then
                colocation = txtColocatedWith.Text
            End If

            SaveEisOptOut(airs, True, currentUser.DbUpdateUser, eiYear, "1", colocated, colocation)
            LoadEiStatusCookies(airs, Response)
            Response.Redirect("Default.aspx")
        Else
            Response.Redirect("rp_facilitythreshold.aspx")
        End If

    End Sub

End Class