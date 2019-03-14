Partial Class EIS_sccfinder
    Inherits Page

    Private Sub GetSccValue()
        Dim SCC As String = GetSCC(rcbLevel1.SelectedItem.Text, rcbLevel2.SelectedItem.Text, rcbLevel3.SelectedItem.Text, rcbLevel4.SelectedItem.Text)

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
        GetSccValue()
    End Sub

End Class