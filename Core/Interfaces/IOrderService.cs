using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IOrderService
    {
        void saveChanges();
        Task<Order> createOrderAsync(Address shippingAddress,string userEmail, string basketId);
        IReadOnlyList<Order> getOrdersForUser(string userEmail);
        IReadOnlyList<Order> getAllOrders();

         Order? getOrderById(int orderId);
        bool removeOrder(int orderId);
        Task <Tuple<bool, List<string>>> isProductsAvalibleAync(string basketId);
    }
}
