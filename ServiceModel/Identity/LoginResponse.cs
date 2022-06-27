namespace Business.ServiceModel.Identity
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}