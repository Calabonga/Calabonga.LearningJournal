using Calabonga.LearningJournal;

const int DailyGoalMinutes = 45;
const int TopTopicsCount = 3;

string folder = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "Calabonga.LearningJournal");
IJournalStorage storage = new JsonJournalStorage(Path.Combine(folder, "journal.json"));

List<StudyRecord> records;
try
{
    records = [.. storage.Load()];
}
catch (InvalidDataException ex)
{
    Console.WriteLine(ex.Message);
    Console.WriteLine("Начинаем с пустого дневника.");
    records = [];
}
catch (IOException ex)
{
    Console.WriteLine($"Не удалось прочитать дневник: {ex.Message}");
    return;
}

Console.WriteLine($"Загружено записей: {records.Count}");

while (true)
{
    ShowMenu();
    int choice = ConsoleInput.ReadInt("Ваш выбор: ", 0, 3);
    if (choice == 0)
    {
        break;
    }

    switch (choice)
    {
        case 1:
            AddRecord(records);
            TrySave(storage, records);
            break;
        case 2:
            ShowRecords(records);
            break;
        case 3:
            ShowReport(records);
            break;
    }
}

Console.WriteLine("До свидания!");

static void ShowMenu() => Console.WriteLine("""

    === Дневник обучения ===
    1. Добавить запись
    2. Показать записи
    3. Отчёт
    0. Выход
    """);

static void AddRecord(List<StudyRecord> records)
{
    string topic = ConsoleInput.ReadText("Тема: ");
    if (string.IsNullOrWhiteSpace(topic))
    {
        Console.WriteLine("Тема не может быть пустой.");
        return;
    }

    int minutes = ConsoleInput.ReadMinutes("Минут: ");
    string note = ConsoleInput.ReadText("Заметка (Enter — без заметки): ");

    records.Add(new StudyRecord(
        DateOnly.FromDateTime(DateTime.Today),
        topic,
        minutes,
        string.IsNullOrWhiteSpace(note) ? null : note));
    Console.WriteLine($"Добавлено. Всего записей: {records.Count}");
}

static void ShowRecords(List<StudyRecord> records)
{
    if (records.Count == 0)
    {
        Console.WriteLine("Записей пока нет.");
        return;
    }

    foreach (StudyRecord record in records.OrderBy(r => r.Date))
    {
        string note = record.Note is null ? "" : $" — {record.Note}";
        Console.WriteLine($"{record.Date:dd.MM.yyyy}  {record.Topic,-20} {record.Minutes,4} мин.{note}");
    }
}

static void ShowReport(List<StudyRecord> records)
{
    if (records.Count == 0)
    {
        Console.WriteLine("Записей пока нет.");
        return;
    }

    int totalMinutes = JournalStats.TotalMinutes(records);
    int studyDays = JournalStats.StudyDays(records);
    int goalDays = JournalStats.GoalDays(records, DailyGoalMinutes);

    Console.WriteLine($"Всего: {totalMinutes / 60.0:F1} ч за {studyDays} дн.");
    Console.WriteLine($"Цель {DailyGoalMinutes} мин. выполнена в {goalDays} днях из {studyDays}");
    Console.WriteLine("Лучшие темы:");
    foreach ((string topic, int minutes) in JournalStats.TopTopics(records, TopTopicsCount))
    {
        Console.WriteLine($"  {topic,-20} {minutes,5} мин.");
    }
}

static void TrySave(IJournalStorage storage, List<StudyRecord> records)
{
    try
    {
        storage.Save(records);
    }
    catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
    {
        Console.WriteLine($"Запись добавлена, но не сохранена: {ex.Message}");
    }
}
