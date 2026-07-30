using AutoMapper;
using ProductSphere.Application.DTOs.ProductDtos;
using ProductSphere.Domain.Entities;

namespace ProductSphere.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();

            CreateMap<CreateProductDto, Product>();

            CreateMap<UpdateProductDto, Product>();
        }
    }
}