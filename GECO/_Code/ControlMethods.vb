Module ControlMethods

    Public Sub ClearAll(c As Control)
        NotNull(c, NameOf(c))

        For Each Ctrl As Control In c.Controls
            Select Case TypeName(Ctrl)
                Case "TextBox" 'Do the following code if control is a Text Box
                    CType(Ctrl, TextBox).Text = String.Empty
                Case "CheckBox" 'Do the following code if control is a Check Box
                    CType(Ctrl, CheckBox).Checked = False
                Case "DropDownList" 'Do the following code if control is a Drop Down List
                    CType(Ctrl, DropDownList).ClearSelection()
                Case "CheckBoxList" 'Do the following code if control is a Checkbox List
                    CType(Ctrl, CheckBoxList).ClearSelection()
                Case "Label" 'Do the following code if control is a Label
                    CType(Ctrl, Label).Text = String.Empty
                Case "RadioButtonList" 'Do the following code if control is a Radiobutton List
                    CType(Ctrl, RadioButtonList).ClearSelection()
                Case Else
                    If Ctrl.Controls.Count > 0 Then 'Check for container control
                        ClearAll(Ctrl) 'Recursively call sub for controls in container
                    End If
            End Select
        Next
    End Sub

End Module
