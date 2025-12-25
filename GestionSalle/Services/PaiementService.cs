using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionSalle.DTOs;
using GestionSalle.Models;
using GestionSalle.Repositories;

namespace GestionSalle.Services
{
 public interface IPaiementService
 {
 Task CreatePaiementAsync(PaiementCreateDto dto);
 Task<IEnumerable<PaiementListDto>> ListPaiementsAsync();
 Task<IEnumerable<PaiementListDto>> ListByMembreAsync(int idMembre);
 Task<PaiementListDto?> GetPaiementByIdAsync(int id);
 Task UpdatePaiementAsync(int id, PaiementUpdateDto dto);
 Task DeletePaiementAsync(int id);
 }
 public class PaiementService : IPaiementService
 {
 private readonly IPaiementRepository _repo;
 private readonly IRepository<Membre> _membreRepo;
 private readonly IRepository<Paiement> _paiementRepo;
 public PaiementService(IPaiementRepository repo, IRepository<Membre> membreRepo, IRepository<Paiement> paiementRepo)
 {
 _repo = repo; _membreRepo = membreRepo; _paiementRepo = paiementRepo;
 }
 public async Task CreatePaiementAsync(PaiementCreateDto dto)
 {
 var membre = await _membreRepo.GetByIdAsync(dto.IdMembre);
 if (membre == null) throw new KeyNotFoundException("Membre not found");
 var p = new Paiement { IdMembre = dto.IdMembre, Montant = dto.Montant, Methode = dto.Methode, DatePaiement = System.DateTime.Now };
 await _repo.AddAsync(p);
 await _repo.SaveChangesAsync();
 }
 public async Task<IEnumerable<PaiementListDto>> ListPaiementsAsync()
 {
 var all = await _repo.GetAllAsync();
 var membres = (await _membreRepo.GetAllAsync()).ToDictionary(m=>m.IdMembre,m=>m.NomComplet);
 return all.Select(p=> new PaiementListDto(p.IdPaiement,p.IdMembre,membres.ContainsKey(p.IdMembre)?membres[p.IdMembre]:"-",p.Montant,p.DatePaiement,p.Methode)).ToList();
 }
 public async Task<IEnumerable<PaiementListDto>> ListByMembreAsync(int idMembre)
 {
 var list = await _repo.GetByMembreAsync(idMembre);
 var membres = (await _membreRepo.GetAllAsync()).ToDictionary(m=>m.IdMembre,m=>m.NomComplet);
 return list.Select(p=> new PaiementListDto(p.IdPaiement,p.IdMembre,membres.ContainsKey(p.IdMembre)?membres[p.IdMembre]:"-",p.Montant,p.DatePaiement,p.Methode)).ToList();
 }
 public async Task<PaiementListDto?> GetPaiementByIdAsync(int id)
 {
 var p = await _repo.GetByIdAsync(id);
 if (p == null) return null;
 var membre = await _membreRepo.GetByIdAsync(p.IdMembre);
 return new PaiementListDto(p.IdPaiement, p.IdMembre, membre?.NomComplet ?? "-", p.Montant, p.DatePaiement, p.Methode);
 }
 public async Task UpdatePaiementAsync(int id, PaiementUpdateDto dto)
 {
 var p = await _paiementRepo.GetByIdAsync(id);
 if (p == null) throw new KeyNotFoundException("Paiement not found");
 var membre = await _membreRepo.GetByIdAsync(dto.IdMembre);
 if (membre == null) throw new KeyNotFoundException("Membre not found");
 p.IdMembre = dto.IdMembre;
 p.Montant = dto.Montant;
 p.Methode = dto.Methode;
 _paiementRepo.Update(p);
 await _paiementRepo.SaveChangesAsync();
 }
 public async Task DeletePaiementAsync(int id)
 {
 var p = await _paiementRepo.GetByIdAsync(id);
 if (p == null) throw new KeyNotFoundException("Paiement not found");
 _paiementRepo.Remove(p);
 await _paiementRepo.SaveChangesAsync();
 }
 }
}
