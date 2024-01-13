using Hays.Application.Configuration;
using Hays.Application.Exceptions;
using Hays.Application.Functions.Commands;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using Hays.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hays.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly ApplicationDbContext _applicationDbContext;

        public UsersService(AuthenticationConfiguration authenticationConfiguration,
            ApplicationDbContext applicationDbContext,
            IPasswordHasherService passwordHasherService)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _applicationDbContext = applicationDbContext;
            _passwordHasherService = passwordHasherService;
        }

        public async Task<User> CreateUserAsync(CreateUserCommand createUserCommand)
        {
            User user;
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    byte[] salt = _passwordHasherService.GenerateRandomSalt(_authenticationConfiguration.SaltSize);

                    user = new User
                    {
                        Email = createUserCommand.Email,
                        Password = _passwordHasherService.HashPassword(createUserCommand.Password, salt),
                        Salt = salt,
                        Name = createUserCommand.Name,
                        Surname = createUserCommand.Surname,
                    };
                    _applicationDbContext.Users.Add(user);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("User create error");
                }
            }
            return user;
        }

        public async Task CreateUserAsync(User user)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    user.Salt = _passwordHasherService.GenerateRandomSalt(_authenticationConfiguration.SaltSize);
                    user.Password = _passwordHasherService.HashPassword(user.Password, user.Salt);

                    _applicationDbContext.Users.Add(user);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw ex;
                }
            }
        }

        public async Task<bool> ExistsUserAsync(string email)
        {
            User user = await GetUserAsync(email);
            return user is not null;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            return await _applicationDbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserAsync(int id)
        {
            return await _applicationDbContext.Users
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetUserAsync(string email)
        {
            return await _applicationDbContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task UpdateUserAsync(UpdateUserCommand updateUserCommand)
        {
            User user = await _applicationDbContext.Users.FirstOrDefaultAsync(x => x.Id == updateUserCommand.UserId);
            if (user is null)
            {
                throw new BadRequestException("User not exists");
            }

            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    if(updateUserCommand.Email is string email)
                    {
                        user.Email = email;
                    }

                    if (updateUserCommand.Name is string name)
                    {
                        user.Name = name;
                    }

                    if (updateUserCommand.Surname is string surname)
                    {
                        user.Surname = surname;
                    }

                    if (!string.IsNullOrEmpty(updateUserCommand.Password))
                    {
                        byte[] salt = _passwordHasherService.GenerateRandomSalt(_authenticationConfiguration.SaltSize);
                        string hash = _passwordHasherService.HashPassword(updateUserCommand.Password, salt);

                        user.Password = hash;
                        user.Salt = salt;
                    }

                    _applicationDbContext.Update(user);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("User update error");
                }
            }
        }

        public async Task DeleteUserAsync(int userId)
        {
            using (IDbContextTransaction transaction = _applicationDbContext.Database.BeginTransaction())
            {
                try
                {
                    User user = new User() { Id = userId };
                    _applicationDbContext.Remove(user);
                    await _applicationDbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("User delete error");
                }
            }
        }
    }
}
