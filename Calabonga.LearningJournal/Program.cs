int[] minutes = [50, 30, 45, 60, 0, 90, 120, 40, 45, 75];

var sum = 0;
var count = 0;
for (var i = 1; i < minutes.Length; i++)
{
    if (minutes[i] > 45)
    {
        sum += minutes[i];
        count++;
    }
}

var average = (double)sum / count;
Console.WriteLine($"Средняя длительность дней с выполненной целью: {average:F1}");


public static class Grading
{
    public static int GradeDay(int minutes) => minutes switch
    {
        < 0 => 0,
        < 30 => 1,
        < 60 => 2,
        < 120 => 3,
        _ => 4
    };
}
