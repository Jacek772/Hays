using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IPasswordHasherService
    {
        bool ComparePasswordHash(string password, User user);
        bool ComparePasswordHash(string password, string hash, byte[] salt);
        string HashPassword(string password, byte[] salt);
        byte[] GenerateRandomSalt(int bytes);
    }
}
