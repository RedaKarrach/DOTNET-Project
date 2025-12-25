using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionSalle.DTOs;
using GestionSalle.Models;
using GestionSalle.Repositories;

namespace GestionSalle.Services
{
 public interface IMembreService
 {
 Task<IEnumerable<MembreListDto>> ListMembresAsync();
 Task<MembreDetailsDto?> GetMembreByIdAsync(int id);
 Task CreateMembreAsync(MembreCreateDto dto);
 Task UpdateMembreAsync(int id, MembreUpdateDto dto);
 Task DeleteMembreAsync(int id);
 }
 public class MembreService : IMembreService
 {
 private readonly IMembreRepository _repo;
 private readonly IRepository<Utilisateur> _userRepo;
 private readonly IPasswordService _passwordService;
 public MembreService(IMembreRepository repo, IRepository<Utilisateur> userRepo, IPasswordService passwordService) { _repo = repo; _userRepo = userRepo; _passwordService = passwordService; }
 public async Task CreateMembreAsync(MembreCreateDto dto)
 {
 var exists = (await _repo.SearchAsync(dto.Email, null)).Any(m => m.Email == dto.Email);
 if (exists) throw new InvalidOperationException("Email already exists");
 
 // Create a user account automatically for the member
 var username = dto.Email.Split('@')[0]; // Use email prefix as username
 var tempPassword = GenerateTemporaryPassword();
 
 var user = new Utilisateur
 {
 NomUtilisateur = username,
 MotDePasse = _passwordService.HashPassword(tempPassword),
 Role = "User"
 };
 
 await _userRepo.AddAsync(user);
 await _userRepo.SaveChangesAsync();
 
 var entity = new Membre
 {
 NomComplet = dto.NomComplet,
 Email = dto.Email,
 Telephone = dto.Telephone,
 Adresse = dto.Adresse,
 Sexe = dto.Sexe,
 IdPlan = dto.IdPlan,
 IdEntraineur = dto.IdEntraineur,
 IdUtilisateur = user.IdUtilisateur // Link to created user
 };
 
 await _repo.AddAsync(entity);
 await _repo.SaveChangesAsync();
 }
 private string GenerateTemporaryPassword()
 {
 // Generate a simple temporary password
 return $"Temp{DateTime.Now:yyyyMMddHHmmss}!";
 }
 public async Task DeleteMembreAsync(int id)
 {
 var e = await _repo.GetByIdAsync(id);
 if (e == null) throw new KeyNotFoundException();
 _repo.Remove(e);
 await _repo.SaveChangesAsync();
 }
 public async Task<MembreDetailsDto?> GetMembreByIdAsync(int id)
 {
 var e = await _repo.GetByIdAsync(id);
 if (e == null) return null;
 return new MembreDetailsDto(e.IdMembre,e.NomComplet,e.Email,e.Telephone,e.Adresse,e.Statut);
 }
 public async Task<IEnumerable<MembreListDto>> ListMembresAsync()
 {
 var all = await _repo.GetAllAsync();
 return all.Select(m => new MembreListDto(m.IdMembre,m.NomComplet,m.Email,m.Telephone,m.Statut)).ToList();
 }
 public async Task UpdateMembreAsync(int id, MembreUpdateDto dto)
 {
 var e = await _repo.GetByIdAsync(id);
 if (e == null) throw new KeyNotFoundException();
 e.NomComplet = dto.NomComplet;
 e.Email = dto.Email;
 e.Telephone = dto.Telephone;
 e.Adresse = dto.Adresse;
 e.Sexe = dto.Sexe;
 e.IdPlan = dto.IdPlan;
 e.IdEntraineur = dto.IdEntraineur;
 _repo.Update(e);
 await _repo.SaveChangesAsync();
 }
 }
}
