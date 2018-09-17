Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
<System.Web.Script.Services.ScriptService()>
Public Class GetAirsNo
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function AirsNumber(ByVal prefixText As String, ByVal count As Integer) As String()
        Dim sql As String
        Dim airsList As New List(Of String)
        Dim conn As New SqlConnection(oradb)
        Dim da As SqlDataAdapter
        Dim dt, dt1 As DataTable
        If HttpContext.Current.Cache("AirsNo") Is Nothing Then
            sql = "Select DISTINCT substring(strairsnumber, 5, LEN(strairsnumber)) as strairsnumber, " _
                           + "strfacilityname as facilityname " _
                           + "FROM APBFacilityInformation " _
                           + "order by strairsnumber"

            Dim SqlCommand As New SqlCommand(sql, conn)
            da = New SqlDataAdapter(SqlCommand)
            dt = New DataTable("facilityInfo")
            da.Fill(dt)
            HttpContext.Current.Cache.Insert("AirsNo", dt, Nothing, Cache.NoAbsoluteExpiration,
              TimeSpan.FromHours(14))
        End If

        dt1 = DirectCast(HttpContext.Current.Cache("AirsNo"), DataTable)
        For i As Integer = 0 To dt1.Rows.Count - 1
            'If dt1.Rows(i)("strairsnumber").ToString().StartsWith(prefixText.ToString) Then
            '    AirsNo = dt1.Rows(i)("strairsnumber").ToString()
            '    FacilityName = dt1.Rows(i)("facilityname").ToString()
            '    airsList.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(AirsNo, FacilityName))
            'End If
            airsList.Add(dt1.Rows(i)("strairsnumber").ToString())
            i = i + 1
        Next
        Dim filteredList As New List(Of String)

        For Each s As String In airsList
            If s.StartsWith(prefixText.ToString) Then
                filteredList.Add("'" + s + "'")
            End If
        Next
        Return filteredList.ToArray
        'Return airsList.ToArray()
    End Function

End Class