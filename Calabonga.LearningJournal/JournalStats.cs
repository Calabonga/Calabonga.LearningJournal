namespace Calabonga.LearningJournal;

/// <summary>
/// Подсчёты для отчёта «Дневника».
/// </summary>
public static class JournalStats
{
    public static int TotalMinutes(IEnumerable<StudyRecord> records)
        => records.Sum(r => r.Minutes);

    /// <summary>
    /// Число разных дней, в которые были занятия.
    /// </summary>
    public static int StudyDays(IEnumerable<StudyRecord> records)
        => records.Select(r => r.Date).Distinct().Count();

    /// <summary>
    /// Число дней, в которые сумма минут всех записей дня достигла дневной цели.
    /// </summary>
    public static int GoalDays(IEnumerable<StudyRecord> records, int dailyGoal)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(dailyGoal);

        return records
            .GroupBy(r => r.Date)
            .Count(g => g.Sum(r => r.Minutes) >= dailyGoal);
    }

    /// <summary>
    /// Темы с наибольшим числом минут, по убыванию; темы сравниваются без учёта регистра.
    /// </summary>
    public static IReadOnlyList<(string Topic, int Minutes)> TopTopics(IEnumerable<StudyRecord> records, int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);

        return records
            .GroupBy(r => r.Topic, StringComparer.OrdinalIgnoreCase)
            .Select(g => (Topic: g.Key, Minutes: g.Sum(r => r.Minutes)))
            .OrderByDescending(t => t.Minutes)
            .ThenBy(t => t.Topic, StringComparer.OrdinalIgnoreCase)
            .Take(count)
            .ToList();
    }
}
