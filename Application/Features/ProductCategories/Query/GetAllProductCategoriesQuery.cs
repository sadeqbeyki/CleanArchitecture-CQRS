using Application.DTOs.ProductCategories;
using MediatR;

namespace Application.Features.ProductCategories.Query;

public record GetAllProductCategoriesQuery : IRequest<IEnumerable<ProductCategoryDto>>;

