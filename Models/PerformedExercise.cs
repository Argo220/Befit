
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models;

public class PerformedExercise
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Sesja treningowa")]
    public int TrainingSessionId { get; set; }
    public TrainingSession? TrainingSession { get; set; }

    [Required]
    [Display(Name = "Typ ćwiczenia")]
    public int ExerciseTypeId { get; set; }
    public ExerciseType? ExerciseType { get; set; }

    [Range(0, 1000, ErrorMessage = "Obciążenie musi być w zakresie 0–1000 kg.")]
    [Display(Name = "Obciążenie (kg)")]
    public decimal? LoadKg { get; set; }

    [Required]
    [Range(1, 20, ErrorMessage = "Liczba serii musi być w zakresie 1–20.")]
    [Display(Name = "Liczba serii")]
    public int Sets { get; set; }

    [Required]
    [Range(1, 1000, ErrorMessage = "Liczba powtórzeń musi być w zakresie 1–1000.")]
    [Display(Name = "Powtórzeń w serii")]
    public int RepsPerSet { get; set; }
}
