﻿using Application.DTOs;
using Application.Exceptions;
using Application.Interface.Query;
using AutoMapper;
using Domain.Entities.Products;
using Domain.Interface;
using Domain.Interface.Queries;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Services.Queries
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Product, Guid> _repository;
        private readonly IProductQueryRepository _productRepository;
        private readonly IMemoryCache _memoreCache;


        public ProductQueryService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IProductQueryRepository productRepository,
            IMemoryCache memoreCache)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _repository = _unitOfWork.GetRepository<Product, Guid>();
            _memoreCache = memoreCache;
        }

        //public async Task<List<ProductDetailsDto>> GetProducts()
        //{
        //    var products = await _repository.GetAll();
        //    var mapProducts = _mapper.Map<List<ProductDetailsDto>>(products).ToList();
        //    return mapProducts;
        //}

        public async Task<List<ProductDetailsDto>> GetProducts()
        {
            var products = await _repository.GetAll();
            var mapProducts = _mapper.Map<List<ProductDetailsDto>>(products).ToList();
            var cacheKey = "productList";
            if (!_memoreCache.TryGetValue(cacheKey, out List<ProductDetailsDto> productList))
            {
                productList = mapProducts.ToList();
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };
                _memoreCache.Set(cacheKey, productList, cacheExpiryOptions);
            }
            return productList;
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
