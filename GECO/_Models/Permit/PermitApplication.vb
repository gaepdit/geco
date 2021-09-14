Namespace GecoModels
    Public Class PermitApplication

        ' Basic info
        Public Property AppNumber As Integer
        Public Property FacilityID As ApbFacilityId
        Public Property StaffResponsible As ApbStaff
        Public Property UnitResponsible As String ' APBUNIT
        Public Property ApplicationType As String ' STRAPPLICATIONTYPE
        Public Property ApplicationResult As String ' STRPERMITTYPE
        Public Property ReasonForApplication As String ' STRAPPLICATIONNOTES

        ' Application Status
        Public Property Status As String ' generated
        Public Property StatusDate As Date? ' generated
        Public Property DateApplicationFinalized As Date?
        Public Property ApplicationWithdrawn As Boolean = False ' generated
        Public Property DateApplicationWithdrawn As Date?
        Public Property PublicAdvisoryNeeded As String

        ' Permit
        Public Property PermitNumber As String
        Public Property DatePermitIssued As Date?
        Public Property PermitFileName As String

        Public Property PermitNumberInDB As String
            Get
                ' Permit number stored in DB has had hyphens removed 😒
                Return PermitNumber.Replace("-", "")
            End Get
            Set(value As String)
                PermitNumber = FormatPermitNumber(value.Trim())
            End Set
        End Property

        ' Facility info
        Public Property FacilityName As String
        Public Property FacilityAddress As Address
        Public Property FacilitySicCode As String
        Public Property FacilityNaicsCode As String
        Public Property FacilityCounty As String
        Public Property FacilityDistrict As String
        Public Property FacilityDescription As String

        ' Tracking dates
        Public Property DateSentByFacility As Date?
        Public Property DateReceivedByApb As Date?
        Public Property DateAssignedToStaff As Date?
        Public Property DateReassignedToStaff As Date?
        Public Property DateAcknowledgementLetterSent As Date?
        Public Property DatePAExpires As Date?
        Public Property DateToUnitManager As Date?
        Public Property DateToProgramManager As Date?
        Public Property DateDraftIssued As Date?
        Public Property DatePNExpires As Date?
        Public Property DateEpaCommentPeriodEnds As Date?
        Public Property DateEpaCommentPeriodWaived As Date?
        Public Property DateToAdministrativeReview As Date?

        ' Fees info
        Public Property IsInvoiceGenerated As Boolean
        Public Property ApplicationFeeInfo As ApplicationFeeInfo
        Public Property ApplicationInvoices As DataTable
        Public Property ApplicationPayments As DataTable

        Public Shared Function FormatPermitNumber(value As String) As String
            If String.IsNullOrWhiteSpace(value) Then
                Return Nothing
            End If

            If value.Substring(0, 3) = "ERC" Then
                Return ConcatNonEmptyStrings("-", {value.Substring(0, 3), value.SubstringFrom(3)})
            End If

            If Not Char.IsDigit(value(0)) OrElse value.Contains(" ") Then
                Return value
            End If

            If value.Length() = 15 AndAlso IsNumeric(Left(value, 11)) AndAlso Not IsNumeric(value.Substring(11, 1)) Then
                Return ConcatNonEmptyStrings("-", {value.Substring(0, 4), value.Substring(4, 3), value.Substring(7, 4),
                                             value.Substring(11, 1), value.Substring(12, 2), value.SubstringFrom(14)})
            End If

            Return ConcatNonEmptyStrings("-", {value.Substring(0, 4), value.Substring(4, 3), value.SubstringFrom(7)})
        End Function

        Public Function GetPermitFileLink() As String
            If String.IsNullOrEmpty(PermitFileName) Then
                Return Nothing
            End If

            Return "https://permitsearch.gaepd.org/permit.aspx?id=" & PermitFileName
        End Function

        Public Shared Function GetPermitAirsSearchLink(airs As ApbFacilityId) As String
            If airs Is Nothing Then
                Return Nothing
            End If

            Return "https://permitsearch.gaepd.org/?AirsNumber=" & airs.ShortString
        End Function

    End Class
End Namespace
