
using BeFit.Data;
using BeFit.Models.Stats;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers;

[Authorize]
public class StatsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public StatsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var uid = _userManager.GetUserId(User)!;
        var since = DateTime.Today.AddDays(-28);

        var query = from pe in _context.PerformedExercises
                    join s in _context.TrainingSessions on pe.TrainingSessionId equals s.Id
                    join et in _context.ExerciseTypes on pe.ExerciseTypeId equals et.Id
                    where s.UserId == uid && s.StartTime >= since
                    group new { pe, et } by new { et.Name } into g
                    select new ExerciseStatsRow
                    {
                        ExerciseName = g.Key.Name,
                        TimesPerformed = g.Count(),
                        TotalReps = g.Sum(x => x.pe.Sets * x.pe.RepsPerSet),
                        AvgLoad = g.Average(x => x.pe.LoadKg),
                        MaxLoad = g.Max(x => x.pe.LoadKg)
                    };

        var data = await query.OrderBy(r => r.ExerciseName).AsNoTracking().ToListAsync();
        return View(data);
    }
}
