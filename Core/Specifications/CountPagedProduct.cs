using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class CountPagedProduct:BaseSpecification<Product>
    {
        public CountPagedProduct(string? categoryName,int? maxPrice, int? miniPrice, int? rate)
        {
            if (rate != null) addNewCratriaToWhere(p => p.Rate >= rate);
            if (categoryName != null) addNewCratriaToWhere(p => p.Category.Name == categoryName);
            if (maxPrice != null && miniPrice != null) addNewCratriaToWhere(p => p.Rate >= miniPrice && p.Rate <= maxPrice);
          

        }
    }
}
