using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class ISpecification<T>
    {
        public Expression<Func<T, bool>>? WhereCriteria { get; protected set; }
        public List<string>? Includes { get; protected set; }
        public Expression<Func<T, object>>? OrderCriteria { get; protected set; }
        public bool IsAscendingOrder { get; protected set; }
        public int Take { get; protected set; }
        public int Skip { get; protected set; }
        public bool IsPagingEnabled { get; protected set; }
    }
}
