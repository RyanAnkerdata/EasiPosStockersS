using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using EasiPosStockers.CostCentres;

namespace EasiPosStockers.Controllers.CostCentres
{
    [RemoteService]
    [Area("app")]
    [ControllerName("CostCentre")]
    [Route("api/app/cost-centres")]

    public class CostCentreController : CostCentreControllerBase, ICostCentresAppService
    {
        public CostCentreController(ICostCentresAppService costCentresAppService) : base(costCentresAppService)
        {
        }
    }
}