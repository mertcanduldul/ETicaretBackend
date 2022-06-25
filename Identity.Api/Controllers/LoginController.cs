using Business.ServiceModel.Identity;
using Data.Dapper.Repository.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceModel.Identity;
using Data.Entity.Identity;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Successfully reached - ETicaretBackend.Login.Api - LoginController";
        }

        [HttpPost]
        [Route("Login")]
        public LoginResponse Login(LoginRequest request)
        {
            var response = new LoginResponse();
            IdentityRepository repository = new IdentityRepository();
            KU_KULLANICI kullanici = repository.UserLogin(request.E_MAIL, request.SIFRE);


            if (kullanici != null)
            {
                response.IsSuccess = true;
                response.Message = "Giriş işlemi başarılı !";
                response.UserName = kullanici.AD_SOYAD;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Giriş işlemi başarısız !";
                response.UserName = "";
            }

            return response;
        }
    }
}