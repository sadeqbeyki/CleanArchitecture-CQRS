using Application.DTOs.ProductCategories;
using Application.Helper;
using Application.Interface.Query;
using AutoMapper;
using Domain.Entities.BookCategoryAgg;
using Domain.Interface;
using Domain.Interface.Queries;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;

namespace Services.Queries;

public class ProductCategoryQueryService : IProductCategoryQueryService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<ProductCategory, int> _repository;
    private readonly IProductQueryRepository _productRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly IConfiguration _configuration;

    public ProductCategoryQueryService(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IProductQueryRepository productRepository,

    IDistributedCache distributedCache,
    IConfiguration configuration)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _productRepository = productRepository;
        _repository = _unitOfWork.GetRepository<ProductCategory, int>();

        _distributedCache = distributedCache;
        _configuration = configuration;
    }
    public async Task<List<ProductCategoryDto>> GetProductCategories()
    {
        var productCategories = await _repository.GetAll();
        var mapProductCategories = _mapper.Map<List<ProductCategoryDto>>(productCategories).ToList();

        var cacheKey = "GetAllProductCategories";
        var data = await _distributedCache.GetRecordAsync<List<ProductCategoryDto>>(cacheKey);

        if (data is null)
        {
            data = mapProductCategories.ToList();
            await _distributedCache.SetRecordAsync(cacheKey, data, _configuration);
        }
        return data;
    }
}