using Domain.Entities.BookCategoryAgg;
using FluentValidation;

namespace Application.Validation.Products;

public class ProductCategoryValidator : AbstractValidator<ProductCategory>
{
    public ProductCategoryValidator()
    {
        RuleFor(category => category.Name).NotNull().NotEmpty();
        RuleForEach(category => category.Products).NotEmpty()
            .WithMessage("Products {CollectionIndex} is required."); ;
    }
}
