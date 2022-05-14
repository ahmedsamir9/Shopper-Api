using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShopperAPi.DTOS;
using ShopperAPi.DTOS.ProductDTOs;
using ShopperAPi.Errors;
using ShopperAPi.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopperAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBaseRepository<Product> ProductRepsitory;
        private readonly IImageHandler ImageHandler;
        private readonly IMapper _mapper;

        public ProductsController(IBaseRepository<Product> prdrepo, IImageHandler imageHandler,IMapper mapper)
        {
            this.ProductRepsitory = prdrepo;
            ImageHandler = imageHandler;
            _mapper = mapper;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public IActionResult GetProducts()
        {
            var result = ProductRepsitory.All()
                .Select(e=> _mapper.Map<ProductDto>(e)).ToList();
            return Ok(result);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id:int}" ,Name ="getProduct")]
        public IActionResult GetProducts(int id)
        {
            var product = _mapper.Map<ProductDto>(ProductRepsitory.Get(id));
            if (product == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }

            return Ok(product);
        }

        // POST api/<ProductsController>
        [HttpPost]


        public IActionResult PostProducts([FromForm] ProductMainpulationsDto product)
        {
         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try {
                var resultProduct = _mapper.Map<Product>(product);
                ProductRepsitory.Add(resultProduct);
                ProductRepsitory.SaveChanges();
          
                return Created("getProduct", resultProduct);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(400,ex.Message));
            }
        }

        // PUT api/<ProductsController>/5
        //[HttpPut("{id}")]
        //public IActionResult EditProducts(int id, [FromForm] Product category)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    if (id != category.Id)
        //    {
        //        return BadRequest();
        //    }
        //    try
        //    {

        //        var result = CatRepo.FindOne(c => c.Id == id);
        //        result.Name = category.Name;
        //        CatRepo.SaveChanges();
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProducts(int id)
        {
            var result = ProductRepsitory.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            ProductRepsitory.Delete(result);
            ProductRepsitory.SaveChanges();
            return Ok(result);
        }


    }
}
