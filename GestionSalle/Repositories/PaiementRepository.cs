using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionSalle.Models;

namespace GestionSalle.Repositories
{
 public interface IPaiementRepository : IRepository<Paiement>
 {
 Task<IEnumerable<Paiement>> GetByMembreAsync(int idMembre);
 }
 public class PaiementRepository : Repository<Paiement>, IPaiementRepository
 {
 public PaiementRepository(SalleDbContext context) : base(context) { }
 public async Task<IEnumerable<Paiement>> GetByMembreAsync(int idMembre)
 {
 return await _context.Paiements.Where(p => p.IdMembre == idMembre).ToListAsync();
 }
 }
}
