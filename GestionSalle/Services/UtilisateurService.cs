using System.Threading.Tasks;
using GestionSalle.DTOs;
using GestionSalle.Models;
using GestionSalle.Repositories;
using System.Linq;

namespace GestionSalle.Services
{
 public interface IUtilisateurService
 {
 Task<UtilisateurDto?> AuthenticateAsync(string username, string password);
 Task CreateUtilisateurAsync(UtilisateurCreateDto dto);
 }
 public class UtilisateurService : IUtilisateurService
 {
 private readonly IRepository<Utilisateur> _repo;
 private readonly IPasswordService _passwordService;
 public UtilisateurService(IRepository<Utilisateur> repo, IPasswordService passwordService)
 {
 _repo = repo; _passwordService = passwordService;
 }
 public async Task CreateUtilisateurAsync(UtilisateurCreateDto dto)
 {
 var u = new Utilisateur
 {
 NomUtilisateur = dto.NomUtilisateur,
 MotDePasse = _passwordService.HashPassword(dto.MotDePasse),
 Role = dto.Role
 };
 await _repo.AddAsync(u);
 await _repo.SaveChangesAsync();
 }
 public async Task<UtilisateurDto?> AuthenticateAsync(string username, string password)
 {
 var list = await _repo.GetAllAsync();
 var user = list.FirstOrDefault(u => string.Equals(u.NomUtilisateur?.Trim(), username?.Trim(), System.StringComparison.OrdinalIgnoreCase));
 if (user == null) return null;
 // Try hashed verification
 if (_passwordService.VerifyPassword(user.MotDePasse, password))
 {
 return new UtilisateurDto(user.IdUtilisateur, user.NomUtilisateur, user.Role);
 }
 // Fallback: legacy plain-text stored password - accept and upgrade to hashed
 if (user.MotDePasse == password)
 {
 user.MotDePasse = _passwordService.HashPassword(password);
 _repo.Update(user);
 await _repo.SaveChangesAsync();
 return new UtilisateurDto(user.IdUtilisateur, user.NomUtilisateur, user.Role);
 }
 return null;
 }
 }
}
