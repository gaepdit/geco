Imports System.Runtime.CompilerServices

Module DateDisplayFormats

    ' DateTime display formats
    Public Const EditDate As String = "{0:d}"
    Public Const EditDateTime As String = "{0:g}"
    Public Const LongDate As String = "MMMM d, yyyy"
    Public Const ShortDate As String = "d-MMM-yyyy"
    Public Const ShortDateComposite As String = "{0:d-MMM-yyyy}"
    Public Const DateTimeComposite As String = "{0:d-MMM-yyyy, h:mm tt}"

    <Extension>
    Public Function ToShortDate(d As Date) As String
        Return d.ToString(ShortDate)
    End Function

    <Extension>
    Public Function ToLongDate(d As Date) As String
        Return d.ToString(LongDate)
    End Function

End Module
