Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels

Public Module eis_Access

    ' | EISACCESSCODE | STRDESC                                                 |
    ' |---------------|---------------------------------------------------------|
    ' | 0             | FI access allowed with edit; EI access allowed, no edit |
    ' | 1             | FI and EI access allowed, both with edit                |
    ' | 2             | FI and EI access allowed, both no edit                  |
    ' | 3             | Facility not in EIS                                     |
    ' | 4             | Facility has no access to FI or EI                      |

    ' Reminder: EISAccess = 3 indicates that the facility is not in the EIS_Admin table.
    ' "3" is never stored in the admin table. It is set in the GetEISStatus routine in Facility/Default.aspx.vb

    ' | EISSTATUSCODE | STRDESC                  |
    ' |---------------|--------------------------|
    ' | 0             | Not applicable           |
    ' | 1             | Applicable - not started |
    ' | 2             | In progress              |
    ' | 3             | Submitted                |
    ' | 4             | QA Process               |
    ' | 5             | Complete                 |

    Private Function GetEiStatusDataRow(airs As ApbFacilityId) As DataRow
        NotNull(airs, NameOf(airs))

        Dim query As String = "select INVENTORYYEAR, EISSTATUSCODE, EISACCESSCODE,
               convert(bit, STROPTOUT) as OPTOUT, convert(bit, STRENROLLMENT) as ENROLLMENT,
               DATFINALIZE, STRCONFIRMATIONNUMBER
            FROM EIS_ADMIN
            where FACILITYSITEID = @airs
              and INVENTORYYEAR =
                  (select max(INVENTORYYEAR)
                   FROM EIS_ADMIN
                   where FACILITYSITEID = @airs)"

        Dim param As New SqlParameter("@airs", airs.ShortString)

        Return DB.GetDataRow(query, param)
    End Function

    Public Function GetEiStatus(airs As ApbFacilityId) As EiStatus
        NotNull(airs, NameOf(airs))

        Dim dr As DataRow = GetEiStatusDataRow(airs)

        If dr Is Nothing Then
            Return New EiStatus With {.AccessCode = 3, .Enrolled = False}
        End If

        Dim eiStatus As New EiStatus With {
            .MaxYear = dr("INVENTORYYEAR")
        }

        If eiStatus.MaxYear <> Now.Year - 1 Then
            eiStatus.AccessCode = 0
            eiStatus.StatusCode = 0
            eiStatus.Enrolled = False

            Return eiStatus
        End If

        ' enrollment status: 0 = not enrolled; 1 = enrolled for EI year
        eiStatus.Enrolled = dr.Item("ENROLLMENT")

        If eiStatus.Enrolled Then
            eiStatus.StatusCode = dr.Item("EISSTATUSCODE")
            eiStatus.AccessCode = dr.Item("EISACCESSCODE")
            eiStatus.OptOut = GetNullable(Of Boolean?)(dr.Item("OPTOUT"))
            eiStatus.DateFinalized = GetNullableDateTime(dr.Item("DATFINALIZE"))
            eiStatus.ConfirmationNumber = GetNullableString(dr.Item("STRCONFIRMATIONNUMBER")).EmptyStringIfNothing()
        End If

        Return eiStatus
    End Function

    Public Sub LoadEiStatusCookies(airs As ApbFacilityId, response As HttpResponse)
        NotNull(airs, NameOf(airs))
        NotNull(response, NameOf(response))

        Dim eiStatus As EiStatus = GetEiStatus(airs)

        Dim enrollment As String = "0"
        If eiStatus.Enrolled Then enrollment = "1"

        Dim optOut As String = "NULL"
        If eiStatus.OptOut.HasValue Then optOut = -CInt(eiStatus.OptOut)

        Dim dateFinalized As String = ""
        If eiStatus.DateFinalized.HasValue Then dateFinalized = eiStatus.DateFinalized.Value.ToString("g")

        Dim EISCookies As New HttpCookie("EISAccessInfo")
        EISCookies.Values(EisCookie.EISMaxYear.ToString) = EncryptText(eiStatus.MaxYear.ToString)
        EISCookies.Values(EisCookie.Enrollment.ToString) = EncryptText(enrollment)
        EISCookies.Values(EisCookie.EISStatus.ToString) = EncryptText(eiStatus.StatusCode.ToString)
        EISCookies.Values(EisCookie.EISAccess.ToString) = EncryptText(eiStatus.AccessCode.ToString)
        EISCookies.Values(EisCookie.OptOut.ToString) = EncryptText(optOut)
        EISCookies.Values(EisCookie.DateFinalize.ToString) = EncryptText(dateFinalized)
        EISCookies.Values(EisCookie.ConfNumber.ToString) = EncryptText(eiStatus.ConfirmationNumber)

        EISCookies.Expires = Date.Now.AddHours(8)

        response.Cookies.Add(EISCookies)
    End Sub

End Module

Public Class EiStatus
    Public Property AccessCode As Integer
    Public Property Enrolled As Boolean
    Public Property StatusCode As Integer
    Public Property DateFinalized As Date?
    Public Property MaxYear As Integer
    Public Property OptOut As Boolean?
    Public Property ConfirmationNumber As String = ""
End Class
