using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController : ControllerBase
{
    public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
    {
        Repository = repository;
        Logger = logger;
    }

    private IProductRepository Repository { get; }
    private ILogger<CatalogController> Logger { get; }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        return Ok(await Repository.GetProducts());
    }

    [HttpGet("{id:length(24)}", Name = "GetProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> GetProductById(string id)
    {
        var foundProduct = await Repository.GetProduct(id);
        if (foundProduct == null)
        {
            Logger.LogError($"Product with id {id} not found!");
            return NotFound();
        }

        return Ok(foundProduct);
    }

    [Route("[action]/{category}", Name = "GetProductByCategory")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
    {
        return Ok(await Repository.GetProductByCategory(category));
    }

    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        await Repository.CreateProduct(product);
        return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    [HttpPut]
    [ProducesResponseType(typeof(Product), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        await Repository.UpdateProduct(product);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteByProductId(string id)
    {
        await Repository.DeleteProduct(id);
        return NoContent();
    }
}