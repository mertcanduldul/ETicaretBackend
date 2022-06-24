using System;
namespace Business.ServiceModel.Security
{
    public class SecurityResponse
    {
        public string DecryptPass { get; set; }
        public string EncryptPass { get; set; }
        public string Message { get; set; }
        public bool IsSuccess{ get; set; }
    }
}

