Imports System.Data.SqlClient
Imports System.Data
Imports GECO.MapHelper

Partial Class eis_facility_details
    Inherits Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim EISStatus As String = GetCookie(EisCookie.EISStatus)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            LoadFacilityDetails()
            LoadPhoneNumbers()
        End If

        ShowFacilityInventoryMenu()
        ShowEISHelpMenu()

        If EISStatus = "2" Then
            ShowEmissionInventoryMenu()
        Else
            HideEmissionInventoryMenu()
        End If

        HideTextBoxBorders(Me)
    End Sub
    Private Sub LoadFacilityDetails()
        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        TxtLocationAddressStateCode.Text = "GA"
        Dim UpdateUser As String
        Dim UpdateDateTime As String
        Dim HORACCURACYMEASURE As Decimal
        Dim StatusDesc As String
        Dim OperatingStatusCodeYear As String
        Dim NAICSCode As String
        Dim NAICSDesc As String

        Try
            Dim query = "select strFacilitySiteName, " &
                "strMailingAddressText, " &
                "strSupplementalAddressText, " &
                "strMailingAddressCityName, " &
                "strMailingAddressStateCode, " &
                "strMailingAddressPostalCode, " &
                "strLocationAddressText, " &
                "strSupplementalLocationText , " &
                "strLocalityName, " &
                "strLocationAddressPostalCode , " &
                "strAddressComment, " &
                "strFacilitySiteStatusDesc, " &
                "intFacilitySiteStatusCodeYear, " &
                "strFacilitySiteDescription, " &
                "strNAICSCode, " &
                "strFacilitySiteComment , " &
                "numLatitudeMeasure, " &
                "numLongitudeMeasure, " &
                "STRHORCOLLMETDesc, " &
                "INTHORACCURACYMEASURE , " &
                "STRHORREFDATUMDesc, " &
                "strGeographicComment, " &
                "strNamePrefixText, " &
                "strFirstName, " &
                "strLastName, " &
                "strIndividualTitleText , " &
                "strFSAIMAddressText, " &
                "strFSAISAddressText, " &
                "strFSAIMAddressCityName , " &
                "strFSAIMAddressStateCode, " &
                "strFSAIMAddressPostalCode, " &
                "StrElectronicAddressText, " &
                "UpdateUser_mailingAddress, " &
                "convert(char, UpdateDateTime_mailingAddress, 20)  As UpdateDateTime_mailingAddress,  " &
                "convert(char, LastEPASubmitDate_MAddress, 101) As LastEISSubmitDate_MAddress, " &
                "UpdateUser_FacilitySite, " &
                "convert(char, UpdateDateTime_FacilitySite, 20) As UpdateDateTime_FacilitySite,  " &
                "convert(char, LastEPASubmitDate_FacilitySite, 101) As LastEISSubmitDate_FacilitySite, " &
                "UpdateUser_GeoCoord, " &
                "convert(char, UpdateDateTime_GeoCoord, 20) As UpdateDateTime_GeoCoord,  " &
                "convert(char, LastEPASubmitDate_GeoCoord, 101) As LastEISSubmitDate_GeoCoord, " &
                "UpdateUser_AffIndiv, " &
                "convert(char, UpdateDateTime_AffIndiv, 20) As UpdateDateTime_AffIndiv,  " &
                "convert(char, LastEPASubmitDate_AffIndiv, 101) As LastEISSubmitDate_AffIndiv, " &
                "strFSAIAddressComment " &
                "FROM VW_EIS_facility where VW_EIS_facility.FACILITYSITEID = @FacilitySiteID "

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dr = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then

                'Facility Name and Location
                If IsDBNull(dr("strFacilitySiteName")) Then
                    txtFacilitySiteName.Text = ""
                Else
                    txtFacilitySiteName.Text = dr.Item("strFacilitySiteName")
                End If

                If IsDBNull(dr("strLocationAddressText")) Then
                    TxtLocationAddressText.Text = ""
                Else
                    TxtLocationAddressText.Text = dr.Item("strLocationAddressText")
                End If
                If IsDBNull(dr("strSupplementalLocationText")) Then
                    TxtSupplementalLocationText2.Text = ""
                Else
                    TxtSupplementalLocationText2.Text = dr.Item("strSupplementalLocationText")
                End If

                If IsDBNull(dr("strLocalityName")) Then
                    TxtLocalityName.Text = ""
                Else
                    TxtLocalityName.Text = dr.Item("strLocalityName")
                End If
                If IsDBNull(dr("strLocationAddressPostalCode")) Then
                    TxtLocationAddressPostalCode.Text = ""
                Else
                    TxtLocationAddressPostalCode.Text = dr.Item("strLocationAddressPostalCode")
                End If

                'Mailing Address
                If IsDBNull(dr("strMailingAddressText")) Then
                    txtmailingAddressText.Text = ""
                Else
                    txtmailingAddressText.Text = dr.Item("strMailingAddressText")
                End If
                If IsDBNull(dr("strSupplementalAddressText")) Then
                    TxtSupplementalAddressText.Text = ""
                Else
                    TxtSupplementalAddressText.Text = dr.Item("strSupplementalAddressText")
                End If

                If IsDBNull(dr("strMailingAddressCityName")) Then
                    txtMailingAddressCityName.Text = ""
                Else
                    txtMailingAddressCityName.Text = dr.Item("strMailingAddressCityName")
                End If

                If IsDBNull(dr("strMailingAddressStateCode")) Then
                    txtMailingAddressStateCode.Text = ""
                Else
                    txtMailingAddressStateCode.Text = dr.Item("strMailingAddressStateCode")
                End If
                If IsDBNull(dr("strMailingAddressPostalCode")) Then
                    TxtMailingAddressPostalCode.Text = ""
                Else
                    TxtMailingAddressPostalCode.Text = dr.Item("strMailingAddressPostalCode")
                End If

                If IsDBNull(dr("strAddressComment")) Then
                    TxtMailingAddressComment.Text = ""
                    TxtMailingAddressComment.Visible = False
                Else
                    TxtMailingAddressComment.Text = dr.Item("strAddressComment")
                End If
                If IsDBNull(dr("LastEISSubmitDate_MAddress")) Then
                    txtLastEISSubmit_M.Text = "Never submitted"
                Else
                    txtLastEISSubmit_M.Text = dr.Item("LastEISSubmitDate_MAddress")
                End If
                If IsDBNull(dr("UpdateUser_mailingAddress")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr.Item("UpdateUser_mailingAddress")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr("UpdateDateTime_mailingAddress")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr.Item("UpdateDateTime_mailingAddress")
                End If

                txtlastUpdate_M.Text = UpdateDateTime & " by " & UpdateUser

                'Description
                If IsDBNull(dr("strFacilitySiteStatusDesc")) Then
                    StatusDesc = ""
                Else
                    StatusDesc = dr.Item("strFacilitySiteStatusDesc")

                End If
                If IsDBNull(dr("intFacilitySiteStatusCodeYear")) Then
                    OperatingStatusCodeYear = ""
                    txtFacilitySiteStatusCode.Text = StatusDesc
                Else
                    OperatingStatusCodeYear = dr.Item("intFacilitySiteStatusCodeYear")
                    txtFacilitySiteStatusCode.Text = StatusDesc & " as reported in " & OperatingStatusCodeYear
                End If
                If IsDBNull(dr("strFacilitySiteDescription")) Then
                    TxtFacilitySiteDescription.Text = ""
                Else
                    TxtFacilitySiteDescription.Text = dr.Item("strFacilitySiteDescription")
                End If

                If IsDBNull(dr("strNAICSCode")) Then
                    NAICSCode = ""
                Else
                    NAICSCode = dr.Item("strNAICSCode")
                    NAICSDesc = GetNaicsCodeDesc(NAICSCode)
                    TxtNAICSCode.Text = NAICSCode & " - " & NAICSDesc
                End If

                If IsDBNull(dr("strFacilitySiteComment")) Then
                    TxtFacilitySiteComment.Text = ""
                    TxtFacilitySiteComment.Visible = False
                Else
                    TxtFacilitySiteComment.Text = dr.Item("strFacilitySiteComment")
                End If
                If IsDBNull(dr("LastEISSubmitDate_FacilitySite")) Then
                    txtLastEISSubmit.Text = "Never submitted"
                Else
                    txtLastEISSubmit.Text = dr.Item("LastEISSubmitDate_FacilitySite")
                End If
                If IsDBNull(dr("UpdateUser_FacilitySite")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr.Item("UpdateUser_FacilitySite")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr("UpdateDateTime_FacilitySite")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr.Item("UpdateDateTime_FacilitySite")
                End If

                txtLastUpdate.Text = UpdateDateTime & " by " & UpdateUser

                'G.C. info
                If IsDBNull(dr("numLatitudeMeasure")) Then
                    TxtLatitudeMeasure.Text = ""
                Else
                    TxtLatitudeMeasure.Text = dr.Item("numLatitudeMeasure")
                End If
                If IsDBNull(dr("numLongitudeMeasure")) Then
                    TxtLongitudeMeasure.Text = ""
                Else
                    TxtLongitudeMeasure.Text = dr.Item("numLongitudeMeasure")
                End If
                If IsDBNull(dr("STRHORCOLLMETDesc")) Then
                    TxtHorCollectionMetCode.Text = ""
                Else
                    TxtHorCollectionMetCode.Text = dr.Item("STRHORCOLLMETDesc")
                End If

                If IsDBNull(dr("INTHORACCURACYMEASURE")) Then
                    TxtHorizontalAccuracyMeasure.Text = ""
                Else
                    HORACCURACYMEASURE = dr.Item("INTHORACCURACYMEASURE")
                    If HORACCURACYMEASURE = -1 Then
                        TxtHorizontalAccuracyMeasure.Text = ""
                    Else
                        TxtHorizontalAccuracyMeasure.Text = HORACCURACYMEASURE
                    End If
                End If
                If IsDBNull(dr("STRHORREFDATUMDesc")) Then
                    TxtHorReferenceDatCode.Text = ""
                Else
                    TxtHorReferenceDatCode.Text = dr.Item("STRHORREFDATUMDesc")
                End If
                If IsDBNull(dr("strGeographicComment")) Then
                    TxtGeographicComment.Text = ""
                    TxtGeographicComment.Visible = False
                Else
                    TxtGeographicComment.Text = dr.Item("strGeographicComment")
                End If
                If TxtLatitudeMeasure.Text <> "" AndAlso TxtLongitudeMeasure.Text <> "" Then
                    imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(New Coordinate(TxtLatitudeMeasure.Text, TxtLongitudeMeasure.Text))
                    lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(New Coordinate(TxtLatitudeMeasure.Text, TxtLongitudeMeasure.Text))
                Else
                    imgGoogleStaticMap.ImageUrl = GoogleMaps.GetStaticMapUrl(TxtLocationAddressText.Text, TxtLocalityName.Text)
                    lnkGoogleMap.NavigateUrl = GoogleMaps.GetMapLinkUrl(TxtLocationAddressText.Text, TxtLocalityName.Text)
                End If
                If IsDBNull(dr("LastEISSubmitDate_GeoCoord")) Then
                    txtLastSubmitEPA_GC.Text = "Never submitted"
                Else
                    txtLastSubmitEPA_GC.Text = dr.Item("LastEISSubmitDate_GeoCoord")
                End If
                If IsDBNull(dr("UpdateUser_GeoCoord")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr.Item("UpdateUser_GeoCoord")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr("UpdateDateTime_GeoCoord")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr.Item("UpdateDateTime_GeoCoord")
                End If

                txtLastUpdate_GC.Text = UpdateDateTime & " by " & UpdateUser

                'Contact info

                If IsDBNull(dr("strNamePrefixText")) Then
                    txtNamePrefix.Text = ""
                Else
                    txtNamePrefix.Text = dr.Item("strNamePrefixText")
                End If
                If IsDBNull(dr("strFirstName")) Then
                    txtfirstname.Text = ""
                Else
                    txtfirstname.Text = dr.Item("strFirstName")
                End If

                If IsDBNull(dr("strLastName")) Then
                    txtLastName.Text = ""
                Else
                    txtLastName.Text = dr.Item("strLastName")
                End If
                If IsDBNull(dr("STRINDIVIDUALTITLETEXT")) Then
                    txtIndividualTitleText.Text = ""
                Else
                    txtIndividualTitleText.Text = dr.Item("STRINDIVIDUALTITLETEXT")
                End If
                If IsDBNull(dr("STRFSAIMADDRESSTEXT")) Then
                    txtMailingAddressText_contact.Text = ""
                Else
                    txtMailingAddressText_contact.Text = dr.Item("STRFSAIMADDRESSTEXT")
                End If

                If IsDBNull(dr("STRFSAISADDRESSTEXT")) Then
                    txtSupplementalAddressText_contact.Text = ""
                Else
                    txtSupplementalAddressText_contact.Text = dr.Item("STRFSAISADDRESSTEXT")
                End If
                If IsDBNull(dr("STRFSAIMADDRESSCITYNAME")) Then
                    txtMailingAddressCityName_contact.Text = ""
                Else
                    txtMailingAddressCityName_contact.Text = dr.Item("STRFSAIMADDRESSCITYNAME")
                End If

                If IsDBNull(dr("STRFSAIMADDRESSSTATECODE")) Then
                    txtMailingAddressStateCode_contact.Text = ""
                Else
                    txtMailingAddressStateCode_contact.Text = dr.Item("STRFSAIMADDRESSSTATECODE")
                End If

                If IsDBNull(dr("STRFSAIMADDRESSPOSTALCODE")) Then
                    txtMailingAddressPostalCode_Contact.Text = ""
                Else
                    txtMailingAddressPostalCode_Contact.Text = dr.Item("STRFSAIMADDRESSPOSTALCODE")
                End If
                If IsDBNull(dr("StrElectronicAddressText")) Then
                    txtElectronicAddressText.Text = ""
                Else
                    txtElectronicAddressText.Text = dr.Item("StrElectronicAddressText")
                End If
                If IsDBNull(dr("strFSAIAddressComment")) Then
                    txtAddressComment_contact.Text = ""
                    txtAddressComment_contact.Visible = False
                Else
                    txtAddressComment_contact.Text = dr.Item("strFSAIAddressComment")
                End If
                If IsDBNull(dr("LastEISSubmitDate_AffIndiv")) Then
                    txtLastEISSubmitEPA_C.Text = "Never submitted"
                Else
                    txtLastEISSubmitEPA_C.Text = dr.Item("LastEISSubmitDate_AffIndiv")
                End If
                If IsDBNull(dr("UpdateUser_AffIndiv")) Then
                    UpdateUser = ""
                Else
                    UpdateUser = dr.Item("UpdateUser_AffIndiv")
                    UpdateUser = Mid(UpdateUser, InStr(UpdateUser, "-") + 1)
                End If
                If IsDBNull(dr("UpdateDateTime_AffIndiv")) Then
                    UpdateDateTime = ""
                Else
                    UpdateDateTime = dr.Item("UpdateDateTime_AffIndiv")
                End If

                txtLastUpdate_C.Text = UpdateDateTime & " by " & UpdateUser
            End If

        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Private Sub LoadPhoneNumbers()
        Try
            Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)

            Dim query = " select * FROM EIS_TELEPHONECOMM " &
                " where EIS_TELEPHONECOMM.FACILITYSITEID = @FacilitySiteID "

            Dim param As New SqlParameter("@FacilitySiteID", FacilitySiteID)

            Dim dt As DataTable = DB.GetDataTable(query, param)

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows
                    Select Case dr.Item("TELEPHONENUMBERTYPECODE")
                        Case "W"
                            txtTelephoneNumberText.Text = dr.Item("STRTELEPHONENUMBERTEXT")
                        Case "F"
                            txtTelephoneNumber_Fax.Text = dr.Item("STRTELEPHONENUMBERTEXT")
                        Case "M"
                            txtTelephoneNumber_Mobile.Text = dr.Item("STRTELEPHONENUMBERTEXT")
                    End Select
                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnEditAllInfo_Click(sender As Object, e As EventArgs) Handles btnEditAllInfo.Click
        Response.Redirect("facility_edit.aspx")
    End Sub

#Region "  Menu Routines  "

    Private Sub ShowFacilityInventoryMenu()

        Dim menuFacilityInventory As Panel

        menuFacilityInventory = CType(Master.FindControl("pnlFacilityInventory"), Panel)

        If Not menuFacilityInventory Is Nothing Then
            menuFacilityInventory.Visible = True
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

#End Region

End Class