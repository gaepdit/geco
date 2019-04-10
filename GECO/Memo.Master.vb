Imports System.IO
Imports GECO.DAL
Imports SelectPdf

Public Class Memo
    Inherits MasterPage

    Public Property PdfTitle As String = ""

    Private Property asPDF As Boolean = False
    Private Property queryID As Integer = -1

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Integer.TryParse(Request.QueryString("id"), queryID) Then
            queryID = -1
        End If

        lblDirector.Text = GetManagerName("EpdDirector")

        If Boolean.TryParse(Request.QueryString("pdf"), asPDF) And asPDF Then
            pnlToolbar.Visible = False
        End If
    End Sub

    Protected Overrides Sub Render(writer As HtmlTextWriter)
        If asPDF Then
            Dim sw As New StringWriter(), htw As New HtmlTextWriter(sw), ms As New MemoryStream()

            Try
                MyBase.Render(htw)

                Dim pdfConverter As New HtmlToPdf(768)
                Dim fileName As String

                If Not String.IsNullOrEmpty(PdfTitle) Then
                    fileName = Replace(Replace(PdfTitle, " ", "-"), "#", "") & ".pdf"
                Else
                    fileName = Replace(Replace(Request.Url.AbsolutePath.Trim("/"c), ".aspx", ""), "/", "_") &
                        If(queryID < 0, "", String.Concat("-", queryID.ToString())) & ".pdf"
                End If

                With pdfConverter.Options
                    .PdfPageSize = PdfPageSize.Letter
                    .PdfPageOrientation = PdfPageOrientation.Portrait
                    .MarginTop = 18
                    .MarginBottom = 18
                    .MarginLeft = 18
                    .MarginRight = 18
                    .MaxPageLoadTime = 600
                    .EmbedFonts = False
                End With

                LogToTextFile("Starting PDF conversion")

                Dim doc As PdfDocument = pdfConverter.ConvertHtmlString(sw.ToString(), Request.Url.AbsoluteUri)

                If Not String.IsNullOrEmpty(PdfTitle) Then
                    doc.DocumentInformation.Title = PdfTitle
                End If

                doc.Save(ms)
                doc.Close()

                LogToTextFile("PDF conversion complete")

                Response.ClearContent()
                Response.ContentType = "application/pdf"
                Response.AddHeader("Content-Disposition", String.Concat("inline; filename=", fileName))
                Response.AddHeader("Content-Length", ms.Length.ToString())
                Response.BinaryWrite(ms.ToArray())
                Response.End()
            Finally
                If ms IsNot Nothing Then ms.Dispose()
                If htw IsNot Nothing Then htw.Dispose()
            End Try
        Else
            MyBase.Render(writer)
        End If
    End Sub

    Protected Sub btnPdf_Click() Handles btnPdf.Click
        Dim url As String = Request.Url.GetLeftPart(UriPartial.Path) &
            If(Request.QueryString.ToString() = "", "?pdf=true", "?" + Request.QueryString.ToString() + "&pdf=true")

        Response.Redirect(url)
    End Sub

End Class