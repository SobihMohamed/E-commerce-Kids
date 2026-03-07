using E_commerce.Domain.Contracts;
using E_commerce.Domain.Contracts.GenericRepos;
using E_commerce.Persistence.E_commerceDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Persistence.ImplementsContracts.RepoImplementatoin
{
    public class GenericRepo<TEntity, TKey>
        : IGenericRepo<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        private readonly DbSet<TEntity> _dbSet;
        public GenericRepo(EcommerceDbContext context)
        {
            _dbSet = context.Set<TEntity>();
        }
        public async Task<IReadOnlyList<TEntity>> GetAllAsync()
            => await _dbSet.AsNoTracking().ToListAsync(); // AsNoTracking() is used to improve performance when you don't need to track changes to the entities.

        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await _dbSet.FindAsync(id);
        public async Task AddAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);
        public void Delete(TEntity entity)
            => _dbSet.Remove(entity);


    }
}
