namespace Calabonga.LearningJournal;

/// <summary>
/// Баллы за учебный день.
/// </summary>
public static class Grading
{
    /// <summary>
    /// Баллы за день по сумме минут: меньше 0 — 0, до 30 — 1, до 60 — 2, до 120 — 3, от двух часов — 4.
    /// </summary>
    public static int GradeDay(int minutes) => minutes switch
    {
        < 0 => 0,
        < 30 => 1,
        < 60 => 2,
        < 120 => 3,
        _ => 4
    };
}
