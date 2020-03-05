Public Module UrlHelper

    <CodeAnalysis.SuppressMessage("Design", "CA1055")>
    <CodeAnalysis.SuppressMessage("Design", "CA1054:Uri parameters should not be strings")>
    Public Function FullyQualifiedUrl(relativeUrl As String) As String
        Dim request As HttpRequest = HttpContext.Current.Request

        If request IsNot Nothing Then
            Dim page As Page = HttpContext.Current.Handler

            If page IsNot Nothing Then
                Return request.Url.GetLeftPart(UriPartial.Authority) & page.ResolveUrl(relativeUrl)
            End If
        End If

        Return Nothing
    End Function

End Module