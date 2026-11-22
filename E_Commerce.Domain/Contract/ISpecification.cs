using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contract
{
    public interface ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
        Expression<Func<TEntity, bool>> WhereExpression { get; }
        public Expression<Func<TEntity, object>> OrderProductAsc { get;}
        public Expression<Func<TEntity, object>> OrderProductDesc { get; }

        int Skip { get; }
        int Take { get; }
        bool IsPagination { get; set; }


    }
}
