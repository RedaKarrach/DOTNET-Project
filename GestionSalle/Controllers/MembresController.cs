using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GestionSalle.Services;
using GestionSalle.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace GestionSalle.Controllers
{
    [Authorize]
    public class MembresController : Controller
    {
        private readonly IMembreService _service;
        public MembresController(IMembreService service) { _service = service; }

        public async Task<IActionResult> Index(string? q, string? statut)
        {
            var list = await _service.ListMembresAsync();
            return View(list);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var dto = await _service.GetMembreByIdAsync(id.Value);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(MembreCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                await _service.CreateMembreAsync(dto);
                TempData["Success"] = "Membre created";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var dto = await _service.GetMembreByIdAsync(id.Value);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, MembreUpdateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            try
            {
                await _service.UpdateMembreAsync(id, dto);
                TempData["Success"] = "Membre updated";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(dto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteMembreAsync(id);
                TempData["Success"] = "Membre deleted";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
