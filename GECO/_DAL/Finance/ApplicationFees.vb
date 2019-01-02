Imports System.Data.SqlClient
Imports EpdIt.DBUtilities
Imports GECO.GecoModels

Namespace DAL
    Public Module ApplicationFees

        Public Function GetApplicationFeesInfo(appNumber As Integer) As ApplicationFeeInfo
            Dim query As String = " select ApplicationFeeApplies, " &
            "        i1.Description as ApplicationFeeDescription, " &
            "        ApplicationFeeAmount, " &
            "        ExpeditedFeeApplies, " &
            "        i2.Description as ExpeditedFeeDescription, " &
            "        ExpeditedFeeAmount, " &
            "        FeeDataFinalized, " &
            "        convert(date, DateFacilityNotifiedOfFees) as DateFacilityNotifiedOfFees, " &
            "        convert(date, DateFeeDataFinalized) as DateFeeDataFinalized, " &
            "        case " &
            "            when m.STRPERMITTYPE = 11 " &
            "                  then convert(bit, 1) " &
            "            else convert(bit, 0) " &
            "        end as ApplicationWithdrawn " &
            " from SSPPAPPLICATIONDATA d " &
            "      inner join SSPPAPPLICATIONMASTER m " &
            "              on d.STRAPPLICATIONNUMBER = m.STRAPPLICATIONNUMBER " &
            "      left join fees.RateItem i1 " &
            "              on d.ApplicationFeeType = i1.RateItemID " &
            "      left join fees.RateItem i2 " &
            "              on d.ExpeditedFeeType = i2.RateItemID " &
            " where convert(int, d.STRAPPLICATIONNUMBER) = @AppNumber "

            Dim dr As DataRow = DB.GetDataRow(query, New SqlParameter("@AppNumber", appNumber))

            If dr Is Nothing Then
                Return Nothing
            End If

            Return New ApplicationFeeInfo With {
                    .ApplicationID = appNumber,
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

        Public Function GetApplicationPayments(appNumber As Integer) As DataTable
            Dim query As String = "select " &
                "    convert(date, r.DepositDate) as [Date], " &
                "    X.AmountApplied   as [Amount applied], " &
                "    i.InvoiceId as [Invoice #] " &
                " from fees.Deposit r " &
                "    inner join fees.Deposit_Invoice X " &
                "        on r.DepositID = X.DepositID " &
                "    inner join fees.VW_Invoices i " &
                "        on X.InvoiceID = i.InvoiceID " &
                " where i.ApplicationID = @appNumber " &
                " order by r.DepositDate, i.InvoiceID"

            Return DB.GetDataTable(query, New SqlParameter("@appNumber", appNumber))
        End Function

        Public Function GetApplicationInvoices(appNumber As Integer) As DataTable
            Dim query As String = "select " &
                "    InvoiceID as [Invoice #], " &
                "    InvoiceGuid, " &
                "    convert(date, InvoiceDate) as [Invoice Date], " &
                "    TotalAmount        as Amount, " &
                " fees.InvoiceBalance(InvoiceID) as Balance, " &
                "    SettlementStatus   as Status " &
                " from fees.VW_Invoices" &
                " where ApplicationID = @appNumber " &
                " order by InvoiceID"

            Return DB.GetDataTable(query, New SqlParameter("@appNumber", appNumber))
        End Function

        Public Function IsInvoiceGeneratedForApplication(appNumber As Integer) As Boolean
            Dim query As String = "select convert(bit, count(*)) " &
                " from fees.VW_Invoices" &
                " where ApplicationID = @appNumber and Voided = 0"

            Return DB.GetBoolean(query, New SqlParameter("@appNumber", appNumber))
        End Function

        Public Function GenerateInvoice(appNumber As Integer, userId As Integer, ByRef invoiceId As Integer) As GenerateInvoiceResult
            Dim spName As String = "fees.GenerateInvoiceForPermitApplication"

            Dim params As SqlParameter() = {
                New SqlParameter("@AppNumber", appNumber),
                New SqlParameter("@CurrentUserID", userId)
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
    End Enum

End Namespace