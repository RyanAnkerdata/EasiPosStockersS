using EasiPosStockers.Branches;
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
using EasiPosStockers.CostCentres;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using EasiPosStockers.Shared;

namespace EasiPosStockers.CostCentres
{
    [RemoteService(IsEnabled = false)]
    [Authorize(EasiPosStockersPermissions.CostCentres.Default)]
    public abstract class CostCentresAppServiceBase : ApplicationService
    {
        protected IDistributedCache<CostCentreExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected ICostCentreRepository _costCentreRepository;
        protected CostCentreManager _costCentreManager;
        protected IRepository<Branch, Guid> _branchRepository;

        public CostCentresAppServiceBase(ICostCentreRepository costCentreRepository, CostCentreManager costCentreManager, IDistributedCache<CostCentreExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<Branch, Guid> branchRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _costCentreRepository = costCentreRepository;
            _costCentreManager = costCentreManager; _branchRepository = branchRepository;
        }

        public virtual async Task<PagedResultDto<CostCentreWithNavigationPropertiesDto>> GetListAsync(GetCostCentresInput input)
        {
            var totalCount = await _costCentreRepository.GetCountAsync(input.FilterText, input.CostCentreReference, input.CostCentreName, input.IsDisabled, input.BranchId);
            var items = await _costCentreRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.CostCentreReference, input.CostCentreName, input.IsDisabled, input.BranchId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<CostCentreWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<CostCentreWithNavigationProperties>, List<CostCentreWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<CostCentreWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<CostCentreWithNavigationProperties, CostCentreWithNavigationPropertiesDto>
                (await _costCentreRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<CostCentreDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<CostCentre, CostCentreDto>(await _costCentreRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetBranchLookupAsync(LookupRequestDto input)
        {
            var query = (await _branchRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.BranchName != null &&
                         x.BranchName.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<Branch>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Branch>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(EasiPosStockersPermissions.CostCentres.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _costCentreRepository.DeleteAsync(id);
        }

        [Authorize(EasiPosStockersPermissions.CostCentres.Create)]
        public virtual async Task<CostCentreDto> CreateAsync(CostCentreCreateDto input)
        {

            var costCentre = await _costCentreManager.CreateAsync(
            input.BranchId, input.CostCentreReference, input.CostCentreName, input.IsDisabled
            );

            return ObjectMapper.Map<CostCentre, CostCentreDto>(costCentre);
        }

        [Authorize(EasiPosStockersPermissions.CostCentres.Edit)]
        public virtual async Task<CostCentreDto> UpdateAsync(Guid id, CostCentreUpdateDto input)
        {

            var costCentre = await _costCentreManager.UpdateAsync(
            id,
            input.BranchId, input.CostCentreReference, input.CostCentreName, input.IsDisabled, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<CostCentre, CostCentreDto>(costCentre);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(CostCentreExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var costCentres = await _costCentreRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.CostCentreReference, input.CostCentreName, input.IsDisabled);
            var items = costCentres.Select(item => new
            {
                CostCentreReference = item.CostCentre.CostCentreReference,
                CostCentreName = item.CostCentre.CostCentreName,
                IsDisabled = item.CostCentre.IsDisabled,

                Branch = item.Branch?.BranchName,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "CostCentres.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new CostCentreExcelDownloadTokenCacheItem { Token = token },
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