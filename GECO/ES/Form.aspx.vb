﻿Imports System.Data.SqlClient
Imports System.DateTime
Imports System.Math
Imports GECO.GecoModels

Partial Class es_form
    Inherits Page

    Private SavedES As Boolean
    Private SavedAPB As Boolean

    Private Property CurrentAirs As ApbFacilityId

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()
        AirsSelectedCheck()

        'Check if the user has access to the Application
        Dim facilityAccess = GetCurrentUser().GetFacilityAccess(New ApbFacilityId(GetCookie(Cookie.AirsNumber).ToString))

        If Not facilityAccess.ESAccess Then
            Response.Redirect("~/NoAccess.aspx")
        End If

        Dim airs As String = GetSessionItem(Of String)("esAirsNumber")

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/Home/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.IsFacilitySet = True

        Dim esYear As String = (Now.Year - 1).ToString
        Session("ESYear") = esYear
        Dim airsYear As String = CurrentAirs.DbFormattedString & esYear
        Session("AirsYear") = airsYear

        If Not IsPostBack Then

            LoadState()
            LoadHorizontalCollectionCode()
            LoadHorizontalDatumReferenceCode()
            rngValXCoordinate.MinimumValue = CStr(GetSessionItem(Of Decimal)("LongMin"))
            rngValXCoordinate.MaximumValue = CStr(GetSessionItem(Of Decimal)("LongMax"))
            rngValYCoordinate.MinimumValue = CStr(GetSessionItem(Of Decimal)("LatMin"))
            rngValYCoordinate.MaximumValue = CStr(GetSessionItem(Of Decimal)("LatMax"))

            If CheckESExist(airsYear) AndAlso CheckESEntry(airsYear) Then
                LoadESSchema()
            Else
                LoadFacilityLocation()
                LoadContactInfo()
            End If

            pnlFacility.Visible = True
            pnlLatLongConvert.Visible = False

        End If

    End Sub

    ' Load Routines

    Private Sub LoadHorizontalCollectionCode()

        cboHorizontalCollectionCode.Items.Add(" --Select a Method-- ")

        Dim dt = GetHorizontalCollectionMethods()

        For Each dr As DataRow In dt.Rows
            Dim desc As String = dr.Item("strHorizCollectionMethodDesc").ToString
            Dim code As String = dr.Item("strHorizCollectionMethodCode").ToString
            cboHorizontalCollectionCode.Items.Add(desc & "  [" & code & "]")
        Next

    End Sub

    Private Sub LoadHorizontalDatumReferenceCode()

        cboHorizontalReferenceCode.Items.Add(" --Select a Code-- ")

        Dim dt = GetHorizontalDatumReferenceCodes()

        For Each dr As DataRow In dt.Rows
            Dim desc As String = dr.Item("strHorizontalReferenceDesc").ToString
            Dim code As String = dr.Item("strHorizontalReferenceDatum").ToString
            cboHorizontalReferenceCode.Items.Add(desc & "  [" & code & "]")
        Next

    End Sub

    Private Sub LoadState()

        cboContactState.Items.Add(" -- ")

        Dim dt = GetEsStates()

        For Each dr As DataRow In dt.Rows
            cboContactState.Items.Add(dr.Item("Abbreviation").ToString)
        Next

        cboContactState.SelectedIndex = 0

    End Sub

    ' Button Routines 

    Protected Sub cboYesNo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboYesNo.SelectedIndexChanged

        If cboYesNo.SelectedValue = "NO" Then
            pnlEmissions.Visible = True
            pEmissionsHelpYes.Visible = False
            pEmissionsHelpNo.Visible = True
        ElseIf cboYesNo.SelectedValue = "YES" Then
            txtVOC.Text = "0"
            txtNOx.Text = "0"
            pnlEmissions.Visible = False
            pEmissionsHelpYes.Visible = True
            pEmissionsHelpNo.Visible = False
        Else
            pnlEmissions.Visible = False
            pEmissionsHelpYes.Visible = True
            pEmissionsHelpNo.Visible = True
        End If

    End Sub

    Protected Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click

        If cboYesNo.SelectedValue = "YES" Then
            Session("ESOptOut") = "YES"
        Else
            Session("ESOptOut") = "NO"
        End If
        Response.Redirect("confirm.aspx")

    End Sub

    Protected Sub btnContinueToContact_Click(sender As Object, e As EventArgs) Handles btnContinueToContact.Click
        mltiViewESFacility.ActiveViewIndex = 1
    End Sub

    Protected Sub btnContinueToEmissions_Click(sender As Object, e As EventArgs) Handles btnContinueToEmissions.Click
        mltiViewESFacility.ActiveViewIndex = 2
    End Sub

    Protected Sub btnCancelLocation_Click(sender As Object, e As EventArgs) Handles btnCancelLocation.Click
        Response.Redirect("~/ES/")
    End Sub

    Protected Sub btnCancelContact_Click(sender As Object, e As EventArgs) Handles btnCancelContact.Click
        Response.Redirect("~/ES/")
    End Sub

    Protected Sub btnCancelEmission_Click(sender As Object, e As EventArgs) Handles btnCancelEmission.Click
        Response.Redirect("~/ES/")
    End Sub

    Protected Sub btnbackToLocation_Click(sender As Object, e As EventArgs) Handles btnbackToLocation.Click
        mltiViewESFacility.ActiveViewIndex = 0
    End Sub

    Protected Sub btnBackToContactInfo_Click(sender As Object, e As EventArgs) Handles btnBackToContactInfo.Click
        mltiViewESFacility.ActiveViewIndex = 1
    End Sub

    ' Load Facility & Contact Info

    Private Sub LoadESSchema()

        Dim FacilityZip As String
        Dim AirsYear As String = CurrentAirs.DbFormattedString & GetSessionItem(Of String)("ESYear")
        Dim ContactZip As String
        Dim YesNo As String
        Dim VOCAmt As Double
        Dim NOXAmt As Double
        Dim HCCcode As String
        Dim HCCdesc As String
        Dim HRCcode As String
        Dim HRCdesc As String

        Dim dr = GetFacilityEsSchema(AirsYear)

        If dr IsNot Nothing Then

            If Convert.IsDBNull(dr("strFacilityName")) Then
                txtFacilityName.Text = ""
            Else
                txtFacilityName.Text = dr.Item("strFacilityName").ToString
            End If
            If Convert.IsDBNull(dr("strFacilityAddress")) Then
                txtLocationAddress.Text = ""
            Else
                txtLocationAddress.Text = dr.Item("strFacilityAddress").ToString
            End If
            If Convert.IsDBNull(dr("strFacilityCity")) Then
                txtCity.Text = ""
            Else
                txtCity.Text = dr.Item("strFacilityCity").ToString
            End If

            txtState.Text = "GA"

            If Convert.IsDBNull(dr("strFacilityZip")) Then
                txtZipCode.Text = ""
            Else
                FacilityZip = dr("strFacilityZip").ToString
                FacilityZip = FacilityZip.Replace("-", "")
                If FacilityZip.Length() > 5 Then
                    txtZipCode.Text = Left(FacilityZip, 5) & "-" & FacilityZip.Substring(5, 4)
                Else
                    txtZipCode.Text = FacilityZip
                End If
            End If

            If Convert.IsDBNull(dr("strCounty")) Then
                txtCounty.Text = ""
            Else
                txtCounty.Text = dr.Item("strCounty").ToString
            End If

            If Convert.IsDBNull(dr("dblXCoordinate")) Then
                txtXCoordinate.Text = ""
            Else
                txtXCoordinate.Text = Round(Abs(CDbl(dr.Item("dblXCoordinate"))), 6).ToString
            End If
            If Convert.IsDBNull(dr("dblYCoordinate")) Then
                txtYCoordinate.Text = ""
            Else
                txtYCoordinate.Text = Round(Abs(CDbl(dr.Item("dblYCoordinate"))), 6).ToString
            End If
            If Convert.IsDBNull(dr("strHorizontalCollectionCode")) Then
                cboHorizontalCollectionCode.SelectedIndex = 0
            Else
                HCCcode = dr.Item("strHorizontalCollectionCode").ToString
                HCCdesc = GetHorizCollDesc(HCCcode)
                cboHorizontalCollectionCode.SelectedValue = HCCdesc & "  [" & HCCcode & "]"
            End If
            If Convert.IsDBNull(dr("strHorizontalReferenceCode")) Then
                cboHorizontalReferenceCode.SelectedIndex = 0
            Else
                HRCcode = dr.Item("strHorizontalReferenceCode").ToString
                HRCdesc = GetHorizRefDesc(HRCcode)
                cboHorizontalReferenceCode.SelectedValue = HRCdesc & "  [" & HRCcode & "]"
            End If
            If Convert.IsDBNull(dr("strHorizontalAccuracyMeasure")) Then
                txtHorizontalAccuracyMeasure.Text = ""
            Else
                txtHorizontalAccuracyMeasure.Text = dr.Item("strHorizontalAccuracyMeasure").ToString
            End If

            If Convert.IsDBNull(dr("strContactPrefix")) Then
                txtContactPrefix.Text = ""
            Else
                txtContactPrefix.Text = dr.Item("strContactPrefix").ToString
            End If
            If Convert.IsDBNull(dr("strContactTitle")) Then
                txtContactTitle.Text = ""
            Else
                txtContactTitle.Text = dr.Item("strContactTitle").ToString
            End If

            If Convert.IsDBNull(dr("strContactFirstName")) Then
                txtContactFirstName.Text = ""
            Else
                txtContactFirstName.Text = dr.Item("strContactFirstName").ToString
            End If

            If Convert.IsDBNull(dr("strContactLastName")) Then
                txtContactLastName.Text = ""
            Else
                txtContactLastName.Text = dr.Item("strContactLastName").ToString
            End If

            If Convert.IsDBNull(dr("strContactCompany")) Then
                txtContactCompanyName.Text = ""
            Else
                txtContactCompanyName.Text = dr.Item("strContactCompany").ToString
            End If
            If Not Convert.IsDBNull(dr("strContactPhoneNumber")) Then
                txtOfficePhoneNbr.Text = dr.Item("strContactPhoneNumber").ToString
            End If
            If Not Convert.IsDBNull(dr("strContactFaxNumber")) Then
                txtFaxNbr.Text = dr.Item("strContactFaxNumber").ToString
            End If
            If Convert.IsDBNull(dr("strContactEmail")) Then
                txtContactEmail.Text = ""
            Else
                txtContactEmail.Text = dr.Item("strContactEmail").ToString
            End If
            If Convert.IsDBNull(dr("strContactAddress1")) Then
                txtContactAddress1.Text = ""
            Else
                txtContactAddress1.Text = dr.Item("strContactAddress1").ToString
            End If
            If Convert.IsDBNull(dr("strContactCity")) Then
                txtContactCity.Text = ""
            Else
                txtContactCity.Text = dr.Item("strContactCity").ToString
            End If
            If Convert.IsDBNull(dr("strContactState")) Then
                cboContactState.SelectedIndex = 0
            Else
                cboContactState.SelectedValue = dr.Item("strContactState").ToString
            End If

            If Convert.IsDBNull(dr("strContactZip")) Then
                txtContactZipCode.Text = ""
            Else
                ContactZip = dr.Item("strContactZip").ToString
                ContactZip = ContactZip.Replace("-", "")
                If ContactZip.Length() > 5 Then
                    txtContactZipCode.Text = Left(dr.Item("strContactZip").ToString, 5)
                    txtContactZipPlus4.Text = dr.Item("strContactZip").ToString.Substring(5, 4)
                Else
                    txtContactZipCode.Text = dr.Item("strContactZip").ToString
                End If
            End If

            If Convert.IsDBNull(dr("strOptOut")) Then
                YesNo = ""
                cboYesNo.SelectedIndex = 0
                pnlEmissions.Visible = True
            Else
                YesNo = dr.Item("strOptOut").ToString
                cboYesNo.SelectedValue = YesNo.ToUpper
                If YesNo = "NO" Then
                    pnlEmissions.Visible = True
                Else
                    pnlEmissions.Visible = False
                End If
            End If

            If YesNo = "NO" Then
                If Convert.IsDBNull(dr("dblVOCEmission")) Then
                    txtVOC.Text = "0"
                Else
                    VOCAmt = CDbl(dr.Item("dblVOCEmission"))
                    If VOCAmt <= 0 Then
                        txtVOC.Text = "0"
                    Else
                        txtVOC.Text = Round(VOCAmt, 2).ToString
                    End If
                End If
                If Convert.IsDBNull(dr("dblNOXEmission")) Then
                    txtNOx.Text = "0"
                Else
                    NOXAmt = CDbl(dr.Item("dblNOXEmission"))
                    If NOXAmt < 0 Then
                        txtNOx.Text = "0"
                    Else
                        txtNOx.Text = Round(NOXAmt, 2).ToString
                    End If
                End If
                pEmissionsHelpYes.Visible = False
                pnlEmissions.Visible = True
            ElseIf YesNo = "YES" Then
                txtVOC.Text = "0"
                txtNOx.Text = "0"
                pEmissionsHelpNo.Visible = False
            ElseIf YesNo = "--" Then
                If Convert.IsDBNull(dr("dblVOCEmission")) Then
                    txtVOC.Text = "0"
                Else
                    VOCAmt = CDbl(dr.Item("dblVOCEmission"))
                    If VOCAmt <= 0 Then
                        txtVOC.Text = "0"
                    Else
                        txtVOC.Text = Round(VOCAmt, 2).ToString
                    End If
                End If
                If Convert.IsDBNull(dr("dblNOXEmission")) Then
                    txtNOx.Text = "0"
                Else
                    NOXAmt = CDbl(dr.Item("dblNOXEmission"))
                    If NOXAmt <= 0 Then
                        txtNOx.Text = "0"
                    Else
                        txtNOx.Text = Round(NOXAmt, 2).ToString
                    End If
                End If
                pnlEmissions.Visible = True
            End If
        End If

    End Sub

    Private Sub LoadFacilityLocation()

        'Load facility and contact info FROM  apbFacilityInformation table

        Dim FacilityZip As String
        Dim HCCcode As String
        Dim HCCdesc As String
        Dim HRCcode As String
        Dim HRCdesc As String

        Dim dr = GetFacilityLocation(CurrentAirs)

        If dr IsNot Nothing Then

            If Convert.IsDBNull(dr("strFacilityName")) Then
                txtFacilityName.Text = ""
            Else
                txtFacilityName.Text = dr.Item("strFacilityName").ToString
            End If
            If Convert.IsDBNull(dr("strFacilityStreet1")) Then
                txtLocationAddress.Text = ""
            Else
                txtLocationAddress.Text = dr.Item("strFacilityStreet1").ToString
            End If
            If Convert.IsDBNull(dr("strFacilityCity")) Then
                txtCity.Text = ""
            Else
                txtCity.Text = dr.Item("strFacilityCity").ToString
            End If

            txtState.Text = "GA"

            If Convert.IsDBNull(dr("strFacilityZipCode")) Then
                txtZipCode.Text = ""
            Else
                FacilityZip = dr("strFacilityZipCode").ToString
                FacilityZip = FacilityZip.Replace("-", "")
                If FacilityZip.Length() > 5 Then
                    txtZipCode.Text = Left(FacilityZip, 5) & "-" & FacilityZip.Substring(5, 4)
                Else
                    txtZipCode.Text = FacilityZip
                End If
            End If

            txtCounty.Text = GetCountyName(CurrentAirs.CountySubstring)

            If Convert.IsDBNull(dr("numFacilityLongitude")) Then
                txtXCoordinate.Text = ""
            Else
                txtXCoordinate.Text = Round(Abs(CDec(dr.Item("numFacilityLongitude"))), 6).ToString
            End If
            If Convert.IsDBNull(dr("numFacilityLatitude")) Then
                txtYCoordinate.Text = ""
            Else
                txtYCoordinate.Text = Round(CDec(dr.Item("numFacilityLatitude")), 6).ToString
            End If
            If Convert.IsDBNull(dr("strHorizontalCollectionCode")) Then
                cboHorizontalCollectionCode.SelectedIndex = 0
            Else
                HCCcode = dr.Item("strHorizontalCollectionCode").ToString
                HCCdesc = GetHorizCollDesc(HCCcode)
                cboHorizontalCollectionCode.SelectedValue = HCCdesc & "  [" & HCCcode & "]"
            End If
            If Convert.IsDBNull(dr("strHorizontalReferenceCode")) Then
                cboHorizontalReferenceCode.SelectedIndex = 0
            Else
                HRCcode = dr.Item("strHorizontalReferenceCode").ToString
                HRCdesc = GetHorizRefDesc(HRCcode)
                cboHorizontalReferenceCode.SelectedValue = HRCdesc & "  [" & HRCcode & "]"
            End If
            If Convert.IsDBNull(dr("strHorizontalAccuracyMeasure")) Then
                txtHorizontalAccuracyMeasure.Text = ""
            Else
                txtHorizontalAccuracyMeasure.Text = dr.Item("strHorizontalAccuracyMeasure").ToString
            End If

        End If

    End Sub

    Private Sub LoadContactInfo()
        Dim ContactZip As String

        Dim dr = GetEsContactInfo(CurrentAirs, Now.Year - 1)

        If dr IsNot Nothing Then

            If Convert.IsDBNull(dr("strContactPrefix")) Then
                txtContactPrefix.Text = ""
            Else
                txtContactPrefix.Text = dr.Item("strContactPrefix").ToString
            End If
            If Convert.IsDBNull(dr("strContactFirstName")) Then
                txtContactFirstName.Text = ""
            Else
                txtContactFirstName.Text = dr.Item("strContactFirstName").ToString
            End If
            If Convert.IsDBNull(dr("strContactLastName")) Then
                txtContactLastName.Text = ""
            Else
                txtContactLastName.Text = dr.Item("strContactLastName").ToString
            End If
            If Convert.IsDBNull(dr("strContactTitle")) Then
                txtContactTitle.Text = ""
            Else
                txtContactTitle.Text = dr.Item("strContactTitle").ToString
            End If
            If Convert.IsDBNull(dr("strContactCompanyName")) Then
                txtContactCompanyName.Text = ""
            Else
                txtContactCompanyName.Text = dr.Item("strContactCompanyName").ToString
            End If
            If Not Convert.IsDBNull(dr("strContactPhoneNumber1")) Then
                txtOfficePhoneNbr.Text = dr.Item("strContactPhoneNumber1").ToString
            End If
            If Not Convert.IsDBNull(dr("strContactFaxNumber")) Then
                txtFaxNbr.Text = dr.Item("strContactFaxNumber").ToString
            End If
            If Convert.IsDBNull(dr("strContactEmail")) Then
                txtContactEmail.Text = ""
            Else
                txtContactEmail.Text = dr.Item("strContactEmail").ToString
            End If
            If Convert.IsDBNull(dr("strContactAddress1")) Then
                txtContactAddress1.Text = ""
            Else
                txtContactAddress1.Text = dr.Item("strContactAddress1").ToString
            End If
            If Convert.IsDBNull(dr("strContactCity")) Then
                txtContactCity.Text = ""
            Else
                txtContactCity.Text = dr.Item("strContactCity").ToString
            End If
            If Convert.IsDBNull(dr("strContactState")) Then
                cboContactState.SelectedIndex = 0
            Else
                cboContactState.SelectedValue = dr.Item("strContactState").ToString
            End If

            If Convert.IsDBNull(dr("strContactZipCode")) Then
                txtContactZipCode.Text = ""
            Else
                ContactZip = dr.Item("strContactZipCode").ToString
                ContactZip = ContactZip.Replace("-", "")
                If ContactZip.Length() > 5 Then
                    txtContactZipCode.Text = Left(ContactZip, 5)
                    txtContactZipPlus4.Text = dr.Item("strContactZipCode").ToString.Substring(5, 4)
                Else
                    txtContactZipCode.Text = ContactZip
                End If
            End If
        End If
    End Sub

    ' Save Facility & Contact Info

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        lblVOCNOXZero.Visible = False

        If String.IsNullOrWhiteSpace(txtNOx.Text) OrElse Not IsNumeric(txtNOx.Text) OrElse
           CDec(txtNOx.Text) < 0 Then
            txtNOx.Text = "0"
        End If

        If String.IsNullOrWhiteSpace(txtVOC.Text) OrElse Not IsNumeric(txtVOC.Text) OrElse
           CDec(txtVOC.Text) < 0 Then
            txtVOC.Text = "0"
        End If

        If cboYesNo.SelectedValue = "NO" AndAlso CDec(txtNOx.Text) < 25 AndAlso CDec(txtVOC.Text) < 25 Then
            lblVOCNOXZero.Visible = True
        Else
            SaveES()
            SaveContactAPB()
            UpdateAPBFacilityInfo()
        End If

        If SavedES AndAlso SavedAPB Then
            If cboYesNo.SelectedValue = "YES" Then
                Session("ESOptOut") = "YES"
            Else
                Session("ESOptOut") = "NO"
            End If

            Response.Redirect("confirm.aspx")
        End If

    End Sub

    Private Sub SaveES()

        Dim AirsYear As String = GetSessionItem(Of String)("AirsYear")
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
        Dim UserID As Integer = GetCurrentUser().UserId
        Dim day As String = Now.ToString("d-MMM-yyyy")
        Dim hr As String = Now.Hour.ToString
        Dim min As String = Now.Minute.ToString
        Dim sec As String = Now.Second.ToString
        If hr.Length() < 2 Then hr = "0" & hr
        If min.Length() < 2 Then min = "0" & min
        If sec.Length() < 2 Then sec = "0" & sec
        Dim TimeLastLogin As String = hr & ":" & min & ":" & sec
        Dim DateLastLogin As String = day.ToUpper
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

        ConfNum = CurrentAirs.ShortString & DateLastLogin.Replace("-", "") & TimeLastLogin.Replace(":", "")

        SavedES = False

        Dim FirstConfirm As Boolean = CheckFirstConfirm(AirsYear)

        LocationAddress = txtLocationAddress.Text
        City = txtCity.Text
        State = txtState.Text
        ZipCode = txtZipCode.Text.Replace("-", "")
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
        HorizontalCollectionCode = HCD.Substring(HCD.Length() - 4, 3)

        HRD = cboHorizontalReferenceCode.SelectedValue
        HorizontalReferenceCode = HRD.Substring(HRD.Length() - 4, 3)

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

        If CheckESExist(AirsYear) Then
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

        Dim ContactKey As String = CurrentAirs.DbFormattedString & "42"
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
            New SqlParameter("@AirsNumber", CurrentAirs.DbFormattedString),
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

    ' Check Routines

    Private Function ContactExistAPB() As Boolean

        Dim key As String = CurrentAirs.DbFormattedString & "42"

        Dim query = "Select strContactKey FROM apbContactInformation Where strContactKey = @key "
        Dim param As New SqlParameter("@key", key)

        Return DB.ValueExists(query, param)

    End Function

    Protected Sub btnConvert_Click(sender As Object, e As EventArgs) Handles btnConvert.Click
        txtLongDec.Text = Abs(GetDecDegree(txtLonDeg.Text, txtLonMin.Text, txtLonSec.Text)).ToString
        txtLatDec.Text = GetDecDegree(txtLatDeg.Text, txtLatMin.Text, txtLatSec.Text).ToString
    End Sub

    Protected Sub btnCancelLatLong_Click(sender As Object, e As EventArgs) Handles btnCancelLatLong.Click

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

    Protected Sub btnUseLatLong_Click(sender As Object, e As EventArgs) Handles btnUseLatLong.Click

        lblDecLatLongEmpty.Text = ""

        If txtLongDec.Text = "" OrElse txtLatDec.Text = "" Then
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

    Protected Sub btnLatLongConvert_Click(sender As Object, e As EventArgs) Handles btnLatLongConvert.Click

        rngValLongDec.MinimumValue = CStr(GetSessionItem(Of Decimal)("LongMin"))
        rngValLongDec.MaximumValue = CStr(GetSessionItem(Of Decimal)("LongMax"))
        rngValLatDec.MinimumValue = CStr(GetSessionItem(Of Decimal)("LatMin"))
        rngValLatDec.MaximumValue = CStr(GetSessionItem(Of Decimal)("LatMax"))
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

        HCC = cboHorizontalCollectionCode.SelectedValue
        HorizontalCollectionCode = HCC.Substring(HCC.IndexOf("[") + 1, 3)
        HorizontalAccuracyMeasure = txtHorizontalAccuracyMeasure.Text
        HRC = cboHorizontalReferenceCode.SelectedValue
        HorizontalReferenceCode = HRC.Substring(HRC.IndexOf("[") + 0, 3)
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
            New SqlParameter("@AirsNumber", CurrentAirs.DbFormattedString)
        }

        DB.RunCommand(query, params)

    End Sub

End Class