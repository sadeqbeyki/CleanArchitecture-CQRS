using Application.DTOs;
using Application.Interface.Query;
using Domain.Repositories.Queries;

namespace Services.Queries
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IProductQueryRepository _repository;

        public ProductQueryService(IProductQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductDetailsDto> GetById(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            ProductDetailsDto productDetails = new()
            {
                Id = product.Id,
                Name = product.Name,
                ProduceDate = DateTime.Now,
                ManufacturerPhone = product.ManufacturerPhone,
                ManufacturerEmail = product.ManufacturerEmail,
            };
            return productDetails;
        }
    }
}
