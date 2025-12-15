
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers;

[Authorize]
public class TrainingSessionsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public TrainingSessionsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    private string? CurrentUserId() => _userManager.GetUserId(User);

    public async Task<IActionResult> Index()
    {
        var uid = CurrentUserId();
        var list = await _context.TrainingSessions
            .Where(s => s.UserId == uid)
            .OrderByDescending(s => s.StartTime)
            .AsNoTracking()
            .ToListAsync();
        return View(list);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var uid = CurrentUserId();
        var item = await _context.TrainingSessions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id && s.UserId == uid);
        if (item == null) return NotFound();
        return View(item);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StartTime,EndTime")] TrainingSession session)
    {
        if (!ModelState.IsValid) return View(session);
        session.UserId = CurrentUserId()!;
        _context.Add(session);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var uid = CurrentUserId();
        var item = await _context.TrainingSessions.FirstOrDefaultAsync(s => s.Id == id && s.UserId == uid);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime")] TrainingSession session)
    {
        if (id != session.Id) return NotFound();
        var uid = CurrentUserId();
        var existing = await _context.TrainingSessions.FirstOrDefaultAsync(s => s.Id == id && s.UserId == uid);
        if (existing == null) return NotFound();
        if (!ModelState.IsValid) return View(session);
        existing.StartTime = session.StartTime;
        existing.EndTime = session.EndTime;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var uid = CurrentUserId();
        var item = await _context.TrainingSessions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id && s.UserId == uid);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var uid = CurrentUserId();
        var item = await _context.TrainingSessions.FirstOrDefaultAsync(s => s.Id == id && s.UserId == uid);
        if (item != null)
        {
            _context.TrainingSessions.Remove(item);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
