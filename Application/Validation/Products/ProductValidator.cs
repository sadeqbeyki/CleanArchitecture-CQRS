using Application.DTOs;
using Domain.Entities.Products;
using FluentValidation;


namespace Application.Validation.Products
{
    public class ProductValidator : AbstractValidator<AddProductDto>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name).Length(0, 10);
            RuleFor(x => x.ManufacturerEmail).EmailAddress();
            //RuleFor(product => product.Category).SetValidator(new ProductCategoryValidator());
            //RuleFor(product => product.Category.Description).NotNull();
            //RuleFor(customer => customer.Category.Description).NotNull().When(customer => customer.Category != null);
        }
    }
}
