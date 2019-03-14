Imports System.Data.SqlClient
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
        Try
            Dim cascadingDropDownNameValues As New List(Of CascadingDropDownNameValue)
            Dim query As String = "SELECT Distinct STRDESC1 FROM EISLK_SourceClassCode Where Active = '1' ORDER By STRDESC1"
            Dim dt As DataTable = DB.GetDataTable(query)
            For Each dtrow As DataRow In dt.Rows
                cascadingDropDownNameValues.Add(New CascadingDropDownNameValue(dtrow("STRDESC1").ToString, dtrow("STRDESC1").ToString))
            Next
            Return cascadingDropDownNameValues.ToArray()
        Catch ex As Exception
            ErrorReport(ex)
            Return Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetLevel2(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Try
            Dim stringDictionaries As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If (stringDictionaries.ContainsKey("Level1")) Then
                Dim cascadingDropDownNameValues As New List(Of CascadingDropDownNameValue)
                Dim query As String = "SELECT Distinct STRDESC2 FROM EISLK_SourceClassCode " &
                    " Where Active = '1' and STRDESC1 = @Level1 ORDER By STRDESC2"
                Dim param As New SqlParameter("@Level1", stringDictionaries("Level1"))
                Dim dt As DataTable = DB.GetDataTable(query, param)
                For Each dtrow As DataRow In dt.Rows
                    cascadingDropDownNameValues.Add(New CascadingDropDownNameValue(dtrow("STRDESC2").ToString, dtrow("STRDESC2").ToString))
                Next
                Return cascadingDropDownNameValues.ToArray()
            Else
                Return Nothing
            End If
        Catch
            Return Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetLevel3(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Try
            Dim stringDictionaries As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If stringDictionaries.ContainsKey("Level1") AndAlso stringDictionaries.ContainsKey("Level2") Then
                Dim cascadingDropDownNameValues As New List(Of CascadingDropDownNameValue)
                Dim query As String = "SELECT Distinct STRDESC3 FROM EISLK_SourceClassCode " &
                    " Where Active = '1' and STRDESC1 = @Level1 and STRDESC2 = @Level2 ORDER By STRDESC3"
                Dim param As SqlParameter() = {
                    New SqlParameter("@Level1", stringDictionaries("Level1")),
                    New SqlParameter("@Level2", stringDictionaries("Level2"))
                }
                Dim dt As DataTable = DB.GetDataTable(query, param)
                For Each dtrow As DataRow In dt.Rows
                    cascadingDropDownNameValues.Add(New CascadingDropDownNameValue(dtrow("STRDESC3").ToString, dtrow("STRDESC3").ToString))
                Next
                Return cascadingDropDownNameValues.ToArray()
            Else
                Return Nothing
            End If
        Catch
            Return Nothing
        End Try
    End Function

    <WebMethod()>
    Public Function GetLevel4(knownCategoryValues As String, category As String) As CascadingDropDownNameValue()
        Try
            Dim stringDictionaries As StringDictionary = CascadingDropDown.ParseKnownCategoryValuesString(knownCategoryValues)
            If stringDictionaries.ContainsKey("Level1") AndAlso stringDictionaries.ContainsKey("Level2") AndAlso stringDictionaries.ContainsKey("Level3") Then
                Dim cascadingDropDownNameValues As New List(Of CascadingDropDownNameValue)
                Dim query As String = "SELECT Distinct STRDESC4 FROM EISLK_SourceClassCode " &
                    " Where Active = '1' and STRDESC1 = @Level1 and STRDESC2 = @Level2 and STRDESC3 = @Level3 ORDER By STRDESC4"
                Dim param As SqlParameter() = {
                    New SqlParameter("@Level1", stringDictionaries("Level1")),
                    New SqlParameter("@Level2", stringDictionaries("Level2")),
                    New SqlParameter("@Level3", stringDictionaries("Level3"))
                }
                Dim dt As DataTable = DB.GetDataTable(query, param)
                For Each dtrow As DataRow In dt.Rows
                    cascadingDropDownNameValues.Add(New CascadingDropDownNameValue(dtrow("STRDESC4").ToString, dtrow("STRDESC4").ToString))
                Next
                Return cascadingDropDownNameValues.ToArray()
            Else
                Return Nothing
            End If
        Catch
            Return Nothing
        End Try
    End Function

End Class