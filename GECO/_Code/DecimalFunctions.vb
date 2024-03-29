﻿Imports GaEpd.DBUtilities

Public Module DecimalFunctions

    Public Function NullableDecimalProduct(a As Object, b As Object) As Decimal?
        Dim aNull As Integer? = GetNullable(Of Integer?)(a)
        Dim bNull As Decimal? = GetNullable(Of Decimal?)(b)

        If aNull.HasValue AndAlso bNull.HasValue Then
            Return aNull.Value * bNull.Value
        End If

        Return Nothing
    End Function

End Module
