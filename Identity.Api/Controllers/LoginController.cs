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
         

           


            return response;
        }
    }
}