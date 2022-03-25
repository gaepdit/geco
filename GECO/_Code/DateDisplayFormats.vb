Imports System.Runtime.CompilerServices

Module DateDisplayFormats


    <Extension>
    Public Function ToShortDate(d As Date) As String
        Return d.ToString("d-MMM-yyyy")
    End Function

    <Extension>
    Public Function ToLongDate(d As Date) As String
        Return d.ToString("MMMM d, yyyy")
    End Function

    <Extension>
    Public Function ToFullDateTime(d As Date) As String
        Return d.ToString("ddd, MMM d \a\t h:mm tt")
    End Function

End Module
