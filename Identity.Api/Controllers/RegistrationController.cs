using System.Net;
using Business.ServiceModel.Identity;
using Business.ServiceModel.Security;
using Dapper.Repository.Base;
using Data.Dapper.Repository.Identity;
using Data.Entity.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.IO;
using System.Text;

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
        public RegistrationResponse Registration(RegistrationRequest request)
        {
            try
            {
                RegistrationResponse response = new RegistrationResponse();
                IdentityRepository identityRepository = new IdentityRepository();
                KU_KULLANICI existUser = identityRepository.HaveAccountWithThatEmail(request.E_MAIL);

                if (existUser != null)
                {
                    response.IsSuccess = false;
                    response.Message = "E-Mail Adresi kullanılıyor. Üyelik Başarısız !";
                    return response;
                }

                SecurityRequest securityRequest = new SecurityRequest();
                securityRequest.SIFRE = request.SIFRE;

                var httpWebRequest =
                    (HttpWebRequest)WebRequest.Create(ConfigReader.GetAppSettingsValue("EncryptionData"));
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