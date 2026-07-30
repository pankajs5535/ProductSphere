
//Auto-Mappper
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ProductSphere.API.Filters;
using ProductSphere.Application.Interfaces.IRepositories;
using ProductSphere.Application.Interfaces.IServices;
using ProductSphere.Application.Mapping;
using ProductSphere.Infrastructure.Data;
using ProductSphere.Infrastructure.Data.Repositories;
using ProductSphere.Infrastructure.Services;

using ProductSphere.Application.Validators.Product;

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

            // registrations
            return services;
        }
    }
}
