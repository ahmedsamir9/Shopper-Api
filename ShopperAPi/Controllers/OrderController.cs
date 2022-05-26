using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopperAPi.DTOS.OrderDto;
using ShopperAPi.Errors;
using ShopperAPi.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopperAPi.Controllers
{
    [Authorize(Roles =$"{RolesConstantHelper.AdminRole},{RolesConstantHelper.ClientRole}")]
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
            var _currentUserData = User.Claims.getCurrentUserIdAndEmail();
            var basketId = _currentUserData.Item1.hashStrings(_currentUserData.Item2);
            var isProductAvalible = await _orderService.isProductsAvalibleAync(basketId);
            if (!isProductAvalible.Item1) {
                isProductAvalible.Item2.ForEach(e =>
                {
                    ModelState.AddModelError("",e);
                });
                return BadRequest(ModelState);
            }
            var order = await _orderService.createOrderAsync(_mapper.Map<Address>(userOrder.Address)
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
