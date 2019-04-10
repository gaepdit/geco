Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels

Namespace DAL
    Public Module Invoices

        Public Function InvoiceExists(invoiceId As Integer) As Boolean
            Return DB.SPGetBoolean("fees.InvoiceExists", New SqlParameter("@InvoiceID", invoiceId))
        End Function

        Public Function InvoiceHasPaymentsApplied(invoiceId As Integer) As Boolean
            Return DB.SPGetBoolean("fees.InvoiceHasPaymentsApplied", New SqlParameter("@InvoiceID", invoiceId))
        End Function

        Public Function GetInvoiceByGuid(invoiceGuid As Guid) As Invoice
            Dim ds As DataSet = DB.SPGetDataSet("fees.GetInvoiceByGuid", New SqlParameter("@InvoiceGuid", invoiceGuid))

            If ds Is Nothing OrElse ds.Tables.Count <> 3 OrElse ds.Tables(0).Rows.Count <> 1 Then Return Nothing

            Dim invoice As Invoice = InvoiceFromDataRow(ds.Tables(0).Rows(0))

            For Each dr As DataRow In ds.Tables(1).Rows
                invoice.InvoiceItems.Add(InvoiceItemFromDataRow(dr))
            Next

            For Each dr As DataRow In ds.Tables(2).Rows
                invoice.DepositsApplied.Add(DepositAppliedFromDataRow(dr))
            Next

            Return invoice
        End Function

        Private Function InvoiceFromDataRow(dr As DataRow) As Invoice
            Return New Invoice With {
                .InvoiceID = CInt(dr("InvoiceID")),
                .InvoiceGuid = CType(dr("InvoiceGuid"), Guid),
                .CeasedCollections = CBool(dr("CeasedCollections")),
                .Comment = GetNullableString(dr("Comment")),
                .FacilityID = New ApbFacilityId(CStr(dr("FacilityID"))),
                .FacilityName = GetNullableString(dr("FacilityName")),
                .InvoiceCategoryID = CChar(dr("InvoiceCategoryID")),
                .InvoiceDate = CDate(dr("InvoiceDate")),
                .DueDate = CDate(dr("DueDate")),
                .InvoiceType = New InvoiceType With {
                    .Active = CBool(dr("InvoiceTypeActive")),
                    .Description = GetNullableString(dr("InvoiceTypeDescription")),
                    .InvoiceTypeID = CInt(dr("InvoiceTypeID"))
                },
                .SettlementStatus = GetNullableString(dr("SettlementStatus")),
                .TotalAmountDue = CDec(dr("TotalAmount")),
                .PaymentsApplied = CDec(dr("PaymentsApplied")),
                .Voided = CBool(dr("Voided")),
                .VoidedDate = GetNullableDateTime(dr.Item("VoidedDate")),
                .ApplicationID = GetNullable(Of Integer)(dr("ApplicationID")),
                .FeeYear = GetNullable(Of Integer)(dr("FeeYear"))
            }
        End Function

        Private Function InvoiceItemFromDataRow(dr As DataRow) As InvoiceItem
            Return New InvoiceItem With {
                .InvoiceID = GetNullable(Of Integer?)(dr("InvoiceID")),
                .InvoiceItemID = CInt(dr("InvoiceItemID")),
                .Amount = CDec(dr("Amount")),
                .ItemStatus = CType(dr("ItemStatusID"), InvoiceItemStatus),
                .RateCategory = CType(dr("RateCategoryID"), FeeRateCategory),
                .ApplicationID = GetNullable(Of Integer?)(dr("ApplicationID")),
                .FacilityID = New ApbFacilityId(CStr(dr("FacilityID"))),
                .FeeYear = GetNullable(Of Integer?)(dr("FeeYear")),
                .InvoiceCategoryID = CChar(dr("InvoiceCategoryID"))
            }
        End Function

        Private Function DepositAppliedFromDataRow(dr As DataRow) As DepositApplied
            Return New DepositApplied With {
                .DepositAppliedID = CInt(dr("DepositAppliedID")),
                .DepositID = CInt(dr("DepositID")),
                .InvoiceID = CInt(dr("InvoiceID")),
                .FacilityID = New ApbFacilityId(CStr(dr("FacilityID"))),
                .AmountApplied = CDec(dr("AmountApplied")),
                .DepositDate = CDate(dr("DepositDate")),
                .InvoiceVoided = CBool(dr("InvoiceVoided")),
                .DepositDeleted = CBool(dr("DepositDeleted")),
                .BatchNumber = GetNullableString(dr("BatchNumber")),
                .CheckNumber = GetNullableString(dr("CheckNumber")),
                .CreditCardConf = GetNullableString(dr("CreditCardConf")),
                .DepositNumber = GetNullableString(dr("DepositNumber"))
            }
        End Function

    End Module
End Namespace