Partial Class EIS_sccfinder
    Inherits Page

    Private Sub LookUpScc()
        Dim SccDetails As SccDetails = GetSccDetails(rcbLevel1.SelectedItem.Text, rcbLevel2.SelectedItem.Text, rcbLevel3.SelectedItem.Text, rcbLevel4.SelectedItem.Text)

        If SccDetails IsNot Nothing Then
            dSccDetails.Visible = True
            lblSCC.Text = SccDetails.SCC
            lCategory.Text = SccDetails.Category
            lDesc.Text = SccDetails.Description
            lShortName.Text = SccDetails.ShortName
            lSector.Text = SccDetails.Sector
            lUsage.Text = SccDetails.UsageNotes
            lUpdated.Text = SccDetails.LastUpdated
            lTier1.Text = SccDetails.Tier1
            lTier2.Text = SccDetails.Tier2
            lTier3.Text = SccDetails.Tier3
        Else
            lblSCC.Visible = False
            btnUseSCC.Visible = False
        End If
    End Sub

    Private Sub btnLookUp_Click(sender As Object, e As EventArgs) Handles btnLookUp.Click
        LookUpScc()
    End Sub

End Class