
namespace BeFit.Models.Stats;

public class ExerciseStatsRow
{
    public string ExerciseName { get; set; } = string.Empty;
    public int TimesPerformed { get; set; }
    public int TotalReps { get; set; }
    public decimal? AvgLoad { get; set; }
    public decimal? MaxLoad { get; set; }
}
