
using BeFit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<ExerciseType> ExerciseTypes => Set<ExerciseType>();
    public DbSet<TrainingSession> TrainingSessions => Set<TrainingSession>();
    public DbSet<PerformedExercise> PerformedExercises => Set<PerformedExercise>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<PerformedExercise>()
            .Property(p => p.LoadKg)
            .HasPrecision(6, 2);

        builder.Entity<PerformedExercise>()
            .HasOne(p => p.TrainingSession)
            .WithMany(s => s.PerformedExercises)
            .HasForeignKey(p => p.TrainingSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<PerformedExercise>()
            .HasOne(p => p.ExerciseType)
            .WithMany()
            .HasForeignKey(p => p.ExerciseTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
