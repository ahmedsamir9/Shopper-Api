using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IProductRepository:IBaseRepository<Product>
    {
        List<Product> getRelatedProduct(BaseSpecification<Product> productSpec);
        List<Product> getPagedProduct(BaseSpecification<Product> productSpec);
        int getPagesCount(BaseSpecification<Product> productSpec);
        int getProductsCountInCategory(int categoryName);
    }
}
