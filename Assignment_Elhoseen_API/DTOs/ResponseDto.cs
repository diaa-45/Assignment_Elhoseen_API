namespace Assignment_Elhoseen_API.DTOs
{
    public class ResponseDto
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime Expiration { get; set; }
    }
}
