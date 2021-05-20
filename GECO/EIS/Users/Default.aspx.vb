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

        Master.Master.ClearDefaultButton()

        If Not IsPostBack Then
            LoadStates()
            LoadCurrentUsers()
        End If
    End Sub

    Private Sub LoadCurrentUsers()
        Dim dt As DataTable = GetFacilityCaerContacts(CurrentAirs)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            hidCertifiersCount.Value = dt.Select($"CaerRole = '{CaerRole.Certifier}'").Length.ToString
            hidPreparersCount.Value = dt.Select($"CaerRole = '{CaerRole.Preparer}'").Length.ToString

            grdCaersUsers.Visible = True
            grdCaersUsers.DataSource = dt
            grdCaersUsers.DataBind()
            pNoUsersNotice.Visible = False

            pAddNew.Visible = True
            btnCancelNew.Visible = True
            pnlAddNew.Visible = False
        Else
            hidCertifiersCount.Value = "0"
            hidPreparersCount.Value = "0"

            grdCaersUsers.DataSource = Nothing
            grdCaersUsers.Visible = False
            pNoUsersNotice.Visible = True

            pAddNew.Visible = False
            btnCancelNew.Visible = False
            pnlAddNew.Visible = True
        End If

        rRoleNew.Visible = CInt(hidCertifiersCount.Value) = 0
        reqvRoleNew.Visible = CInt(hidCertifiersCount.Value) = 0
        rRolePreparer.Visible = CInt(hidCertifiersCount.Value) > 0
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
        pAddNew.Visible = False
        pnlAddNew.Visible = True
        pnlEditUser.Visible = False
        btnProceed.Visible = False
    End Sub

    Private Sub CancelNewUser_Click(sender As Object, e As EventArgs) Handles btnCancelNew.Click
        pAddNew.Visible = True
        pnlAddNew.Visible = False
        btnProceed.Visible = True
    End Sub

    Private Sub btnSaveNew_Click(sender As Object, e As EventArgs) Handles btnSaveNew.Click
        If Page.IsValid Then
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

            pAddNew.Visible = True
            pnlAddNew.Visible = False
            btnProceed.Visible = True

            LoadCurrentUsers()
        End If
    End Sub

    Private Sub custEmailNew_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custEmailNew.ServerValidate
        args.IsValid = rRoleNew.SelectedValue = CaerRole.Certifier.ToString OrElse Not CaerPreparerExists(args.Value, CurrentAirs)
    End Sub

    ' Edit existing user

    Private Sub grdCaersUsers_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdCaersUsers.RowEditing
        NotNull(e, NameOf(e))

        Dim userId As Guid = CType(grdCaersUsers.DataKeys(e.NewEditIndex).Item("Id"), Guid)
        Dim caerUser As CaerContact = GetCaerContact(userId)

        If caerUser IsNot Nothing AndAlso caerUser.Active Then
            pnlEditUser.Visible = True
            pnlAddNew.Visible = False
            hidEditId.Value = userId.ToString
            pAddNew.Visible = False
            btnProceed.Visible = False

            txtStreetEdit.Text = caerUser.Contact.Address.Street
            txtStreet2Edit.Text = caerUser.Contact.Address.Street2
            txtCityEdit.Text = caerUser.Contact.Address.City
            ddlStateEdit.SelectedValue = caerUser.Contact.Address.State
            txtPostalCodeEdit.Text = caerUser.Contact.Address.PostalCode
            txtCompanyEdit.Text = caerUser.Contact.Company
            txtEmailEdit.Text = caerUser.Contact.Email
            txtPrefixEdit.Text = caerUser.Contact.Honorific
            txtFirstNameEdit.Text = caerUser.Contact.FirstName
            txtLastNameEdit.Text = caerUser.Contact.LastName
            txtTelephoneEdit.Text = caerUser.Contact.PhoneNumber
            txtTitleEdit.Text = caerUser.Contact.Title

            ddlRoleEdit.Items.Clear()
            ddlRoleEdit.Items.Add(CaerRole.Preparer.ToString)
            If CInt(hidCertifiersCount.Value) = 0 OrElse caerUser.CaerRole = CaerRole.Certifier Then
                ddlRoleEdit.Items.Add(CaerRole.Certifier.ToString)
                ddlRoleEdit.Enabled = True
            Else
                ddlRoleEdit.Enabled = False
            End If
            ddlRoleEdit.SelectedValue = caerUser.CaerRole.ToString()
        End If
    End Sub

    Private Sub btnCancelEdit_Click(sender As Object, e As EventArgs) Handles btnCancelEdit.Click
        pnlEditUser.Visible = False
        pAddNew.Visible = True
        btnProceed.Visible = True
    End Sub

    Private Sub btnSaveEdit_Click(sender As Object, e As EventArgs) Handles btnSaveEdit.Click
        If Page.IsValid Then
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

            Dim role As CaerRole = CType([Enum].Parse(GetType(CaerRole), ddlRoleEdit.SelectedValue), CaerRole)

            Dim caerContact As New CaerContact With {
                .Active = True,
                .CaerRole = role,
                .Contact = contact,
                .FacilitySiteId = CurrentAirs
            }

            UpdateCaerContact(caerContact, New Guid(hidEditId.Value))

            pAddNew.Visible = True
            pnlAddNew.Visible = False
            pnlEditUser.Visible = False
            btnProceed.Visible = True

            LoadCurrentUsers()
        End If
    End Sub

    Private Sub custEmailEdit_ServerValidate(source As Object, args As ServerValidateEventArgs) Handles custEmailEdit.ServerValidate
        args.IsValid = Not (ddlRoleEdit.SelectedValue = CaerRole.Preparer.ToString AndAlso
            CaerPreparerExists(args.Value, CurrentAirs, New Guid(hidEditId.Value)))
    End Sub

    Private Sub btnProceed_Click(sender As Object, e As EventArgs) Handles btnProceed.Click
        If CInt(hidCertifiersCount.Value) = 1 AndAlso CInt(hidPreparersCount.Value) >= 1 Then
            Response.Redirect("~/EIS/Process/Submit.aspx")
        End If
    End Sub

End Class
