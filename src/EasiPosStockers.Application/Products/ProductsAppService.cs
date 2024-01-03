using EasiPosStockers.CostCentres;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using EasiPosStockers.Permissions;
using EasiPosStockers.Products;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using EasiPosStockers.Shared;

namespace EasiPosStockers.Products
{
    [RemoteService(IsEnabled = false)]
    [Authorize(EasiPosStockersPermissions.Products.Default)]
    public abstract class ProductsAppServiceBase : ApplicationService
    {
        protected IDistributedCache<ProductExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected IProductRepository _productRepository;
        protected ProductManager _productManager;
        protected IRepository<CostCentre, Guid> _costCentreRepository;

        public ProductsAppServiceBase(IProductRepository productRepository, ProductManager productManager, IDistributedCache<ProductExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<CostCentre, Guid> costCentreRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _productRepository = productRepository;
            _productManager = productManager; _costCentreRepository = costCentreRepository;
        }

        public virtual async Task<PagedResultDto<ProductWithNavigationPropertiesDto>> GetListAsync(GetProductsInput input)
        {
            Console.WriteLine("We made it to GetListAsync in ProductsAppService\n\n");
            var totalCount = await _productRepository.GetCountAsync(input.FilterText, input.Description, input.ProductName, input.CostCentreId);
            var items = await _productRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Description, input.ProductName, input.CostCentreId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<ProductWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<ProductWithNavigationProperties>, List<ProductWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<ProductWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<ProductWithNavigationProperties, ProductWithNavigationPropertiesDto>
                (await _productRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<ProductDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Product, ProductDto>(await _productRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetCostCentreLookupAsync(LookupRequestDto input)
        {
            var query = (await _costCentreRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.CostCentreName != null &&
                         x.CostCentreName.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<CostCentre>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<CostCentre>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(EasiPosStockersPermissions.Products.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _productRepository.DeleteAsync(id);
        }

        [Authorize(EasiPosStockersPermissions.Products.Create)]
        public virtual async Task<ProductDto> CreateAsync(ProductCreateDto input)
        {
            var product = await _productManager.CreateAsync(input.CostCentreIds, input.Description, input.ProductName);
            return ObjectMapper.Map<Product, ProductDto>(product);
        }


        [Authorize(EasiPosStockersPermissions.Products.Edit)]
        public virtual async Task<ProductDto> UpdateAsync(Guid id, ProductUpdateDto input)
        {
            try
            {
                var product = await _productManager.UpdateAsync(id, input.CostCentreIds, input.Description, input.ProductName, input.ConcurrencyStamp);
                return ObjectMapper.Map<Product, ProductDto>(product); // This is the statement that leads to the db being populated!
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nException Caught!\n");
                Console.WriteLine(ex);
                Console.WriteLine("\n\n");
                throw;
            }
        }


        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProductExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var products = await _productRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Description, input.ProductName);
            var items = products.Select(item => new
            {
                Description = item.Product.Description,
                ProductName = item.Product.ProductName,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Products.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new ProductExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}