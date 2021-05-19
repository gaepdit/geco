Imports System.IO
Imports ClosedXML.Excel

Public Module GridViewExportUtil

    Public Sub ExportAsExcel(fileName As String, gridView As GridView)

        If gridView Is Nothing OrElse gridView.Rows.Count = 0 Then
            Return
        End If

        Dim datatable As DataTable = CType(gridView.DataSource, DataTable)
        If datatable Is Nothing OrElse datatable.Rows.Count = 0 Then
            Return
        End If

        If String.IsNullOrWhiteSpace(datatable.TableName) Then
            datatable.TableName = "Sheet1"
        End If

        Dim wb As New XLWorkbook(), ms As New MemoryStream()

        Try
            Dim ws As IXLWorksheet = wb.AddWorksheet(datatable)
            ws.Columns().AdjustToContents(8.0R, 80.0R)

            Dim httpResponse As HttpResponse = HttpContext.Current.Response
            httpResponse.Clear()
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            httpResponse.AddHeader("content-disposition", String.Format("attachment; filename=""{0}.xlsx""", fileName))

            wb.SaveAs(ms)
            ms.WriteTo(httpResponse.OutputStream)
            ms.Close()

            httpResponse.End()
        Finally
            If wb IsNot Nothing Then wb.Dispose()
        End Try

    End Sub

End Module
