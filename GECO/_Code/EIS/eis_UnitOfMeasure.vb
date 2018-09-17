Imports System.Data.SqlClient

Public Module eis_UnitOfMeasure

    Public Function GetRequiredUomForScc(SourceClassificationCode As String) As Tuple(Of String, String)
        If SourceClassificationCode Is Nothing Then
            Return Nothing
        End If

        Dim query As String = " select " &
        "     UnitOfMeasure, " &
        "     STRDESC as UnitOfMeasureDesc " &
        " from EISLK_SCC_UOM u " &
        "     inner join EISLK_CALCPARAMUOMCODE c " &
        "         on u.UnitOfMeasure = c.CALCPARAMUOMCODE " &
        " where SourceClassificationCode = @SourceClassificationCode "

        Dim param As New SqlParameter("@SourceClassificationCode", SourceClassificationCode)

        Dim dr = DB.GetDataRow(query, param)

        If dr IsNot Nothing Then
            Return New Tuple(Of String, String)(dr("UnitOfMeasure").ToString, dr("UnitOfMeasureDesc").ToString)
        End If

        Return Nothing
    End Function

    Function GetParamUomDesc(code As String) As String
        Dim query As String = " select STRDESC " &
        " from EISLK_CALCPARAMUOMCODE " &
        " where CALCPARAMUOMCODE = @code "

        Dim param As New SqlParameter("@code", code)

        Return DB.GetString(query, param)
    End Function

End Module