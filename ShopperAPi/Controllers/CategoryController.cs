using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = CatRepo.All().ToList();
            return Ok(result);
        }
        [HttpGet("id:int")]
        public async Task<IActionResult> Get(int id)
        {
            var category = (Category)CatRepo.Find(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Category category)
        {
 
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            var result =CatRepo.All().ToList();
            bool check= result.Contains(category);
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
        [HttpPost]
        public async Task<IActionResult> Edit( int id,[FromForm] Category category)
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
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
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
