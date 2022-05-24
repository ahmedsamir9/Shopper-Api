using Core.Entities;
using Inferastructure.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inferastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>
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
    }
}
