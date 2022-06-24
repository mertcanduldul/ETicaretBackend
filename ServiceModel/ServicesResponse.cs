using System;
namespace ServiceModel
{
    [Serializable]
    public class ServicesResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public ServicesResponse()
        {
            IsSuccess = true;
            Message = "";
        }

        public ServicesResponse(ServicesResponse response)
        {
            IsSuccess = response.IsSuccess;
            Message = response.Message;
        }
    }
}

