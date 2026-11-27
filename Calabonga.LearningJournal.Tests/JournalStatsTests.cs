namespace Calabonga.LearningJournal.Tests;

public class JournalStatsTests
{
    [Fact]
    public void GoalDays_TwoRecordsSameDay_SumsMinutesPerDay()
    {
        // Подготовка
        DateOnly day1 = new(2026, 11, 10);
        DateOnly day2 = new(2026, 11, 11);
        List<StudyRecord> records =
        [
            new(day1, "Циклы", 30),
            new(day1, "Массивы", 20),
            new(day2, "Методы", 40)
        ];

        // Действие
        var result = JournalStats.GoalDays(records, dailyGoal: 45);

        // Проверка
        Assert.Equal(1, result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void GoalDays_NonPositiveGoal_ThrowsArgumentOutOfRange(int dailyGoal)
    {
        var ex = Assert.Throws<ArgumentOutOfRangeException>(
            () => JournalStats.GoalDays([], dailyGoal));

        Assert.Equal("dailyGoal", ex.ParamName);
    }
}
