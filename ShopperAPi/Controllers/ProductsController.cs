using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopperAPi.DTOS;
using ShopperAPi.DTOS.ProductDTOs;
using ShopperAPi.Errors;
using ShopperAPi.Helpers;
using ShopperAPi.Resources_Params;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopperAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository ProductRepsitory;
        private readonly IImageHandler ImageHandler;
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Category> _categoryRepository;
        public ProductsController(IProductRepository prdrepo, IImageHandler imageHandler, IBaseRepository<Category> categoryRepo, IMapper mapper)
        {
            this.ProductRepsitory = prdrepo;
            ImageHandler = imageHandler;
            _mapper = mapper;
            this._categoryRepository = categoryRepo;

        }
        // GET: api/<ProductsController>
        [HttpGet]
        [ProducesResponseType(typeof(List<ProductDto>), 200)]
        public IActionResult GetProducts()
        {
            var result = ProductRepsitory.All()
                .Select(e=> _mapper.Map<ProductDto>(e)).ToList();
            return Ok(result);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id:int}" ,Name ="getProduct")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public IActionResult GetProducts(int id)
        {
            var product = _mapper.Map<ProductDto>(ProductRepsitory.Get(id));
            if (product == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }

            return Ok(product);
        }

        [HttpGet("{id:int}/relatedproducts")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public IActionResult GetRelatedProducts(int id)
        {
            var product = _mapper.Map<ProductDto>(ProductRepsitory.Get(id));
            if(product == null) return NotFound(new ApiErrorResponse(404));
            var productSpec = new RelatedProductSpec(product.CategoryName,product.Id);
            var products = _mapper.Map<List<ProductDto>>(ProductRepsitory.getRelatedProduct(productSpec));
            if (products == null || products.Count == 0)
            {
                return NotFound(new ApiErrorResponse(404));
            }

            return Ok(products);
        }
        [HttpGet("PagedProducts")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public IActionResult GetProducts([FromQuery]ProductParams productParams)
        {
            var productSpec = new PagedProductSpec(productParams.CategoryName,productParams.PageSize 
                ,productParams.PageNumber.Value,productParams.maxPrice,productParams.miniPrice,productParams.rate);
            var productCountSpec = new CountPagedProduct(productParams.CategoryName
                , productParams.maxPrice, productParams.miniPrice, productParams.rate);
            var products = _mapper.Map<List<ProductDto>>(ProductRepsitory.getPagedProduct(productSpec));
            var count = ProductRepsitory.getPagesCount(productCountSpec);
           
            var pagedList = new PagingList<ProductDto>(productParams.PageNumber.Value, productParams.PageSize, count, products);
            return Ok(pagedList);
        }
        // POST api/<ProductsController>
        [Authorize(Roles =RolesConstantHelper.AdminRole)]
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
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
        [HttpGet("{categoryName}/ProductsCount")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public IActionResult GetProductCountInCategory(string categoryName)
        {
            var catgeory = _categoryRepository.FindOne(c=> c.Name == categoryName);
            if (catgeory == null) return NotFound(new ApiErrorResponse(404));
            var count = ProductRepsitory.getProductsCountInCategory(catgeory.Id);
            return Ok(count);
        }
        // PUT api/<ProductsController>/5
        [Authorize(Roles =RolesConstantHelper.AdminRole)]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        [ProducesResponseType(typeof(ApiErrorResponse), 400)]
        public IActionResult EditProducts(int id, [FromForm] ProductMainpulationsDto product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = ProductRepsitory.FindOne(c => c.Id == id);
            if (result == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }
            try
            {
                var resultProduct = _mapper.Map<Product>(product);
                result.CategoryID = resultProduct.CategoryID;
                result.Name = resultProduct.Name;
                result.Description = resultProduct.Description;
                result.ImagePath = resultProduct.ImagePath;
                result.Price = resultProduct.Price;
               result.NumberInStock = product.NumberInStock;

                ProductRepsitory.Update(result);

                ProductRepsitory.SaveChanges();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize(Roles = $"{RolesConstantHelper.AdminRole},{RolesConstantHelper.ClientRole}")]
        [HttpPut("{id}/Rate")]
        public IActionResult EditProducts(int id, int newRate)
        {
           
            var result = ProductRepsitory.FindOne(c => c.Id == id);
            if (result == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }
            try
            {
                float rate = (result.Rate + newRate) / 2;
                result.Rate = rate;
                ProductRepsitory.Update(result);

                ProductRepsitory.SaveChanges();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ProductsController>/5
        [Authorize(Roles =RolesConstantHelper.AdminRole)]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ProductDto), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public IActionResult DeleteProducts(int id)
        {
            var result = ProductRepsitory.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            ImageHandler.RemoveImage(result.ImagePath);
            ProductRepsitory.Delete(result);
            ProductRepsitory.SaveChanges();
            return Ok(result);
        }


    }
}
