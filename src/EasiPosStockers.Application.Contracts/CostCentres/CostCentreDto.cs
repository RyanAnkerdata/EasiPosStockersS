using System;
using System.Collections.Generic;

using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentreDtoBase : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string CostCentreReference { get; set; } = null!;
        public string CostCentreName { get; set; } = null!;
        public bool IsDisabled { get; set; }
        public Guid? BranchId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;

    }
}