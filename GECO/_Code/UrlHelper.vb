Public Module UrlHelper

    Public Function FullyQualifiedUrl(relativeUrl As String) As String
        Dim request As HttpRequest = HttpContext.Current.Request

        If request IsNot Nothing Then
            Dim page As Page = CType(HttpContext.Current.Handler, Page)

            If page IsNot Nothing Then
                Return request.Url.GetLeftPart(UriPartial.Authority) & page.ResolveUrl(relativeUrl)
            End If
        End If

        Return Nothing
    End Function

End Module
