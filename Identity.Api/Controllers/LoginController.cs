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
            LoginResponse loginResponse = new LoginResponse();
            SecurityResponse securityResponse = new SecurityResponse();
            IdentityRepository repository = new IdentityRepository();

            SecurityRequest securityRequest = new SecurityRequest();
            securityRequest.SIFRE = request.SIFRE;

            KU_KULLANICI LoginObj = new KU_KULLANICI();
            LoginObj.E_MAIL = request.E_MAIL;
            LoginObj.SIFRE = request.SIFRE;

            #region #EncryptedPassword

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
                securityResponse = JsonConvert.DeserializeObject<SecurityResponse>(responsePassword);
            }

            #endregion

            if (securityResponse.IsSuccess) //Şifreleme başarılı ise
            {
                LoginObj.SIFRE = securityResponse.EncryptPass;
                KU_KULLANICI usernamePasswordCorrect = repository.UserLogin(LoginObj.E_MAIL, LoginObj.SIFRE);
                KU_KULLANICI onlyUsernameCorrect = repository.UserLoginWithoutPassword(LoginObj.E_MAIL);

                if (usernamePasswordCorrect != null) //Kullanıcı adı ve şifre doğru
                {
                    onlyUsernameCorrect =
                        null; //Kullanıcı adı ve şifre doğru ise sadece kullanıcı adı doğru olanı null yapıyoruz.
                    if (usernamePasswordCorrect.IS_ACTIVE == true &&
                        usernamePasswordCorrect.LOGIN_SAYISI < 3) //Hesap aktif problem yok
                    {
                        loginResponse.IsSuccess = true;
                        loginResponse.Message = "Giriş işlemi başarılı";
                        loginResponse.UserName = usernamePasswordCorrect.E_MAIL;
                        return loginResponse;
                    }
                    else if (usernamePasswordCorrect.IS_ACTIVE == false) //Hesap bloke
                    {
                        loginResponse.IsSuccess = false;
                        loginResponse.Message = "Hesabınız aktif değil";
                        loginResponse.UserName = usernamePasswordCorrect.E_MAIL;
                        return loginResponse;
                    }
                } // Doğru login girişlerinde kullanılacak

                if (onlyUsernameCorrect != null) //Kullanıcı adı doğru şifre yanlış
                {
                    if (onlyUsernameCorrect.IS_ACTIVE == false &&
                        onlyUsernameCorrect.LOGIN_SAYISI > 3) // Hesap zaten bloke, bir daha yanlış giriliyor
                    {
                        loginResponse.Message = "Bloklanmış hesaba giriş yapılamaz !";
                        loginResponse.IsSuccess = false;
                        return loginResponse;
                    }

                    if (onlyUsernameCorrect.IS_ACTIVE == true &&
                        onlyUsernameCorrect.LOGIN_SAYISI < 3) // Hesap aktif ve 3 defa yanlış giriş yapılmamış ise
                    {
                        repository.UpdateWrongUserLoginCount(onlyUsernameCorrect);
                        loginResponse.Message = "Kullanıcı adı ile şifre eşleşmiyor";
                        loginResponse.IsSuccess = false;
                        loginResponse.UserName = onlyUsernameCorrect.E_MAIL;
                        return loginResponse;
                    }

                    if (onlyUsernameCorrect.IS_ACTIVE == true &&
                        onlyUsernameCorrect.LOGIN_SAYISI == 3) //Hesabı bloke etmek için
                    {
                        repository.BlockAccount(onlyUsernameCorrect);
                        repository.UpdateWrongUserLoginCount(onlyUsernameCorrect);
                        loginResponse.Message = "Hesabınız bloke edildi";
                        loginResponse.IsSuccess = false;
                        loginResponse.UserName = onlyUsernameCorrect.E_MAIL;
                    }
                }
                else //Kullanıcı ve şifre ile eşleşen hesap bulunamadı !
                {
                    loginResponse.Message = "Hesap bulunamadı !";
                    loginResponse.IsSuccess = false;
                }
            }

            return loginResponse;
        }
    }
}