using System;
namespace ServiceModel.Identity
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string UserName { get; set; }

        public LoginResponse()
        {
            IsSuccess = false;
            UserName = "";
        }

    }
}

