Imports System.Data
Imports System.IO
Imports ClosedXML.Excel

Public Module GridViewExportUtil

    Public Sub ExportAsExcel(fileName As String, gridView As GridView)

        If gridView Is Nothing OrElse gridView.Rows.Count = 0 Then Exit Sub

        Dim datatable As DataTable = gridView.DataSource
        If datatable Is Nothing OrElse datatable.Rows.Count = 0 Then Exit Sub

        If String.IsNullOrWhiteSpace(datatable.TableName) Then
            datatable.TableName = "Sheet1"
        End If

        Using wb As New XLWorkbook()
            wb.AddWorksheet(datatable)

            Dim httpResponse As HttpResponse = HttpContext.Current.Response
            httpResponse.Clear()
            httpResponse.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            httpResponse.AddHeader("content-disposition", String.Format("attachment; filename=""{0}.xlsx""", fileName))

            Using ms As MemoryStream = New MemoryStream()
                wb.SaveAs(ms)
                ms.WriteTo(httpResponse.OutputStream)
                ms.Close()
            End Using

            httpResponse.End()
        End Using

    End Sub

End Module
