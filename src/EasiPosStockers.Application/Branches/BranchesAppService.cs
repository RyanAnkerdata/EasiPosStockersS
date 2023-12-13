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
using EasiPosStockers.Branches;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using EasiPosStockers.Shared;
using AutoMapper;
using Polly;

namespace EasiPosStockers.Branches
{
    [RemoteService(IsEnabled = false)]
    [Authorize(EasiPosStockersPermissions.Branches.Default)]

    public abstract class BranchesAppServiceBase : ApplicationService
    {
        protected IDistributedCache<BranchExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        protected IBranchRepository _branchRepository;
        protected BranchManager _branchManager;

        public BranchesAppServiceBase(IBranchRepository branchRepository, BranchManager branchManager, IDistributedCache<BranchExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _branchRepository = branchRepository;
            _branchManager = branchManager;
        }

        public virtual async Task<PagedResultDto<BranchDto>> GetListAsync(GetBranchesInput input)
        {
            var totalCount = await _branchRepository.GetCountAsync(input.FilterText, input.BranchReference, input.BranchName, input.TaxRegistrationNumber);
            var items = await _branchRepository.GetListAsync(input.FilterText, input.BranchReference, input.BranchName, input.TaxRegistrationNumber, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<BranchDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Branch>, List<BranchDto>>(items)
            };
        }

        public virtual async Task<BranchDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Branch, BranchDto>(await _branchRepository.GetAsync(id));
        }

        [Authorize(EasiPosStockersPermissions.Branches.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _branchRepository.DeleteAsync(id);
        }

        [Authorize(EasiPosStockersPermissions.Branches.Create)]
        public virtual async Task<BranchDto> CreateAsync(BranchCreateDto input)
        {

            var branch = await _branchManager.CreateAsync(
            input.BranchReference, input.BranchName, input.TaxRegistrationNumber
            );

            return ObjectMapper.Map<Branch, BranchDto>(branch);
        }

        [Authorize(EasiPosStockersPermissions.Branches.Edit)]
        public virtual async Task<BranchDto> UpdateAsync(Guid id, BranchUpdateDto input)
        {

            var branch = await _branchManager.UpdateAsync(
            id,
            input.BranchReference, input.BranchName, input.TaxRegistrationNumber, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Branch, BranchDto>(branch);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(BranchExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _branchRepository.GetListAsync(input.FilterText, input.BranchReference, input.BranchName, input.TaxRegistrationNumber);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Branch>, List<BranchExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Branches.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public virtual async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new BranchExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }


        public async Task<Branch> GetBranchByIdAsync(Guid id)
        {

           Console.WriteLine("----------this is calling the task like it should ");
            var branch = await _branchRepository.GetAsync(id);
            Console.WriteLine("----------this is calling the task like it should " + branch);
            return branch;




            //return await _branchRepository.GetAsync(id)
            //   .Include(q => q.Author)
            //   .ProjectTo<BranchDto>(mapper.ConfigurationProvider)
            //   .FirstOrDefaultAsync(q => q.Id == id);


        }




    }
}