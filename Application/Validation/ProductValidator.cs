using Application.DTOs;
using Domain.Entities.Products;
using FluentValidation;


namespace Application.Validation
{
    public class ProductValidator : AbstractValidator<AddProductDto>
    {
        public ProductValidator()
        {
            //RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Name).Length(0, 10);
            RuleFor(x => x.ManufacturerEmail).EmailAddress();
            //RuleFor(x => x.Age).InclusiveBetween(18, 60);
        }
    }
}
