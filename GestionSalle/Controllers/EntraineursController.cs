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
    [Authorize]
    public class EntraineursController : Controller
    {
        private readonly SalleDbContext _context;

        public EntraineursController(SalleDbContext context)
        {
            _context = context;
        }

        // GET: Entraineurs
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var salleDbContext = _context.Entraineurs.Include(e => e.IdUtilisateurNavigation);
            return View(await salleDbContext.ToListAsync());
        }

        // GET: Entraineurs/Details/5
        [AllowAnonymous]
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["IdUtilisateur"] = new SelectList(_context.Utilisateurs, "IdUtilisateur", "IdUtilisateur");
            return View();
        }

        // POST: Entraineurs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("IdEntraineur,NomComplet,Email,Specialite,Telephone,Statut,IdUtilisateur")] Entraineur entraineur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(entraineur);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUtilisateur"] = new SelectList(_context.Utilisateurs, "IdUtilisateur", "IdUtilisateur", entraineur.IdUtilisateur);
            return View(entraineur);
        }

        // GET: Entraineurs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entraineur = await _context.Entraineurs.FindAsync(id);
            if (entraineur == null)
            {
                return NotFound();
            }
            ViewData["IdUtilisateur"] = new SelectList(_context.Utilisateurs, "IdUtilisateur", "IdUtilisateur", entraineur.IdUtilisateur);
            return View(entraineur);
        }

        // POST: Entraineurs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("IdEntraineur,NomComplet,Email,Specialite,Telephone,Statut,IdUtilisateur")] Entraineur entraineur)
        {
            if (id != entraineur.IdEntraineur)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(entraineur);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EntraineurExists(entraineur.IdEntraineur))
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
            ViewData["IdUtilisateur"] = new SelectList(_context.Utilisateurs, "IdUtilisateur", "IdUtilisateur", entraineur.IdUtilisateur);
            return View(entraineur);
        }

        // GET: Entraineurs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Entraineurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var entraineur = await _context.Entraineurs.FindAsync(id);
            if (entraineur != null)
            {
                _context.Entraineurs.Remove(entraineur);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EntraineurExists(int id)
        {
            return _context.Entraineurs.Any(e => e.IdEntraineur == id);
        }
    }
}
