namespace Przychodnia.Shared
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string? AccessToken { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string? User { get; set; }
        public string Role { get; set; }
    }
}
