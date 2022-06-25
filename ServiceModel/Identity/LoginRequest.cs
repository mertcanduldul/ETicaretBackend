using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceModel.Identity
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email zorunlu alandır !")]
        public string E_MAIL { get; set; }

        [Required(ErrorMessage = "Sifre zorunlu alandır !")]
        public string SIFRE { get; set; }
    }
}

