using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GestionSalle.DTOs;
using GestionSalle.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace GestionSalle.Controllers
{
 public class AccountController : Controller
 {
 private readonly IUtilisateurService _userService;
 public AccountController(IUtilisateurService userService) { _userService = userService; }
 public IActionResult Login() => View();
 [HttpPost]
 public async Task<IActionResult> Login(string username, string password, bool remember = false)
 {
 var user = await _userService.AuthenticateAsync(username, password);
 if (user == null)
 {
 ModelState.AddModelError(string.Empty, "Invalid credentials");
 return View();
 }
 var claims = new List<Claim>
 {
 new Claim(ClaimTypes.NameIdentifier, user.IdUtilisateur.ToString()),
 new Claim(ClaimTypes.Name, user.NomUtilisateur),
 new Claim(ClaimTypes.Role, user.Role)
 };
 var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
 var principal = new ClaimsPrincipal(identity);
 await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = remember });
 return RedirectToAction("Index","Home");
 }
 public async Task<IActionResult> Logout()
 {
 await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
 return RedirectToAction("Login");
 }
 }
}
