using Microsoft.EntityFrameworkCore;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inferastructure.Repositories
{

    public class SpecificationEvaluator<T> where T : class 
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            if (spec.WhereCriteria != null)
            {
                query = query.Where(spec.WhereCriteria);
            }

            if (spec.OrderCriteria != null)
            {
                if (spec.IsAscendingOrder) {
                    query = query.OrderBy(spec.OrderCriteria);
                }
                else
                {
                    query = query.OrderByDescending(spec.OrderCriteria);
                }
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            query = spec.Includes?.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}