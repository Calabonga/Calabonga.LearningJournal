namespace Calabonga.LearningJournal;

/// <summary>
/// Ввод значений с клавиатуры с повтором вопроса при ошибке.
/// </summary>
public static class ConsoleInput
{
    public static int ReadInt(string prompt) => ReadInt(prompt, int.MinValue, int.MaxValue);

    public static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            string line = ReadLineOrThrow(prompt);
            if (int.TryParse(line, out int value) && value >= min && value <= max)
            {
                return value;
            }

            Console.WriteLine($"Нужно целое число от {min} до {max}. Попробуйте ещё раз.");
        }
    }

    public static int ReadMinutes(string prompt)
    {
        while (true)
        {
            string line = ReadLineOrThrow(prompt);
            if (TryReadMinutes(line, out int minutes) && minutes <= StudyRecord.MaxMinutes)
            {
                return minutes;
            }

            Console.WriteLine($"Нужно целое число минут от 0 до {StudyRecord.MaxMinutes}. Попробуйте ещё раз.");
        }
    }

    /// <summary>
    /// Разбирает строку в целое число минут. Отрицательные значения не принимаются.
    /// </summary>
    /// <returns><c>true</c>, если строка — неотрицательное целое число; иначе <c>false</c> и 0.</returns>
    public static bool TryReadMinutes(string? text, out int minutes)
    {
        if (int.TryParse(text?.Trim(), out int value) && value >= 0)
        {
            minutes = value;
            return true;
        }

        minutes = 0;
        return false;
    }

    public static string ReadText(string prompt) => ReadLineOrThrow(prompt);

    private static string ReadLineOrThrow(string prompt)
    {
        Console.Write(prompt);
        string? line = Console.ReadLine();
        return line?.Trim() ?? throw new InvalidOperationException("Ввод закончился.");
    }
}
