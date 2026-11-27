namespace Calabonga.LearningJournal;

/// <summary>
/// Хранилище записей дневника.
/// </summary>
public interface IJournalStorage
{
    IReadOnlyList<StudyRecord> Load();

    void Save(IReadOnlyList<StudyRecord> records);
}
