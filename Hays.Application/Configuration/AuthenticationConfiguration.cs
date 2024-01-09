namespace Hays.Application.Configuration
{
    public class AuthenticationConfiguration
    {
        public string JwtKey { get; set; } = default!;
        public TimeSpan JwtLifeTime { get; set; }
        public TimeSpan RefreshTokenLifeTime { get; set; }
        public string JwtIssuer { get; set; } = default!;
        public string JwtAudience { get; set; } = default!;
        public int SaltSize { get; set; }
        public string AdminLogin { get; set; } = default!;
        public string AdminPassword { get; set; } = default!;
        public string AdminEmail { get; set; } = default!;
        public string AdminRoleName { get; set; } = default!;
    }
}
