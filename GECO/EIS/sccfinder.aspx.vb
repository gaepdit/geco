Partial Class EIS_sccfinder
    Inherits Page

    Private Sub LookUpScc()
        Dim SCC As String = GetSccValue(rcbLevel1.SelectedItem.Text, rcbLevel2.SelectedItem.Text, rcbLevel3.SelectedItem.Text, rcbLevel4.SelectedItem.Text)

        If Not String.IsNullOrEmpty(SCC) Then
            lblSCC.Text = SCC
            lblSCC.Visible = True
            btnUseSCC.Visible = True
        Else
            lblSCC.Visible = False
            btnUseSCC.Visible = False
        End If
    End Sub

    Private Sub btnLookUp_Click(sender As Object, e As EventArgs) Handles btnLookUp.Click
        LookUpScc()
    End Sub

End Class