using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Calabonga.LearningJournal;

/// <summary>
/// Хранит записи дневника в файле JSON с безопасной записью через временный файл.
/// </summary>
public sealed class JsonJournalStorage(string filePath) : IJournalStorage
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
    };

    private readonly string _filePath = filePath;

    /// <summary>
    /// Загружает записи. Если файла нет — пустой список.
    /// </summary>
    /// <exception cref="InvalidDataException">Файл повреждён; он сохранён под другим именем.</exception>
    public IReadOnlyList<StudyRecord> Load()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        try
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<StudyRecord>>(json, JsonOptions) ?? [];
        }
        catch (JsonException ex)
        {
            string backupPath = Path.ChangeExtension(_filePath, $".broken-{DateTime.Now:yyyyMMdd-HHmmss}.json");
            File.Move(_filePath, backupPath);
            throw new InvalidDataException($"Файл дневника повреждён и сохранён как {backupPath}.", ex);
        }
    }

    /// <summary>
    /// Сохраняет записи: сначала во временный файл, затем переносит его на место основного.
    /// </summary>
    public void Save(IReadOnlyList<StudyRecord> records)
    {
        string? folder = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(folder))
        {
            Directory.CreateDirectory(folder);
        }

        string json = JsonSerializer.Serialize(records, JsonOptions);
        string tempPath = _filePath + ".tmp";
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, _filePath, overwrite: true);
    }
}
