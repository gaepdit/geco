Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels

Partial Class FacilitySummary
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property facilityAccess As FacilityAccess
    Private Property currentAirs As ApbFacilityId

#Region " Page Load "

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
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

        MainLoginCheck(Page.ResolveUrl("~/Facility/Summary.aspx?airs=" & currentAirs.ShortString))

        ' Current user
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Home/")
        End If

        If Not IsPostBack Then
            LoadFacilityLocation()
            LoadFacilityHeaderData()
            LoadStateContactInformation()

            Dim mpUserLabel, mpFacilityLabel, mpAirsLabel As Label
            mpUserLabel = CType(Master.FindControl("lblUserName"), Label)
            mpUserLabel.Text = "Welcome, " & currentUser.FullName & " | "
            mpFacilityLabel = CType(Master.FindControl("lblFacilityName"), Label)
            mpFacilityLabel.Text = "Facility: " & lblFacilityDisplay.Text & " | "
            mpAirsLabel = CType(Master.FindControl("lblAirsNo"), Label)
            mpAirsLabel.Text = "AIRS No: " & currentAirs.FormattedString()

            Title = "GECO Facility Summary - " & lblFacilityDisplay.Text
            lblAIRS.Text = currentAirs.FormattedString

            Master.IsFacilitySubpage = True
        End If
    End Sub

#End Region

#Region " Load data "

    Protected Sub LoadFacilityLocation()
        Try
            Dim query = "Select * " &
            " FROM VW_APBFacilityLocation " &
            " where strAIRSNumber = @airs "

            Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then
                lblFacilityDisplay.Text = GetNullableString(dr.Item("STRFACILITYNAME")) & ", " & GetNullableString(dr.Item("STRFACILITYCITY"))

                lblAddress.Text = GetNullableString(dr.Item("strFacilityStreet1"))
                lblCityStateZip.Text = GetNullableString(dr.Item("strFacilityCity")) & ", " &
                    GetNullableString(dr.Item("strFacilityState")) & " " &
                    Address.FormatPostalCode(GetNullableString(dr.Item("strFacilityZipCode")))
                lblLatitude.Text = GetNullableString(dr.Item("numFacilityLatitude"))
                lblLongitude.Text = GetNullableString(dr.Item("numFacilityLongitude"))
                lblCounty.Text = GetNullableString(dr.Item("strCountyName"))
                lblDistrict.Text = GetNullableString(dr.Item("strDistrictName"))
                lblOffice.Text = GetNullableString(dr.Item("strOfficeName"))

                If IsDBNull(dr.Item("strDistrictResponsible")) OrElse
                    dr.Item("strDistrictResponsible") <> "True" Then
                    hlDistrict.Visible = False
                Else
                    hlDistrict.Visible = True
                End If
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub LoadFacilityHeaderData()
        Dim query = "Select * " &
        " FROM VW_APBFacilityHeader " &
        " where strAIRSNumber = @airs "

        Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

        Dim dr As DataRow = DB.GetDataRow(query, param)

        If dr IsNot Nothing Then

            Select Case GetNullableString(dr.Item("strOperationalstatus"))
                Case "O"
                    lblOpStatus.Text = "Operational"
                Case "P"
                    lblOpStatus.Text = "Planned"
                Case "C"
                    lblOpStatus.Text = "Under Construction"
                Case "T"
                    lblOpStatus.Text = "Temporarily Closed"
                Case "X"
                    lblOpStatus.Text = "Closed/Dismantled"
                Case "I"
                    lblOpStatus.Text = "Seasonal Operation"
                Case Else
                    lblOpStatus.Text = "Unknown (fix needed)"
            End Select

            lblClassification.Text = GetNullableString(dr.Item("strClass"))

            Dim startupDate As Date? = GetNullableDateTime(dr.Item("datStartupDate"))
            lblStartUp.Text = If(startupDate.HasValue, startupDate.Value.ToShortDateString, String.Empty)

            Dim shutdownDate As Date? = GetNullableDateTime(dr.Item("datShutDownDate"))
            lblClosed.Text = If(shutdownDate.HasValue, shutdownDate.Value.ToShortDateString, String.Empty)

            lblCMSStatus.Text = GetNullableString(dr.Item("strCMSMember"))

            lblSICCode.Text = GetNullableString(dr.Item("strSICCode"))

            AddAirProgramCodes(GetNullableString(dr.Item("strAIRProgramcodes")))
        End If
    End Sub

    Protected Sub AddAirProgramCodes(AirProgramCode As String)
        If String.IsNullOrEmpty(AirProgramCode) Then
            Exit Sub
        End If

        Dim codes As New List(Of String)

        If Mid(AirProgramCode, 1, 1) = 1 Then
            codes.Add("SIP")
        End If
        If Mid(AirProgramCode, 5, 1) = 1 Then
            codes.Add("PSD")
        End If
        If Mid(AirProgramCode, 6, 1) = 1 Then
            codes.Add("NSR")
        End If
        If Mid(AirProgramCode, 7, 1) = 1 Then
            codes.Add("NESHAP")
        End If
        If Mid(AirProgramCode, 8, 1) = 1 Then
            codes.Add("NSPS")
        End If
        If Mid(AirProgramCode, 12, 1) = 1 Then
            codes.Add("MACT")
        End If
        If Mid(AirProgramCode, 13, 1) = 1 Then
            codes.Add("Title V")
        End If
        If Mid(AirProgramCode, 10, 1) = 1 Then
            codes.Add("Acid Precipitation")
        End If

        lblAirProgramCodes.Text = ConcatNonEmptyStrings(", ", codes.ToArray)
    End Sub

    Protected Sub LoadStateContactInformation()
        Try
            Dim query As String =
            " SELECT PermittingStaff.strAIRSNumber, " &
            "        SSCPEngineer, " &
            "        SSCPUnit, " &
            "        SSCPEmailAddress, " &
            "        SSCPPhone, " &
            "        ISMPEngineer, " &
            "        ISMPUnit, " &
            "        ISMPEmailAddress, " &
            "        ISMPPhone, " &
            "        SSPPEngineer, " &
            "        SSPPUnit, " &
            "        SSPPEmailAddress, " &
            "        SSPPPhone, " &
            "        DistrictOffice " &
            " FROM " &
            " ( " &
            "     SELECT DISTINCT " &
            "            ((strLastName+', '+strFirstName)) AS SSPPEngineer, " &
            "            SSPPApplicationMaster.strAIRSnumber, " &
            "            strEmailAddress AS SSPPEmailAddress, " &
            "            strPhone AS SSPPPhone, " &
            "            strUnitDesc AS SSPPUnit " &
            "     FROM EPDUserProfiles " &
            "          LEFT JOIN LookUpEPDUnits  " &
            " 	    ON EPDUserProfiles.numUnit = LookUpEPDUnits.numUnitCode " &
            "          LEFT JOIN SSPPApplicationMaster  " &
            " 	    ON EPDUserProfiles.numUserID = strStaffResponsible " &
            "                                             AND SSPPApplicationMaster.strApplicationNumber = " &
            "     ( " &
            "         SELECT DISTINCT " &
            "                (MAX(CAST(strApplicationNumber AS INT))) AS GreatestApplication " &
            "         FROM SSPPApplicationMaster " &
            "         WHERE SSPPApplicationMaster.strAIRSNumber = @airs " &
            "     ) " &
            "     WHERE SSPPApplicationMaster.strAIRSnumber = @airs " &
            " ) PermittingStaff " &
            "  " &
            " LEFT JOIN " &
            " ( " &
            "     SELECT(strLastName+', '+strFirstName) AS SSCPEngineer, " &
            "           strEmailAddress AS SSCPEmailAddress, " &
            "           strPhone AS SSCPPhone, " &
            "           SSCPFacilityAssignment.strAIRSNumber, " &
            "           strUnitDesc AS SSCPUnit " &
            "     FROM EPDUserProfiles " &
            "          LEFT JOIN LookUpEPDUnits  " &
            " 	    ON EPDUserProfiles.numUnit = LookUpEPDUnits.numUnitCode " &
            "          LEFT JOIN SSCPFacilityAssignment  " &
            " 	    ON EPDUserProfiles.numUserID = SSCPFacilityAssignment.strSSCPEngineer " &
            "     WHERE SSCPFacilityAssignment.[strAIRSNumber] = @airs " &
            " ) ComplianceStaff ON PermittingStaff.strairsnumber = ComplianceStaff.strairsnumber " &
            "  " &
            " LEFT JOIN " &
            " ( " &
            "     SELECT DISTINCT " &
            "            ((strLastName+', '+strFirstName)) AS ISMPEngineer, " &
            "            ISMPMaster.strAIRSNumber, " &
            "            strEmailAddress AS ISMPEmailAddress, " &
            "            strPhone AS ISMPPhone, " &
            "            strUnitDesc AS ISMPUnit " &
            "     FROM EPDUserProfiles " &
            "          LEFT JOIN LookUpEPDUnits  " &
            " 	    ON EPDUserProfiles.numUnit = LookUpEPDunits.numunitCode " &
            "          LEFT JOIN ISMPReportInformation  " &
            " 	    ON EPDUserProfiles.numUserID = strReviewingEngineer " &
            "          INNER JOIN ISMPMaster  " &
            " 	    ON ISMPMaster.strReferenceNumber = ISMPReportInformation.strReferenceNumber " &
            "                                   AND strClosed = 'True' " &
            "     WHERE ISMPReportInformation.datCompleteDate = " &
            "     ( " &
            "         SELECT DISTINCT " &
            "                (MAX(datCompleteDate)) AS CompleteDate " &
            "         FROM ISMPReportInformation, " &
            "              ISMPMaster " &
            "         WHERE ISMPReportInformation.strReferenceNumber = ISMPMaster.strReferenceNumber " &
            "               AND ISMPMaster.strAIRSNumber = @airs " &
            "               AND strClosed = 'True' " &
            "     ) " &
            "           AND ISMPMaster.strAIRSNumber = @airs " &
            " ) MonitoringStaff  " &
            " ON PermittingStaff.strairsnumber = monitoringstaff.strairsnumber " &
            "  " &
            " LEFT JOIN " &
            " ( " &
            "     SELECT SSCPDistrictResponsible.strAIRSNumber, " &
            "            CASE " &
            "                WHEN strDistrictResponsible = 'True' " &
            "                THEN strOfficeName " &
            "                ELSE '' " &
            "            END DistrictOffice " &
            "     FROM SSCPDistrictResponsible, " &
            "          LookUpDistrictInformation, " &
            "          LookUpDistrictOffice " &
            "     WHERE LookUpDistrictOffice.strDistrictCode = LookUpDistrictInformation.strDistrictCode " &
            "           AND LookUpDistrictInformation.strDistrictCounty = SUBSTRING(SSCPDistrictResponsible.strAIRSNumber, 5, 3) " &
            "           AND SSCPDistrictResponsible.strAIRSNumber = @airs " &
            " ) DistrictStaff  " &
            " ON PermittingStaff.strairsnumber = DistrictStaff.strairsnumber"

            Dim param As New SqlParameter("@airs", currentAirs.DbFormattedString)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr IsNot Nothing Then

                If IsDBNull(dr.Item("SSCPengineer")) OrElse
                    Left(GetNullableString(dr.Item("SSCPengineer")), 10) = "Unassigned" Then
                    lblComplianceContactName.Text = "Unassigned"
                Else
                    lblComplianceContactName.Text = GetNullableString(dr.Item("SSCPengineer"))
                End If
                If IsDBNull(dr.Item("SSCPemailaddress")) Then
                    hlComplianceContactEmail.Text = "N/A"
                Else
                    hlComplianceContactEmail.Text = GetNullableString(dr.Item("SSCPemailaddress"))
                    hlComplianceContactEmail.NavigateUrl = "mailto:" & GetNullableString(dr.Item("SSCPemailaddress"))
                End If
                If IsDBNull(dr.Item("SSCPphone")) Then
                    lblComplianceContactPhone.Text = "N/A"
                Else
                    lblComplianceContactPhone.Text = GecoUser.FormatPhoneNumber(GetNullableString(dr.Item("SSCPphone")))
                End If

                If IsDBNull(dr.Item("ISMPengineer")) OrElse
                    Left(GetNullableString(dr.Item("ISMPengineer")), 10) = "Unassigned" Then
                    lblMonitoringContactName.Text = "Unassigned"
                Else
                    lblMonitoringContactName.Text = GetNullableString(dr.Item("ISMPengineer"))
                End If
                If IsDBNull(dr.Item("ISMPemailaddress")) Then
                    hlMonitoringContactEmail.Text = "N/A"
                Else
                    hlMonitoringContactEmail.Text = GetNullableString(dr.Item("ISMPemailaddress"))
                    hlMonitoringContactEmail.NavigateUrl = "mailto:" & GetNullableString(dr.Item("ISMPemailaddress"))
                End If
                If IsDBNull(dr.Item("ISMPphone")) Then
                    lblMonitoringContactPhone.Text = "N/A"
                Else
                    lblMonitoringContactPhone.Text = GecoUser.FormatPhoneNumber(GetNullableString(dr.Item("ISMPphone")))
                End If

                If IsDBNull(dr.Item("SSPPengineer")) OrElse
                    Left(GetNullableString(dr.Item("SSPPengineer")), 10) = "Unassigned" Then
                    lblPermitContactName.Text = "Unassigned"
                Else
                    lblPermitContactName.Text = GetNullableString(dr.Item("SSPPengineer"))
                End If
                If IsDBNull(dr.Item("SSPPemailaddress")) Then
                    hlPermitContactEmail.Text = "N/A"
                Else
                    hlPermitContactEmail.Text = GetNullableString(dr.Item("SSPPemailaddress"))
                    hlPermitContactEmail.NavigateUrl = "mailto:" & GetNullableString(dr.Item("SSPPemailaddress"))
                End If
                If IsDBNull(dr.Item("SSPPphone")) Then
                    lblPermitContactPhone.Text = "N/A"
                Else
                    lblPermitContactPhone.Text = GecoUser.FormatPhoneNumber(GetNullableString(dr.Item("SSPPphone")))
                End If
            Else
                lblPermitContactPhone.Text = "N/A"
                hlPermitContactEmail.Text = "N/A"
                lblPermitContactName.Text = "Unassigned"
                lblMonitoringContactPhone.Text = "N/A"
                hlMonitoringContactEmail.Text = "N/A"
                lblMonitoringContactName.Text = "Unassigned"
                lblComplianceContactPhone.Text = "N/A"
                hlComplianceContactEmail.Text = "N/A"
                lblComplianceContactName.Text = "Unassigned"
            End If

        Catch exThreadAbort As Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try

    End Sub

#End Region

End Class
