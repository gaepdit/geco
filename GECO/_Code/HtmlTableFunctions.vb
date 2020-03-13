Imports System.Runtime.CompilerServices

Public Module HtmlTableFunctions

    <Extension()>
    Public Sub AddTableRow(table As Table, label As String, value As String, Optional labelIsHeader As Boolean = True, Optional classList As String() = Nothing)
        NotNull(table, NameOf(table))

        Dim tr As New TableRow()

        If labelIsHeader Then
            tr.Cells.Add(New TableHeaderCell() With {.Text = label})
        Else
            tr.Cells.Add(New TableCell() With {.Text = label})
        End If

        Dim td As New TableCell With {.Text = If(value, "N/A")}

        If classList IsNot Nothing AndAlso classList.Length > 0 Then
            td.CssClass = ConcatNonEmptyStrings(" ", classList)
        End If

        tr.Cells.Add(td)

        table.Rows.Add(tr)
    End Sub

    <Extension()>
    Public Sub AddTableRow(table As Table, label As String, value As WebControl, Optional labelIsHeader As Boolean = True, Optional classList As String() = Nothing)
        NotNull(table, NameOf(table))

        Dim tr As New TableRow()

        If labelIsHeader Then
            tr.Cells.Add(New TableHeaderCell() With {.Text = label})
        Else
            tr.Cells.Add(New TableCell() With {.Text = label})
        End If

        Dim td As New TableCell()
        td.Controls.Add(value)

        If classList IsNot Nothing AndAlso classList.Length > 0 Then
            td.CssClass = ConcatNonEmptyStrings(" ", classList)
        End If

        tr.Cells.Add(td)

        table.Rows.Add(tr)
    End Sub

    <Extension()>
    Public Sub AddTableRow(table As Table, label As String, value As Decimal, Optional asCurrency As Boolean = False, Optional labelIsHeader As Boolean = True)
        NotNull(table, NameOf(table))

        If asCurrency Then
            table.AddTableRow(label, String.Format(Globalization.CultureInfo.CurrentCulture, "{0:C}", value), labelIsHeader, {"table-cell-alignright"})
        Else
            table.AddTableRow(label, value, labelIsHeader, {"table-cell-alignright"})
        End If
    End Sub

    <Extension()>
    Public Sub AddTableRow(table As Table, label As String, dateValue As Date?, Optional skipIfNull As Boolean = True, Optional labelIsHeader As Boolean = True, Optional classList As String() = Nothing)
        NotNull(table, NameOf(table))

        If Not dateValue.HasValue AndAlso skipIfNull Then
            Return
        End If

        table.AddTableRow(label, If(dateValue.HasValue, dateValue.Value.ToString(ShortishDateFormat), Nothing), labelIsHeader, classList)
    End Sub

    <Extension()>
    Public Sub AddTableHeaderRow(table As Table, label As String, value As String)
        NotNull(table, NameOf(table))

        Dim tr As New TableRow With {
            .TableSection = TableRowSection.TableHeader
        }

        tr.Cells.Add(New TableHeaderCell() With {.Text = label})
        tr.Cells.Add(New TableHeaderCell() With {.Text = value})

        table.Rows.Add(tr)
    End Sub

    <Extension()>
    Public Sub AddTableFooterRow(table As Table, label As String, value As String, Optional labelIsHeader As Boolean = True)
        NotNull(table, NameOf(table))

        Dim tr As New TableRow With {
            .TableSection = TableRowSection.TableFooter
        }

        If labelIsHeader Then
            tr.Cells.Add(New TableHeaderCell() With {.Text = label})
        Else
            tr.Cells.Add(New TableCell() With {.Text = label})
        End If

        tr.Cells.Add(New TableCell() With {.Text = value})

        table.Rows.Add(tr)
    End Sub

    <Extension()>
    Public Sub AddTableFooterRow(table As Table, label As String, value As Decimal, Optional asCurrency As Boolean = False, Optional labelIsHeader As Boolean = True)
        NotNull(table, NameOf(table))

        Dim tr As New TableRow With {
            .TableSection = TableRowSection.TableFooter
        }

        If labelIsHeader Then
            tr.Cells.Add(New TableHeaderCell() With {.Text = label})
        Else
            tr.Cells.Add(New TableCell() With {.Text = label})
        End If

        Dim td As New TableCell With {
            .CssClass = "table-cell-alignright"
        }

        If asCurrency Then
            td.Text = String.Format(Globalization.CultureInfo.CurrentCulture, "{0:C}", value)
        Else
            td.Text = value
        End If

        tr.Cells.Add(td)

        table.Rows.Add(tr)
    End Sub

End Module