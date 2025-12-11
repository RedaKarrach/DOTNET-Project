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
 public MembreService(IMembreRepository repo) { _repo = repo; }
 public async Task CreateMembreAsync(MembreCreateDto dto)
 {
 var exists = (await _repo.SearchAsync(dto.Email, null)).Any(m => m.Email == dto.Email);
 if (exists) throw new InvalidOperationException("Email already exists");
 var entity = new Membre
 {
 NomComplet = dto.NomComplet,
 Email = dto.Email,
 Telephone = dto.Telephone,
 Adresse = dto.Adresse,
 IdPlan = dto.IdPlan,
 IdEntraineur = dto.IdEntraineur,
 IdUtilisateur = dto.IdUtilisateur
 };
 await _repo.AddAsync(entity);
 await _repo.SaveChangesAsync();
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
 e.IdPlan = dto.IdPlan;
 e.IdEntraineur = dto.IdEntraineur;
 e.IdUtilisateur = dto.IdUtilisateur;
 _repo.Update(e);
 await _repo.SaveChangesAsync();
 }
 }
}
