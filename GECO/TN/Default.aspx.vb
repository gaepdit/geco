Imports Microsoft.Data.SqlClient
Imports System.DateTime
Imports GaEpd.DBUtilities
Imports GECO.GecoModels

Partial Class TN_Default
    Inherits Page

    Private Property currentAirs As ApbFacilityId

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
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

            If Not ApbFacilityId.IsValidAirsNumberFormat(airsString) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            currentAirs = New ApbFacilityId(airsString)
            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        Master.CurrentAirs = currentAirs
        Master.IsFacilitySet = True

        If Not IsPostBack Then
            loaddgrTestNotify()
        End If
    End Sub

#Region " Miscellaneous Routines "

    Private Sub getEPDContact(epdid As String)
        Dim EPDFaxNumber As String
        Dim EPDTelephone As String

        Try
            Dim query = "select * from EPDUSERPROFILES where NUMUSERID = @epdid"
            Dim param As New SqlParameter("@epdid", epdid)

            Dim dr = DB.GetDataRow(query, param)

            If dr Is Nothing Then
                lblEPDContact.Text = "** Not Assigned **"
                lblEPDContact.ForeColor = Drawing.Color.Red
                lblEPDTelephone.Text = "404-363-7000 (ask for Source Monitoring Unit)"
                lblEPDFax.Text = "404-363-7100"
            Else
                lblEPDContact.Text = GetNullableString(dr.Item("STRFIRSTNAME")) & " " & GetNullableString(dr.Item("STRLASTNAME"))

                If Convert.IsDBNull(dr.Item("STRPHONE")) Then
                    lblEPDTelephone.Text = "404-363-7000 (ask For Source Monitoring Unit)"
                Else
                    EPDTelephone = CStr(dr.Item("STRPHONE"))
                    lblEPDTelephone.Text = EPDTelephone.Substring(0, 3) & "-" & EPDTelephone.Substring(3, 3) & "-" & EPDTelephone.Substring(6)
                End If

                If Convert.IsDBNull(dr.Item("STRFAX")) Then
                    lblEPDFax.Text = "404-363-7100"
                Else
                    EPDFaxNumber = CStr(dr.Item("STRFAX"))
                    lblEPDFax.Text = EPDFaxNumber.Substring(0, 3) & "-" & EPDFaxNumber.Substring(3, 3) & "-" & EPDFaxNumber.Substring(6)
                End If

                lblEPDEmail.Text = GetNullableString(dr.Item("STREMAILADDRESS"))
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

#Region " Datagrid Routine "

    Private Sub loaddgrTestNotify()

        Dim query As String = "select STRTESTLOGNUMBER, " &
            "STREMISSIONUNIT, " &
            "DATPROPOSEDSTARTDATE, DATPROPOSEDENDDATE, " &
            "STRCOMMENTS, NUMUSERID, STRCONTACTEMAIL, STRCONFIRMATIONNUMBER, STRAIRSNUMBER, " &
            "STRSTAFFRESPONSIBLE, STRONLINEFIRSTNAME, STRONLINELASTNAME, STRTELEPHONE, " &
            "STRFAX, DATTESTNOTIFICATION, " &
            "STRPOLLUTANTS " &
            "FROM ISMPTestNotification where strAirsNumber = @AirsNumber " &
            "order by datProposedStartDate Desc"

        Dim dt As DataTable = DB.GetDataTable(query, New SqlParameter("@AirsNumber", currentAirs.DbFormattedString))

        Dim dv As New DataView(dt)

        SessionAdd(GecoSession.TestNotifications, dv)
        dgrTestNotify.DataSource = dv
        dgrTestNotify.DataBind()
    End Sub

    Protected Sub dgrTestNotify_PageIndexChanged(s As Object, e As DataGridPageChangedEventArgs)
        NotNull(e, NameOf(e))

        dgrTestNotify.DataSource = GetSessionItem(Of DataView)(GecoSession.TestNotifications)
        dgrTestNotify.CurrentPageIndex = e.NewPageIndex
        dgrTestNotify.DataBind()

    End Sub

    Protected Sub RequestDetails(s As Object, e As DataGridCommandEventArgs)
        NotNull(e, NameOf(e))

        If e.CommandName = "Page" Then
            Return
        End If

        Dim FirstName As String
        Dim LastName As String
        Dim Telephone As String
        Dim AreaCode As String = ""
        Dim Prefix As String = ""
        Dim TelNbr As String = ""
        Dim TelExt As String = ""
        Dim Fax As String
        Dim StartDate As Date
        Dim StaffResponsible As String

        Dim TestLogNbr As String = CType(e.Item.Cells(0).Controls(0), Button).Text

        lblTestLogNumber.Text = TestLogNbr

        Dim query As String = "select * FROM ISMPTestNotification where strTestLogNumber = @TestLogNbr"

        Dim dr As DataRow = DB.GetDataRow(query, New SqlParameter("@TestLogNbr", TestLogNbr))

        If dr IsNot Nothing Then

            StaffResponsible = GetNullableString(dr.Item("strStaffResponsible"))
            If String.IsNullOrWhiteSpace(StaffResponsible) Then StaffResponsible = "000"

            lblTestLogNumber.Text = GetNullableString(dr.Item("strTestLogNumber"))
            lblEmissionUnit.Text = GetNullableString(dr.Item("strEmissionUnit"))
            lblPollutants.Text = GetNullableString(dr.Item("strPollutants"))
            lblNotificationDate.Text = GetNullableString(dr.Item("datTestNotification"))
            StartDate = CDate(dr.Item("datProposedStartDate"))
            lblStartDate.Text = StartDate.ToShortDateString
            Dim endDate As Date = CDate(dr.Item("datProposedEndDate"))
            lblEndDate.Text = endDate.ToShortDateString
            lblComment.Text = GetNullableString(dr.Item("strComments"))
            lblConfNo.Text = GetNullableString(dr.Item("strConfirmationNumber"))
            FirstName = GetNullableString(dr.Item("strOnlineFirstName"))
            LastName = GetNullableString(dr.Item("strOnlineLastName"))
            lblContactName.Text = FirstName & " " & LastName

            If Convert.IsDBNull(dr("strTelephone")) Then
                lblTelephone.Text = "404-363-7000 (ask for Source Monitoring Unit)"
                lblExt.Text = ""
            Else
                Telephone = GetNullableString(dr.Item("strTelephone"))
                If Telephone.Length() > 10 Then
                    AreaCode = Left(Telephone, 3)
                    Prefix = Telephone.Substring(3, 3)
                    TelNbr = Telephone.Substring(6, 4)
                    TelExt = Telephone.Substring(10)
                End If

                If Telephone.Length() = 10 Then
                    AreaCode = Left(Telephone, 3)
                    Prefix = Telephone.Substring(3, 3)
                    TelNbr = Telephone.Substring(6)
                    TelExt = ""
                End If

                If Telephone.Length() < 10 Then
                    AreaCode = ""
                    Prefix = ""
                    TelNbr = ""
                    TelExt = ""
                End If

                lblTelephone.Text = AreaCode & "-" & Prefix & "-" & TelNbr
                lblExt.Text = TelExt
            End If

            If Convert.IsDBNull(dr("strFax")) Then
                lblFax.Text = "404-363-7100"
            Else
                Fax = CStr(dr.Item("strFax"))
                If Fax.Length() < 10 Then
                    lblFax.Text = ""
                Else
                    lblFax.Text = Fax.Substring(0, 3) & "-" & Fax.Substring(3, 3) & "-" & Fax.Substring(6, 4)
                End If
            End If

            lblEmail.Text = GetNullableString(dr.Item("strContactEmail"))

            If StaffResponsible = "" OrElse StaffResponsible = "000" Then
                lblEPDContact.Text = "** Not Assigned **"
                lblEPDContact.ForeColor = Drawing.Color.Red
                lblEPDTelephone.Text = "404-363-7000 (ask for Source Monitoring Unit)"
                lblEPDFax.Text = "404-363-7100"
            Else
                getEPDContact(StaffResponsible)
            End If


            If StartDate > Today Then
                lblTestPlan.Visible = True
            Else
                lblTestPlan.Visible = False
            End If

            pnlDetails.Visible = True
        Else
            pnlDetails.Visible = False
        End If
    End Sub

#End Region

End Class
