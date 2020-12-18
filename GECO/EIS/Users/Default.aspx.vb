Imports GECO.GecoModels.EIS
Imports GECO.GecoModels
Imports GECO.DAL.EIS


Public Class EIS_Users_Default
    Inherits Page

    Public Property CurrentAirs As ApbFacilityId

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        Dim airs As String = GetCookie(Cookie.AirsNumber)

        If String.IsNullOrEmpty(airs) Then
            Response.Redirect("~/")
        End If

        CurrentAirs = New ApbFacilityId(airs)
        Master.CurrentAirs = CurrentAirs
        Master.SelectedTab = EIS.EisTab.Users

        Dim eiStatus As EiStatus = GetEiStatus(CurrentAirs)

        If eiStatus.AccessCode > 1 Then
            pAddNew.Visible = False
            grdCaersUsers.Columns.RemoveAt(grdCaersUsers.Columns.Count - 1)
        End If

        If Not IsPostBack Then
            LoadDropdownLists()
            LoadCurrentUsers()
        End If
    End Sub

    Private Sub LoadCurrentUsers()
        Dim dt As DataTable = GetCaerContacts(CurrentAirs)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            grdCaersUsers.DataSource = dt
            pNoUsersNotice.Visible = False
        Else
            grdCaersUsers.DataSource = Nothing
            grdCaersUsers.Visible = False
            pNoUsersNotice.Visible = True
        End If

        grdCaersUsers.DataBind()

        If pAddNew.Visible Then
            Master.Master.SetDefaultButton(btnAddNew)
        Else
            Master.Master.ClearDefaultButton()
        End If
    End Sub

    Private Sub LoadDropdownLists()
        LoadStates()
        LoadRoles()
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

    Private Sub LoadRoles()
        ddlRoleNew.Items.Add("Preparer")
        ddlRoleNew.Items.Add("Certifier")
        ddlRoleEdit.Items.Add("Preparer")
        ddlRoleEdit.Items.Add("Certifier")
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

        Master.Master.SetDefaultButton(btnSaveNew)
    End Sub

    Private Sub CancelNewUser_Click(sender As Object, e As EventArgs) Handles btnCancelNew.Click
        btnAddNew.Visible = True
        pnlAddNew.Visible = False
        Master.Master.SetDefaultButton(btnAddNew)
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

        Dim role As CaerRole = [Enum].Parse(GetType(CaerRole), ddlRoleNew.SelectedValue)

        Dim caerContact As New CaerContact With {
            .Active = True,
            .CaerRole = role,
            .Contact = contact,
            .FacilitySiteId = CurrentAirs
        }

        SaveCaerContact(caerContact)

        btnAddNew.Visible = True
        pnlAddNew.Visible = False
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

            ddlRoleEdit.SelectedValue = user.CaerRole.ToString()

            hidEditId.Value = id.ToString

            btnAddNew.Visible = False
            Master.Master.SetDefaultButton(btnSaveEdit)
        End If
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object, e As EventArgs) Handles btnCancelEdit.Click
        pnlEditUser.Visible = False

        If pAddNew.Visible Then
            btnAddNew.Visible = True
            Master.Master.SetDefaultButton(btnAddNew)
        Else
            Master.Master.ClearDefaultButton()
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
        LoadCurrentUsers()
    End Sub

End Class
