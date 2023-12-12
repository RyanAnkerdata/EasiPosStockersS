using Volo.Abp.Application.Dtos;
using System;

namespace EasiPosStockers.CostCentres
{
    public abstract class GetCostCentresInputBase : PagedAndSortedResultRequestDto
    {

        public string? FilterText { get; set; }

        public string? CostCentreReference { get; set; }
        public string? CostCentreName { get; set; }
        public bool? IsDisabled { get; set; }
        public Guid? BranchId { get; set; }

        public GetCostCentresInputBase()
        {

        }
    }
}