using Microsoft.EntityFrameworkCore;
using Sky.Api.Application.Interfaces;
using Sky.Api.Domain.Definitions;
using Sky.Api.Infrastructure.Data;

namespace Sky.Api.Infrastructure.Repositories
{
    public class Repository<T>(AppDbContext context) : IRepository<T> where T : Entity
    {
        protected readonly DbSet<T> _dbSet = context.Set<T>();
        public Task CreateAsync(T entidade, CancellationToken cancellationToken = default)
        {
            _dbSet.AddAsync(entidade, cancellationToken);
            return context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await GetByIdAsync(id, cancellationToken);

            if (entity is not null)
            {
                _dbSet.Remove(entity);
                await context.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<IEnumerable<T>?> GetAllAsync(CancellationToken cancellationToken = default)
            => await _dbSet.AsNoTracking().ToListAsync(cancellationToken).ContinueWith(t=> (IEnumerable<T>) t.Result, cancellationToken);

        public Task<IEnumerable<T>?> GetAllAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
        {
            var result =  _dbSet.AsNoTracking().Where(predicate).ToList();           
            return Task.FromResult(result?.AsEnumerable());
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task<T?> GetAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.AsNoTracking().FirstOrDefaultAsync(e => predicate(e), cancellationToken);

        public Task UpdateAsync(T entidade, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entidade);
            return context.SaveChangesAsync(cancellationToken);
        }
    }
}
