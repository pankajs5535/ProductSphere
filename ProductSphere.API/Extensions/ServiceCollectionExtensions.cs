
//Auto-Mappper
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProductSphere.API.Filters;
using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Application.Interfaces.IServices;
using ProductSphere.Application.Mapping;
using ProductSphere.Application.Validators.Product;
using ProductSphere.Infrastructure.Data;
using ProductSphere.Infrastructure.Data.Repositories;
using ProductSphere.Infrastructure.Identity;
using ProductSphere.Infrastructure.Services;


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


// API_Versioning   
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;


namespace ProductSphere.API.Extensions
{

    //DI
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repo
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<IItemRepository, ItemRepository>();

            // Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IItemService, ItemService>();

            //Validation
            services.AddScoped<ValidationFilter>();
            services.AddValidatorsFromAssemblyContaining<CreateProductDtoValidator>();


            services.AddFluentValidationAutoValidation();

            //Auto-Mapper
            services.AddAutoMapper(typeof(ProductProfile).Assembly);

            // JWT_Auth - Start

            services.Configure<JwtSettings>(
                configuration.GetSection("JwtSettings"));

            var jwtSettings = configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>();

            if (jwtSettings == null)
                throw new InvalidOperationException("JWT configuration is missing.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),

                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IAuthService, AuthService>();

            // JWT_Auth - End


            // API_Versioning - Start

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

         

            // API_Versioning - End

            // registrations
            return services;
        }
    }
}
