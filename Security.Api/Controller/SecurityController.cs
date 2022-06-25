using Microsoft.AspNetCore.Mvc;
using System.Text;
using Business.ServiceModel.Security;
using System.Security.Cryptography;

namespace Security.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private static readonly string SECRET_KEY = "b14ca5898a4e4133bbce2ea2315a1916";

        private readonly ILogger<SecurityController> _logger;

        public SecurityController(ILogger<SecurityController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Successfully reached - ETicaretBackend.Security.Api - SecurityController";
        }

        [Route("EncryptionData")]
        [HttpPost]
        public SecurityResponse EncryptionData(SecurityRequest request)
        {
            SecurityResponse securityResponse = new SecurityResponse();
            byte[] iv = new byte[16];
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(SECRET_KEY);
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream =
                           new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(request.Key);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            securityResponse.EncryptPass = Convert.ToBase64String(array);
            securityResponse.IsSuccess = true;
            securityResponse.Message = "Encryption Başarılı ";
            return securityResponse;
        }


        [Route("DecryptionData")]
        [HttpPost]
        public SecurityResponse DecryptionData(SecurityRequest request)
        {
            SecurityResponse securityResponse = new SecurityResponse();

            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(request.Key);
                using(Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(SECRET_KEY);
                    aes.IV = iv;
                    ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream =
                               new CryptoStream((Stream)memoryStream, decrypt, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                securityResponse.DecryptPass = streamReader.ReadToEnd();
                                securityResponse.IsSuccess = true;
                                securityResponse.Message = "Decryption Basarılı ";
                                return securityResponse;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                securityResponse.Message = ex.Message + "-" + request.Key;
                securityResponse.IsSuccess = false;
                securityResponse.Message = "Decryption Başarısız !";
                return securityResponse;
            }
        }
    }
}