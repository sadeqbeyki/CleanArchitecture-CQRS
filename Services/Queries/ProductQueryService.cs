using Application.DTOs;
using Application.Exceptions;
using Application.Interface.Query;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Interface;
using Domain.Interface.Queries;
using MediatR;

namespace Services.Queries
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product, Guid> _repository;
        private readonly IProductQueryRepository _productRepository;

        public ProductQueryService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IProductQueryRepository productRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _repository = _unitOfWork.GetRepository<Product, Guid>();
        }

        public async Task<List<ProductDetailsDto>> GetProducts()
        {
            var products = await _repository.GetAll();
            var mapProducts = _mapper.Map<List<ProductDetailsDto>>(products).ToList();
            return mapProducts;
        }

        public async Task<ProductDetailsDto> GetProductById(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            var mapProduct = _mapper.Map<ProductDetailsDto>(product);
            return mapProduct;
        }

        public async Task<List<ProductDetailsDto>> GetProductsByEmail(string email)
        {
            var allProducts = await _repository.GetAll();
            var products = allProducts.Where(p => p.ManufacturerEmail == email).ToList();

            var result = _mapper.Map<List<ProductDetailsDto>>(products);
            return result;
        }

        public async Task<List<ProductDetailsDto>> GetProductsByEmailPhone(string mailORphone)
        {
            var products = await _productRepository.GetProductsByName(mailORphone);
            var mapProducts = _mapper.Map<List<ProductDetailsDto>>(products);
            return mapProducts;
        }
    }
}
