using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSalle.Models;
using Microsoft.AspNetCore.Authorization;
using GestionSalle.Services;
using GestionSalle.DTOs;
using GestionSalle.Enums;

namespace GestionSalle.Controllers
{
    [Authorize(Roles = "Admin")]
    public class EntraineursController : Controller
    {
    private readonly SalleDbContext _context;
        private readonly IEntraineurService _service;

        public EntraineursController(SalleDbContext context, IEntraineurService service)
        {
      _context = context;
      _service = service;
        }

        // GET: Entraineurs
        public async Task<IActionResult> Index()
        {
      var list = await _service.ListEntraineursAsync();
      return View(list);
        }

 // GET: Entraineurs/Details/5
 public async Task<IActionResult> Details(int? id)
        {
        if (id == null)
        {
  return NotFound();
       }

            var entraineur = await _context.Entraineurs
                .Include(e => e.IdUtilisateurNavigation)
 .FirstOrDefaultAsync(m => m.IdEntraineur == id);
      if (entraineur == null)
            {
         return NotFound();
        }

return View(entraineur);
        }

        // GET: Entraineurs/Create
   public IActionResult Create()
        {
  // Populate Specialite enum dropdown
     ViewData["Specialites"] = new SelectList(Enum.GetValues(typeof(SpecialiteEntraineur))
  .Cast<SpecialiteEntraineur>()
    .Select(s => new { Value = s, Text = s.ToString().Replace("_", " ") }), "Value", "Text");
            return View();
        }

        // POST: Entraineurs/Create
  [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EntraineurCreateDto dto)
  {
            if (!ModelState.IsValid)
    {
    ViewData["Specialites"] = new SelectList(Enum.GetValues(typeof(SpecialiteEntraineur))
        .Cast<SpecialiteEntraineur>()
          .Select(s => new { Value = s, Text = s.ToString().Replace("_", " ") }), "Value", "Text");
    return View(dto);
  }
        
      try
     {
  await _service.CreateEntraineurAsync(dto);
  TempData["Success"] = "Entraîneur créé avec succès";
      return RedirectToAction(nameof(Index));
            }
  catch (Exception ex)
      {
    ModelState.AddModelError(string.Empty, ex.Message);
      ViewData["Specialites"] = new SelectList(Enum.GetValues(typeof(SpecialiteEntraineur))
        .Cast<SpecialiteEntraineur>()
       .Select(s => new { Value = s, Text = s.ToString().Replace("_", " ") }), "Value", "Text");
          return View(dto);
       }
        }

 // GET: Entraineurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
    {
       if (id == null) return NotFound();
    
   var entraineur = await _context.Entraineurs.FindAsync(id);
            if (entraineur == null) return NotFound();
      
      // Convert to UpdateDto
      var updateDto = new EntraineurUpdateDto
      {
      NomComplet = entraineur.NomComplet,
      Email = entraineur.Email,
       Telephone = entraineur.Telephone,
    Specialite = Enum.TryParse<SpecialiteEntraineur>(entraineur.Specialite?.Replace(" ", "_"), out var spec) 
        ? spec 
        : SpecialiteEntraineur.Coaching_Personnel
  };
        
   ViewData["Specialites"] = new SelectList(Enum.GetValues(typeof(SpecialiteEntraineur))
         .Cast<SpecialiteEntraineur>()
      .Select(s => new { Value = s, Text = s.ToString().Replace("_", " ") }), "Value", "Text", updateDto.Specialite);
    ViewData["IdEntraineur"] = id;
  return View(updateDto);
    }

        // POST: Entraineurs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EntraineurUpdateDto dto)
    {
      if (!ModelState.IsValid)
    {
         ViewData["Specialites"] = new SelectList(Enum.GetValues(typeof(SpecialiteEntraineur))
  .Cast<SpecialiteEntraineur>()
    .Select(s => new { Value = s, Text = s.ToString().Replace("_", " ") }), "Value", "Text");
       ViewData["IdEntraineur"] = id;
      return View(dto);
     }
          
         try
      {
    await _service.UpdateEntraineurAsync(id, dto);
      TempData["Success"] = "Entraîneur modifié avec succès";
    return RedirectToAction(nameof(Index));
      }
         catch (Exception ex)
     {
     ModelState.AddModelError(string.Empty, ex.Message);
    ViewData["Specialites"] = new SelectList(Enum.GetValues(typeof(SpecialiteEntraineur))
        .Cast<SpecialiteEntraineur>()
       .Select(s => new { Value = s, Text = s.ToString().Replace("_", " ") }), "Value", "Text");
       ViewData["IdEntraineur"] = id;
        return View(dto);
     }
        }

  // GET: Entraineurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
      {
            if (id == null)
            {
                return NotFound();
            }

            var entraineur = await _context.Entraineurs
                .Include(e => e.IdUtilisateurNavigation)
                .Include(e => e.Membres)
                .Include(e => e.Seances)
                .FirstOrDefaultAsync(m => m.IdEntraineur == id);
            if (entraineur == null)
            {
                return NotFound();
            }

            // Pass additional data to view to show warnings about related records
            ViewBag.HasMembres = entraineur.Membres?.Count > 0;
            ViewBag.HasSeances = entraineur.Seances?.Count > 0;
            ViewBag.MembresCount = entraineur.Membres?.Count ?? 0;
            ViewBag.SeancesCount = entraineur.Seances?.Count ?? 0;

            return View(entraineur);
        }

        // POST: Entraineurs/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Get the entraineur with related data
                var entraineur = await _context.Entraineurs
                    .Include(e => e.Membres)
                    .Include(e => e.Seances)
                    .FirstOrDefaultAsync(e => e.IdEntraineur == id);

                if (entraineur == null)
                {
                    TempData["Error"] = "Entraineur not found";
                    return RedirectToAction(nameof(Index));
                }

                // Check for related records
                if (entraineur.Membres?.Count > 0 || entraineur.Seances?.Count > 0)
                {
                    TempData["Error"] = $"Cannot delete entraineur. Entraineur has {entraineur.Membres?.Count ?? 0} member(s) and {entraineur.Seances?.Count ?? 0} session(s). Please delete or reassign these records first.";
                    return RedirectToAction(nameof(Index));
                }

                // If no related records, proceed with deletion
                _context.Entraineurs.Remove(entraineur);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Entraineur deleted successfully";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "Cannot delete entraineur due to existing related records. Please contact administrator.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while deleting the entraineur: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
