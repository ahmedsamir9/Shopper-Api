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

        public async Task<Basket> addToBasketAsync(string basketId, BasketItem basketItem)
        {

            var data = await getBasketAsync(basketId);
            if (data == null)
            {
                data = new Basket()
                {
                    Id = basketId
                };
            }

            if (!data.Items.Contains(basketItem)) {
                data.Items.Add(basketItem);
                if (!await addDataToRedis(data)) return null;
            } 
            return data;
        }

        public async Task<bool> deleteBasketAsync(string basketId)
        {
            return await _database.KeyDeleteAsync(basketId);
        }

        public async Task<Basket> deleteFromBasketAsync(string basketId, int itemId)
        {
            var data = await getBasketAsync(basketId);
            if (data == null) return null;
            var basketItem = data.Items.FirstOrDefault(x => x.Id == itemId);
            if (basketItem == null) return null;
            data.Items.Remove(basketItem);
            await addDataToRedis(data);
            return data;
        }

        public async Task<Basket?> getBasketAsync(string basketId)
        {
            var data = await _database.StringGetAsync(basketId);

            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(data);
        }

        public async Task<Basket> updateBasketItemAsync(string basketId, BasketItem basketItem)
        {
            var data = await getBasketAsync(basketId);
            var isNotFound = true;
            if (data == null) return null;
            for(int i = 0; i < data.Items.Count; i++)
            {
                if(data.Items[i].Id == basketItem.Id)
                {
                    data.Items.RemoveAt(i);
                    data.Items.Add(basketItem);
                    isNotFound = false;
                    await addDataToRedis(data);
                    break;
                }
            }
            if(isNotFound) return null;
            return data;
        }
        public async Task<bool> addDataToRedis(Basket basket) {
            return await _database.StringSetAsync(basket.Id,
                        JsonSerializer.Serialize(basket), TimeSpan.FromDays(30));
            
        }
    }
}
