Imports GECO.GecoModels

Partial Class FacilityAdmin
    Inherits Page

    Protected Property FacilityAccess As FacilityAccess
    Private Property CurrentAirs As ApbFacilityId

    Protected Property ReviewRequested As Boolean

#Region " Page Load "

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        MainLoginCheck()

        If IsPostBack Then
            If Not ApbFacilityId.TryParse(GetCookie(Cookie.AirsNumber), CurrentAirs) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If
        Else
            ' AIRS number
            Dim airsString As String

            If Request.QueryString("airs") IsNot Nothing Then
                airsString = Request.QueryString("airs")
            Else
                airsString = GetCookie(Cookie.AirsNumber)
            End If

            If Not ApbFacilityId.TryParse(airsString, CurrentAirs) Then
                HttpContext.Current.Response.Redirect("~/Home/")
            End If

            SetCookie(Cookie.AirsNumber, CurrentAirs.ShortString())
        End If

        Master.CurrentAirs = CurrentAirs
        Master.IsFacilitySet = True

        ' Current user facility access
        Dim currentUser As GecoUser = GetCurrentUser()
        FacilityAccess = currentUser.GetFacilityAccess(CurrentAirs)

        If FacilityAccess Is Nothing Then
            HttpContext.Current.Response.Redirect("~/Facility/")
        End If

        pnlAddNewUser.Visible = FacilityAccess.AdminAccess
        ReviewRequested = FacilityAccess.AdminAccess AndAlso UserAccessReviewRequested(CurrentAirs)

        If ReviewRequested Then
            UpdateUserAccessAsReviewed(CurrentAirs, currentUser.UserId)
        End If

        If Not IsPostBack Then
            LoadUserGrid()
            Title = $"GECO Facility Admin - {GetFacilityNameAndCity(CurrentAirs)}"
        End If
    End Sub

#End Region

#Region " Admin/User Tools "

    Private Sub LoadUserGrid()
        If Not FacilityAccess.AdminAccess Then
            grdUsers.Columns.Item(0).Visible = False
        End If

        grdUsers.DataSource = GetUserAccess(CurrentAirs)
        grdUsers.DataBind()
    End Sub

    Private Sub grdUsers_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles grdUsers.RowDeleting
        NotNull(e, NameOf(e))

        Dim userid As Integer = CInt(grdUsers.DataKeys(e.RowIndex).Values("NUMUSERID"))

        DeleteUserAccess(userid, CurrentAirs)
        LoadUserGrid()
    End Sub

    Private Sub grdUsers_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles grdUsers.RowUpdating
        NotNull(e, NameOf(e))

        Dim userid As Integer = CInt(grdUsers.DataKeys(e.RowIndex).Values("NUMUSERID"))
        Dim intAdminAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(3).Controls(0), CheckBox).Checked
        Dim intFeeAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(4).Controls(0), CheckBox).Checked
        Dim intEIAccess As Boolean = TryCast(grdUsers.Rows(e.RowIndex).Cells(5).Controls(0), CheckBox).Checked

        UpdateUserAccess(intAdminAccess, intFeeAccess, intEIAccess, userid, CurrentAirs)

        grdUsers.EditIndex = -1
        LoadUserGrid()
    End Sub

    Private Sub grdUsers_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles grdUsers.RowEditing
        NotNull(e, NameOf(e))

        grdUsers.EditIndex = e.NewEditIndex
        LoadUserGrid()
    End Sub

    Private Sub grdUsers_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) _
        Handles grdUsers.RowCancelingEdit
        grdUsers.EditIndex = -1
        LoadUserGrid()
    End Sub

    Private Sub grdUsers_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles grdUsers.RowDataBound
        NotNull(e, NameOf(e))
        If e.Row.RowType <> DataControlRowType.DataRow Then Return

        For Each button As Button In e.Row.Cells(0).Controls.OfType(Of Button)
            If button.CommandName = "Delete" Then
                button.Attributes("onclick") =
                    "if (!confirm('Are you sure you want to remove this user?')) {return false;} "
                button.Attributes("class") = "button-small button-danger"
            Else
                button.Attributes("class") = "button-small"
            End If
        Next
    End Sub

    Private Sub btnAddUser_Click(sender As Object, e As EventArgs) Handles btnAddUser.Click
        lblMessage.Visible = False
        Dim returnValue As Integer = InsertUserAccess(txtEmail.Text, CurrentAirs)

        Select Case returnValue
            Case 1 'Successfully added
                lblMessage.Text = "New user successfully added."
                lblMessage.Visible = True
                txtEmail.Text = ""
                LoadUserGrid()

            Case -1 'User not registered
                lblMessage.Text = "A GECO account could not be found for that email."
                lblMessage.Visible = True

            Case -2 'User access already exists
                lblMessage.Text = "The user already has access to this facility."
                lblMessage.Visible = True

            Case Else
                lblMessage.Text =
                    "There was an error adding the user. Please try again or contact EPD if the problem persists."
                lblMessage.Visible = True

        End Select
    End Sub

#End Region
End Class
