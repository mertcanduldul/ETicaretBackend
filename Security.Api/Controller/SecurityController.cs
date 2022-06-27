using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Business.ServiceModel.Security;


namespace Security.Api.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SecurityController : ControllerBase
    {
        private static readonly string SECRET_KEY = "b14ca5898a4e4133bbce2ea2315a1916"; //AES Secret Key

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
            securityResponse.EncryptPass = EncryptString(request.SIFRE, SECRET_KEY);
            securityResponse.DecryptPass = request.SIFRE;
            securityResponse.IsSuccess = true;
            securityResponse.Message = "Encryption Başarılı ";
            return securityResponse;
        }

        [Route("DecryptionData")]
        [HttpPost]
        public SecurityResponse DecryptionData(SecurityRequest request)
        {
            SecurityResponse securityResponse = new SecurityResponse();
            securityResponse.DecryptPass = DecryptString(request.SIFRE, SECRET_KEY);
            securityResponse.EncryptPass = request.SIFRE;
            securityResponse.IsSuccess = true;
            securityResponse.Message = "Decryption Başarılı ";
            return securityResponse;
        }

        public static string EncryptString(string text, string password)
        {
            CryptoBusiness cryptoBusiness = new CryptoBusiness();
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // şifre SHA256 ile hashleniyor.
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);

            byte[] baSalt = GetRandomBytes();
            byte[] baEncrypted = new byte[baSalt.Length + baText.Length];

            // Salt dizisi ve text dizisi birleştiriliyor.
            for (int i = 0; i < baSalt.Length; i++)
                baEncrypted[i] = baSalt[i];
            for (int i = 0; i < baText.Length; i++)
                baEncrypted[i + baSalt.Length] = baText[i];

            baEncrypted = cryptoBusiness.AES_Encrypt(baEncrypted, baPwdHash);

            string result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        public static string DecryptString(string text, string password)
        {
            CryptoBusiness cryptoBusiness = new CryptoBusiness();
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // şifre SHA256 ile hashleniyor.
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Convert.FromBase64String(text);

            byte[] baDecrypted = cryptoBusiness.AES_Decrypt(baText, baPwdHash);

            // Salt kaldırılıyor
            int saltLength = GetSaltLength();
            byte[] baResult = new byte[baDecrypted.Length - saltLength];
            for (int i = 0; i < baResult.Length; i++)
                baResult[i] = baDecrypted[i + saltLength];

            string result = Encoding.UTF8.GetString(baResult);
            return result;
        }

        public static byte[] GetRandomBytes()
        {
            int saltLength = GetSaltLength();
            byte[] ba = new byte[saltLength];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        public static int GetSaltLength()
        {
            return 8;
        }
    }
}