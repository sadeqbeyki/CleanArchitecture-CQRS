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
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IMapper _mapper;

        public ProductQueryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = _unitOfWork.GetRepository<Product, Guid>();
        }

        public async Task<ProductDetailsDto> GetProductById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            var mapProduct = _mapper.Map<ProductDetailsDto>(product);
            return mapProduct;
        }

        public List<ProductDetailsDto> GetProducts()
        {
            var product = _productRepository.GetAll();
            var mapProduct = _mapper.Map<List<ProductDetailsDto>>(product).ToList();
            return mapProduct;
        }

        //public void CreateProduct(Product product)
        //{
        //    // Perform business logic and repository operations using _productRepository...

        //    _unitOfWork.Commit();
        //}
    }
}
