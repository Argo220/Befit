
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers;

[Authorize]
public class PerformedExercisesController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public PerformedExercisesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private string? CurrentUserId() => _userManager.GetUserId(User);

    private async Task<SelectList> UserSessionsSelectList()
    {
        var uid = CurrentUserId();
        var sessions = await _context.TrainingSessions
            .Where(s => s.UserId == uid)
            .OrderByDescending(s => s.StartTime)
            .Select(s => new { s.Id, Label = s.StartTime })
            .ToListAsync();
        return new SelectList(sessions, "Id", "Label");
    }

    private SelectList ExerciseTypesSelectList()
    {
        var types = _context.ExerciseTypes.OrderBy(e => e.Name).Select(e => new { e.Id, e.Name }).ToList();
        return new SelectList(types, "Id", "Name");
    }

    public async Task<IActionResult> Index()
    {
        var uid = CurrentUserId();
        var list = await _context.PerformedExercises
            .Include(p => p.ExerciseType)
            .Include(p => p.TrainingSession)
            .Where(p => p.TrainingSession!.UserId == uid)
            .OrderByDescending(p => p.TrainingSession!.StartTime)
            .AsNoTracking()
            .ToListAsync();
        return View(list);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var uid = CurrentUserId();
        var item = await _context.PerformedExercises
            .Include(p => p.ExerciseType)
            .Include(p => p.TrainingSession)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.TrainingSession!.UserId == uid);
        if (item == null) return NotFound();
        return View(item);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["TrainingSessionId"] = await UserSessionsSelectList();
        ViewData["ExerciseTypeId"] = ExerciseTypesSelectList();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TrainingSessionId,ExerciseTypeId,LoadKg,Sets,RepsPerSet")] PerformedExercise pe)
    {
        var uid = CurrentUserId();
        var ownsSession = await _context.TrainingSessions.AnyAsync(s => s.Id == pe.TrainingSessionId && s.UserId == uid);
        if (!ownsSession) return Forbid();
        if (!ModelState.IsValid)
        {
            ViewData["TrainingSessionId"] = await UserSessionsSelectList();
            ViewData["ExerciseTypeId"] = ExerciseTypesSelectList();
            return View(pe);
        }
        _context.Add(pe);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var uid = CurrentUserId();
        var item = await _context.PerformedExercises
            .Include(p => p.TrainingSession)
            .FirstOrDefaultAsync(p => p.Id == id && p.TrainingSession!.UserId == uid);
        if (item == null) return NotFound();
        ViewData["TrainingSessionId"] = await UserSessionsSelectList();
        ViewData["ExerciseTypeId"] = ExerciseTypesSelectList();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,TrainingSessionId,ExerciseTypeId,LoadKg,Sets,RepsPerSet")] PerformedExercise pe)
    {
        if (id != pe.Id) return NotFound();
        var uid = CurrentUserId();
        var ownsNewSession = await _context.TrainingSessions.AnyAsync(s => s.Id == pe.TrainingSessionId && s.UserId == uid);
        if (!ownsNewSession) return Forbid();
        var existing = await _context.PerformedExercises
            .Include(p => p.TrainingSession)
            .FirstOrDefaultAsync(p => p.Id == id && p.TrainingSession!.UserId == uid);
        if (existing == null) return NotFound();
        if (!ModelState.IsValid)
        {
            ViewData["TrainingSessionId"] = await UserSessionsSelectList();
            ViewData["ExerciseTypeId"] = ExerciseTypesSelectList();
            return View(pe);
        }
        existing.TrainingSessionId = pe.TrainingSessionId;
        existing.ExerciseTypeId = pe.ExerciseTypeId;
        existing.LoadKg = pe.LoadKg;
        existing.Sets = pe.Sets;
        existing.RepsPerSet = pe.RepsPerSet;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var uid = CurrentUserId();
        var item = await _context.PerformedExercises
            .Include(p => p.ExerciseType)
            .Include(p => p.TrainingSession)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && p.TrainingSession!.UserId == uid);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var uid = CurrentUserId();
        var item = await _context.PerformedExercises
            .Include(p => p.TrainingSession)
            .FirstOrDefaultAsync(p => p.Id == id && p.TrainingSession!.UserId == uid);
        if (item != null)
        {
            _context.PerformedExercises.Remove(item);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
