using Data.Dapper.Repository.Product;
using Data.Entity.Product;
using Microsoft.AspNetCore.Mvc;

namespace Product.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;

    public ProductController(ILogger<ProductController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        return "Successfully reached - ETicaretBackend.Product.Api - ProductController";
    }

    [HttpGet]
    [Route("GetAll")]
    public IActionResult GetAll()
    {
        var productRepository = new ProductRepository();
        var products = productRepository.GetAllProducts();
        return Ok(products);
    }

    [HttpGet]
    [Route("GetProductByCategoryName")]
    public IActionResult GetProductByCategoryName(string categoryName)
    {
        ProductRepository repository = new ProductRepository();
        var productList = repository.GetProductByCategoryName(categoryName);
        return Ok(productList);
    }

    [HttpGet]
    [Route("GetAllColors")]
    public IActionResult GetAllColors()
    {
        ProductRepository repository = new ProductRepository();
        var colorList = repository.GetAllColors();
        return Ok(colorList);
    }

    [HttpGet]
    [Route("GetAllBrands")]
    public IActionResult GetAllBrands()
    {
        ProductRepository repository = new ProductRepository();
        var brandList = repository.GetAllBrands();
        return Ok(brandList);
    }

    [HttpGet]
    [Route("GetAllUsageStatuses")]
    public IActionResult GetAllUsageStatuses()
    {
        ProductRepository repository = new ProductRepository();
        var usageStatusList = repository.GetAllUsageStatuses();
        return Ok(usageStatusList);
    }
}