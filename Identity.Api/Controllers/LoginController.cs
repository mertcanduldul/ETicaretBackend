using System.Net;
using Business.ServiceModel.Identity;
using Business.ServiceModel.Security;
using Dapper.Repository.Base;
using Data.Dapper.Repository.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceModel.Identity;
using Data.Entity.Identity;
using Newtonsoft.Json;

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
        public LoginResponse Login(LoginRequest request)
        {
            var response = new LoginResponse();
            IdentityRepository repository = new IdentityRepository();
            SecurityRequest securityRequest = new SecurityRequest();
            securityRequest.SIFRE = request.SIFRE;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigReader.GetAppSettingsValue("EncryptionData"));
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = JsonConvert.SerializeObject(securityRequest);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responsePassword = streamReader.ReadToEnd();
                var password = JsonConvert.DeserializeObject<SecurityResponse>(responsePassword);
                request.SIFRE = password.EncryptPass;
            }

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