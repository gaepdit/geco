﻿Imports GECO.GecoModels

Partial Class Home
    Inherits Page

    Private Property currentUser As GecoUser

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        currentUser = GetCurrentUser()

        If Not IsPostBack Then
            LoadAccessTable()
            LoadYearLabels()

            If currentUser.ProfileUpdateRequired Then
                pUpdateRequired.Visible = True
            End If
        End If
    End Sub

    Private Sub LoadYearLabels()
        lblEIYear2.Text = CStr(Now.Year - 1) 'This is the EI calendar year
        lblEIYear3.Text = CStr(Now.Year - 1) 'This is the EI calendar year
        lblEIYear4.Text = CStr(Now.Year) 'This is the EI due date
        lblEIYear5.Text = CStr(Now.Year - 1) 'This is the EI calendar year
        lblEIYear6.Text = CStr(Now.Year - 1) 'This is the EI calendar year

        lblESYear1.Text = CStr(Now.Year - 1) 'This is the ES calendar year
        lblESYear2.Text = CStr(Now.Year - 1) 'This is the ES calendar year
        lblESYear3.Text = CStr(Now.Year) 'This is the ES due date

        lblFeeYear1.Text = CStr(Now.Year - 1) ' Fee Calendar year
        lblFeeYear2.Text = CStr(Now.Year - 1) ' Fee Calendar year
        lblFeeYear3.Text = CStr(Now.Year) ' Fees dues year
        lblFeeYear4.Text = CStr(Now.Year) ' Fees dues year

        If Now.Year Mod 3 = 2 Then
            lblTriennialEIText.Visible = True
            lblAnnualEIText.Visible = False
        End If
    End Sub

    Private Sub LoadAccessTable()
        Dim dtAccess As DataTable = currentUser.FacilityAccessTable

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

    Protected Sub grdAccess_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdAccess.RowDataBound
        NotNull(e, NameOf(e))

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As DataRowView = CType(e.Row.DataItem, DataRowView)
            Dim facilityName As String = row.Item("Facility").ToString()
            Dim airsNumber As ApbFacilityId = New ApbFacilityId(row.Item("AirsNumber").ToString)
            Dim url As String = String.Format("~/Facility/?airs={0}", airsNumber.ShortString())

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
