using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShopperAPi.Errors;
using ShopperAPi.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopperAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {

            _basketRepository = basketRepository;
        }

        [HttpGet]
        public async Task<ActionResult<Basket>> GetBasketForUser()
        {
            var sid = "ss";
            var basketId = sid.hashStrings("Email");
            var basket = await _basketRepository.GetBasketAsync(basketId);
            if (basket == null) return BadRequest(new ApiErrorResponse(400, "You got No Basket"));
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> AddToBasketBasket(BasketItem basketItem)
        {
            var sid = "ss";
            var basketId = sid.hashStrings("Email");
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basketId,basketItem);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteUserBasket()
        {
            var sid = "ss";
            var basketId = sid.hashStrings("Email");
            await _basketRepository.DeleteBasketAsync(basketId);
        }
    }
}
