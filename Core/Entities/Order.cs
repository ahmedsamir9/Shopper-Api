using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserEmail { get; set; }
        public OrderState OrderState { get; set; } = OrderState.Pendding;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
        public Address ShippedAddress { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
