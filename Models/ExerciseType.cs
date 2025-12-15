
using System.ComponentModel.DataAnnotations;

namespace BeFit.Models;

public class ExerciseType
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Nazwa musi mieć od 2 do 80 znaków.")]
    [Display(Name = "Nazwa ćwiczenia")]
    public string Name { get; set; } = string.Empty;

    [StringLength(300, ErrorMessage = "Opis może mieć maksymalnie 300 znaków.")]
    [Display(Name = "Opis (opcjonalnie)")]
    public string? Description { get; set; }
}
