Imports System.Web.Services
Imports AjaxControlToolkit

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<CompilerServices.DesignerGenerated()>
Public Class SourceClassCode
    Inherits WebService

    <WebMethod()>
    Public Function GetLevel1(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Dim vals As New List(Of CascadingDropDownNameValue)

        Using dt As DataTable = GetSccLevel1()
            For Each dtrow As DataRow In dt.Rows
                vals.Add(New CascadingDropDownNameValue(dtrow("scc level one").ToString, dtrow("scc level one").ToString))
            Next
        End Using

        Return vals.ToArray()
    End Function

    <WebMethod()>
    Public Function GetLevel2(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Dim sd As StringDictionary = ParseKnownCategoryValuesString(knownCategoryValues)

        If (sd.ContainsKey("Level1")) Then
            Dim vals As New List(Of CascadingDropDownNameValue)

            Using dt As DataTable = GetSccLevel2(sd("Level1"))
                For Each dtrow As DataRow In dt.Rows
                    vals.Add(New CascadingDropDownNameValue(dtrow("scc level two").ToString, dtrow("scc level two").ToString))
                Next
            End Using

            Return vals.ToArray()
        End If

        Return Nothing
    End Function

    <WebMethod()>
    Public Function GetLevel3(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Dim sd As StringDictionary = ParseKnownCategoryValuesString(knownCategoryValues)

        If sd.ContainsKey("Level1") AndAlso sd.ContainsKey("Level2") Then
            Dim vals As New List(Of CascadingDropDownNameValue)

            Using dt As DataTable = GetSccLevel3(sd("Level1"), sd("Level2"))
                For Each dtrow As DataRow In dt.Rows
                    vals.Add(New CascadingDropDownNameValue(dtrow("scc level three").ToString, dtrow("scc level three").ToString))
                Next
            End Using

            Return vals.ToArray()
        End If

        Return Nothing
    End Function

    <WebMethod()>
    Public Function GetLevel4(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Dim sd As StringDictionary = ParseKnownCategoryValuesString(knownCategoryValues)

        If sd.ContainsKey("Level1") AndAlso sd.ContainsKey("Level2") AndAlso sd.ContainsKey("Level3") Then
            Dim vals As New List(Of CascadingDropDownNameValue)

            Using dt As DataTable = GetSccLevel4(sd("Level1"), sd("Level2"), sd("Level3"))
                For Each dtrow As DataRow In dt.Rows
                    vals.Add(New CascadingDropDownNameValue(dtrow("scc level four").ToString, dtrow("scc level four").ToString))
                Next
            End Using

            Return vals.ToArray()
        End If

        Return Nothing
    End Function

    Private Function ParseKnownCategoryValuesString(knownCategoryValues As String) As StringDictionary
        ' Sometimes our category values contain colons or semicolons which confuses 
        ' CascadingDropDown.ParseKnownCategoryValuesString
        ' This function works around that limitation
        knownCategoryValues = Left(knownCategoryValues, knownCategoryValues.Length - 1)

        knownCategoryValues = Replace(knownCategoryValues, "Level1:", "LEVEL_1_DELIMITER")
        knownCategoryValues = Replace(knownCategoryValues, ";Level2:", "LEVEL_2_DELIMITER")
        knownCategoryValues = Replace(knownCategoryValues, ";Level3:", "LEVEL_3_DELIMITER")

        knownCategoryValues = Replace(knownCategoryValues, ":", "_COLON_REPLACEMENT")
        knownCategoryValues = Replace(knownCategoryValues, ";", "SEMICOLON_REPLACEMENT")

        knownCategoryValues = Replace(knownCategoryValues, "LEVEL_1_DELIMITER", "Level1:")
        knownCategoryValues = Replace(knownCategoryValues, "LEVEL_2_DELIMITER", ";Level2:")
        knownCategoryValues = Replace(knownCategoryValues, "LEVEL_3_DELIMITER", ";Level3:")

        Dim sd As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        Dim keys As New List(Of String)

        For Each key As String In sd.Keys
            keys.Add(key)
        Next

        For Each key As String In keys
            sd(key) = Replace(sd(key), "SEMICOLON_REPLACEMENT", ";")
            sd(key) = Replace(sd(key), "_COLON_REPLACEMENT", ":")
        Next

        Return sd
    End Function

End Class