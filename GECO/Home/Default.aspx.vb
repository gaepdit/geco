﻿Imports System.DateTime
Imports GECO.GecoModels

Partial Class Home
    Inherits Page

    Private Property CurrentUser As GecoUser

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        CurrentUser = GetCurrentUser()

        If Not IsPostBack Then
            LoadAccessTable()
            LoadYearLabels()

            If CurrentUser.ProfileUpdateRequired Then
                pUpdateRequired.Visible = True
            End If
        End If
    End Sub

    Private Sub LoadYearLabels()
        Dim eiCurrentYear = DAL.EIS.GetCurrentEiYear()
        Dim eiCurrentYearString = eiCurrentYear.ToString()

        lblEIYear2.Text = eiCurrentYearString  'This is the EI reporting year
        lblEIYear3.Text = eiCurrentYearString  'This is the EI reporting year
        lblEIYear4.Text = (eiCurrentYear + 1).ToString 'This is the EI due date year
        lblEIYear5.Text = eiCurrentYearString  'This is the EI reporting year
        lblEIYear6.Text = eiCurrentYearString  'This is the EI reporting year

        Dim feeDueYear As String = CStr(Now.Year)
        Dim feeCalendarYear As String = CStr(Now.Year - 1)

        lblFeeYear1.Text = feeCalendarYear ' Fee Calendar year
        lblFeeYear2.Text = feeCalendarYear ' Fee Calendar year
        lblFeeYear3.Text = feeDueYear ' Fees dues year
        lblFeeYear4.Text = feeDueYear ' Fees dues year

        If eiCurrentYear Mod 3 = 1 Then
            lblTriennialEIText.Visible = True
            lblAnnualEIText.Visible = False
        End If
    End Sub

    Private Sub LoadAccessTable()
        Dim dtAccess As DataTable = CurrentUser.FacilityAccessTable

        If dtAccess Is Nothing OrElse dtAccess.Rows.Count = 0 Then
            'This user has NO Facility assigned
            lblNone.Visible = True
            lblAccess.Visible = False
            grdAccess.Visible = False
        Else
            lblNone.Visible = False
            lblAccess.Visible = True
            grdAccess.Visible = True
            grdAccess.DataSource = dtAccess
            grdAccess.DataBind()
        End If
    End Sub

    Private Sub grdAccess_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdAccess.RowDataBound
        NotNull(e, NameOf(e))

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim facilityName As String = row.Item("Facility").ToString()
            Dim airsNumber As New ApbFacilityId(row.Item("AirsNumber").ToString)
            Dim url As String = $"~/Facility/?airs={airsNumber.ShortString()}"

            Dim hlFacility As HyperLink = CType(e.Row.FindControl("hlFacility"), HyperLink)
            If hlFacility IsNot Nothing Then
                hlFacility.Text = facilityName
                hlFacility.NavigateUrl = url
            End If

            Dim hlAirs As HyperLink = CType(e.Row.FindControl("hlAirs"), HyperLink)
            If hlAirs IsNot Nothing Then
                hlAirs.Text = airsNumber.FormattedString
                hlAirs.NavigateUrl = url
            End If
        End If
    End Sub
End Class
