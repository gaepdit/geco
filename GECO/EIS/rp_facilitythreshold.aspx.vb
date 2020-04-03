Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels

Partial Class EIS_rp_threshold
    Inherits Page

    Private SummerDayRequired As Boolean
    Private EIType As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim EISAccessCode As String = GetCookie(EisCookie.EISAccess)
        Dim EIYear As String = GetCookie(EisCookie.EISMaxYear)

        FIAccessCheck(EISAccessCode)

        If Not IsPostBack Then
            SummerDayRequired = CheckSummerDayRequired(FacilitySiteID)
            EIType = GetEIType(EIYear)
            If SummerDayRequired Then
                lblLocation.Text = "located in the ozone nonattainment area."
            Else
                lblLocation.Text = "not located in the ozone nonattainment area."
            End If

            LoadpageInfo(EIType)
            txtComment.Text = GetAdminComment(FacilitySiteID, EIYear)
        End If

    End Sub

    Private Sub LoadpageInfo(eit As String)

        Dim Pollutant As String
        Dim Threshold As String
        Dim ThresholdNAA As String

        Try
            Dim query = "select " &
                "strPollutant, " &
                "numThreshold, " &
                "numThresholdNAA " &
                "from " &
                "eiThresholds " &
                "where " &
                "strType = @eit"

            Dim param As New SqlParameter("@eit", eit)

            Dim dt = DB.GetDataTable(query, param)

            If dt IsNot Nothing Then
                For Each dr As DataRow In dt.Rows

                    'Load values from table
                    Pollutant = GetNullableString(dr.Item("strPollutant"))
                    Threshold = GetNullable(Of Decimal)(dr.Item("numThreshold"))
                    ThresholdNAA = GetNullable(Of Decimal)(dr.Item("numThresholdNAA"))

                    'Populate page info
                    Select Case Pollutant
                        Case "VOC"
                            If SummerDayRequired Then
                                lblVOCThreshold.Text = ThresholdNAA
                            Else
                                lblVOCThreshold.Text = Threshold
                            End If
                        Case "SOx"
                            If SummerDayRequired Then
                                lblSOxThreshold.Text = ThresholdNAA
                            Else
                                lblSOxThreshold.Text = Threshold
                            End If
                        Case "NOx"
                            If SummerDayRequired Then
                                lblNOxThreshold.Text = ThresholdNAA
                            Else
                                lblNOxThreshold.Text = Threshold
                            End If
                        Case "CO"
                            If SummerDayRequired Then
                                lblCOThreshold.Text = ThresholdNAA
                            Else
                                lblCOThreshold.Text = Threshold
                            End If
                        Case "PM10"
                            If SummerDayRequired Then
                                lblPM10Threshold.Text = ThresholdNAA
                            Else
                                lblPM10Threshold.Text = Threshold
                            End If
                        Case "PM25"
                            If SummerDayRequired Then
                                lblPM25Threshold.Text = ThresholdNAA
                            Else
                                lblPM25Threshold.Text = Threshold
                            End If
                        Case "NH3"
                            If SummerDayRequired Then
                                lblNH3Threshold.Text = ThresholdNAA
                            Else
                                lblNH3Threshold.Text = Threshold
                            End If
                        Case "PB"
                            If eit = "ANNUAL" Then
                                lblPb.Text = ""
                                lblPbThreshold.Text = ""
                                rblPb.Visible = False
                                rblPb.Enabled = False
                                reqvPb.Enabled = False
                                tblThreshold.Rows.RemoveAt(8)
                            Else
                                If SummerDayRequired Then
                                    lblPbThreshold.Text = ThresholdNAA
                                Else
                                    lblPbThreshold.Text = Threshold
                                End If
                            End If
                    End Select

                Next
            End If
        Catch ex As Exception
            ErrorReport(ex)
        End Try
    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        Dim FacilitySiteID As String = GetCookie(Cookie.AirsNumber)
        Dim airs As New ApbFacilityId(FacilitySiteID)
        Dim currentUser = GetCurrentUser()
        Dim eiYear As String = GetCookie(EisCookie.EISMaxYear)

        Dim SOX As String = rblSOx.SelectedValue
        Dim VOC As String = rblVOC.SelectedValue
        Dim NOX As String = rblNOx.SelectedValue
        Dim CO As String = rblCO.SelectedValue
        Dim PM10 As String = rblPM10.SelectedValue
        Dim PM25 As String = rblPM25.SelectedValue
        Dim NH3 As String = rblNH3.SelectedValue
        Dim Pb As String = rblPb.SelectedValue

        EIType = GetEIType(eiYear)

        SaveAdminComment(airs, eiYear, txtComment.Text)

        If EIType = "ANNUAL" Then
            Pb = "Yes"
        End If

        If SOX = "Yes" AndAlso VOC = "Yes" AndAlso NOX = "Yes" AndAlso CO = "Yes" AndAlso
            PM10 = "Yes" AndAlso PM25 = "Yes" AndAlso NH3 = "Yes" AndAlso Pb = "Yes" Then

            'Facility will be opted out of the EI
            Dim colocated As Boolean = (rblIsColocated.SelectedValue = "Yes")
            Dim colocation As String = Nothing

            If rblIsColocated.SelectedValue = "Yes" Then
                colocation = txtColocatedWith.Text
            End If

            SaveEisOptOut(airs, True, currentUser.DbUpdateUser, eiYear, "2", colocated, colocation)
        Else
            'Facility will be opted into the EI
            SaveEisOptOut(airs, False, currentUser.DbUpdateUser, eiYear)
        End If

        LoadEiStatusCookies(airs, Response)
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub btnContinue_Click(sender As Object, e As EventArgs) Handles btnContinue.Click

        Dim eiYear As String = GetCookie(EisCookie.EISMaxYear)
        Dim SOX As String = rblSOx.SelectedValue
        Dim VOC As String = rblVOC.SelectedValue
        Dim NOX As String = rblNOx.SelectedValue
        Dim CO As String = rblCO.SelectedValue
        Dim PM10 As String = rblPM10.SelectedValue
        Dim PM25 As String = rblPM25.SelectedValue
        Dim NH3 As String = rblNH3.SelectedValue
        Dim Pb As String

        EIType = GetEIType(eiYear)

        If EIType = "ANNUAL" Then
            Pb = "Yes"
            rblPb.Enabled = False
            tblThreshold.Rows.RemoveAt(8)
        Else
            Pb = rblPb.SelectedValue
        End If

        DisableRbls()

        If SOX = "Yes" AndAlso VOC = "Yes" AndAlso NOX = "Yes" AndAlso CO = "Yes" AndAlso
            PM10 = "Yes" AndAlso PM25 = "Yes" AndAlso NH3 = "Yes" AndAlso Pb = "Yes" Then
            'Facility will be opted out of the EI - show opt OUT status message and submit button
            lblOptOutStatus1.Text = "The facility will not participate in the Emissions Inventory process for " & eiYear & "."
            pnlColocate.Visible = True
        Else
            'Facility will be opted into the EI - show opt IN status message and submit button
            lblOptOutStatus1.Text = "The facility will participate in the Emissions Inventory process for " & eiYear & "."
            pnlColocate.Visible = False
        End If

        dOptOut.Visible = True
        lblNextButton.Text = "Submit"

        btnContinue.Visible = False
        btnSubmit.Visible = True
        btnCancel.Visible = True

    End Sub

    Private Sub DisableRbls()

        rblSOx.Enabled = False
        rblVOC.Enabled = False
        rblNOx.Enabled = False
        rblCO.Enabled = False
        rblPM10.Enabled = False
        rblPM25.Enabled = False
        rblNH3.Enabled = False
        rblPb.Enabled = False

    End Sub

    Private Sub EnableRbls()

        rblSOx.Enabled = True
        rblVOC.Enabled = True
        rblNOx.Enabled = True
        rblCO.Enabled = True
        rblPM10.Enabled = True
        rblPM25.Enabled = True
        rblNH3.Enabled = True
        rblPb.Enabled = True

    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Dim eiYear As String = GetCookie(EisCookie.EISMaxYear)

        EIType = GetEIType(eiYear)

        If EIType = "ANNUAL" Then
            rblPb.Enabled = False
            tblThreshold.Rows.RemoveAt(8)
        End If

        EnableRbls()
        dOptOut.Visible = False
        lblNextButton.Text = "Continue"

        btnCancel.Visible = False
        btnSubmit.Visible = False
        btnContinue.Visible = True

    End Sub

    Private Sub rblIsColocated_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblIsColocated.SelectedIndexChanged

        pnlColocation.Visible = (rblIsColocated.SelectedValue = "Yes")

    End Sub

End Class
