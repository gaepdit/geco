Imports GECO.DAL
Imports GECO.GecoModels

Public Class InvoiceDefault
    Inherits Page

    Private Property InvoiceGuid As Guid
    Private thisInvoice As Invoice

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Guid.TryParse(Request.QueryString("id"), InvoiceGuid) Then
            Throw New HttpException(404, "Invoice not found.")
        End If

        thisInvoice = GetInvoiceByGuid(InvoiceGuid)

        If thisInvoice Is Nothing Then
            Throw New HttpException(404, "Invoice ID not found.")
        End If

        If Not IsPostBack Then
            DisplayInvoice()
        End If
    End Sub

    Private Sub DisplayInvoice()
        Dim docName As String = "Invoice #" & thisInvoice.InvoiceID.ToString()

        Master.PdfTitle = docName
        Title = docName
        lblInvoiceId.Text = docName

        lblInvoiceDate.Text = thisInvoice.InvoiceDate.ToString(LongishDateFormat)
        lblDueDate.Text = thisInvoice.InvoiceDate.AddDays(14).ToString(LongishDateFormat)
        lblAirsNo.Text = thisInvoice.FacilityID.FormattedString()
        lblCompany.Text = thisInvoice.FacilityName

        Select Case thisInvoice.InvoiceCategory
            Case InvoiceCategory.EmissionsFees
                lblWhatFor.Text = thisInvoice.FeeYear.Value.ToString & " Emissions Fees"
                lblInvoiceType.Text = thisInvoice.InvoiceType.Description

            Case InvoiceCategory.PermitApplicationFees
                lblWhatFor.Text = "Permit Application #" & thisInvoice.ApplicationID.ToString()
                lblInvoiceType.Visible = False

        End Select

        grdInvoiceItems.DataSource = thisInvoice.InvoiceItems
        grdInvoiceItems.DataBind()

        grdPayments.DataSource = thisInvoice.DepositsApplied
        grdPayments.DataBind()

        If thisInvoice.Voided Then
            pnlVoidedInvoice.Visible = True

            If thisInvoice.VoidedDate.HasValue Then
                pnlVoidedDate.Visible = True
                lblVoidedDate.Text = "Voided on " & thisInvoice.VoidedDate.Value.ToString(LongishDateFormat)
            End If
        End If
    End Sub

    Protected Sub grdInvoiceItems_DataBound() Handles grdInvoiceItems.DataBound
        If grdInvoiceItems.Rows.Count > 0 Then
            grdInvoiceItems.FooterRow.Cells(0).Text = "Total"
            grdInvoiceItems.FooterRow.Cells(0).CssClass = "table-cell-alignright"
            grdInvoiceItems.FooterRow.Cells(1).Text = thisInvoice.TotalAmountDue.ToString("c")
            grdInvoiceItems.FooterRow.Cells(1).CssClass = "table-cell-alignright"

            grdInvoiceItems.HeaderRow.TableSection = TableRowSection.TableHeader
            grdInvoiceItems.FooterRow.TableSection = TableRowSection.TableFooter
        End If
    End Sub

    Protected Sub grdPayments_DataBound() Handles grdPayments.DataBound
        If grdPayments.Rows.Count = 0 Then
            pnlPayments.Visible = False
        Else
            grdPayments.FooterRow.Cells(0).Text = "Total"
            grdPayments.FooterRow.Cells(0).CssClass = "table-cell-alignright"
            grdPayments.FooterRow.Cells(1).Text = thisInvoice.PaymentsApplied.ToString("c")
            grdPayments.FooterRow.Cells(1).CssClass = "table-cell-alignright"

            grdPayments.HeaderRow.TableSection = TableRowSection.TableHeader
            grdPayments.FooterRow.TableSection = TableRowSection.TableFooter

            lblBalance.Text = thisInvoice.CurrentBalance.ToString("c")
        End If
    End Sub

End Class