using Hays.Application.Services.Abstracts;
using Hays.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Hays.Application.Seeds;

namespace Hays.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IIncomesService, IncomesService>();
            services.AddScoped<IIncomeDefinitionsService, IncomeDefinitionsService>();
            services.AddScoped<IExpensesService, ExpensesService>();
            services.AddScoped<IExpenseDefinitionsService, ExpenseDefinitionsService>();
            services.AddScoped<IBudgetsService, BudgetsService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<ISpendingGoalsService, SpendingGoalsService>();

            // Seeds
            services.AddScoped<UsersSeed>();
            services.AddScoped<BudgetsSeed>();
            services.AddScoped<ExpenseDefinitionsSeed>();
            services.AddScoped<IncomeDefinitionsSeed>();
            services.AddScoped<ExpensesSeed>();
            services.AddScoped<IncomesSeed>();

            // MediatR
            services.AddMediatR(x => x.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

            // AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
