using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GestionSalle.Models;

namespace GestionSalle.Repositories
{
 public class Repository<T> : IRepository<T> where T : class
 {
 protected readonly SalleDbContext _context;
 private readonly DbSet<T> _dbSet;
 public Repository(SalleDbContext context)
 {
 _context = context;
 _dbSet = _context.Set<T>();
 }
 public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
 public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
 public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
 public void Remove(T entity) => _dbSet.Remove(entity);
 public void Update(T entity) => _dbSet.Update(entity);
 public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
 }
}
