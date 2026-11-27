namespace Calabonga.LearningJournal.Tests;

public class JournalStatsTests
{
    private static readonly DateOnly Day1 = new(2026, 11, 10);
    private static readonly DateOnly Day2 = new(2026, 11, 11);

    [Fact]
    public void TotalMinutes_EmptyList_ReturnsZero()
        => Assert.Equal(0, JournalStats.TotalMinutes([]));

    [Fact]
    public void TotalMinutes_SeveralRecords_ReturnsSum()
    {
        List<StudyRecord> records = [new(Day1, "Циклы", 30), new(Day1, "Массивы", 20), new(Day2, "Методы", 40)];

        Assert.Equal(90, JournalStats.TotalMinutes(records));
    }

    [Fact]
    public void StudyDays_TwoRecordsSameDay_CountsDayOnce()
    {
        List<StudyRecord> records = [new(Day1, "Циклы", 30), new(Day1, "Массивы", 20), new(Day2, "Методы", 40)];

        Assert.Equal(2, JournalStats.StudyDays(records));
    }

    [Fact]
    public void GoalDays_TwoRecordsSameDay_SumsMinutesPerDay()
    {
        // Подготовка
        List<StudyRecord> records =
        [
            new(Day1, "Циклы", 30),
            new(Day1, "Массивы", 20),
            new(Day2, "Методы", 40)
        ];

        // Действие
        int result = JournalStats.GoalDays(records, dailyGoal: 45);

        // Проверка
        Assert.Equal(1, result);
    }

    [Theory]
    [InlineData(45, 1)]
    [InlineData(44, 0)]
    public void GoalDays_OneDayAtBoundary_ReturnsExpected(int minutes, int expected)
    {
        List<StudyRecord> records = [new(Day1, "Тесты", minutes)];

        Assert.Equal(expected, JournalStats.GoalDays(records, dailyGoal: 45));
    }

    [Fact]
    public void GoalDays_EmptyList_ReturnsZero()
        => Assert.Equal(0, JournalStats.GoalDays([], dailyGoal: 45));

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GoalDays_NonPositiveGoal_ThrowsArgumentOutOfRange(int dailyGoal)
    {
        ArgumentOutOfRangeException ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => JournalStats.GoalDays([], dailyGoal));

        Assert.Equal("dailyGoal", ex.ParamName);
    }

    [Fact]
    public void TopTopics_TopicsInDifferentCase_GroupedAndOrderedByMinutes()
    {
        List<StudyRecord> records =
        [
            new(Day1, "LINQ", 30),
            new(Day1, "Циклы", 50),
            new(Day2, "linq", 40),
            new(Day2, "Методы", 10)
        ];

        IReadOnlyList<(string Topic, int Minutes)> top = JournalStats.TopTopics(records, count: 2);

        Assert.Equal(2, top.Count);
        Assert.Equal(("LINQ", 70), top[0]);
        Assert.Equal(("Циклы", 50), top[1]);
    }
}
