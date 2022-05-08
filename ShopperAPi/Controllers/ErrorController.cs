using Microsoft.AspNetCore.Mvc;
using ShopperAPi.Errors;

namespace ShopperAPi.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : Controller
    {
        [HttpGet]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new ApiErrorResponse(statusCode));
        }
    }
}
