using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class BasketItem
    { 
            public int Id { get; set; }
            public string ProductName { get; set; }
            public float Price { get; set; }
            public int Quantity { get; set; }
            public string ProductImage { get; set; }
          
        public override bool Equals(object? obj)
        {
            var item = obj as BasketItem;
            if (item == null)
            {
                return false;
            }
            if(ReferenceEquals(this,item))return true;
            return this.Id == item.Id;
        }
    }
}
