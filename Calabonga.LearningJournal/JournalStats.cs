namespace Calabonga.LearningJournal;

/// <summary>
/// Подсчёты для отчёта «Дневника».
/// </summary>
public static class JournalStats
{
    public static int TotalMinutes(IEnumerable<StudyRecord> records)
        => records.Sum(r => r.Minutes);

    public static int GoalDays(IEnumerable<StudyRecord> records, int dailyGoal)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dailyGoal);

        return records
            .GroupBy(r => r.Date)
            .Count(g => g.Sum(r => r.Minutes) >= dailyGoal);
    }
}