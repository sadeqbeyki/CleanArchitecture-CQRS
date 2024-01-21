using Application.DTOs.ProductCategories;
using MediatR;

namespace Application.Features.ProductCategories.Query;

public record GetAllProductCategoryQuery : IRequest<IEnumerable<ProductCategoryDto>>;

