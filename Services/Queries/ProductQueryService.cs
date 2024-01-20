using Application.DTOs;
using Application.Exceptions;
using Application.Helper;
using Application.Interface.Query;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Interface;
using Domain.Interface.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Services.Queries
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product, Guid> _repository;
        private readonly IProductQueryRepository _productRepository;

        //private readonly IMemoryCache _memoreCache;
        private readonly IDistributedCache _distributedCache;
        private readonly IConfiguration _configuration;


        public ProductQueryService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IProductQueryRepository productRepository,

            IDistributedCache distributedCache,
            IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _repository = _unitOfWork.GetRepository<Product, Guid>();

            _distributedCache = distributedCache;
            _configuration = configuration;
        }

        //public async Task<List<ProductDetailsDto>> GetProducts()
        //{
        //    var products = await _repository.GetAll();
        //    var mapProducts = _mapper.Map<List<ProductDetailsDto>>(products).ToList();
        //    return mapProducts;
        //}

        //public async Task<List<ProductDetailsDto>> GetProducts()
        //{
        //    var products = await _repository.GetAll();
        //    var mapProducts = _mapper.Map<List<ProductDetailsDto>>(products).ToList();
        //    var cacheKey = "productList";
        //    if (!_memoreCache.TryGetValue(cacheKey, out List<ProductDetailsDto> productList))
        //    {
        //        productList = mapProducts.ToList();
        //        var cacheExpiryOptions = new MemoryCacheEntryOptions
        //        {
        //            AbsoluteExpiration = DateTime.Now.AddSeconds(50),
        //            Priority = CacheItemPriority.High,
        //            SlidingExpiration = TimeSpan.FromSeconds(20)
        //        };
        //        _memoreCache.Set(cacheKey, productList, cacheExpiryOptions);
        //    }
        //    return productList;
        //}

        public async Task<List<ProductDetailsDto>> GetProducts()
        {
            var products = await _repository.GetAll();
            var mapProducts = _mapper.Map<List<ProductDetailsDto>>(products).ToList();

            var cacheKey = "GetAllProducts";
            var data = await _distributedCache.GetRecordAsync<List<ProductDetailsDto>>(cacheKey);

            if (data is null)
            {
                data = mapProducts.ToList();
                await _distributedCache.SetRecordAsync(cacheKey, data, _configuration);
            }
            return data;
            //docker run -p 6379:6379 --name -redis -d redis
        }

        public async Task<ProductDetailsDto> GetProductById(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            var mapProduct = _mapper.Map<ProductDetailsDto>(product);

            string cacheKey = $"product-{id}";
            var data = await _distributedCache.GetRecordAsync<ProductDetailsDto>(cacheKey);
            if (data is null)
            {
                data = mapProduct;
                await _distributedCache.SetRecordAsync(cacheKey, data, _configuration);
            }
            return data;
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
