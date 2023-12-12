using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace EasiPosStockers.Branches
{
    public abstract class BranchUpdateDtoBase : IHasConcurrencyStamp
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

        public string ConcurrencyStamp { get; set; } = null!;
    }
}