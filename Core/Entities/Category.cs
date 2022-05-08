using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
