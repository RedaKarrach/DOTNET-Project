using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GestionSalle.Services;
using GestionSalle.DTOs;
using Microsoft.AspNetCore.Authorization;
using GestionSalle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionSalle.Controllers
{
    [Authorize(Roles = "Admin")]
  public class MembresController : Controller
    {
        private readonly IMembreService _service;
        private readonly SalleDbContext _context;
        public MembresController(IMembreService service, SalleDbContext context) { _service = service; _context = context; }

        public async Task<IActionResult> Index(string? q, string? statut)
      {
var list = await _service.ListMembresAsync();
    return View(list);
  }

      public async Task<IActionResult> Details(int? id)
        {
 if (id == null) return NotFound();
 var membre = await _context.Membres
       .Include(m => m.IdEntraineurNavigation)
   .Include(m => m.IdPlanNavigation)
    .Include(m => m.IdUtilisateurNavigation)
     .FirstOrDefaultAsync(m => m.IdMembre == id);
      if (membre == null) return NotFound();
         return View(membre);
     }

    public async Task<IActionResult> Create()
        {
  // Load dropdown data
     var entraineurs = await _context.Entraineurs.ToListAsync();
            var plans = await _context.PlanAbonnements.ToListAsync();
     
        ViewData["Entraineurs"] = new SelectList(entraineurs, "IdEntraineur", "NomComplet");
            ViewData["Plans"] = new SelectList(plans, "IdPlan", "Nom");
          
      return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MembreCreateDto dto)
        {
   if (!ModelState.IsValid)
  {
      var entraineurs = await _context.Entraineurs.ToListAsync();
    var plans = await _context.PlanAbonnements.ToListAsync();
        
        ViewData["Entraineurs"] = new SelectList(entraineurs, "IdEntraineur", "NomComplet");
    ViewData["Plans"] = new SelectList(plans, "IdPlan", "Nom");
      return View(dto);
      }
      
   try
            {
      await _service.CreateMembreAsync(dto);
       TempData["Success"] = "Membre créé avec succès";
        return RedirectToAction(nameof(Index));
      }
        catch (Exception ex)
 {
 ModelState.AddModelError(string.Empty, ex.Message);
   var entraineurs = await _context.Entraineurs.ToListAsync();
      var plans = await _context.PlanAbonnements.ToListAsync();
        
 ViewData["Entraineurs"] = new SelectList(entraineurs, "IdEntraineur", "NomComplet");
      ViewData["Plans"] = new SelectList(plans, "IdPlan", "Nom");
      return View(dto);
         }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
  var membre = await _context.Membres.FindAsync(id);
            if (membre == null) return NotFound();
         
      // Convert to UpdateDto
         var updateDto = new MembreUpdateDto
        {
 NomComplet = membre.NomComplet,
        Email = membre.Email,
     Telephone = membre.Telephone,
      Adresse = membre.Adresse,
        Sexe = membre.Sexe,
        IdPlan = membre.IdPlan,
    IdEntraineur = membre.IdEntraineur
    };
    
      var entraineurs = await _context.Entraineurs.ToListAsync();
      var plans = await _context.PlanAbonnements.ToListAsync();
      
      ViewData["Entraineurs"] = new SelectList(entraineurs, "IdEntraineur", "NomComplet", updateDto.IdEntraineur);
     ViewData["Plans"] = new SelectList(plans, "IdPlan", "Nom", updateDto.IdPlan);
  ViewData["IdMembre"] = id;
   
      return View(updateDto);
        }

        [HttpPost]
     [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, MembreUpdateDto dto)
        {
    if (!ModelState.IsValid)
   {
        var entraineurs = await _context.Entraineurs.ToListAsync();
      var plans = await _context.PlanAbonnements.ToListAsync();
        
        ViewData["Entraineurs"] = new SelectList(entraineurs, "IdEntraineur", "NomComplet");
  ViewData["Plans"] = new SelectList(plans, "IdPlan", "Nom");
      ViewData["IdMembre"] = id;
            return View(dto);
  }
      
   try
  {
         await _service.UpdateMembreAsync(id, dto);
    TempData["Success"] = "Membre modifié avec succès";
    return RedirectToAction(nameof(Index));
    }
        catch (Exception ex)
     {
      ModelState.AddModelError(string.Empty, ex.Message);
       var entraineurs = await _context.Entraineurs.ToListAsync();
        var plans = await _context.PlanAbonnements.ToListAsync();
      
    ViewData["Entraineurs"] = new SelectList(entraineurs, "IdEntraineur", "NomComplet");
        ViewData["Plans"] = new SelectList(plans, "IdPlan", "Nom");
        ViewData["IdMembre"] = id;
           return View(dto);
         }
    }

        public async Task<IActionResult> Delete(int? id)
   {
   if (id == null) return NotFound();
    var membre = await _context.Membres
       .Include(m => m.Paiements)
  .Include(m => m.Seances)
    .FirstOrDefaultAsync(m => m.IdMembre == id);
    if (membre == null) return NotFound();
          
  // Pass additional data to view to show warnings about related records
      ViewBag.HasPaiements = membre.Paiements?.Count > 0;
     ViewBag.HasSeances = membre.Seances?.Count > 0;
 ViewBag.PaiementsCount = membre.Paiements?.Count ?? 0;
 ViewBag.SeancesCount = membre.Seances?.Count ?? 0;
 ViewBag.WillCascadeDelete = true; // Indicate this will cascade delete
     
  return View(membre);
     }

     [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
      {
            try
          {
    // Get the member with related data
       var membre = await _context.Membres
    .Include(m => m.Paiements)
      .Include(m => m.Seances)
         .FirstOrDefaultAsync(m => m.IdMembre == id);

 if (membre == null)
       {
      TempData["Error"] = "Membre not found";
        return RedirectToAction(nameof(Index));
 }

  // Count what will be deleted for logging
    int totalPaiements = membre.Paiements?.Count ?? 0;
    int totalSeances = membre.Seances?.Count ?? 0;

     // With cascade delete enabled in DbContext, this will automatically delete:
    // - All Paiements of this Membre
        // - All Seances of this Membre
     await _service.DeleteMembreAsync(id);
    
   // Build success message showing what was deleted
    var deletedItems = new List<string>();
            if (totalPaiements > 0) deletedItems.Add($"{totalPaiements} payment(s)");
         if (totalSeances > 0) deletedItems.Add($"{totalSeances} session(s)");

       string cascadeMessage = deletedItems.Count > 0 
          ? $" and {string.Join(", ", deletedItems)}"
   : "";

TempData["Success"] = $"Membre deleted successfully{cascadeMessage}.";
}
 catch (DbUpdateException)
          {
    // Handle database constraint violations
   TempData["Error"] = "Cannot delete member due to database constraints. Please contact administrator.";
     }
     catch (Exception ex)
  {
          TempData["Error"] = "An error occurred while deleting the member: " + ex.Message;
        }
   return RedirectToAction(nameof(Index));
      }
    }
}
