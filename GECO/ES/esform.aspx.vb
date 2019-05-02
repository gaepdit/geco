Imports System.Data.SqlClient
Imports System.DateTime
Imports System.Math

Partial Class es_esform
    Inherits Page

    Private SavedES As Boolean
    Private SavedAPB As Boolean
    Private ESExist As Boolean

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim CountyName As String
        Dim intESYear As Integer = Now.Year - 1
        Dim strESYear As String = intESYear.ToString
        Dim EntryBegan As Boolean

        Session("ESYear") = strESYear
        Session("esAirsNumber") = "0413" & GetCookie(Cookie.AirsNumber)
        Session("AirsYear") = Session("esAirsNumber") & Session("ESYear")
        Dim AirsYear As String = Session("AirsYear")

        If Not IsPostBack Then

            LoadState()
            LoadHorizontalCollectionCode()
            LoadHorizontalDatumReferenceCode()
            rngValXCoordinate.MinimumValue = Session("LongMin")
            rngValXCoordinate.MaximumValue = Session("LongMax")
            rngValYCoordinate.MinimumValue = Session("LatMin")
            rngValYCoordinate.MaximumValue = Session("LatMax")

            ESExist = CheckESExist(AirsYear)
            EntryBegan = CheckESEntry(AirsYear)

            If ESExist And EntryBegan Then
                LoadESSchema()
            Else
                LoadFacilityLocation()
                LoadContactInfo()
            End If

            pnlFacility.Visible = True
            pnlLatLongConvert.Visible = False
            ShowFacilityHelp()
            HideContactHelp()
            HideEmissionsHelp()
            HideSubmitHelp()

        End If

    End Sub

#Region " Load Routines "

    Private Sub LoadHorizontalCollectionCode()

        Dim query = "Select strHorizCollectionMethodCode, strHorizCollectionMethodDesc " &
            "FROM eiLookupHorizColMethod order by strHorizCollectionMethodDesc"

        cboHorizontalCollectionCode.Items.Add(" --Select a Method-- ")

        Dim dt = DB.GetDataTable(query)

        For Each dr In dt.Rows
            Dim desc = dr.Item("strHorizCollectionMethodDesc")
            Dim code = dr.Item("strHorizCollectionMethodCode")
            cboHorizontalCollectionCode.Items.Add(desc & "  [" & code & "]")
        Next

    End Sub

    Private Sub LoadHorizontalDatumReferenceCode()

        Dim query = "Select strHorizontalReferenceDatum, strHorizontalReferenceDesc FROM eiLookupHorizRefDatum order by strHorizontalReferenceDesc"

        cboHorizontalReferenceCode.Items.Add(" --Select a Code-- ")

        Dim dt = DB.GetDataTable(query)

        For Each dr In dt.Rows
            Dim desc = dr.Item("strHorizontalReferenceDesc")
            Dim code = dr.Item("strHorizontalReferenceDatum")
            cboHorizontalReferenceCode.Items.Add(desc & "  [" & code & "]")
        Next

    End Sub

    Private Sub LoadState()

        Dim query = "Select Abbreviation FROM tblState order by Abbreviation"

        cboContactState.Items.Add(" -- ")

        Dim dt = DB.GetDataTable(query)

        For Each dr In dt.Rows
            cboContactState.Items.Add(dr.Item("Abbreviation"))
        Next

        cboContactState.SelectedIndex = 0

    End Sub

#End Region

#Region " Button Routines "

    Protected Sub cboYesNo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYesNo.SelectedIndexChanged

        If cboYesNo.SelectedValue = "YES" Then
            txtVOC.Text = ""
            txtNOx.Text = ""
            pnlEmissions.Visible = False
        Else
            pnlEmissions.Visible = True
        End If

    End Sub

    Protected Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinue.Click

        If cboYesNo.SelectedValue = "YES" Then
            Session("ESOptOut") = "YES"
        Else
            Session("ESOptOut") = "NO"
        End If
        Response.Redirect("confirm.aspx")

    End Sub

    Protected Sub btnContinueToContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueToContact.Click
        mltiViewESFacility.ActiveViewIndex = 1
        HideFacilityHelp()
        ShowContactHelp()
        HideEmissionsHelp()
        HideSubmitHelp()
        lblTop.Focus()
    End Sub

    Protected Sub btnContinueToEmissions_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinueToEmissions.Click
        mltiViewESFacility.ActiveViewIndex = 2
        HideFacilityHelp()
        HideContactHelp()
        ShowEmissionsHelp()
        HideSubmitHelp()
        lblTop.Focus()
    End Sub

    Protected Sub btnCancelLocation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelLocation.Click
        Response.Redirect("~/Facility/")
    End Sub

    Protected Sub btnCancelContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelContact.Click
        Response.Redirect("~/Facility/")
    End Sub

    Protected Sub btnCancelEmission_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelEmission.Click
        Response.Redirect("~/Facility/")
    End Sub

    Protected Sub btnbackToLocation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnbackToLocation.Click
        mltiViewESFacility.ActiveViewIndex = 0
        ShowFacilityHelp()
        HideContactHelp()
        HideEmissionsHelp()
    End Sub

    Protected Sub btnBackToContactInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBackToContactInfo.Click
        mltiViewESFacility.ActiveViewIndex = 1
        HideFacilityHelp()
        ShowContactHelp()
        HideEmissionsHelp()
    End Sub

    Protected Sub btnRequestChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequestChange.Click

        Session("fname") = txtFacilityName.Text
        Session("faddress") = txtLocationAddress.Text
        Session("fcity") = txtCity.Text
        Session("fzip") = txtZipCode.Text
        Session("fcounty") = txtCounty.Text
        Response.Redirect("reqchange.aspx")

    End Sub

#End Region

#Region " Load Facility & Contact Info "

    Private Sub LoadESSchema()

        Dim FacilityZip As String
        Dim AirsNumber As String = Session("esAirsNumber")
        Dim AirsYear As String = AirsNumber & Session("ESYear")
        Dim ContactFaxNumber As String
        Dim ContactZip As String
        Dim YesNo As String
        Dim VOCAmt As Double
        Dim NOXAmt As Double
        Dim HCCcode As String
        Dim HCCdesc As String
        Dim HRCcode As String
        Dim HRCdesc As String

        Dim query = "Select strFacilityName, " &
            "strFacilityAddress, " &
            "strFacilityCity, " &
            "strFacilityZip, " &
            "strCounty, " &
            "strHorizontalCollectionCode, " &
            "strHorizontalAccuracyMeasure, " &
            "strHorizontalReferenceCode, " &
            "dblXCoordinate, " &
            "dblYCoordinate, " &
            "strContactPrefix, " &
            "strContactFirstName, " &
            "strContactLastName, " &
            "strContactTitle, " &
            "strContactCompany, " &
            "strContactPhoneNumber, " &
            "strContactFaxNumber, " &
            "strContactEmail, " &
            "strContactAddress1, " &
            "strContactCity, " &
            "strContactState, " &
            "strContactZip, " &
            "dblVOCEmission, " &
            "dblNOXEmission, " &
            "strOptOut " &
            "FROM esSchema Where strAirsYear = @AirsYear "

        Dim param As New SqlParameter("@AirsYear", AirsYear)

        Dim dr = DB.GetDataRow(query, param)

        If dr IsNot Nothing Then

            If IsDBNull(dr("strFacilityName")) Then
                txtFacilityName.Text = ""
            Else
                txtFacilityName.Text = dr.Item("strFacilityName")
            End If
            If IsDBNull(dr("strFacilityAddress")) Then
                txtLocationAddress.Text = ""
            Else
                txtLocationAddress.Text = dr.Item("strFacilityAddress")
            End If
            If IsDBNull(dr("strFacilityCity")) Then
                txtCity.Text = ""
            Else
                txtCity.Text = dr.Item("strFacilityCity")
            End If

            txtState.Text = "GA"

            If IsDBNull(dr("strFacilityZip")) Then
                txtZipCode.Text = ""
            Else
                FacilityZip = dr("strFacilityZip")
                FacilityZip = Replace(FacilityZip, "-", "")
                If Len(FacilityZip) > 5 Then
                    txtZipCode.Text = Left(FacilityZip, 5) & "-" & Mid(FacilityZip, 6, 4)
                Else
                    txtZipCode.Text = FacilityZip
                End If
            End If

            If IsDBNull(dr("strCounty")) Then
                txtCounty.Text = ""
            Else
                txtCounty.Text = dr.Item("strCounty")
            End If

            If IsDBNull(dr("dblXCoordinate")) Then
                txtXCoordinate.Text = ""
            Else
                txtXCoordinate.Text = Round(Abs(dr.Item("dblXCoordinate")), 6)
            End If
            If IsDBNull(dr("dblYCoordinate")) Then
                txtYCoordinate.Text = ""
            Else
                txtYCoordinate.Text = Round(Abs(dr.Item("dblYCoordinate")), 6)
            End If
            If IsDBNull(dr("strHorizontalCollectionCode")) Then
                cboHorizontalCollectionCode.SelectedIndex = 0
            Else
                HCCcode = dr.Item("strHorizontalCollectionCode")
                HCCdesc = GetHorizCollDesc(HCCcode)
                cboHorizontalCollectionCode.SelectedValue = HCCdesc & "  [" & HCCcode & "]"
            End If
            If IsDBNull(dr("strHorizontalReferenceCode")) Then
                cboHorizontalReferenceCode.SelectedIndex = 0
            Else
                HRCcode = dr.Item("strHorizontalReferenceCode")
                HRCdesc = GetHorizRefDesc(HRCcode)
                cboHorizontalReferenceCode.SelectedValue = HRCdesc & "  [" & HRCcode & "]"
            End If
            If IsDBNull(dr("strHorizontalAccuracyMeasure")) Then
                txtHorizontalAccuracyMeasure.Text = ""
            Else
                txtHorizontalAccuracyMeasure.Text = dr.Item("strHorizontalAccuracyMeasure")
            End If

            If IsDBNull(dr("strContactPrefix")) Then
                txtContactPrefix.Text = ""
            Else
                txtContactPrefix.Text = dr.Item("strContactPrefix")
            End If
            If IsDBNull(dr("strContactTitle")) Then
                txtContactTitle.Text = ""
            Else
                txtContactTitle.Text = dr.Item("strContactTitle")
            End If

            If IsDBNull(dr("strContactFirstName")) Then
                txtContactFirstName.Text = ""
            Else
                txtContactFirstName.Text = dr.Item("strContactFirstName")
            End If

            If IsDBNull(dr("strContactLastName")) Then
                txtContactLastName.Text = ""
            Else
                txtContactLastName.Text = dr.Item("strContactLastName")
            End If

            If IsDBNull(dr("strContactCompany")) Then
                txtContactCompanyName.Text = ""
            Else
                txtContactCompanyName.Text = dr.Item("strContactCompany")
            End If
            If Not IsDBNull(dr("strContactPhoneNumber")) Then
                txtOfficePhoneNbr.Text = dr.Item("strContactPhoneNumber")
            End If
            If Not IsDBNull(dr("strContactFaxNumber")) Then
                ContactFaxNumber = dr.Item("strContactFaxNumber")
                txtFaxNbr.Text = Mid(ContactFaxNumber, 1, 10)
            End If
            If IsDBNull(dr("strContactEmail")) Then
                txtContactEmail.Text = ""
            Else
                txtContactEmail.Text = dr.Item("strContactEmail")
            End If
            If IsDBNull(dr("strContactAddress1")) Then
                txtContactAddress1.Text = ""
            Else
                txtContactAddress1.Text = dr.Item("strContactAddress1")
            End If
            If IsDBNull(dr("strContactCity")) Then
                txtContactCity.Text = ""
            Else
                txtContactCity.Text = dr.Item("strContactCity")
            End If
            If IsDBNull(dr("strContactState")) Then
                cboContactState.SelectedIndex = 0
            Else
                cboContactState.SelectedValue = dr.Item("strContactState")
            End If

            If IsDBNull(dr("strContactZip")) Then
                txtContactZipCode.Text = ""
            Else
                ContactZip = dr.Item("strContactZip")
                ContactZip = Replace(ContactZip, "-", "")
                If Len(ContactZip) > 5 Then
                    txtContactZipCode.Text = Left(dr.Item("strContactZip"), 5)
                    txtContactZipPlus4.Text = Mid(dr.Item("strContactZip"), 6, 4)
                Else
                    txtContactZipCode.Text = dr.Item("strContactZip")
                End If
            End If

            If IsDBNull(dr("strOptOut")) Then
                YesNo = ""
                cboYesNo.SelectedIndex = 0
                pnlEmissions.Visible = True
            Else
                YesNo = dr.Item("strOptOut")
                cboYesNo.SelectedValue = YesNo.ToUpper
                If YesNo = "NO" Then
                    pnlEmissions.Visible = True
                Else
                    pnlEmissions.Visible = False
                End If
            End If

            If YesNo = "NO" Then
                If IsDBNull(dr("dblVOCEmission")) Then
                    txtVOC.Text = ""
                Else
                    VOCAmt = dr.Item("dblVOCEmission")
                    If VOCAmt <= 0 Then
                        txtVOC.Text = ""
                    Else
                        txtVOC.Text = Round(VOCAmt, 2)
                    End If
                End If
                If IsDBNull(dr("dblNOXEmission")) Then
                    txtNOx.Text = ""
                Else
                    NOXAmt = dr.Item("dblNOXEmission")
                    If NOXAmt < 0 Then
                        txtNOx.Text = ""
                    Else
                        txtNOx.Text = Round(NOXAmt, 2)
                    End If
                End If
                pnlEmissions.Visible = True
            ElseIf YesNo = "YES" Then
                txtVOC.Text = ""
                txtNOx.Text = ""
                'pnlEmissions.Visible = False
            ElseIf YesNo = "--" Then
                If IsDBNull(dr("dblVOCEmission")) Then
                    txtVOC.Text = ""
                Else
                    VOCAmt = dr.Item("dblVOCEmission")
                    If VOCAmt <= 0 Then
                        txtVOC.Text = ""
                    Else
                        txtVOC.Text = Round(VOCAmt, 2)
                    End If
                End If
                If IsDBNull(dr("dblNOXEmission")) Then
                    txtNOx.Text = ""
                Else
                    NOXAmt = dr.Item("dblNOXEmission")
                    If NOXAmt <= 0 Then
                        txtNOx.Text = ""
                    Else
                        txtNOx.Text = Round(NOXAmt, 2)
                    End If
                End If
                pnlEmissions.Visible = True
            End If
        End If

    End Sub

    Private Sub LoadFacilityLocation()

        'Load facility and contact info FROM  apbFacilityInformation table

        Dim FacilityZip As String
        Dim AirsNumber As String = Session("esAirsNumber")
        Dim HCCcode As String
        Dim HCCdesc As String
        Dim HRCcode As String
        Dim HRCdesc As String

        Dim query = "select strFacilityName, " &
            "strFacilityStreet1, " &
            "strFacilityCity, " &
            "strFacilityState, " &
            "strFacilityZipCode, " &
            "strHorizontalCollectionCode, " &
            "strHorizontalAccuracyMeasure, " &
            "strHorizontalReferenceCode, " &
            "numFacilityLongitude, " &
            "numFacilityLatitude " &
            "FROM apbFacilityInformation where strAirsNumber = @AirsNumber "

        Dim param As New SqlParameter("@AirsNumber", AirsNumber)

        Dim dr = DB.GetDataRow(query, param)

        If dr IsNot Nothing Then

            If IsDBNull(dr("strFacilityName")) Then
                txtFacilityName.Text = ""
            Else
                txtFacilityName.Text = dr.Item("strFacilityName")
            End If
            If IsDBNull(dr("strFacilityStreet1")) Then
                txtLocationAddress.Text = ""
            Else
                txtLocationAddress.Text = dr.Item("strFacilityStreet1")
            End If
            If IsDBNull(dr("strFacilityCity")) Then
                txtCity.Text = ""
            Else
                txtCity.Text = dr.Item("strFacilityCity")
            End If

            txtState.Text = "GA"

            If IsDBNull(dr("strFacilityZipCode")) Then
                txtZipCode.Text = ""
            Else
                FacilityZip = dr("strFacilityZipCode")
                FacilityZip = Replace(FacilityZip, "-", "")
                If Len(FacilityZip) > 5 Then
                    txtZipCode.Text = Left(FacilityZip, 5) & "-" & Mid(FacilityZip, 6, 4)
                Else
                    txtZipCode.Text = FacilityZip
                End If
            End If

            txtCounty.Text = GetCounty(AirsNumber)

            If IsDBNull(dr("numFacilityLongitude")) Then
                txtXCoordinate.Text = ""
            Else
                txtXCoordinate.Text = Round(Abs(dr.Item("numFacilityLongitude")), 6)
            End If
            If IsDBNull(dr("numFacilityLatitude")) Then
                txtYCoordinate.Text = ""
            Else
                txtYCoordinate.Text = Round(dr.Item("numFacilityLatitude"), 6)
            End If
            If IsDBNull(dr("strHorizontalCollectionCode")) Then
                cboHorizontalCollectionCode.SelectedIndex = 0
            Else
                HCCcode = dr.Item("strHorizontalCollectionCode")
                HCCdesc = GetHorizCollDesc(HCCcode)
                cboHorizontalCollectionCode.SelectedValue = HCCdesc & "  [" & HCCcode & "]"
            End If
            If IsDBNull(dr("strHorizontalReferenceCode")) Then
                cboHorizontalReferenceCode.SelectedIndex = 0
            Else
                HRCcode = dr.Item("strHorizontalReferenceCode")
                HRCdesc = GetHorizRefDesc(HRCcode)
                cboHorizontalReferenceCode.SelectedValue = HRCdesc & "  [" & HRCcode & "]"
            End If
            If IsDBNull(dr("strHorizontalAccuracyMeasure")) Then
                txtHorizontalAccuracyMeasure.Text = ""
            Else
                txtHorizontalAccuracyMeasure.Text = dr.Item("strHorizontalAccuracyMeasure")
            End If

        End If

    End Sub

    Private Sub LoadContactInfo()

        'Load contact info FROM  apbContactInformation table

        Dim ContactFaxNumber As String
        Dim ContactZip As String
        Dim ContactKey = Session("esAirsNumber") & "42"
        Dim Exist As Boolean

        'Check if contact exists in apbContactInformation table.
        'If it does not, the database call for the contact info is skipped.
        Exist = ContactExistAPB()

        If Exist Then

            Dim query = "select strContactPrefix, " &
                "strContactFirstName, " &
                "strContactLastName, " &
                "strContactTitle, " &
                "strContactCompanyName, " &
                "strContactPhoneNumber1, " &
                "strContactPhoneNumber2, " &
                "strContactFaxNumber, " &
                "strContactEmail, " &
                "strContactAddress1, " &
                "strContactCity, " &
                "strContactState, " &
                "strContactZipCode " &
                "FROM apbContactInformation where strContactKey = @ContactKey "

            Dim param As New SqlParameter("@ContactKey", ContactKey)

            Dim dr = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then

                If IsDBNull(dr("strContactPrefix")) Then
                    txtContactPrefix.Text = ""
                Else
                    txtContactPrefix.Text = dr.Item("strContactPrefix")
                End If
                If IsDBNull(dr("strContactFirstName")) Then
                    txtContactFirstName.Text = ""
                Else
                    txtContactFirstName.Text = dr.Item("strContactFirstName")
                End If
                If IsDBNull(dr("strContactLastName")) Then
                    txtContactLastName.Text = ""
                Else
                    txtContactLastName.Text = dr.Item("strContactLastName")
                End If
                If IsDBNull(dr("strContactTitle")) Then
                    txtContactTitle.Text = ""
                Else
                    txtContactTitle.Text = dr.Item("strContactTitle")
                End If
                If IsDBNull(dr("strContactCompanyName")) Then
                    txtContactCompanyName.Text = ""
                Else
                    txtContactCompanyName.Text = dr.Item("strContactCompanyName")
                End If
                If Not IsDBNull(dr("strContactPhoneNumber1")) Then
                    txtOfficePhoneNbr.Text = dr.Item("strContactPhoneNumber1")
                End If
                If Not IsDBNull(dr("strContactFaxNumber")) Then
                    ContactFaxNumber = dr.Item("strContactFaxNumber")
                    txtFaxNbr.Text = Mid(ContactFaxNumber, 1, 10)
                End If
                If IsDBNull(dr("strContactEmail")) Then
                    txtContactEmail.Text = ""
                Else
                    txtContactEmail.Text = dr.Item("strContactEmail")
                End If
                If IsDBNull(dr("strContactAddress1")) Then
                    txtContactAddress1.Text = ""
                Else
                    txtContactAddress1.Text = dr.Item("strContactAddress1")
                End If
                If IsDBNull(dr("strContactCity")) Then
                    txtContactCity.Text = ""
                Else
                    txtContactCity.Text = dr.Item("strContactCity")
                End If
                If IsDBNull(dr("strContactState")) Then
                    cboContactState.SelectedIndex = 0
                Else
                    cboContactState.SelectedValue = dr.Item("strContactState")
                End If

                If IsDBNull(dr("strContactZipCode")) Then
                    txtContactZipCode.Text = ""
                Else
                    ContactZip = dr.Item("strContactZipCode")
                    ContactZip = Replace(ContactZip, "-", "")
                    If Len(ContactZip) > 5 Then
                        txtContactZipCode.Text = Left(dr.Item("strContactZipCode"), 5)
                        txtContactZipPlus4.Text = Mid(dr.Item("strContactZipCode"), 6, 4)
                    Else
                        txtContactZipCode.Text = dr.Item("strContactZipCode")
                    End If
                End If

            End If

        End If

    End Sub

#End Region

#Region " Save Facility & Contact Info "

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click

        If txtNOx.Text = "0" And txtVOC.Text = "0" Then
            lblVOCNOXZero.Text = "VOC and NOx cannot both be zero."
        Else
            SaveES()
            SaveContactAPB()
            UpdateAPBFacilityInfo()
        End If

        If SavedES And SavedAPB Then

            If cboYesNo.SelectedValue = "YES" Then
                Session("ESOptOut") = "YES"
            Else
                Session("ESOptOut") = "NO"
            End If

            Response.Redirect("confirm.aspx")

        End If

    End Sub

    Private Sub SaveES()

        Dim AirsNumber As String = Session("esAirsNumber")
        Dim AirsYear As String = Session("AirsYear")
        Dim LocationAddress As String
        Dim City As String
        Dim State As String
        Dim ZipCode As String
        Dim County As String
        Dim ContactFirstName As String = txtContactFirstName.Text
        Dim ContactLastName As String = txtContactLastName.Text
        Dim ContactPrefix As String
        Dim ContactTitle As String
        Dim ContactCompanyName As String
        Dim ContactPhoneNumber1 As String
        Dim ContactFaxNumber As String
        Dim ContactEmail As String
        Dim ContactAddress1 As String
        Dim ContactCity As String
        Dim ContactState As String
        Dim ContactZipCode As String
        Dim UserID As Integer = GetCookie(GecoCookie.UserID)
        Dim day As String = Now.ToString("d-MMM-yyyy")
        Dim hr As String = Now.Hour
        Dim min As String = Now.Minute
        Dim sec As String = Now.Second
        If Len(hr) < 2 Then hr = "0" & hr
        If Len(min) < 2 Then min = "0" & min
        If Len(sec) < 2 Then sec = "0" & sec
        Dim TimeLastLogin As String = hr & ":" & min & ":" & sec
        Dim DateLastLogin As String = day.ToUpper
        Dim FirstConfirm As Boolean
        Dim VOCAmt As String
        Dim NOXAmt As String
        Dim OptOut As String
        Dim ConfNum As String
        Dim XCoordinate As Decimal
        Dim YCoordinate As Decimal
        Dim HorizontalCollectionCode As String
        Dim HorizontalAccuracyMeasure As String
        Dim HorizontalReferenceCode As String
        Dim HCD As String
        Dim HRD As String

        ConfNum = Right(AirsNumber, 8) & Replace(DateLastLogin, "-", "") & Replace(TimeLastLogin, ":", "")

        SavedES = False
        ESExist = CheckESExist(AirsYear)
        FirstConfirm = CheckFirstConfirm(AirsYear)

        LocationAddress = txtLocationAddress.Text
        City = txtCity.Text
        State = txtState.Text
        ZipCode = Replace(txtZipCode.Text, "-", "")
        County = txtCounty.Text
        ContactPrefix = txtContactPrefix.Text
        ContactTitle = txtContactTitle.Text
        ContactCompanyName = txtContactCompanyName.Text
        ContactPhoneNumber1 = txtOfficePhoneNbr.Text
        ContactFaxNumber = txtFaxNbr.Text
        ContactEmail = txtContactEmail.Text
        ContactAddress1 = txtContactAddress1.Text
        ContactCity = txtContactCity.Text
        ContactState = cboContactState.SelectedValue
        ContactZipCode = txtContactZipCode.Text & txtContactZipPlus4.Text

        XCoordinate = -1 * CDec(txtXCoordinate.Text)
        YCoordinate = CDec(txtYCoordinate.Text)

        HCD = cboHorizontalCollectionCode.SelectedValue
        HorizontalCollectionCode = Mid(HCD, Len(HCD) - 3, 3)

        HRD = cboHorizontalReferenceCode.SelectedValue
        HorizontalReferenceCode = Mid(HRD, Len(HRD) - 3, 3)

        HorizontalAccuracyMeasure = txtHorizontalAccuracyMeasure.Text

        If cboYesNo.SelectedValue = "YES" Then
            VOCAmt = Nothing
            NOXAmt = Nothing
            OptOut = "YES"
        Else
            VOCAmt = Round(CDec(txtVOC.Text), 2).ToString
            NOXAmt = Round(CDec(txtNOx.Text), 2).ToString
            OptOut = "NO"
        End If

        Dim query As String

        If ESExist Then
            If FirstConfirm Then
                query = "Update esSchema " &
                    " Set STRFACILITYADDRESS = @LocationAddress, " &
                    " STRFACILITYCITY = @City, " &
                    " STRFACILITYSTATE = @State, " &
                    " STRFACILITYZIP = @ZipCode, " &
                    " STRCOUNTY = @County, " &
                    " STRCONTACTPREFIX = @ContactPrefix, " &
                    " STRCONTACTFIRSTNAME = @ContactFirstName, " &
                    " STRCONTACTLASTNAME = @ContactLastName, " &
                    " STRCONTACTTITLE = @ContactTitle, " &
                    " STRCONTACTCOMPANY = @ContactCompanyName, " &
                    " STRCONTACTPHONENUMBER = @ContactPhoneNumber1, " &
                    " STRCONTACTFAXNUMBER = @ContactFaxNumber, " &
                    " STRCONTACTEMAIL = @ContactEmail, " &
                    " STRCONTACTADDRESS1 = @ContactAddress1, " &
                    " STRCONTACTCITY = @ContactCity, " &
                    " STRCONTACTSTATE = @ContactState, " &
                    " STRCONTACTZIP = @ContactZipCode, " &
                    " STRHORIZONTALCOLLECTIONCODE = @HorizontalCollectionCode, " &
                    " STRHORIZONTALREFERENCECODE = @HorizontalReferenceCode, " &
                    " STRHORIZONTALACCURACYMEASURE = @HorizontalAccuracyMeasure, " &
                    " DBLXCOORDINATE = @XCoordinate, " &
                    " DBLYCOORDINATE = @YCoordinate, " &
                    " NUMUSERID = @UserID, " &
                    " DBLVOCEMISSION = @VOCAmt, " &
                    " DBLNOXEMISSION = @NOXAmt, " &
                    " STROPTOUT = @OptOut, " &
                    " STRCONFIRMATIONNBR = @ConfNum, " &
                    " strDateLastLogin = @DateLastLogin, " &
                    " strTimeLastLogin = @TimeLastLogin, " &
                    " DATTRANSACTION = getdate() " &
                    " where STRAIRSYEAR = @AirsYear "
            Else
                query = "Update esSchema " &
                    " Set STRFACILITYADDRESS = @LocationAddress, " &
                    " STRFACILITYCITY = @City, " &
                    " STRFACILITYSTATE = @State, " &
                    " STRFACILITYZIP = @ZipCode, " &
                    " STRCOUNTY = @County, " &
                    " STRCONTACTPREFIX = @ContactPrefix, " &
                    " STRCONTACTFIRSTNAME = @ContactFirstName, " &
                    " STRCONTACTLASTNAME = @ContactLastName, " &
                    " STRCONTACTTITLE = @ContactTitle, " &
                    " STRCONTACTCOMPANY = @ContactCompanyName, " &
                    " STRCONTACTPHONENUMBER = @ContactPhoneNumber1, " &
                    " STRCONTACTFAXNUMBER = @ContactFaxNumber, " &
                    " STRCONTACTEMAIL = @ContactEmail, " &
                    " STRCONTACTADDRESS1 = @ContactAddress1, " &
                    " STRCONTACTCITY = @ContactCity, " &
                    " STRCONTACTSTATE = @ContactState, " &
                    " STRCONTACTZIP = @ContactZipCode, " &
                    " STRHORIZONTALCOLLECTIONCODE = @HorizontalCollectionCode, " &
                    " STRHORIZONTALREFERENCECODE = @HorizontalReferenceCode, " &
                    " STRHORIZONTALACCURACYMEASURE = @HorizontalAccuracyMeasure, " &
                    " DBLXCOORDINATE = @XCoordinate, " &
                    " DBLYCOORDINATE = @YCoordinate, " &
                    " NUMUSERID = @UserID, " &
                    " DBLVOCEMISSION = @VOCAmt, " &
                    " DBLNOXEMISSION = @NOXAmt, " &
                    " STROPTOUT = @OptOut, " &
                    " STRCONFIRMATIONNBR = @ConfNum, " &
                    " strDateLastLogin = @DateLastLogin, " &
                    " strTimeLastLogin = @TimeLastLogin, " &
                    " strDateFirstConfirm = @DateLastLogin, " &
                    " DATTRANSACTION = getdate() " &
                    " where STRAIRSYEAR = @AirsYear "
            End If

            Dim params As SqlParameter() = {
                New SqlParameter("@LocationAddress", LocationAddress),
                New SqlParameter("@City", City),
                New SqlParameter("@State", State),
                New SqlParameter("@ZipCode", ZipCode),
                New SqlParameter("@County", County),
                New SqlParameter("@ContactPrefix", ContactPrefix),
                New SqlParameter("@ContactFirstName", ContactFirstName),
                New SqlParameter("@ContactLastName", ContactLastName),
                New SqlParameter("@ContactTitle", ContactTitle),
                New SqlParameter("@ContactCompanyName", ContactCompanyName),
                New SqlParameter("@ContactPhoneNumber1", ContactPhoneNumber1),
                New SqlParameter("@ContactFaxNumber", ContactFaxNumber),
                New SqlParameter("@ContactEmail", ContactEmail),
                New SqlParameter("@ContactAddress1", ContactAddress1),
                New SqlParameter("@ContactCity", ContactCity),
                New SqlParameter("@ContactState", ContactState),
                New SqlParameter("@ContactZipCode", ContactZipCode),
                New SqlParameter("@HorizontalCollectionCode", HorizontalCollectionCode),
                New SqlParameter("@HorizontalReferenceCode", HorizontalReferenceCode),
                New SqlParameter("@HorizontalAccuracyMeasure", HorizontalAccuracyMeasure),
                New SqlParameter("@XCoordinate ", XCoordinate),
                New SqlParameter("@YCoordinate ", YCoordinate),
                New SqlParameter("@UserID ", UserID),
                New SqlParameter("@VOCAmt ", VOCAmt),
                New SqlParameter("@NOXAmt ", NOXAmt),
                New SqlParameter("@OptOut", OptOut),
                New SqlParameter("@ConfNum", ConfNum),
                New SqlParameter("@DateLastLogin", DateLastLogin),
                New SqlParameter("@TimeLastLogin", TimeLastLogin),
                New SqlParameter("@AirsYear ", AirsYear)
            }

            DB.RunCommand(query, params)
        End If

        SavedES = True

    End Sub

    Private Sub SaveContactAPB()

        Dim AirsNumber As String = Session("esAirsNumber")
        Dim ContactKey As String = Session("esAirsNumber") & "42"
        Dim Ckey As String = "42"
        Dim ContactPrefix As String = txtContactPrefix.Text
        Dim ContactFirstName As String = txtContactFirstName.Text
        Dim ContactLastName As String = txtContactLastName.Text
        Dim ContactTitle As String = txtContactTitle.Text
        Dim ContactEmail As String = txtContactEmail.Text
        Dim OfficePhone As String = txtOfficePhoneNbr.Text
        Dim Fax As String = txtFaxNbr.Text
        Dim ContactCompanyName As String = txtContactCompanyName.Text
        Dim ContactAddress1 As String = txtContactAddress1.Text
        Dim ContactCity As String = txtContactCity.Text
        Dim ContactState As String = cboContactState.SelectedValue
        Dim ContactZipCode As String = txtContactZipCode.Text & txtContactZipPlus4.Text
        Dim ModPerson As String = "0"
        Dim ContactDescription As String = "ES Contact"
        Dim Exist As Boolean

        Exist = ContactExistAPB()
        SavedAPB = False

        Dim query As String

        If Exist Then
            query = "Update apbContactInformation Set " &
                "strContactPrefix = @ContactPrefix, " &
                "strContactFirstName = @ContactFirstName, " &
                "strContactLastName = @ContactLastName, " &
                "strContactTitle = @ContactTitle, " &
                "strContactEmail = @ContactEmail, " &
                "strContactPhoneNumber1 = @OfficePhone, " &
                "strContactFaxNumber = @Fax, " &
                "strContactCompanyName = @ContactCompanyName, " &
                "strContactAddress1 = @ContactAddress1, " &
                "strContactCity = @ContactCity, " &
                "strContactState = @ContactState, " &
                "strContactZipCode = @ContactZipCode, " &
                "strModifingPerson = @ModPerson, " &
                "strContactDescription = @ContactDescription, " &
                "datModifingDate = getdate() " &
                "where strContactKey = @ContactKey "
        Else
            query = "Insert into apbContactInformation (" &
                " strContactKey, " &
                " strKey, " &
                " strAirsNumber, " &
                " strContactPrefix, " &
                " strContactFirstName, " &
                " strContactLastName, " &
                " strContactTitle, " &
                " strContactEmail, " &
                " strContactPhoneNumber1, " &
                " strContactFaxNumber, " &
                " strContactCompanyName, " &
                " strContactAddress1, " &
                " strContactCity, " &
                " strContactState, " &
                " strContactZipCode, " &
                " strModifingPerson, " &
                " strContactDescription, " &
                " datModifingDate) " &
                " Values (" &
                " @ContactKey, " &
                " @Ckey, " &
                " @AirsNumber, " &
                " @ContactPrefix, " &
                " @ContactFirstName, " &
                " @ContactLastName, " &
                " @ContactTitle, " &
                " @ContactEmail, " &
                " @OfficePhone, " &
                " @Fax, " &
                " @ContactCompanyName, " &
                " @ContactAddress1, " &
                " @ContactCity, " &
                " @ContactState, " &
                " @ContactZipCode, " &
                " @ModPerson, " &
                " @ContactDescription, " &
                " getdate() ) "
        End If

        Dim params As SqlParameter() = {
            New SqlParameter("@ContactKey", ContactKey),
            New SqlParameter("@Ckey", Ckey),
            New SqlParameter("@AirsNumber", AirsNumber),
            New SqlParameter("@ContactPrefix", ContactPrefix),
            New SqlParameter("@ContactFirstName", ContactFirstName),
            New SqlParameter("@ContactLastName", ContactLastName),
            New SqlParameter("@ContactTitle", ContactTitle),
            New SqlParameter("@ContactEmail", ContactEmail),
            New SqlParameter("@OfficePhone", OfficePhone),
            New SqlParameter("@Fax", Fax),
            New SqlParameter("@ContactCompanyName", ContactCompanyName),
            New SqlParameter("@ContactAddress1", ContactAddress1),
            New SqlParameter("@ContactCity", ContactCity),
            New SqlParameter("@ContactState", ContactState),
            New SqlParameter("@ContactZipCode", ContactZipCode),
            New SqlParameter("@ModPerson", ModPerson),
            New SqlParameter("@ContactDescription", ContactDescription)
        }

        DB.RunCommand(query, params)

        SavedAPB = True

    End Sub

#End Region

#Region "Check Routines "

    Private Function ContactExistAPB() As Boolean

        Dim key As String = Session("esAirsNumber") & "42"

        Dim query = "Select strContactKey FROM apbContactInformation Where strContactKey = @key "
        Dim param As New SqlParameter("@key", key)

        Return DB.ValueExists(query, param)

    End Function

#End Region

#Region " Help Panel Routines "

    Private Sub ShowFacilityHelp()

        Dim FacilityHelp = CType(Master.FindControl("pnlFacilityHelp"), Panel)

        If Not FacilityHelp Is Nothing Then
            FacilityHelp.Visible = True
        End If

    End Sub

    Private Sub HideFacilityHelp()

        Dim FacilityHelp = CType(Master.FindControl("pnlFacilityHelp"), Panel)

        If Not FacilityHelp Is Nothing Then
            FacilityHelp.Visible = False
        End If

    End Sub

    Private Sub ShowContactHelp()

        Dim ContactHelp = CType(Master.FindControl("pnlContactHelp"), Panel)

        If Not ContactHelp Is Nothing Then
            ContactHelp.Visible = True
        End If

    End Sub

    Private Sub HideContactHelp()

        Dim ContactHelp = CType(Master.FindControl("pnlContactHelp"), Panel)

        If Not ContactHelp Is Nothing Then
            ContactHelp.Visible = False
        End If

    End Sub

    Private Sub ShowEmissionsHelp()

        Dim EmissionsHelp = CType(Master.FindControl("pnlEmissionsHelp"), Panel)

        If Not EmissionsHelp Is Nothing Then
            EmissionsHelp.Visible = True
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

    Protected Sub btnConvert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConvert.Click
        txtLongDec.Text = Abs(GetDecDegree(txtLonDeg.Text, txtLonMin.Text, txtLonSec.Text))
        txtLatDec.Text = GetDecDegree(txtLatDeg.Text, txtLatMin.Text, txtLatSec.Text)
    End Sub

    Protected Sub btnCancelLatLong_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelLatLong.Click

        pnlFacility.Visible = True
        pnlLatLongConvert.Visible = False
        txtLonDeg.Text = ""
        txtLonMin.Text = ""
        txtLonSec.Text = ""
        txtLatDeg.Text = ""
        txtLatMin.Text = ""
        txtLatSec.Text = ""
        txtLongDec.Text = ""
        txtLatDec.Text = ""
        lblDecLatLongEmpty.Text = ""
        txtXCoordinate.Focus()

    End Sub

    Protected Sub btnUseLatLong_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUseLatLong.Click

        lblDecLatLongEmpty.Text = ""

        If txtLongDec.Text = "" Or txtLatDec.Text = "" Then
            lblDecLatLongEmpty.Text = "No Longitude or Latitude values were converted to use. Perform a conversion first."
            lblDecLatLongEmpty.Visible = True
        Else
            lblDecLatLongEmpty.Text = ""
            pnlFacility.Visible = True
            pnlLatLongConvert.Visible = False
            txtXCoordinate.Text = txtLongDec.Text
            txtYCoordinate.Text = txtLatDec.Text
            txtYCoordinate.Focus()
            txtLonDeg.Text = ""
            txtLonMin.Text = ""
            txtLonSec.Text = ""
            txtLatDeg.Text = ""
            txtLatMin.Text = ""
            txtLatSec.Text = ""
            txtLongDec.Text = ""
            txtLatDec.Text = ""
        End If

    End Sub

    Protected Sub btnLatLongConvert_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLatLongConvert.Click

        rngValLongDec.MinimumValue = Session("LongMin")
        rngValLongDec.MaximumValue = Session("LongMax")
        rngValLatDec.MinimumValue = Session("LatMin")
        rngValLatDec.MaximumValue = Session("LatMax")
        pnlFacility.Visible = False
        pnlLatLongConvert.Visible = True

    End Sub

    Private Sub UpdateAPBFacilityInfo()

        Dim XCoordinate As Decimal
        Dim YCoordinate As Decimal
        Dim HorizontalCollectionCode As String
        Dim HCC As String
        Dim HorizontalAccuracyMeasure As String
        Dim HorizontalReferenceCode As String
        Dim HRC As String
        Dim AirsNumber As String = Session("esAirsNumber")

        HCC = cboHorizontalCollectionCode.SelectedValue
        HorizontalCollectionCode = Mid(HCC, InStr(HCC, "[") + 1, 3)
        HorizontalAccuracyMeasure = txtHorizontalAccuracyMeasure.Text
        HRC = cboHorizontalReferenceCode.SelectedValue
        HorizontalReferenceCode = Mid(HRC, InStr(HRC, "[") + 1, 3)
        XCoordinate = -1 * Abs(CDec(txtXCoordinate.Text))
        YCoordinate = CDec(txtYCoordinate.Text)

        Dim query = "Update APBFACILITYINFORMATION Set " &
            "STRHORIZONTALCOLLECTIONCODE = @HorizontalCollectionCode," &
            "STRHORIZONTALACCURACYMEASURE = @HorizontalAccuracyMeasure," &
            "STRHORIZONTALREFERENCECODE = @HorizontalReferenceCode," &
            "NUMFACILITYLONGITUDE = @XCoordinate, " &
            "NUMFACILITYLATITUDE =  @YCoordinate " &
            "where strAirsNumber = @AirsNumber "

        Dim params As SqlParameter() = {
            New SqlParameter("@HorizontalCollectionCode", HorizontalCollectionCode),
            New SqlParameter("@HorizontalAccuracyMeasure", HorizontalAccuracyMeasure),
            New SqlParameter("@HorizontalReferenceCode", HorizontalReferenceCode),
            New SqlParameter("@XCoordinate", XCoordinate),
            New SqlParameter("@YCoordinate", YCoordinate),
            New SqlParameter("@AirsNumber", AirsNumber)
        }

        DB.RunCommand(query, params)

    End Sub

End Class