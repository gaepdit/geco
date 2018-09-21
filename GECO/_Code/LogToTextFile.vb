Imports System.IO

Public Module Logging

    Public Sub LogToTextFile(value As String)
        Dim fileName As String = "GecoLog.txt"
        Dim folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings("LoggingFolder"))
        Directory.CreateDirectory(folder)

        Try
            Using sw As StreamWriter = New StreamWriter(Path.Combine(folder, fileName), True)
                sw.WriteLine("===== BEGIN LOG " & Date.Now.ToString)
                sw.WriteLine()
                sw.WriteLine(value)
                sw.WriteLine()
                sw.Flush()
                sw.Close()
            End Using
        Catch ex As Exception
            'do nothing
        End Try
    End Sub

End Module