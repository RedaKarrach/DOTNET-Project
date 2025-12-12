using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GestionSalle.DTOs;
using GestionSalle.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;

namespace GestionSalle.Controllers
{
 public class AccountController : Controller
 {
 private readonly IUtilisateurService _userService;
 private readonly ILogger<AccountController> _logger;
 private readonly IConfiguration _config;
 private readonly IWebHostEnvironment _env;
 public AccountController(IUtilisateurService userService, ILogger<AccountController> logger, IConfiguration config, IWebHostEnvironment env) { _userService = userService; _logger = logger; _config = config; _env = env; }
 public IActionResult Login() => View();

 [HttpPost]
 [ValidateAntiForgeryToken]
 public async Task<IActionResult> Login(string username, string password, bool remember = false)
 {
 _logger.LogInformation("Login attempt for user: {username}", username);
 var user = await _userService.AuthenticateAsync(username, password);
 if (user == null)
 {
 _logger.LogWarning("Login failed for user: {username}", username);
 ModelState.AddModelError(string.Empty, "Invalid credentials");
 return View();
 }
 _logger.LogInformation("User {username} authenticated successfully", user.NomUtilisateur);
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

 [HttpGet]
 public IActionResult AccessDenied()
 {
 return View("~/Views/Shared/AccessDenied.cshtml");
 }

 [HttpPost]
 [ValidateAntiForgeryToken]
 public async Task<IActionResult> ResetAdmin(string key)
 {
 // Only allow in Development
 if (!_env.IsDevelopment()) return Forbid();
 var configured = _config["DevAdminResetKey"];
 if (configured != key) return Forbid();
 // reset admin password to Admin@123 (create if missing)
 await _userService.CreateUtilisateurAsync(new UtilisateurCreateDto { NomUtilisateur = "admin", MotDePasse = "Admin@123", Role = "Admin" });
 _logger.LogInformation("Admin reset or created via development endpoint");
 return Ok("Admin reset");
 }
 }
}
