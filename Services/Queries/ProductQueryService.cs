using Application.DTOs;
using Application.Interface.Query;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Interface;
using Domain.Interface.Queries;
using Persistance.Repositories;
using Persistance.Repositories.Query;

namespace Services.Queries
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product, Guid> _repository;
        private readonly IMapper _mapper;

        public ProductQueryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repository = _unitOfWork.GetRepository<Product, Guid>();
        }

        public async Task<ProductDetailsDto> GetProductById(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            var mapProduct = _mapper.Map<ProductDetailsDto>(product);
            return mapProduct;
        }

        public List<ProductDetailsDto> GetProducts()
        {
            var product = _repository.GetAll();
            var mapProduct = _mapper.Map<List<ProductDetailsDto>>(product).ToList();
            return mapProduct;
        }

        public List<ProductDetailsDto> GetProductsByEmail(string email)
        {
            var allProducts = _repository.GetAll();
            var products =  allProducts.Where(p => p.ManufacturerEmail == email).ToList();
            var result = _mapper.Map<List<ProductDetailsDto>>(products);
            return result;
        }
    }
}
