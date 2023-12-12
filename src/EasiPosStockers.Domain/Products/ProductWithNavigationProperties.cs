using EasiPosStockers.CostCentres;

using System;
using System.Collections.Generic;

namespace EasiPosStockers.Products
{
    public abstract class ProductWithNavigationPropertiesBase
    {
        public Product Product { get; set; } = null!;

        

        public List<CostCentre> CostCentres { get; set; } = null!;
        
    }
}