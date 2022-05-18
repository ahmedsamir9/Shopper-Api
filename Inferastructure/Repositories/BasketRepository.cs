using Core.Entities;
using Core.Interfaces;

using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Inferastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }


        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<Basket?> GetBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(data);
        }

       

        public async Task<Basket> UpdateBasketAsync(string basketId, BasketItem basket)
        {
            var data =await GetBasketAsync(basketId);
            if (data == null)
            {
                data = new Basket()
                {
                    Id = basketId
                };
            }
            data.Items.Add(basket);
            var created = await _database.StringSetAsync(basketId,
                JsonSerializer.Serialize(data), TimeSpan.FromDays(30));

            if (!created) return null;

            return data;
        }
    }
}
