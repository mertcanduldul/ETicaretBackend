using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceModel.Identity
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email zorunlu alandır !")]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)$", ErrorMessage = ("E_MAIL dogru formatta olmalıdır !"))]
        public string E_MAIL { get; set; }

        [MinLength(8, ErrorMessage = "Şifre en az 8 karakter olmalıdır !")]
        [MaxLength(50, ErrorMessage = "Şifre en fazla 20 karakter olmalıdır !")]
        [Required(ErrorMessage = "Sifre zorunlu alandır !")]
        public string SIFRE { get; set; }
    }
}

