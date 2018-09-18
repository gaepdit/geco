Imports Xunit

Public Class UnitTestTests

    <Fact>
    Private Sub PassingTest()
        Assert.Equal(4, Add(2, 2))
    End Sub

    '<Fact>
    'Private Sub FailingTest()
    '    Assert.Equal(5, Add(2, 2))
    'End Sub

    <Theory>
    <InlineData(1)>
    <InlineData(2)>
    <InlineData(3)>
    <InlineData(4)>
    Private Sub TestOddEven(value As Integer)
        If value Mod 2 = 0 Then
            Assert.False(IsOdd(value))
            Assert.True(IsEven(value))
        Else
            Assert.True(IsOdd(value))
            Assert.False(IsEven(value))
        End If

    End Sub

    Private Function Add(x As Integer, y As Integer) As Integer
        Return x + y
    End Function

    Private Function IsOdd(x As Integer) As Boolean
        Return x Mod 2 = 1
    End Function

    Private Function IsEven(x As Integer) As Boolean
        Return x Mod 2 = 0
    End Function

End Class
