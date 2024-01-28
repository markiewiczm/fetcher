using Microsoft.AspNetCore.Mvc;
using Extractor.WebAPI.Queries;

namespace Extractor.WebAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/Products")]
    public class ProductsController : Controller
    {
        private readonly IProductQueryService _productService;
        public ProductsController(IProductQueryService productService)
        {
            _productService = productService;
        }

        [HttpGet()]
        public IActionResult GetProducts()
        {
            var products = _productService.GetProducts();
            return new OkObjectResult(products);
        }
    }
}
