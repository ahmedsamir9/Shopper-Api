using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopperAPi.Errors;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShopperAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IBaseRepository<Category> CatRepository;


        public CategoryController(IBaseRepository<Category> catrepo)
        {
            this.CatRepository = catrepo;
        }
        // GET: api/<CategoryController>
        [HttpGet]
        public IActionResult GetCategories()
        {
            var result = CatRepository.All().ToList();
            return Ok(result);
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id:int}")]
        public IActionResult GetCategory(int id)
        {
            var category = CatRepository.FindOne(c => c.Id == id);
            if (category == null)
            {
                return NotFound( new ApiErrorResponse(404));
            }

            return Ok(category);
        }

        // POST api/<CategoryController>
        [Authorize(Roles =RolesConstantHelper.AdminRole)]
        [HttpPost]
        public IActionResult PostCategories(string name)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = CatRepository.FindOne(c => c.Name == name);
            if (result != null)
            {
                return BadRequest(result);
            }
            try
            {
                var category = new Category() { Name = name};
                CatRepository.Add(category);
                CatRepository.SaveChanges();
                //string url = Url.Link("getRoute", new { id = category.Id });
                return Created("lll", category);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT api/<CategoryController>/5
        [Authorize(Roles =RolesConstantHelper.AdminRole)]
        [HttpPut("{id}")]
        public IActionResult EditCategories(int id, [FromForm] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //if (id != category.Id)
            //{
            //    return BadRequest();
            //}
            try
            {

                var result = CatRepository.FindOne(c => c.Id == id);
                result.Name = category.Name;
                CatRepository.SaveChanges();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles =RolesConstantHelper.AdminRole)]
        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var result = CatRepository.Get(id);
            if (result == null)
            {
                return NotFound(new ApiErrorResponse(404));
            }
            CatRepository.Delete(result);
            CatRepository.SaveChanges();
            return Ok(result);
        }
    }
}
