Imports System.Drawing

Namespace GecoColors

    Public Module GecoColors

        ' https://webaim.org/resources/contrastchecker/?fcolor=333333&bcolor=FFFFFF
        Public Property Normal As New GecoColor With {
            .HtmlForeColor = "#333333",
            .HtmlBackColor = "#FFFFFF"
        }

        ' https://webaim.org/resources/contrastchecker/?fcolor=750006&bcolor=FFBABA
        Public Property ErrorsTextbox As New GecoColor With {
            .HtmlForeColor = "#750006",
            .HtmlBackColor = "#FFBABA"
        }

        ' https://webaim.org/resources/contrastchecker/?fcolor=AA0000&bcolor=FFFFFF
        Public Property Errors As New GecoColor With {
            .HtmlForeColor = "#AA0000",
            .HtmlBackColor = "#FFFFFF"
        }

        ' https://webaim.org/resources/contrastchecker/?fcolor=AA0000&bcolor=FFFF75
        Public Property ErrorsHighlighted As New GecoColor With {
            .HtmlForeColor = "#AA0000",
            .HtmlBackColor = "#FFFF75"
        }

        ' https://webaim.org/resources/contrastchecker/?fcolor=000000&bcolor=FFFF75
        Public Property NormalHighlighted As New GecoColor With {
            .HtmlForeColor = "#000000",
            .HtmlBackColor = "#FFFF75"
        }

        ' https://webaim.org/resources/contrastchecker/?fcolor=AA0000&bcolor=FFFF75
        Public Property InProgressPanel As New GecoColor With {
            .HtmlForeColor = "#FFFFFF",
            .HtmlBackColor = "#006600"
        }

        ' https://webaim.org/resources/contrastchecker/?fcolor=FFFFFF&bcolor=0000FF
        Public Property CompletedPanel As New GecoColor With {
            .HtmlForeColor = "#FFFFFF",
            .HtmlBackColor = "#0000FF"
        }

        ' https://webaim.org/resources/contrastchecker/?fcolor=FFFFFF&bcolor=0000FF
        Public Property ErrorPanel As New GecoColor With {
            .HtmlForeColor = "#FFFFFF",
            .HtmlBackColor = "#AA0000"
        }

        ' https://webaim.org/resources/contrastchecker/?fcolor=E7ECFF&bcolor=000084
        Public Property EisHeader As New GecoColor With {
            .HtmlForeColor = "#E7ECFF",
            .HtmlBackColor = "#000084"
        }

    End Module

End Namespace

Public Class GecoColor

    Public Property HtmlForeColor As String

    Public Property HtmlBackColor As String

    Public Property ForeColor As Color
        Get
            Return ColorTranslator.FromHtml(HtmlForeColor)
        End Get
        Set(value As Color)
            HtmlForeColor = ColorTranslator.ToHtml(value)
        End Set
    End Property

    Public Property BackColor As Color
        Get
            Return ColorTranslator.FromHtml(HtmlBackColor)
        End Get
        Set(value As Color)
            HtmlBackColor = ColorTranslator.ToHtml(value)
        End Set
    End Property

End Class