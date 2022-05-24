using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public class RelatedProductSpec :BaseSpecification<Product>
    {
       public RelatedProductSpec(string categoryName , int productId)
        {
            addNewCratriaToWhere(p => p.Category.Name == categoryName);
            addNewCratriaToWhere(p => p.Id != productId);
            // p => p.Id != productId && 1
            AddInclude("Category");
        }
    }
}
