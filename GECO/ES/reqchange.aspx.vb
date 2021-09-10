Imports System.Data.SqlClient
Imports GECO.GecoModels

Partial Class ei_reqchange
    Inherits Page

    Private Property CurrentAirs As ApbFacilityId

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim airs As String = GetSessionItem(Of String)("esAirsNumber")

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)

        If Not IsPostBack Then

            pnlTop.Visible = True
            pnlConfirm.Visible = False
            txtFacilityName.Text = GetSessionItem(Of String)("fname")
            txtStreetAddress.Text = GetSessionItem(Of String)("faddress")
            txtCity.Text = GetSessionItem(Of String)("fcity")
            txtZipCode.Text = GetSessionItem(Of String)("fzip")
            txtCounty.Text = GetSessionItem(Of String)("fcounty")
            LoadCounty()
            lblConfirmSubmit.Text = ""
            lblConfirmSubmit.Visible = False

            HideFacilityHelp()
            HideContactHelp()
            HideEmissionsHelp()
            HideSubmitHelp()

        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
        Response.Redirect("esform.aspx")
    End Sub

#Region " Help Panel Routines "

    Private Sub HideFacilityHelp()

        Dim FacilityHelp = CType(Master.FindControl("pnlFacilityHelp"), Panel)

        If FacilityHelp IsNot Nothing Then
            FacilityHelp.Visible = False
        End If

    End Sub

    Private Sub HideContactHelp()

        Dim ContactHelp = CType(Master.FindControl("pnlContactHelp"), Panel)

        If ContactHelp IsNot Nothing Then
            ContactHelp.Visible = False
        End If

    End Sub

    Private Sub HideEmissionsHelp()

        Dim EmissionsHelp = CType(Master.FindControl("pnlEmissionsHelp"), Panel)

        If EmissionsHelp IsNot Nothing Then
            EmissionsHelp.Visible = False
        End If

    End Sub

    Private Sub HideSubmitHelp()

        Dim SubmitHelp = CType(Master.FindControl("pnlSubmitHelp"), Panel)

        If SubmitHelp IsNot Nothing Then
            SubmitHelp.Visible = False
        End If

    End Sub

#End Region

#Region " Load Routines "

    Private Sub LoadCounty()

        cboCountyNew.Items.Add(" --Select a County-- ")

        Dim query = "Select strCountyName FROM EISLK_COUNTYLATLON order by strCountyName"
        Dim dt = DB.GetDataTable(query)

        For Each dr As DataRow In dt.Rows
            cboCountyNew.Items.Add(dr.Item("strCountyName").ToString)
        Next

    End Sub

#End Region

#Region " Button Routines "
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click

        Dim fn As Integer
        Dim sa As Integer
        Dim cy As Integer
        Dim zc As Integer
        Dim z4 As Integer
        Dim ct As Integer
        Dim cm As Integer
        Dim empty As Integer

        'Get string lengths for entries on form to see if anything entered.
        fn = txtFacilityNameNew.Text.Length()
        sa = txtStreetAddressNew.Text.Length()
        cy = txtCityNew.Text.Length()
        zc = txtZipCodeNew.Text.Length()
        z4 = txtZipPlus4New.Text.Length()
        ct = cboCountyNew.SelectedValue.Length()
        If cboCountyNew.SelectedValue = " --Select a County-- " Then ct = 0
        cm = txtComments.Text.Length()
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

    Protected Sub btnCancelConf_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelConf.Click

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

    Protected Sub btnSubmitRequest_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmitRequest.Click

        Dim UserID As Integer = GetCurrentUser().UserId
        Dim FacilityName As String = txtFacilityNameNew.Text
        Dim StreetAddress As String = txtStreetAddressNew.Text
        Dim City As String = txtCityNew.Text
        Dim Zipcode As String = txtZipCodeNew.Text & txtZipPlus4New.Text
        Dim County As String = cboCountyNew.SelectedValue
        Dim Comment As String = txtComments.Text
        Dim day As String = Now.ToString("d-MMM-yyyy")
        Dim hr As String = Now.Hour.ToString
        Dim min As String = Now.Minute.ToString
        Dim sec As String = Now.Second.ToString
        If hr.Length() < 2 Then hr = "0" & hr
        If min.Length() < 2 Then min = "0" & min
        If sec.Length() < 2 Then sec = "0" & sec
        Dim TransactionTime As String = hr & ":" & min & ":" & sec
        Dim TransactionDate As String = day.ToUpper
        Dim ReqOrig As String = "ES" & GetSessionItem(Of String)("ESYear")

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
            New SqlParameter("@AirsNumber", CurrentAirs.DbFormattedString),
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

    Protected Sub btnReturn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReturn.Click

        Response.Redirect("esform.aspx")

    End Sub

#End Region

End Class
