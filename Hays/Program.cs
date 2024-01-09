using Hays.Application;
using Hays.Application.Configuration;
using Hays.Application.Seeds;
using Hays.Application.Seeds.abstracts;
using Hays.Infrastructure;
using Hays.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

// Configuration
DatabaseConfiguration databaseConfiguration = new();
builder.Configuration.GetSection(nameof(DatabaseConfiguration)).Bind(databaseConfiguration);
builder.Services.AddSingleton(databaseConfiguration);

AuthenticationConfiguration authenticationConfiguration = new();
builder.Configuration.GetSection(nameof(AuthenticationConfiguration)).Bind(authenticationConfiguration);
builder.Services.AddSingleton(authenticationConfiguration);

// Authentication
TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = authenticationConfiguration.JwtIssuer,
    ValidAudience = authenticationConfiguration.JwtAudience,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.JwtKey)),
    ClockSkew = TimeSpan.Zero
};

builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(cfg =>
{
    cfg.RequireHttpsMetadata = false;
    cfg.SaveToken = true;
    cfg.TokenValidationParameters = tokenValidationParameters;
    cfg.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers.Add("Token-Expired", "true");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddApplication();
builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();

if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hays v1");
    });
    app.UseDeveloperExceptionPage();
}

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
    if (!dbContext.Database.CanConnect())
    {
        throw new Exception("Connection string is incorrect or database not exists!");
    }

    if (builder.Environment.IsDevelopment())
    {
        // Seeds
        ISeed usersSeed = scope.ServiceProvider.GetService<UsersSeed>();
        await usersSeed.Seed();

        ISeed budgetsSeed = scope.ServiceProvider.GetService<BudgetsSeed>();
        await budgetsSeed.Seed();

        ISeed expenseDefinitionsSeed = scope.ServiceProvider.GetService<ExpenseDefinitionsSeed>();
        await expenseDefinitionsSeed.Seed();

        ISeed incomeDefinitionsSeed = scope.ServiceProvider.GetService<IncomeDefinitionsSeed>();
        await incomeDefinitionsSeed.Seed();

        ISeed expensesSeed = scope.ServiceProvider.GetService<ExpensesSeed>();
        await expensesSeed.Seed();

        ISeed incomesSeed = scope.ServiceProvider.GetService<IncomesSeed>();
        await incomesSeed.Seed();
    }
}

app.Run();
