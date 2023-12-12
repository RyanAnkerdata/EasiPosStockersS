using EasiPosStockers.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using EasiPosStockers.CostCentres;
using Volo.Abp.Content;
using EasiPosStockers.Shared;

namespace EasiPosStockers.Controllers.CostCentres
{
    [RemoteService]
    [Area("app")]
    [ControllerName("CostCentre")]
    [Route("api/app/cost-centres")]

    public abstract class CostCentreControllerBase : AbpController
    {
        protected ICostCentresAppService _costCentresAppService;

        public CostCentreControllerBase(ICostCentresAppService costCentresAppService)
        {
            _costCentresAppService = costCentresAppService;
        }

        [HttpGet]
        public virtual Task<PagedResultDto<CostCentreWithNavigationPropertiesDto>> GetListAsync(GetCostCentresInput input)
        {
            return _costCentresAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public virtual Task<CostCentreWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return _costCentresAppService.GetWithNavigationPropertiesAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<CostCentreDto> GetAsync(Guid id)
        {
            return _costCentresAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("branch-lookup")]
        public virtual Task<PagedResultDto<LookupDto<Guid>>> GetBranchLookupAsync(LookupRequestDto input)
        {
            return _costCentresAppService.GetBranchLookupAsync(input);
        }

        [HttpPost]
        public virtual Task<CostCentreDto> CreateAsync(CostCentreCreateDto input)
        {
            return _costCentresAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<CostCentreDto> UpdateAsync(Guid id, CostCentreUpdateDto input)
        {
            return _costCentresAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _costCentresAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(CostCentreExcelDownloadDto input)
        {
            return _costCentresAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public virtual Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _costCentresAppService.GetDownloadTokenAsync();
        }
    }
}