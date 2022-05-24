using Core.Entities;
using Core.Interfaces;
using Inferastructure.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inferastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _appDbContext;
        private readonly  IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(AppDbContext appDbContext
            , IBasketRepository basketRepository, IProductRepository productRepository)
        {
            _appDbContext = appDbContext;
            _basketRepository = basketRepository;
            _productRepository = productRepository;

            _basketItems = new List<OrderItem>();
            _products = new List<Product>();

        }

        private List<Product> _products { get; set; }
        private List<OrderItem> _basketItems { get; set; }

        
        public IReadOnlyList<Order> getAllOrders()
        {
            return _appDbContext.Orders.Include(o => o.OrderItems)
               .ToList();

        }

        public Order? getOrderById(int orderId)
        {
            var order = _appDbContext.Orders.FirstOrDefault(o => o.OrderId == orderId);
      
            return order;
        }

        public IReadOnlyList<Order> getOrdersForUser(string userEmail)
        {
            return _appDbContext.Orders.Include(o => o.OrderItems)
                .Where(o=> o.UserEmail == userEmail).ToList();
        }

        

        public bool removeOrder(int orderId)
        {
            var order = _appDbContext.Orders.Include(o=> o.OrderItems).FirstOrDefault(o=> o.OrderId == orderId);
            if(order == null) return false;
            order.OrderItems.ForEach(e =>
            {
                var product = _productRepository.FindOne(p=>p.Id == e.ProductIdInDb);
                if (product != null) {

                    product.NumberInStock += e.Quantity;
                    _productRepository.Update(product);
                } 
            });
            _appDbContext.Orders.Remove(order);
            return true;
        }

        public void saveChanges()
        {
            _appDbContext.SaveChanges();
        }

        
        public async Task<Order> createOrderAsync(Address shippingAddress, string userEmail, string basketId)
        {

            var order = new Order()
            {
                ShippedAddress = shippingAddress,
                UserEmail = userEmail,
                OrderItems = _basketItems
            };
            _appDbContext.Orders.Add(order);

            _products.ForEach(p =>
            {
                _productRepository.Update(p);
            });
            await _basketRepository.deleteBasketAsync(basketId);
            return order;

        }

     public async Task<Tuple<bool, List<string>>> isProductsAvalibleAync(string basketId)
        {
            var errorList = new List<string>();
            var dataInBasket = await _basketRepository.getBasketAsync(basketId);
            if (dataInBasket == null)
            {
                errorList.Add("No basket to make Order");
            }
            var orderItems = dataInBasket.Items.Select(e => new OrderItem()
            {
                ProductIdInDb = e.Id,
                Price = e.Price
                ,
                ProductImage = e.ProductImage,
                ProductName = e.ProductName,
                Quantity = e.Quantity
            });
            foreach(var e in orderItems)
            {
                var product = _productRepository.FindOne(p => p.Id == e.ProductIdInDb);
                if (product == null)
                {
                    errorList.Add($"the {e.ProductName} is not  available on the system Now");
                }
                else
                {
                    if (product.NumberInStock == 0)
                    {
                        errorList.Add($"the {e.ProductName} is not  available on Stock Now");

                    }
                    else if (product.NumberInStock < e.Quantity)
                    {
                        errorList.Add($"the {e.ProductName}  Quantity is not  available on Stock Now" +
                            ", please decrease it ");
                    }
                    else
                    {
                        _basketItems.Add(e);
                        product.NumberInStock -= e.Quantity;
                        _products.Add(product);
                    }
                }
            }
            
            if (errorList.Count > 0)
            {
                _products = null;
                _basketItems = null;
                return new Tuple<bool, List<string>>(false, errorList);
            }
            return new Tuple<bool, List<string>>(true, errorList);
        }
    }
}
