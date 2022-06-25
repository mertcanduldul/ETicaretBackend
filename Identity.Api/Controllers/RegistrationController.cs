using System;
using Business.ServiceModel.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public string Get()
        {
            return "Successfully reached - ETicaretBackend.Login.Api - RegistrationController";
        }

        [HttpPost]
        [Route("Registration")]
        public RegistrationResponse Registration(RegistrationRequest request)
        {
            var response = new RegistrationResponse();

        }
    }
}

