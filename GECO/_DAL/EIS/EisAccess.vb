Imports System.Data.SqlClient
Imports GaEpd.DBUtilities
Imports GECO.GecoModels

Namespace DAL.EIS
    Public Module EisAccess

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

        Public Function GetEiStatus(airs As ApbFacilityId) As EisStatus
            NotNull(airs, NameOf(airs))

            Dim query As String = "select INVENTORYYEAR, EISSTATUSCODE, EISACCESSCODE,
               convert(bit, STROPTOUT) as OPTOUT, convert(bit, STRENROLLMENT) as ENROLLMENT,
               DATFINALIZE, STRCONFIRMATIONNUMBER, STROPTOUTREASON
            FROM EIS_ADMIN
            where FACILITYSITEID = @airs
              and INVENTORYYEAR =
                  (select max(INVENTORYYEAR)
                   FROM EIS_ADMIN
                   where FACILITYSITEID = @airs)"

            Dim param As New SqlParameter("@airs", airs.ShortString)

            Dim dr As DataRow = DB.GetDataRow(query, param)

            If dr Is Nothing Then
                Return New EisStatus With {.AccessCode = 3, .Enrolled = False}
            End If

            Dim eiStatus As New EisStatus With {
                .MaxYear = CInt(dr("INVENTORYYEAR"))
            }

            If eiStatus.MaxYear <> Date.Now.Year - 1 Then
                eiStatus.AccessCode = 0
                eiStatus.StatusCode = 0
                eiStatus.Enrolled = False

                Return eiStatus
            End If

            ' enrollment status: 0 = not enrolled; 1 = enrolled for EI year
            eiStatus.Enrolled = CBool(dr.Item("ENROLLMENT"))

            If eiStatus.Enrolled Then
                eiStatus.StatusCode = CInt(dr.Item("EISSTATUSCODE"))
                eiStatus.AccessCode = CInt(dr.Item("EISACCESSCODE"))
                eiStatus.OptOut = GetNullable(Of Boolean?)(dr.Item("OPTOUT"))
                eiStatus.DateFinalized = GetNullableDateTime(dr.Item("DATFINALIZE"))
                eiStatus.ConfirmationNumber = GetNullableString(dr.Item("STRCONFIRMATIONNUMBER")).EmptyStringIfNothing()

                Dim optOutReason As String = GetNullableString(dr.Item("STROPTOUTREASON"))
                If String.IsNullOrEmpty(optOutReason) Then
                    eiStatus.OptOutReason = Nothing
                Else
                    eiStatus.OptOutReason = CInt(optOutReason)
                End If
            End If

            Return eiStatus
        End Function

    End Module

    Public Class EisStatus
        Public Property AccessCode As Integer
        Public Property Enrolled As Boolean
        Public Property StatusCode As Integer
        Public Property DateFinalized As Date?
        Public Property MaxYear As Integer
        Public Property OptOut As Boolean?
        Public Property OptOutReason As Integer?
        Public Property ConfirmationNumber As String = ""
    End Class

End Namespace
