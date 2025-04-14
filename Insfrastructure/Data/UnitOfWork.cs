using Core.Enitities;
using Core.Interfaces;
using System.Collections.Concurrent;

namespace Insfrastructure.Data
{
    public class UnitOfWork(StoreContext context) : IUnitOfWork
    {
        private readonly ConcurrentDictionary<string, object> _repository = new();
        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            return (IGenericRepository<TEntity>)_repository.GetOrAdd(type, t =>
            {
                var respositoryType = typeof(GenericRepository<>).MakeGenericType(typeof(TEntity));
                return Activator.CreateInstance(respositoryType, context)
                    ?? throw new InvalidOperationException($"Could not create repository instance for {t}");
            });
        }
    }
}
