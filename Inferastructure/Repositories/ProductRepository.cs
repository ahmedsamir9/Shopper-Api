using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Inferastructure.DB;
using Inferastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inferastructure.Repositories
{
    public class ProductRepository :GenericRepository<Product>, IProductRepository
    {
        private AppDbContext Context { get; }
        public ProductRepository(AppDbContext _Context) : base(_Context)
        {
            Context = _Context;
        }
        public override IEnumerable<Product> All()
        {
            return Context.products.AsQueryable().Include(p => p.Category).ToList();
        }
        public override Product Get(int id)
        {
            var product = Context.products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
            return product;  
        }

        
        public List<Product> getRelatedProduct(BaseSpecification<Product> productSpec)
        {
            var query = Context.products.AsQueryable();
            return SpecificationEvaluator<Product>.GetQuery(query, productSpec).Take(Utilts.RELATED_PRODUCT_COUNT).ToList();
        }

        public List<Product> getPagedProduct(BaseSpecification<Product> productSpec)
        {
            var query = Context.products.AsQueryable();
            return SpecificationEvaluator<Product>.GetQuery(query, productSpec).ToList();
        }

        public int getPagesCount(BaseSpecification<Product> productSpec)
        {
            var query = Context.products.AsQueryable();
            //query = ;
            //var finalQuery = query ?? Context.products.AsQueryable();
            return SpecificationEvaluator<Product>.GetQuery(query, productSpec).Count();
        }
    }
}
