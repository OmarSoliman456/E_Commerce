using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specification
{
    public class BaseSpicification<TEntity, Tkey> : ISpecification<TEntity, Tkey> where TEntity : BaseEntity<Tkey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = [];
        public Expression<Func<TEntity, bool>> WhereExpression { get; }
        public Expression<Func<TEntity, object>>? OrderProductAsc { get; private set; }
        public Expression<Func<TEntity, object>>? OrderProductDesc { get; private set; }

        public int Take {  get; private set; }

        public int Skip { get; private set; }

        public bool IsPagination { get; set; }

        public void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpressions.Add(expression);
        }
        public BaseSpicification(Expression<Func<TEntity, bool>> expression)
        {
            WhereExpression = expression;
        }

        protected void AddOrderAsc(Expression<Func<TEntity, object>> expression)
        {
            OrderProductAsc = expression;
        }
        protected void AddOrderDesc(Expression<Func<TEntity, object>> expression)
        {
            OrderProductDesc = expression;
        }
        protected void AddPagination(int pageIndex, int pageSize)
        {
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize;
            IsPagination = true;
        }
    }
}
