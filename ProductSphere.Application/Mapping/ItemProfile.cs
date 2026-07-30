using AutoMapper;
using ProductSphere.Application.DTOs.ItemDtos;
using ProductSphere.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProductSphere.Application.Mapping
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>();

            CreateMap<CreateItemDto, Item>();

            CreateMap<UpdateItemDto, Item>();
        }
    }
}