Imports GECO.DAL
Imports GECO.GecoModels

Public Class InvoiceDefault
    Inherits Page

    ' Annual Emissions Fees
    Private InvoiceGuid As Guid

    ' Application Fees
    Private FeeYear As Integer
    Private FacilityID As ApbFacilityId
    Private InvoiceId As Integer = 0

    'Private InvoiceCategory As InvoiceCategory
    Private Invoices As New List(Of Invoice)

    Public ReadOnly FeeContactInfo As String = ConfigurationManager.AppSettings("FeeContactInfo")

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If Request.QueryString("id") IsNot Nothing AndAlso
                Guid.TryParse(Request.QueryString("id"), InvoiceGuid) Then

                Dim invoice As Invoice = GetInvoiceByGuid(InvoiceGuid)

                If invoice IsNot Nothing Then
                    Invoices.Add(invoice)
                End If

                'InvoiceCategory = InvoiceCategory.PermitApplicationFees
            ElseIf Request.QueryString("FeeYear") IsNot Nothing AndAlso
                Request.QueryString("Facility") IsNot Nothing AndAlso
                Integer.TryParse(Request.QueryString("FeeYear"), FeeYear) AndAlso
                ApbFacilityId.TryParse(Request.QueryString("Facility"), FacilityID) Then

                ' If TryParse fails, set InvoiceId = 0
                If Request.QueryString("InvoiceId") IsNot Nothing AndAlso
                    Not Integer.TryParse(Request.QueryString("InvoiceId"), InvoiceId) Then
                    InvoiceId = 0
                End If

                Invoices = GetEmissionFeeInvoices(FeeYear, FacilityID, InvoiceId)
                'InvoiceCategory = InvoiceCategory.EmissionsFees
            End If

            DisplayInvoices()

        End If
    End Sub

    Private Sub DisplayInvoices()
        If Invoices Is Nothing OrElse Invoices.Count = 0 Then
            Master.MemoPageCount = 0
            pnlNotFound.Visible = True
            invoicePages.Visible = False
            Title = "Not found"
        Else
            Master.MemoPageCount = Invoices.Count
            invoicePages.DataSource = Invoices
            invoicePages.DataBind()
            Title = "Invoice #" & ConcatNonEmptyStrings(", ", Invoices.[Select](Function(x) x.InvoiceID.ToString()).ToArray())
        End If

    End Sub

    Protected Sub invoicePages_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles invoicePages.ItemDataBound
        NotNull(e, NameOf(e))

        Dim currentInvoice As Invoice = CType(e.Item.DataItem, Invoice)

        Dim grdInvoiceItems As GridView = CType(e.Item.FindControl("grdInvoiceItems"), GridView)
        grdInvoiceItems.DataSource = currentInvoice.InvoiceItems
        grdInvoiceItems.DataBind()

        Dim grdPayments As GridView = CType(e.Item.FindControl("grdPayments"), GridView)
        grdPayments.DataSource = currentInvoice.DepositsApplied
        grdPayments.DataBind()
    End Sub

    Protected Sub gridView_DataBound(sender As Object, e As EventArgs)
        NotNull(sender, NameOf(sender))

        Dim gridView As GridView = CType(sender, GridView)
        ' EmptyDataText property of the GridView is only used to pass in an aggregate 
        ' value from the Repeater data item. It is reset here to prevent the value from 
        ' being displayed in empty cells.

        If gridView.Rows.Count > 0 Then
            gridView.FooterRow.Cells(0).Text = "Total"
            gridView.FooterRow.Cells(0).CssClass = "table-cell-alignright"
            gridView.FooterRow.Cells(1).Text = CDec(gridView.EmptyDataText).ToString("c")
            gridView.FooterRow.Cells(1).CssClass = "table-cell-alignright"
            gridView.EmptyDataText = Nothing

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
        NotNull(invoice, NameOf(invoice))

        If invoice.InvoiceCategory = InvoiceCategory.EmissionsFees Then
            Return "<b>" & invoice.FeeYear.ToString & " Emission Fees</b><br />" &
                "Total emission fees: " & invoice.FeeYearTotalEmissionFees?.ToString("c") & "<br />" &
                "Administrative fee: " & invoice.FeeYearTotalAdminFee?.ToString("c")
        End If

        Return "Permit Application #" & invoice.ApplicationID.ToString()
    End Function

End Class
