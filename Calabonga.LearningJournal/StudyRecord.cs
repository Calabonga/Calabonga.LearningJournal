namespace Calabonga.LearningJournal;

/// <summary>
/// Запись дневника: дата, тема, минуты занятий и необязательная заметка.
/// </summary>
public record StudyRecord(DateOnly Date, string Topic, int Minutes, string? Note = null)
{
    /// <summary>
    /// Наибольшее число минут в одной записи (10 часов).
    /// </summary>
    public const int MaxMinutes = 600;
}
