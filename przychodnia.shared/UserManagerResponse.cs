namespace Przychodnia.Shared
{
    public class UserManagerResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();
        public string? AccessToken { get; set; } = string.Empty;
        public DateTime? ExpireDate { get; set; }
        public string? User { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
