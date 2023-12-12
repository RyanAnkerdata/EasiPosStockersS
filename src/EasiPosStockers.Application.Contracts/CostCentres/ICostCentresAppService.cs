using EasiPosStockers.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using EasiPosStockers.Shared;

namespace EasiPosStockers.CostCentres
{
    public partial interface ICostCentresAppService : IApplicationService
    {

        Task<PagedResultDto<CostCentreWithNavigationPropertiesDto>> GetListAsync(GetCostCentresInput input);

        Task<CostCentreWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<CostCentreDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetBranchLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<CostCentreDto> CreateAsync(CostCentreCreateDto input);

        Task<CostCentreDto> UpdateAsync(Guid id, CostCentreUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(CostCentreExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}