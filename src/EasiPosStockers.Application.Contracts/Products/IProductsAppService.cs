using EasiPosStockers.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using EasiPosStockers.Shared;

namespace EasiPosStockers.Products
{
    public partial interface IProductsAppService : IApplicationService
    {

        Task<PagedResultDto<ProductWithNavigationPropertiesDto>> GetListAsync(GetProductsInput input);

        Task<ProductWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<ProductDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetCostCentreLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<ProductDto> CreateAsync(ProductCreateDto input);

        Task<ProductDto> UpdateAsync(Guid id, ProductUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(ProductExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}