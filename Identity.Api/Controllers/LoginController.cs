using System.Net;
using Business.ServiceModel.Identity;
using Business.ServiceModel.Security;
using Dapper.Repository.Base;
using Data.Dapper.Repository.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceModel.Identity;
using Data.Entity.Identity;
using Data.Entity.Token;
using Newtonsoft.Json;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        readonly IConfiguration _configuration;

        public LoginController(ILogger<LoginController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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

            KU_KULLANICI FrontEndLoginObj = new KU_KULLANICI();
            FrontEndLoginObj.E_MAIL = request.E_MAIL;
            FrontEndLoginObj.SIFRE = request.SIFRE;

            KU_KULLANICI BackEndLoginObj = new KU_KULLANICI();
            BackEndLoginObj.E_MAIL = request.E_MAIL;

            KU_KULLANICI whichAccount = repository.UserLoginWithoutPassword(FrontEndLoginObj.E_MAIL);
            if (whichAccount == null)
            {
                loginResponse.Message = "Kullanıcı bulunamadı.";
                loginResponse.IsSuccess = false;
                return loginResponse;
            }

            #region #DecryptPassword from db

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(ConfigReader.GetAppSettingsValue("DecryptionData"));
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                SecurityRequest securityRequest = new SecurityRequest();
                securityRequest.SIFRE = whichAccount.SIFRE;
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

            if (securityResponse.IsSuccess) //Şifre Çözme başarılı ise
            {
                BackEndLoginObj.SIFRE = securityResponse.DecryptPass;

                KU_KULLANICI usernamePasswordCorrect = null;
                KU_KULLANICI onlyUsernameCorrect = null;

                if (FrontEndLoginObj.SIFRE == BackEndLoginObj.SIFRE)
                    usernamePasswordCorrect = repository.UserLoginWithoutPassword(FrontEndLoginObj.E_MAIL);
                else
                    onlyUsernameCorrect = repository.UserLoginWithoutPassword(FrontEndLoginObj.E_MAIL);


                if (usernamePasswordCorrect != null) //Kullanıcı adı ve şifre doğru
                {
                    if (usernamePasswordCorrect.IS_ACTIVE == true &&
                        usernamePasswordCorrect.LOGIN_SAYISI < 3) //Hesap aktif problem yok
                    {
                        //Token Burada üretildiği için kullanıcıya gönderiyoruz.
                        TokenHandler tokenHandler = new TokenHandler(_configuration);
                        Token token = tokenHandler.CreateAccessToken(usernamePasswordCorrect);
                        loginResponse.IsSuccess = true;
                        loginResponse.Message = "Giriş işlemi başarılı !";
                        loginResponse.UserName = usernamePasswordCorrect.AD_SOYAD;
                        loginResponse.Token = token.AccessToken;
                        loginResponse.UserId = usernamePasswordCorrect.ID_KULLANICI;
                        loginResponse.UserMail = usernamePasswordCorrect.E_MAIL;
                        return loginResponse;
                    }
                    else if (usernamePasswordCorrect.IS_ACTIVE == false) //Hesap bloke
                    {
                        loginResponse.IsSuccess = false;
                        loginResponse.Message = "Hesabınız aktif değil !";
                        loginResponse.UserName = usernamePasswordCorrect.AD_SOYAD;
                        loginResponse.UserId = usernamePasswordCorrect.ID_KULLANICI;
                        loginResponse.UserMail = usernamePasswordCorrect.E_MAIL;
                        return loginResponse;
                    }
                }

                if (onlyUsernameCorrect != null) //Kullanıcı adı doğru şifre yanlış
                {
                    if (onlyUsernameCorrect.IS_ACTIVE == false &&
                        onlyUsernameCorrect.LOGIN_SAYISI >= 3) // Hesap zaten bloke, bir daha yanlış giriliyor
                    {
                        loginResponse.Message = "Kullanıcı adı ile şifre eşleşmiyor !";
                        loginResponse.IsSuccess = false;
                        loginResponse.UserId = onlyUsernameCorrect.ID_KULLANICI;
                        return loginResponse;
                    }

                    if (onlyUsernameCorrect.IS_ACTIVE == true &&
                        onlyUsernameCorrect.LOGIN_SAYISI < 3) // Hesap aktif ve 3 defa yanlış giriş yapılmamış ise
                    {
                        repository.UpdateWrongUserLoginCount(onlyUsernameCorrect);
                        loginResponse.Message = "Kullanıcı adı ile şifre eşleşmiyor !";
                        loginResponse.IsSuccess = false;
                        loginResponse.UserId = onlyUsernameCorrect.ID_KULLANICI;
                        return loginResponse;
                    }

                    if (onlyUsernameCorrect.IS_ACTIVE == true &&
                        onlyUsernameCorrect.LOGIN_SAYISI == 3) //Hesabı bloke etmek için
                    {
                        repository.BlockAccount(onlyUsernameCorrect);
                        repository.UpdateWrongUserLoginCount(onlyUsernameCorrect);
                        loginResponse.Message = "Hesabınız bloke edildi !";
                        loginResponse.IsSuccess = false;
                        loginResponse.UserId = onlyUsernameCorrect.ID_KULLANICI;
                        return loginResponse;
                    }
                }
                else //Kullanıcı ve şifre ile eşleşen hesap bulunamadı !
                {
                    loginResponse.Message = "Hesap bulunamadı !";
                    loginResponse.IsSuccess = false;
                    return loginResponse;
                }
            }

            return loginResponse;
        }
    }
}