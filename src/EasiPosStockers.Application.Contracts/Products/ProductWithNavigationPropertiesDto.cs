using EasiPosStockers.CostCentres;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace EasiPosStockers.Products
{
    public abstract class ProductWithNavigationPropertiesDtoBase
    {
        public ProductDto Product { get; set; } = null!;

        public List<CostCentreDto> CostCentres { get; set; } = new List<CostCentreDto>();

    }
}