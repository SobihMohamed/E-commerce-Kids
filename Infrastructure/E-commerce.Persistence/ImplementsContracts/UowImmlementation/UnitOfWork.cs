using E_commerce.Domain.Contracts.GenericReposPattern;
using E_commerce.Domain.Contracts.UnitOfWorkPattern;
using E_commerce.Persistence.E_commerceDbContext;
using E_commerce.Persistence.ImplementsContracts.RepoImplementatoin;

namespace E_commerce.Persistence.ImplementsContracts.UowImmlementation
{
    public class UnitOfWork(EcommerceDbContext _context) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        IGenericRepo<TEntity, TKey> IUnitOfWork.GetRepository<TEntity, TKey>()
        {
            var type = typeof(TEntity).Name;
            if (_repositories.ContainsKey(type))
                return (IGenericRepo<TEntity, TKey>)_repositories[type]; // cast the object to the correct type before returning it
            var repositoryInstance = new GenericRepo<TEntity, TKey>(_context); // create a new instance of the repository
            _repositories[type] = repositoryInstance; // store the repository instance in the dictionary for future use
            return repositoryInstance; // return the newly created repository instance

        }
        // SaveChangesAsync() is used to save all changes made to the database in a single transaction,
        // it returns the number of state entries written to the database.
        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync(); 
        public async ValueTask DisposeAsync() // is used to close the database connection when the unit of work is disposed, ensuring that resources are released properly.
         => await _context.DisposeAsync();

    }
}
