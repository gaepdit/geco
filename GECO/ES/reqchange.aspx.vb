Imports System.Data.SqlClient

Partial Class ei_reqchange
    Inherits Page

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            pnlTop.Visible = True
            pnlConfirm.Visible = False
            txtFacilityName.Text = Session("fname")
            txtStreetAddress.Text = Session("faddress")
            txtCity.Text = Session("fcity")
            txtZipCode.Text = Session("fzip")
            txtCounty.Text = Session("fcounty")
            LoadCounty()
            lblConfirmSubmit.Text = ""
            lblConfirmSubmit.Visible = False

            HideFacilityHelp()
            HideContactHelp()
            HideEmissionsHelp()
            HideSubmitHelp()

        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("esform.aspx")
    End Sub

#Region " Help Panel Routines "

    Private Sub HideFacilityHelp()

        Dim FacilityHelp = CType(Master.FindControl("pnlFacilityHelp"), Panel)

        If Not FacilityHelp Is Nothing Then
            FacilityHelp.Visible = False
        End If

    End Sub

    Private Sub HideContactHelp()

        Dim ContactHelp = CType(Master.FindControl("pnlContactHelp"), Panel)

        If Not ContactHelp Is Nothing Then
            ContactHelp.Visible = False
        End If

    End Sub

    Private Sub HideEmissionsHelp()

        Dim EmissionsHelp = CType(Master.FindControl("pnlEmissionsHelp"), Panel)

        If Not EmissionsHelp Is Nothing Then
            EmissionsHelp.Visible = False
        End If

    End Sub

    Private Sub HideSubmitHelp()

        Dim SubmitHelp = CType(Master.FindControl("pnlSubmitHelp"), Panel)

        If Not SubmitHelp Is Nothing Then
            SubmitHelp.Visible = False
        End If

    End Sub

#End Region

#Region " Load Routines "

    Private Sub LoadCounty()

        cboCountyNew.Items.Add(" --Select a County-- ")

        Dim query = "Select strCountyName FROM EILookupCountyLatLon order by strCountyName"
        Dim dt = DB.GetDataTable(query)

        For Each dr In dt.Rows
            cboCountyNew.Items.Add(dr.Item("strCountyName"))
        Next

    End Sub

#End Region

#Region " Button Routines "
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

        Dim fn As Integer
        Dim sa As Integer
        Dim cy As Integer
        Dim zc As Integer
        Dim z4 As Integer
        Dim ct As Integer
        Dim cm As Integer
        Dim empty As Integer

        'Get string lengths for entries on form to see if anything entered.
        fn = Len(txtFacilityNameNew.Text)
        sa = Len(txtStreetAddressNew.Text)
        cy = Len(txtCityNew.Text)
        zc = Len(txtZipCodeNew.Text)
        z4 = Len(txtZipPlus4New.Text)
        ct = Len(cboCountyNew.SelectedValue)
        If cboCountyNew.SelectedValue = " --Select a County-- " Then ct = 0
        cm = Len(txtComments.Text)
        empty = fn + sa + cy + zc + z4 + ct + cm

        If empty = 0 Then

            lblBlankWarning.Text = "At least one field must contain a change ..."
            lblBlankWarning.Visible = True
        Else

            txtFacilityNameConf.Text = txtFacilityNameNew.Text
            txtStreetAddressConf.Text = txtStreetAddressNew.Text
            txtCityConf.Text = txtCityNew.Text
            txtZipCodeConf.Text = txtZipCodeNew.Text
            txtZipPlus4Conf.Text = txtZipPlus4New.Text
            txtCountyConf.Text = cboCountyNew.SelectedValue
            txtCommentsConf.Text = txtComments.Text

            If cboCountyNew.SelectedValue = " --Select a County-- " Then
                txtCountyConf.Text = ""
            End If

            pnlTop.Visible = False
            pnlConfirm.Visible = True
            btnSubmitRequest.Visible = True
            btnCancelConf.Visible = True
            btnReturn.Visible = False
            lblConfirmSubmit.Text = ""
            lblConfirmSubmit.Visible = False

        End If

    End Sub

    Protected Sub btnCancelConf_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelConf.Click

        pnlTop.Visible = True
        pnlConfirm.Visible = False
        btnSubmitRequest.Visible = True
        btnCancelConf.Visible = True
        btnReturn.Visible = False
        lblConfirmSubmit.Text = ""
        lblConfirmSubmit.Visible = False
        lblBlankWarning.Text = ""
        lblBlankWarning.Visible = False

    End Sub

    Protected Sub btnSubmitRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmitRequest.Click

        Dim AirsNumber As String = Session("esAirsNumber")
        Dim UserID As String = GetCookie(GecoCookie.UserID)
        Dim FacilityName As String = txtFacilityNameNew.Text
        Dim StreetAddress As String = txtStreetAddressNew.Text
        Dim City As String = txtCityNew.Text
        Dim Zipcode As String = txtZipCodeNew.Text & txtZipPlus4New.Text
        Dim County As String = cboCountyNew.SelectedValue
        Dim Comment As String = txtComments.Text
        Dim day As String = Now.ToString("d-MMM-yyyy")
        Dim hr As String = Now.Hour
        Dim min As String = Now.Minute
        Dim sec As String = Now.Second
        If Len(hr) < 2 Then hr = "0" & hr
        If Len(min) < 2 Then min = "0" & min
        If Len(sec) < 2 Then sec = "0" & sec
        Dim TransactionTime As String = hr & ":" & min & ":" & sec
        Dim TransactionDate As String = day.ToUpper
        Dim ReqOrig As String = "ES" & Session("ESYear")

        If cboCountyNew.SelectedValue = " --Select a County-- " Then
            County = ""
        End If

        Dim query = "Insert Into eiRequest (strAirsNumber, " &
            " strUserID, " &
            " strFacilityName, " &
            " strStreetAddress, " &
            " strCity, " &
            " strZipCode, " &
            " strCounty, " &
            " strTransactionDate, " &
            " strTransactionTime, " &
            " strReqOrig, " &
            " strComments) " &
            " Values (" &
            " @AirsNumber, " &
            " @UserID, " &
            " @FacilityName, " &
            " @StreetAddress, " &
            " @City, " &
            " @Zipcode, " &
            " @County, " &
            " @TransactionDate, " &
            " @TransactionTime, " &
            " @ReqOrig, " &
            " @Comment ) "

        Dim params As SqlParameter() = {
            New SqlParameter("@AirsNumber", AirsNumber),
            New SqlParameter("@UserID", UserID),
            New SqlParameter("@FacilityName", FacilityName),
            New SqlParameter("@StreetAddress", StreetAddress),
            New SqlParameter("@City", City),
            New SqlParameter("@Zipcode", Zipcode),
            New SqlParameter("@County", County),
            New SqlParameter("@TransactionDate", TransactionDate),
            New SqlParameter("@TransactionTime", TransactionTime),
            New SqlParameter("@ReqOrig", ReqOrig),
            New SqlParameter("@Comment", Comment)
        }

        DB.RunCommand(query, params)

        btnSubmitRequest.Visible = False
        btnCancelConf.Visible = False
        btnReturn.Visible = True
        lblConfirmSubmit.Text = "Request submitted ..."
        lblConfirmSubmit.Visible = True

    End Sub

    Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click

        Response.Redirect("esform.aspx")

    End Sub

#End Region

End Class