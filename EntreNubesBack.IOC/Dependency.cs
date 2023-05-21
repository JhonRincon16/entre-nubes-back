using System.Text;
using EntreNubesBack.BLL.Services;
using EntreNubesBack.BLL.Services.Contracts;
using EntreNubesBack.DAL.DBContext;
using EntreNubesBack.DAL.Repositories;
using EntreNubesBack.DAL.Repositories.Contracts;
using EntreNubesBack.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace EntreNubesBack.IOC;

public static class Dependency
{
    public static void InjectDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<EntrenubesContext>(options =>
        {
            options.UseMySQL(configuration.GetConnectionString("DataBaseConnection"));
        });

        #region Repositories
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IThirdPartyRepository, ThirdPartyRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IPurchaseRepository, PurchaseRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped<ICashClosingRepository, CashClosingRepository>();
        #endregion
        
        #region Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRolService, RolService>();
        services.AddScoped<IThirdPartyService, ThirdPartyService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IExpenseService, ExpenseService>();
        services.AddScoped<IPurchaseService, PurchaseService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ISaleService, SaleService>();
        services.AddScoped<ICashClosingService, CashClosingService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IDashBoardService, DashBoardService>();
        #endregion
      
        services.AddAutoMapper(typeof(AutoMapperProfile));
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("JwtKey").Value)),
        };
        //services.AddSingleton(jwtParameters);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = jwtParameters;
        });
    }
}