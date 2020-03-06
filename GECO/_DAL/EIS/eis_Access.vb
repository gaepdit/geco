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

    Public Function GetEiStatus(airs As ApbFacilityId) As DataRow
        Dim query As String = "select FACILITYSITEID,
                INVENTORYYEAR,
                EISSTATUSCODE,
                DATEISSTATUS,
                EISACCESSCODE,
                STROPTOUT,
                STRENROLLMENT,
                DATFINALIZE,
                STRCONFIRMATIONNUMBER
        FROM EIS_ADMIN
        where FACILITYSITEID = @airs
            and INVENTORYYEAR =
                (select max(INVENTORYYEAR)
                FROM EIS_ADMIN
                where FACILITYSITEID = @airs)"

        Dim param As New SqlParameter("@airs", airs.ShortString)

        Return DB.GetDataRow(query, param)
    End Function

    Public Function LoadEiStatusCookies(airs As ApbFacilityId, response As HttpResponse) As EiStatus
        Dim EISCookies As New HttpCookie("EISAccessInfo")

        Dim enrolled As String = "NULL"
        Dim status As String = "NULL"
        Dim access As String = "NULL"
        Dim optout As String = "NULL"
        Dim dateFinalized As String = "NULL"
        Dim confirmationnumber As String = "NULL"
        Dim eiMaxYear As String = ""

        Dim dr As DataRow = GetEiStatus(airs)

        If dr Is Nothing Then
            'Set EISAccess cookie to "3" if facility does not exist in EIS_Admin table
            EISCookies.Values("EISAccess") = EncryptText("3")
        Else
            'get max year from EIS Admin table
            eiMaxYear = GetNullableString(dr("InventoryYear"))
            EISCookies.Values("EISMaxYear") = EncryptText(eiMaxYear)

            If CInt(eiMaxYear) = Now.Year - 1 Then
                'Check enrollment
                'get enrollment status: 0 = not enrolled; 1 = enrolled for EI year
                If Not IsDBNull(dr("strEnrollment")) Then
                    enrolled = dr.Item("strEnrollment")
                End If

                EISCookies.Values("Enrollment") = EncryptText(enrolled)

                If enrolled = "1" Then
                    'getEISStatus for EISMaxYear
                    If Not IsDBNull(dr("EISStatusCode")) Then
                        status = dr.Item("EISStatusCode")
                    End If

                    EISCookies.Values("EISStatus") = EncryptText(status)

                    'get EIS Access Code from database
                    If Not IsDBNull(dr("EISAccessCode")) Then
                        access = dr.Item("EISAccessCode")
                    End If

                    EISCookies.Values("EISAccess") = EncryptText(access)

                    If Not IsDBNull(dr("strOptOut")) Then
                        optout = dr.Item("strOptOut")
                    End If

                    EISCookies.Values("OptOut") = EncryptText(optout)

                    If Not IsDBNull(dr("datFinalize")) Then
                        dateFinalized = dr.Item("datFinalize")
                    End If

                    EISCookies.Values("DateFinalize") = EncryptText(dateFinalized)

                    If Not IsDBNull(dr("strConfirmationNumber")) Then
                        confirmationnumber = dr.Item("strConfirmationNumber")
                    End If

                    EISCookies.Values("ConfNumber") = EncryptText(confirmationnumber)
                End If
            Else
                'Set EISAccessCode = "0" for Facility Inventory Access only
                EISCookies.Values("EISAccess") = EncryptText("0")
                EISCookies.Values("EISStatus") = EncryptText("0")
                EISCookies.Values("Enrollment") = EncryptText("0")
            End If
        End If

        EISCookies.Expires = Date.Now.AddHours(8)
        response.Cookies.Add(EISCookies)

        Return New EiStatus With {
            .Access = access,
            .DateFinalized = dateFinalized,
            .EIMaxYear = eiMaxYear,
            .Enrolled = enrolled,
            .Status = status
        }

    End Function

End Module

Public Class EiStatus
    'Return (accesscode, enrolled, eisStatus, dateFinalize, EISMaxYear)
    Public Property Access As String
    Public Property Enrolled As String
    Public Property Status As String
    Public Property DateFinalized As String
    Public Property EIMaxYear As String

End Class
