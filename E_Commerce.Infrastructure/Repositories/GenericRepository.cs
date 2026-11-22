using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contract;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.EvaluatorSpecification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, Tkey>(StoreDbContext context) : IGenericRepository<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        

        public void add(TEntity entity)
        {
            context.Set<TEntity>().Add(entity);
        }
        public void update(TEntity entity)
        {
            context.Set<TEntity>().Update(entity);
        }
        public void delete(TEntity entity)
        {
            context.Set<TEntity>().Remove(entity);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct)
        {
            return await context.Set<TEntity>().ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id, CancellationToken ct)
        {
            return await context.Set<TEntity>().FindAsync(id, ct);
        }
        public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity, Tkey> specification, CancellationToken ct)
        {
            return await SpecificationEvaluator.Evaluator<TEntity, Tkey>(context.Set<TEntity>(), specification).ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id, ISpecification<TEntity, Tkey> specification, CancellationToken ct)
        {
            return await SpecificationEvaluator.Evaluator<TEntity, Tkey>(context.Set<TEntity>(), specification).FirstOrDefaultAsync(ct);
        }

        public async Task<int> CountAsync(ISpecification<TEntity, Tkey> specification, CancellationToken ct)
        {
            return await SpecificationEvaluator.Evaluator<TEntity, Tkey>(context.Set<TEntity>(), specification).CountAsync(ct);
        }
    }
}
