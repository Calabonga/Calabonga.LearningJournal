namespace Calabonga.LearningJournal.Tests;

public sealed class JsonJournalStorageTests : IDisposable
{
    private readonly string _folder = Path.Combine(
        Path.GetTempPath(), "Calabonga.LearningJournal.Tests", Guid.NewGuid().ToString("N"));

    private string FilePath => Path.Combine(_folder, "journal.json");

    public void Dispose()
    {
        if (Directory.Exists(_folder))
        {
            Directory.Delete(_folder, recursive: true);
        }
    }

    [Fact]
    public void Load_NoFile_ReturnsEmpty()
        => Assert.Empty(new JsonJournalStorage(FilePath).Load());

    [Fact]
    public void SaveThenLoad_ReturnsSameRecords()
    {
        List<StudyRecord> records =
        [
            new(new DateOnly(2026, 11, 26), "Тесты", 60, "Первый тест"),
            new(new DateOnly(2026, 11, 27), "Git", 30)
        ];

        new JsonJournalStorage(FilePath).Save(records);
        IReadOnlyList<StudyRecord> loaded = new JsonJournalStorage(FilePath).Load();

        Assert.Equal(records, loaded);
    }

    [Fact]
    public void Save_CyrillicTopic_WrittenAsIsWithoutEscaping()
    {
        new JsonJournalStorage(FilePath).Save([new(new DateOnly(2026, 11, 26), "Тесты", 60)]);

        Assert.Contains("Тесты", File.ReadAllText(FilePath));
    }

    [Fact]
    public void Save_NoTempFileLeft()
    {
        new JsonJournalStorage(FilePath).Save([new(new DateOnly(2026, 11, 26), "Тесты", 60)]);

        Assert.False(File.Exists(FilePath + ".tmp"));
    }

    [Fact]
    public void Load_BrokenFile_ThrowsInvalidDataAndKeepsCopy()
    {
        Directory.CreateDirectory(_folder);
        File.WriteAllText(FilePath, "это не JSON");

        Assert.Throws<InvalidDataException>(() => new JsonJournalStorage(FilePath).Load());
        Assert.Single(Directory.GetFiles(_folder, "journal.broken-*.json"));
        Assert.False(File.Exists(FilePath));
    }
}
