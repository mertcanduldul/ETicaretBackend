using Data.Dapper.Repository.Product;
using Data.Entity.Product;
using Microsoft.AspNetCore.Mvc;
using ServiceModel;
using ServiceModel.Product;

namespace Product.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(ILogger<CategoryController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        return "Successfully reached - ETicaretBackend.Product.Api - CategoryController";
    }

    [HttpGet]
    [Route("GetAllCategory")]
    public List<CategoryResponse> GetAllCategory()
    {
        List<CategoryResponse> categoryResponses = new List<CategoryResponse>();
        CategoryRepository productRepository = new CategoryRepository();
        List<UR_KATEGORI> KategoriList = productRepository.GetAll().ToList();

        foreach (var item in KategoriList)
        {
            CategoryResponse categoryResponse = new CategoryResponse();
            categoryResponse.ID_KATEGORI = item.ID_KATEGORI;
            categoryResponse.KATEGORI_ADI = item.KATEGORI_ADI;
            categoryResponse.IsSuccess = true;
            categoryResponses.Add(categoryResponse);
        }

        return categoryResponses;
    }

    [HttpGet]
    [Route("GetCategoryByCategoryName")]
    public CategoryResponse GetCategoryByCategoryName(string name)
    {
        CategoryRepository productRepository = new CategoryRepository();
        CategoryResponse response = new CategoryResponse();
        UR_KATEGORI category = productRepository.GetCategoryByCategoryName(name);

        if (category != null)
        {
            response.ID_KATEGORI = category.ID_KATEGORI;
            response.KATEGORI_ADI = category.KATEGORI_ADI;
            response.IsSuccess = true;
        }
        else
        {
            response.IsSuccess = false;
        }

        return response;
    }

    [HttpPost]
    [Route("AddCategory")]
    public ServicesResponse AddCategory(CategoryRequest request)
    {
        CategoryRepository productRepository = new CategoryRepository();
        ServicesResponse response = new ServicesResponse();
        UR_KATEGORI category = new UR_KATEGORI();

        category.KATEGORI_ADI = request.KATEGORI_ADI;
        category.CREDATE = DateTime.Now;
        productRepository.Add(category);

        response.IsSuccess = true;
        return response;
    }

    [HttpPost]
    [Route("UpdateCategoryByIdCategory")]
    public ServicesResponse UpdateCategoryByIdCategory(UpdateCategoryRequest request)
    {
        CategoryRepository productRepository = new CategoryRepository();
        ServicesResponse response = new ServicesResponse();
        UR_KATEGORI category = new UR_KATEGORI();

        category.ID_KATEGORI = request.ID_KATEGORI;
        category.KATEGORI_ADI = request.KATEGORI_ADI;
        category.CREDATE = DateTime.Now;
        productRepository.Update(category);

        response.IsSuccess = true;
        return response;
    }
}