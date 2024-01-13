using Hays.Application.Configuration;
using Hays.Application.Seeds.abstracts;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;

namespace Hays.Application.Seeds
{
    public class UsersSeed : ISeed
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IUsersService _usersService;

        public UsersSeed(AuthenticationConfiguration authenticationConfiguration,
            IUsersService usersService)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _usersService = usersService;
        }

        public async Task Seed()
        {
            await CreateDefaultAdministrator();
        }

        private async Task CreateDefaultAdministrator()
        {
            bool userExists = await _usersService.ExistsUserAsync(_authenticationConfiguration.AdminEmail);
            if (userExists)
            {
                return;
            }

            User user = new User
            {
                Email = _authenticationConfiguration.AdminEmail,
                Name = _authenticationConfiguration.AdminLogin,
                Surname = _authenticationConfiguration.AdminLogin,
                Password = _authenticationConfiguration.AdminPassword,
            };
            await _usersService.CreateUserAsync(user);
        }
    }
}
