
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Controllers;

public class ExerciseTypesController : Controller
{
    private readonly ApplicationDbContext _context;

    public ExerciseTypesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var list = await _context.ExerciseTypes.AsNoTracking().OrderBy(e => e.Name).ToListAsync();
        return View(list);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.ExerciseTypes.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([Bind("Name,Description")] ExerciseType exerciseType)
    {
        if (!ModelState.IsValid) return View(exerciseType);
        _context.Add(exerciseType);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.ExerciseTypes.FindAsync(id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ExerciseType exerciseType)
    {
        if (id != exerciseType.Id) return NotFound();
        if (!ModelState.IsValid) return View(exerciseType);
        try
        {
            _context.Update(exerciseType);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.ExerciseTypes.AnyAsync(e => e.Id == id)) return NotFound();
            throw;
        }
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var item = await _context.ExerciseTypes.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        if (item == null) return NotFound();
        return View(item);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var item = await _context.ExerciseTypes.FindAsync(id);
        if (item != null)
        {
            _context.ExerciseTypes.Remove(item);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
