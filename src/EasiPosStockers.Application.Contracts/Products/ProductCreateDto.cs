using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EasiPosStockers.Products
{
    public abstract class ProductCreateDtoBase
    {
        [Required(AllowEmptyStrings = true)]
        [StringLength(ProductConsts.DescriptionMaxLength)]
        public string Description { get; set; } = null!;
        [Required(AllowEmptyStrings = true)]
        [StringLength(ProductConsts.ProductNameMaxLength)]
        public string ProductName { get; set; } = null!;
        public List<Guid> CostCentreIds { get; set; }
    }
}