using System.Linq.Expressions;

namespace AquaFarm.Infrastructure.Repositories.Contracts
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        Task<TEntity> InsertAsync(TEntity entity);

        Task InsertRangeAsync(IEnumerable<TEntity> entityList);

        Task<TEntity?> GetByIdAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<int> CountAsync();

        Task DeleteByIdAsync(int id);

        Task<IEnumerable<TEntity>> QueryAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includeProperties
       );

        Task<IEnumerable<TResult>> ProjectAsync<TResult>(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            Expression<Func<TEntity, TResult>>? selector = null,
            int? skip = null,
            int? take = null,
            params Expression<Func<TEntity, object>>[] includeProperties
        );
    }
}
