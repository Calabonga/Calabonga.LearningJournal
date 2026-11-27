namespace Calabonga.LearningJournal;

public record StudyRecord(DateOnly Date, string Topic, int Minutes, string? Note = null);
