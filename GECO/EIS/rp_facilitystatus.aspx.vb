Imports System.Data.SqlClient

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

            HideFacilityInventoryMenu()
            HideEmissionInventoryMenu()
            ShowEISHelpMenu()
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
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim eiYear As Integer = CInt(GetCookie(EisCookie.EISMaxYear))

        'Saving FacilityStatus as OP even if not during
        SaveFacilityStatus(FacilitySiteID, "OP", UpdateUser, eiYear)

        If Len(txtComment.Text) > 0 Then
            SaveAdminComment(FacilitySiteID, eiYear, txtComment.Text)
        End If

        If rblOperate.SelectedValue = "No" Then
            Dim colocated As Boolean = (rblIsColocated.SelectedValue = "Yes")
            Dim colocation As String = Nothing

            If rblIsColocated.SelectedValue = "Yes" Then
                colocation = txtColocatedWith.Text
            End If

            SaveOption(FacilitySiteID, "1", UpdateUser, eiYear, "1", colocated, colocation)
            ResetCookies(FacilitySiteID)
            Response.Redirect("Default.aspx")
        Else
            Response.Redirect("rp_facilitythreshold.aspx")
        End If

    End Sub

    Private Sub SaveFacilityStatus(fsid As String, facstatus As String, UpdUser As String, eiyr As Integer)
        Try
            Dim query = "update eis_FacilitySite " &
                " set strFacilitySiteStatusCode = @facstatus, " &
                " intFacilitySiteStatusCodeYear = @eiyr, " &
                " UpdateUser = @UpdUser, " &
                " UpdateDateTime = getdate() " &
                " where " &
                " FacilitySiteID = @fsid "

            Dim params = {
                New SqlParameter("@facstatus", facstatus),
                New SqlParameter("@eiyr", eiyr),
                New SqlParameter("@UpdUser", UpdUser),
                New SqlParameter("@fsid", fsid)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub ResetCookies(fsid As String)

        Dim EISCookies As New HttpCookie("EISAccessInfo")
        Dim EISMaxYear As Integer
        Dim enrolled As String = ""
        Dim eisStatus As String = ""
        Dim accesscode As String = ""
        Dim optout As String = ""
        Dim dateFinalize As String = ""
        Dim confirmationnumber As String = ""
        Dim CurrentEIYear As Integer = Now.Year - 1

        Try
            Dim query = "Select eis_admin.FacilitySiteID, eis_admin.InventoryYear, " &
                "EIS_Admin.EISStatusCode, EIS_Admin.datEISStatus, " &
                "EIS_Admin.EISAccessCode, EIS_Admin.strOptOut, " &
                "EIS_Admin.strEnrollment, EIS_Admin.datFinalize, " &
                "EIS_Admin.strConfirmationNumber FROM EIS_Admin, " &
                "(select max(inventoryYear) as MaxYear, " &
                "EIS_Admin.FacilitySiteID " &
                "FROM EIS_Admin GROUP BY EIS_Admin.FacilitySiteID ) MaxResults  " &
                "where EIS_Admin.FacilitySiteID = @fsid " &
                "and EIS_Admin.inventoryYear = maxresults.maxyear " &
                "and EIS_Admin.FacilitySiteID = maxresults.FacilitySiteID " &
                "group by EIS_Admin.FacilitySiteID, " &
                "EIS_Admin.inventoryYear, " &
                "EIS_Admin.EISStatusCode, EIS_Admin.datEISStatus, " &
                "EIS_Admin.EISAccessCode, EIS_Admin.strOptOut, " &
                "EIS_Admin.strEnrollment, EIS_Admin.datFinalize, " &
                "EIS_Admin.strConfirmationNumber"

            Dim param As New SqlParameter("@fsid", fsid)

            Dim dr = DB.GetDataRow(query, param)

            If dr Is Nothing Then
                'Set EISAccess cookie to "3" id facility does not exist in EIS Admin table
                EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText("3")
            Else
                'get max year from EIS Admin table
                If IsDBNull(dr("InventoryYear")) Then
                    'Do nothing - leave EISMaxYear null
                Else
                    EISMaxYear = dr.Item("InventoryYear")
                End If
                EISCookies.Values("EISMaxYear") = EncryptDecrypt.EncryptText(EISMaxYear)

                If EISMaxYear = CurrentEIYear Then
                    'Check enrollment
                    'get enrollment status: 0 = not enrolled; 1 = enrolled for EI year
                    If IsDBNull(dr("strEnrollment")) Then
                        enrolled = "NULL"
                    Else
                        enrolled = dr.Item("strEnrollment")
                    End If
                    EISCookies.Values("Enrollment") = EncryptDecrypt.EncryptText(enrolled)

                    If enrolled = "1" Then
                        'getEISStatus for EISMaxYear
                        If IsDBNull(dr("EISStatusCode")) Then
                            eisStatus = "NULL"
                        Else
                            eisStatus = dr.Item("EISStatusCode")
                        End If
                        EISCookies.Values("EISStatus") = EncryptDecrypt.EncryptText(eisStatus)

                        'get EIS Access Code from database
                        If IsDBNull(dr("EISAccessCode")) Then
                            accesscode = "NULL"
                        Else
                            accesscode = dr.Item("EISAccessCode")
                        End If
                        EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText(accesscode)

                        If IsDBNull(dr("strOptOut")) Then
                            optout = "NULL"
                        Else
                            optout = dr.Item("strOptOut")
                        End If
                        EISCookies.Values("OptOut") = EncryptDecrypt.EncryptText(optout)

                        If IsDBNull(dr("datFinalize")) Then
                            dateFinalize = "NULL"
                        Else
                            dateFinalize = dr.Item("datFinalize")
                        End If
                        EISCookies.Values("DateFinalize") = EncryptDecrypt.EncryptText(dateFinalize)

                        If IsDBNull(dr("strConfirmationNumber")) Then
                            confirmationnumber = "NULL"
                        Else
                            confirmationnumber = dr.Item("strConfirmationNumber")
                        End If
                        EISCookies.Values("ConfNumber") = EncryptDecrypt.EncryptText(confirmationnumber)
                    End If
                Else
                    EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText("0")
                End If
            End If

            EISCookies.Expires = DateTime.Now.AddHours(8)
            Response.Cookies.Add(EISCookies)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#Region "  Menu Routines  "

    Private Sub HideFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = False
        End If

    End Sub

    Private Sub HideEmissionInventoryMenu()

        Dim menuEmissionInventory As Panel

        menuEmissionInventory = CType(Master.FindControl("pnlEmissionInventory"), Panel)

        If Not menuEmissionInventory Is Nothing Then
            menuEmissionInventory.Visible = False
        End If

    End Sub

    Private Sub ShowEISHelpMenu()

        Dim menuEISHelp As Panel

        menuEISHelp = CType(Master.FindControl("pnlEISHelp"), Panel)

        If Not menuEISHelp Is Nothing Then
            menuEISHelp.Visible = True
        End If

    End Sub

#End Region

End Class