namespace IOMate.Application.UseCases.Authentication
{
    public sealed record AuthenticationResponseDto
    {
        public string Token { get; set; } = string.Empty;
    }
}
