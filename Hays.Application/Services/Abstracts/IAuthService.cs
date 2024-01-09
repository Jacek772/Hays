using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IAuthService
    {
        string GenerateTokenAsync(User user, string password);
    }
}
