Imports GECO.GecoModels
Imports GECO.GecoModels.ApbFacilityId
Imports Xunit

Public Class ApbFacilityIdTest

    <Theory>
    <InlineData(Nothing)>
    <InlineData("")>
    <InlineData("111")>
    <InlineData("ABC")>
    <InlineData("0010001")>
    <InlineData("001-0001")>
    Private Sub RejectsInvalidAirsNumbers(input As String)
        Assert.False(IsValidAirsNumberFormat(input))
    End Sub

    <Theory>
    <InlineData("00100001")>
    <InlineData("001-00001")>
    <InlineData("041300100001")>
    <InlineData("04-13-001-00001")>
    Private Sub AcceptsValidAirsNumbers(input As String)
        Assert.True(IsValidAirsNumberFormat(input))
    End Sub

    <Fact>
    Private Sub FacilityIdPropertiesCorrect()
        Dim airs As New ApbFacilityId("12300456")

        Assert.Equal("12300456", airs.ShortString)
        Assert.Equal("123", airs.CountySubstring)
        Assert.Equal("041312300456", airs.DbFormattedString)
        Assert.Equal("GA0000001312300456", airs.EpaFacilityIdentifier)
        Assert.Equal("123-00456", airs.FormattedString)
        Assert.Equal(12300456, airs.ToInt)
        Assert.Equal("123-0456", airs.PermitFormattedString)
    End Sub

    <Fact>
    Private Sub FacilityIdCTypeWorks()
        Dim airs = CType("123-00456", ApbFacilityId)

        Assert.Equal("12300456", airs.ShortString)
        Assert.Equal("123", airs.CountySubstring)
        Assert.Equal("041312300456", airs.DbFormattedString)
        Assert.Equal("GA0000001312300456", airs.EpaFacilityIdentifier)
        Assert.Equal("123-00456", airs.FormattedString)
        Assert.Equal(12300456, airs.ToInt)
        Assert.Equal("123-0456", airs.PermitFormattedString)
    End Sub

End Class
