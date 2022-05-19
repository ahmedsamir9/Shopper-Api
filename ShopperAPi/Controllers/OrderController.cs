﻿using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShopperAPi.DTOS.OrderDto;
using ShopperAPi.Errors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopperAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }


      
        [HttpGet]
        public IActionResult Get()
        {
            var data = _orderService.getAllOrders();
            if (data == null) return NotFound(new ApiErrorResponse(
                 404, "No orders"));
            return Ok(data);
        }

        // GET api/<OrderController>/5
        [HttpGet("{userEmail}")]
        public IActionResult Get(string userEmail)
        {
           var order = _orderService.getOrdersForUser(userEmail);
            if(order == null) return NotFound(new ApiErrorResponse(404));
            return Ok(order);
        }

        // POST api/<OrderController>
        [HttpPost]
        public async Task<IActionResult> Post(UserOrderDto userOrder)
        {
            string basketId = "sss";
            var isProductAvalible = await _orderService.isProductsAvalibleAyncAsync(basketId);
            if (!isProductAvalible.Item1) {
                isProductAvalible.Item2.ForEach(e =>
                {
                    ModelState.AddModelError(e, e);
                });
                return BadRequest(ModelState);
            }
            var order = _orderService.createOrderAsync(_mapper.Map<Address>(userOrder.Address)
                ,userOrder.UserEmail,basketId);
            _orderService.saveChanges();
            return Created("", order);
        }

       

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _orderService.removeOrder(id);
            _orderService.saveChanges();
            return isDeleted ? NoContent() : NotFound(new ApiErrorResponse(404, "Order Not Found"));
        }
    }
}