using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contract
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        void add(TEntity entity);
        void update(TEntity entity);
        void delete(TEntity entity);

        Task<TEntity?> GetByIdAsync(TKey id, CancellationToken ct);
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct);
        Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity, TKey> specification, CancellationToken ct);
        Task<TEntity?> GetByIdAsync(TKey id, ISpecification<TEntity, TKey> specification, CancellationToken ct);
        Task<int> CountAsync(ISpecification<TEntity, TKey> specification, CancellationToken ct);


    }
}
