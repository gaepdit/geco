Imports System.Data
Imports System.Data.SqlClient

Partial Class EIS_rp_reset
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim EIYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        EIAccessCheck(EISAccessCode, EISStatus)

        If Not IsPostBack Then
            lblMessage.Text = ""
            lblMessage.Visible = False
            btnReset.Visible = True
            btnConfReset.Visible = False
            btnContinue.Visible = False
            lblEIYear.Text = EIYear
        End If

    End Sub

    Protected Sub Reset_Click(sender As Object, e As EventArgs) Handles btnConfReset.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EIYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName

        ResetEI(FacilitySiteID, EIYear)
        ResetAdmin(FacilitySiteID, EIYear, UpdateUser)

        lblMessage.Text = "Emissions Inventory data has been reset. You will be returned to the EI home page after clicking the button below."
        lblMessage.Visible = True
        btnReset.Visible = False
        btnConfReset.Visible = False
        btnCancel.Visible = False
        btnContinue.Visible = True

    End Sub

    Private Sub ResetAdmin(fsid As String, eiyr As String, uuser As String)

        Dim eisAccessCode As String = "1"
        Dim eisStatusCode As String = "1"
        Dim Enrollment As String = GetCookie(EisCookie.Enrollment)
        Dim EISMaxYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim EISCookies As New HttpCookie("EISAccessInfo")

        Try
            Dim query = "update eis_Admin set " &
                    " strOptOut = null, " &
                    " eisAccessCode = @eisAccessCode, " &
                    " eisStatusCode = @eisStatusCode, " &
                    " datEISStatus = getdate(), " &
                    " strConfirmationNumber = null, " &
                    " datFinalize = null, " &
                    " intPrePopYear = null, " &
                    " IsColocated = null, " &
                    " ColocatedWith = null, " &
                    " UpdateUser = @uuser, " &
                    " UpdateDateTime = getdate() " &
                    " where " &
                    " FacilitySiteID = @fsid and " &
                    " InventoryYear = @eiyr "

            Dim params = {
                New SqlParameter("@eisAccessCode", eisAccessCode),
                New SqlParameter("@eisStatusCode", eisStatusCode),
                New SqlParameter("@uuser", uuser),
                New SqlParameter("@fsid", fsid),
                New SqlParameter("@eiyr", eiyr)
            }

            DB.RunCommand(query, params)

            'Reset cookies
            Dim optout = "NULL"
            Dim ConfNumber = "NULL"
            Dim DateFinalize = ""
            EISCookies.Values("OptOut") = EncryptDecrypt.EncryptText(optout)
            EISCookies.Values("EISAccess") = EncryptDecrypt.EncryptText(eisAccessCode)
            EISCookies.Values("EISStatus") = EncryptDecrypt.EncryptText(eisStatusCode)
            EISCookies.Values("ConfNumber") = EncryptDecrypt.EncryptText(ConfNumber)
            EISCookies.Values("Enrollment") = EncryptDecrypt.EncryptText(Enrollment)
            EISCookies.Values("DateFinalize") = EncryptDecrypt.EncryptText(DateFinalize)
            EISCookies.Values("EISMaxYear") = EncryptDecrypt.EncryptText(EISMaxYear)
            Response.Cookies("").Expires = DateTime.Now.AddHours(8)
            Response.Cookies.Add(EISCookies)

        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    Protected Sub btnReset_Click(sender As Object, e As EventArgs) Handles btnReset.Click

        btnReset.Visible = False
        btnConfReset.Visible = True
        btnCancel.Visible = True
        btnContinue.Visible = False
        lblMessage.Text = ""
        lblMessage.Visible = False

    End Sub

    Protected Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click

        Response.Redirect("Default.aspx")

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        btnReset.Visible = True
        btnConfReset.Visible = False
        btnCancel.Visible = False
        btnContinue.Visible = False
        lblMessage.Text = ""
        lblMessage.Visible = False

    End Sub

End Class