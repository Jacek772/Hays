using Hays.Application.Functions.Commands;
using Hays.Domain.Entities;

namespace Hays.Application.Services.Abstracts
{
    public interface IUsersService
    {
        Task CreateUserAsync(CreateUserCommand createUserCommand);
        Task CreateUserAsync(User user);
        Task<bool> ExistsUserAsync(string email);
        Task<List<User>> GetUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> GetUserAsync(string email);
        Task UpdateUserAsync(UpdateUserCommand updateUserCommand);
        Task DeleteUserAsync(int userId);
    }
}
