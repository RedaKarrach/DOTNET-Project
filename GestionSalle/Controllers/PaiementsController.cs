using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GestionSalle.Services;
using GestionSalle.DTOs;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionSalle.Controllers
{
 public class PaiementsController : Controller
 {
 private readonly IPaiementService _paiementService;
 private readonly IMembreService _membreService;
 public PaiementsController(IPaiementService paiementService, IMembreService membreService) { _paiementService = paiementService; _membreService = membreService; }

 public async Task<IActionResult> Index()
 {
 var list = await _paiementService.ListPaiementsAsync();
 return View(list);
 }

 public async Task<IActionResult> Create()
 {
 var membres = await _membreService.ListMembresAsync();
 ViewData["Membres"] = new SelectList(membres, "IdMembre", "NomComplet");
 ViewData["Methodes"] = new SelectList(new[] { "Carte", "Espèces", "Virement", "Mobile", "Chèque" });
 return View();
 }

 [HttpPost]
 [ValidateAntiForgeryToken]
 public async Task<IActionResult> Create(PaiementCreateDto dto)
 {
 if (!ModelState.IsValid)
 {
 var membres = await _membreService.ListMembresAsync();
 ViewData["Membres"] = new SelectList(membres, "IdMembre", "NomComplet");
 ViewData["Methodes"] = new SelectList(new[] { "Carte", "Espèces", "Virement", "Mobile", "Chèque" });
 return View(dto);
 }
 try
 {
 await _paiementService.CreatePaiementAsync(dto);
 TempData["Success"] = "Paiement enregistré";
 return RedirectToAction(nameof(Index));
 }
 catch (System.Exception ex)
 {
 ModelState.AddModelError(string.Empty, ex.Message);
 var membres = await _membreService.ListMembresAsync();
 ViewData["Membres"] = new SelectList(membres, "IdMembre", "NomComplet");
 ViewData["Methodes"] = new SelectList(new[] { "Carte", "Espèces", "Virement", "Mobile", "Chèque" });
 return View(dto);
 }
 }
 }
}
