using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GestionSalle.Services;
using GestionSalle.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionSalle.Controllers
{
 [Authorize(Roles = "Admin")]
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

 public async Task<IActionResult> Edit(int? id)
 {
 if (id == null) return NotFound();
 var paiement = await _paiementService.GetPaiementByIdAsync(id.Value);
 if (paiement == null) return NotFound();
 
 // Convert to UpdateDto for the form
 var updateDto = new PaiementUpdateDto
 {
 IdMembre = paiement.IdMembre,
 Montant = paiement.Montant,
 Methode = paiement.Methode
 };
 
 var membres = await _membreService.ListMembresAsync();
 ViewData["Membres"] = new SelectList(membres, "IdMembre", "NomComplet", paiement.IdMembre);
 ViewData["Methodes"] = new SelectList(new[] { "Carte", "Espèces", "Virement", "Mobile", "Chèque" }, paiement.Methode);
 ViewData["IdPaiement"] = paiement.IdPaiement; // Pass the ID separately
 return View(updateDto);
 }

 [HttpPost]
 [ValidateAntiForgeryToken]
 public async Task<IActionResult> Edit(int id, PaiementUpdateDto dto)
 {
 if (!ModelState.IsValid)
 {
 var membres = await _membreService.ListMembresAsync();
 ViewData["Membres"] = new SelectList(membres, "IdMembre", "NomComplet");
 ViewData["Methodes"] = new SelectList(new[] { "Carte", "Espèces", "Virement", "Mobile", "Chèque" });
 ViewData["IdPaiement"] = id;
 return View(dto);
 }
 try
 {
 await _paiementService.UpdatePaiementAsync(id, dto);
 TempData["Success"] = "Paiement modifié";
 return RedirectToAction(nameof(Index));
 }
 catch (System.Exception ex)
 {
 ModelState.AddModelError(string.Empty, ex.Message);
 var membres = await _membreService.ListMembresAsync();
 ViewData["Membres"] = new SelectList(membres, "IdMembre", "NomComplet");
 ViewData["Methodes"] = new SelectList(new[] { "Carte", "Espèces", "Virement", "Mobile", "Chèque" });
 ViewData["IdPaiement"] = id;
 return View(dto);
 }
 }

 public async Task<IActionResult> Delete(int? id)
 {
 if (id == null) return NotFound();
 var paiement = await _paiementService.GetPaiementByIdAsync(id.Value);
 if (paiement == null) return NotFound();
 return View(paiement);
 }

 [HttpPost]
 [ValidateAntiForgeryToken]
 public async Task<IActionResult> Delete(int id)
 {
 try
 {
 await _paiementService.DeletePaiementAsync(id);
 TempData["Success"] = "Paiement supprimé";
 }
 catch (System.Exception ex)
 {
 TempData["Error"] = ex.Message;
 }
 return RedirectToAction(nameof(Index));
 }
 }
}
