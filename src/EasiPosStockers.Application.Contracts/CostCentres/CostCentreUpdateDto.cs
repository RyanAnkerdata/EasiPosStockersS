using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentreUpdateDtoBase : IHasConcurrencyStamp
    {
        [Required(AllowEmptyStrings = true)]
        [StringLength(CostCentreConsts.CostCentreReferenceMaxLength)]
        public string CostCentreReference { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(CostCentreConsts.CostCentreNameMaxLength)]
        public string CostCentreName { get; set; } = null!;
        public bool IsDisabled { get; set; }
        public Guid? BranchId { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}