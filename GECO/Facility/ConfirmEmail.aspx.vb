Imports GECO.DAL.Facility

Public Class ConfirmEmail
    Inherits UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim seq As String = Request.QueryString("seq")
            Dim token As String = Request.QueryString("token")

            If String.IsNullOrEmpty(seq) AndAlso String.IsNullOrEmpty(token) Then
                HttpContext.Current.Response.Redirect("~/")
            End If

            If String.IsNullOrEmpty(seq) OrElse String.IsNullOrEmpty(token) Then
                Throw New HttpException(400, "Bad Request")
            End If

            Dim result As ConfirmContactEmailResult = ConfirmContactEmail(seq, token)

            If result.Success Then
                lblAirs.Text = result.FacilityId.FormattedString
                lblFacility.Text = result.FacilityName & ", " & result.FacilityCity
                lblCategory.Text = result.CategoryDesc
                MultiView1.SetActiveView(ConfirmSuccess)
            Else
                MultiView1.SetActiveView(ConfirmFailed)
            End If

        End If
    End Sub

End Class
