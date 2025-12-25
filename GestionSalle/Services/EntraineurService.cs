using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GestionSalle.DTOs;
using GestionSalle.Models;
using GestionSalle.Repositories;

namespace GestionSalle.Services
{
    public interface IEntraineurService
    {
        Task<IEnumerable<EntraineurListDto>> ListEntraineursAsync();
        Task CreateEntraineurAsync(EntraineurCreateDto dto);
        Task UpdateEntraineurAsync(int id, EntraineurUpdateDto dto);
        Task DeleteEntraineurAsync(int id);
    }

    public class EntraineurService : IEntraineurService
    {
        private readonly IRepository<Entraineur> _repo;
  private readonly IRepository<Utilisateur> _userRepo;
  private readonly IPasswordService _passwordService;

        public EntraineurService(IRepository<Entraineur> repo, IRepository<Utilisateur> userRepo, IPasswordService passwordService)
        {
    _repo = repo;
  _userRepo = userRepo;
      _passwordService = passwordService;
        }

  public async Task CreateEntraineurAsync(EntraineurCreateDto dto)
        {
   var all = await _repo.GetAllAsync();
       if (all.Any(e => e.Email == dto.Email))
   throw new InvalidOperationException("Email already exists");

// Create a user account automatically for the trainer
   var username = dto.Email.Split('@')[0];
     var tempPassword = GenerateTemporaryPassword();

        var user = new Utilisateur
    {
   NomUtilisateur = username,
    MotDePasse = _passwordService.HashPassword(tempPassword),
      Role = "User"
   };

   await _userRepo.AddAsync(user);
   await _userRepo.SaveChangesAsync();

    var entity = new Entraineur
   {
      NomComplet = dto.NomComplet,
Email = dto.Email,
          Telephone = dto.Telephone,
    Specialite = dto.Specialite.ToString().Replace("_", " "),
        IdUtilisateur = user.IdUtilisateur
    };

      await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
        }

private string GenerateTemporaryPassword()
        {
   return $"Temp{DateTime.Now:yyyyMMddHHmmss}!";
        }

 public async Task DeleteEntraineurAsync(int id)
   {
     var e = await _repo.GetByIdAsync(id);
     if (e == null) throw new KeyNotFoundException();
      _repo.Remove(e);
     await _repo.SaveChangesAsync();
   }

public async Task<IEnumerable<EntraineurListDto>> ListEntraineursAsync()
    {
       var all = await _repo.GetAllAsync();
   return all.Select(e => new EntraineurListDto(
     e.IdEntraineur,
           e.NomComplet,
           e.Email,
    e.Telephone,
  e.Specialite ?? "",
                e.Statut
       )).ToList();
        }

        public async Task UpdateEntraineurAsync(int id, EntraineurUpdateDto dto)
 {
  var e = await _repo.GetByIdAsync(id);
    if (e == null) throw new KeyNotFoundException();
            
    e.NomComplet = dto.NomComplet;
       e.Email = dto.Email;
   e.Telephone = dto.Telephone;
     e.Specialite = dto.Specialite.ToString().Replace("_", " ");
            
            _repo.Update(e);
   await _repo.SaveChangesAsync();
        }
    }
}
