using FluentValidation;
using ProductSphere.Application.DTOs.ProductDtos;

namespace ProductSphere.Application.Validators.Product
{
    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.CreatedBy)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}