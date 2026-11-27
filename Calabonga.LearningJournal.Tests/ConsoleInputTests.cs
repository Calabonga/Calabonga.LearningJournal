namespace Calabonga.LearningJournal.Tests;

public class ConsoleInputTests
{
    [Theory]
    [InlineData("45", true, 45)]
    [InlineData(" 45 ", true, 45)]
    [InlineData("0", true, 0)]
    [InlineData("", false, 0)]
    [InlineData("сорок", false, 0)]
    [InlineData("-10", false, 0)]
    [InlineData(null, false, 0)]
    public void TryReadMinutes_Text_ReturnsExpected(string? text, bool expectedOk, int expectedMinutes)
    {
        bool ok = ConsoleInput.TryReadMinutes(text, out int minutes);

        Assert.Equal(expectedOk, ok);
        Assert.Equal(expectedMinutes, minutes);
    }
}
