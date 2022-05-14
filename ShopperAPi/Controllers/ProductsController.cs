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
    public class ProductsController : ControllerBase
    {
        private readonly IBaseRepository<Product> prdRepo;
        private readonly IImageHandler ImageHandler;


        public ProductsController(IBaseRepository<Product> prdrepo, IImageHandler imageHandler)
        {
            this.prdRepo = prdrepo;
            ImageHandler = imageHandler;
        }
        // GET: api/<ProductsController>
        [HttpGet]
        public IActionResult GetProducts()
        {
            var result = prdRepo.All().ToList();
            return Ok(result);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id:int}")]
        public IActionResult GetProducts(int id)
        {
            var product = prdRepo.Get(id);
            if (product == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }

            return Ok(product);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public IActionResult PostProducts([FromForm] Product product)
        {
           // var res = product.files[0];
         
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = prdRepo.All().ToList();
            bool check = result.Contains(product);
            if (check)
            {
                return BadRequest(result);
            }
            try
            {
                string uniqueFileName = ImageHandler.UploadImage(product);
                product.ImagePath = uniqueFileName;
                prdRepo.Add(product);
                prdRepo.SaveChanges();
                //string url = Url.Link("getRoute", new { id = category.Id });
                return Created("lll", product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
            var result = prdRepo.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            prdRepo.Delete(result);
            prdRepo.SaveChanges();
            return Ok(result);
        }


    }
}
