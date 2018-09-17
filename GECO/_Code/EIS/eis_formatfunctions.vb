Public Module eis_formatfunctions

    Public Sub HideTextBoxBorders(ByVal root As Control)
        For Each ctrl As Control In root.Controls
            If TypeOf ctrl Is TextBox AndAlso CType(ctrl, TextBox).TextMode <> TextBoxMode.MultiLine Then
                CType(ctrl, TextBox).BorderColor = Drawing.Color.White
            Else
                HideTextBoxBorders(ctrl)
            End If
        Next ctrl
    End Sub

End Module