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
        Dim cascadingDropDownNameValues As New List(Of CascadingDropDownNameValue)

        Using dt As DataTable = GetSccLevel1()
            For Each dtrow As DataRow In dt.Rows
                cascadingDropDownNameValues.Add(New CascadingDropDownNameValue(dtrow("scc level one").ToString, dtrow("scc level one").ToString))
            Next
        End Using

        Return cascadingDropDownNameValues.ToArray()
    End Function

    <WebMethod()>
    Public Function GetLevel2(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Dim stringDictionaries As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        If (stringDictionaries.ContainsKey("Level1")) Then
            Dim cascadingDropDownNameValues As New List(Of CascadingDropDownNameValue)

            Using dt As DataTable = GetSccLevel2(stringDictionaries("Level1"))
                For Each dtrow As DataRow In dt.Rows
                    cascadingDropDownNameValues.Add(New CascadingDropDownNameValue(dtrow("scc level two").ToString, dtrow("scc level two").ToString))
                Next
            End Using

            Return cascadingDropDownNameValues.ToArray()
        End If

        Return Nothing
    End Function

    <WebMethod()>
    Public Function GetLevel3(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Dim stringDictionaries As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        If stringDictionaries.ContainsKey("Level1") AndAlso stringDictionaries.ContainsKey("Level2") Then
            Dim cascadingDropDownNameValues As New List(Of CascadingDropDownNameValue)

            Using dt As DataTable = GetSccLevel3(stringDictionaries("Level1"), stringDictionaries("Level2"))
                For Each dtrow As DataRow In dt.Rows
                    cascadingDropDownNameValues.Add(New CascadingDropDownNameValue(dtrow("scc level three").ToString, dtrow("scc level three").ToString))
                Next
            End Using

            Return cascadingDropDownNameValues.ToArray()
        End If

        Return Nothing
    End Function

    <WebMethod()>
    Public Function GetLevel4(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Dim stringDictionaries As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)

        If stringDictionaries.ContainsKey("Level1") AndAlso stringDictionaries.ContainsKey("Level2") AndAlso stringDictionaries.ContainsKey("Level3") Then
            Dim cascadingDropDownNameValues As New List(Of CascadingDropDownNameValue)

            Using dt As DataTable = GetSccLevel4(stringDictionaries("Level1"), stringDictionaries("Level2"), stringDictionaries("Level3"))
                For Each dtrow As DataRow In dt.Rows
                    cascadingDropDownNameValues.Add(New CascadingDropDownNameValue(dtrow("scc level four").ToString, dtrow("scc level four").ToString))
                Next
            End Using

            Return cascadingDropDownNameValues.ToArray()
        End If

        Return Nothing
    End Function

End Class