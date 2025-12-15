
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BeFit.Models;

public class TrainingSession : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Data i czas rozpoczęcia są wymagane.")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Początek sesji")]
    public DateTime StartTime { get; set; }

    [Required(ErrorMessage = "Data i czas zakończenia są wymagane.")]
    [DataType(DataType.DateTime)]
    [Display(Name = "Koniec sesji")]
    public DateTime EndTime { get; set; }

    [Required]
    public string UserId { get; set; } = null!;
    public IdentityUser? User { get; set; }

    public ICollection<PerformedExercise> PerformedExercises { get; set; } = new List<PerformedExercise>();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (EndTime <= StartTime)
            yield return new ValidationResult("Koniec sesji musi być później niż początek.",
                new[] { nameof(EndTime) });

        if (StartTime < DateTime.Today.AddYears(-1))
            yield return new ValidationResult("Data rozpoczęcia jest zbyt odległa w przeszłości.",
                new[] { nameof(StartTime) });
    }
}
