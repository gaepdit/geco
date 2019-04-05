Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.MapHelper
Imports Reimers.Core.Maps
Imports Reimers.Google.Map

Partial Class EIS_rp_entry
    Inherits Page

    Private NAICSExists As Boolean

#Region " Load Routines "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim OptOut As String = GetCookie(EisCookie.OptOut)

        EIEntryAccessCheck(EISAccessCode, OptOut)

        If Not IsPostBack Then
            LoadHorCollectDDL()
            LoadHorRefDatumDDL()
            LoadFacilityStateDDL()
            LoadContactStateDDL()
            LoadFacilityGCIValidation()
            LoadFacilityDetails()
            LoadPhoneNumbers()
            PopulateNAICSGridView()

            pnlFacilityEdit.Visible = True
            pnlNAICSCodeLookup.Visible = False

            txtMapLat.Attributes.Add("readonly", "readonly")
            txtMapLon.Attributes.Add("readonly", "readonly")
        End If

        HideFacilityInventoryMenu()
        HideEmissionInventoryMenu()
        ShowEISHelpMenu()
        HideTextBoxBorders(Me)
    End Sub

    Private Sub LoadHorCollectDDL()
        ddlHorCollectionMetCode.Items.Add("--Select Horizontal Collection Method--")
        Try
            Dim query = "select strDesc, HorCollMetCode FROM EISLK_HORCOLLMETCODE where Active = '1' order by strDesc "

            Dim dt As DataTable = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("HorCollMetCode")
                    }
                    ddlHorCollectionMetCode.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadHorRefDatumDDL()
        ddlHorReferenceDatCode.Items.Add("--Select Horizontal Reference Datum--")
        Try
            Dim query = " select strDesc, HorRefDatumCode FROM EISLK_HORREFDATUMCODE where ACTIVE = '1' order by strDesc "

            Dim dt As DataTable = DB.GetDataTable(query)

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows
                    Dim newListItem As New ListItem With {
                        .Text = dr.Item("strdesc"),
                        .Value = dr.Item("HorRefDatumCode")
                    }
                    ddlHorReferenceDatCode.Items.Add(newListItem)
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadFacilityStateDDL()
        ddlFacility_StateMail.Items.Add("--Select a State--")

        Try
            Dim query As String = "Select strState, strAbbrev FROM LookUpStates order by strAbbrev"

            Dim dt As DataTable = DB.GetDataTable(query)

            For Each dr As DataRow In dt.Rows
                Dim newListItem As New ListItem With {
                    .Text = dr.Item("strState").ToString,
                    .Value = dr.Item("strAbbrev").ToString
                }
                ddlFacility_StateMail.Items.Add(newListItem)
            Next
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadContactStateDDL()
        ddlContact_MailState.Items.Add("--Select a State--")

        Try
            Dim query As String = "Select strState, strAbbrev FROM LookUpStates order by strAbbrev"

            Dim dt As DataTable = DB.GetDataTable(query)

            For Each dr As DataRow In dt.Rows
                Dim newListItem As New ListItem With {
                    .Text = dr.Item("strState").ToString,
                    .Value = dr.Item("strAbbrev").ToString
                }
                ddlContact_MailState.Items.Add(newListItem)
            Next
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadFacilityGCIValidation()
        Dim CountyCode As String = GetCookie(Cookie.AirsNumber).Substring(0, 3)
        Dim mm As MinMaxLatLon = GetCountyLatLong(CountyCode)

        rngvLatitudeMeasure.MaximumValue = mm.MaxLat
        rngvLatitudeMeasure.MinimumValue = mm.MinLat
        rngvLatitudeMeasure.ErrorMessage = "The Latitude must be between " & mm.MinLat.ToString & " and " & mm.MaxLat.ToString & "."
        rngvLatitudeMeasure.Text = "Must be between " & mm.MinLat.ToString & " and " & mm.MaxLat.ToString

        rngvLongitudeMeasure.MaximumValue = mm.MaxLon
        rngvLongitudeMeasure.MinimumValue = mm.MinLon
        rngvLongitudeMeasure.ErrorMessage = "The Latitude must be between " & mm.MinLon.ToString & " and " & mm.MaxLon.ToString & "."
        rngvLongitudeMeasure.Text = "Must be between " & mm.MinLon.ToString & " and " & mm.MaxLon.ToString
    End Sub

    Private Sub LoadFacilityDetails()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        TxtLocationAddressStateCode.Text = "GA"

        Try
            Dim query As String = "SELECT " &
                "     strFacilitySiteName, " &
                "     strMailingAddressText, " &
                "     strSupplementalAddressText, " &
                "     strMailingAddressCityName, " &
                "     strMailingAddressStateCode, " &
                "     strMailingAddressPostalCode, " &
                "     strLocationAddressText, " &
                "     strSupplementalLocationText, " &
                "     strLocalityName, " &
                "     strLocationAddressPostalCode, " &
                "     strAddressComment, " &
                "     strFacilitySiteDescription, " &
                "     strNAICSCode, " &
                "     strFacilitySiteComment, " &
                "     numLatitudeMeasure, " &
                "     numLongitudeMeasure, " &
                "     HorCollMetCode, " &
                "     intHorAccuracyMeasure, " &
                "     HorRefDatumCode, " &
                "     strGeographicComment, " &
                "     strNamePrefixText, " &
                "     strFirstName, " &
                "     strLastName, " &
                "     strIndividualTitleText, " &
                "     strFSAIMAddressText, " &
                "     strFSAISAddressText, " &
                "     strFSAIMAddressCityName, " &
                "     strFSAIMAddressStateCode, " &
                "     strFSAIMAddressPostalCode, " &
                "     StrElectronicAddressText, " &
                "     UpdateUser_mailingAddress, " &
                "     strFSAIAddressComment " &
                " FROM VW_EIS_facility f " &
                " WHERE FACILITYSITEID = @FacilitySiteID "

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then

                'Facility Name and Location
                txtFacilitySiteName.Text = GetNullableString(dr.Item("strFacilitySiteName"))
                TxtLocationAddressText.Text = GetNullableString(dr.Item("strLocationAddressText"))
                TxtSupplementalLocationText2.Text = GetNullableString(dr.Item("strSupplementalLocationText"))
                TxtLocalityName.Text = GetNullableString(dr.Item("strLocalityName"))
                TxtLocationAddressPostalCode.Text = GetNullableString(dr.Item("strLocationAddressPostalCode"))
                txtMailingAddressText.Text = GetNullableString(dr.Item("strMailingAddressText"))
                TxtSupplementalAddressText.Text = GetNullableString(dr.Item("strSupplementalAddressText"))
                txtMailingAddressCityName.Text = GetNullableString(dr.Item("strMailingAddressCityName"))

                If IsDBNull(dr("strMailingAddressStateCode")) Then
                    ddlFacility_StateMail.SelectedValue = ""
                Else
                    ddlFacility_StateMail.SelectedValue = dr.Item("strMailingAddressStateCode")
                End If

                txtMailingAddressPostalCode.Text = GetNullableString(dr.Item("strMailingAddressPostalCode"))
                TxtMailingAddressComment.Text = GetNullableString(dr.Item("strAddressComment"))
                TxtFacilitySiteDescription.Text = GetNullableString(dr.Item("strFacilitySiteDescription"))
                txtNAICSCode.Text = GetNullableString(dr.Item("strNAICSCode"))
                txtFacilitySiteComment.Text = GetNullableString(dr.Item("strFacilitySiteComment"))
                TxtLatitudeMeasure.Text = GetNullableString(dr.Item("numLatitudeMeasure"))
                TxtLongitudeMeasure.Text = GetNullableString(dr.Item("numLongitudeMeasure"))

                If TxtLatitudeMeasure.Text <> "" And TxtLongitudeMeasure.Text <> "" Then
                    imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(TxtLatitudeMeasure.Text, TxtLongitudeMeasure.Text))
                    lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(New Coordinate(TxtLatitudeMeasure.Text, TxtLongitudeMeasure.Text))
                Else
                    imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(TxtLocationAddressText.Text, TxtLocalityName.Text)
                    lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(TxtLocationAddressText.Text, TxtLocalityName.Text)
                End If

                If IsDBNull(dr("HorCollMetCode")) Then
                    ddlHorCollectionMetCode.SelectedValue = ""
                Else
                    ddlHorCollectionMetCode.SelectedValue = dr.Item("HorCollMetCode")
                End If

                TxtHorizontalAccuracyMeasure.Text = GetNullableString(dr("intHorAccuracyMeasure"))

                If TxtHorizontalAccuracyMeasure.Text = "-1" Then
                    TxtHorizontalAccuracyMeasure.Text = ""
                End If

                If IsDBNull(dr("HorRefDatumCode")) Then
                    ddlHorReferenceDatCode.SelectedValue = ""
                Else
                    ddlHorReferenceDatCode.SelectedValue = dr.Item("HorRefDatumCode")
                End If

                txtGeographicComment.Text = GetNullableString(dr.Item("strGeographicComment"))

                'Facility Inventory Contact information
                If IsDBNull(dr("strNamePrefixText")) Then
                    txtPrefix.Text = ""
                Else
                    txtPrefix.Text = dr.Item("strNamePrefixText")
                End If

                txtFirstName.Text = GetNullableString(dr.Item("strFirstName"))
                txtLastName.Text = GetNullableString(dr.Item("strLastName"))
                txtIndividualTitleText.Text = GetNullableString(dr.Item("STRINDIVIDUALTITLETEXT"))
                txtMailingAddressText_Contact.Text = GetNullableString(dr.Item("STRFSAIMADDRESSTEXT"))
                txtSupplementalAddressText_Contact.Text = GetNullableString(dr.Item("STRFSAISADDRESSTEXT"))
                txtMailingAddressCityName_Contact.Text = GetNullableString(dr.Item("STRFSAIMADDRESSCITYNAME"))

                If IsDBNull(dr("STRFSAIMADDRESSSTATECODE")) Then
                    ddlContact_MailState.SelectedValue = ""
                Else
                    ddlContact_MailState.SelectedValue = dr.Item("STRFSAIMADDRESSSTATECODE")
                End If

                txtMailingAddressPostalCode_Contact.Text = GetNullableString(dr.Item("STRFSAIMADDRESSPOSTALCODE"))
                txtElectronicAddressText.Text = GetNullableString(dr.Item("StrElectronicAddressText"))
                txtAddressComment_Contact.Text = GetNullableString(dr.Item("strFSAIAddressComment"))
            End If

            ' Store previous Geographic Data for comparison on submit
            hidLatitude.Value = TxtLatitudeMeasure.Text
            hidLongitude.Value = TxtLongitudeMeasure.Text
            hidHorCollectionMetCode.Value = ddlHorCollectionMetCode.SelectedValue
            hidHorCollectionMetDesc.Value = ddlHorCollectionMetCode.SelectedItem.Text
            hidHorizontalAccuracyMeasure.Value = TxtHorizontalAccuracyMeasure.Text
            hidHorReferenceDatCode.Value = ddlHorReferenceDatCode.SelectedValue
            hidHorReferenceDatDesc.Value = ddlHorReferenceDatCode.SelectedItem.Text
            hidGeographicComment.Value = txtGeographicComment.Text

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadPhoneNumbers()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Try
            Dim query As String = "select TELEPHONENUMBERTYPECODE, " &
                " STRTELEPHONENUMBERTEXT, " &
                " STRTELEPHONENUMBEREXT " &
                " FROM EIS_TELEPHONECOMM " &
                " where FACILITYSITEID = @FacilitySiteID "

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dt As DataTable = DB.GetDataTable(query, param)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    Select Case dr.Item("TELEPHONENUMBERTYPECODE")
                        Case "W"
                            txtTelephoneNumberText.Text = GetNullableString(dr.Item("STRTELEPHONENUMBERTEXT"))
                            txtTelephoneExtensionNumberText.Text = GetNullableString(dr.Item("STRTELEPHONENUMBEREXT"))
                        Case "F"
                            txtTelephoneNumber_Fax.Text = GetNullableString(dr.Item("STRTELEPHONENUMBERTEXT"))
                        Case "M"
                            txtTelephoneNumber_Mobile.Text = GetNullableString(dr.Item("STRTELEPHONENUMBERTEXT"))
                    End Select
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

#Region " NAICS "

    Private Sub PopulateNAICSGridView()
        If Cache("NAICSDataTable") Is Nothing Then
            Dim NAICSDataTable As DataTable = GetNaicsDataTable()
            Cache.Insert("NAICSDataTable", NAICSDataTable, Nothing, DateTime.Now.AddHours(24), Cache.NoSlidingExpiration)
            Session("MyNAICSView") = NAICSDataTable
        Else
            Session("MyNAICSView") = DirectCast(Cache("NAICSDataTable"), DataTable)
        End If

        If Session("MyNAICSView").Rows.Count > 0 Then
            gvwNAICS.DataSource = Session("MyNAICSView")
            gvwNAICS.DataBind()
            lblRowCount.Text = "No. of NAICS Codes: " & Session("MyNAICSView").Rows.Count
        Else
            gvwNAICS.DataSource = Nothing
            lblRowCount.Text = ""
        End If
    End Sub


    Protected Sub btnUseNAICSCode_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUseNAICSCode.Click

        txtNAICSCode.Text = txtSelectedNAICSCode.Text
        txtLookupNAICSCode.Text = ""
        txtLookupNAICSDesc.Text = ""
        txtSelectedNAICSCode.Text = ""
        pnlFacilityEdit.Visible = True
        pnlNAICSCodeLookup.Visible = False
        txtNAICSCode.Focus()

    End Sub

    Protected Sub btnNAICSLoopup_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNAICSLoopup.Click

        MaintainScrollPositionOnPostBack = False
        pnlFacilityEdit.Visible = False
        pnlNAICSCodeLookup.Visible = True

    End Sub

    Protected Sub btnSearchNAICS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchNAICS.Click

        Dim dview As New DataView
        Dim NAICSDataTable As New DataTable("NAICS")
        NAICSDataTable = Cache("NAICSDataTable")
        dview.Table = NAICSDataTable

        'No search text in any field
        If txtLookupNAICSCode.Text = "" And txtLookupNAICSDesc.Text = "" Then
            dview.RowFilter = ""
        End If

        'Search text only in NAICS Code
        If txtLookupNAICSCode.Text <> "" And txtLookupNAICSDesc.Text = "" Then
            dview.RowFilter = "NAICSCode LIKE '%" & txtLookupNAICSCode.Text & "%'"
        End If

        'Search text only in NAICS Description
        If txtLookupNAICSCode.Text = "" And txtLookupNAICSDesc.Text <> "" Then
            dview.RowFilter = "strDesc LIKE '%" & txtLookupNAICSDesc.Text & "%'"
        End If

        'Search text in both fields
        If txtLookupNAICSCode.Text <> "" And txtLookupNAICSDesc.Text <> "" Then
            dview.RowFilter = "NAICSCode LIKE '%" & txtLookupNAICSCode.Text & "%' or strDesc LIKE '%" & txtLookupNAICSDesc.Text & "%'"
        End If

        gvwNAICS.DataSource = dview
        Session("MyNAICSView") = dview.ToTable

        gvwNAICS.DataBind()
        lblRowCount.Text = "No. of NAICS Codes: " & Session("MyNAICSView").Rows.Count

    End Sub

    Protected Sub NAICSCheck(ByVal Sender As Object, ByVal args As ServerValidateEventArgs)
        Try
            args.IsValid = False
            NAICSExists = False

            If DoesNaicsCodeExist(args.Value) Then
                args.IsValid = True
                NAICSExists = True
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnCancelNAICS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelNAICS.Click

        txtLookupNAICSCode.Text = ""
        txtLookupNAICSDesc.Text = ""
        txtSelectedNAICSCode.Text = ""
        pnlFacilityEdit.Visible = True
        pnlNAICSCodeLookup.Visible = False

    End Sub

    Protected Sub gvwNAICS_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvwNAICS.PageIndexChanging

        gvwNAICS.DataSource = SortDataTable(Session("MyNAICSView"), True)
        gvwNAICS.PageIndex = e.NewPageIndex
        gvwNAICS.DataBind()
        lblRowCount.Text = "No. of NAICS Codes: " & Session("MyNAICSView").Rows.Count

    End Sub

    Protected Sub gvwNAICS_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvwNAICS.SelectedIndexChanged

        Dim NAICSCode As String = gvwNAICS.SelectedValue

        txtSelectedNAICSCode.Text = NAICSCode

    End Sub

#End Region

#Region " Sorting "

    Private Property GridViewSortDirection() As String

        Get
            Return IIf(ViewState("SortDirection") = Nothing, "ASC", ViewState("SortDirection"))
        End Get
        Set(ByVal value As String)
            ViewState("SortDirection") = value
        End Set

    End Property

    Private Property GridViewSortExpression() As String

        Get
            Return IIf(ViewState("SortExpression") = Nothing, String.Empty, ViewState("SortExpression"))
        End Get
        Set(ByVal value As String)
            ViewState("SortExpression") = value
        End Set

    End Property

    Private Function GetSortDirection() As String

        Select Case GridViewSortDirection
            Case "ASC"
                GridViewSortDirection = "DESC"
            Case "DESC"
                GridViewSortDirection = "ASC"
        End Select

        Return GridViewSortDirection

    End Function

    Protected Function SortDataTable(ByVal pdataTable As DataTable, ByVal isPageIndexChanging As Boolean) As DataView

        If Not pdataTable Is Nothing Then
            Dim pdataView As New DataView(pdataTable)
            If GridViewSortExpression <> String.Empty Then
                If isPageIndexChanging Then
                    pdataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection)
                Else
                    pdataView.Sort = String.Format("{0} {1}", GridViewSortExpression, GetSortDirection())
                End If
            End If
            Return pdataView
        Else
            Return New DataView()
        End If

    End Function

#End Region

#Region " Menu Routines "

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

#Region " Save Routines "

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If NAICSExists = False Then
            'Do nothing - enables display of custom validator message
        Else
            SaveFacilityMailingAddress()
            SaveFacilitySiteinfo()
            SaveFacilityGCInfo()
            SaveFacilityContact()
            SaveContactPhoneNumber()
            SaveAPBContactInformation()

            'Send to determine if facility operated in EIYear
            Response.Redirect("rp_facilitystatus.aspx")
        End If
    End Sub

    Private Sub SaveFacilitySiteinfo()
        Dim facilityDescription As String = TxtFacilitySiteDescription.Text
        Dim facilityComment As String = txtFacilitySiteComment.Text
        Dim NAICScode As String = txtNAICSCode.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        'Truncate comment if >400 chars
        facilityComment = Left(facilityComment, 400)

        Try
            Dim query As String = "Update EIS_FACILITYSITE " &
               "Set STRFACILITYSITEDESCRIPTION = @facilityDescription, " &
               "STRNAICSCODE = @NAICScode, " &
               "STRFACILITYSITECOMMENT = @facilityComment, " &
               "UPDATEUSER = @UpdateUser, " &
               "UpdateDateTime = getdate() " &
               "where FACILITYSITEID = @FacilitySiteID "

            Dim params As SqlParameter() = {
                New SqlParameter("@facilityDescription", facilityDescription),
                New SqlParameter("@NAICScode", NAICScode),
                New SqlParameter("@facilityComment", facilityComment),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub SaveFacilityMailingAddress()
        Dim FacilityMailingAddress As String = txtMailingAddressText.Text
        Dim FacilityMailingAddress2 As String = lblSupplementalAddressText.Text
        Dim FacilityMailingAddressCity As String = txtMailingAddressCityName.Text
        Dim FacilityMailingAddressState As String = ddlFacility_StateMail.SelectedValue
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim FacilityMailingAddressZip As String = txtMailingAddressPostalCode.Text
        Dim FacilityMailingAddressComment = TxtMailingAddressComment.Text

        'Truncate comment if >400 chars
        FacilityMailingAddressComment = Left(FacilityMailingAddressComment, 400)

        Try
            Dim query As String = "Update EIS_FACILITYSITEADDRESS " &
                " Set STRMAILINGADDRESSTEXT = @FacilityMailingAddress, " &
                " STRSUPPLEMENTALADDRESSTEXT = @FacilityMailingAddress2, " &
                " STRMAILINGADDRESSCITYNAME = @FacilityMailingAddressCity, " &
                " STRMAILINGADDRESSSTATECODE = @FacilityMailingAddressState, " &
                " STRMAILINGADDRESSPOSTALCODE = @FacilityMailingAddressZip, " &
                " STRADDRESSCOMMENT = @FacilityMailingAddressComment, " &
                " UPDATEUSER = @UpdateUser, " &
                " UpdateDateTime = getdate() " &
                " where EIS_FACILITYSITEADDRESS.FACILITYSITEID = @FacilitySiteID "

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilityMailingAddress", FacilityMailingAddress),
                New SqlParameter("@FacilityMailingAddress2", FacilityMailingAddress2),
                New SqlParameter("@FacilityMailingAddressCity", FacilityMailingAddressCity),
                New SqlParameter("@FacilityMailingAddressState", FacilityMailingAddressState),
                New SqlParameter("@FacilityMailingAddressZip", FacilityMailingAddressZip),
                New SqlParameter("@FacilityMailingAddressComment", FacilityMailingAddressComment),
                New SqlParameter("@UpdateUser", UpdateUser),
                New SqlParameter("@FacilitySiteID", FacilitySiteID)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub SaveFacilityGCInfo()
        Try
            Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
            Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
            Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
            Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

            Dim curGoogleMapLink As String = "none"
            If Decimal.TryParse(hidLatitude.Value, Nothing) AndAlso Decimal.TryParse(hidLongitude.Value, Nothing) Then
                curGoogleMapLink = GoogleMaps.GetMapLinkUrl(New Coordinate(hidLatitude.Value, hidLongitude.Value))
            End If

            Dim gcUpdated As Boolean =
                hidLatitude.Value <> TxtLatitudeMeasure.Text OrElse
                hidLongitude.Value <> TxtLongitudeMeasure.Text OrElse
                hidHorCollectionMetCode.Value <> ddlHorCollectionMetCode.SelectedValue OrElse
                hidHorizontalAccuracyMeasure.Value <> TxtHorizontalAccuracyMeasure.Text OrElse
                hidHorReferenceDatCode.Value <> ddlHorReferenceDatCode.SelectedValue

            If gcUpdated Then

                ' Email APB if any Geographic Coordinate Information changed

                Dim newGoogleMapLink As String = "none"
                If Decimal.TryParse(TxtLatitudeMeasure.Text, Nothing) AndAlso Decimal.TryParse(TxtLongitudeMeasure.Text, Nothing) Then
                    newGoogleMapLink = GoogleMaps.GetMapLinkUrl(New Coordinate(TxtLatitudeMeasure.Text, TxtLongitudeMeasure.Text))
                End If

                Dim plainBody As String = "An update has been submitted for the EIS Facility Geographic Coordinate Information " &
                    "for the following facility. The updated information has NOT been saved in the database. If approved, you must " &
                    "manually update the facility using the IAIP EIS Tool." & vbNewLine &
                    vbNewLine &
                    "Facility Site ID: " & FacilitySiteID & vbNewLine &
                    vbNewLine &
                    "Update User: " & UpdateUserName & " (" & UpdateUserID & ")" & vbNewLine &
                    vbNewLine &
                    "Current Geographic Coordinate Information: " & vbNewLine &
                    vbNewLine &
                    "    Latitude: " & hidLatitude.Value & vbNewLine &
                    "    Longitude: " & hidLongitude.Value & vbNewLine &
                    "    Horizontal Collection Method: " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & vbNewLine &
                    "    Accuracy Measure: " & hidHorizontalAccuracyMeasure.Value & vbNewLine &
                    "    Horizontal Reference Datum: " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & vbNewLine &
                    "    Google Map: " & curGoogleMapLink & vbNewLine &
                    vbNewLine &
                    "Updated Geographic Coordinate Information submitted by user: " & vbNewLine &
                    vbNewLine &
                    "    Latitude: " & TxtLatitudeMeasure.Text & vbNewLine &
                    "    Longitude: " & TxtLongitudeMeasure.Text & vbNewLine &
                    "    Horizontal Collection Method: " & ddlHorCollectionMetCode.SelectedValue & " - " & ddlHorCollectionMetCode.SelectedItem.Text & vbNewLine &
                    "    Accuracy Measure: " & TxtHorizontalAccuracyMeasure.Text & vbNewLine &
                    "    Horizontal Reference Datum: " & ddlHorReferenceDatCode.SelectedValue & " - " & ddlHorReferenceDatCode.SelectedItem.Text & vbNewLine &
                    "    Google Map: " & newGoogleMapLink & vbNewLine &
                    vbNewLine &
                    "Comment submitted by user: " & vbNewLine &
                    vbNewLine &
                    txtGeographicComment.Text & vbNewLine

                Dim htmlBody As String = "<p>An update has been submitted for the EIS Facility Geographic Coordinate Information " &
                    "for the following facility. The updated information has <em>NOT</em> been saved in the database. If approved, you must " &
                    "manually update the facility using the IAIP EIS Tool.</p>" &
                    "<p><b>Facility Site ID:</b> " & FacilitySiteID & "</p>" &
                    "<p><b>Update User:</b> " & UpdateUserName & " (" & UpdateUserID & ")" & "</p>" &
                    "<p><b>Current Geographic Coordinate Information:</b> " & "</p>" &
                    "<ul>" &
                    "<li><b>Latitude:</b> " & hidLatitude.Value & "</li>" &
                    "<li><b>Longitude:</b> " & hidLongitude.Value & "</li>" &
                    "<li><b>Horizontal Collection Method:</b> " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & "</li>" &
                    "<li><b>Accuracy Measure:</b> " & hidHorizontalAccuracyMeasure.Value & "</li>" &
                    "<li><b>Horizontal Reference Datum:</b> " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & "</li>" &
                    "<li><b>Google Map:</b> " & curGoogleMapLink & "</li>" &
                    "</ul>" &
                    "<p><b>Updated Geographic Coordinate Information submitted by user:</b> " & "</p>" &
                    "<ul>" &
                    "<li><b>Latitude:</b> " & TxtLatitudeMeasure.Text & "</li>" &
                    "<li><b>Longitude:</b> " & TxtLongitudeMeasure.Text & "</li>" &
                    "<li><b>Horizontal Collection Method:</b> " & ddlHorCollectionMetCode.SelectedValue & " - " & ddlHorCollectionMetCode.SelectedItem.Text & "</li>" &
                    "<li><b>Accuracy Measure:</b> " & TxtHorizontalAccuracyMeasure.Text & "</li>" &
                    "<li><b>Horizontal Reference Datum:</b> " & ddlHorReferenceDatCode.SelectedValue & " - " & ddlHorReferenceDatCode.SelectedItem.Text & "</li>" &
                    "<li><b>Google Map:</b> " & newGoogleMapLink & "</li>" &
                    "</ul>" &
                    "<p><b>Comment submitted by user:</b> " & "</p>" &
                    "<blockquote><pre>" & txtGeographicComment.Text & "</pre></blockquote>"

                SendEmail(GecoContactEmail, "GECO Emission Inventory - Facility Geographic Info Update Request", plainBody, htmlBody,
                          caller:="EIS_rp_entry.SaveFacilityGCInfo (1)")

            ElseIf hidGeographicComment.Value <> txtGeographicComment.Text Then

                ' Send a different email to APB if only the Geographic Coordinate Information comment was changed

                Dim plainBody As String = "A comment has been submitted for the EIS Facility Geographic Coordinate Information " &
                    "for the following facility. No other geographic information was changed." & vbNewLine &
                    vbNewLine &
                    "Facility Site ID: " & FacilitySiteID & vbNewLine &
                    vbNewLine &
                    "Update User: " & UpdateUserName & " (" & UpdateUserID & ")" & vbNewLine &
                    vbNewLine &
                    "Current Geographic Coordinate Information: " & vbNewLine &
                    vbNewLine &
                    "    Latitude: " & hidLatitude.Value & vbNewLine &
                    "    Longitude: " & hidLongitude.Value & vbNewLine &
                    "    Horizontal Collection Method: " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & vbNewLine &
                    "    Accuracy Measure: " & hidHorizontalAccuracyMeasure.Value & vbNewLine &
                    "    Horizontal Reference Datum: " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & vbNewLine &
                    "    Google Map: " & curGoogleMapLink & vbNewLine &
                    vbNewLine &
                    "Comment submitted by user: " & vbNewLine &
                    vbNewLine &
                    txtGeographicComment.Text & vbNewLine

                Dim htmlBody As String = "<p>A comment has been submitted for the EIS Facility Geographic Coordinate Information " &
                    "for the following facility. No other geographic information was changed.</p>" &
                    "<p><b>Facility Site ID:</b> " & FacilitySiteID & "</p>" &
                    "<p><b>Update User:</b> " & UpdateUserName & " (" & UpdateUserID & ")" & "</p>" &
                    "<p><b>Current Geographic Coordinate Information:</b> " & "</p>" &
                    "<ul>" &
                    "<li><b>Latitude:</b> " & hidLatitude.Value & "</li>" &
                    "<li><b>Longitude:</b> " & hidLongitude.Value & "</li>" &
                    "<li><b>Horizontal Collection Method:</b> " & hidHorCollectionMetCode.Value & " - " & hidHorCollectionMetDesc.Value & "</li>" &
                    "<li><b>Accuracy Measure:</b> " & hidHorizontalAccuracyMeasure.Value & "</li>" &
                    "<li><b>Horizontal Reference Datum:</b> " & hidHorReferenceDatCode.Value & " - " & hidHorReferenceDatDesc.Value & "</li>" &
                    "<li><b>Google Map:</b> " & curGoogleMapLink & "</li>" &
                    "</ul>" &
                    "<p><b>Comment submitted by user:</b> " & "</p>" &
                    "<blockquote><pre>" & txtGeographicComment.Text & "</pre></blockquote>"

                SendEmail(GecoContactEmail, "GECO Emission Inventory - Facility Geographic Info Update Request", plainBody, htmlBody,
                          caller:="EIS_rp_entry.SaveFacilityGCInfo (2)")
            End If

            ' Update database if comment was changed
            If hidGeographicComment.Value <> txtGeographicComment.Text Then
                Dim query = "Update EIS_FACILITYGEOCOORD " &
                    " Set STRGEOGRAPHICCOMMENT = @GeographicComment, " &
                    " UPDATEUSER = @UpdateUser, " &
                    " UpdateDateTime = getdate() " &
                    " where FACILITYSITEID = @FacilitySiteID "

                Dim params = {
                    New SqlParameter("@GeographicComment", Left(txtGeographicComment.Text, 200)),
                    New SqlParameter("@UpdateUser", UpdateUser),
                    New SqlParameter("@FacilitySiteID", FacilitySiteID)
                }

                DB.RunCommand(query, params)
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    'Saves Contact information to the EIS tables
    Private Sub SaveFacilityContact()

        Dim ContactPrefix As String = txtPrefix.Text
        Dim ContactFirstName As String = txtFirstName.Text
        Dim ContactLastName As String = txtLastName.Text
        Dim ContactTitle As String = txtIndividualTitleText.Text
        Dim ContactEmail As String = txtElectronicAddressText.Text
        Dim ContactAddress1 As String = txtMailingAddressText_Contact.Text
        Dim ContactAddress2 As String = txtSupplementalAddressText_Contact.Text
        Dim ContactCity As String = txtMailingAddressCityName_Contact.Text
        Dim ContactState As String = ddlContact_MailState.SelectedValue
        Dim ContactZipCode As String = txtMailingAddressPostalCode_Contact.Text
        Dim ContactComment As String = txtAddressComment_Contact.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        eis_FacilityData.SaveFacilityContact(
            ContactPrefix,
            ContactFirstName,
            ContactLastName,
            ContactTitle,
            ContactEmail,
            ContactAddress1,
            ContactAddress2,
            ContactCity,
            ContactState,
            ContactZipCode,
            ContactComment,
            UpdateUser,
            FacilitySiteID
        )

    End Sub

    Private Sub SaveContactPhoneNumber()
        Dim query As String

        Dim phoneNumber As String = txtTelephoneNumberText.Text
        Dim phoneExtension As String = txtTelephoneExtensionNumberText.Text
        Dim mobilePhone As String = txtTelephoneNumber_Mobile.Text
        Dim FaxNumber As String = txtTelephoneNumber_Fax.Text
        Dim UpdateUserID As String = GetCookie(GecoCookie.UserID)
        Dim UpdateUserName As String = GetCookie(GecoCookie.UserName)
        Dim UpdateUser As String = UpdateUserID & "-" & UpdateUserName
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

        Dim facParam As New SqlParameter("@FacilitySiteID", FacilitySiteID)
        Dim params As SqlParameter()

        Try
            If phoneNumber <> "" Then
                query = "Select " &
                        " STRTELEPHONENUMBERTEXT " &
                        " FROM EIS_TELEPHONECOMM " &
                        " where EIS_TELEPHONECOMM.FACILITYSITEID = @FacilitySiteID " &
                        " and TelephoneNumberTypeCode = 'W' " &
                        " and active = '1' "

                If DB.ValueExists(query, facParam) Then
                    query = "Update EIS_TELEPHONECOMM Set " &
                            " STRTELEPHONENUMBERTEXT = @phoneNumber, " &
                            " STRTELEPHONENUMBEREXT = @phoneExtension, " &
                            " updateUser = @UpdateUser, " &
                            " UPDATEDATETIME = getdate() " &
                            " where FACILITYSITEID = @FacilitySiteID " &
                            " and TELEPHONENUMBERTYPECODE= 'W' "
                Else
                    query = "Insert Into EIS_TELEPHONECOMM " &
                            " (FACILITYSITEID, " &
                            " TELEPHONENUMBERTYPECODE, " &
                            " STRTELEPHONENUMBERTEXT, " &
                            " STRTELEPHONENUMBEREXT, " &
                            " ACTIVE, " &
                            " UPDATEUSER, " &
                            " UPDATEDATETIME, " &
                            " CREATEDATETIME) " &
                            " Values " &
                            " (@FACILITYSITEID, " &
                            " 'W', " &
                            " @phoneNumber, " &
                            " @phoneExtension, " &
                            " '1', " &
                            " @UpdateUser, " &
                            " getdate(), " &
                            " getdate()) "
                End If

                params = {
                    New SqlParameter("@FacilitySiteID", FacilitySiteID),
                    New SqlParameter("@phoneNumber", phoneNumber),
                    New SqlParameter("@phoneExtension", phoneExtension),
                    New SqlParameter("@UpdateUser", UpdateUser)
                }

                DB.RunCommand(query, params)
            End If

            If mobilePhone <> "" Then
                query = "Select " &
                        " STRTELEPHONENUMBERTEXT " &
                        " FROM EIS_TELEPHONECOMM " &
                        " where EIS_TELEPHONECOMM.FACILITYSITEID = @FacilitySiteID " &
                        " and TelephoneNumberTypeCode = 'M' " &
                        " and active = '1' "

                If DB.ValueExists(query, facParam) Then
                    query = "Update EIS_TELEPHONECOMM Set " &
                            " STRTELEPHONENUMBERTEXT = @mobilePhone, " &
                            " UPDATEUSER = @UpdateUser, " &
                            " UPDATEDATETIME = getdate() " &
                            " where FACILITYSITEID = @FacilitySiteID " &
                            " and TELEPHONENUMBERTYPECODE= 'M' "
                Else
                    query = "Insert Into EIS_TELEPHONECOMM " &
                            " (FACILITYSITEID, " &
                            " TELEPHONENUMBERTYPECODE, " &
                            " STRTELEPHONENUMBERTEXT, " &
                            " ACTIVE, " &
                            " UPDATEUSER, " &
                            " UPDATEDATETIME, " &
                            " CREATEDATETIME) " &
                            " Values " &
                            " (@FACILITYSITEID, " &
                            " 'M', " &
                            " @mobilePhone, " &
                            " '1', " &
                            " @UpdateUser, " &
                            " getdate(), " &
                            " getdate()) "
                End If

                params = {
                    New SqlParameter("@FacilitySiteID", FacilitySiteID),
                    New SqlParameter("@mobilePhone", mobilePhone),
                    New SqlParameter("@UpdateUser", UpdateUser)
                }

                DB.RunCommand(query, params)
            End If

            If FaxNumber <> "" Then
                query = "Select " &
                        " STRTELEPHONENUMBERTEXT " &
                        " FROM EIS_TELEPHONECOMM " &
                        " where EIS_TELEPHONECOMM.FACILITYSITEID = @FacilitySiteID " &
                        " and TelephoneNumberTypeCode = 'F' " &
                        " and active = '1' "

                If DB.ValueExists(query, facParam) Then
                    query = "Update EIS_TELEPHONECOMM Set " &
                            " STRTELEPHONENUMBERTEXT = @FaxNumber, " &
                            " UPDATEUSER = @UpdateUser, " &
                            " UPDATEDATETIME = getdate() " &
                            " where FACILITYSITEID = @FacilitySiteID " &
                            " and TELEPHONENUMBERTYPECODE= 'F' "
                Else
                    query = "Insert Into EIS_TELEPHONECOMM " &
                            " (FACILITYSITEID, " &
                            " TELEPHONENUMBERTYPECODE, " &
                            " STRTELEPHONENUMBERTEXT, " &
                            " ACTIVE, " &
                            " UPDATEUSER, " &
                            " UPDATEDATETIME, " &
                            " CREATEDATETIME) " &
                            " Values " &
                            " (@FACILITYSITEID, " &
                            " 'F', " &
                            " @FaxNumber, " &
                            " '1', " &
                            " @UpdateUser, " &
                            " getdate(), " &
                            " getdate()) "
                End If

                params = {
                    New SqlParameter("@FacilitySiteID", FacilitySiteID),
                    New SqlParameter("@FaxNumber", FaxNumber),
                    New SqlParameter("@UpdateUser", UpdateUser)
                }

                DB.RunCommand(query, params)
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

    'Saves Contact information to the IAIP
    Private Sub SaveAPBContactInformation()
        Dim query As String
        Dim ContactPrefix As String = txtPrefix.Text
        Dim ContactFirstName As String = txtFirstName.Text
        Dim ContactLastName As String = txtLastName.Text
        Dim ContactTitle As String = txtIndividualTitleText.Text
        Dim ContactEmail As String = txtElectronicAddressText.Text
        Dim EIKey As String = "41"
        Dim ContactAddress1 As String = txtMailingAddressText_Contact.Text
        Dim ContactAddress2 As String = txtSupplementalAddressText_Contact.Text
        Dim ContactCity As String = txtMailingAddressCityName_Contact.Text
        Dim ContactState As String = ddlContact_MailState.SelectedValue
        Dim ContactZipCode As String = txtMailingAddressPostalCode_Contact.Text
        Dim contactphoneNo As String = txtTelephoneNumberText.Text
        Dim contactMobileNo As String = txtTelephoneNumber_Mobile.Text
        Dim contactFax As String = txtTelephoneNumber_Fax.Text
        Dim contactComment As String = txtAddressComment_Contact.Text
        Dim IAIPUserID As String = GecoUserID
        Dim FacilitySiteID As String = "0413" & GetCookie(Cookie.AirsNumber)
        Dim ContactKey As String = FacilitySiteID & EIKey
        contactComment = "Contact info updated from the GECO EIS Application on " & Now.Date.ToString & ". Comments from GECO:" & contactComment

        'Truncate comment if >400 chars
        contactComment = Left(contactComment, 400)

        Try
            query = "Select strkey FROM APBCONTACTINFORMATION " &
                  " where strAIRSNumber = @FacilitySiteID " &
                  " and strkey = @EIKey "

            Dim params As SqlParameter() = {
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EIKey", EIKey)
            }

            If DB.ValueExists(query, params) Then
                query = "Update APBCONTACTINFORMATION Set " &
                        " STRCONTACTPREFIX = @ContactPrefix, " &
                        " STRCONTACTFIRSTNAME = @ContactFirstName, " &
                        " STRCONTACTLASTNAME = @ContactLastName, " &
                        " STRCONTACTTITLE = @ContactTitle, " &
                        " STRCONTACTEMAIL = @ContactEmail, " &
                        " STRCONTACTADDRESS1 = @ContactAddress1, " &
                        " STRCONTACTADDRESS2 = @ContactAddress2, " &
                        " STRCONTACTCITY = @ContactCity, " &
                        " STRCONTACTSTATE = @ContactState, " &
                        " STRCONTACTZIPCODE = @ContactZipCode, " &
                        " STRCONTACTPHONENUMBER1 = @contactphoneNo, " &
                        " STRCONTACTPHONENUMBER2 = @contactMobileNo, " &
                        " STRCONTACTFAXNUMBER = @contactFax, " &
                        " STRCONTACTDESCRIPTION = @contactComment, " &
                        " STRMODIFINGPERSON = @IAIPUserID," &
                        " DATMODIFINGDATE = getdate() " &
                        " where strAIRSNumber = @FacilitySiteID " &
                        " and strkey = @EIKey "
            Else
                query = "Insert into APBCONTACTINFORMATION (" &
                        " STRCONTACTKEY, " &
                        " STRAIRSNUMBER, " &
                        " STRKEY, " &
                        " STRCONTACTFIRSTNAME, " &
                        " STRCONTACTLASTNAME, " &
                        " STRCONTACTPREFIX, " &
                        " STRCONTACTTITLE, " &
                        " STRCONTACTPHONENUMBER1, " &
                        " STRCONTACTPHONENUMBER2, " &
                        " STRCONTACTFAXNUMBER, " &
                        " STRCONTACTEMAIL, " &
                        " STRCONTACTADDRESS1, " &
                        " STRCONTACTADDRESS2, " &
                        " STRCONTACTCITY, " &
                        " STRCONTACTSTATE, " &
                        " STRCONTACTZIPCODE, " &
                        " STRMODIFINGPERSON, " &
                        " DATMODIFINGDATE, " &
                        " STRCONTACTDESCRIPTION) " &
                        " Values (" &
                        " @ContactKey, " &
                        " @FacilitySiteID, " &
                        " @EIKey, " &
                        " @ContactFirstName, " &
                        " @ContactLastName, " &
                        " @ContactPrefix, " &
                        " @ContactTitle, " &
                        " @contactphoneNo, " &
                        " @contactMobileNo, " &
                        " @contactFax, " &
                        " @ContactEmail, " &
                        " @ContactAddress1, " &
                        " @ContactAddress2, " &
                        " @ContactCity, " &
                        " @ContactState, " &
                        " @ContactZipCode, " &
                        " @IAIPUserID, " &
                        " getdate(), " &
                        " @contactComment " &
                        " ) "
            End If

            params = {
                New SqlParameter("@ContactKey", ContactKey),
                New SqlParameter("@FacilitySiteID", FacilitySiteID),
                New SqlParameter("@EIKey", EIKey),
                New SqlParameter("@ContactFirstName", ContactFirstName),
                New SqlParameter("@ContactLastName", ContactLastName),
                New SqlParameter("@ContactPrefix", ContactPrefix),
                New SqlParameter("@ContactTitle", ContactTitle),
                New SqlParameter("@contactphoneNo", contactphoneNo),
                New SqlParameter("@contactMobileNo", contactMobileNo),
                New SqlParameter("@contactFax", contactFax),
                New SqlParameter("@ContactEmail", ContactEmail),
                New SqlParameter("@ContactAddress1", ContactAddress1),
                New SqlParameter("@ContactAddress2", ContactAddress2),
                New SqlParameter("@ContactCity", ContactCity),
                New SqlParameter("@ContactState", ContactState),
                New SqlParameter("@ContactZipCode", Replace(ContactZipCode, "-", "")),
                New SqlParameter("@IAIPUserID", IAIPUserID),
                New SqlParameter("@contactComment", contactComment)
            }

            DB.RunCommand(query, params)
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

#End Region

#Region " Google Map Routines "

    Protected Sub lbtnGetLatLon_Click(sender As Object, e As EventArgs) Handles lbtnGetLatLon.Click
        Try
            lbtnGetLatLon_ModalPopupExtender.Show()
            GMap.ApiKey = ConfigurationManager.AppSettings("GoogleMapsAPIKey")
            Dim latitude As Double
            Dim longitude As Double

            If Double.TryParse(TxtLatitudeMeasure.Text, latitude) AndAlso Double.TryParse(TxtLongitudeMeasure.Text, longitude) Then
                GMap.Center = LatLng.Create(latitude, longitude)
                txtMapLat.Text = Left(latitude.ToString(), 8)
                txtMapLon.Text = Left(longitude.ToString(), 9)
            ElseIf TxtLocationAddressText.Text <> "" AndAlso TxtLocalityName.Text <> "" Then
                Dim address As String = TxtLocationAddressText.Text
                Dim citystatezip = TxtLocalityName.Text & ", " & TxtLocationAddressStateCode.Text & " " & TxtLocationAddressPostalCode.Text
                Dim geocoder = New Geocoding.GoogleGeocoder()
                Dim georesult = geocoder.Geocode(address & ", " & citystatezip)
                If georesult.Status = GeocodeStatus.OK Then
                    GMap.Center = georesult.Locations.FirstOrDefault.Point.ToLatLng()
                    latitude = GMap.Center.Latitude
                    longitude = GMap.Center.Longitude
                End If
            Else
                Dim CountyCode As String = GetCookie(Cookie.AirsNumber).Substring(0, 3)
                Dim mm As MinMaxLatLon = GetCountyLatLong(CountyCode)
                latitude = (mm.MaxLat + mm.MinLat) / 2
                longitude = (mm.MaxLon + mm.MinLon) / 2
                GMap.Center = LatLng.Create(latitude, longitude)
                txtMapLat.Text = Left(latitude.ToString(), 8)
                txtMapLon.Text = Left(longitude.ToString(), 9)
            End If
            GMap.MapControls.Add(New Controls.ScaleControl())
            GMap.MapControls.Add(New Controls.ZoomControl())
            GMap.MapControls.Add(New Controls.MapTypeControl())
            GMap.Zoom = 15
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub GMap_Click(sender As Object, e As CoordinatesEventArgs) Handles GMap.Click
        GMap.Overlays.Clear()

        Dim myOverlay As New Marker(New Guid(), e.Coordinates.Latitude, e.Coordinates.Longitude)
        GMap.Overlays.Add(myOverlay)

        Dim Mapcommand As String = GMap.UpdateOverlays()
        Mapcommand &= String.Format("document.getElementById('{0}').value = " & e.Coordinates.Latitude & ";", txtMapLat.ClientID)
        Mapcommand &= String.Format("document.getElementById('{0}').value = " & e.Coordinates.Longitude & ";", txtMapLon.ClientID)

        e.MapCommand = Mapcommand
    End Sub

    Protected Sub btnUseLatLon_Click(sender As Object, e As EventArgs) Handles btnUseLatLon.Click
        TxtLatitudeMeasure.Text = Left(txtMapLat.Text, 8)
        TxtLongitudeMeasure.Text = Left(txtMapLon.Text, 9)

        ddlHorCollectionMetCode.SelectedValue = "007"
        TxtHorizontalAccuracyMeasure.Text = "25"
        ddlHorReferenceDatCode.SelectedValue = "002"

        If TxtLatitudeMeasure.Text <> "" And TxtLongitudeMeasure.Text <> "" Then
            imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(TxtLatitudeMeasure.Text, TxtLongitudeMeasure.Text))
            lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(New Coordinate(TxtLatitudeMeasure.Text, TxtLongitudeMeasure.Text))
        Else
            imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(TxtLocationAddressText.Text, TxtLocalityName.Text)
            lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(TxtLocationAddressText.Text, TxtLocalityName.Text)
        End If
    End Sub

#End Region

End Class