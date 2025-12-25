using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSalle.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestionSalle.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UtilisateursController : Controller
    {
        private readonly SalleDbContext _context;

        public UtilisateursController(SalleDbContext context)
        {
            _context = context;
        }

        // GET: Utilisateurs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Utilisateurs.ToListAsync());
        }

        // GET: Utilisateurs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateurs
                .FirstOrDefaultAsync(m => m.IdUtilisateur == id);
            if (utilisateur == null)
            {
                return NotFound();
            }

            return View(utilisateur);
        }

        // GET: Utilisateurs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Utilisateurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUtilisateur,NomUtilisateur,MotDePasse,Role,DateCreation")] Utilisateur utilisateur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(utilisateur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateurs.FindAsync(id);
            if (utilisateur == null)
            {
                return NotFound();
            }
            return View(utilisateur);
        }

        // POST: Utilisateurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUtilisateur,NomUtilisateur,MotDePasse,Role,DateCreation")] Utilisateur utilisateur)
        {
            if (id != utilisateur.IdUtilisateur)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(utilisateur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UtilisateurExists(utilisateur.IdUtilisateur))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(utilisateur);
        }

        // GET: Utilisateurs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var utilisateur = await _context.Utilisateurs
     .Include(u => u.Entraineurs)
           .Include(u => u.Membres)
  .ThenInclude(m => m.Paiements)  // Include payments for cascade info
 .Include(u => u.Membres)
       .ThenInclude(m => m.Seances)   // Include sessions for cascade info
          .FirstOrDefaultAsync(m => m.IdUtilisateur == id);
            if (utilisateur == null)
      {
           return NotFound();
   }

 // Count total related records for information
     int totalEntraineurs = utilisateur.Entraineurs?.Count ?? 0;
        int totalMembres = utilisateur.Membres?.Count ?? 0;
        int totalPaiements = utilisateur.Membres?.Sum(m => m.Paiements?.Count ?? 0) ?? 0;
     int totalSeances = utilisateur.Membres?.Sum(m => m.Seances?.Count ?? 0) ?? 0;

    // Pass information to view about what will be deleted
ViewBag.HasEntraineurs = totalEntraineurs > 0;
            ViewBag.HasMembres = totalMembres > 0;
            ViewBag.EntraineursCount = totalEntraineurs;
   ViewBag.MembresCount = totalMembres;
ViewBag.PaiementsCount = totalPaiements;
    ViewBag.SeancesCount = totalSeances;
            ViewBag.WillCascadeDelete = true; // Indicate this will cascade delete

     return View(utilisateur);
        }

        // POST: Utilisateurs/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
{
       // Get the utilisateur with all related data for cascade deletion
        var utilisateur = await _context.Utilisateurs
           .Include(u => u.Entraineurs)
      .Include(u => u.Membres)
     .ThenInclude(m => m.Paiements)
            .Include(u => u.Membres)
       .ThenInclude(m => m.Seances)
       .FirstOrDefaultAsync(u => u.IdUtilisateur == id);

   if (utilisateur == null)
    {
          TempData["Error"] = "Utilisateur not found";
          return RedirectToAction(nameof(Index));
      }

     // Count what will be deleted for logging
            int totalEntraineurs = utilisateur.Entraineurs?.Count ?? 0;
          int totalMembres = utilisateur.Membres?.Count ?? 0;
     int totalPaiements = utilisateur.Membres?.Sum(m => m.Paiements?.Count ?? 0) ?? 0;
     int totalSeances = utilisateur.Membres?.Sum(m => m.Seances?.Count ?? 0) ?? 0;

   // With cascade delete enabled in DbContext, this will automatically delete:
        // - All related Entraineurs
         // - All related Membres 
                // - All Paiements of those Membres
                // - All Seances of those Membres
      _context.Utilisateurs.Remove(utilisateur);
        await _context.SaveChangesAsync();

// Build success message showing what was deleted
      var deletedItems = new List<string>();
  if (totalEntraineurs > 0) deletedItems.Add($"{totalEntraineurs} entraineur(s)");
       if (totalMembres > 0) deletedItems.Add($"{totalMembres} member(s)");
     if (totalPaiements > 0) deletedItems.Add($"{totalPaiements} payment(s)");
          if (totalSeances > 0) deletedItems.Add($"{totalSeances} session(s)");

           string cascadeMessage = deletedItems.Count > 0 
          ? $" and {string.Join(", ", deletedItems)}"
   : "";

             TempData["Success"] = $"Utilisateur deleted successfully{cascadeMessage}.";
 }
       catch (DbUpdateException)
        {
       TempData["Error"] = "Cannot delete utilisateur due to database constraints. Please contact administrator.";
     }
    catch (Exception ex)
      {
      TempData["Error"] = "An error occurred while deleting the utilisateur: " + ex.Message;
       }
      return RedirectToAction(nameof(Index));
        }

        private bool UtilisateurExists(int id)
        {
            return _context.Utilisateurs.Any(e => e.IdUtilisateur == id);
        }
    }
}
