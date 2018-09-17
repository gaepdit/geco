Partial Class EIS_sccfinder
    Inherits Page

    Protected Sub rcbLevel1_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rcbLevel1.SelectedIndexChanged, rcbLevel2.SelectedIndexChanged, rcbLevel3.SelectedIndexChanged, rcbLevel4.SelectedIndexChanged
        Try
            Dim sCC As String = GetSCC(Me.rcbLevel1.SelectedItem.Text, Me.rcbLevel2.SelectedItem.Text, Me.rcbLevel3.SelectedItem.Text, Me.rcbLevel4.SelectedItem.Text)
            If Not String.IsNullOrEmpty(sCC) Then
                Me.txtSCC.Text = sCC
                Me.btnUseSCC.Visible = True
                Me.txtSCC.Visible = True
            Else
                Me.txtSCC.Text = "No Code found!"
                Me.btnUseSCC.Visible = False
                Me.txtSCC.Visible = False
            End If
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub
End Class