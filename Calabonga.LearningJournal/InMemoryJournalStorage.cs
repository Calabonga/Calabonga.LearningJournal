namespace Calabonga.LearningJournal;

/// <summary>
/// Хранит записи в памяти. Используется в тестах и для проверки логики без диска.
/// </summary>
public sealed class InMemoryJournalStorage : IJournalStorage
{
    private List<StudyRecord> _records = [];

    public IReadOnlyList<StudyRecord> Load() => [.. _records];

    public void Save(IReadOnlyList<StudyRecord> records) => _records = [.. records];
}
