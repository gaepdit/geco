Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels

Namespace DAL
    Public Module ApplicationFees

        Public Function ApplicationFeeInfoFromDataRow(appNumber As Integer, dr As DataRow) As ApplicationFeeInfo
            If dr Is Nothing Then
                Return Nothing
            End If

            Return New ApplicationFeeInfo With {
                    .ApplicationID = CInt(dr.Item("AppNumber")),
                    .ApplicationWithdrawn = CBool(dr.Item("ApplicationWithdrawn")),
                    .ApplicationFeeApplies = CBool(dr.Item("ApplicationFeeApplies")),
                    .ApplicationFeeDescription = GetNullableString(dr.Item("ApplicationFeeDescription")),
                    .ApplicationFeeAmount = GetNullable(Of Decimal)(dr.Item("ApplicationFeeAmount")),
                    .ExpeditedFeeApplies = CBool(dr.Item("ExpeditedFeeApplies")),
                    .ExpeditedFeeDescription = GetNullableString(dr.Item("ExpeditedFeeDescription")),
                    .ExpeditedFeeAmount = GetNullable(Of Decimal)(dr.Item("ExpeditedFeeAmount")),
                    .FeeDataFinalized = CBool(dr.Item("FeeDataFinalized")),
                    .DateFeeDataFinalized = GetNullableDateTime(dr.Item("DateFeeDataFinalized")),
                    .DateFacilityNotifiedOfFees = GetNullableDateTime(dr.Item("DateFacilityNotifiedOfFees"))
                }
        End Function

        Public Function IsInvoiceGeneratedForApplication(appNumber As Integer) As Boolean
            Return DB.GetBoolean("select fees.IsInvoiceGeneratedForApplication(@AppNumber)", New SqlParameter("@AppNumber", appNumber))
        End Function

        Public Function GenerateInvoice(appNumber As Integer, userId As Integer, ByRef invoiceId As Integer) As GenerateInvoiceResult
            Dim spName As String = "fees.GenerateInvoiceForPermitApplication"

            Dim params As SqlParameter() = {
                New SqlParameter("@AppNumber", appNumber),
                New SqlParameter("@UserID", userId)
            }

            Dim returnValue As Integer = -1

            invoiceId = DB.SPGetInteger(spName, params, returnValue)

            Return returnValue
        End Function

    End Module

    Public Enum GenerateInvoiceResult
        DbError = -1
        Success = 0
        NoApplication = 1
        InvoiceExists = 2
        NoLineItems = 3
        InvalidInvoiceTotal = 4
        InvalidFacilityId = 5
    End Enum

End Namespace