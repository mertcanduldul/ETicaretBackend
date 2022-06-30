using Business.ServiceModel.Product;
using Data.Dapper.Repository.Product;
using Data.Entity.Product;
using Microsoft.AspNetCore.Mvc;
using ServiceModel;

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

    [HttpPost]
    [Route("AddProduct")]
    public ServicesResponse AddProduct(AddProductRequest request)
    {
        ProductRepository repository = new ProductRepository();
        UR_URUN obj = new UR_URUN();
        obj.URUN_ADI = request.URUN_ADI;
        obj.URUN_ACIKLAMA = request.URUN_ACIKLAMA;
        obj.ID_RENK = request.ID_RENK;
        obj.ID_MARKA = request.ID_MARKA;
        obj.ID_KATEGORI = request.ID_KATEGORI;
        obj.ID_KULLANIM_DURUMU = request.ID_KULLANIM_DURUMU;
        obj.IS_OFFERABLE = request.IS_OFFERABLE;
        obj.FIYAT = request.FIYAT;
        obj.ID_KULLANICI = request.ID_URUN_SAHIBI;
        obj.CREDATE = DateTime.Now;

        repository.Add(obj);
        return new ServicesResponse()
        {
            IsSuccess = true,
            Message = "Ürün başarıyla eklendi."
        };
    }
    
    [HttpGet]
    [Route("GetUserProduct")]
    public IActionResult GetUserProduct(int userId)
    {
        ProductRepository repository = new ProductRepository();
        var productList = repository.GetUserProduct(userId);
        return Ok(productList);
    }
}