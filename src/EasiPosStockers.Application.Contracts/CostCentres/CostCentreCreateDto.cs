using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EasiPosStockers.CostCentres
{
    public abstract class CostCentreCreateDtoBase
    {
        [Required(AllowEmptyStrings = true)]
        [StringLength(CostCentreConsts.CostCentreReferenceMaxLength)]
        public string CostCentreReference { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(CostCentreConsts.CostCentreNameMaxLength)]
        public string CostCentreName { get; set; } = null!;
        public bool IsDisabled { get; set; }
        public Guid? BranchId { get; set; }
    }
}