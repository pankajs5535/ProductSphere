using FluentValidation;
using ProductSphere.Application.DTOs.ProductDtos;

namespace ProductSphere.Application.Validators.Product
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.ModifiedBy)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.ModifiedBy));
        }
    }
}