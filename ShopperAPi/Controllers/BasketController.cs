﻿using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopperAPi.Errors;
using ShopperAPi.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopperAPi.Controllers
{
   // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private Tuple<string, string>? _currentUserData;
        public BasketController(IBasketRepository basketRepository)
        {
            
            _basketRepository = basketRepository;
        }
        
        [HttpGet(Name ="getBasket")]
        public async Task<ActionResult<Basket>> GetBasketForUser()
        {
            //_currentUserData = User.Claims.getCurrentUserIdAndEmail();

            var basketId = "sss";
            var basket = await _basketRepository.getBasketAsync(basketId);
            if (basket == null) return BadRequest(new ApiErrorResponse(400, "You got No Basket"));
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<Basket>> AddToBasketBasket(BasketItem basketItem)
        {
            //var basketId = _currentUserData.Item1.hashStrings(_currentUserData.Item2);
            var basketId = "sss";
            var updatedBasket = await _basketRepository.addToBasketAsync(basketId, basketItem);
            return Created("getBasket()",updatedBasket);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserBasket()
        {
           // var basketId = _currentUserData.Item1.hashStrings(_currentUserData.Item2);
            var basketId = "sss";
            var isDeleted = await _basketRepository.deleteBasketAsync(basketId);
            if (!isDeleted) return BadRequest(new ApiErrorResponse(400, "No basket To Remove it"));
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemBasket(int id)
        {
            var basketId = _currentUserData.Item1.hashStrings(_currentUserData.Item2);
            var basket = await _basketRepository.deleteFromBasketAsync(basketId,id);
            if(basket == null) return NotFound(new ApiErrorResponse(404,"the Item is not in basket"));
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItemBasket(int id,BasketItem basketItem)
        {
            var basketId = _currentUserData.Item1.hashStrings(_currentUserData.Item2);
            var basket = await _basketRepository.updateBasketItemAsync(basketId, basketItem);
            if (basket == null) return NotFound(new ApiErrorResponse(404));
            return NoContent();
        }
    }
}
