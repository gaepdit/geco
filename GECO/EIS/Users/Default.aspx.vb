Imports GECO.GecoModels.EIS
Imports GECO.GecoModels
Imports GECO.DAL.EIS

Public Class EIS_Users_Default
    Inherits Page

    Public Property CurrentAirs As ApbFacilityId
    Public Property IsBeginEisProcess As Boolean = False

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.Users

        If Session("EisProcessStarted") IsNot Nothing Then
            If Session("EisProcess") Is Nothing Then
                Response.Redirect("~/EIS/")
            End If

            IsBeginEisProcess = True
            Master.IsBeginEisProcess = True
        End If

        Dim eiStatus As EisStatus = GetEiStatus(CurrentAirs)

        If eiStatus.AccessCode > 1 Then
            pAddNew.Visible = False
            grdCaersUsers.Columns.RemoveAt(grdCaersUsers.Columns.Count - 1)
        End If

        Master.Master.ClearDefaultButton()

        If Not IsPostBack Then
            LoadStates()
            LoadCurrentUsers()
        End If
    End Sub

    Private Sub LoadCurrentUsers()
        Dim dt As DataTable = GetFacilityCaerContacts(CurrentAirs)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            hidCertifiersCount.Value = dt.Select($"CaerRole = '{CaerRole.Certifier}'").Length
            hidPreparersCount.Value = dt.Select($"CaerRole = '{CaerRole.Preparer}'").Length

            grdCaersUsers.Visible = True
            grdCaersUsers.DataSource = dt
            grdCaersUsers.DataBind()
            pNoUsersNotice.Visible = False

            btnAddNew.Visible = True
            btnCancelNew.Visible = True
            pnlAddNew.Visible = False
        Else
            hidCertifiersCount.Value = 0
            hidPreparersCount.Value = 0

            grdCaersUsers.DataSource = Nothing
            grdCaersUsers.Visible = False
            pNoUsersNotice.Visible = True

            btnAddNew.Visible = False
            btnCancelNew.Visible = False
            pnlAddNew.Visible = True
        End If
    End Sub

    Private Sub LoadStates()
        ddlStateNew.Items.Add("--Select a State--")
        ddlStateEdit.Items.Add("--Select a State--")

        Dim query As String = "Select strState, strAbbrev FROM LookUpStates order by strAbbrev"
        Dim dt As DataTable = DB.GetDataTable(query)

        For Each dr As DataRow In dt.Rows
            Dim newListItem As New ListItem With {
                .Text = dr.Item("strState").ToString,
                .Value = dr.Item("strAbbrev").ToString
            }
            ddlStateNew.Items.Add(newListItem)
            ddlStateEdit.Items.Add(newListItem)
        Next
    End Sub

    ' Delete existing user

    Private Sub grdCaersUsers_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdCaersUsers.RowDeleting
        NotNull(e, NameOf(e))
        Dim userid As Guid = Guid.Parse(grdCaersUsers.DataKeys(e.RowIndex).Values("Id").ToString())
        DeleteCaerContact(userid)
        LoadCurrentUsers()
    End Sub

    ' Add new user

    Private Sub btnAddNew_Click(sender As Object, e As EventArgs) Handles btnAddNew.Click
        btnAddNew.Visible = False
        pnlAddNew.Visible = True
        pnlEditUser.Visible = False
        btnProceed.Visible = False
    End Sub

    Private Sub CancelNewUser_Click(sender As Object, e As EventArgs) Handles btnCancelNew.Click
        btnAddNew.Visible = True
        pnlAddNew.Visible = False
        btnProceed.Visible = True
    End Sub

    Private Sub btnSaveNew_Click(sender As Object, e As EventArgs) Handles btnSaveNew.Click
        Dim address As New Address With {
            .Street = txtStreetNew.Text,
            .Street2 = txtStreet2New.Text,
            .City = txtCityNew.Text,
            .State = ddlStateNew.SelectedValue,
            .PostalCode = txtPostalCodeNew.Text
        }

        Dim contact As New Person With {
            .Address = address,
            .Company = txtCompanyNew.Text,
            .Email = txtEmailNew.Text,
            .Honorific = txtPrefixNew.Text,
            .FirstName = txtFirstNameNew.Text,
            .LastName = txtLastNameNew.Text,
            .PhoneNumber = txtTelephoneNew.Text,
            .Title = txtTitleNew.Text
        }

        Dim caerContact As New CaerContact With {
            .Active = True,
            .Contact = contact,
            .FacilitySiteId = CurrentAirs
        }

        Select Case rRoleNew.SelectedValue
            Case CaerRole.Certifier.ToString
                caerContact.CaerRole = CaerRole.Certifier
                SaveCaerContact(caerContact)
            Case "Both"
                caerContact.CaerRole = CaerRole.Preparer
                SaveCaerContact(caerContact)
                caerContact.CaerRole = CaerRole.Certifier
                SaveCaerContact(caerContact)
            Case Else
                caerContact.CaerRole = CaerRole.Preparer
                SaveCaerContact(caerContact)
        End Select

        btnAddNew.Visible = True
        pnlAddNew.Visible = False
        btnProceed.Visible = True

        LoadCurrentUsers()
    End Sub

    ' Edit existing user

    Private Sub grdCaersUsers_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdCaersUsers.RowEditing
        NotNull(e, NameOf(e))

        Dim id = grdCaersUsers.DataKeys(e.NewEditIndex).Item("Id")
        Dim user As CaerContact = GetCaerContact(id)

        If user IsNot Nothing AndAlso user.Active Then
            pnlEditUser.Visible = True
            pnlAddNew.Visible = False
            hidEditId.Value = id.ToString
            btnAddNew.Visible = False
            btnProceed.Visible = False

            txtStreetEdit.Text = user.Contact.Address.Street
            txtStreet2Edit.Text = user.Contact.Address.Street2
            txtCityEdit.Text = user.Contact.Address.City
            ddlStateEdit.SelectedValue = user.Contact.Address.State
            txtPostalCodeEdit.Text = user.Contact.Address.PostalCode
            txtCompanyEdit.Text = user.Contact.Company
            txtEmailEdit.Text = user.Contact.Email
            txtPrefixEdit.Text = user.Contact.Honorific
            txtFirstNameEdit.Text = user.Contact.FirstName
            txtLastNameEdit.Text = user.Contact.LastName
            txtTelephoneEdit.Text = user.Contact.PhoneNumber
            txtTitleEdit.Text = user.Contact.Title

            ddlRoleEdit.Items.Clear()
            ddlRoleEdit.Items.Add(CaerRole.Preparer.ToString)
            If hidCertifiersCount.Value = 0 OrElse user.CaerRole = CaerRole.Certifier Then
                ddlRoleEdit.Items.Add(CaerRole.Certifier.ToString)
                ddlRoleEdit.Enabled = True
            Else
                ddlRoleEdit.Enabled = False
            End If
            ddlRoleEdit.SelectedValue = user.CaerRole.ToString()
        End If
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object, e As EventArgs) Handles btnCancelEdit.Click
        pnlEditUser.Visible = False

        If pAddNew.Visible Then
            btnAddNew.Visible = True
            btnProceed.Visible = True
        End If
    End Sub

    Private Sub btnSaveEdit_Click(sender As Object, e As EventArgs) Handles btnSaveEdit.Click
        Dim address As New Address With {
            .Street = txtStreetEdit.Text,
            .Street2 = txtStreet2Edit.Text,
            .City = txtCityEdit.Text,
            .State = ddlStateEdit.SelectedValue,
            .PostalCode = txtPostalCodeEdit.Text
        }

        Dim contact As New Person With {
            .Address = address,
            .Company = txtCompanyEdit.Text,
            .Email = txtEmailEdit.Text,
            .Honorific = txtPrefixEdit.Text,
            .FirstName = txtFirstNameEdit.Text,
            .LastName = txtLastNameEdit.Text,
            .PhoneNumber = txtTelephoneEdit.Text,
            .Title = txtTitleEdit.Text
        }

        Dim role As CaerRole = [Enum].Parse(GetType(CaerRole), ddlRoleEdit.SelectedValue)

        Dim caerContact As New CaerContact With {
            .Active = True,
            .CaerRole = role,
            .Contact = contact,
            .FacilitySiteId = CurrentAirs
        }

        UpdateCaerContact(caerContact, New Guid(hidEditId.Value))

        If pAddNew.Visible Then
            btnAddNew.Visible = True
        End If

        pnlAddNew.Visible = False
        pnlEditUser.Visible = False
        btnProceed.Visible = True

        LoadCurrentUsers()
    End Sub

    Private Sub btnProceed_Click(sender As Object, e As EventArgs) Handles btnProceed.Click
        If hidCertifiersCount.Value = 1 AndAlso hidPreparersCount.Value >= 1 Then
            Response.Redirect("~/EIS/Process/Submit.aspx")
        End If
    End Sub

End Class
