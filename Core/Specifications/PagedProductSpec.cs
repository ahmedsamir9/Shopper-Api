using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class PagedProductSpec:BaseSpecification<Product>
    {
        public PagedProductSpec(string? categoryName, int pageSize, int pageNum
            , int? maxPrice, int? miniPrice, int? rate) 
        {
            if (rate != null) addNewCratriaToWhere(p => p.Rate >= rate);
            if (categoryName != null) addNewCratriaToWhere(p => p.Category.Name == categoryName);
            if (maxPrice != null && miniPrice != null) addNewCratriaToWhere(p=> p.Price >= miniPrice && p.Price <= maxPrice);
            AddInclude("Category");
            ApplyPaging(pageNum, pageSize);
        
        }
    }
}
