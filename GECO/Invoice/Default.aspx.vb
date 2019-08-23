Imports GECO.DAL
Imports GECO.GecoModels

Public Class InvoiceDefault
    Inherits Page

    Private Property InvoiceGuid As Guid
    Private Invoices As New List(Of Invoice)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Guid.TryParse(Request.QueryString("id"), InvoiceGuid) Then
                Throw New HttpException(404, "Invoice not found.")
            End If

            Dim thisInvoice As Invoice = GetInvoiceByGuid(InvoiceGuid)

            If thisInvoice Is Nothing Then
                Throw New HttpException(404, "Invoice ID not found.")
            End If

            Invoices.Add(thisInvoice)

            DisplayInvoices()
        End If
    End Sub

    Private Sub DisplayInvoices()
        If Invoices Is Nothing OrElse Invoices.Count = 0 Then
            Master.MemoPageCount = 0
            pnlNotFound.Visible = True
            invoicePages.Visible = False
        Else
            Master.MemoPageCount = Invoices.Count
            invoicePages.DataSource = Invoices
            invoicePages.DataBind()
        End If

        Title = "Invoice #" & ConcatNonEmptyStrings(", ", Invoices.[Select](Function(x) x.InvoiceID.ToString()).ToArray())
    End Sub

    Protected Sub invoicePages_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles invoicePages.ItemDataBound
        Dim currentInvoice As Invoice = CType(e.Item.DataItem, Invoice)

        Dim grdInvoiceItems As GridView = CType(e.Item.FindControl("grdInvoiceItems"), GridView)
        grdInvoiceItems.DataSource = currentInvoice.InvoiceItems
        grdInvoiceItems.DataBind()

        Dim grdPayments As GridView = CType(e.Item.FindControl("grdPayments"), GridView)
        grdPayments.DataSource = currentInvoice.DepositsApplied
        grdPayments.DataBind()
    End Sub

    Protected Sub gridView_DataBound(sender As Object, e As EventArgs)
        Dim gridView As GridView = CType(sender, GridView)

        If gridView.Rows.Count > 0 Then
            gridView.FooterRow.Cells(0).Text = "Total"
            gridView.FooterRow.Cells(0).CssClass = "table-cell-alignright"
            gridView.FooterRow.Cells(1).Text = CInt(gridView.EmptyDataText).ToString("c")
            gridView.FooterRow.Cells(1).CssClass = "table-cell-alignright"

            gridView.HeaderRow.TableSection = TableRowSection.TableHeader
            gridView.FooterRow.TableSection = TableRowSection.TableFooter
        End If
    End Sub

    Protected Shared Function DisplayObject(o As Object, Optional format As String = "{0}") As String
        If o Is Nothing Then
            Return ""
        End If

        Return String.Format(format, o.ToString)
    End Function

    Protected Shared Function DisplayDate(d As Date, Optional format As String = "{0}") As String
        Return String.Format(format, d.ToString(LongishDateFormat))
    End Function

    Protected Shared Function DisplayNullableDate(d As Date?, Optional format As String = "{0}") As String
        If Not d.HasValue Then
            Return ""
        End If

        Return String.Format(format, d.Value.ToString(LongishDateFormat))
    End Function

    Protected Shared Function DisplayWhatFor(invoice As Invoice) As String
        If invoice.InvoiceCategory = InvoiceCategory.EmissionsFees Then
            Return invoice.FeeYear.ToString & " Annual Permit Fees"
        End If

        Return "Permit Application #" & invoice.ApplicationID.ToString()
    End Function

    Protected Shared Function DisplayInvoiceTypeDescription(invoice As Invoice) As String
        If invoice.InvoiceCategory = InvoiceCategory.EmissionsFees Then
            Return invoice.InvoiceType.Description
        End If

        Return ""
    End Function

End Class