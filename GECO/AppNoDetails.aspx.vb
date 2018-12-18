Imports System.Data
Imports System.Data.SqlClient

Partial Class AppNoDetails
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MainLoginCheck()

        If Not IsPostBack Then
            Dim easymenu = CType(Master.FindControl("EasyMenu1"), Sequentum.EasyMenu)
            easymenu.Visible = False

            Dim appNo, Permit As String
            appNo = Request.QueryString("ID")
            Permit = Request.QueryString("Permit")
            lblAppNo.Text = appNo
            LoadAppDetails(appNo)
            LoadCountyDetails()
            lblPermitNo.NavigateUrl = "http://permitsearch.gaepd.org/permit.aspx?id=" & Permit
        End If
    End Sub

    Protected Sub LoadAppDetails(ByVal appNo As String)
        Dim temp As String
        Try
            Dim sb As New System.Text.StringBuilder(1299)
            sb.AppendLine("Select strAIRSNumber, strStaffResponsible, struserfirstname, struserlastname, strphonenumber, struseremail, strApplicationType, ")
            sb.AppendLine("strPermitType, APBUnit, strFacilityName, strFacilityStreet1, strFacilityCity, strFacilityState, ")
            sb.AppendLine("strFacilityZipCode, strOperationalStatus, strClass, strAirProgramCodes, strSICCode, strPermitNumber, strPlantDescription, ")
            sb.AppendLine("strApplicationNotes, strStateProgramCodes, datReceivedDate, datSentByFacility, datAssignedToEngineer, ")
            sb.AppendLine("datReassignedToEngineer, datAcknowledgementLetterSent, strPublicInvolvement, datToPMI, datToPMII, datReturnedToEngineer, ")
            sb.AppendLine("datPermitIssued, datApplicationDeadline, datDraftIssued, strPAReady, strPNReady, datEPAWaived, datEPAEnds, ")
            sb.AppendLine("datToBranchCheif, datToDirector, datPAExpires, datPNExpires, strStateprogramcodes, strTrackedRules, STRSIGNIFICANTCOMMENTS, ")
            sb.AppendLine("strPAPosted, strPNPosted from SSPPApplicationMaster LEFT JOIN SSPPApplicationData ON SSPPApplicationMaster.strApplicationNumber = SSPPApplicationData.strApplicationNumber")
            sb.AppendLine("LEFT JOIN SSPPApplicationTracking ON SSPPApplicationMaster.strApplicationNumber = SSPPApplicationTracking.strApplicationNumber")
            sb.AppendLine("LEFT JOIN ApbUsers ON SSPPApplicationMaster.strStaffResponsible = APBUsers.strUserGcode ")
            sb.AppendLine("where SSPPApplicationMaster.strApplicationNumber = '" & appNo & "' ")

            Dim conn As New SqlConnection(oradb)
            Dim cmd = New SqlCommand(sb.ToString, conn)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim dr = cmd.ExecuteReader
            Dim recExist = dr.Read
            If recExist = True Then
                If IsDBNull(dr("strAirsNumber")) Then
                Else
                    lblAirs.Text = Mid(dr.Item("strAirsNumber"), 5)
                End If
                If IsDBNull(dr("strApplicationType")) Then
                    lblAplType.Text = ""
                Else
                    temp = GetApplicationType(dr.Item("strApplicationType"))
                    lblAplType.Text = temp
                End If
                If IsDBNull(dr("APBUnit")) Then
                    lblAplUnit.Text = ""
                Else
                    temp = GetAPBUnit(dr.Item("APBUnit"))
                    lblAplUnit.Text = temp
                End If
                If IsDBNull(dr("strPermitType")) Then
                    lblActionType.Text = ""
                Else
                    temp = GetPermitType(dr.Item("strPermitType"))
                    lblActionType.Text = temp
                End If
                If IsDBNull(dr("strFacilityname")) Then
                Else
                    lblFacName.Text = dr.Item("strFacilityname")
                End If
                If IsDBNull(dr("strFacilityStreet1")) Then
                Else
                    lblFacAddress.Text = dr.Item("strFacilityStreet1")
                End If
                lblFacCityStateZip.Text = dr.Item("strFacilityCity") & ", " & dr.Item("strFacilityState") & " " & dr.Item("strFacilityZipCode")
                If IsDBNull(dr("struserfirstname")) Then
                Else
                    lblEngName.Text = dr.Item("struserfirstname") & " " & dr.Item("struserlastname")
                End If
                If IsDBNull(dr("strphonenumber")) Then
                Else
                    lblEngPhone.Text = dr.Item("strphonenumber")
                End If
                If IsDBNull(dr("struseremail")) Then
                Else
                    lblEngEmail.NavigateUrl = "mailto:" & dr.Item("struseremail")
                    lblEngEmail.Text = dr.Item("struseremail")
                End If
                If IsDBNull(dr("datsentbyfacility")) Then
                    AddDateRow("Sent by Facility:", "N/A")
                Else
                    AddDateRow("Sent by Facility:", dr.Item("datsentbyfacility"))
                End If
                If IsDBNull(dr("datreceiveddate")) Then
                    AddDateRow("Received by Air Branch:", "N/A")
                Else
                    AddDateRow("Received by Air Branch:", dr.Item("datreceiveddate"))
                End If
                If IsDBNull(dr("datassignedtoengineer")) Then
                    AddDateRow("Assigned to Engineer:", "N/A")
                Else
                    AddDateRow("Assigned to Engineer:", dr.Item("datassignedtoengineer"))
                End If
                If IsDBNull(dr("datreassignedtoengineer")) Then
                Else
                    AddDateRow("Re-assigned to Engineer:", dr.Item("datreassignedtoengineer"))
                End If
                If IsDBNull(dr("datAcknowledgementLetterSent")) Then
                    AddDateRow("Acknowledgement Letter Sent:", "N/A")
                Else
                    AddDateRow("Acknowledgement Letter Sent:", dr.Item("datAcknowledgementLetterSent"))
                End If
                If IsDBNull(dr("strPublicInvolvement")) Then
                Else
                    temp = dr.Item("strPublicInvolvement")
                    Select Case temp
                        Case "0"
                            AddDateRow("Public Advisory (PA):", "Not Decided")
                        Case "1"
                            AddDateRow("Public Advisory (PA):", "PA Needed")
                        Case "2"
                            AddDateRow("Public Advisory (PA):", "PA Not Needed")
                        Case Else
                            AddDateRow("Public Advisory (PA):", "Not Decided")
                    End Select
                End If
                'If IsDBNull(dr.Item("strPAReady")) Then
                'Else
                '    If dr.Item("strPAReady") = "True" Then
                '        If IsDBNull(dr.Item("strPAposted")) Then
                '            AddDateRow("PA Ready:", "Yes")
                '        Else
                '            AddDateRow("PA Ready:", "Yes; " & dr.Item("strPAposted"))
                '        End If
                '    Else
                '        AddDateRow("PA Ready:", "No")
                '    End If
                'End If
                If IsDBNull(dr("datPAExpires")) Then
                Else
                    AddDateRow("PA Expires:", dr.Item("datPAExpires"))
                End If
                If IsDBNull(dr("datToPMI")) Then
                    AddDateRow("Sent to Unit Coordinator:", "N/A")
                Else
                    AddDateRow("Sent to Unit Coordinator:", dr.Item("datToPMI"))
                End If
                If IsDBNull(dr("datToPMII")) Then
                    AddDateRow("Sent to Program Manager:", "N/A")
                Else
                    AddDateRow("Sent to Program Manager:", dr.Item("datToPMII"))
                End If
                If IsDBNull(dr("datDraftIssued")) Then
                Else
                    AddDateRow("Draft Issued:", dr.Item("datDraftIssued"))
                End If
                'If IsDBNull(dr.Item("strPNReady")) Then
                'Else
                '    If dr.Item("strPNReady") = "True" Then
                '        If IsDBNull(dr.Item("strPNposted")) Then
                '            AddDateRow("Public Notice (PN) Ready:", "Yes")
                '        Else
                '            AddDateRow("Public Notice (PN) Ready:", "Yes; " & dr.Item("strPNposted"))
                '        End If
                '    Else
                '        AddDateRow("Public Notice (PN) Ready:", "No")
                '    End If
                'End If
                If IsDBNull(dr("datPNExpires")) Then
                Else
                    AddDateRow("PN Expires:", dr.Item("datPNExpires"))
                End If
                If IsDBNull(dr("datEPAWaived")) Then
                Else
                    AddDateRow("EPA 45-Day Waived:", dr.Item("datEPAWaived"))
                End If
                If IsDBNull(dr("datEPAEnds")) Then
                Else
                    AddDateRow("EPA 45-Day Ends:", dr.Item("datEPAEnds"))
                End If
                If IsDBNull(dr("datToBranchCheif")) Then
                    'AddDateRow("Sent to Branch Chief:", "N/A")
                Else
                    AddDateRow("Sent to Administrative Review:", dr.Item("datToBranchCheif"))
                End If
                'If IsDBNull(dr("datToDirector")) Then
                '    'AddDateRow("Sent to Director's Office:", "N/A")
                'Else
                '    AddDateRow("Sent to Administrative Review:", dr.Item("datToDirector"))
                'End If
                If IsDBNull(dr("datPermitIssued")) Then
                    AddDateRow("Final Action:", "N/A")
                Else
                    AddDateRow("Final Action:", dr.Item("datPermitIssued"))
                End If
                'If IsDBNull(dr("datApplicationDeadline")) Then
                'Else
                '    AddDateRow("Deadline:", dr.Item("datApplicationDeadline"))
                'End If
                If IsDBNull(dr("strSicCode")) Then
                Else
                    txtSICCode.Text = dr.Item("strSicCode")
                End If
                'If IsDBNull(dr("strOperationalStatus")) Then
                'Else
                '    temp = dr.Item("strOperationalStatus")
                '    Select Case temp
                '        Case "O"
                '            txtOpStatus.Text = "O - Operating"
                '        Case "P"
                '            txtOpStatus.Text = "P - Planned"
                '        Case "C"
                '            txtOpStatus.Text = "C - Under Construction"
                '        Case "T"
                '            txtOpStatus.Text = "T - Temporarily Closed"
                '        Case "X"
                '            txtOpStatus.Text = "X - Permanently Closed"
                '        Case "I"
                '            txtOpStatus.Text = "I - Seasonal Operation"
                '        Case Else
                '            txtOpStatus.Text = "U - Undefined"

                '    End Select
                'End If
                'If IsDBNull(dr("strClass")) Then
                'Else
                '    temp = dr.Item("strClass")
                '    Select Case temp
                '        Case "A"
                '            txtClass.Text = "A - MAJOR"
                '        Case "B"
                '            txtClass.Text = "B - MINOR"
                '        Case "C"
                '            txtClass.Text = "C - UNKNOWN"
                '        Case "SM"
                '            txtClass.Text = "SM - SYNTHETIC MINOR"
                '        Case "PR"
                '            txtClass.Text = "PR - PERMIT BY RULE"
                '        Case Else
                '            txtClass.Text = "U - Undefined"

                '    End Select
                'End If
                If IsDBNull(dr("strPlantDescription")) Then
                Else
                    txtDesc.Text = dr.Item("strPlantDescription")
                End If
                If IsDBNull(dr("strApplicationNotes")) Then
                Else
                    txtReason.Text = dr.Item("strApplicationNotes")
                End If
                'If IsDBNull(dr("strStateprogramcodes")) Then
                'Else
                '    Select Case Mid(dr.Item("strStateprogramcodes"), 1, 1)
                '        Case 0
                '            chbNSRMajor.Checked = False
                '        Case 1
                '            chbNSRMajor.Checked = True
                '        Case Else
                '            chbNSRMajor.Checked = False
                '    End Select
                '    Select Case Mid(dr.Item("strStateprogramcodes"), 2, 1)
                '        Case 0
                '            chbHAPsMajor.Checked = False
                '        Case 1
                '            chbHAPsMajor.Checked = True
                '        Case Else
                '            chbHAPsMajor.Checked = False
                '    End Select
                'End If
                If IsDBNull(dr.Item("strPermitNumber")) Then
                    lblPermitNo.Text = ""
                Else
                    If lblAplType.Text = "ERC" Then
                        Select Case Len(dr.Item("strPermitNumber"))
                            Case Is <= 3
                                lblPermitNo.Text = "ERC"
                            Case Is > 8
                                lblPermitNo.Text = Mid(dr.Item("strPermitNumber"), 1, 3) & "-" & Mid(dr.Item("strPermitNumber"), 4, 5) & "-" & Mid(dr.Item("strPermitNumber"), 9)
                            Case Is > 3 <= 8
                                lblPermitNo.Text = Mid(dr.Item("strPermitNumber"), 1, 3) & "-" & Mid(dr.Item("strPermitNumber"), 4)

                            Case Else
                                lblPermitNo.Text = dr.Item("strPermitNumber")
                        End Select
                    Else
                        Select Case Len(dr.Item("strPermitNumber"))
                            Case 15
                                lblPermitNo.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5, 3) &
                                "-" & Mid(dr.Item("strPermitNumber"), 8, 4) & "-" & Mid(dr.Item("strPermitNumber"), 12, 1) &
                                "-" & Mid(dr.Item("strPermitNumber"), 13, 2) & "-" & Mid(dr.Item("strPermitNumber"), 15, 1)
                            Case Is >= 13 = 14
                                lblPermitNo.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5, 3) _
                                & "-" & Mid(dr.Item("strPermitNumber"), 8, 4) & "-" & Mid(dr.Item("strPermitNumber"), 12, 1) _
                                & "-" & Mid(dr.Item("strPermitNumber"), 13)
                            Case 12
                                lblPermitNo.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5, 3) _
                                & "-" & Mid(dr.Item("strPermitNumber"), 8, 4) & "-" & Mid(dr.Item("strPermitNumber"), 12, 1)
                            Case Is >= 8 <= 11
                                lblPermitNo.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5, 3) _
                                & "-" & Mid(dr.Item("strPermitNumber"), 8)
                            Case Is >= 5 <= 7
                                lblPermitNo.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5)
                            Case Is <= 4
                                lblPermitNo.Text = Mid(dr.Item("strPermitNumber"), 1)
                            Case Else
                                lblPermitNo.Text = dr.Item("strPermitNumber")
                        End Select
                    End If
                End If
                'AddAirProgramCodes(dr.Item("strAIRProgramcodes"))

                'If IsDBNull(dr.Item("strPermitType")) Then
                '    cboPermitAction.SelectedIndex = 0
                'Else
                '    cboPermitAction.SelectedValue = dr.Item("strPermitType")
                '    If cboPermitAction.SelectedValue Is Nothing Then
                '        temp = dr.Item("strPermitType")
                '        Select Case temp
                '            Case 1
                '                cboPermitAction.Text = "Amendment"
                '            Case 3
                '                cboPermitAction.Text = "Draft"
                '            Case 4
                '                cboPermitAction.Text = "New Permit"
                '            Case 8
                '                cboPermitAction.Text = "PRMT-DNL"
                '            Case 12
                '                cboPermitAction.Text = "Initial Title V Permit"
                '            Case 13
                '                cboPermitAction.Text = "Renewal Title V Permit"
                '        End Select
                '    End If
                'End If

                'If IsDBNull(dr.Item("strPermitNumber")) Then
                '    txtPermitNumber.Clear()
                'Else
                '    If cboApplicationType.Text = "ERC" Then
                '        Select Case Len(dr.Item("strPermitNumber"))
                '            Case Is <= 3
                '                txtPermitNumber.Text = "ERC"
                '            Case Is > 8
                '                txtPermitNumber.Text = Mid(dr.Item("strPermitNumber"), 1, 3) & "-" & Mid(dr.Item("strPermitNumber"), 4, 5) & "-" & Mid(dr.Item("strPermitNumber"), 9)
                '            Case Is > 3 <= 8
                '                txtPermitNumber.Text = Mid(dr.Item("strPermitNumber"), 1, 3) & "-" & Mid(dr.Item("strPermitNumber"), 4)

                '            Case Else
                '                txtPermitNumber.Text = dr.Item("strPermitNumber")
                '        End Select
                '    Else
                '        Select Case Len(dr.Item("strPermitNumber"))
                '            Case 15
                '                txtPermitNumber.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5, 3) & _
                '                "-" & Mid(dr.Item("strPermitNumber"), 8, 4) & "-" & Mid(dr.Item("strPermitNumber"), 12, 1) & _
                '                "-" & Mid(dr.Item("strPermitNumber"), 13, 2) & "-" & Mid(dr.Item("strPermitNumber"), 15, 1)
                '            Case Is >= 13 = 14
                '                txtPermitNumber.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5, 3) _
                '                & "-" & Mid(dr.Item("strPermitNumber"), 8, 4) & "-" & Mid(dr.Item("strPermitNumber"), 12, 1) _
                '                & "-" & Mid(dr.Item("strPermitNumber"), 13)
                '            Case 12
                '                txtPermitNumber.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5, 3) _
                '                & "-" & Mid(dr.Item("strPermitNumber"), 8, 4) & "-" & Mid(dr.Item("strPermitNumber"), 12, 1)
                '            Case Is >= 8 <= 11
                '                txtPermitNumber.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5, 3) _
                '                & "-" & Mid(dr.Item("strPermitNumber"), 8)
                '            Case Is >= 5 <= 7
                '                txtPermitNumber.Text = Mid(dr.Item("strPermitNumber"), 1, 4) & "-" & Mid(dr.Item("strPermitNumber"), 5)
                '            Case Is <= 4
                '                txtPermitNumber.Text = Mid(dr.Item("strPermitNumber"), 1)
                '            Case Else
                '                txtPermitNumber.Text = dr.Item("strPermitNumber")
                '        End Select
                '    End If
                'End If

                'If IsDBNull(dr.Item("strPlantDescription")) Then
                '    txtPlantDescription.Clear()
                'Else
                '    txtPlantDescription.Text = dr.Item("strPlantDescription")
                'End If

                'If IsDBNull(dr.Item("strApplicationNotes")) Then
                '    txtReasonAppSubmitted.Clear()
                'Else
                '    txtReasonAppSubmitted.Text = dr.Item("strApplicationNotes")
                'End If

            Else
                Response.Write("No Data Available for this Application Number")
            End If

            If dr.IsClosed = False Then dr.Close()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub AddDateRow(ByVal label As String, ByVal lbldate As String)
        Dim tr As New HtmlTableRow()
        Dim td1 As New HtmlTableCell()
        ' Create a label control dynamically
        Dim label1 As New Label()
        label1.Text = label
        ' Add control to the table cell
        td1.Controls.Add(label1)

        ' Create column 2
        Dim td2 As New HtmlTableCell()
        Dim label2 As New Label()
        label2.Text = lbldate
        td2.Controls.Add(label2)
        td2.BgColor = "#CCCCCC"
        ' Add cell to the row
        tr.Cells.Add(td1)
        tr.Cells.Add(td2)
        ' Add row to the table.
        tblDates.Rows.Add(tr)
    End Sub

    Private Function GetApplicationType(ByVal AppTypeCode As Integer) As String
        Try
            Dim sql = "Select strapplicationtypedesc FROM lookupapplicationtypes " &
                "where strapplicationtypecode = '" & AppTypeCode.ToString & "'"
            Dim conn As New SqlConnection(oradb)
            Dim cmd = New SqlCommand(sql, conn)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim dr = cmd.ExecuteReader
            Dim recExist = dr.Read
            If recExist = True Then
                Return dr.Item("strapplicationtypedesc")
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ErrorReport(ex, False)
            Return Nothing
        End Try

    End Function

    Private Function GetAPBUnit(ByVal unitCode As String) As String
        Try
            Dim sql = "Select strUnitTitle FROM APBUnits " &
                "where strunit = '" & unitCode.ToString & "'"
            Dim conn As New SqlConnection(oradb)
            Dim cmd = New SqlCommand(sql, conn)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim dr = cmd.ExecuteReader
            Dim recExist = dr.Read
            If recExist = True Then
                Return dr.Item("strUnitTitle")
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ErrorReport(ex, False)
            Return Nothing
        End Try
    End Function

    Private Function GetPermitType(ByVal permitCode As String) As String
        Try
            Dim sql = "Select strpermittypedescription FROM lookuppermittypes " &
                "where strpermittypecode = '" & permitCode.ToString & "'"
            Dim conn As New SqlConnection(oradb)
            Dim cmd = New SqlCommand(sql, conn)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim dr = cmd.ExecuteReader
            Dim recExist = dr.Read
            If recExist = True Then
                Return dr.Item("strpermittypedescription")
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ErrorReport(ex, False)
            Return Nothing
        End Try
    End Function

    Protected Sub LoadCountyDetails()
        Dim CountyCode = Mid(lblAirs.Text, 1, 3)
        Dim attainment As String = "00000"
        Try
            Dim sql = "Select strCountyName, strOfficeName, strDistrictName, strnonattainment " &
                    "FROM LookUpCountyInformation, LooKUPDistrictOffice, " &
                    "LookUpDistrictInformation, LookUPDistricts " &
                    "where " &
                    "LookUpCountyInformation.strCountyCode = LookUpDistrictInformation.strDistrictCounty " &
                    "and LookUpDistrictinformation.strDistrictCode = LooKUPDistrictOffice.strDistrictCode " &
                    "and LookUpDistrictInformation.strDistrictCode = LookUPDistricts.strDistrictCode " &
                    "and LookUpCountyInformation.strCountyCode = '" & CountyCode.ToString & "'"

            Dim conn As New SqlConnection(oradb)
            Dim cmd = New SqlCommand(sql, conn)
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If
            Dim dr = cmd.ExecuteReader
            Dim recExist = dr.Read
            If recExist = True Then
                lblcounty.Text = dr.Item("strcountyname")
                attainment = dr.Item("strnonattainment")
                lblDistrict.Text = dr.Item("strDistrictName")
            End If

            If dr.IsClosed = False Then dr.Close()
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If

            'Select Case Mid(attainment, 2, 1)
            '    Case 0
            '        txt1HourOzone.Text = "No"
            '    Case 1
            '        txt1HourOzone.Text = "Yes"
            '    Case 2
            '        txt1HourOzone.Text = "Contributing"
            '    Case Else
            '        txt1HourOzone.Text = "No"
            'End Select
            'Select Case Mid(attainment, 3, 1)
            '    Case 0
            '        txt8HROzone.Text = "No"
            '    Case 1
            '        txt8HROzone.Text = "Atlanta"
            '    Case 2
            '        txt8HROzone.Text = "Macon"
            '    Case Else
            '        txt8HROzone.Text = "No"
            'End Select
            'Select Case Mid(attainment, 4, 1)
            '    Case 0
            '        txtPM.Text = "No"
            '    Case 1
            '        txtPM.Text = "Atlanta"
            '    Case 2
            '        txtPM.Text = "Chattanooga"
            '    Case 3
            '        txtPM.Text = "Floyd"
            '    Case 4
            '        txtPM.Text = "Macon"
            '    Case Else
            '        txtPM.Text = "No"
            'End Select

        Catch exThreadAbort As System.Threading.ThreadAbortException
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

End Class