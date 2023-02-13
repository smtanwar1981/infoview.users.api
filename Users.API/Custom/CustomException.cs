namespace Users.API.Custom
{
    public class CustomException
    {
        public string ErrorMessage { get; set; } = string.Empty;
        public int ErrorCode { get; set; }
    }
}
