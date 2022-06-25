using System;
namespace Business.ServiceModel.Identity
{
    public class RegistrationResponse
    {
        public string AD_SOYAD { get; set; }

        public string E_MAIL { get; set; }

        public bool IsSuccess { get; set; }

        public string Message { get; set; }
    }
}

