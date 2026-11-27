namespace Calabonga.LearningJournal.Tests;

public class InMemoryJournalStorageTests
{
    [Fact]
    public void Load_NothingSaved_ReturnsEmpty()
        => Assert.Empty(new InMemoryJournalStorage().Load());

    [Fact]
    public void Save_ListChangedAfterSave_StoredCopyUnchanged()
    {
        InMemoryJournalStorage storage = new();
        List<StudyRecord> records = [new(new DateOnly(2026, 11, 26), "Тесты", 60)];

        storage.Save(records);
        records.Add(new StudyRecord(new DateOnly(2026, 11, 27), "Git", 30));

        Assert.Single(storage.Load());
    }
}
