using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionSalle.Models;

namespace GestionSalle.Repositories
{
 public interface IMembreRepository : IRepository<Membre>
 {
 Task<IEnumerable<Membre>> SearchAsync(string? query, string? statut);
 }
 public class MembreRepository : Repository<Membre>, IMembreRepository
 {
 public MembreRepository(SalleDbContext context) : base(context) { }
 public async Task<IEnumerable<Membre>> SearchAsync(string? query, string? statut)
 {
 var q = _context.Membres.AsQueryable();
 if (!string.IsNullOrEmpty(query))
 {
 q = q.Where(m => m.NomComplet.Contains(query) || m.Email.Contains(query) || (m.Telephone ?? string.Empty).Contains(query));
 }
 if (!string.IsNullOrEmpty(statut)) q = q.Where(m => m.Statut == statut);
 return await q.ToListAsync();
 }
 }
}
