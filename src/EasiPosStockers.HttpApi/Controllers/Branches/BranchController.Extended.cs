using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using EasiPosStockers.Branches;

namespace EasiPosStockers.Controllers.Branches
{
    [RemoteService]
    [Area("app")]
    [ControllerName("Branch")]
    [Route("api/app/branches")]

    public class BranchController : BranchControllerBase, IBranchesAppService
    {
        public BranchController(IBranchesAppService branchesAppService) : base(branchesAppService)
        {
        }
    }
}