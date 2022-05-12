using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ShopperAPi.Errors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopperAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IBaseRepository<Category> CatRepo;


        public CategoryController(IBaseRepository<Category> catrepo)
        {
            this.CatRepo = catrepo;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult GetCategories()
        {
            var result = CatRepo.All().ToList();
            return Ok(result);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id:int}")]
        public IActionResult GetCategory(int id)
        {
            var category = CatRepo.FindOne(c => c.Id == id);
            if (category == null)
            {
                return NotFound( new ApiErrorResponse(404));
            }

            return Ok(category);
        }

        // POST api/<CategoryController>
        [HttpPost]
        public IActionResult PostCategories([FromForm] Category category)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = CatRepo.All().ToList();
            bool check = result.Contains(category);
            if (check)
            {
                return BadRequest(result);
            }
            try
            {
                CatRepo.Add(category);
                CatRepo.SaveChanges();
                string url = Url.Link("getRoute", new { id = category.Id });
                return Created(url, category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public IActionResult EditCategories(int id, [FromForm] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != category.Id)
            {
                return BadRequest();
            }
            try
            {

                var result = (Category)CatRepo.Find(c => c.Id == id);
                result.Name = category.Name;
                CatRepo.SaveChanges();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var result = CatRepo.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            CatRepo.Delete(result);
            CatRepo.SaveChanges();
            return Ok(result);
        }
    }
}
