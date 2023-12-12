using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace EasiPosStockers.Products
{
    public abstract class ProductUpdateDtoBase : IHasConcurrencyStamp
    {
        [Required(AllowEmptyStrings = true)]
        [StringLength(ProductConsts.DescriptionMaxLength)]
        public string Description { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(ProductConsts.ProductNameMaxLength)]
        public string ProductName { get; set; } = null!;
        public List<Guid> CostCentreIds { get; set; }

        public string ConcurrencyStamp { get; set; } = null!;
    }
}