using System;
using System.ComponentModel.DataAnnotations;

namespace Business.ServiceModel.Identity
{
    public class RegistrationRequest
    {
        [Required(ErrorMessage = "E_MAIL alanı dolu olmalıdır !")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)$", ErrorMessage = "E_MAIL doğru formatta olmalıdır !")]
        public string E_MAIL { get; set; }

        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır !")]
        [MaxLength(20, ErrorMessage = "Şifre en fazla 20 karakter olmalıdır !")]
        [Required(ErrorMessage = "Şifre alanı dolu olmalıdır !")]
        public string SIFRE { get; set; }
    }
}

