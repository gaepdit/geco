Imports System.Data
Imports System.Data.SqlClient
Imports System.DateTime
Imports EpdIt.DBUtilities

Partial Class TN_Default
    Inherits Page

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
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

                If IsDBNull(dr.Item("STRPHONE")) Then
                    lblEPDTelephone.Text = "404-363-7000 (ask For Source Monitoring Unit)"
                Else
                    EPDTelephone = dr.Item("STRPHONE")
                    lblEPDTelephone.Text = Mid(EPDTelephone, 1, 3) & "-" & Mid(EPDTelephone, 4, 3) & "-" & Mid(EPDTelephone, 7)
                End If

                If IsDBNull(dr.Item("STRFAX")) Then
                    lblEPDFax.Text = "404-363-7100"
                Else
                    EPDFaxNumber = dr.Item("STRFAX")
                    lblEPDFax.Text = Mid(EPDFaxNumber, 1, 3) & "-" & Mid(EPDFaxNumber, 4, 3) & "-" & Mid(EPDFaxNumber, 7)
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
        Dim AirsNumber As String = "0413" & GetCookie(Cookie.AirsNumber)

        Dim query As String = "select STRTESTLOGNUMBER, " &
            "STREMISSIONUNIT, " &
            "DATPROPOSEDSTARTDATE, DATPROPOSEDENDDATE, " &
            "STRCOMMENTS, NUMUSERID, STRCONTACTEMAIL, STRCONFIRMATIONNUMBER, STRAIRSNUMBER, " &
            "STRSTAFFRESPONSIBLE, STRONLINEFIRSTNAME, STRONLINELASTNAME, STRTELEPHONE, " &
            "STRFAX, DATTESTNOTIFICATION, " &
            "STRPOLLUTANTS " &
            "FROM ISMPTestNotification where strAirsNumber = @AirsNumber " &
            "order by datProposedStartDate Desc"

        Dim dt As DataTable = DB.GetDataTable(query, New SqlParameter("@AirsNumber", AirsNumber))

        Dim dv As New DataView(dt)

        SessionAdd(GecoSession.TestNotifications, dv)
        dgrTestNotify.DataSource = dv
        dgrTestNotify.DataBind()
    End Sub

    Protected Sub dgrTestNotify_PageIndexChanged(s As Object, e As DataGridPageChangedEventArgs)

        dgrTestNotify.DataSource = GetSessionItem(GecoSession.TestNotifications)
        dgrTestNotify.CurrentPageIndex = e.NewPageIndex
        dgrTestNotify.DataBind()

    End Sub

    Protected Sub RequestDetails(s As Object, e As DataGridCommandEventArgs)
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
            StartDate = GetNullableDateTime(dr.Item("datProposedStartDate"))
            lblStartDate.Text = StartDate.ToShortDateString
            Dim endDate As Date = GetNullableDateTime(dr.Item("datProposedEndDate"))
            lblEndDate.Text = endDate.ToShortDateString
            lblComment.Text = GetNullableString(dr.Item("strComments"))
            lblConfNo.Text = GetNullableString(dr.Item("strConfirmationNumber"))
            FirstName = GetNullableString(dr.Item("strOnlineFirstName"))
            LastName = GetNullableString(dr.Item("strOnlineLastName"))
            lblContactName.Text = FirstName & " " & LastName

            If IsDBNull(dr("strTelephone")) Then
                lblTelephone.Text = "404-363-7000 (ask for Source Monitoring Unit)"
                lblExt.Text = ""
            Else
                Telephone = GetNullableString(dr.Item("strTelephone"))
                If Len(Telephone) > 10 Then
                    AreaCode = Left(Telephone, 3)
                    Prefix = Mid(Telephone, 4, 3)
                    TelNbr = Mid(Telephone, 7, 4)
                    TelExt = Mid(Telephone, 11)
                End If

                If Len(Telephone) = 10 Then
                    AreaCode = Left(Telephone, 3)
                    Prefix = Mid(Telephone, 4, 3)
                    TelNbr = Mid(Telephone, 7)
                    TelExt = ""
                End If

                If Len(Telephone) < 10 Then
                    AreaCode = ""
                    Prefix = ""
                    TelNbr = ""
                    TelExt = ""
                End If

                lblTelephone.Text = AreaCode & "-" & Prefix & "-" & TelNbr
                lblExt.Text = TelExt
            End If

            If IsDBNull(dr("strFax")) Then
                lblFax.Text = "404-363-7100"
            Else
                Fax = dr.Item("strFax")
                If Len(Fax) < 10 Then
                    lblFax.Text = ""
                Else
                    lblFax.Text = Mid(Fax, 1, 3) & "-" & Mid(Fax, 4, 3) & "-" & Mid(Fax, 7, 4)
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