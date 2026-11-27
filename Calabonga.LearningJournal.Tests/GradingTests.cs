namespace Calabonga.LearningJournal.Tests;

public class GradingTests
{
    [Fact]
    public void GradeDay_NegativeMinutes_ReturnsZero()
        => Assert.Equal(0, Grading.GradeDay(-5));

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(29, 1)]
    [InlineData(30, 2)]
    [InlineData(59, 2)]
    [InlineData(60, 3)]
    [InlineData(119, 3)]
    [InlineData(120, 4)]
    public void GradeDay_Boundaries(int minutes, int expected)
        => Assert.Equal(expected, Grading.GradeDay(minutes));

    [Fact]
    public void GradeDay_AlmostTwoHours_ReturnsThree()
        => Assert.Equal(3, Grading.GradeDay(119));
}
