using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EasiPosStockers.Branches
{
    public abstract class BranchCreateDtoBase
    {
        [Required]
        [StringLength(BranchConsts.BranchReferenceMaxLength)]
        public string BranchReference { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(BranchConsts.BranchNameMaxLength)]
        public string BranchName { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(BranchConsts.TaxRegistrationNumberMaxLength)]
        public string TaxRegistrationNumber { get; set; } = null!;
    }
}