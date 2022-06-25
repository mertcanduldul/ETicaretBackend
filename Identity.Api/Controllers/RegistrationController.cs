using Business.ServiceModel.Identity;
using Data.Dapper.Repository.Identity;
using Data.Entity.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

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
            try
            {
                RegistrationResponse response = new RegistrationResponse();
                IdentityRepository identityRepository = new IdentityRepository();
                KU_KULLANICI existUser = new KU_KULLANICI();
                existUser = identityRepository.HaveAccountWithThatEmail(request.E_MAIL);

                if (existUser.ID_KULLANICI > 0)
                {
                    response.IsSuccess = false;
                    response.Message = "E-Mail Adresi kullanılıyor. Üyelik Başarısız !";
                    return response;
                }


                KU_KULLANICI kullanici = new KU_KULLANICI();
                kullanici.AD_SOYAD = request.AD_SOYAD;
                kullanici.E_MAIL = request.E_MAIL;
                kullanici.SIFRE = request.SIFRE;
                kullanici.CREDATE = DateTime.Now;
                identityRepository.Add(kullanici);

                if (kullanici.ID_KULLANICI != 0)
                {
                    response.E_MAIL = kullanici.E_MAIL;
                    response.AD_SOYAD = kullanici.AD_SOYAD;
                    response.IsSuccess = true;
                    response.Message = "Üyelik Başarılı !";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = "Üyelik Başarısız !";
                }

                return response;
            }
            catch (SystemException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}