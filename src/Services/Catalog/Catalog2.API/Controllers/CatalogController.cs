using Catalog2.API.Entities;
using Catalog2.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog2.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await repository.GetProducts();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await repository.GetProduct(id);
            if(product == null)
            {
                _logger.LogError($"Product with id: {id}, not found");
                return NotFound();
            }
            return Ok(product);
        }

        [HttpGet]
        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<Product>> GetProductByCategory(string category)
        {
            var products = await repository.GetProductByCategory(category);
            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await repository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { Id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await repository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProductById(string id)
        {
            return Ok(await repository.deleteProduct(id));
        }
    }
}
