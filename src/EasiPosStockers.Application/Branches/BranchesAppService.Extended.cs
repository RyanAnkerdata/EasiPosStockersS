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
using EasiPosStockers.Branches;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using EasiPosStockers.Shared;

namespace EasiPosStockers.Branches
{
    public class BranchesAppService : BranchesAppServiceBase, IBranchesAppService
    {
        //<suite-custom-code-autogenerated>
        public BranchesAppService(IBranchRepository branchRepository, BranchManager branchManager, IDistributedCache<BranchExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
            : base(branchRepository, branchManager, excelDownloadTokenCache)
        {
        }
        //</suite-custom-code-autogenerated>

        //Write your custom code...
    }
}