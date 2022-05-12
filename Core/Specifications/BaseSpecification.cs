using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            WhereCriteria = criteria;
        }
      
        protected void AddInclude(string includeName)
        {
            if (Includes == null) {
                Includes = new List<string>();
            }
            Includes?.Add(includeName);
        }
      
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            IsAscendingOrder = true;
            OrderCriteria = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            IsAscendingOrder = false;
            OrderCriteria= orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
        public void addNewCratriaToWhere(Expression<Func<T, bool>> newCriteria) {
            if (WhereCriteria == null)
            {
                WhereCriteria = newCriteria;
            }
            else {
                var invokedExpression = Expression.Invoke(newCriteria, WhereCriteria.Parameters);
                WhereCriteria = (Expression<Func<T, bool>>)Expression.Lambda(Expression.AndAlso(WhereCriteria.Body, invokedExpression), WhereCriteria.Parameters);
            }
        }
    }
    
}
