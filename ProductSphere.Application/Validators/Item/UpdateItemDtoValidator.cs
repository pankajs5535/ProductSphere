using FluentValidation;
using ProductSphere.Application.DTOs.ItemDtos;

namespace ProductSphere.Application.Validators.Item
{
    public class UpdateItemDtoValidator : AbstractValidator<UpdateItemDto>
    {
        public UpdateItemDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}