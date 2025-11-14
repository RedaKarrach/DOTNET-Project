using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionSalle.Models;

namespace GestionSalle.Controllers
{
    public class MembresController : Controller
    {
        private readonly SalleDbContext _context;

        public MembresController(SalleDbContext context)
        {
            _context = context;
        }

        // GET: Membres
        public async Task<IActionResult> Index()
        {
            var salleDbContext = _context.Membres.Include(m => m.IdEntraineurNavigation).Include(m => m.IdPlanNavigation).Include(m => m.IdUtilisateurNavigation);
            return View(await salleDbContext.ToListAsync());
        }

        // GET: Membres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membre = await _context.Membres
                .Include(m => m.IdEntraineurNavigation)
                .Include(m => m.IdPlanNavigation)
                .Include(m => m.IdUtilisateurNavigation)
                .FirstOrDefaultAsync(m => m.IdMembre == id);
            if (membre == null)
            {
                return NotFound();
            }

            return View(membre);
        }

        // GET: Membres/Create
        public IActionResult Create()
        {
            ViewData["IdEntraineur"] = new SelectList(_context.Entraineurs, "IdEntraineur", "IdEntraineur");
            ViewData["IdPlan"] = new SelectList(_context.PlanAbonnements, "IdPlan", "IdPlan");
            ViewData["IdUtilisateur"] = new SelectList(_context.Utilisateurs, "IdUtilisateur", "IdUtilisateur");
            return View();
        }

        // POST: Membres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMembre,NomComplet,Email,Telephone,DateInscription,Sexe,Adresse,IdPlan,IdEntraineur,Statut,IdUtilisateur")] Membre membre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(membre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEntraineur"] = new SelectList(_context.Entraineurs, "IdEntraineur", "IdEntraineur", membre.IdEntraineur);
            ViewData["IdPlan"] = new SelectList(_context.PlanAbonnements, "IdPlan", "IdPlan", membre.IdPlan);
            ViewData["IdUtilisateur"] = new SelectList(_context.Utilisateurs, "IdUtilisateur", "IdUtilisateur", membre.IdUtilisateur);
            return View(membre);
        }

        // GET: Membres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membre = await _context.Membres.FindAsync(id);
            if (membre == null)
            {
                return NotFound();
            }
            ViewData["IdEntraineur"] = new SelectList(_context.Entraineurs, "IdEntraineur", "IdEntraineur", membre.IdEntraineur);
            ViewData["IdPlan"] = new SelectList(_context.PlanAbonnements, "IdPlan", "IdPlan", membre.IdPlan);
            ViewData["IdUtilisateur"] = new SelectList(_context.Utilisateurs, "IdUtilisateur", "IdUtilisateur", membre.IdUtilisateur);
            return View(membre);
        }

        // POST: Membres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMembre,NomComplet,Email,Telephone,DateInscription,Sexe,Adresse,IdPlan,IdEntraineur,Statut,IdUtilisateur")] Membre membre)
        {
            if (id != membre.IdMembre)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(membre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MembreExists(membre.IdMembre))
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
            ViewData["IdEntraineur"] = new SelectList(_context.Entraineurs, "IdEntraineur", "IdEntraineur", membre.IdEntraineur);
            ViewData["IdPlan"] = new SelectList(_context.PlanAbonnements, "IdPlan", "IdPlan", membre.IdPlan);
            ViewData["IdUtilisateur"] = new SelectList(_context.Utilisateurs, "IdUtilisateur", "IdUtilisateur", membre.IdUtilisateur);
            return View(membre);
        }

        // GET: Membres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var membre = await _context.Membres
                .Include(m => m.IdEntraineurNavigation)
                .Include(m => m.IdPlanNavigation)
                .Include(m => m.IdUtilisateurNavigation)
                .FirstOrDefaultAsync(m => m.IdMembre == id);
            if (membre == null)
            {
                return NotFound();
            }

            return View(membre);
        }

        // POST: Membres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var membre = await _context.Membres.FindAsync(id);
            if (membre != null)
            {
                _context.Membres.Remove(membre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembreExists(int id)
        {
            return _context.Membres.Any(e => e.IdMembre == id);
        }
    }
}
