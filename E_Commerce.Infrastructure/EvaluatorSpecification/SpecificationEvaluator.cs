using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.EvaluatorSpecification
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> Evaluator<TEntity, Tkey>(IQueryable<TEntity> Inputquery, ISpecification<TEntity, Tkey> specification) where TEntity : BaseEntity<Tkey>
        {
            var query = Inputquery;

            if(specification.WhereExpression != null)
            {
                query = query.Where(specification.WhereExpression);
            }

            if(specification.IncludeExpressions != null)
            {
                foreach(var expression in specification.IncludeExpressions)
                {
                    query = query.Include(expression);
                }
            }
            if(specification.OrderProductAsc != null)
            {
                query = query.OrderBy(specification.OrderProductAsc);
            }
            else if(specification.OrderProductDesc != null)
                query = query.OrderByDescending(specification.OrderProductDesc);

            if (specification.IsPagination)
            {
                query = query.Skip(specification.Skip).Take(specification.Take);
            }

            return query;
        }
    }
}
