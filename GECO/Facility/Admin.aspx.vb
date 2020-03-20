Imports GECO.GecoModels

Partial Class FacilityAdmin
    Inherits Page

    Private Property currentUser As GecoUser
    Private Property facilityAccess As FacilityAccess
    Private Property currentAirs As ApbFacilityId
    Private Property currentFacility As String = Nothing

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
            Master.currentAirs = currentAirs
            SetCookie(Cookie.AirsNumber, currentAirs.ShortString())
        End If

        MainLoginCheck(Page.ResolveUrl("~/Facility/Admin.aspx?airs=" & currentAirs.ShortString))

        ' Current user and facility access
        currentUser = GetCurrentUser()

        facilityAccess = currentUser.GetFacilityAccess(currentAirs)

        If facilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Facility/")
        End If

        If Not IsPostBack Then
            LoadFacilityInfo()
            LoadUserGrid()

            Master.SetFacility(ConcatNonEmptyStrings(", ", {currentAirs.FormattedString(), currentFacility}))

            Title = "GECO Facility Admin - " & lblFacilityDisplay.Text
            lblAIRS.Text = currentAirs.FormattedString

            Master.IsFacilitySubpage = True
        End If
    End Sub

    Private Sub LoadFacilityInfo()
        currentFacility = GetFacilityName(currentAirs) & ", " & GetFacilityCity(currentAirs)
        lblFacilityDisplay.Text = currentFacility
    End Sub

#End Region

#Region " Admin/User Tools "

    Private Sub LoadUserGrid()
        grdUsers.DataSource = GetUserAccess()
        grdUsers.DataBind()
    End Sub

    Protected Sub grdUsers_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdUsers.RowDeleting
        NotNull(e, NameOf(e))

        Dim userid As Decimal = Convert.ToDecimal(grdUsers.DataKeys(e.RowIndex).Values("NUMUSERID").ToString())
        Dim airsnumber As String = grdUsers.DataKeys(e.RowIndex).Values("STRAIRSNUMBER").ToString()

        DeleteUserAccess(userid, airsnumber)

        LoadUserGrid()
    End Sub

    Protected Sub grdUsers_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdUsers.RowUpdating
        NotNull(e, NameOf(e))

        Dim userid As Decimal = Convert.ToDecimal(grdUsers.DataKeys(e.RowIndex).Values("NUMUSERID").ToString())
        Dim airsnumber As String = grdUsers.DataKeys(e.RowIndex).Values("STRAIRSNUMBER").ToString()
        Dim intAdminAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(5).Controls(0), CheckBox).Checked
        Dim intFeeAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(6).Controls(0), CheckBox).Checked
        Dim intEIAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(7).Controls(0), CheckBox).Checked
        Dim intESAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(8).Controls(0), CheckBox).Checked

        UpdateUserAccess(intAdminAccess, intFeeAccess, intEIAccess, intESAccess, userid, airsnumber)

        grdUsers.EditIndex = -1
        LoadUserGrid()
    End Sub

    Protected Sub grdUsers_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdUsers.RowEditing
        NotNull(e, NameOf(e))

        grdUsers.EditIndex = e.NewEditIndex
        LoadUserGrid()
    End Sub

    Protected Sub grdUsers_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles grdUsers.RowCancelingEdit
        grdUsers.EditIndex = -1
        LoadUserGrid()
    End Sub

    Protected Sub grdUsers_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUsers.RowDataBound
        NotNull(e, NameOf(e))

        If facilityAccess.AdminAccess Then
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim linkBtn As LinkButton = CType(e.Row.Cells.Item(0).Controls.Item(0), LinkButton)
                linkBtn.ValidationGroup = False
            End If
        Else
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
        End If
    End Sub

    Protected Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        Dim returnValue As Integer = InsertUserAccess(txtEmail.Text, currentAirs)

        Select Case returnValue
            Case 1 'Successfully added
                lblMessage.Visible = False
                txtEmail.Text = ""
                LoadUserGrid()

            Case -1 'User not registered
                lblMessage.Visible = True

            Case -2 'User access already exists
                lblMessage.Visible = True
                lblMessage.Text = "The user already has access to the facility."

            Case Else
                lblMessage.Visible = True
                lblMessage.Text = "There was an error adding the User. Please try again or contact us if the problem persists"

        End Select
    End Sub

#End Region

End Class
