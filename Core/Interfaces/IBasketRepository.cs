using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<Basket> getBasketAsync(string basketId);
        Task<Basket> addToBasketAsync(string basketId,BasketItem basketItem);
        Task<Basket> updateBasketItemAsync(string basketId,BasketItem basketItem);
        Task<Basket> deleteFromBasketAsync(string basketId,int itemId);
        
        Task<bool> deleteBasketAsync(string basketId);
    }
}
