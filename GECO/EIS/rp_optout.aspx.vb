Imports System.Data
Imports System.Data.SqlClient

Partial Class EIS_rp_optout
    Inherits Page

    Public conn, conn1 As New SqlConnection(oradb)
    Public eiYear As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim OptOut As String = GetCookie(EisCookie.OptOut)

        EIEntryAccessCheck(EISAccessCode, OptOut)

        eiYear = GetCookie(EisCookie.EISMaxYear)

        LoadGVWThresholds()
        lblEIYear1.Text = eiYear

        HideFacilityInventoryMenu()
        HideEmissionInventoryMenu()
        ShowEISHelpMenu()

    End Sub

    Private Sub LoadGVWThresholds()

        Dim eiType As String = GetEIType(eiYear)

        sqldsThresholds.ConnectionString = oradb
        sqldsThresholds.ProviderName = setProviderName()
        sqldsThresholds.SelectCommand = "select strPollutant, numThreshold, numThresholdNAA, strType FROM EIThresholds"
        sqldsThresholds.FilterExpression = String.Format("(strType = '" & eiType & "')")
        dgvThresholds.AllowPaging = False
        dgvThresholds.AllowSorting = False
        dgvThresholds.DataBind()

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim eiYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim opt As String = rbtnYesNo.SelectedValue
        Dim test As String = ""

        SaveOption(FacilitySiteID, opt, UpdateUser, eiYear)
        ResetCookies(FacilitySiteID)

        If opt = "0" Then
            Response.Redirect("rp_prepop.aspx")
        Else
            Response.Redirect("Default.aspx")
        End If

    End Sub

    Private Sub SaveOption(ByVal fsid As String, ByVal opt As String, ByVal uuser As String, ByVal eiyr As String)

        Dim eisAccessCode As String = ""
        Dim eisStatusCode As String = ""
        Dim sql1 As String = ""
        Dim sql2 As String = ""

        If opt = "1" Then
            eisAccessCode = "2"
            eisStatusCode = "3"
        Else
            eisAccessCode = "1"
            eisStatusCode = "2"
        End If

        Try
            sql1 = "select datFinalize FROM eis_Admin where " &
                    "FacilitySiteID = '" & fsid & "' and InventoryYear = '" & eiyr & "' and datInitialFinalize is not null"

            Dim cmd1 As New SqlCommand(sql1, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr1 As SqlDataReader = cmd1.ExecuteReader

            Dim recExist As Boolean = dr1.Read

            If opt = "1" And recExist = False Then
                'If datInitialFinalize is null
                sql2 = "update eis_Admin set " &
                            "strOptOut = '" & opt & "', " &
                            "eisAccessCode = '" & eisAccessCode & "', " &
                            "eisStatusCode = '" & eisStatusCode & "', " &
                            "datEISStatus = getdate(), " &
                            "strConfirmationNumber = Next Value for EIS_SEQ_ConfNum, " &
                            "datInitialFinalize = getdate(), " &
                            "datFinalize = getdate(), " &
                            "UpdateUser = '" & Replace(uuser, "'", "''") & "', " &
                            "UpdateDateTime = getdate() " &
                            "where " &
                            "FacilitySiteID = '" & fsid & "' and " &
                            "InventoryYear = " & eiYear
            ElseIf opt = "1" And recExist = True Then
                'If datInitialFinalize is not null
                sql2 = "update eis_Admin set " &
                            "strOptOut = '" & opt & "', " &
                            "eisAccessCode = '" & eisAccessCode & "', " &
                            "eisStatusCode = '" & eisStatusCode & "', " &
                            "strConfirmationNumber = Next Value for EIS_SEQ_ConfNum, " &
                            "datFinalize = getdate(), " &
                            "UpdateUser = '" & Replace(uuser, "'", "''") & "', " &
                            "UpdateDateTime = getdate() " &
                            "where " &
                            "FacilitySiteID = '" & fsid & "' and " &
                            "InventoryYear = " & eiYear
            Else
                sql2 = "update eis_Admin set " &
                            "strOptOut = '" & opt & "', " &
                            "eisAccessCode = '" & eisAccessCode & "', " &
                            "eisStatusCode = '" & eisStatusCode & "', " &
                            "UpdateUser = '" & Replace(uuser, "'", "''") & "', " &
                            "UpdateDateTime = getdate() " &
                            "where " &
                            "FacilitySiteID = '" & fsid & "' and " &
                            "InventoryYear = " & eiYear
            End If

            Dim cmd2 As New SqlCommand(sql2, conn)

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim dr2 As SqlDataReader = cmd2.ExecuteReader

        Catch ex As Exception
            ErrorReport(ex)
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

#Region "  Menu Routines  "

    Private Sub ShowFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = True
        End If

    End Sub

    Private Sub HideFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = False
        End If

    End Sub

    Private Sub ShowEmissionInventoryMenu()

        Dim menuEmissionInventory As Panel

        menuEmissionInventory = CType(Master.FindControl("pnlEmissionInventory"), Panel)

        If Not menuEmissionInventory Is Nothing Then
            menuEmissionInventory.Visible = True
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

    Private Sub HideEISHelpMenu()

        Dim menuEISHelp As Panel

        menuEISHelp = CType(Master.FindControl("pnlEISHelp"), Panel)

        If Not menuEISHelp Is Nothing Then
            menuEISHelp.Visible = False
        End If

    End Sub

#End Region

    Private Sub ResetCookies(ByVal fsid As String)

        Dim EISCookies As New HttpCookie("EISAccessInfo")
        Dim EISMaxYear As Integer
        Dim enrolled As String = ""
        Dim eisStatus As String = ""
        Dim accesscode As String = ""
        Dim optout As String = ""
        Dim dateFinalize As String = ""
        Dim confirmationnumber As String = ""
        Dim CurrentEIYear As Integer = Now.Year - 1
        Dim sql As String = ""

        Try
            sql = "Select eis_admin.FacilitySiteID, eis_admin.InventoryYear, " &
                    "EIS_Admin.EISStatusCode, EIS_Admin.datEISStatus, " &
                    "EIS_Admin.EISAccessCode, EIS_Admin.strOptOut, " &
                    "EIS_Admin.strEnrollment, EIS_Admin.datFinalize, " &
                    "EIS_Admin.strConfirmationNumber FROM EIS_Admin, " &
                    "(select max(inventoryYear) as MaxYear, " &
                    "EIS_Admin.FacilitySiteID " &
                    "FROM EIS_Admin GROUP BY EIS_Admin.FacilitySiteID ) MaxResults  " &
                    "where EIS_Admin.FacilitySiteID = '" & fsid & "' " &
                    "and EIS_Admin.inventoryYear = maxresults.maxyear " &
                    "and EIS_Admin.FacilitySiteID = maxresults.FacilitySiteID " &
                    "group by EIS_Admin.FacilitySiteID, " &
                    "EIS_Admin.inventoryYear, " &
                    "EIS_Admin.EISStatusCode, EIS_Admin.datEISStatus, " &
                    "EIS_Admin.EISAccessCode, EIS_Admin.strOptOut, " &
                    "EIS_Admin.strEnrollment, EIS_Admin.datFinalize, " &
                    "EIS_Admin.strConfirmationNumber"

            If conn.State = ConnectionState.Open Then
            Else
                conn.Open()
            End If

            Dim cmd As New SqlCommand(sql, conn)
            Dim dr = cmd.ExecuteReader
            Dim recExist As Boolean = dr.Read

            If recExist = False Then
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
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

End Class