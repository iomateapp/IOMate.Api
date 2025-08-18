namespace IOMate.Application.UseCases.Authentication.Auth
{
    public sealed record AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
