using FluentValidation;
using ProductSphere.Application.DTOs.ItemDtos;

namespace ProductSphere.Application.Validators.Item
{
    public class CreateItemDtoValidator : AbstractValidator<CreateItemDto>
    {
        public CreateItemDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0);

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}